using OzemstvoConsole;
using OzemstvoWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace OzemstvoWPF
{
    public class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string? stringToValidate = value as string;
            if (string.IsNullOrEmpty(stringToValidate))
            {
                return new ValidationResult(false, "This field is required");
            }
            return ValidationResult.ValidResult;
        }
    }

    public class TemplateValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            string? stringToValidate = value as string;
            ValidationResult result = new ValidationResult(false, "Template must contain {{url}}");
            if (string.IsNullOrEmpty(stringToValidate))
            {
                return result;
            }
            if (!stringToValidate.Contains("{{url}}"))
            {
                return result;
            }
            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// Interaction logic for RuleEditorWindow.xaml
    /// </summary>
    public partial class RuleEditorWindow : Window
    {
        private Ozemstvo _ozemstvo { get; set; }
        private ObservableCollection<RuleProperty> _rules { get; set; }

        public RuleProperty Rule { get; set; } = new();

        public string[] Browsers { get; set; } = Array.Empty<string>();
        public string[] Types { get; set; } = Array.Empty<string>();

        public string TemplateDescription { get; set; } = "Command template, this will be passed to the browser. {{url}} will be replaced with the URL of the page you want to open. It must include {{url}}.";

        public RuleEditorWindow(
            Ozemstvo ozemstvo,
            ObservableCollection<RuleProperty> rules,
            RuleProperty? rule = null)
        {
            _ozemstvo = ozemstvo;
            _rules = rules;
            Browsers = _ozemstvo.Browsers.Select(b => b.Name).ToArray();
            Browser? defaultBrowser = _ozemstvo.Browsers.Where(b => b.Default).FirstOrDefault();
            defaultBrowser ??= _ozemstvo.Browsers.First();
            Types = Enum.GetNames(typeof(RuleType));

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
                Rule.Type = Types.First();
            }

            InitializeComponent();
            updateDataInputLabel();
        }

        private void TypeInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dataInputLabel is null) return;
            updateDataInputLabel();
        }

        private void updateDataInputLabel()
        {
            if (Rule.Type == RuleType.Host.ToString())
            {
                dataInputLabel.Content = "Host to match";
            }
            else if (Rule.Type == RuleType.Path.ToString())
            {
                dataInputLabel.Content = "Path to match";
            }
            else if (Rule.Type == RuleType.Port.ToString())
            {
                dataInputLabel.Content = "Port to match";
            }
            else if (Rule.Type == RuleType.Regex.ToString())
            {
                dataInputLabel.Content = "Regex to match";
            }
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
            if (string.IsNullOrEmpty(Rule.Type))
            {
                MessageBox.Show("Type is required");
                return false;
            }
            if (string.IsNullOrEmpty(Rule.Data))
            {
                MessageBox.Show("Data is required");
                return false;
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
                    rule.Type = Rule.Type;
                    rule.Data = Rule.Data;
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
            Browser browser = _ozemstvo.Browsers.First(b => b.Name == Rule.Browser);
            RuleType ruleType = Enum.TryParse<RuleType>(Rule.Type, out RuleType type) ? type : RuleType.Host;
            OzemstvoConsole.Rule rule = new OzemstvoConsole.Rule(Rule.Name, browser, ruleType, Rule.Data, Rule.Template);
            Ozemstvo.Run(new Uri(Rule.Example), new List<OzemstvoConsole.Rule> { rule }, _ozemstvo.Browsers);
        }
    }
}
