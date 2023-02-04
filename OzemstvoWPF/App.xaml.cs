using OzemstvoConsole;
using OzemstvoWPF.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Windows;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //TODO: preventSecond();
        }

        private const string UniqueEventName = "Ozemstvo";
        private readonly Ozemstvo _ozemstvo = new();
        private ObservableCollection<RuleProperty> _rules { get; set; } = new();
        private ObservableCollection<BrowserProperty> _browsers = new();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _ozemstvo.Init();
            LoadRulesProperties();
            LoadBrowsersProperties();

            if (e.Args.Length > 0)
            {
                string target = e.Args[0];
                Uri.TryCreate(target, UriKind.Absolute, out Uri? uri);
                if (uri is null)
                {
                    Console.WriteLine("Invalid URL");
                    Shutdown();
                    return;
                }

                _ozemstvo.Rules = GetRules();
                _ozemstvo.Browsers = GetBrowsers();

                _ozemstvo.Run(new Uri(e.Args[0]));
                Shutdown();
            }
            else
            {
                MainWindow main = new MainWindow(_rules, _browsers);
                main.Show();
            }
        }

        private void preventSecond()
        {
            try
            {
                EventWaitHandle.OpenExisting(UniqueEventName);
                Shutdown();
            }
            catch (WaitHandleCannotBeOpenedException)
            {
                new EventWaitHandle(false, EventResetMode.AutoReset, UniqueEventName);
            }
        }

        public void SaveRulesProperties()
        {
            var rules = JsonSerializer.Serialize(_rules);
            Settings.Default.Rules = rules;
            Settings.Default.Save();
        }

        public void SaveBrowserProperties()
        {
            ObservableCollection<BrowserProperty> browsers = new();
            foreach (var browser in _browsers)
            {
                if (browser.UserDefined)
                {
                    browsers.Add(browser);
                }
            }
            Settings.Default.Browsers = JsonSerializer.Serialize(browsers);
            Settings.Default.Save();
        }

        public void LoadRulesProperties()
        {
            var rules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (rules is not null)
            {
                _rules = rules;
            }
        }

        public void LoadBrowsersProperties()
        {
            foreach (var browser in _ozemstvo.Browsers)
            {
                _browsers.Add(new BrowserProperty
                {
                    Id = browser.Id,
                    Name = browser.Name,
                    Path = browser.Path,
                    UserDefined = false,
                    Default = browser.Default
                });
            }
            var browsers = JsonSerializer.Deserialize<ObservableCollection<BrowserProperty>>(Settings.Default.Browsers.ToString());
            if (browsers is null) return;
            foreach (var browser in browsers)
            {
                if (browser.Default) foreach (var b in _browsers) b.Default = false;
                _browsers.Add(browser);
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

        public List<Rule> GetRules()
        {
            List<Rule> rules = new List<Rule>();
            foreach (var rule in _rules)
            {
                var browser = _ozemstvo.Browsers.Find(b => b.Name == rule.Browser);
                if (browser is null) continue;
                Enum.TryParse(rule.Type, true, out RuleType type);
                rules.Add(
                    new Rule(rule.Name, browser, type, rule.Data, rule.Template, rule.Id));
            }
            return rules;
        }
        
        public List<Browser> GetBrowsers()
        {
            List<Browser> browsers = new List<Browser>();
            foreach (var browser in _browsers)
            {
                browsers.Add(new Browser(browser.Name, browser.Path, browser.Default));
            }
            return browsers;
        }
    }
}
