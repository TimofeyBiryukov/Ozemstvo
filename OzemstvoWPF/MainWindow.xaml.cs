using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using OzemstvoConsole;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class RuleSetting
        {
            public string BrowserName { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
            public string Template { get; set; } = "{{url}}";
        }

        public Ozemstvo ozemstvo = new();

        public MainWindow()
        {
            InitializeComponent();
            ozemstvo.Init();
            //rulesGrid.ItemsSource = ozemstvo.Rules;
        }
    }
}
