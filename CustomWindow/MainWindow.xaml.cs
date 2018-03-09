using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CustomWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly List<ResourceDictionary> _resourceDictionaries = new List<ResourceDictionary>();

        public MainWindow()
        {
            InitializeComponent();

            InitializeThemes();
        }


        private void InitializeThemes()
        {
            foreach (var resourceDictionary in Application.Current.Resources.MergedDictionaries.ToArray())
            {
                if (resourceDictionary.Source.OriginalString == "Themes/DarkStyle.xaml" &&
                    ThemeToggleSwitch.IsChecked == false)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(resourceDictionary);
                }
                _resourceDictionaries.Add(resourceDictionary);
            }
        }



        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            ResourceDictionary darkResourceDictionary = _resourceDictionaries.FirstOrDefault(x => x.Source.OriginalString == "Themes/DarkStyle.xaml");
            ResourceDictionary lightResourceDictionary = _resourceDictionaries.FirstOrDefault(x => x.Source.OriginalString == "Themes/LightStyle.xaml");

            ResourceDictionary removedStyleDictionary;
            ResourceDictionary addedStyleDictionary;

            if (ThemesToggleSwitch.IsOn)
            {
                addedStyleDictionary = darkResourceDictionary;
                removedStyleDictionary = lightResourceDictionary;
            }
            else
            {
                addedStyleDictionary = lightResourceDictionary;
                removedStyleDictionary = darkResourceDictionary;
            }

            Application.Current.Resources.MergedDictionaries.Add(addedStyleDictionary);

            Application.Current.Resources.MergedDictionaries.Remove(removedStyleDictionary);
        }
    }
}
