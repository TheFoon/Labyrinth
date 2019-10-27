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

        //parameters of objects

        int buttonWidth = 200;
        int buttonHeight = 75;
        Color buttonColor = Color.DarkOrange;

        int pictureBoxWidth = 200;
        int picureBoxHeight = 100;

        int labelWidth = 300;
        int labelHeight = 75;
        Color labelColor = Color.Orange;

        private void Form1_Load(object sender, EventArgs e)
        {
            #region Objects

            TableLayoutPanel mainScreen = new TableLayoutPanel();
            TableLayoutPanel joinScreen = new TableLayoutPanel();
            Panel titleScreenInner = new Panel();
            Panel gameScreenInner = new Panel();
            Panel hostScreenInner = new Panel();


            Button startButton = new_Button("Start", new Point(0, 0));
            Button quitButton = new_Button("Quit", new Point(0, 0));

            Button hostButton = new_Button("Host game ", new Point(0, 0));
            Button joinButton = new_Button("Join to a game ", new Point(0, 80));
            Button backToTSButton = new_Button("Back", new Point(0, 160));

            Button host7x7Game = new_Button("7x7 game", new Point(0, 0));
            Button host9x9Game = new_Button("9x9 game", new Point(0, 80));
            Button backToGSButton = new_Button("Back", new Point(0, 160));


            PictureBox logoPictureBox = new_PictureBox("../../", "logo.png");

            this.SuspendLayout();
            mainScreen.SuspendLayout();
            joinScreen.SuspendLayout();
            titleScreenInner.SuspendLayout();
            gameScreenInner.SuspendLayout();
            logoPictureBox.SuspendLayout();
            hostScreenInner.SuspendLayout();

            //make panels fill their parents
            mainScreen.Dock = DockStyle.Fill;
            joinScreen.Dock = DockStyle.Fill;
            titleScreenInner.Dock = DockStyle.Fill;
            gameScreenInner.Dock = DockStyle.Fill;
            logoPictureBox.Dock = DockStyle.Fill;
            hostScreenInner.Dock = DockStyle.Fill;
            #endregion

            //add rows and columns to tableLayoutPanel
            mainScreen.ColumnCount = 3;
            mainScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300f));
            mainScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            mainScreen.RowCount = 3;
            mainScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));
            mainScreen.RowStyles.Add(new RowStyle(SizeType.Absolute, 200f));
            mainScreen.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            #region add objects to their screen
            //title screen
            titleScreenInner.Controls.Add(startButton);

            //before lobby screen

            gameScreenInner.Controls.Add(hostButton);
            gameScreenInner.Controls.Add(joinButton);

            //host screen

            hostScreenInner.Controls.Add(host7x7Game);
            hostScreenInner.Controls.Add(host9x9Game);
            #endregion

            //construct title screen
            this.ResumeLayout(false);
            mainScreen.ResumeLayout(false);
            titleScreenInner.ResumeLayout(false);
            mainScreen.Controls.Add(titleScreenInner, 1, 1);
            mainScreen.Controls.Add(quitButton, 1, 2);
            mainScreen.Controls.Add(logoPictureBox, 1, 0);
            this.Controls.Add(mainScreen);
            this.BackColor = Color.SandyBrown;


            //events

            startButton.Click += (s, ev) =>
            {
                gameScreenInner.ResumeLayout(false);
                mainScreen.Controls.Remove(titleScreenInner);
                mainScreen.Controls.Remove(quitButton);
                mainScreen.Controls.Add(gameScreenInner, 1, 1);
                mainScreen.Controls.Add(backToTSButton, 1, 2);
            };

            hostButton.Click += (s, ev) =>
            {
                hostScreenInner.ResumeLayout(false);
                mainScreen.Controls.Remove(gameScreenInner);
                mainScreen.Controls.Remove(backToTSButton);
                mainScreen.Controls.Add(hostScreenInner, 1, 1);
                mainScreen.Controls.Add(backToGSButton, 1, 2);
            };

            host7x7Game.Click += (s, ev) =>
            {
                BoardHandler.Board board = new BoardHandler.Board(7);

                board.FillBoardWithTile();

                
                TestForm testForm = new TestForm();
                testForm.Show();
                
            };

            host9x9Game.Click += (s, ev) =>
            {
                BoardHandler.Board board = new BoardHandler.Board(9);

                board.FillBoardWithTile();

                
                TestForm testForm = new TestForm();
                testForm.Show();
                
            };

            backToGSButton.Click += (s, ev) =>
            {

                mainScreen.Controls.Remove(hostScreenInner);
                mainScreen.Controls.Remove(backToGSButton);
                mainScreen.Controls.Add(gameScreenInner, 1, 1);
                mainScreen.Controls.Add(backToTSButton, 1, 2);
            };

            joinButton.Click += (s, ev) =>
            {
                //initalize row(s) for tableLayoutPanel

                if (GameHandler.playerIPs.Count != 0)
                {
                    mainScreen.Hide();

                    joinScreen.ColumnCount = 5;
                    joinScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25f));
                    joinScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));
                    joinScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 300f));
                    joinScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75f));
                    joinScreen.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 25f));

                    joinScreen.RowCount = GameHandler.playerIPs.Count + 3;
                    joinScreen.RowStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                    for (int i = 1; i < GameHandler.playerIPs.Count + 1; i++)
                    {
                        new_Label("label_Game" + i, GameHandler.playerIPs[i].Address.ToString(), new Point(0, i * 80));
                    }
                    joinScreen.RowStyles.Add(new ColumnStyle(SizeType.Absolute, 150f));
                    joinScreen.RowStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                }
                else
                {
                    MessageBox.Show("Jelenleg egy host sincsen");
                }

            };

            backToTSButton.Click += (s, ev) =>
            {
                mainScreen.Controls.Remove(gameScreenInner);
                mainScreen.Controls.Remove(backToTSButton);
                mainScreen.Controls.Add(titleScreenInner, 1, 1);
                mainScreen.Controls.Add(quitButton, 1, 2);
            };

            quitButton.Click += (s, ev) => {
                this.Close();
            };
        }

        private Button new_Button(string text, Point point)
        {
            Button button = new Button();
            button.Text = text;
            button.Size = new Size(buttonWidth, buttonHeight);
            button.BackColor = buttonColor;
            button.Location = point;
            button.Anchor = ((AnchorStyles)((AnchorStyles.Top) | (AnchorStyles.Left) | (AnchorStyles.Right)));
            button.TextAlign = ContentAlignment.MiddleLeft;
            button.Font = new Font("Bodoni MT", 25);
            button.ForeColor = Color.Brown;
            button.TabStop = false;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private PictureBox new_PictureBox(string location, string localName)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = new Bitmap(location + localName);
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox.Size = new Size(pictureBoxWidth, picureBoxHeight);
            return pictureBox;
        }

        private Label new_Label(string name, string text, Point point)
        {
            Label label = new Label();
            label.Name = name;
            label.Text = text;
            label.Size = new Size(labelWidth, labelHeight);
            label.BackColor = labelColor;
            label.Location = point;
            label.Anchor = ((AnchorStyles)((AnchorStyles.Top) | (AnchorStyles.Left) | (AnchorStyles.Right)));
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Font = new Font("Bodoni MT", 25);
            label.ForeColor = Color.Brown;
            return label;
        }
    }
}
