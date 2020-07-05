namespace rels
{
    partial class Form1
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.listView1 = new System.Windows.Forms.ListView();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.infoPage = new System.Windows.Forms.TabPage();
            this.altNamesBox = new System.Windows.Forms.ComboBox();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.nameFlag = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.nameLabel = new System.Windows.Forms.Label();
            this.ancestorsPage = new System.Windows.Forms.TabPage();
            this.ancestorsView = new System.Windows.Forms.TreeView();
            this.descendantsPage = new System.Windows.Forms.TabPage();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.infoPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.ancestorsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Window;
            this.panel1.Controls.Add(this.listView1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(678, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(305, 588);
            this.panel1.TabIndex = 2;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 33);
            this.listView1.MultiSelect = false;
            this.listView1.Name = "listView1";
            this.listView1.ShowItemToolTips = true;
            this.listView1.Size = new System.Drawing.Size(305, 555);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listView1.TabIndex = 3;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(155, 3);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(59, 26);
            this.button3.TabIndex = 2;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(77, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 26);
            this.button2.TabIndex = 1;
            this.button2.Text = "Countries";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(3, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(70, 26);
            this.button1.TabIndex = 0;
            this.button1.Text = "Centuries";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.SystemColors.Window;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(672, 588);
            this.splitContainer1.SplitterDistance = 294;
            this.splitContainer1.TabIndex = 4;
            // 
            // tabControl1
            // 
            this.tabControl1.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabControl1.Controls.Add(this.infoPage);
            this.tabControl1.Controls.Add(this.ancestorsPage);
            this.tabControl1.Controls.Add(this.descendantsPage);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowToolTips = true;
            this.tabControl1.Size = new System.Drawing.Size(672, 294);
            this.tabControl1.TabIndex = 0;
            // 
            // infoPage
            // 
            this.infoPage.Controls.Add(this.altNamesBox);
            this.infoPage.Controls.Add(this.richTextBox1);
            this.infoPage.Controls.Add(this.nameFlag);
            this.infoPage.Controls.Add(this.pictureBox1);
            this.infoPage.Controls.Add(this.nameLabel);
            this.infoPage.Location = new System.Drawing.Point(4, 4);
            this.infoPage.Name = "infoPage";
            this.infoPage.Padding = new System.Windows.Forms.Padding(3);
            this.infoPage.Size = new System.Drawing.Size(664, 264);
            this.infoPage.TabIndex = 0;
            this.infoPage.Text = "info";
            this.infoPage.UseVisualStyleBackColor = true;
            // 
            // altNamesBox
            // 
            this.altNamesBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.altNamesBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.altNamesBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.altNamesBox.FormattingEnabled = true;
            this.altNamesBox.Location = new System.Drawing.Point(198, 29);
            this.altNamesBox.Name = "altNamesBox";
            this.altNamesBox.Size = new System.Drawing.Size(460, 25);
            this.altNamesBox.Sorted = true;
            this.altNamesBox.TabIndex = 6;
            this.altNamesBox.SelectedIndexChanged += new System.EventHandler(this.altNamesBox_SelectedIndexChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox1.Location = new System.Drawing.Point(198, 60);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.richTextBox1.Size = new System.Drawing.Size(459, 81);
            this.richTextBox1.TabIndex = 5;
            this.richTextBox1.Text = "";
            // 
            // nameFlag
            // 
            this.nameFlag.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.nameFlag.Location = new System.Drawing.Point(253, 6);
            this.nameFlag.Margin = new System.Windows.Forms.Padding(0);
            this.nameFlag.Name = "nameFlag";
            this.nameFlag.Size = new System.Drawing.Size(24, 17);
            this.nameFlag.TabIndex = 3;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(6, 6);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(186, 252);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // nameLabel
            // 
            this.nameLabel.AutoEllipsis = true;
            this.nameLabel.AutoSize = true;
            this.nameLabel.Font = new System.Drawing.Font("Microsoft YaHei UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameLabel.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.nameLabel.Location = new System.Drawing.Point(198, 3);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(49, 20);
            this.nameLabel.TabIndex = 0;
            this.nameLabel.Text = "name";
            this.nameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ancestorsPage
            // 
            this.ancestorsPage.Controls.Add(this.ancestorsView);
            this.ancestorsPage.Location = new System.Drawing.Point(4, 4);
            this.ancestorsPage.Name = "ancestorsPage";
            this.ancestorsPage.Padding = new System.Windows.Forms.Padding(3);
            this.ancestorsPage.Size = new System.Drawing.Size(664, 268);
            this.ancestorsPage.TabIndex = 1;
            this.ancestorsPage.Text = "ancestors";
            this.ancestorsPage.ToolTipText = "ancestors";
            this.ancestorsPage.UseVisualStyleBackColor = true;
            // 
            // ancestorsView
            // 
            this.ancestorsView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ancestorsView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ancestorsView.FullRowSelect = true;
            this.ancestorsView.Location = new System.Drawing.Point(3, 3);
            this.ancestorsView.Name = "ancestorsView";
            this.ancestorsView.Size = new System.Drawing.Size(658, 262);
            this.ancestorsView.TabIndex = 0;
            this.ancestorsView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.ancestorsView_AfterSelect);
            // 
            // descendantsPage
            // 
            this.descendantsPage.Location = new System.Drawing.Point(4, 4);
            this.descendantsPage.Name = "descendantsPage";
            this.descendantsPage.Padding = new System.Windows.Forms.Padding(3);
            this.descendantsPage.Size = new System.Drawing.Size(664, 268);
            this.descendantsPage.TabIndex = 2;
            this.descendantsPage.Text = "descendants";
            this.descendantsPage.ToolTipText = "descendants";
            this.descendantsPage.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 588);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Opacity = 0.9D;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.infoPage.ResumeLayout(false);
            this.infoPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ancestorsPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage infoPage;
        private System.Windows.Forms.TabPage ancestorsPage;
        private System.Windows.Forms.Label nameLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label nameFlag;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.TabPage descendantsPage;
        private System.Windows.Forms.TreeView ancestorsView;
        private System.Windows.Forms.ComboBox altNamesBox;
    }
}

