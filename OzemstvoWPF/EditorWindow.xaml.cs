using OzemstvoConsole;
using System;
using System.Collections.Generic;
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

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        private readonly MainWindow _mainWindow;
        public EditorWindow(Rule? rule = null)
        {
            InitializeComponent();
            _mainWindow = (MainWindow)Application.Current.MainWindow;

            OpenInInput.ItemsSource = _mainWindow.ozemstvo.Browsers.Select(b => b.Name).ToArray();
            var defaultBrowser = _mainWindow.ozemstvo.Browsers.Find(b => b.Default);
            if (defaultBrowser is not null)
            {
                OpenInInput.SelectedItem = defaultBrowser.Name;
            }
            TypeInput.ItemsSource = Enum.GetNames(typeof(Rule.RuleTypes));
            TypeInput.SelectedIndex = 0;

            if (rule is not null)
            {
                RuleNameInput.Text = rule.Name;
                OpenInInput.SelectedValue = rule.Browser.Name;
                TypeInput.SelectedItem = rule.Type.ToString();
                if (rule.Type == Rule.RuleTypes.Host)
                {
                    TypeInput.SelectedValue = "Host";
                    DataInput.Text = rule.Host;
                }
                else if (rule.Type == Rule.RuleTypes.Regex)
                {
                    TypeInput.SelectedValue = "Regex";
                    if (rule.Regex is not null)
                    {
                        DataInput.Text = rule.Regex.ToString();
                    }
                }
                TemplateInput.Text = rule.Template;
            }
        }

        private void TypeInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TypeInput.SelectedItem.ToString() == "Host")
            {
                DataInputLabel.Content = "Host to match";
            }
            else if (TypeInput.SelectedItem.ToString() == "Regex")
            {
                DataInputLabel.Content = "Regex to match";
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var name = RuleNameInput.Text;
            var browser = _mainWindow.ozemstvo.Browsers.Find(b => b.Name == OpenInInput.SelectedItem.ToString());
            if (browser is null) throw new Exception();
            Rule.RuleTypes type = Rule.RuleTypes.Host;
            if (TypeInput.SelectedItem.ToString() == "Regex")
            {
                type = Rule.RuleTypes.Regex;
            }
            var data = DataInput.Text;
            var template = TemplateInput.Text;
            var rule = new Rule(name, browser, type, data, template);
            _mainWindow.ozemstvo.Rules.Add(rule);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
