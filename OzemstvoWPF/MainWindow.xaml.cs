using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public class RuleProperty
        {
            public string? Id { get; set; } = null;
            public string Name { get; set; } = string.Empty;
            public string Browser { get; set; } = string.Empty;
            public string Data { get; set; } = string.Empty;
            public string Template { get; set; } = "{{url}}";
            public string Type { get; set; } = string.Empty;
        }

        public ObservableCollection<RuleProperty> Rules { get; set; } = new ObservableCollection<RuleProperty>();

        public readonly Ozemstvo Ozemstvo = new();

        public MainWindow()
        {
            Ozemstvo.Init();

            var chromeIndex = Ozemstvo.Browsers.FindIndex(b => b.Name == "Google Chrome");
            if (chromeIndex > -1)
            {
                Ozemstvo.Browsers[0].Default = false;
                Ozemstvo.Browsers[chromeIndex].Default = true;
            }

            var firefox = Ozemstvo.Browsers.Find(x => x.Name.Contains("Firefox"));
            if (firefox is not null)
            {
                Rules.Add(new RuleProperty
                {
                    Name = "Youtube",
                    Browser = firefox.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "youtube.com"
                });
                Rules.Add(new RuleProperty
                {
                    Name = "youtu.be",
                    Browser = firefox.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "youtu.be"
                });
                Rules.Add(new RuleProperty
                {
                    Name = "Twitch",
                    Browser = firefox.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "twitch.tv"
                });
                Rules.Add(new RuleProperty
                {
                    Name = "dzen",
                    Browser = firefox.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "dzen.ru"
                });
            }

            var edge = Ozemstvo.Browsers.Find(x => x.Name.Contains("Edge"));
            if (edge is not null)
            {
                Rules.Add(new RuleProperty
                {
                    Name = "microsoft.com",
                    Browser = edge.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "microsoft.com"
                });
            }

            var chrome = Ozemstvo.Browsers.Find(x => x.Name.Contains("Google Chrome"));
            if (chrome is not null)
            {
                Rules.Add(new RuleProperty
                {
                    Name = "Google Meet",
                    Browser = chrome.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "meet.google.com"
                });
                Rules.Add(new RuleProperty
                {
                    Name = "Tagspace",
                    Browser = chrome.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "tagspace.com",
                    Template = "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"
                });
            }

            var steam = Ozemstvo.Browsers.Find(x => x.Name.Contains("Steam"));
            if (steam is not null)
            {
                Rules.Add(new RuleProperty
                {
                    Name = "Steam",
                    Browser = steam.Name,
                    Type = RuleType.Host.ToString(),
                    Data = "store.steampowered.com",
                    Template = "steam://openurl/{{url}}"
                });
            }

            InitializeComponent();
            itemsControlRulesList.ItemsSource = Rules;
        }

        private void Rule_OnRemove(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = Rules.Single(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                Rules.Remove(rule);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new EditorWindow().ShowDialog();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            List<Rule> rules = new List<Rule>();
            foreach (var rule in Rules)
            {
                var browser = Ozemstvo.Browsers.Find(b => b.Name == rule.Browser);
                if (browser is null) continue;
                Enum.TryParse(rule.Type, true, out RuleType type);
                rules.Add(
                    new Rule(rule.Name, browser, type, rule.Data, rule.Template, rule.Id));
            }
            OzemstvoConsole.Ozemstvo.Run(new Uri(TestInput.Text), rules, Ozemstvo.Browsers);
        }
    }
}
