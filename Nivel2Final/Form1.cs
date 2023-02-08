using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;

namespace Nivel2Final
{
    public partial class FrmPrincipal : Form
    {
        private List<Articulo> listaArticulos;
        public FrmPrincipal()
        {
            InitializeComponent();
        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            Cargar();
        }

        private void Cargar()
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            try
            {
                listaArticulos = negocio.Listar();
                dgvArticulos.DataSource = listaArticulos;
                dgvArticulos.Columns["ImagenUrl"].Visible = false;
                dgvArticulos.Columns["Descripcion"].Visible = false;
                dgvArticulos.Columns["IdArticulo"].Visible = false;
                CargarImagen(listaArticulos[0].ImagenUrl);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
private void dgvArticulos_SelectionChanged(object sender, EventArgs e)
        {
            Articulo seleccionado = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            CargarImagen(seleccionado.ImagenUrl);
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxImagenUrl.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxImagenUrl.Load("https://thealmanian.com/wp-content/uploads/2019/01/product_image_thumbnail_placeholder.png");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fmrAltaArticulo alta = new fmrAltaArticulo();
            alta.ShowDialog();
            Cargar();

        }

        private void Cancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            Articulo seleccion;
            seleccion = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
            fmrAltaArticulo modificar = new fmrAltaArticulo(seleccion);
            modificar.ShowDialog();
            Cargar();
        }
    }
}
