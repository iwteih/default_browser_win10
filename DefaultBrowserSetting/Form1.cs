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

            if (!SetDefaultBrowserAtStartup())
            {
                return;
            }

            if (CloseWindowsAutomatically())
            {
                this.WindowState = FormWindowState.Minimized;
                this.ShowInTaskbar = false;
            }

            ShowDefaultAppsUI();
            Process p = PopBrowserList(1500);

            SetDefaultBrowser(p);

            if (CloseWindowsAutomatically())
            {
                Close();
            }
        }
        private bool IsWindows10()
        {
            var reg = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            string productName = (string)reg.GetValue("ProductName");

            return productName.StartsWith("Windows 10");
        }
        private int GetBrowerIndex()
        {
            return int.Parse(ConfigurationManager.AppSettings["desiredBrowerIndex"]);
        }

        private bool CloseWindowsAutomatically()
        {
            return bool.Parse(ConfigurationManager.AppSettings["closeWindowsAutomatically"]);
        }
        private bool SetDefaultBrowserAtStartup()
        {
            return bool.Parse(ConfigurationManager.AppSettings["setDefaultBrowserAtStartup"]);
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
            ShowDefaultAppsUI();
            Process p = PopBrowserList(500);
            SetDefaultBrowser(p);
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
