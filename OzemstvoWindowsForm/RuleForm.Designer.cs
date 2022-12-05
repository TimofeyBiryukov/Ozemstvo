namespace OzemstvoWindowsForm
{
    partial class RuleForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.BrowserNameTextBox = new System.Windows.Forms.TextBox();
            this.BrowserNameLabel = new System.Windows.Forms.Label();
            this.RyleTypeLabel = new System.Windows.Forms.Label();
            this.TypeTextBox = new System.Windows.Forms.TextBox();
            this.DataLabel = new System.Windows.Forms.Label();
            this.DataTextBox = new System.Windows.Forms.TextBox();
            this.SubmitButton = new System.Windows.Forms.Button();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.RulesListBox = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // BrowserNameTextBox
            // 
            this.BrowserNameTextBox.Location = new System.Drawing.Point(12, 27);
            this.BrowserNameTextBox.Name = "BrowserNameTextBox";
            this.BrowserNameTextBox.PlaceholderText = "Google Chrome";
            this.BrowserNameTextBox.Size = new System.Drawing.Size(100, 23);
            this.BrowserNameTextBox.TabIndex = 0;
            // 
            // BrowserNameLabel
            // 
            this.BrowserNameLabel.AutoSize = true;
            this.BrowserNameLabel.Location = new System.Drawing.Point(12, 9);
            this.BrowserNameLabel.Name = "BrowserNameLabel";
            this.BrowserNameLabel.Size = new System.Drawing.Size(84, 15);
            this.BrowserNameLabel.TabIndex = 1;
            this.BrowserNameLabel.Text = "Browser Name";
            // 
            // RyleTypeLabel
            // 
            this.RyleTypeLabel.AutoSize = true;
            this.RyleTypeLabel.Location = new System.Drawing.Point(118, 9);
            this.RyleTypeLabel.Name = "RyleTypeLabel";
            this.RyleTypeLabel.Size = new System.Drawing.Size(57, 15);
            this.RyleTypeLabel.TabIndex = 3;
            this.RyleTypeLabel.Text = "Rule Type";
            // 
            // TypeTextBox
            // 
            this.TypeTextBox.Location = new System.Drawing.Point(118, 27);
            this.TypeTextBox.Name = "TypeTextBox";
            this.TypeTextBox.PlaceholderText = "Host";
            this.TypeTextBox.Size = new System.Drawing.Size(100, 23);
            this.TypeTextBox.TabIndex = 2;
            // 
            // DataLabel
            // 
            this.DataLabel.AutoSize = true;
            this.DataLabel.Location = new System.Drawing.Point(224, 9);
            this.DataLabel.Name = "DataLabel";
            this.DataLabel.Size = new System.Drawing.Size(31, 15);
            this.DataLabel.TabIndex = 5;
            this.DataLabel.Text = "Data";
            // 
            // DataTextBox
            // 
            this.DataTextBox.Location = new System.Drawing.Point(224, 27);
            this.DataTextBox.Name = "DataTextBox";
            this.DataTextBox.PlaceholderText = "google.com";
            this.DataTextBox.Size = new System.Drawing.Size(100, 23);
            this.DataTextBox.TabIndex = 4;
            // 
            // SubmitButton
            // 
            this.SubmitButton.Location = new System.Drawing.Point(330, 27);
            this.SubmitButton.Name = "SubmitButton";
            this.SubmitButton.Size = new System.Drawing.Size(75, 23);
            this.SubmitButton.TabIndex = 6;
            this.SubmitButton.Text = "Add";
            this.SubmitButton.UseVisualStyleBackColor = true;
            this.SubmitButton.Click += new System.EventHandler(this.SubmitButton_Click);
            // 
            // RemoveButton
            // 
            this.RemoveButton.Enabled = false;
            this.RemoveButton.Location = new System.Drawing.Point(325, 322);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(75, 23);
            this.RemoveButton.TabIndex = 8;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // RulesListBox
            // 
            this.RulesListBox.FormattingEnabled = true;
            this.RulesListBox.ItemHeight = 15;
            this.RulesListBox.Location = new System.Drawing.Point(12, 56);
            this.RulesListBox.Name = "RulesListBox";
            this.RulesListBox.Size = new System.Drawing.Size(388, 259);
            this.RulesListBox.TabIndex = 9;
            this.RulesListBox.SelectedIndexChanged += new System.EventHandler(this.RulesListBox_SelectedIndexChanged);
            // 
            // RuleForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 352);
            this.Controls.Add(this.RulesListBox);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.SubmitButton);
            this.Controls.Add(this.DataLabel);
            this.Controls.Add(this.DataTextBox);
            this.Controls.Add(this.RyleTypeLabel);
            this.Controls.Add(this.TypeTextBox);
            this.Controls.Add(this.BrowserNameLabel);
            this.Controls.Add(this.BrowserNameTextBox);
            this.Name = "RuleForm";
            this.Text = "RuleForm";
            this.Load += new System.EventHandler(this.RuleForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox BrowserNameTextBox;
        private Label BrowserNameLabel;
        private Label RyleTypeLabel;
        private TextBox TypeTextBox;
        private Label DataLabel;
        private TextBox DataTextBox;
        private Button SubmitButton;
        private Button RemoveButton;
        private ListBox RulesListBox;
    }
}