using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
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

        public MainWindow(Ozemstvo ozemstvo, ObservableCollection<RuleProperty> rules)
        {
            _ozemstvo = ozemstvo;
            _rules = rules;
            InitializeComponent();
            itemsControlRulesList.ItemsSource = _rules;
            ObservableCollection<BrowserProperty> browsers = new ObservableCollection<BrowserProperty>();
            foreach (var browser in _ozemstvo.Browsers)
            {
                browsers.Add(new BrowserProperty { Name = browser.Name, Path = browser.Path });
            }
            browsersList.ItemsSource = browsers;
        }

        private void RuleItem_OnEdit(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = _rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                new EditorWindow(_ozemstvo, _rules, rule).ShowDialog();
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new EditorWindow(_ozemstvo, _rules).ShowDialog();
        }
    }
}
