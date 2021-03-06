using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using gView.Framework.UI;
using gView.Framework.Carto;
using gView.Framework.Data;
using gView.Framework.system;
using gView.Explorer.UI;

namespace gView.Framework.UI.Controls
{
    [gView.Framework.system.RegisterPlugIn("3143FA90-587B-4cd0-B508-1BE88882E6B3")]
    public partial class PreviewControl : UserControl, IExplorerTabPage
    {
        private IExplorerObject _exObject = null;
        private ToolButton _activeToolButton;
        private MapDocument _mapDocument;
        private IMap _map;
        private List<ITool> _tools = new List<ITool>();

        public PreviewControl()
        {
            InitializeComponent();
        }

        #region IExplorerTabPage Members

        public new Control Control
        {
            get { return this; }
        }

        public void OnCreate(object hook)
        {
            if (hook is IGUIApplication)
            {
                _mapDocument = new MapDocument(new ExplorerMapApplication((IGUIApplication)hook, this, mapView1));
                _map = new Map();
                _map.Display.refScale = 0;
                _map.DrawingLayer += new DrawingLayerEvent(DrawingLayer);

                _mapDocument.AddMap(_map);
                _mapDocument.FocusMap = _map;

                PlugInManager compMan = new PlugInManager();

                ITool zoomin = compMan.CreateInstance(KnownObjects.Tools_DynamicZoomIn) as ITool;
                ITool zoomout = compMan.CreateInstance(KnownObjects.Tools_DynamicZoomOut) as ITool;
                ITool smartNav = compMan.CreateInstance(KnownObjects.Tools_SmartNavigation) as ITool;
                ITool pan = compMan.CreateInstance(KnownObjects.Tools_Pan) as ITool;
                //ITool zoomextent = compMan.CreateInstance(KnownObjects.Tools_Zoom2Extent) as ITool;
                //ITool toc = compMan.CreateInstance(KnownObjects.Tools_TOC) as ITool;

                ITool identify = compMan.CreateInstance(KnownObjects.Tools_Identify) as ITool;
                ITool queryCombo = compMan.CreateInstance(KnownObjects.Tools_QueryThemeCombo) as ITool;

                if (zoomin != null) zoomin.OnCreate(_mapDocument);
                if (zoomout != null) zoomout.OnCreate(_mapDocument);
                if (smartNav != null) smartNav.OnCreate(_mapDocument);
                if (pan != null) pan.OnCreate(_mapDocument);
                if (identify != null)
                {
                    identify.OnCreate(_mapDocument);
                    identify.OnCreate(this);
                }
                if (queryCombo != null) queryCombo.OnCreate(_mapDocument);
                //if (zoomextent != null) zoomextent.OnCreate(_mapDocument);
                //if (toc != null) toc.OnCreate(_mapDocument);

                if (zoomin != null) toolStrip.Items.Add(new ToolButton(zoomin));
                if (zoomout != null) toolStrip.Items.Add(new ToolButton(zoomout));
                if (smartNav != null) toolStrip.Items.Add(_activeToolButton = new ToolButton(smartNav));
                if (pan != null) toolStrip.Items.Add(new ToolButton(pan));
                //if(zoomextent!=null) toolStrip.Items.Add(new ToolButton(zoomextent));
                //if(toc!=null) toolStrip.Items.Add(new ToolButton(toc));
                if (identify != null) toolStrip.Items.Add(new ToolButton(identify));
                if (queryCombo is IToolItem) toolStrip.Items.Add(((IToolItem)queryCombo).ToolItem);

                if (zoomin != null) _tools.Add(zoomin);
                if (zoomout != null) _tools.Add(zoomout);
                if (smartNav != null) _tools.Add(smartNav);
                if (pan != null) _tools.Add(pan);
                //if (zoomextent != null) _tools.Add(zoomextent);
                //if (toc != null) _tools.Add(toc);
                if (identify != null) _tools.Add(identify);
                if (queryCombo != null) _tools.Add(queryCombo);

                _activeToolButton.Checked = true;
                mapView1.Map = _map;
                _map.NewBitmap += new NewBitmapEvent(mapView1.NewBitmapCreated);
                _map.DoRefreshMapView += new DoRefreshMapViewEvent(mapView1.MakeMapViewRefresh);
                mapView1.Tool = _activeToolButton.Tool;
            }
        }

        public void OnShow()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { OnShow(); }));
            }
            else
            {
                mapView1.RefreshMap(DrawPhase.Geography);
            }
        }

        public void OnHide()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { OnHide(); }));
            }
            else
            {
                mapView1.CancelDrawing(DrawPhase.All);
            }
        }

        private void InkokeSetExplorerObject()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { InkokeSetExplorerObject(); }));
            }
            else
            {
                mapView1.CancelDrawing(DrawPhase.All);
                foreach (IDatasetElement element in _map.MapElements)
                {
                    _map.RemoveLayer(element as ILayer);
                }

                if (_exObject != null)
                {
                    if (_exObject.Object is IFeatureClass && ((IFeatureClass)_exObject.Object).Envelope != null)
                    {
                        mapView1.Map = _map;
                        _map.AddLayer(LayerFactory.Create(_exObject.Object as IClass));
                        _map.Display.Limit = ((IFeatureClass)_exObject.Object).Envelope;
                        _map.Display.ZoomTo(((IFeatureClass)_exObject.Object).Envelope);
                    }
                    else if (_exObject.Object is IRasterClass && ((IRasterClass)_exObject.Object).Polygon != null)
                    {
                        mapView1.Map = _map;
                        _map.AddLayer(LayerFactory.Create(_exObject.Object as IClass));
                        _map.Display.Limit = ((IRasterClass)_exObject.Object).Polygon.Envelope;
                        _map.Display.ZoomTo(((IRasterClass)_exObject.Object).Polygon.Envelope);
                    }
                    else if (_exObject.Object is IWebServiceClass && ((IWebServiceClass)_exObject.Object).Envelope != null)
                    {
                        mapView1.Map = _map;
                        _map.AddLayer(LayerFactory.Create(_exObject.Object as IClass));
                        _map.Display.Limit = ((IWebServiceClass)_exObject.Object).Envelope;
                        _map.Display.ZoomTo(((IWebServiceClass)_exObject.Object).Envelope);
                    }
                    else if (_exObject.Object is IFeatureDataset)
                    {
                        mapView1.Map = _map;
                        IFeatureDataset dataset = (IFeatureDataset)_exObject.Object;
                        foreach (IDatasetElement element in dataset.Elements)
                        {
                            ILayer layer = LayerFactory.Create(element.Class) as ILayer;
                            if (layer == null) continue;
                            _map.AddLayer(layer);
                        }
                        _map.Display.Limit = dataset.Envelope;
                        _map.Display.ZoomTo(dataset.Envelope);
                    }
                    else if (_exObject.Object is Map)
                    {
                        Map map = (Map)_exObject.Object;

                        map.NewBitmap -= new NewBitmapEvent(mapView1.NewBitmapCreated);
                        map.DoRefreshMapView -= new DoRefreshMapViewEvent(mapView1.MakeMapViewRefresh);

                        map.NewBitmap += new NewBitmapEvent(mapView1.NewBitmapCreated);
                        map.DoRefreshMapView += new DoRefreshMapViewEvent(mapView1.MakeMapViewRefresh);

                        mapView1.Map = (Map)_exObject.Object;
                    }
                }
            }
        }

        public IExplorerObject ExplorerObject
        {
            get
            {
                return _exObject;
            }
            set
            {
                if (_exObject == value || _map == null) return;

                _exObject = value;
                InkokeSetExplorerObject();
            }
        }

        public bool ShowWith(IExplorerObject exObject)
        {
            if (exObject == null) return false;
            if (TypeHelper.Match(exObject.ObjectType, typeof(IFeatureClass)) ||
                TypeHelper.Match(exObject.ObjectType, typeof(IRasterClass)) ||
                TypeHelper.Match(exObject.ObjectType, typeof(IWebServiceClass)) ||
                TypeHelper.Match(exObject.ObjectType, typeof(IFeatureDataset)) ||
                TypeHelper.Match(exObject.ObjectType, typeof(Map))) return true;

            return false;
        }

        public string Title
        {
            get { return "Preview"; }
        }

        public void RefreshContents()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { RefreshContents(); }));
            }
            else
            {
                mapView1.CancelDrawing(DrawPhase.All);
                mapView1.RefreshMap(DrawPhase.All);
            }
        }
        #endregion

        private void toolStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (!(e.ClickedItem is ToolButton) || _activeToolButton == e.ClickedItem) return;

            _activeToolButton.Checked = false;
            _activeToolButton = (ToolButton)e.ClickedItem;
            _activeToolButton.Checked = true;

            mapView1.Tool = _activeToolButton.Tool;
        }

        internal ITool Tool(Guid guid)
        {
            foreach (ITool tool in _tools)
            {
                if (PlugInManager.PlugInID(tool) == guid) return tool;
            }
            return null;
        }

        public void RefreshMap()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { RefreshMap(); }));
            }
            else
            {
                if (mapView1 != null)
                    mapView1.RefreshMap(DrawPhase.All);
            }
        }

        #region IOrder Members

        public int SortOrder
        {
            get { return 10; }
        }

        #endregion

        #region mapView
        private delegate void DrawingLayerCallback(string layerName);
        private void DrawingLayer(string layerName)
        {
            if (statusStrip1.InvokeRequired)
            {
                DrawingLayerCallback d = new DrawingLayerCallback(DrawingLayer);
                this.Invoke(d, new object[] { layerName });
            }
            else
            {
                if (layerName == "")
                    toolStripStatusLabel1.Text = "";
                else
                    toolStripStatusLabel1.Text = "Drawing Layer " + layerName + "...";
            }
        }

        private void mapView1_AfterRefreshMap()
        {
            DrawingLayer("");
        }

        private delegate void CursorMoveCallback(int x, int y, double X, double Y);
        private void mapView1_CursorMove(int x, int y, double X, double Y)
        {
            if (statusStrip1.InvokeRequired)
            {
                CursorMoveCallback d = new CursorMoveCallback(mapView1_CursorMove);
                this.Invoke(d, new object[] { x, y, X, Y });
            }
            else
            {
                toolStripStatusLabel2.Text = "X: " + Math.Round(X, 2);
                toolStripStatusLabel3.Text = "Y: " + Math.Round(Y, 2);
            }
        }
        #endregion
    }

    internal class ToolButton : ToolStripButton
    {
        private ITool _tool;

        public ToolButton(ITool tool)
            : base((Image)tool.Image)
        {
            _tool = tool;
        }

        public ITool Tool
        {
            get { return _tool; }
        }
    }

    [gView.Framework.system.RegisterPlugIn("62661EF5-2B98-4189-BFCD-7629476FA91C")]
    public class DataTablePage : IExplorerTabPage
    {
        gView.Framework.UI.Dialogs.FormDataTable _table = null;
        IExplorerObject _exObject = null;

        #region IExplorerTabPage Members

        public Control Control
        {
            get
            {
                if (_table == null)
                    _table = new gView.Framework.UI.Dialogs.FormDataTable(null);
                _table.StartWorkerOnShown = false;
                _table.ShowExtraButtons = false;

                return _table;
            }
        }

        public void OnCreate(object hook)
        {
        }

        public void OnShow()
        {
            OnHide();
            if (_exObject == null) return;

            if (_exObject.Object is ITableClass && _table != null)
            {
                _table.TableClass = (ITableClass)_exObject.Object;
                _table.StartWorkerThread();
            }
        }
        public void OnHide()
        {
            //MessageBox.Show("get hidden...");
            if (_table != null)
            {
                _table.TableClass = null;
            }
        }

        public IExplorerObject ExplorerObject
        {
            get
            {
                return _exObject;
            }
            set
            {
                _exObject = value;
                //OnShow();
            }
        }

        public bool ShowWith(IExplorerObject exObject)
        {
            if (exObject == null) return false;
            if (TypeHelper.Match(exObject.ObjectType, typeof(ITableClass)))
            {
                //if (TypeHelper.Match(exObject.ObjectType, typeof(INetworkClass)) &&
                //   ((IFeatureClass)exObject.Object).GeometryType == gView.Framework.Geometry.geometryType.Network)
                //    return false;

                return true;
            }
            return false;
        }

        public string Title
        {
            get { return "Data Table"; }
        }

        public void RefreshContents()
        {
        }
        #endregion

        #region IOrder Members

        public int SortOrder
        {
            get { return 20; }
        }

        #endregion
    }
}
