using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using OzemstvoConsole;
using OzemstvoWPF.Controls;
using OzemstvoWPF.Models;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<RuleProperty> Rules { get; set; } = new ObservableCollection<RuleProperty>();
        public ObservableCollection<BrowserProperty> Browsers { get; set; } = new ObservableCollection<BrowserProperty>();

        public BrowserProperty? SelectedBrowser { get; set; } = null;

        public MainWindow(
            ObservableCollection<RuleProperty> rules,
            ObservableCollection<BrowserProperty> browsers)
        {
            Rules = rules;
            Browsers = browsers;
            InitializeComponent();
        }

        private void RuleItem_OnEdit(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = Rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is null) return;
            new RuleEditorWindow(Rules, Browsers, rule).ShowDialog();
        }

        private void RuleItem_OnRemove(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = Rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is null) return;
            Rules.Remove(rule);
            ((App)Application.Current).SaveRulesProperties();
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            new RuleEditorWindow(Rules, Browsers).ShowDialog();
        }

        private void AddBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            new BrowserEditorWindow(Browsers).ShowDialog();
        }

        private void browsersList_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedBrowser = (BrowserProperty)browsersList.SelectedItem;
            if (SelectedBrowser is null)
            {
                RemoveBrowserButton.IsEnabled = false;
                MakeDefaultBrowserButton.IsEnabled = false;
            }
            else
            {
                RemoveBrowserButton.IsEnabled = SelectedBrowser.UserDefined;
                MakeDefaultBrowserButton.IsEnabled = true;
            }
        }

        private void RemoveBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBrowser is null) return;
            BrowserProperty browser = Browsers.First(b => b.Id == SelectedBrowser.Id);
            if (browser is null) return;
            Browsers.Remove(browser);
            SelectedBrowser = null;
            RemoveBrowserButton.IsEnabled = false;
            ((App)Application.Current).SaveBrowserProperties();
        }

        private void MakeDefaultBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedBrowser is null) return;
            BrowserProperty browser = Browsers.First(b => b.Id == SelectedBrowser.Id);
            if (browser is null) return;
            foreach (BrowserProperty b in Browsers)
            {
                b.SetDefault(false);
            }
            browser.SetDefault(true);
            ((App)Application.Current).SaveBrowserProperties();
        }
    }
}
