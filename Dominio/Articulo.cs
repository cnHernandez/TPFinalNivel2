using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dominio
{
    public class Articulo
    {
        [DisplayName("Código")]
        public string Codigo { get; set; }
        public string Nombre  { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }
        [DisplayName("Imagen URL")]
        public string  ImagenUrl { get; set; }
        public decimal Precio { get; set; }

        public int IdArticulo { get; set; }

        public override string ToString()
        {
            return Descripcion;
        }

        public Categoria categoria { get; set; }
        public Marca marca { get; set; }
    }

    
}
