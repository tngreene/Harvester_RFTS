namespace GSDIIITool
{
    partial class HelpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HelpForm));
            this.helpTabs = new System.Windows.Forms.TabControl();
            this.formationCreator = new System.Windows.Forms.TabPage();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.mainTool = new System.Windows.Forms.TabPage();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.helpTabs.SuspendLayout();
            this.formationCreator.SuspendLayout();
            this.mainTool.SuspendLayout();
            this.SuspendLayout();
            // 
            // helpTabs
            // 
            this.helpTabs.Controls.Add(this.formationCreator);
            this.helpTabs.Controls.Add(this.mainTool);
            this.helpTabs.Location = new System.Drawing.Point(12, 12);
            this.helpTabs.Name = "helpTabs";
            this.helpTabs.SelectedIndex = 0;
            this.helpTabs.Size = new System.Drawing.Size(668, 627);
            this.helpTabs.TabIndex = 0;
            // 
            // formationCreator
            // 
            this.formationCreator.Controls.Add(this.textBox2);
            this.formationCreator.Controls.Add(this.textBox3);
            this.formationCreator.Location = new System.Drawing.Point(4, 25);
            this.formationCreator.Name = "formationCreator";
            this.formationCreator.Size = new System.Drawing.Size(660, 598);
            this.formationCreator.TabIndex = 6;
            this.formationCreator.Text = "Formation Creation";
            this.formationCreator.UseVisualStyleBackColor = true;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(0, 3);
            this.textBox3.Multiline = true;
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(661, 592);
            this.textBox3.TabIndex = 1;
            this.textBox3.Text = resources.GetString("textBox3.Text");
            // 
            // mainTool
            // 
            this.mainTool.Controls.Add(this.textBox1);
            this.mainTool.Location = new System.Drawing.Point(4, 25);
            this.mainTool.Name = "mainTool";
            this.mainTool.Size = new System.Drawing.Size(660, 598);
            this.mainTool.TabIndex = 0;
            this.mainTool.Text = "Part 2";
            this.mainTool.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(661, 592);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.SystemColors.Control;
            this.textBox2.Location = new System.Drawing.Point(182, 492);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(258, 79);
            this.textBox2.TabIndex = 3;
            this.textBox2.Text = "This tool was designed by Theodore Greene for Team Cascioli Ravioli for use with " +
    "the game Harvester (c)2012";
            this.textBox2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // HelpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 651);
            this.Controls.Add(this.helpTabs);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "HelpForm";
            this.Text = "HelpForm";
            this.helpTabs.ResumeLayout(false);
            this.formationCreator.ResumeLayout(false);
            this.formationCreator.PerformLayout();
            this.mainTool.ResumeLayout(false);
            this.mainTool.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl helpTabs;
        private System.Windows.Forms.TabPage mainTool;
        private System.Windows.Forms.TabPage formationCreator;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
    }
}