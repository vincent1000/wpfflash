using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;

namespace WpfApp3
{
    public class Helper
    {
        private static ResourceDictionary mAppResourceDictionary;
        private static readonly List<Uri> _staticResourceUris = new List<Uri>();
        public static void LoadTheme(bool isHighContrast)
        {
            mAppResourceDictionary = Application.Current.Resources.MergedDictionaries[0];
            LoadBrushResource(isHighContrast);
        }

        public static void OnHighContrastChanged(bool isHighContrast)
        {
            LoadBrushResource(isHighContrast);
        }

        private static  void LoadBrushResource(bool isHighContrast)
        {
            mAppResourceDictionary.MergedDictionaries.Clear();
            var resource = new ResourceDictionary();
            resource.Source = isHighContrast
                ? new Uri("pack://application:,,,/WpfApp3;component/dark.xaml", UriKind.RelativeOrAbsolute)
                : new Uri("pack://application:,,,/WpfApp3;component/default.xaml", UriKind.RelativeOrAbsolute);

            mAppResourceDictionary.MergedDictionaries.Add(resource);
            var resource1 = new ResourceDictionary();
            resource1.Source = new Uri("pack://application:,,,/WpfApp3;component/Dictionary1.xaml", UriKind.RelativeOrAbsolute);
            mAppResourceDictionary.MergedDictionaries.Add(resource1);
        }
    }
}
