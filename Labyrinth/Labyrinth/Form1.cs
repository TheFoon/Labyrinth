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
            Panel titleScreen = new Panel();
            titleScreen.Dock = DockStyle.Fill;
            Label TitleLabel = new Label();
            TitleLabel.Text = "Labyrinth";
            titleScreen.Controls.Add(TitleLabel);
            Button StartButton = new Button();
            StartButton.Text = "Start Game";
            StartButton.Click += (s, ev) => {};
            titleScreen.Controls.Add(StartButton);
            this.Controls.Add(titleScreen);
        }
    }
}
