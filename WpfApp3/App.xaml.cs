using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            ResourceDictionary resource = new ResourceDictionary();
            resource.Source = new Uri(@"pack://application:,,,/WpfApp3;component/Dictionary.xaml", UriKind.RelativeOrAbsolute);
            this.Resources.MergedDictionaries.Add(resource);

            Helper.LoadTheme(SystemParameters.HighContrast);
            base.OnStartup(e);
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
            MainWindow message = new MainWindow();
        }
        private void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == null) return;
            if (e.PropertyName.Equals("HighContrast", StringComparison.OrdinalIgnoreCase))
            {
                Helper.OnHighContrastChanged(SystemParameters.HighContrast);
            }
        }
    }

}
