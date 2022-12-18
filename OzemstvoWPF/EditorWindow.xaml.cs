﻿using OzemstvoConsole;
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
using static OzemstvoWPF.MainWindow;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        private OzemstvoObservable _ozemstvo { get; set; }

        public EditorWindow(Rule? rule = null)
        {
            InitializeComponent();
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            _ozemstvo = mainWindow.Ozemstvo;

            openInInput.ItemsSource = _ozemstvo.Browsers.Select(b => b.Name).ToArray();
            var defaultBrowser = _ozemstvo.Browsers.Find(b => b.Default);
            if (defaultBrowser is not null)
            {
                openInInput.SelectedItem = defaultBrowser.Name;
            }
            typeInput.ItemsSource = Enum.GetNames(typeof(Rule.RuleTypes));
            typeInput.SelectedIndex = 0;

            if (rule is not null)
            {
                ruleNameInput.Text = rule.Name;
                openInInput.SelectedValue = rule.Browser.Name;
                typeInput.SelectedItem = rule.Type.ToString();
                dataInput.Text = rule.Data;
                typeInput.Text = rule.Type.ToString();
                templateInput.Text = rule.Template;
            }
        }

        private void TypeInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (typeInput.SelectedItem.ToString() == Rule.RuleTypes.Host.ToString())
            {
                dataInputLabel.Content = "Host to match";
            }
            else if (typeInput.SelectedItem.ToString() == Rule.RuleTypes.Regex.ToString())
            {
                dataInputLabel.Content = "Regex to match";
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            var name = ruleNameInput.Text;
            var browser = _ozemstvo.Browsers.Find(b => b.Name == openInInput.SelectedItem.ToString());
            if (browser is null) throw new Exception();
            Rule.RuleTypes type = Rule.RuleTypes.Host;
            if (typeInput.SelectedItem.ToString() == Rule.RuleTypes.Regex.ToString())
            {
                type = Rule.RuleTypes.Regex;
            }
            var data = dataInput.Text;
            var template = templateInput.Text;
            var rule = new Rule(name, browser, type, data, template);
            _ozemstvo.Rules.Add(rule);
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
