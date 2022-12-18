using OzemstvoConsole;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using static OzemstvoWPF.MainWindow;

namespace OzemstvoWPF
{
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

        public EditorWindow(RuleProperty? rule = null)
        {
            _mainWindow = (MainWindow)Application.Current.MainWindow;
            Ozemstvo Ozemstvo = _mainWindow.Ozemstvo;

            Browsers = Ozemstvo.Browsers.Select(b => b.Name).ToArray();
            Browser? defaultBrowser = Ozemstvo.Browsers.Where(b => b.Default).FirstOrDefault();
            defaultBrowser ??= Ozemstvo.Browsers.First();
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
            Browser browser = _mainWindow.Ozemstvo.Browsers.First(b => b.Name == Rule.Browser);
            Enum.TryParse(Rule.Type, true, out RuleType type);
            if (browser is null)
            {
                //throw?
                return;
            }

            if (Rule.Id is not null)
            {
                // update rule
                Rule? rule = _mainWindow.Ozemstvo.Rules.First(b => b.Id == Rule.Id);
                if (rule is not null)
                {
                    rule.Name = Rule.Name;
                    rule.Browser = browser;
                    rule.Type = type;
                    rule.Data = Rule.Data;
                    rule.Template = Rule.Template;
                }
                else
                {
                    _mainWindow.Rules.Add(Rule);
                }
            }
            else
            {
                _mainWindow.Rules.Add(Rule);
                
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
