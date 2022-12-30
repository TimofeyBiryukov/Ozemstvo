using OzemstvoConsole;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
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
        private ObservableCollection<RuleProperty> _rules { get; set; } = new ObservableCollection<RuleProperty>();


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _ozemstvo.Init();
            LoadRulesProperties();

            // TODO: ask user about default browser
            var chromeIndex = _ozemstvo.Browsers.FindIndex(b => b.Name == "Google Chrome");
            if (chromeIndex > -1)
            {
                _ozemstvo.Browsers[0].Default = false;
                _ozemstvo.Browsers[chromeIndex].Default = true;
            }

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
                _ozemstvo.Run(new Uri(e.Args[0]));
                Shutdown();
            }
            else
            {
                MainWindow main = new MainWindow(_ozemstvo, _rules);
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

        public void LoadRulesProperties()
        {
            var rules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (rules is not null)
            {
                _rules = rules;
            }
        }

        static public List<OzemstvoConsole.Rule> GetRules(List<Browser> browsers)
        {
            List<OzemstvoConsole.Rule> rules = new List<OzemstvoConsole.Rule>();
            var savedRules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (savedRules is null) return rules;
            foreach (var rule in savedRules)
            {
                var browser = browsers.Find(b => b.Name == rule.Browser);
                if (browser is null) continue;
                Enum.TryParse(rule.Type, true, out RuleType type);
                rules.Add(
                    new OzemstvoConsole.Rule(rule.Name, browser, type, rule.Data, rule.Template, rule.Id));
            }
            return rules;
        }

        public List<OzemstvoConsole.Rule> GetRules()
        {
            List<OzemstvoConsole.Rule> rules = new List<OzemstvoConsole.Rule>();
            foreach (var rule in _rules)
            {
                var browser = _ozemstvo.Browsers.Find(b => b.Name == rule.Browser);
                if (browser is null) continue;
                Enum.TryParse(rule.Type, true, out RuleType type);
                rules.Add(
                    new OzemstvoConsole.Rule(rule.Name, browser, type, rule.Data, rule.Template, rule.Id));
            }
            return rules;
        }
    }
}
