//using ImageProcess2;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace DIP_Coins_Activity
{
    public partial class Form1 : Form
    {
        Bitmap loaded;
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
        }

        private void openFileDialog1_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            pictureBox1.Image = loaded = new Bitmap(openFileDialog1.FileName);

            if (loaded == null) return;
            Filter.Binary(loaded, 200);
            Coins.CountCoin(loaded, ref label2);
        }
    }
}
