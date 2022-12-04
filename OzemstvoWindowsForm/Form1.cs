using System.Configuration;

namespace OzemstvoWindowsForm
{
    public partial class Form1 : Form
    {
        public OzemstvoConsole.Ozemstvo ozemstvo = new();

        public Form1()
        {
            InitializeComponent();
            ozemstvo.Init();
            // var foo = Properties.Settings.Default.Foo;
            // Console.WriteLine(foo);

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