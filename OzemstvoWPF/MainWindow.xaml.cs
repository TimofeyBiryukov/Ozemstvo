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
using static OzemstvoWPF.Controls.RuleItem;

namespace OzemstvoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class OzemstvoObservable : Ozemstvo
        {
            new public ObservableCollection<Rule> Rules = new();
        }

        public readonly OzemstvoObservable Ozemstvo = new();

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

            InitializeComponent();
            itemsControlRulesList.ItemsSource = Ozemstvo.Rules;
        }

        private void Rule_OnRemove(object sender, RoutedEventArgs e)
        {
            RuleItem ruleItem = (RuleItem)sender;
            Rule rule = Ozemstvo.Rules.Single(r => r.Name.Contains(ruleItem.Rule.Name));
            if (rule is not null)
            {
                Ozemstvo.Rules.Remove(rule);
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
