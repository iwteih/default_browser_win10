using System;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using Microsoft.Win32;

namespace DefaultBrowserSetting
{
    public partial class Form1 : Form
    {
        private const string APPLICATIONNAME = "DefaultBrowserSetting";
        private const string REGISTRYPATH = @"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run\";

        public Form1()
        {
            InitializeComponent();
        }

        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(int point);

        private void Form1_Load(object sender, EventArgs e)
        {
            bool isWin10 = IsWindows10();

            if (!isWin10)
            {
                MessageBox.Show("only work for Windows 10");
                return;
            }

            StartUpInRegedit();
            
            if (!SetDefaultBrowserAtStartup())
            {
                return;
            }

            if (CloseWindowsAutomatically())
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }

            bool success = SetBrowser();
            if (success)
            {
                if (CloseWindowsAutomatically())
                {
                    Close();
                }
            }
            else
            {
                ShowDefaultAppsUI();
                PopBrowserList(300);
                Close();
            }

        }

        private void StartUpInRegedit()
        {
            string appStartPath = System.IO.Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName);
            bool AutoStart = AutoStartAtLoggonWindows();
            RunWhenStart(AutoStart, APPLICATIONNAME, System.IO.Path.Combine(appStartPath, APPLICATIONNAME + ".exe"));
        }

        private bool IsWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows 10");
        }
        private int GetBrowerIndex()
        {
            return int.Parse(ConfigurationManager.AppSettings["desiredBrowerIndex"].Split(',')[0]);
        }

        private String GetDesiredBrower()
        {
            return ConfigurationManager.AppSettings["desiredBrowerIndex"].Split(',')[1];
        }

        private bool CloseWindowsAutomatically()
        {
            return bool.Parse(ConfigurationManager.AppSettings["closeWindowsAutomatically"]);
        }
        private bool SetDefaultBrowserAtStartup()
        {
            return bool.Parse(ConfigurationManager.AppSettings["setDefaultBrowserAtStartup"]);
        }

        private bool AutoStartAtLoggonWindows()
        {
            return bool.Parse(ConfigurationManager.AppSettings["runAtWindowsStartup"]);
        }
        private int GetRetryCount()
        {
            if (ConfigurationManager.AppSettings.AllKeys.Contains("retryCount"))
            {
                return int.Parse(ConfigurationManager.AppSettings["retryCount"]);
            }
            return 10;
        }

        private void ShowDefaultAppsUI()
        {
            ApplicationActivationManager appActiveManager = new ApplicationActivationManager();//Class not registered
            uint pid;
            String appUserModelId = "windows.immersivecontrolpanel_cw5n1h2txyewy!microsoft.windows.immersivecontrolpanel";
            //String arguments = "page=SettingsPageAppsDefaults";
            String arguments = "page=SettingsPageAppsDefaults&target=SystemSettings_DefaultApps_Browser";
            IntPtr hr = appActiveManager.ActivateApplication(appUserModelId, arguments, ActivateOptions.None, out pid);
        }

        private Process PopBrowserList(int millseconds)
        {
            Process p = Process.GetProcessesByName("SystemSettings").FirstOrDefault();
            if (p != null)
            {
                System.Threading.Thread.Sleep(millseconds);
                SetForegroundWindow(p.Id);
                SendKeys.SendWait("{Enter}");
            }
            return p;
        }

        private void SetDefaultBrowser(Process process)
        {
            if (process == null)
            {
                return;
            }

            int browserIndex = GetBrowerIndex();
            System.Threading.Thread.Sleep(100);

            for (int i = 0; i < browserIndex - 1; i++)
            {
                SetForegroundWindow(process.Id);
                SendKeys.SendWait("{Tab}");
                System.Threading.Thread.Sleep(100);

            }
            SetForegroundWindow(process.Id);
            SendKeys.SendWait("{Enter}");
            System.Threading.Thread.Sleep(1000);
            try
            {
                process.Kill();
            }
            catch
            { }
        }
        private void btnDefaultBrowser_Click(object sender, EventArgs e)
        {
            SetBrowser();
        }

        private bool IsSetSuccessful()
        {
            return GetDesiredBrower().Trim().ToLower() == GetSystemDefaultBrowser().Trim().ToLower();
        }

        private bool SetBrowser()
        {
            int maxRetry = GetRetryCount();

            if(maxRetry <= 0)
            {
                ShowDefaultAppsUI();
                PopBrowserList(500);
                return true;
            }

            while (!IsSetSuccessful() && maxRetry > 0)
            {
                ShowDefaultAppsUI();
                Process p = PopBrowserList(500);
                SetDefaultBrowser(p);
                maxRetry--;
                System.Threading.Thread.Sleep(1000);
            }

            bool success = IsSetSuccessful();
            this.lbMessage.Visible = !success;
            return success;
        }

        private string GetSystemDefaultBrowser()
        {
            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            string progId;
            String browser = "Unknown";
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoice))
            {
                if (userChoiceKey == null)
                {
                    return "Unknown";
                }
                object progIdValue = userChoiceKey.GetValue("Progid");
                if (progIdValue == null)
                {
                    browser = "Unknown";
                }
                progId = progIdValue.ToString();
                switch (progId)
                {
                    case "IE.HTTP":
                        browser = "InternetExplorer";
                        break;
                    case "FirefoxURL":
                        browser = "Firefox";
                        break;
                    case "ChromeHTML":
                        browser = "Chrome";
                        break;
                    case "OperaStable":
                        browser = "Opera";
                        break;
                    case "SafariHTML":
                        browser = "Safari";
                        break;
                    case "AppXq0fevzme2pys62n3e0fbqa7peapykr8v":
                        browser = "Edge";
                        break;
                    default:
                        browser = "Unknown";
                        break;
                }
            }
            return browser;
        }

        private bool RunWhenStart(bool started, string name, string path)
        {
            bool success = false;

            RegistryKey HKLM = null;
            RegistryKey Run = null;

            try
            {
                HKLM = Registry.CurrentUser;
                Run = HKLM.CreateSubKey(REGISTRYPATH);

                if (started)
                {
                    Run.SetValue(name, path);
                }
                else
                {
                    if (IsRegeditExisted(name))
                    {
                        Run.DeleteValue(name);
                    }
                }

                success = true;
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }
            finally
            {
                if (Run != null)
                {
                    Run.Close();
                }

                if (HKLM != null)
                {
                    HKLM.Close();
                }
            }

            return success;
        }

        private bool IsRegeditExisted(string name)
        {
            bool existed = false;

            try
            {
                string[] subkeyNames = Microsoft.Win32.Registry.CurrentUser
                            .OpenSubKey("Software")
                            .OpenSubKey("Microsoft")
                            .OpenSubKey("Windows")
                            .OpenSubKey("CurrentVersion")
                            .OpenSubKey("Run")
                            .GetValueNames();

                foreach (string keyName in subkeyNames)
                {
                    if (keyName == name)
                    {
                        existed = true;
                        break;
                    }
                }
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp);
            }

            return existed;
        }
        public enum ActivateOptions
        {
            None = 0x00000000,  // No flags set
            DesignMode = 0x00000001,  // The application is being activated for design mode, and thus will not be able to
                                      // to create an immersive window. Window creation must be done by design tools which
                                      // load the necessary components by communicating with a designer-specified service on
                                      // the site chain established on the activation manager.  The splash screen normally
                                      // shown when an application is activated will also not appear.  Most activations
                                      // will not use this flag.
            NoErrorUI = 0x00000002,  // Do not show an error dialog if the app fails to activate.                                
            NoSplashScreen = 0x00000004,  // Do not show the splash screen when activating the app.
        }

        [ComImport, Guid("2e941141-7f97-4756-ba1d-9decde894a3d"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        interface IApplicationActivationManager
        {
            // Activates the specified immersive application for the "Launch" contract, passing the provided arguments
            // string into the application.  Callers can obtain the process Id of the application instance fulfilling this contract.
            IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);
            IntPtr ActivateForFile([In] String appUserModelId, [In] IntPtr /*IShellItemArray* */ itemArray, [In] String verb, [Out] out UInt32 processId);
            IntPtr ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
        }

        [ComImport, Guid("45BA127D-10A8-46EA-8AB7-56EA9078943C")]//Application Activation Manager
        class ApplicationActivationManager : IApplicationActivationManager
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)/*, PreserveSig*/]
            public extern IntPtr ActivateApplication([In] String appUserModelId, [In] String arguments, [In] ActivateOptions options, [Out] out UInt32 processId);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public extern IntPtr ActivateForFile([In] String appUserModelId, [In] IntPtr /*IShellItemArray* */ itemArray, [In] String verb, [Out] out UInt32 processId);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            public extern IntPtr ActivateForProtocol([In] String appUserModelId, [In] IntPtr /* IShellItemArray* */itemArray, [Out] out UInt32 processId);
        }


    }
}
