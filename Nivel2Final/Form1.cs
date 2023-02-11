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
            cbCampo.Items.Add("Nombre");
            cbCampo.Items.Add("Categoria");
            cbCampo.Items.Add("Marca");
            cbCampo.Items.Add("Precio");

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
            try
            {
                Articulo seleccion;
                if (dgvArticulos.CurrentRow != null)
                {
                    DialogResult respuesta = MessageBox.Show("El registro se modificará permanentemente  \t                                               ¿ Estás seguro ?", "Modificando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {
                        seleccion = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                        fmrAltaArticulo modificar = new fmrAltaArticulo(seleccion);
                        modificar.ShowDialog();
                        Cargar();
                    }
                }
                else
                {
                    MessageBox.Show("seleccione un articulo para modificar", "alerta", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ArticuloNegocio negocio = new ArticuloNegocio();
            Articulo art;
            try
            {
                if (dgvArticulos.CurrentRow != null)
                {
                    DialogResult respuesta = MessageBox.Show("El registro se borrará permanentemente de la base de datos \t                                 ¿ Estás seguro ?", "eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (respuesta == DialogResult.Yes)
                    {

                        art = (Articulo)dgvArticulos.CurrentRow.DataBoundItem;
                        negocio.Eliminar(art.IdArticulo);
                        Cargar();

                    }
                }
                else
                {
                    MessageBox.Show("seleccione un articulo para eliminar", "alerta", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<Articulo> listaFiltros;
            string filtro = txtFiltro.Text;

            if (filtro != " ")
            { listaFiltros = listaArticulos.FindAll(x => x.Nombre.ToUpper().Contains(filtro.ToUpper()) || x.categoria.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.marca.Descripcion.ToUpper().Contains(filtro.ToUpper()) || x.Codigo.ToUpper().Contains(filtro.ToUpper())); }
            else { listaFiltros = listaArticulos; }

            dgvArticulos.DataSource = null;
            dgvArticulos.DataSource = listaFiltros;
            dgvArticulos.Columns["ImagenUrl"].Visible = false;
            dgvArticulos.Columns["Descripcion"].Visible = false;
            dgvArticulos.Columns["IdArticulo"].Visible = false;
        }

        private void cbCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string opcionSeleccionada = cbCampo.SelectedItem.ToString();
            if (opcionSeleccionada == "Precio")
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Precio menor a: ");
                cbCriterio.Items.Add("Precio mayor a: ");
            }
            else
            {
                cbCriterio.Items.Clear();
                cbCriterio.Items.Add("Contiene las letras: ");
            }
        }

        private bool ValidarFiltro()
        {
            if (cbCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Ingrese el campo para filtrar");
                return true;
            }
            if (cbCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Ingrese el criterio para filtrar", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return true;
            }
            if (cbCampo.SelectedItem.ToString() == "Precio")
            {
                if (!(SoloNumeros(txtFiltroAvanzado.Text)))
                {
                    MessageBox.Show("Ingrese solo números", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    return true;
                }
            }

            return false;
        }
        private bool SoloNumeros(string numeros)
        {
            foreach (char caracter in numeros)
            {
                if (!(char.IsNumber(caracter)))
                {
                    return false;
                }
            }
            return true;
        }
        private void txtFiltroAvanzado_TextChanged(object sender, EventArgs e)
        {
            ArticuloNegocio articulo = new ArticuloNegocio();
            try
            {
                if (ValidarFiltro())
                {
                    return;
                }

                string campo = cbCampo.SelectedItem.ToString();
                string criterio = cbCriterio.SelectedItem.ToString();
                string filtroAvanzado = txtFiltroAvanzado.Text;

                if (!(string.IsNullOrEmpty(txtFiltroAvanzado.Text)))
                { dgvArticulos.DataSource = articulo.filtrar(campo, criterio, filtroAvanzado); }
                
                if(filtroAvanzado=="")
                {
                    dgvArticulos.DataSource = articulo.Listar();
                }
            }

            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }
    }
}
