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
            InitializeComponent();
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
                Ozemstvo.Rules.Add(new Rule("Youtube", firefox, Rule.RuleTypes.Host, "youtube.com"));
                Ozemstvo.Rules.Add(new Rule("youtu.be", firefox, Rule.RuleTypes.Host, "twitch.tv"));
                Ozemstvo.Rules.Add(new Rule("dezn", firefox, Rule.RuleTypes.Host, "dzen.ru"));
            }

            var edge = Ozemstvo.Browsers.Find(x => x.Name.Contains("Edge"));
            if (edge is not null)
            {
                Ozemstvo.Rules.Add(new Rule("microsoft.com", edge, Rule.RuleTypes.Host, "microsoft.com"));
            }

            var chrome = Ozemstvo.Browsers.Find(x => x.Name.Contains("Google Chrome"));
            if (chrome is not null)
            {
                Ozemstvo.Rules.Add(new Rule("Google Meet", chrome, Rule.RuleTypes.Host, "meet.google.com"));
                Ozemstvo.Rules.Add(new Rule("Tagspace", chrome, Rule.RuleTypes.Host, "tagspace.com", "--profile-email=\"timofeybiryukov@tagspace.com\" {{url}}"));
            }

            var steam = Ozemstvo.Browsers.Find(x => x.Name.Contains("Steam"));
            if (steam is not null)
            {
                Ozemstvo.Rules.Add(new Rule("Steam", steam, Rule.RuleTypes.Host, "store.steampowered.com", "steam://openurl/{{url}}"));
            }

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
            Ozemstvo.Run(new Uri(TestInput.Text));
        }
    }
}
