﻿using System;
using System.Collections.Generic;
using System.Text;
using gView.Framework.Data;
using gView.Framework.Network.Build;
using gView.Framework.Geometry;
using System.Threading;
using gView.Framework.UI;
using gView.Framework.system;
using System.Windows.Forms;
using gView.Framework.Offline;
using gView.Framework.FDB;
using gView.Framework.Network;
using System.Collections;
using System.IO;
using gView.DataSources.Fdb.MSAccess;

namespace gView.DataSources.Fdb.UI.MSSql
{
    public class CreateFDBNetworkFeatureclass : IProgressReporter
    {
        private IFeatureDataset _dataset = null;
        private gView.DataSources.Fdb.MSAccess.AccessFDB _fdb = null;
        private List<IFeatureClass> _edgeFcs = null, _nodeFcs = null;
        private string _networkName = String.Empty;
        private CancelTracker _cancelTracker = new CancelTracker();
        private string _onewayFieldName = "oneway";
        private List<int> _complexEdgeFcs;
        private Dictionary<int, string> _switchNodeFcs;
        private Dictionary<int, NetworkNodeType> _nodeTypeFcs;
        private double _tolerance = double.Epsilon;
        private GraphWeights _graphWeights = null;

        public CreateFDBNetworkFeatureclass(IFeatureDataset dataset,
            string networkName,
            List<IFeatureClass> edgeFcs, List<IFeatureClass> nodeFcs)
        {
            if (dataset == null || !(dataset.Database is AccessFDB))
                return;

            _dataset = dataset;
            _fdb = (AccessFDB)_dataset.Database;

            _networkName = networkName;
            _edgeFcs = edgeFcs;
            _nodeFcs = nodeFcs;

            // Zum Testen -> wenn -1 -> alle als ComplexEdges möglich
            _complexEdgeFcs = new List<int>();
        }

        #region Properties
        public double SnapTolerance
        {
            get { return _tolerance; }
            set { _tolerance = value; }
        }
        public List<int> ComplexEdgeFcIds
        {
            get { return _complexEdgeFcs; }
            set { _complexEdgeFcs = value; }
        }
        public Dictionary<int, string> SwitchNodeFcIdAndFieldnames
        {
            get { return _switchNodeFcs; }
            set { _switchNodeFcs = value; }
        }
        public Dictionary<int, NetworkNodeType> NodeTypeFcIds
        {
            get { return _nodeTypeFcs; }
            set { _nodeTypeFcs = value; }
        }
        public GraphWeights GraphWeights
        {
            get { return _graphWeights; }
            set { _graphWeights = value; }
        }
        #endregion

        private void Run()
        {
            if (_dataset == null || !(_fdb is IFeatureDatabaseReplication) || _edgeFcs == null)
                return;

            IFeatureDatabaseReplication db = (IFeatureDatabaseReplication)_fdb;

            if (_dataset[_networkName] != null)
            {
                MessageBox.Show("Featureclass '" + _networkName + "' already exists!");
                return;
            }
            bool succeeded = false;
            List<int> FcIds = new List<int>();
            try
            {
                ProgressReport report = new ProgressReport();

                int datasetId = _fdb.DatasetID(_dataset.DatasetName);
                if (datasetId == -1)
                    return;

                NetworkBuilder networkBuilder = new NetworkBuilder(_dataset.Envelope, _tolerance);
                if (reportProgress != null)
                    networkBuilder.reportProgress += new ProgressReporterEvent(networkBuilder_reportProgress);

                #region Spatial Index
                BinaryTreeDef edgeTreeDef = null, nodeTreeDef = null;
                foreach (IFeatureClass fc in _edgeFcs)
                {
                    BinaryTreeDef treeDef = _fdb.BinaryTreeDef(fc.Name);
                    if (treeDef == null)
                    {
                        IEnvelope bounds = fc.Envelope;
                        if (Envelope.IsNull(bounds))
                            continue;
                        treeDef = new BinaryTreeDef(fc.Envelope, 10);
                    }
                    if (edgeTreeDef == null)
                        edgeTreeDef = new BinaryTreeDef(treeDef.Bounds, Math.Min(treeDef.MaxLevel, 10));
                    else
                    {
                        Envelope bounds = edgeTreeDef.Bounds;
                        bounds.Union(treeDef.Bounds);
                        edgeTreeDef = new BinaryTreeDef(bounds, Math.Min(Math.Max(edgeTreeDef.MaxLevel, treeDef.MaxLevel), 10));
                    }
                }
                foreach (IFeatureClass fc in _nodeFcs)
                {
                    BinaryTreeDef treeDef = _fdb.BinaryTreeDef(fc.Name);
                    if (treeDef == null)
                    {
                        IEnvelope bounds = fc.Envelope;
                        if (Envelope.IsNull(bounds))
                            continue;
                        treeDef = new BinaryTreeDef(fc.Envelope, 10);
                    }
                    if (nodeTreeDef == null)
                        nodeTreeDef = new BinaryTreeDef(treeDef.Bounds, Math.Min(treeDef.MaxLevel, 10));
                    else
                    {
                        Envelope bounds = nodeTreeDef.Bounds;
                        bounds.Union(treeDef.Bounds);
                        nodeTreeDef = new BinaryTreeDef(bounds, Math.Min(Math.Max(nodeTreeDef.MaxLevel, treeDef.MaxLevel), 10));
                    }
                }
                #endregion

                #region Add Edges
                foreach (IFeatureClass fc in _edgeFcs)
                {
                    int fcId = _fdb.FeatureClassID(datasetId, fc.Name);
                    if (fcId == -1)
                        continue;
                    FcIds.Add(fcId);

                    bool useOneway = false;
                    QueryFilter filter = new QueryFilter();
                    filter.AddField(fc.IDFieldName);
                    filter.AddField(fc.ShapeFieldName);
                    if (!String.IsNullOrEmpty(_onewayFieldName) && fc.FindField(_onewayFieldName) != null)
                    {
                        filter.AddField(_onewayFieldName);
                        useOneway = true;
                    }
                    Dictionary<Guid, IGraphWeightFeatureClass> gwfcs = new Dictionary<Guid, IGraphWeightFeatureClass>();
                    if (_graphWeights != null)
                    {
                        foreach (IGraphWeight weight in _graphWeights)
                        {
                            foreach (IGraphWeightFeatureClass gwfc in weight.FeatureClasses)
                            {
                                if (gwfc.FcId == fcId && !String.IsNullOrEmpty(gwfc.FieldName))
                                {
                                    gwfcs.Add(weight.Guid, gwfc);
                                    filter.AddField(gwfc.FieldName);
                                }
                            }
                        }
                    }
                    if (gwfcs.Keys.Count == 0) gwfcs = null;

                    bool useWithComplexEdges = false;
                    if (_complexEdgeFcs != null &&
                       (_complexEdgeFcs.Contains(fcId) || _complexEdgeFcs.Contains(-1)))
                        useWithComplexEdges = true;

                    using (IFeatureCursor cursor = fc.GetFeatures(filter))
                    {
                        #region Report
                        report.Message = "Analize Edges: " + fc.Name;
                        report.featureMax = fc.CountFeatures;
                        report.featurePos = 0;
                        if (reportProgress != null) reportProgress(report);
                        #endregion

                        IFeature feature;
                        while ((feature = cursor.NextFeature) != null)
                        {
                            bool oneway = false;
                            if (useOneway)
                            {
                                int ow = Convert.ToInt32(feature[_onewayFieldName]);
                                oneway = (ow != 0);
                            }
                            Hashtable gw = null;
                            if (gwfcs != null)
                            {
                                foreach (Guid weightGuid in gwfcs.Keys)
                                {
                                    IGraphWeightFeatureClass gwfc = gwfcs[weightGuid];
                                    object objVal = feature[gwfc.FieldName];

                                    double val = (objVal == DBNull.Value) ? 0.0 : Convert.ToDouble(feature[gwfc.FieldName]);
                                    if (gwfc.SimpleNumberCalculation != null)
                                        val = gwfc.SimpleNumberCalculation.Calculate(val);
                                    if (gw == null) gw = new Hashtable();
                                    gw.Add(weightGuid, val);
                                }
                            }
                            networkBuilder.AddEdgeFeature(fcId, feature, oneway, gw, useWithComplexEdges);

                            #region Report
                            report.featurePos++;
                            if (report.featurePos % 1000 == 0)
                                if (reportProgress != null) reportProgress(report);
                            #endregion
                        }
                    }
                }
                #endregion

                #region Calculate ComplexEdges
                if (_complexEdgeFcs != null && _complexEdgeFcs.Count > 0)
                {
                    List<NetworkNode> networkNodes = networkBuilder.NetworkNodes.ToList();

                    #region Report
                    report.Message = "Create Complex Edges...";
                    report.featureMax = networkNodes.Count;
                    report.featurePos = 0;
                    if (reportProgress != null) reportProgress(report);
                    #endregion

                    foreach (NetworkNode node in networkNodes)
                    {
                        networkBuilder.SplitToComplexEdges(node);

                        #region Report
                        report.featurePos++;
                        if (report.featurePos % 1000 == 0)
                            if (reportProgress != null) reportProgress(report);
                        #endregion
                    }
                }
                #endregion

                #region Add Nodes
                if (_nodeFcs != null)
                {
                    foreach (IFeatureClass fc in _nodeFcs)
                    {
                        int fcId = _fdb.FeatureClassID(datasetId, fc.Name);
                        if (fcId == -1)
                            continue;
                        FcIds.Add(fcId);

                        bool isSwitchable = false;
                        string switchStateFieldname = String.Empty;
                        if (_switchNodeFcs != null && _switchNodeFcs.ContainsKey(fcId))
                        {
                            isSwitchable = true;
                            switchStateFieldname = _switchNodeFcs[fcId];
                        }
                        NetworkNodeType nodeType = NetworkNodeType.Unknown;
                        if (_nodeTypeFcs != null && _nodeTypeFcs.ContainsKey(fcId))
                            nodeType = _nodeTypeFcs[fcId];

                        QueryFilter filter = new QueryFilter();
                        filter.AddField(fc.IDFieldName);
                        filter.AddField(fc.ShapeFieldName);
                        filter.AddField(switchStateFieldname);

                        using (IFeatureCursor cursor = fc.GetFeatures(filter))
                        {
                            #region Report
                            report.Message = "Analize Nodes: " + fc.Name;
                            report.featureMax = fc.CountFeatures;
                            report.featurePos = 0;
                            if (reportProgress != null) reportProgress(report);
                            #endregion

                            IFeature feature;
                            while ((feature = cursor.NextFeature) != null)
                            {
                                bool switchState = isSwitchable;
                                if (isSwitchable && !String.IsNullOrEmpty(switchStateFieldname))
                                {
                                    object so = feature[switchStateFieldname];
                                    if (so != null)
                                    {
                                        if (so is bool)
                                            switchState = (bool)so;
                                        else
                                        {
                                            try
                                            {
                                                switchState = Convert.ToInt32(so) > 0;
                                            }
                                            catch { switchState = false; }
                                        }
                                    }
                                }
                                networkBuilder.AddNodeFeature(fcId, feature, isSwitchable, switchState, nodeType);

                                #region Report
                                report.featurePos++;
                                if (report.featurePos % 1000 == 0)
                                    if (reportProgress != null) reportProgress(report);
                                #endregion
                            }
                        }
                    }
                }
                #endregion

                #region CreateGraph
                #region Report
                report.Message = "Create Graph";
                report.featurePos = 0;
                if (reportProgress != null) reportProgress(report);
                #endregion

                networkBuilder.CreateGraph();
                #endregion

                #region Create Edge Featureclass

                #region Simple Edges Table
                Fields fields = new Fields();
                fields.Add(new Field("Page", FieldType.integer));
                fields.Add(new Field("Data", FieldType.binary));
                db.CreateIfNotExists(_networkName + "_Edges", fields);
                #endregion

                #region Edge Index Table
                fields = new Fields();
                fields.Add(new Field("EID", FieldType.integer));
                fields.Add(new Field("FCID", FieldType.integer));
                fields.Add(new Field("OID", FieldType.integer));
                fields.Add(new Field("ISCOMPLEX", FieldType.boolean));
                db.CreateIfNotExists(_networkName + "_EdgeIndex", fields);
                #endregion

                #region Complex Edges FeatureClass
                fields = new Fields();
                fields.Add(new Field("EID", FieldType.integer));
                fields.Add(new Field("N1", FieldType.integer));
                fields.Add(new Field("N2", FieldType.integer));
                fields.Add(new Field("FCID", FieldType.integer));
                fields.Add(new Field("OID", FieldType.integer));
                //fields.Add(new Field("Length", FieldType.integer));
                //fields.Add(new Field("GeoLength", FieldType.integer));

                _fdb.ReplaceFeatureClass(_dataset.DatasetName,
                    _networkName + "_ComplexEdges",
                    new GeometryDef(geometryType.Polyline),
                    fields);
                IDatasetElement element = _dataset[_networkName + "_ComplexEdges"];
                if (element == null || !(element.Class is IFeatureClass))
                    return;
                if (edgeTreeDef != null)
                    _fdb.SetSpatialIndexBounds(_networkName + "_ComplexEdges", "BinaryTree2", edgeTreeDef.Bounds, edgeTreeDef.SplitRatio, edgeTreeDef.MaxPerNode, edgeTreeDef.MaxLevel);
                #endregion

                int edge_page = 0;
                List<IFeature> features = new List<IFeature>(), features2 = new List<IFeature>();
                List<IRow> rows = new List<IRow>();
                IFeatureClass edgeFc = (IFeatureClass)element.Class;
                IFeatureCursor c = networkBuilder.Edges;
                IFeature f;

                #region Report
                report.Message = "Create Edges";
                if (c is ICount) report.featureMax = ((ICount)c).Count;
                report.featurePos = 0;
                if (reportProgress != null) reportProgress(report);
                #endregion

                string tabEdgesName = _fdb.TableName(_networkName + "_Edges");
                string tabEdgeIndexName = _fdb.TableName(_networkName + "_EdgeIndex");

                while ((f = c.NextFeature) != null)
                {
                    int eid = (int)f["EID"];
                    if ((bool)f["ISCOMPLEX"] == true)
                    {
                        #region Complex Edges
                        features.Add(f);

                        report.featurePos++;
                        if (features.Count > 0 && features.Count % 1000 == 0)
                        {
                            if (reportProgress != null) reportProgress(report);
                            _fdb.Insert(edgeFc, features);
                            features.Clear();
                        }
                        #endregion
                    }
                    #region Edges Table
                    features2.Add(f);
                    if (NetworkObjectSerializer.Page(eid + 1) > edge_page)
                    {
                        IRow row = new Row();
                        row.Fields.Add(new FieldValue("Page", edge_page++));
                        row.Fields.Add(new FieldValue("Data", NetworkObjectSerializer.SerializeEdges(features2)));
                        db.InsertRow(tabEdgesName, row, null);
                        features2.Clear();
                    }
                    #endregion
                    #region Edge Index
                    IRow eir = new Row();
                    eir.Fields.Add(new FieldValue("EID", (int)f["EID"]));
                    eir.Fields.Add(new FieldValue("FCID", (int)f["FCID"]));
                    eir.Fields.Add(new FieldValue("OID", (int)f["OID"]));
                    eir.Fields.Add(new FieldValue("ISCOMPLEX", (bool)f["ISCOMPLEX"]));
                    rows.Add(eir);
                    if (rows.Count > 0 && rows.Count % 1000 == 0)
                    {
                        _fdb.InsertRows(tabEdgeIndexName, rows, null);
                        rows.Clear();
                    }

                    #endregion
                }
                if (rows.Count > 0)
                {
                    _fdb.InsertRows(tabEdgeIndexName, rows, null);
                    rows.Clear();
                }
                if (features2.Count > 0)
                {
                    IRow row = new Row();
                    row.Fields.Add(new FieldValue("Page", edge_page));
                    row.Fields.Add(new FieldValue("Data", NetworkObjectSerializer.SerializeEdges(features2)));
                    db.InsertRow(tabEdgesName, row, null);
                    features2.Clear();
                }
                if (features.Count > 0)
                {
                    if (reportProgress != null) reportProgress(report);
                    _fdb.Insert(edgeFc, features);
                }
                _fdb.CalculateExtent(edgeFc);
                #endregion
                #region Create Weights
                if (_graphWeights != null)
                {
                    foreach (IGraphWeight weight in _graphWeights)
                    {
                        fields = new Fields();
                        fields.Add(new Field("Page", FieldType.integer));
                        fields.Add(new Field("Data", FieldType.binary));
                        db.CreateIfNotExists(_networkName + "_Weights_" + weight.Guid.ToString("N").ToLower(), fields);

                        string tabWeightName = _fdb.TableName(_networkName + "_Weights_" + weight.Guid.ToString("N").ToLower());

                        NetworkEdges edges = networkBuilder.NetworkEdges;
                        int weight_page = 0;
                        int counter = 0;
                        BinaryWriter bw = NetworkObjectSerializer.GetBinaryWriter();
                        foreach (NetworkEdge edge in edges)
                        {
                            counter++;
                            if (NetworkObjectSerializer.Page(counter) > weight_page)
                            {
                                IRow row = new Row();
                                row.Fields.Add(new FieldValue("Page", weight_page++));
                                row.Fields.Add(new FieldValue("Data", NetworkObjectSerializer.GetBuffer(bw)));
                                db.InsertRow(tabWeightName, row, null);
                                bw = NetworkObjectSerializer.GetBinaryWriter();
                            }

                            if (edge.Weights != null && edge.Weights.ContainsKey(weight.Guid))
                            {
                                NetworkObjectSerializer.WriteWeight(bw, weight, (double)edge.Weights[weight.Guid]);
                            }
                            else
                            {
                                NetworkObjectSerializer.WriteWeight(bw, weight, (double)0.0);
                            }


                        }

                        if (bw.BaseStream.Position > 0)
                        {
                            IRow row = new Row();
                            row.Fields.Add(new FieldValue("Page", weight_page++));
                            row.Fields.Add(new FieldValue("Data", NetworkObjectSerializer.GetBuffer(bw)));
                            db.InsertRow(tabWeightName, row, null);
                        }
                    }
                }
                #endregion
                #region Create Node Featureclass
                fields = new Fields();
                //fields.Add(new Field("NID", FieldType.integer));
                //fields.Add(new Field("G1", FieldType.integer));
                //fields.Add(new Field("G2", FieldType.integer));
                fields.Add(new Field("SWITCH", FieldType.boolean));
                fields.Add(new Field("STATE", FieldType.boolean));
                fields.Add(new Field("FCID", FieldType.integer));
                fields.Add(new Field("OID", FieldType.integer));
                fields.Add(new Field("NODETYPE", FieldType.integer));

                _fdb.ReplaceFeatureClass(_dataset.DatasetName,
                    _networkName + "_Nodes",
                    new GeometryDef(geometryType.Point),
                    fields);

                element = _dataset[_networkName + "_Nodes"];
                if (element == null || !(element.Class is IFeatureClass))
                    return;
                if (nodeTreeDef != null)
                    _fdb.SetSpatialIndexBounds(_networkName + "_Nodes", "BinaryTree2", nodeTreeDef.Bounds, nodeTreeDef.SplitRatio, nodeTreeDef.MaxPerNode, nodeTreeDef.MaxLevel);
                else if (edgeTreeDef != null)
                    _fdb.SetSpatialIndexBounds(_networkName + "_Nodes", "BinaryTree2", edgeTreeDef.Bounds, edgeTreeDef.SplitRatio, edgeTreeDef.MaxPerNode, edgeTreeDef.MaxLevel);

                features.Clear();
                IFeatureClass nodeFc = (IFeatureClass)element.Class;
                c = networkBuilder.Nodes;

                #region Report
                report.Message = "Create Nodes";
                if (c is ICount) report.featureMax = ((ICount)c).Count;
                report.featurePos = 0;
                if (reportProgress != null) reportProgress(report);
                #endregion

                while ((f = c.NextFeature) != null)
                {
                    features.Add(f);

                    report.featurePos++;
                    if (features.Count > 0 && features.Count % 1000 == 0)
                    {
                        if (reportProgress != null) reportProgress(report);
                        _fdb.Insert(nodeFc, features);
                        features.Clear();
                    }
                }
                if (features.Count > 0)
                {
                    if (reportProgress != null) reportProgress(report);
                    _fdb.Insert(nodeFc, features);
                }
                _fdb.CalculateExtent(nodeFc);
                #endregion
                #region Create Graph Class
                int graph_page = 0;
                fields = new Fields();
                fields.Add(new Field("Page", FieldType.integer));
                fields.Add(new Field("Data", FieldType.binary));
                //fields.Add(new Field("N1", FieldType.integer));
                //fields.Add(new Field("N2", FieldType.integer));
                //fields.Add(new Field("EID", FieldType.integer));
                //fields.Add(new Field("LENGTH", FieldType.Double));
                //fields.Add(new Field("GEOLENGTH", FieldType.Double));

                _fdb.ReplaceFeatureClass(_dataset.DatasetName,
                    _networkName,
                    new GeometryDef(geometryType.Network),
                    fields);

                element = _dataset[_networkName];
                if (element == null || !(element.Class is IFeatureClass))
                    return;

                features.Clear();
                IFeatureClass networkFc = (IFeatureClass)element.Class;
                c = networkBuilder.Graph;

                #region Report
                report.Message = "Create Network";
                if (c is ICount) report.featureMax = ((ICount)c).Count;
                report.featurePos = 0;
                if (reportProgress != null) reportProgress(report);
                #endregion

                string fcNetworkName = _fdb.TableName("FC_" + _networkName);

                while ((f = c.NextFeature) != null)
                {
                    report.featurePos++;

                    // Wenn aktuelles Feature in neue Page gehört -> 
                    // bestehende, vor neuem Einfügen speichern und pageIndex erhöhen (graph_page++)
                    if (NetworkObjectSerializer.Page((int)f["N1"]) > graph_page)
                    {
                        if (reportProgress != null) reportProgress(report);
                        IRow row = new Row();
                        row.Fields.Add(new FieldValue("Page", graph_page++));
                        row.Fields.Add(new FieldValue("Data", NetworkObjectSerializer.SerializeGraph(features)));
                        db.InsertRow(fcNetworkName, row, null);
                        features.Clear();
                    }

                    features.Add(f);
                }

                if (features.Count > 0)
                {
                    IRow row = new Row();
                    row.Fields.Add(new FieldValue("Page", graph_page));
                    row.Fields.Add(new FieldValue("Data", NetworkObjectSerializer.SerializeGraph(features)));
                    db.InsertRow(fcNetworkName, row, null);
                    features.Clear();
                }
                #endregion

                #region Create FDB_Networks
                int netId = _fdb.GetFeatureClassID(_networkName);
                fields = new Fields();
                fields.Add(new Field("ID", FieldType.integer));
                fields.Add(new Field("Properties", FieldType.binary));
                db.CreateIfNotExists("FDB_Networks", fields);

                NetworkObjectSerializer.NetworkProperties networkProps = new NetworkObjectSerializer.NetworkProperties(
                    NetworkObjectSerializer.PageSize, _tolerance);

                IRow row2 = new Row();
                row2.Fields.Add(new FieldValue("ID", netId));
                row2.Fields.Add(new FieldValue("Properties", networkProps.Serialize()));
                db.InsertRow(_fdb.TableName("FDB_Networks"), row2, null);
                #endregion

                #region Create FDB_NetworkClasses
                fields = new Fields();
                fields.Add(new Field("NetworkId", FieldType.integer));
                fields.Add(new Field("FCID", FieldType.integer));
                fields.Add(new Field("Properties", FieldType.binary));

                db.CreateIfNotExists("FDB_NetworkClasses", fields);
                foreach (int fcId in FcIds)
                {
                    bool isSwitchable = (_switchNodeFcs != null && _switchNodeFcs.ContainsKey(fcId));
                    string switchStateFieldname = (isSwitchable ? _switchNodeFcs[fcId] : String.Empty);

                    NetworkObjectSerializer.NetworkClassProperties nwclsProps = new NetworkObjectSerializer.NetworkClassProperties(
                        _complexEdgeFcs.Contains(fcId),
                        isSwitchable, switchStateFieldname);

                    IRow row = new Row();
                    row.Fields.Add(new FieldValue("NetworkId", netId));
                    row.Fields.Add(new FieldValue("FCID", fcId));
                    row.Fields.Add(new FieldValue("Properties", nwclsProps.Serialize()));
                    db.InsertRow(_fdb.TableName("FDB_NetworkClasses"), row, null);
                }
                #endregion

                #region FDB_NetworkWeighs
                fields = new Fields();
                fields.Add(new Field("NetworkId", FieldType.integer));
                fields.Add(new Field("Name", FieldType.String));
                fields.Add(new Field("WeightGuid", FieldType.guid));
                fields.Add(new Field("Properties", FieldType.binary));

                db.CreateIfNotExists("FDB_NetworkWeights", fields);
                if (_graphWeights != null)
                {
                    foreach (IGraphWeight weight in _graphWeights)
                    {
                        IRow row = new Row();
                        row.Fields.Add(new FieldValue("NetworkId", netId));
                        row.Fields.Add(new FieldValue("Name", weight.Name));
                        row.Fields.Add(new FieldValue("WeightGuid", weight.Guid));
                        row.Fields.Add(new FieldValue("Properties", NetworkObjectSerializer.SerializeWeight(weight)));
                        db.InsertRow(_fdb.TableName("FDB_NetworkWeights"), row, null);
                    }
                }
                #endregion

                succeeded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!succeeded)
                {
                    if (_fdb is IAlterDatabase)
                        ((IAlterDatabase)_fdb).DropTable(_networkName + "_Edges");
                    _fdb.DeleteFeatureClass(_networkName + "_ComplexEdges");
                    _fdb.DeleteFeatureClass(_networkName + "_Nodes");
                    _fdb.DeleteFeatureClass(_networkName);
                }
            }
        }

        void networkBuilder_reportProgress(ProgressReport progressEventReport)
        {
            if (reportProgress != null)
                reportProgress(progressEventReport);
        }
        public Thread Thread
        {
            get
            {
                return new Thread(new ThreadStart(this.Run));
            }
        }

        #region IProgressReporter Member

        public event ProgressReporterEvent reportProgress = null;

        public gView.Framework.system.ICancelTracker CancelTracker
        {
            get { return _cancelTracker; }
        }

        #endregion
    }
}