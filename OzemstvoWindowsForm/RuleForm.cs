using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OzemstvoWindowsForm
{
    public partial class RuleForm : Form
    {
        public RuleForm()
        {
            InitializeComponent();
        }

        private void RuleForm_Load(object sender, EventArgs e)
        {
            RulesListBox.Items.Add("Google Crhome | Host | google.com");
            RulesListBox.Items.Add("Firefox | Host | youtube.com");
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            RulesListBox.Items.Add($"{BrowserNameTextBox.Text} | {TypeTextBox.Text} | {DataTextBox.Text}");
            BrowserNameTextBox.Text = "";
            TypeTextBox.Text = "";
            DataTextBox.Text = "";
        }

        private void RulesListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            RemoveButton.Enabled = RulesListBox.SelectedIndex != -1;
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            RulesListBox.Items.RemoveAt(RulesListBox.SelectedIndex);
        }
    }
}
