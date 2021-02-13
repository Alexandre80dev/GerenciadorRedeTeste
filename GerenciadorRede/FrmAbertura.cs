using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GerenciadorRede
{
    public partial class FrmAbertura : Form
    {
        public FrmAbertura()
        {
            InitializeComponent();
        }

        private void FrmAbertura_Shown(object sender, EventArgs e)
        {
            Application.DoEvents();
            Thread.Sleep(2000);
            FrmPrincipal frm = new FrmPrincipal();
            this.Visible = false;
            frm.Show();            
        }
    }
}
