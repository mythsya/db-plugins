using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using SimpleFollow.Helpers;

namespace SimpleFollow.UI
{
    internal class Config
    {
        public int ServerPort { get; set; }

        private static Window _configWindow;

        public static void CloseWindow()
        {
            _configWindow.Close();
        }

        public static Window GetDisplayWindow()
        {
            try
            {
                if (_configWindow == null)
                {
                    _configWindow = new Window();
                }

                string assemblyPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string xamlPath = Path.Combine(assemblyPath, "Plugins", "SimpleFollow", "UI", "Config.xaml");

                string xamlContent = File.ReadAllText(xamlPath);

                // This hooks up our object with our UserControl DataBinding
                _configWindow.DataContext = Settings.Instance;

                UserControl mainControl = (UserControl)XamlReader.Load(new MemoryStream(Encoding.UTF8.GetBytes(xamlContent)));
                _configWindow.Content = mainControl;
                _configWindow.Width = 300;
                _configWindow.Height = 360;
                _configWindow.MinWidth = 300;
                _configWindow.MinHeight = 360;
                _configWindow.ResizeMode = ResizeMode.CanResizeWithGrip;

                _configWindow.Title = "SimpleFollow";

                _configWindow.Closed += ConfigWindow_Closed;
                Application.Current.Exit += ConfigWindow_Closed;

                return _configWindow;
            }
            catch (Exception ex)
            {
                Logr.Error("Unable to open Config window: " + ex);
            }
            return null;
        }

        private static void ConfigWindow_Closed(object sender, System.EventArgs e)
        {
            Settings.Instance.Save();
            if (_configWindow != null)
            {
                _configWindow.Closed -= ConfigWindow_Closed;
                _configWindow = null;
            }
        }
    }
}