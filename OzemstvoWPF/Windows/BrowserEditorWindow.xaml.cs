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
using System.Windows.Shapes;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for BrowserEditorWindow.xaml
    /// </summary>
    public partial class BrowserEditorWindow : Window
    {
        public BrowserEditorWindow()
        {
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
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void submitButton_Click(object sender, RoutedEventArgs e)
        {
            if (nameInput.Text.Length > 0 && pathInput.Text.Length > 0)
            {
                //((App)Application.Current).AddBrowser(nameInput.Text, pathInput.Text);
                //Close();
            }
        }
    }
}
