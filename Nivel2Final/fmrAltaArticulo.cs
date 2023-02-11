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
    public partial class fmrAltaArticulo : Form
    {
        private Articulo articulo = null;
        public fmrAltaArticulo()
        {
            InitializeComponent();
        }
        public fmrAltaArticulo(Articulo art)
        {
            InitializeComponent();
            this.articulo = art;
        }


        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

            ArticuloNegocio negocio = new ArticuloNegocio();

            try
            {

                if (articulo == null)
                { articulo = new Articulo(); }

                articulo.Codigo = txtCodigo.Text;
                articulo.Nombre = txtNombre.Text;
                articulo.Descripcion = txtDescripcion.Text;
                articulo.ImagenUrl = txtImagenUrl.Text;
                articulo.Precio = decimal.Parse(txtPrecio.Text);
                articulo.marca = (Marca)cbMarcas.SelectedItem;
                articulo.categoria = (Categoria)cbCategorias.SelectedItem;

                if (articulo.IdArticulo != 0)
                {
                    negocio.Modificar(articulo);
                    MessageBox.Show("Articulo Modificado");
                }
                else
                {


                    negocio.Agregar(articulo);
                    MessageBox.Show("Articulo Agregado");

                }

                Close();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        private void fmrAltaArticulo_Load(object sender, EventArgs e)
        {
            CategoriaNegocio cat = new CategoriaNegocio();
            MarcaNegocio marca = new MarcaNegocio();
            try
            {
                cbCategorias.DataSource = cat.listar();
                cbCategorias.ValueMember = "IdCategoria";
                cbCategorias.DisplayMember = "Descripcion";

                cbMarcas.DataSource = marca.listar();
                cbMarcas.ValueMember = "IdMarca";
                cbMarcas.DisplayMember = "Descripcion";

                if (articulo != null)
                {
                    txtCodigo.Text = articulo.Codigo;
                    txtDescripcion.Text = articulo.Descripcion;

                    txtImagenUrl.Text = articulo.ImagenUrl;
                    CargarImagen(articulo.ImagenUrl);
                    txtNombre.Text = articulo.Nombre;
                    txtPrecio.Text = articulo.Precio.ToString();
                    cbCategorias.SelectedValue = articulo.categoria.IdCategoria;
                    cbMarcas.SelectedValue = articulo.marca.IdMarca;


                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }



        private void txtImagenUrl_Leave(object sender, EventArgs e)
        {
            CargarImagen(txtImagenUrl.Text);
        }
        private void CargarImagen(string imagen)
        {
            try
            {
                pbxArticuloAlta.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxArticuloAlta.Load("https://thealmanian.com/wp-content/uploads/2019/01/product_image_thumbnail_placeholder.png");
            }
        }

       private void txtPrecio_TextChanged(object sender, EventArgs e)
        {
            if (!(SoloNumeros(txtPrecio.Text)))
            {
                MessageBox.Show("Ingrese solo números", "Alerta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private bool SoloNumeros(string numeros)
        {
            foreach (char caracter in numeros)
            {
                if (!(char.IsDigit(caracter)) && !(char.IsPunctuation(caracter)))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
