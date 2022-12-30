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
            //preventSecond();
        }

        private const string UniqueEventName = "Ozemstvo";
        public ObservableCollection<RuleProperty> Rules { get; set; } = new ObservableCollection<RuleProperty>();
        public Ozemstvo Ozemstvo = new();

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Ozemstvo.Init();
            
            if (e.Args.Length > 0)
            {
                Ozemstvo.Rules = GetRules(Ozemstvo.Browsers);
                Ozemstvo.Run(new Uri(e.Args[0]));
            }
            else
            {
                MainWindow main = new MainWindow(e.Args);
                main.Show();
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

        public void SaveRules()
        {
            var rules = JsonSerializer.Serialize(Rules);
            Settings.Default.Rules = rules;
            Settings.Default.Save();
        }

        public void LoadRules()
        {
            var rules = JsonSerializer.Deserialize<ObservableCollection<RuleProperty>>(Settings.Default.Rules.ToString());
            if (rules is not null)
            {
                Rules = rules;
            }
        }
    }
}
