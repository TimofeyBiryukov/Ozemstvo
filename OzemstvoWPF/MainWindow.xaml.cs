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

        private List<RuleSetting> rules = new List<RuleSetting>();

        public MainWindow()
        {
            InitializeComponent();

            rulesGrid.ItemsSource = rules;

            rules.Add(new RuleSetting
            {
                BrowserName = "FireFox",
                Type = "Host",
                Data = "youtube.com"
            });
            rules.Add(new RuleSetting
            {
                BrowserName = "Google Chrome",
                Type = "Host",
                Data = "google.com"
            });
        }

        private void rulesGrid_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            var data = JsonSerializer.Serialize(rules);
            debugBlock.Text = data;
        }
    }
}
