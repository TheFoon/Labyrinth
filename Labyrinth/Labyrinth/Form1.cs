using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Labyrinth
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            TableLayoutPanel titleScreen = new TableLayoutPanel();
            Panel titleScreenInner = new Panel();
            this.SuspendLayout();
            titleScreen.SuspendLayout();
            titleScreenInner.SuspendLayout();
            titleScreen.Dock = DockStyle.Fill;
            titleScreenInner.Dock = DockStyle.Fill;
            titleScreen.ColumnCount = 3;
            titleScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            titleScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 210f));
            titleScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            titleScreen.RowCount = 3;
            titleScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            titleScreen.RowStyles.Add(new RowStyle(SizeType.Absolute, 210f));
            titleScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            Label titleLabel = new Label();
            titleLabel.Text = "Labyrinth";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Size = new Size(200, 100);
            titleLabel.BackColor = Color.Aqua;
            titleLabel.Location = new Point(0, 0);
            titleLabel.Anchor = ((AnchorStyles)((AnchorStyles.Top) | (AnchorStyles.Left) | (AnchorStyles.Right)));
            titleScreenInner.Controls.Add(titleLabel);
            Button StartButton = new Button();
            StartButton.Text = "Start Game";
            StartButton.BackColor = Color.Crimson;
            StartButton.Size = new Size(200, 100);
            StartButton.Click += (s, ev) => {};
            StartButton.Location = new Point(0, 100);
            StartButton.Anchor = ((AnchorStyles)((AnchorStyles.Top) | (AnchorStyles.Left) | (AnchorStyles.Right)));
            titleScreenInner.Controls.Add(StartButton);
            titleScreen.Controls.Add(titleScreenInner, 1, 1);
            this.Controls.Add(titleScreen);
            this.ResumeLayout(false);
            titleScreen.ResumeLayout(false);
            titleScreenInner.ResumeLayout(false);
        }
    }
}
