using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Dominio;


namespace Negocio
{
    public class ArticuloNegocio
    {
        public List<Articulo> Listar()
        {
            List<Articulo> lista = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();

            try
            {
                datos.setearConsulta("select Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.ImagenUrl, Precio, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca");
                datos.ejecutarLectura();

                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.IdArticulo = (int)datos.Lector["Id"];
                    aux.Codigo = datos.Lector.GetString(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.categoria = new Categoria();

                    aux.categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.categoria.IdCategoria = (int)datos.Lector["IdCategoria"];

                    aux.marca = new Marca();

                    aux.marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.marca.IdMarca = (int)datos.Lector["IdMarca"];

                    lista.Add(aux);
                }
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                datos.cerrarConexion();
            }

        }

        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca, IdCategoria, ImagenUrl,Precio) values ('" + nuevo.Codigo + "','" + nuevo.Nombre + "','" + nuevo.Descripcion + "',@IdMarca,@IdCategoria,'" + nuevo.ImagenUrl + "'," + nuevo.Precio + ")");
                datos.setearParametro("@IdMarca", nuevo.marca.IdMarca);
                datos.setearParametro("@IdCategoria", nuevo.categoria.IdCategoria);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        public void Modificar(Articulo modificado)
        {
            AccesoDatos acceso = new AccesoDatos();
            try
            {
                acceso.setearConsulta("update ARTICULOS set Codigo=@codigo,Nombre=@nombre,Descripcion=@descripcion,idMarca =@idMarca,IdCategoria=@idCategoria,ImagenUrl=@ImagenUrl,Precio=@precio where Id=@iD");
                acceso.setearParametro("@codigo", modificado.Codigo);
                acceso.setearParametro("@nombre", modificado.Nombre);
                acceso.setearParametro("@descripcion", modificado.Descripcion);
                acceso.setearParametro("@idMarca", modificado.marca.IdMarca);
                acceso.setearParametro("@idCategoria", modificado.categoria.IdCategoria);
                acceso.setearParametro("@ImagenUrl", modificado.ImagenUrl);
                acceso.setearParametro("@precio", modificado.Precio);
                acceso.setearParametro("@iD", modificado.IdArticulo);

                acceso.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                acceso.cerrarConexion();
            }
        }

        public void Eliminar(int id)
        {
            try
            {
                AccesoDatos datos = new AccesoDatos();
                datos.setearConsulta("delete from ARTICULOS where Id= @Id ");
                datos.setearParametro("Id", id);
                datos.EjecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<Articulo> filtrar(string campo, string criterio, string filtroAvanzado)
        {
            List<Articulo> articulos = new List<Articulo>();
            AccesoDatos datos = new AccesoDatos();
            try
            {
                string consulta = "select Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.ImagenUrl,Precio, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca and ";
                switch (campo)
                {
                   
                    case "Marca":
                        switch (criterio)
                        {
                            case "Contiene las letras: ":

                                consulta += "M.Descripcion like '%" + filtroAvanzado + "%'";

                                break;
                        }
                        break;
                    case "Categoria":
                        switch (criterio)
                        {
                            case "Contiene las letras: ":
                                consulta += "C.Descripcion like '%" + filtroAvanzado + "%'";
                                break;
                        }
                        break;
                    case "Nombre":
                        switch (criterio)
                        {
                            case "Contiene las letras: ":
                                consulta += "A.Nombre like '%" + filtroAvanzado + "%'"; 
                                break;
                        }
                        break;

                }
            
                if (campo == "Precio")
                {
                    switch (criterio)
                    {
                        case "Precio mayor a: ":
                            consulta += "Precio > " + filtroAvanzado;
                            break;

                        case "Precio menor a: ":
                            consulta += "Precio < " + filtroAvanzado;
                            break;
                    }

                }

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();


                while (datos.Lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.IdArticulo = (int)datos.Lector["Id"];

                    aux.Codigo = datos.Lector.GetString(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];
                    aux.Precio = (decimal)datos.Lector["Precio"];

                    if (!(datos.Lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)datos.Lector["ImagenUrl"];

                    aux.categoria = new Categoria();

                    aux.categoria.Descripcion = (string)datos.Lector["Categoria"];
                    aux.categoria.IdCategoria = (int)datos.Lector["IdCategoria"];

                    aux.marca = new Marca();

                    aux.marca.Descripcion = (string)datos.Lector["Marca"];
                    aux.marca.IdMarca = (int)datos.Lector["IdMarca"];

                    articulos.Add(aux);
                }
                return articulos;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}


