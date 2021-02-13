using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GerenciadorRede
{
    public partial class FrmPrincipal : Form
    {
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        public class DetalheRede
        {
            public virtual string Descricao { get; set; }
            public virtual string MAC { get; set; }
            public virtual string IPV4 { get; set; }
            public virtual string IPV6 { get; set; }
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            //Enviar comando para buscar os dados
            string[] sResultadoComando = ExecutarComando("ipconfig /ALL").Split('\n');

            //Lista para guardar e organizar as informacoes para a grid
            IList<DetalheRede> lstLista = new List<DetalheRede>();
            
            LerInformacoesRede(sResultadoComando, lstLista);
            VerificarCamposEmBranco(lstLista);
            PopularGrid(lstLista);
        }

        private static void LerInformacoesRede(string[] sResultadoComando, IList<DetalheRede> lstLista)
        {
            DetalheRede registro = new DetalheRede();
            foreach (var line in sResultadoComando)
            {
                if (line.ToUpper().Contains("ADAPTADOR"))
                {
                    if (string.IsNullOrEmpty(registro.Descricao))
                    {
                        registro.Descricao = line;
                    }
                    else
                    {
                        lstLista.Add(registro);
                        registro = new DetalheRede();
                    }
                }
                else
                {
                    if (line.Contains("Endereço Físico"))
                    {
                        registro.MAC = line.Split(':')[1];
                    }

                    if (line.ToUpper().Contains("IPV4"))
                    {
                        if (string.IsNullOrEmpty(registro.IPV4))
                        {
                            registro.IPV4 = line.Split(':')[1];
                        }
                        else
                        {
                            registro.IPV4 = "Não encontrado";
                        }


                    }
                    if (line.ToUpper().Contains("IPV6"))
                    {
                        if (string.IsNullOrEmpty(registro.IPV6))
                        {
                            int iContador = 0;
                            foreach (var item in line.Split(':'))
                            {
                                if (iContador > 0)
                                {
                                    registro.IPV6 += item;
                                }
                                iContador++;
                            }

                        }
                    }
                }
            }
        }

        private void PopularGrid(IList<DetalheRede> lstLista)
        {
            dataGridView1.DataSource = lstLista;
            dataGridView1.AutoResizeColumns();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private static void VerificarCamposEmBranco(IList<DetalheRede> lstLista)
        {
            foreach (var item in lstLista)
            {
                if (string.IsNullOrEmpty(item.IPV4))
                {
                    item.IPV4 = "Não encontrado";
                }
                if (string.IsNullOrEmpty(item.IPV6))
                {
                    item.IPV6 = "Não encontrado";
                }
                if (string.IsNullOrEmpty(item.MAC))
                {
                    item.MAC = "Não encontrado";
                }
            }
        }

        public string ExecutarComando(string _comando)
        {
            using (Process processo = new Process())
            {
                processo.StartInfo.FileName = Environment.GetEnvironmentVariable("comspec");
                                
                processo.StartInfo.Arguments = string.Format("/c {0}", _comando);
                processo.StartInfo.RedirectStandardOutput = true;
                processo.StartInfo.UseShellExecute = false;
                processo.StartInfo.CreateNoWindow = true;
                processo.StartInfo.StandardOutputEncoding = Encoding.GetEncoding(850);
                processo.Start();

                return processo.StandardOutput.ReadToEnd();
            }
        }

        private void FrmPrincipal_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns[2].Index)
            {
                if (e.Value.ToString() == "Não encontrado")
                {
                    e.CellStyle.BackColor = Color.LightSalmon;
                }
            }
            if (e.ColumnIndex == dataGridView1.Columns[3].Index)
            {
                if (e.Value.ToString() == "Não encontrado")
                {
                    e.CellStyle.BackColor = Color.LightSalmon;
                }
            }

        }
    }
}
