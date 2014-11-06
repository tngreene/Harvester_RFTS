namespace GSDIIITool
{
    partial class FormationPallete
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
            this.RegularEnemy = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // RegularEnemy
            // 
            this.RegularEnemy.Location = new System.Drawing.Point(12, 12);
            this.RegularEnemy.Name = "RegularEnemy";
            this.RegularEnemy.Size = new System.Drawing.Size(100, 100);
            this.RegularEnemy.TabIndex = 0;
            this.RegularEnemy.Text = "button1";
            this.RegularEnemy.UseVisualStyleBackColor = true;
            // 
            // FormationPallete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 562);
            this.Controls.Add(this.RegularEnemy);
            this.Name = "FormationPallete";
            this.Text = "FormationPallete";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button RegularEnemy;
    }
}