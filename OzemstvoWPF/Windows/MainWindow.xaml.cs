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
        private Ozemstvo _ozemstvo { get; set; }
        private ObservableCollection<RuleProperty> _rules { get; set; } = new ObservableCollection<RuleProperty>();
        private ObservableCollection<BrowserProperty> _browsers { get; set; } = new ObservableCollection<BrowserProperty>();

        public MainWindow(
            Ozemstvo ozemstvo,
            ObservableCollection<RuleProperty> rules,
            ObservableCollection<BrowserProperty> browsers)
        {
            _ozemstvo = ozemstvo;
            _rules = rules;
            _browsers = browsers;
            InitializeComponent();
            itemsControlRulesList.ItemsSource = _rules;
            browsersList.ItemsSource = _browsers;
        }

        private void RuleItem_OnEdit(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = _rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                new RuleEditorWindow(_rules, _browsers, rule).ShowDialog();
            }
        }

        private void RuleItem_OnRemove(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = _rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                _rules.Remove(rule);
                ((App)Application.Current).SaveRulesProperties();
            }
        }

        private void AddRuleButton_Click(object sender, RoutedEventArgs e)
        {
            new RuleEditorWindow(_rules, _browsers).ShowDialog();
        }

        private void AddBrowserButton_Click(object sender, RoutedEventArgs e)
        {
            new BrowserEditorWindow(_browsers).ShowDialog();
        }
    }
}
