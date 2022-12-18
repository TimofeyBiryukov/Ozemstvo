using OzemstvoConsole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;
using static OzemstvoWPF.MainWindow;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        public OzemstvoObservable Ozemstvo { get; set; }

        public class RuleData
        {
            public RuleData() { }
            public string Name { get; set; } = string.Empty;
            public string Browser { get; set; } = string.Empty;
            public string Data { get; set; } = string.Empty;
            public string Template { get; set; } = "{{url}}";
            public string Type { get; set; } = string.Empty;
        }

        public RuleData RuleForm { get; set; } = new();

        public string[] Browsers { get; set; } = Array.Empty<string>();
        public string[] Types { get; set; } = Array.Empty<string>();

        public string DataInputLabel { get; set; } = "Host to match";

        public EditorWindow(Rule? rule = null)
        {
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            Ozemstvo = mainWindow.Ozemstvo;

            Browsers = Ozemstvo.Browsers.Select(b => b.Name).ToArray();
            Browser? defaultBrowser = Ozemstvo.Browsers.Where(b => b.Default).FirstOrDefault();
            defaultBrowser ??= Ozemstvo.Browsers.First();
            if (defaultBrowser is not null)
            {
                RuleForm.Browser = defaultBrowser.Name;
            }

            Types = Enum.GetNames(typeof(RuleTypes));
            RuleForm.Type = Types.First();

            if (rule is not null)
            {
                RuleForm.Name = rule.Name;
                RuleForm.Browser = rule.Browser.Name;
                RuleForm.Type = rule.Type.ToString();
                RuleForm.Data = rule.Data;
                RuleForm.Template = rule.Template;
            }

            InitializeComponent();
        }

        private void TypeInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RuleForm.Type == RuleTypes.Host.ToString())
            {
                DataInputLabel = "Host to match";
            }
            else if (RuleForm.Type == RuleTypes.Regex.ToString())
            {
                DataInputLabel = "Regex to match";
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine(RuleForm.Name);
            //var name = ruleNameInput.Text;
            //var browser = Ozemstvo.Browsers.Find(b => b.Name == openInInput.SelectedItem.ToString());
            //if (browser is null) throw new Exception();
            //RuleTypes type = RuleTypes.Host;
            //if (RuleForm.Type == RuleTypes.Regex.ToString())
            //{
            //    type = RuleTypes.Regex;
            //}
            //var data = dataInput.Text;
            //var template = templateInput.Text;
            //var rule = new Rule(name, browser, type, data, template);
            //Ozemstvo.Rules.Add(rule);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
