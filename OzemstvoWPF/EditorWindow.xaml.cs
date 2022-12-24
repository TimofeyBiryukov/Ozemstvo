using OzemstvoConsole;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static OzemstvoWPF.MainWindow;

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
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        private MainWindow _mainWindow;

        public RuleProperty Rule { get; set; } = new();

        public string[] Browsers { get; set; } = Array.Empty<string>();
        public string[] Types { get; set; } = Array.Empty<string>();

        public string DataInputLabel { get; set; } = "Host to match";
        public string TemplateDescription { get; set; } = "Command template, this will be passed to the browser. {{url}} will be replaced with the URL of the page you want to open. It must include {{url}}.";

        public EditorWindow(MainWindow mainWindow, RuleProperty? rule = null)
        {
            _mainWindow = mainWindow;
            Browsers = _mainWindow.Ozemstvo.Browsers.Select(b => b.Name).ToArray();
            Browser? defaultBrowser = _mainWindow.Ozemstvo.Browsers.Where(b => b.Default).FirstOrDefault();
            defaultBrowser ??= _mainWindow.Ozemstvo.Browsers.First();
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
        }

        private void TypeInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Rule.Type == RuleType.Host.ToString())
            {
                DataInputLabel = "Host to match";
            }
            else if (Rule.Type == RuleType.Regex.ToString())
            {
                DataInputLabel = "Regex to match";
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(Rule.Id))
            {
                AddNewRule();
            }
            else
            {
                RuleProperty? rule = _mainWindow.Rules.First(b => b.Id == Rule.Id);
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
            _mainWindow.SaveRules();
            //Close();
        }

        private void AddNewRule()
        {
            Rule.Id = Guid.NewGuid().ToString();
            //_mainWindow.Rules.Add(Rule);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
