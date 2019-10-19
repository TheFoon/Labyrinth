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
            //panels
            TableLayoutPanel titleScreen = new TableLayoutPanel();
            Panel titleScreenInner = new Panel();
            TableLayoutPanel startGameScreen = new TableLayoutPanel();
            Panel startGameScreenInner = new Panel();


            //labels
            Label titleLabel = new Label();


            //buttons
            Button StartButton = new Button();
            Button QuitButton = new Button();
            Button hostGameButton = new Button();
            Button joinGameButton = new Button();


            this.SuspendLayout();
            titleScreen.SuspendLayout();
            titleScreenInner.SuspendLayout();

            //make panels fill their parents
            titleScreen.Dock = DockStyle.Fill;
            titleScreenInner.Dock = DockStyle.Fill;

            //add rows and columns to tableLayoutPanel
            titleScreen.ColumnCount = 3;
            titleScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            titleScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200f));
            titleScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            titleScreen.RowCount = 3;
            titleScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            titleScreen.RowStyles.Add(new RowStyle(SizeType.Absolute, 215f));
            titleScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            //Title
            titleLabel.Text = "Labyrinth";
            titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            titleLabel.Size = new Size(200, 100);
            titleLabel.BackColor = Color.Aqua;
            titleLabel.Location = new Point(0, 0);
            titleLabel.Anchor = ((AnchorStyles)((AnchorStyles.Top) | (AnchorStyles.Left) | (AnchorStyles.Right)));
            titleScreen.Controls.Add(titleLabel, 1, 0);

            //Start Button
            StartButton.Text = "Start Game";
            StartButton.BackColor = Color.Crimson;
            StartButton.Size = new Size(200, 100);
            StartButton.Click += (s, ev) => {
                //go to start page
                startGameScreen.Dock = DockStyle.Fill;
                startGameScreenInner.Dock = DockStyle.Fill;

                //add rows and columns to tableLayoutPanel
                startGameScreen.ColumnCount = 3;
                startGameScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                startGameScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200f));
                startGameScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                startGameScreen.RowCount = 3;
                startGameScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
                startGameScreen.RowStyles.Add(new RowStyle(SizeType.Absolute, 215f));
                startGameScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));


                hostGameButton.Text = "Start Game";
                hostGameButton.BackColor = Color.Crimson;
                hostGameButton.Size = new Size(200, 100);

                joinGameButton.Text = "Start Game";
                joinGameButton.BackColor = Color.Crimson;
                joinGameButton.Size = new Size(200, 100);


                //construct starting screen
                startGameScreen.Controls.Add(startGameScreenInner, 1, 1);
                this.Controls.Add(startGameScreen);
                startGameScreen.BringToFront();
            };
            StartButton.Location = new Point(0, 5);
            StartButton.Anchor = ((AnchorStyles)((AnchorStyles.Top) | (AnchorStyles.Left) | (AnchorStyles.Right)));
            //disable blue outine
            StartButton.TabStop = false;
            StartButton.FlatStyle = FlatStyle.Flat;
            StartButton.FlatAppearance.BorderSize = 0;

            //Quit Button
            QuitButton.Text = "Quit Game";
            QuitButton.BackColor = Color.Crimson;
            QuitButton.Size = new Size(200, 100);
            QuitButton.Click += (s, ev) => { this.Close(); };
            QuitButton.Location = new Point(0, 110);
            QuitButton.Anchor = ((AnchorStyles)((AnchorStyles.Top) | (AnchorStyles.Left) | (AnchorStyles.Right)));
            //disable blue outine
            QuitButton.TabStop = false;
            QuitButton.FlatStyle = FlatStyle.Flat;
            QuitButton.FlatAppearance.BorderSize = 0;

            //add buttons to title screen
            titleScreenInner.Controls.Add(StartButton);
            titleScreenInner.Controls.Add(QuitButton);

            //construct title screen
            titleScreen.Controls.Add(titleScreenInner, 1, 1);
            this.Controls.Add(titleScreen);
            this.ResumeLayout(false);
            titleScreen.ResumeLayout(false);
            titleScreenInner.ResumeLayout(false);

            titleScreen.BringToFront();
        }
    }
}
