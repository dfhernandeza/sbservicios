using BibliotecaDeClases.Modelos.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica.Servicios
{
   public class ProcesadorEstadoServicios
    {
        public static async Task<int> CreaEstadoServicioAsync(string estado, string descripcion, string creadopor)
        {
            string sql = "Insert into EstadoServicio (Estado, Descripcion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) Values (@Estado, @Descripcion, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";

            EstadoServicioModel modelo = new EstadoServicioModel
            {
                Estado = estado,
                Descripcion = descripcion,
                CreadoEn = DateTime.Now,
                CreadoPor = creadopor,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor
            };
            return await AccesoBD.Comando(sql, modelo);
        }

        public static async Task<int> EliminaEstadoServicioAsync(int id)
        {
            string sql = "Delete from EstadoServicio where ID = @ID";
            EstadoServicioModel modelo = new EstadoServicioModel
            {
                ID = id
            };

          return await  AccesoBD.Comando(sql, modelo);

        }

        public static async Task<int> ActualizaEstadoServicioAsync(int id, string estado, string descripcion, string editadopor)
        {
            string sql = "Update EstadoServicio Set Estado = @Estado, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID";
            EstadoServicioModel modelo = new EstadoServicioModel
            {
                ID = id,
                EditadoEn = DateTime.Now,
                EditadoPor = editadopor,
                Descripcion = descripcion,
                Estado = estado
            };

            return await AccesoBD.Comando(sql, modelo);
        }

        public static async Task<List<EstadoServicioModel>> ListaEstadoServicioAsync ()
        {
            string sql = "Select * from EstadoServicio";
            return await AccesoBD.LoadDataAsync<EstadoServicioModel>(sql);
        }

        public static async Task<EstadoServicioModel> LoadEstadoServicioAsync(int id)
        {
            string sql = "Select * from EstadoServicio where ID = @ID";
            EstadoServicioModel modelo = new EstadoServicioModel
            {
                ID = id
            };

          return await  AccesoBD.LoadAsync(sql, modelo);



        } 
     
        public static async Task<List<SelectListItem>> ListaEstadoServicioItemAsync()
        {

            string sql = "Select * from EstadoServicio ";

            var lista = await AccesoBD.LoadDataAsync<EstadoServicioModel>(sql);

            List<SelectListItem> listado = new List<SelectListItem>();

            foreach(var row in lista)
            {
                SelectListItem item = new SelectListItem
                {
                    Value = row.ID.ToString(),
                    Text = row.Estado
                };

                listado.Add(item);
            }

            return listado;
        }



    }
}
