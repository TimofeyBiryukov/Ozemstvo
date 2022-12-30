using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Windows;
using OzemstvoConsole;
using OzemstvoWPF.Controls;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<RuleProperty> Rules { get; set; } = new ObservableCollection<RuleProperty>();

        public readonly Ozemstvo Ozemstvo = new();

        public MainWindow(string[] args)
        {
            Ozemstvo.Init();
            LoadRules();
            if (args.Length > 0)
            {
                OzemstvoRun(args[0]);
                return;
            }

            var chromeIndex = Ozemstvo.Browsers.FindIndex(b => b.Name == "Google Chrome");
            if (chromeIndex > -1)
            {
                Ozemstvo.Browsers[0].Default = false;
                Ozemstvo.Browsers[chromeIndex].Default = true;
            }

            InitializeComponent();
            itemsControlRulesList.ItemsSource = Rules;
        }

        private void RuleItem_OnEdit(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = Rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                new EditorWindow(this, rule).ShowDialog();
            }
        }

        private void RuleItem_OnRemove(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = Rules.First(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                Rules.Remove(rule);
                SaveRules();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new EditorWindow(this).ShowDialog();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            //OzemstvoRun(TestInput.Text);
        }

        private void OzemstvoRun(string url)
        {
            OzemstvoConsole.Ozemstvo.Run(new Uri(url), GetRules(Ozemstvo.Browsers), Ozemstvo.Browsers);
        }

        public void SaveRules()
        {
            var rules = JsonSerializer.Serialize(Rules);
            Settings.Default.Rules = rules;
            Settings.Default.Save();
        }

        public void LoadRules()
        {
            var rules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (rules is not null)
            {
                Rules = rules;
            }
        }

        static public List<Rule> GetRules(List<Browser> browsers)
        {
            List<Rule> rules = new List<Rule>();
            var savedRules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (savedRules is null) return rules;
            foreach (var rule in savedRules)
            {
                var browser = browsers.Find(b => b.Name == rule.Browser);
                if (browser is null) continue;
                Enum.TryParse(rule.Type, true, out RuleType type);
                rules.Add(
                    new Rule(rule.Name, browser, type, rule.Data, rule.Template, rule.Id));
            }
            return rules;
        }
    }
}
