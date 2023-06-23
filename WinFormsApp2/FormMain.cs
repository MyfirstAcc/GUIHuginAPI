using HAPI;
using System.Diagnostics;
using System.Windows.Forms;
using WinFormsApp2.Domains;

namespace WinFormsApp2
{
    public partial class FormMain : Form
    {
        private About about = new About();
        string path = Directory.GetCurrentDirectory() + @"/simpleBn.net";
        private float[] scale = new float[] { 1f, 1.5f, 1.7f, 2f };
        private short index = 0;

        public FormMain()
        {
            InitializeComponent();
            initNet();
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

            huginControl1.GetDomainFromFile(path, false);
            //  Domain domain = new Domain();
            //new SimpleBN(domain);
            //huginControl1.GetDomain(domain, checkBox1.Checked);
        }

        private void îÏðîãðàììåToolStripMenuItem_Click(object sender, EventArgs e)
        {
            about.ShowDialog();
        }

        private void checkBox1_Click(object sender, EventArgs e)
        {
            Open();
        }

        private void buttonZoomIn_Click(object sender, EventArgs e)
        {          
            short t = index;
            huginControl1.SetZoom(scale[(t < scale.Length-1) ? ++index : scale.Length-1]);           
        }

        private void buttonZoomOut_Click(object sender, EventArgs e)
        {
            huginControl1.SetZoom(scale[(index > 0) ? --index : 0]);
        }
    }
}