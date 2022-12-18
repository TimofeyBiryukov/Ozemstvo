using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using OzemstvoWPF.Controls;
using Windows.Web.AtomPub;
using static OzemstvoWPF.Controls.RuleItem;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public class RuleProperty
        {
            public string? Id { get; set; } = null;
            public string Name { get; set; } = string.Empty;
            public string Browser { get; set; } = string.Empty;
            public string Data { get; set; } = string.Empty;
            public string Template { get; set; } = "{{url}}";
            public string Type { get; set; } = string.Empty;
        }

        public ObservableCollection<RuleProperty> Rules { get; set; } = new ObservableCollection<RuleProperty>();

        public readonly Ozemstvo Ozemstvo = new();

        public MainWindow()
        {
            Ozemstvo.Init();

            var chromeIndex = Ozemstvo.Browsers.FindIndex(b => b.Name == "Google Chrome");
            if (chromeIndex > -1)
            {
                Ozemstvo.Browsers[0].Default = false;
                Ozemstvo.Browsers[chromeIndex].Default = true;
            }

            var firefox = Ozemstvo.Browsers.Find(x => x.Name.Contains("Firefox"));
            if (firefox is not null)
            {
                Ozemstvo.Rules.Add(new Rule("Youtube", firefox, RuleType.Host, "youtube.com"));
                Ozemstvo.Rules.Add(new Rule("youtu.be", firefox, RuleType.Host, "twitch.tv"));
                Ozemstvo.Rules.Add(new Rule("dezn", firefox, RuleType.Host, "dzen.ru"));
            }

            var edge = Ozemstvo.Browsers.Find(x => x.Name.Contains("Edge"));
            if (edge is not null)
            {
                Ozemstvo.Rules.Add(new Rule("microsoft.com", edge, RuleType.Host, "microsoft.com"));
            }

            var chrome = Ozemstvo.Browsers.Find(x => x.Name.Contains("Google Chrome"));
            if (chrome is not null)
            {
                Ozemstvo.Rules.Add(new Rule("Google Meet", chrome, RuleType.Host, "meet.google.com"));
                Ozemstvo.Rules.Add(new Rule("Tagspace", chrome, RuleType.Host, "tagspace.com", "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"));
            }

            var steam = Ozemstvo.Browsers.Find(x => x.Name.Contains("Steam"));
            if (steam is not null)
            {
                Ozemstvo.Rules.Add(new Rule("Steam", steam, RuleType.Host, "store.steampowered.com", "steam://openurl/{{url}}"));
            }

            foreach (var rule in Ozemstvo.Rules)
            {
                Rules.Add(new RuleProperty
                {
                    Id = rule.Id,
                    Name = rule.Name,
                    Browser = rule.Browser.Name,
                    Data = rule.Data,
                    Template = rule.Template,
                    Type = rule.Type.ToString(),
                });
            }

            InitializeComponent();
            itemsControlRulesList.ItemsSource = Rules;
        }

        private void Rule_OnRemove(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            RuleProperty rule = Rules.Single(r => r.Id == ruleItem.Rule.Id);
            if (rule is not null)
            {
                Rules.Remove(rule);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            new EditorWindow().ShowDialog();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            OzemstvoConsole.Ozemstvo.Run(new Uri(TestInput.Text), Ozemstvo.Rules.ToList(), Ozemstvo.Browsers.ToList());
        }
    }
}
