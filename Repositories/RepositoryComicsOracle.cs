using Microsoft.AspNetCore.Http.HttpResults;
using Oracle.ManagedDataAccess.Client;
using SegundaPracticaFundamentos.Models;
using System.Collections.Generic;
using System;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SegundaPracticaFundamentos.Repositories
{
    #region PROCEDIMIENTO CREAR
    /*        
     create or replace procedure SP_CREATE_COMIC
    (sp_nombre COMICS.NOMBRE%TYPE, sp_imagen COMICS.IMAGEN%TYPE,
    sp_descripcion COMICS.DESCRIPCION%TYPE)
    IS
       v_idcomic INTEGER;
    BEGIN
       SELECT MAX(IDCOMIC) INTO v_idcomic FROM COMICS;
            v_idcomic := v_idcomic + 1;


       insert into COMICS values(v_idcomic, sp_nombre, sp_imagen
       , sp_descripcion);
            commit;
    end;*/
    #endregion
    public class RepositoryComicsOracle:IRepositoryComics
    {
        private DataTable tabla;
        private OracleConnection cn;
        private OracleCommand com;

        public RepositoryComicsOracle()
        {
            string connectionString = @"Data Source=LOCALHOST:1521/XE;Persist Security Info=True;User Id=SYSTEM;Password=oracle";
            this.cn = new OracleConnection(connectionString);
            this.com = new OracleCommand();
            this.com.Connection = this.cn;
            this.tabla = new DataTable();
            string sql = "select * from COMICS";
            OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
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

        public void InsertComicLambda(Comic comic)
        {
            int max = 0;
            var consulta = from datos in this.tabla.AsEnumerable()
                           select datos.Field<int>("IDCOMIC");
            max = consulta.Max(x => x) + 1;
            string sql = "insert into COMICS values (:idcomic,:nombre,:imagen,:descripcion)";
            OracleParameter pamId = new OracleParameter(":idcomic", max);
            this.com.Parameters.Add(pamId);
            OracleParameter pamNombre = new OracleParameter(":nombre", comic.Nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":imagen", comic.Imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":descripcion", comic.Descripcion);
            this.com.Parameters.Add(pamDescripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
        public void InsertComicProcedure(Comic comic)
        {
            OracleParameter pamNombre = new OracleParameter(":sp_nombre", comic.Nombre);
            this.com.Parameters.Add(pamNombre);
            OracleParameter pamImagen = new OracleParameter(":sp_imagen", comic.Imagen);
            this.com.Parameters.Add(pamImagen);
            OracleParameter pamDescripcion = new OracleParameter(":sp_descripcion", comic.Descripcion);
            this.com.Parameters.Add(pamDescripcion);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = "SP_CREATE_COMIC";
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }

        public void DeleteComic(int idcomic)
        {
            string sql = "delete from COMICS where IDCOMIC=:idcomic";
            OracleParameter pamId = new OracleParameter(":idcomic", idcomic);
            this.com.Parameters.Add(pamId);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
