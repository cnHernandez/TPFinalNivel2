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
        public List<Articulo> Listar ()
        {
            List<Articulo> lista = new List<Articulo>();
            SqlConnection conexion = new SqlConnection();
            SqlCommand comando = new SqlCommand();
            SqlDataReader lector;
            try
            {
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=CATALOGO_DB; integrated security=true";
                comando.CommandType = System.Data.CommandType.Text;
                comando.CommandText = "select Codigo, Nombre, A.Descripcion, M.Descripcion Marca, C.Descripcion Categoria, A.ImagenUrl, Precio, A.IdCategoria, A.IdMarca, A.Id from ARTICULOS A, CATEGORIAS C, MARCAS M where C.Id = A.IdCategoria and M.Id = A.IdMarca"
;
                comando.Connection = conexion;

                conexion.Open();
                lector = comando.ExecuteReader();

                while (lector.Read())
                {
                    Articulo aux = new Articulo();

                    aux.IdArticulo = (int)lector["Id"];
                    //aux.Codigo = (string)lector["Codigo"];
                    aux.Codigo = lector.GetString(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];
                    aux.Precio = (Decimal)lector["Precio"];

                    if (!(lector["ImagenUrl"] is DBNull))
                        aux.ImagenUrl = (string)lector["ImagenUrl"];

                    aux.categoria = new Categoria();
                    
                    aux.categoria.Descripcion = (string)lector["Categoria"];
                    aux.categoria.IdCategoria = (int)lector["IdCategoria"];
                    
                    aux.marca = new Marca();
                  
                    aux.marca.Descripcion = (string)lector["Marca"];
                    aux.marca.IdMarca = (int)lector["IdMarca"];

                    lista.Add(aux);
                }

                conexion.Close();
                return lista;
            }
            catch (Exception e)
            {
                throw e;
            }
            

        }

        public void Agregar(Articulo nuevo)
        {
            AccesoDatos datos = new AccesoDatos();
            try
            {
                datos.setearConsulta("insert into ARTICULOS (Codigo,Nombre,Descripcion,IdMarca, IdCategoria, ImagenUrl,Precio) values ('"+ nuevo.Codigo+"','"+nuevo.Nombre+"','"+nuevo.Descripcion+"',@IdMarca,@IdCategoria,'"+nuevo.ImagenUrl+"',"+nuevo.Precio+")");
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


    }
    }

