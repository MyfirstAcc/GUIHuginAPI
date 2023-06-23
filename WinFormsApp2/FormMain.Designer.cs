using WinFormsApp2.Controls;

namespace WinFormsApp2
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            aboutToolStripMenuItem = new ToolStripMenuItem();
            оПрограммеToolStripMenuItem = new ToolStripMenuItem();
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            checkBox1 = new CheckBox();
            buttonReset = new Button();
            buttonZoomIn = new Button();
            buttonZoomOut = new Button();
            huginControl1 = new HuginControl();
            openFileDialog1 = new OpenFileDialog();
            menuStrip1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, aboutToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(773, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(59, 24);
            fileToolStripMenuItem.Text = "Файл";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(159, 26);
            openToolStripMenuItem.Text = "Открыть...";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // aboutToolStripMenuItem
            // 
            aboutToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { оПрограммеToolStripMenuItem });
            aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            aboutToolStripMenuItem.Size = new Size(81, 24);
            aboutToolStripMenuItem.Text = "Справка";
            // 
            // оПрограммеToolStripMenuItem
            // 
            оПрограммеToolStripMenuItem.Name = "оПрограммеToolStripMenuItem";
            оПрограммеToolStripMenuItem.Size = new Size(187, 26);
            оПрограммеToolStripMenuItem.Text = "О программе";
            оПрограммеToolStripMenuItem.Click += оПрограммеToolStripMenuItem_Click;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(huginControl1, 0, 1);
            tableLayoutPanel1.Location = new Point(12, 31);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(749, 409);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 60F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(checkBox1, 1, 0);
            tableLayoutPanel2.Controls.Add(buttonReset, 0, 0);
            tableLayoutPanel2.Controls.Add(buttonZoomIn, 2, 0);
            tableLayoutPanel2.Controls.Add(buttonZoomOut, 3, 0);
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(743, 55);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // checkBox1
            // 
            checkBox1.Appearance = Appearance.Button;
            checkBox1.Image = GUIHugin.Properties.Resources.table;
            checkBox1.Location = new Point(63, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(54, 48);
            checkBox1.TabIndex = 0;
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.Click += checkBox1_Click;
            // 
            // buttonReset
            // 
            buttonReset.Image = GUIHugin.Properties.Resources.reload;
            buttonReset.Location = new Point(3, 3);
            buttonReset.Name = "buttonReset";
            buttonReset.Size = new Size(54, 48);
            buttonReset.TabIndex = 3;
            buttonReset.UseVisualStyleBackColor = true;
            buttonReset.Click += buttonReset_Click;
            // 
            // buttonZoomIn
            // 
            buttonZoomIn.Image = GUIHugin.Properties.Resources.zoom_in32;
            buttonZoomIn.Location = new Point(123, 3);
            buttonZoomIn.Name = "buttonZoomIn";
            buttonZoomIn.Size = new Size(54, 48);
            buttonZoomIn.TabIndex = 5;
            buttonZoomIn.UseVisualStyleBackColor = true;
            buttonZoomIn.Click += buttonZoomIn_Click;
            // 
            // buttonZoomOut
            // 
            buttonZoomOut.Image = GUIHugin.Properties.Resources.zoom_out1;
            buttonZoomOut.Location = new Point(183, 3);
            buttonZoomOut.Name = "buttonZoomOut";
            buttonZoomOut.Size = new Size(54, 48);
            buttonZoomOut.TabIndex = 6;
            buttonZoomOut.UseVisualStyleBackColor = true;
            buttonZoomOut.Click += buttonZoomOut_Click;
            // 
            // huginControl1
            // 
            huginControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            huginControl1.AutoScroll = true;
            huginControl1.BackColor = Color.White;
            huginControl1.BorderStyle = BorderStyle.Fixed3D;
            huginControl1.Location = new Point(3, 64);
            huginControl1.Name = "huginControl1";
            huginControl1.Size = new Size(743, 342);
            huginControl1.TabIndex = 2;
            // 
            // FormMain
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(773, 452);
            Controls.Add(tableLayoutPanel1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "FormMain";
            Text = "Form1";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem aboutToolStripMenuItem;
        private TableLayoutPanel tableLayoutPanel1;
        private Button buttonReset;
        private TableLayoutPanel tableLayoutPanel2;
        private OpenFileDialog openFileDialog1;
        private ToolStripMenuItem оПрограммеToolStripMenuItem;
        private HuginControl huginControl1;
        private Button buttonZoomIn;
        private Button buttonZoomOut;
        private CheckBox checkBox1;
    }
}