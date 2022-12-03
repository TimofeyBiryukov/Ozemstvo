namespace OzemstvoWindowsForm
{
    public partial class Form1 : Form
    {
        public OzemstvoConsole.Ozemstvo ozemstvo = new();

        public Form1()
        {
            InitializeComponent();
            ozemstvo.Init();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ClickMe_Click(object sender, EventArgs e)
        {
            // get value of textBox1
            var url = textBox1.Text;
            ozemstvo.Run(new Uri(url));
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}