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
            if (rule is not null)
            {
                new RuleEditorWindow(Rules, Browsers, rule).ShowDialog();
            }
        }

        private void RuleItem_OnRemove(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = Rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                Rules.Remove(rule);
                ((App)Application.Current).SaveRulesProperties();
            }
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            new RuleEditorWindow(Rules, Browsers).ShowDialog();
        }

        private void AddBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            new BrowserEditorWindow(Browsers).ShowDialog();
        }
    }
}
