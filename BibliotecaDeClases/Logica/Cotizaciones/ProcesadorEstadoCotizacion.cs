using BibliotecaDeClases.Modelos.Cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica.Cotizaciones
{
  public  class ProcesadorEstadoCotizacion
    {
        public static async Task<int> CrearEstadoCotizacionAsync(string estado, string descripcion, string creadopor)
        {
            string sql = "Insert into EstadoCotizaciones (Estado, Descripcion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) Values (@Estado, @Descripcion,@CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn) ";

            EstadoCotizacionModel modelo = new EstadoCotizacionModel
            {

                Descripcion = descripcion,
                Estado = estado,
                CreadoPor = creadopor,
                EditadoPor = creadopor,
                EditadoEn = DateTime.Now,
                CreadoEn = DateTime.Now

            };

          return await AccesoBD.Comando(sql, modelo);

        }

        public static async Task<List<EstadoCotizacionModel>> CargarEstadoCotizacionAsync(int id)
        {
            string sql = "Select * from EstadoCotizaciones where ID = @ID";
            EstadoCotizacionModel modelo = new EstadoCotizacionModel
            {
                ID = id
            };

            return await AccesoBD.LoadDataAsync(sql, modelo);

        }

        public static async Task<List<EstadoCotizacionModel>> CargarEstadosCotizacionesAsync()
        {
            string sql = "Select * from EstadoCotizaciones ";

            var data = await AccesoBD.LoadDataAsync<EstadoCotizacionModel>(sql);

            List<EstadoCotizacionModel> Lista = new List<EstadoCotizacionModel>();
         
            foreach(var row in data)
            {
                Lista.Add(new EstadoCotizacionModel { ID = row.ID, Descripcion = row.Descripcion, Estado = row.Estado });


            }

            return Lista;
           
        }

        public static async Task<int> EditarEstadoCotizacionAsync(int id, string estado, string descripcion, string editadopor)
        {

            string sql = "Update EstadoCotizaciones Set Estado = @Estado, Descripcion = @Descripcion, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID ";
            EstadoCotizacionModel model = new EstadoCotizacionModel
            {
                EditadoPor = editadopor,
                EditadoEn = DateTime.Now,
                Descripcion = descripcion,
                Estado = estado,
                ID = id
                
            };

            return await AccesoBD.Comando(sql, model);
        }

        public static async Task<int> EliminarEstadoCotizacionAsync(int id)
        {
            string sql = "Delete from EstadoCotizaciones where ID = @ID";
            EstadoCotizacionModel model = new EstadoCotizacionModel
            {
                ID = id
            };

            return await AccesoBD.Comando(sql, model);

        }

        public static async Task<List<SelectListItem>> ListaEstadoCotizacionAsync()
        {
            string sql = "Select * from EstadoCotizaciones";
            var data = await AccesoBD.LoadDataAsync<EstadoCotizacionModel>(sql);

            List<SelectListItem> Lista = new List<SelectListItem>();

            foreach(var row in data)
            {

                Lista.Add(new SelectListItem { Text = row.Estado, Value = row.ID.ToString() });

            }

            return Lista;
        }




    }
}
