using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OzemstvoConsole;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private class RuleSetting
        {
            public string BrowserName { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
            public string Template { get; set; } = "{{url}}";
        }

        public Ozemstvo ozemstvo = new();

        public MainWindow()
        {
            InitializeComponent();
            ozemstvo.Init();

            var chromeIndex = ozemstvo.Browsers.FindIndex(b => b.Name == "Google Chrome");
            if (chromeIndex > -1)
            {
                ozemstvo.Browsers[0].Default = false;
                ozemstvo.Browsers[chromeIndex].Default = true;
            }

            var firefox = ozemstvo.Browsers.Find(x => x.Name.Contains("Firefox"));
            if (firefox is not null)
            {
                ozemstvo.Rules.Add(new Rule("Youtube", firefox, Rule.RuleTypes.Host, "youtube.com"));
                ozemstvo.Rules.Add(new Rule("youtu.be", firefox, Rule.RuleTypes.Host, "twitch.tv"));
                ozemstvo.Rules.Add(new Rule("dezn", firefox, Rule.RuleTypes.Host, "dzen.ru"));
            }

            var edge = ozemstvo.Browsers.Find(x => x.Name.Contains("Edge"));
            if (edge is not null)
            {
                ozemstvo.Rules.Add(new Rule("microsoft.com", edge, Rule.RuleTypes.Host, "microsoft.com"));
            }

            var chrome = ozemstvo.Browsers.Find(x => x.Name.Contains("Google Chrome"));
            if (chrome is not null)
            {
                ozemstvo.Rules.Add(new Rule("Google Meet", chrome, Rule.RuleTypes.Host, "meet.google.com"));
                ozemstvo.Rules.Add(new Rule("Tagspace", chrome, Rule.RuleTypes.Host, "tagspace.com", "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"));
            }

            var steam = ozemstvo.Browsers.Find(x => x.Name.Contains("Steam"));
            if (steam is not null)
            {
                ozemstvo.Rules.Add(new Rule("Steam", steam, Rule.RuleTypes.Host, "store.steampowered.com", "steam://openurl/{{url}}"));
            }

            itemControlRulesList.ItemsSource = ozemstvo.Rules;
        }
    }
}
