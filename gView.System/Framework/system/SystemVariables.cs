using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using Microsoft.Win32;
using gView.Framework.UI;
using gView.Framework.IO;
using System.Reflection;
using System.Runtime.CompilerServices;

// erzeugung PublicKey
// sn -p key.snk key.xxx
// sn -tp key.xxx >key.txt

[assembly: InternalsVisibleTo("gView.System.UI, PublicKey=0024000004800000940000000602000000240000525341310004000001000100e7f9b6324e02e10e05e5a385d9e60b9dcfcaa6328d68251b810d335e822bc169b1779e3845e95102e1f80bd23d006d9319dbd6910075038f7c2ef0e4eac1a532816d96d96b2888b152c58893ab182557c1f4ccbde9802a3118b35969d89d21c00c5b3205a0e150756ca6ed49251d97b3a24b1f1d1f3e7f0b933cb56099e093bb")]

namespace gView.Framework.system
{
    public class SystemVariables
    {
        static public Version gViewVersion
        {
            get
            {
                return new Version(System.Diagnostics.FileVersionInfo.GetVersionInfo(
                    Assembly.GetAssembly(typeof(SystemVariables)).Location).ProductVersion);
                //return new Version(1, 0, 3000, 0);
            }
        }

        static public string CustomApplicationDirectory = String.Empty;
        static public string ApplicationDirectory
        {
            get
            {
                //System.Environment.CurrentDirectory;
                //DirectoryInfo di = new DirectoryInfo(System.Windows.Forms.Application.StartupPath);
                //return di.FullName;

                if (IsPortable)
                    return PortableRootDirectory;

                if (!String.IsNullOrEmpty(CustomApplicationDirectory))
                    return CustomApplicationDirectory;

                return RegistryApplicationDirectory;
            }
        }
        static public string RegistryApplicationDirectory
        {
            get
            {
                try
                {
                    if (IsPortable)
                        return PortableRootDirectory;

                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gViewGisOS", false);
                    if (key == null)
                        return String.Empty;
                    object val = key.GetValue("Install_Dir");
                    string ret = (val != null) ? val.ToString() : String.Empty;
                    key.Close();

                    return ret;
                }

                catch
                {
                    return "";
                }
            }
            set
            {
                try
                {
                    if (IsPortable)
                        return;

                    Registry.LocalMachine.CreateSubKey(@"Software\gViewGisOS");
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\gViewGisOS", true);
                    if (key != null)
                    {
                        key.SetValue("Install_Dir", value);
                        key.Close();
                    }
                }
                catch { }
            }
        }
        static public string StartupDirectory
        {
            get { return ApplicationDirectory; }
        }

        static public string CustomMyApplicationData = String.Empty;
        static public string MyApplicationData
        {
            get
            {
                if (IsPortable)
                    return PortableRootDirectory + @"\AppData";

                if (!String.IsNullOrEmpty(CustomMyApplicationData))
                    return CustomMyApplicationData;

                string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\gView";
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists) di.Create();

                return di.FullName;
            }
        }

        static public string CustomMyCommonApplicationData = String.Empty;
        static public string MyCommonApplicationData
        {
            get
            {
                if (IsPortable)
                    return PortableRootDirectory + @"\AppData";

                if (!String.IsNullOrEmpty(CustomMyCommonApplicationData))
                    return CustomMyCommonApplicationData;

                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\gView";
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists) di.Create();

                return di.FullName;
            }
        }

        static public string CustomCommonApplicationData = String.Empty;
        static public string CommonApplicationData
        {
            get
            {
                if (IsPortable)
                    return PortableRootDirectory + @"\AppData";

                if (!String.IsNullOrEmpty(CustomCommonApplicationData))
                    return CustomCommonApplicationData;

                string path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\gView";
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists) di.Create();

                return di.FullName;
            }
        }
        static public string MyApplicationConfigPath
        {
            get
            {
                string path = MyApplicationData + @"\config";
                DirectoryInfo di = new DirectoryInfo(path);
                if (!di.Exists) di.Create();

                return di.FullName;
            }
        }

        static public string AppDomainConfigurationFile
        {
            get
            {
                return Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            }
        }

        static public bool IsPortable
        {
            get
            {
                try
                {
                    string path = PortableRootDirectory;
                    return File.Exists(path + @"\gview_portable.config");
                }
                catch { return false; }
            }
        }

        static public string PortableRootDirectory
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetAssembly(typeof(SystemVariables)).Location);
            }
         }

        static public bool IsWebHosted
        {
            get
            {
                return AppDomainConfigurationFile.ToLower() == "web.config";
            }
        }
        
        static public DateTime InstallationTime
        {
            get
            {
                try
                {
                    string guid = new Guid("7CB0A712-FCC7-478e-A7DB-0C7A9AC3F8CB").ToString();
                    RegistryKey key = Registry.ClassesRoot.OpenSubKey(@"Licenses\" + guid, false);
                    if (key == null)
                    {
                        key = Registry.ClassesRoot.CreateSubKey(@"Licenses\" + guid, RegistryKeyPermissionCheck.ReadWriteSubTree);
                        key.Close();
                        key = Registry.ClassesRoot.OpenSubKey(@"Licenses\" + guid, false);
                    }
                    if (key.GetValue("x") == null)
                    {
                        key.Close();
                        key = Registry.ClassesRoot.OpenSubKey(@"Licenses\" + guid, true);
                        key.SetValue("x", Crypto.Encrypt(DateTime.Now.ToBinary().ToString(), guid.ToString()));
                        key.Close();
                        key = Registry.ClassesRoot.OpenSubKey(@"Licenses\" + guid, false);
                    }
                    string x = Crypto.Decrypt((string)key.GetValue("x"), guid.ToString());
                    if (key.GetValue("m") == null)
                    {
                        key.Close();
                        key = Registry.ClassesRoot.OpenSubKey(@"Licenses\" + guid, true);
                        key.SetValue("m", Identity.HashPassword(SystemInfo.MashineName));
                        key.Close();
                        key = Registry.ClassesRoot.OpenSubKey(@"Licenses\" + guid, false);
                    }
                    if (key.GetValue("a") == null)
                    {
                        key.Close();
                        CLicenseManager licMan = new CLicenseManager();
                    }
                    else
                    {
                        key.Close();
                    }
                    return DateTime.FromBinary(Convert.ToInt64(x));
                }
                catch(Exception ex)
                {
                    string msg = ex.Message;
                    return new DateTime(1980, 1, 1);
                }
            }
        }
        static public string InsallationUsername
        {
            get
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\gViewGisOS", false);
                    if (key == null) return String.Empty;
                    object val = key.GetValue("username");
                    string ret = (val != null) ? val.ToString() : String.Empty;
                    key.Close();

                    return ret;
                }

                catch
                {
                    return "Unknown";
                }
            }
        }
        static public string InsallationCompanyname
        {
            get
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"Software\gViewGisOS", false);
                    if (key == null) return String.Empty;
                    object val = key.GetValue("companyname");
                    string ret = (val != null) ? val.ToString() : String.Empty;
                    key.Close();

                    return ret;
                }

                catch
                {
                    return "Unknown";
                }
            }
        }
        static private float _primaryScreenDPI = -1;
        static public float PrimaryScreenDPI
        {
            get
            {
                if (_primaryScreenDPI != -1) return _primaryScreenDPI;

                try
                {
                    using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromHwndInternal((IntPtr)0))
                    {
                        _primaryScreenDPI = gr.DpiX;
                    }
                }
                catch
                {
                    _primaryScreenDPI = 96f;
                }

                return _primaryScreenDPI;
            }
        }
    }

    public interface IProperties
    {
        object SelectedObject { get; }
    }

    public class Cloner
    {
        public object Clone()
        {
            if (!(this is IPersistable)) return null;

            try
            {
                object clone = null;
                if (PlugInManager.IsPlugin(this))
                {
                    PlugInManager compMan = new PlugInManager();
                    clone = compMan.CreateInstance(PlugInManager.PlugInID(this));
                }
                else
                {
                    System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(this.GetType());
                    clone = assembly.CreateInstance(this.GetType().ToString());
                }
                if (clone == null) return null;

                XmlStream stream = new XmlStream("root");
                ((IPersistable)this).Save(stream);
                ((IPersistable)clone).Load(stream);

                return clone;
            }
            catch
            {
            }
            return null;
        }
    }

    public class Bit
    {
        static public bool Has(object val, object bit)
        {
            return ((int)val & (int)bit) == (int)bit;
        }
    }

    public class ProgressDialog
    {
        static public IProgressDialog CreateProgressDialogInstance()
        {
            Assembly uiAssembly = Assembly.LoadFrom(SystemVariables.ApplicationDirectory + @"\gView.Explorer.UI.dll");

            IProgressDialog p = uiAssembly.CreateInstance("gView.Framework.UI.Dialogs.FormProgress") as IProgressDialog;
            return p;
        }
    }

    public class UserData : IUserData
    {
        private Dictionary<string, object> _dic = new Dictionary<string, object>();

        #region IUserDataDictinoary Member

        public void SetUserData(string type, object val)
        {
            try
            {
                object dummy;
                if (_dic.TryGetValue(type, out dummy))
                    _dic[type] = val;
                else
                    _dic.Add(type, val);
            }
            catch { }
        }

        public void RemoveUserData(string type)
        {
            try
            {
                _dic.Remove(type);
            }
            catch { }
        }
        public object GetUserData(string type)
        {
            object val;
            if (_dic.TryGetValue(type, out val))
                return val;

            return null;
        }

        public string[] UserDataTypes
        {
            get
            {
                List<string> keys = new List<string>();
                foreach (string key in _dic.Keys)
                {
                    keys.Add(key);
                }

                return keys.ToArray();
            }
        }
       
        public void CopyUserDataTo(IUserData userData)
        {
            if (userData == null) return;

            foreach (string key in _dic.Keys)
            {
                userData.SetUserData(key, _dic[key]);
            }
        }
        #endregion
    }
}
