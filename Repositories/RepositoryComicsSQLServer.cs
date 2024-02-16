using SegundaPracticaFundamentos.Models;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;

namespace SegundaPracticaFundamentos.Repositories
{
    #region PROCEDIMIENTOS ALMACENADOS
    /* PROCEDURE CREAR COMIC    
    create procedure SP_CREATE_COMIC
    (@nombre NVARCHAR(150), @imagen NVARCHAR(600), @descripcion NVARCHAR(500)) 
    as 
        DECLARE @IDCOMIC INT
        SELECT @IDCOMIC = MAX(IDCOMIC) FROM COMICS
        SET @IDCOMIC = @IDCOMIC + 1


        insert into COMICS values(@IDCOMIC, @nombre, @imagen, @descripcion)
    go*/
    #endregion
    public class RepositoryComicsSQLServer : IRepositoryComics
    {
        private DataTable tabla;
        private SqlConnection cn;
        private SqlCommand com;

        public RepositoryComicsSQLServer()
        {
            string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=sa;Password=MCSD2023";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            this.tabla = new DataTable();
            string sql = "select * from COMICS";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            ad.Fill(this.tabla);
        }
        public List<Comic> GetComics()
        {
            var consulta = from datos in this.tabla.AsEnumerable()
                           select datos;
            List<Comic> comics = new List<Comic>();
            foreach (var row in consulta)
            {
                Comic comic = new Comic
                {
                    IdComic = row.Field<int>("IDCOMIC"),
                    Nombre = row.Field<string>("NOMBRE"),
                    Imagen = row.Field<string>("IMAGEN"),
                    Descripcion = row.Field<string>("DESCRIPCION")
                };
                comics.Add(comic);
            }
            return comics;
        }

        public void InsertComicLambda(Comic comic)
        {
            int max = 0;
            var consulta = from datos in this.tabla.AsEnumerable()
                           select datos.Field<int>("IDCOMIC");
            max = consulta.Max(x => x) + 1;
            string sql = "insert into COMICS values (@idcomic,@nombre,@imagen,@descripcion)";
            this.com.Parameters.AddWithValue("@idcomic", max);
            this.com.Parameters.AddWithValue("@nombre", comic.Nombre);
            this.com.Parameters.AddWithValue("@imagen", comic.Imagen);
            this.com.Parameters.AddWithValue("@descripcion", comic.Descripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
        public void InsertComicProcedure(Comic comic)
        {
            this.com.Parameters.AddWithValue("@nombre", comic.Nombre);
            this.com.Parameters.AddWithValue("@imagen", comic.Imagen);
            this.com.Parameters.AddWithValue("@descripcion", comic.Descripcion);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CREATE_COMIC";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeleteComic(int idcomic)
        {
            throw new NotImplementedException();
        }

        public List<string> GetAllComicsString()
        {
            var consulta = from datos in this.tabla.AsEnumerable()
                           select datos;
            List<string> comics = new List<string>();
            foreach (var row in consulta)
            {
                comics.Add(row.Field<string>("NOMBRE"));
            }
            return comics;
        }

        public Comic GetComic(int idcomic)
        {
            var consulta = from datos in this.tabla.AsEnumerable()
                           where datos.Field<int>("IDCOMIC") == idcomic
                           select datos;
            var row = consulta.First();
            Comic comic = new Comic
            {
                IdComic = row.Field<int>("IDCOMIC"),
                Nombre = row.Field<string>("NOMBRE"),
                Imagen = row.Field<string>("IMAGEN"),
                Descripcion = row.Field<string>("DESCRIPCION")
            };
            return comic;
        }

        

        

        public void UpdateComic(Comic comic)
        {
            throw new NotImplementedException();
        }
    }
}
