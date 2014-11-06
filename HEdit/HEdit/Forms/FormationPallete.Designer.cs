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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormationPallete));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.newToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.printToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.helpToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.FileName = new System.Windows.Forms.TextBox();
            this.FileNameLabel = new System.Windows.Forms.Label();
            this.difficultySelectList = new System.Windows.Forms.ComboBox();
            this.difficultySelectLabel = new System.Windows.Forms.Label();
            this.mouse_coords = new System.Windows.Forms.Label();
            this.kamikaze = new System.Windows.Forms.Button();
            this.bomber = new System.Windows.Forms.Button();
            this.PhxMkII = new System.Windows.Forms.Button();
            this.enemy_fighter = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripButton,
            this.openToolStripButton,
            this.saveToolStripButton,
            this.printToolStripButton,
            this.toolStripSeparator,
            this.helpToolStripButton,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(128, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // newToolStripButton
            // 
            this.newToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripButton.Image")));
            this.newToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newToolStripButton.Name = "newToolStripButton";
            this.newToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.newToolStripButton.Text = "&New";
            this.newToolStripButton.Click += new System.EventHandler(this.newToolStripButton_Click);
            // 
            // openToolStripButton
            // 
            this.openToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.openToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripButton.Image")));
            this.openToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.openToolStripButton.Name = "openToolStripButton";
            this.openToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.openToolStripButton.Text = "&Open";
            this.openToolStripButton.Click += new System.EventHandler(this.openToolStripButton_Click);
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.saveToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripButton.Image")));
            this.saveToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.saveToolStripButton.Text = "&Save";
            this.saveToolStripButton.Click += new System.EventHandler(this.saveToolStripButton_Click);
            // 
            // printToolStripButton
            // 
            this.printToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.printToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("printToolStripButton.Image")));
            this.printToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.printToolStripButton.Name = "printToolStripButton";
            this.printToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.printToolStripButton.Text = "&Print";
            this.printToolStripButton.Click += new System.EventHandler(this.printToolStripButton_Click);
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // helpToolStripButton
            // 
            this.helpToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.helpToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("helpToolStripButton.Image")));
            this.helpToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.helpToolStripButton.Name = "helpToolStripButton";
            this.helpToolStripButton.Size = new System.Drawing.Size(23, 22);
            this.helpToolStripButton.Text = "He&lp";
            this.helpToolStripButton.Click += new System.EventHandler(this.helpToolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // FileName
            // 
            this.FileName.Location = new System.Drawing.Point(4, 292);
            this.FileName.Name = "FileName";
            this.FileName.Size = new System.Drawing.Size(100, 22);
            this.FileName.TabIndex = 53;
            // 
            // FileNameLabel
            // 
            this.FileNameLabel.AutoSize = true;
            this.FileNameLabel.Location = new System.Drawing.Point(4, 272);
            this.FileNameLabel.Name = "FileNameLabel";
            this.FileNameLabel.Size = new System.Drawing.Size(71, 17);
            this.FileNameLabel.TabIndex = 52;
            this.FileNameLabel.Text = "File Name";
            // 
            // difficultySelectList
            // 
            this.difficultySelectList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.difficultySelectList.FormattingEnabled = true;
            this.difficultySelectList.Items.AddRange(new object[] {
            "Easy",
            "Medium",
            "Hard"});
            this.difficultySelectList.Location = new System.Drawing.Point(4, 245);
            this.difficultySelectList.Name = "difficultySelectList";
            this.difficultySelectList.Size = new System.Drawing.Size(121, 24);
            this.difficultySelectList.TabIndex = 54;
            // 
            // difficultySelectLabel
            // 
            this.difficultySelectLabel.AutoSize = true;
            this.difficultySelectLabel.Location = new System.Drawing.Point(4, 225);
            this.difficultySelectLabel.Name = "difficultySelectLabel";
            this.difficultySelectLabel.Size = new System.Drawing.Size(61, 17);
            this.difficultySelectLabel.TabIndex = 57;
            this.difficultySelectLabel.Text = "Difficulty";
            // 
            // mouse_coords
            // 
            this.mouse_coords.AutoSize = true;
            this.mouse_coords.Location = new System.Drawing.Point(1, 208);
            this.mouse_coords.Name = "mouse_coords";
            this.mouse_coords.Size = new System.Drawing.Size(101, 17);
            this.mouse_coords.TabIndex = 58;
            this.mouse_coords.Text = "mouse_coords";
            // 
            // kamikaze
            // 
            this.kamikaze.BackColor = System.Drawing.SystemColors.Window;
            this.kamikaze.Image = global::GSDIIITool.Properties.Resources.kamikaze;
            this.kamikaze.Location = new System.Drawing.Point(7, 87);
            this.kamikaze.Margin = new System.Windows.Forms.Padding(4);
            this.kamikaze.Name = "kamikaze";
            this.kamikaze.Size = new System.Drawing.Size(50, 50);
            this.kamikaze.TabIndex = 6;
            this.kamikaze.UseVisualStyleBackColor = false;
            this.kamikaze.Click += new System.EventHandler(this.Kamikaze_Click);
            // 
            // bomber
            // 
            this.bomber.BackColor = System.Drawing.SystemColors.Window;
            this.bomber.Image = global::GSDIIITool.Properties.Resources.bomber;
            this.bomber.Location = new System.Drawing.Point(65, 29);
            this.bomber.Margin = new System.Windows.Forms.Padding(4);
            this.bomber.Name = "bomber";
            this.bomber.Size = new System.Drawing.Size(50, 50);
            this.bomber.TabIndex = 56;
            this.bomber.UseVisualStyleBackColor = false;
            this.bomber.Click += new System.EventHandler(this.BomberBeetle_Click);
            // 
            // PhxMkII
            // 
            this.PhxMkII.BackColor = System.Drawing.SystemColors.Window;
            this.PhxMkII.Image = global::GSDIIITool.Properties.Resources.phoenix;
            this.PhxMkII.Location = new System.Drawing.Point(65, 87);
            this.PhxMkII.Margin = new System.Windows.Forms.Padding(4);
            this.PhxMkII.Name = "PhxMkII";
            this.PhxMkII.Size = new System.Drawing.Size(50, 50);
            this.PhxMkII.TabIndex = 57;
            this.PhxMkII.UseVisualStyleBackColor = false;
            this.PhxMkII.Click += new System.EventHandler(this.PhxMkII_Click);
            // 
            // enemy_fighter
            // 
            this.enemy_fighter.BackColor = System.Drawing.SystemColors.Window;
            this.enemy_fighter.Image = global::GSDIIITool.Properties.Resources.enemy_fighter;
            this.enemy_fighter.Location = new System.Drawing.Point(7, 29);
            this.enemy_fighter.Margin = new System.Windows.Forms.Padding(4);
            this.enemy_fighter.Name = "enemy_fighter";
            this.enemy_fighter.Size = new System.Drawing.Size(50, 50);
            this.enemy_fighter.TabIndex = 2;
            this.enemy_fighter.UseVisualStyleBackColor = false;
            this.enemy_fighter.Click += new System.EventHandler(this.regularEnemy_Click);
            // 
            // FormationPallete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(128, 320);
            this.Controls.Add(this.PhxMkII);
            this.Controls.Add(this.bomber);
            this.Controls.Add(this.mouse_coords);
            this.Controls.Add(this.kamikaze);
            this.Controls.Add(this.difficultySelectLabel);
            this.Controls.Add(this.enemy_fighter);
            this.Controls.Add(this.difficultySelectList);
            this.Controls.Add(this.FileName);
            this.Controls.Add(this.FileNameLabel);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormationPallete";
            this.Text = "HEdit";
            this.TopMost = true;
            this.LocationChanged += new System.EventHandler(this.FormationPallete_LocationChanged);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.FormationPallete_MouseMove);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton newToolStripButton;
        private System.Windows.Forms.ToolStripButton openToolStripButton;
        private System.Windows.Forms.ToolStripButton saveToolStripButton;
        private System.Windows.Forms.ToolStripButton printToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton helpToolStripButton;
        private System.Windows.Forms.TextBox FileName;
        private System.Windows.Forms.Label FileNameLabel;
        private System.Windows.Forms.ComboBox difficultySelectList;
        private System.Windows.Forms.Label difficultySelectLabel;
        private System.Windows.Forms.Label mouse_coords;
        private System.Windows.Forms.Button enemy_fighter;
        private System.Windows.Forms.Button kamikaze;
        private System.Windows.Forms.Button bomber;
        private System.Windows.Forms.Button PhxMkII;

    }
}