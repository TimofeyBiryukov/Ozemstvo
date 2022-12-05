using System.Text.Json;
using OzemstvoConsole;

namespace OzemstvoWindowsForm
{
    public partial class Form1 : Form
    {
        public Ozemstvo ozemstvo = new();

        private class RuleSetting
        {
            public string BrowserName { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
            public string Template { get; set; } = "{{url}}";
        }

        public Form1()
        {
            InitializeComponent();
            ozemstvo.Init();

            var defaultBrowserName = Properties.Settings.Default.DefaultBrowserName;
            if (defaultBrowserName is not null)
            {
                var defaultBrowser = ozemstvo.Browsers.Find(b => b.Name.Contains(defaultBrowserName));
                if (defaultBrowser is not null)
                {
                    foreach (var browser in ozemstvo.Browsers)
                    {
                        browser.Default = false;
                    }
                    defaultBrowser.Default = true;
                }
            }

            var rules = JsonSerializer.Deserialize<List<RuleSetting>>(Properties.Settings.Default.Rules);
            if (rules is not null)
            {

                //rules.Add(new RuleSetting
                //{
                //    BrowserName = "Firefox",
                //    Type = Rule.RuleTypes.Host.ToString(),
                //    Data = "youtube.com"
                //});
                //Properties.Settings.Default.Rules = JsonSerializer.Serialize(rules);
                //Properties.Settings.Default.Save();

                foreach ( var rule in rules )
                {
                    var app = ozemstvo.Browsers.Find(x => x.Name.Contains(rule.BrowserName));
                    if (app is not null)
                    {
                        Rule.RuleTypes type = Enum.Parse<Rule.RuleTypes>(rule.Type);
                        ozemstvo.Rules.Add(new Rule(app, type, rule.Data, rule.Template));
                    }
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ClickMe_Click(object sender, EventArgs e)
        {
            var url = textBox1.Text;
            ozemstvo.Run(new Uri(url));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}