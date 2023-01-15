using OzemstvoWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for BrowserEditorWindow.xaml
    /// </summary>
    public partial class BrowserEditorWindow : Window
    {
        private ObservableCollection<BrowserProperty> _browsers { get; set; } = new ObservableCollection<BrowserProperty>();

        public BrowserProperty Browser { get; set; } = new();

        public BrowserEditorWindow(
            ObservableCollection<BrowserProperty> browsers,
            BrowserProperty? browser = null)
        {
            _browsers = browsers;

            if (browser is not null)
            {
                Browser = browser;
            }

            InitializeComponent();
        }

        private void browseButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".exe";
            dlg.Filter = "Executable files (*.exe)|*.exe";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                pathInput.Text = dlg.FileName;
                Browser.Path = dlg.FileName;
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Browser.Id))
            {
                AddBrowser();
            }
            else
            {
                BrowserProperty? browser = _browsers.FirstOrDefault(b => b.Id == Browser.Id);
                if (browser is not null)
                {
                    browser.Name = Browser.Name;
                    browser.Path = Browser.Path;
                }
                else
                {
                    AddBrowser();
                }
            }
            ((App)Application.Current).SaveBrowserProperties();
            Close();
        }

        private void AddBrowser()
        {
            Browser.Id = Guid.NewGuid().ToString();
            _browsers.Add(Browser);
        }
    }
}
