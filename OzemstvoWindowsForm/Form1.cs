using System.Configuration;
using System.Text.Json;
using OzemstvoConsole;

namespace OzemstvoWindowsForm
{
    public partial class Form1 : Form
    {
        public Ozemstvo ozemstvo = new();

        private class RuleProperties
        {
            public string BrowserName { get; set; }
            public string Type { get; set; }
            public string Data { get; set; }
        }

        public Form1()
        {
            InitializeComponent();
            ozemstvo.Init();

            //foreach (SettingsProperty settingsProperty in Properties.Rules.Default.Properties)
            //{
            //    var ruleProperties = Properties.Rules.Default[settingsProperty.ToString()].ToString();
            //    if (ruleProperties is null) continue;

            //    var ruleData = JsonSerializer.Deserialize<RuleProperties>(ruleProperties);
            //    if (ruleData is null) continue;

            //    var app = ozemstvo.Browsers.Find(x => x.Name.Contains(ruleData.BrowserName));
            //    if (app is not null)
            //    {
            //        Rule.RuleTypes type = Enum.Parse<Rule.RuleTypes>(ruleData.Type);
            //        ozemstvo.Rules.Add(new Rule(app, type, ruleData.Data));
            //    }
            //}

            // TODO: itirate over all rules
            var ruleData = JsonSerializer.Deserialize<RuleProperties>(Properties.Rules.Default.Bing);
            if (ruleData is not null)
            {
                var app = ozemstvo.Browsers.Find(x => x.Name.Contains(ruleData.BrowserName));
                if (app is not null)
                {
                    Rule.RuleTypes type = Enum.Parse<Rule.RuleTypes>(ruleData.Type);
                    ozemstvo.Rules.Add(new Rule(app, type, ruleData.Data));
                }
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //foreach (SettingsProperty settingsProperty in Properties.Rules.Default.Properties)
            //{
            //    textBox1.Text = settingsProperty.Name;
            //}
            textBox1.Text = Properties.Rules.Default.Properties.Count.ToString();

        }

        private void ClickMe_Click(object sender, EventArgs e)
        {
            var url = textBox1.Text;
            ozemstvo.Run(new Uri(url));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}