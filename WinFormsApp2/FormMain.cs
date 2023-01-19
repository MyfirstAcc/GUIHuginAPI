using HAPI;
using System.Windows.Forms;
using WinFormsApp2.Domains;

namespace WinFormsApp2
{
    public partial class FormMain : Form
    {
        string path = Directory.GetCurrentDirectory()+ @"/simpleBn.net";

        public FormMain()
        {
            InitializeComponent();
            initNet();
        }

        public SimpleBN SimpleBN
        {
            get => default;
            set
            {
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
                path = openFileDialog1.FileName;
            huginControl1.GetDomainFromFile(openFileDialog1.FileName, checkBox1.Checked);
        }

        private void Open()
        {
            if (path != null)
                huginControl1.GetDomainFromFile(path, checkBox1.Checked);
        }

        private void buttonReset_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void initNet()
        {
            Domain domain = new Domain();
            new SimpleBN(domain);
            huginControl1.GetDomain(domain, checkBox1.Checked);
        }

        private void îÏðîãðàììåToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About about = new About();
            about.ShowDialog();
        }



        private void button1_Click(object sender, EventArgs e)
        {


        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            Open();
        }
    }
}