using OzemstvoConsole;
using OzemstvoWPF.Controls;
using OzemstvoWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for RuleEditorWindow.xaml
    /// </summary>
    public partial class RuleEditorWindow : Window
    {
        private ObservableCollection<RuleProperty> _rules { get; set; }
        private ObservableCollection<BrowserProperty> _browsers { get; set; }

        public RuleProperty Rule { get; set; } = new();

        public string[] Browsers { get; set; } = Array.Empty<string>();

        public string TemplateDescription { get; set; } = "Command template, this will be passed to the browser. {{url}} will be replaced with the URL of the page you want to open. It must include {{url}}.";

        public RuleEditorWindow(
            ObservableCollection<RuleProperty> rules,
            ObservableCollection<BrowserProperty> browsers,
            RuleProperty? rule = null)
        {
            _rules = rules;
            _browsers = browsers;
            Browsers = _browsers.Select(b => b.Name).ToArray();
            BrowserProperty? defaultBrowser = _browsers.Where(b => b.Default).FirstOrDefault();
            defaultBrowser ??= _browsers.First();

            if (rule is not null)
            {
                Rule = rule;
            }
            else
            {
                // set defaults for empty rule
                if (defaultBrowser is not null)
                {
                    Rule.Browser = defaultBrowser.Name;
                }
                Rule.Matches.Add(new MatchProperty
                {
                    Id = Guid.NewGuid().ToString()
                });
            }

            InitializeComponent();
        }
        
        private bool Validate()
        {
            if (string.IsNullOrEmpty(Rule.Name))
            {
                MessageBox.Show("Name is required");
                return false;
            }
            if (string.IsNullOrEmpty(Rule.Browser))
            {
                MessageBox.Show("Browser is required");
                return false;
            }
            if (Rule.Matches.Count == 0)
            {
                MessageBox.Show("At least one match is required");
                return false;
            }
            else
            {
                foreach (var match in Rule.Matches)
                {
                    if (string.IsNullOrEmpty(match.Value))
                    {
                        MessageBox.Show("Match value is required");
                        return false;
                    }
                    if (string.IsNullOrEmpty(match.Type))
                    {
                        MessageBox.Show("Match type is required");
                        return false;
                    }
                }
            }
            if (string.IsNullOrEmpty(Rule.Template))
            {
                MessageBox.Show("Template is required");
                return false;
            }
            if (!Rule.Template.Contains("{{url}}"))
            {
                MessageBox.Show("Template must contain {{url}}");
                return false;
            }
            try
            {
                if (!string.IsNullOrEmpty(Rule.Example))
                {
                    new Uri(Rule.Example);
                }
            }
            catch
            {
                MessageBox.Show("Example must be a valid URL");
                return false;
            }
            return true;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;

            if (string.IsNullOrEmpty(Rule.Id))
            {
                AddNewRule();
            }
            else
            {
                RuleProperty? rule = _rules.First(b => b.Id == Rule.Id);
                if (rule is not null)
                {
                    rule.Name = Rule.Name;
                    rule.Browser = Rule.Browser;
                    rule.Matches = Rule.Matches;
                    rule.Template = Rule.Template;
                }
                else
                {
                    AddNewRule();
                }
            }
            ((App)Application.Current).SaveRulesProperties();
            Close();
        }

        private void AddNewRule()
        {
            Rule.Id = Guid.NewGuid().ToString();
            _rules.Add(Rule);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validate()) return;
            if (string.IsNullOrEmpty(Rule.Example))
            {
                MessageBox.Show("Example is required");
                return;
            }
            BrowserProperty browserProperty = _browsers.First(b => b.Name == Rule.Browser);
            Browser browser = new(browserProperty.Name, browserProperty.Path);

            List<Match> matches = new();
            foreach (var match in Rule.Matches)
            {
                MatchType matchType = Enum.TryParse<MatchType>(match.Type, out MatchType type) ? type : MatchType.Host;
                matches.Add(new Match(match.Value, matchType));
            }
            
            OzemstvoConsole.Rule rule = new OzemstvoConsole.Rule(Rule.Name, browser, matches, Rule.Template);
            try
            {
                Ozemstvo.Run(new Uri(Rule.Example), new List<OzemstvoConsole.Rule> { rule }, new List<Browser> { browser });
            }
            catch
            {
                MessageBox.Show("Failed to open URL");
            }
        }

        private void addMatchButton_Click(object sender, RoutedEventArgs e)
        {
            Rule.Matches.Add(new MatchProperty
            {
                Id = Guid.NewGuid().ToString()
            });
        }

        private void MatchItem_Remove(object sender, RoutedEventArgs e)
        {
            MatchItem matchItem = (MatchItem)sender;
            MatchProperty matchProperty = Rule.Matches.First(m => m.Id == matchItem.Match.Id);
            if (matchProperty is null) return;
            Rule.Matches.Remove(matchProperty);
            if (Rule.Matches.Count() < 1)
            {
                Rule.Matches.Add(new MatchProperty
                {
                    Id = Guid.NewGuid().ToString()
                });
                return;
            }
        }
    }
}
