using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.Cotizaciones;
namespace BibliotecaDeClases.Logica.Cotizaciones
{
   public class ProcesadorTipoCotizacion
    {
        public static async Task<int> NuevoTipoCotizacionAsync(string tipocotizacion, string descripcion, string creadopor)
        {
            return await AccesoBD.Comando("Insert into TipoCotizacion (TipoCotizacion, Descripcion, CreadoEn, " +
                "EditadoPor, EditadoEn, CreadoPor) Values(@TipoCotizacion, @Descripcion, @CreadoEn, @EditadoPor, " +
                "@EditadoEn, @CreadoPor)", new { TipoCotizacion = tipocotizacion, Descripcion = descripcion, CreadoEn = DateTime.Now, 
                    EditadoPor = creadopor, EditadoEn = DateTime.Now, CreadoPor = creadopor });
        }

        public static async Task<int> EditarTipoCotizacionAsync(int id, string tipocotizacion, string descripcion, string editadopor)
        {
            return await AccesoBD.Comando("Update  TipoCotizacion Set TipoCotizacion = @TipoCotizacion , Descripcion = @Descripcion, " +
                "EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", new
             {
                 TipoCotizacion = tipocotizacion,
                 Descripcion = descripcion,
                 EditadoPor = editadopor,
                 EditadoEn = DateTime.Now,
                 ID = id                
             });

        }

        public static async Task<int> EliminarTipoCotizacionAsync(int id)
        {
            return await AccesoBD.Comando("Delete from TipoCotizacion where ID = @ID", new { ID = id });
        }

        public static async Task<List<TipoCotizacionModel>> ListaTipoCotizacionAsync()
        {
            return await AccesoBD.LoadDataAsync<TipoCotizacionModel>("Select * from TipoCotizacion");
        }

        public static async Task<TipoCotizacionModel> CargarTipoCotizacionAsync(int id)
        {
            return await AccesoBD.LoadAsync<TipoCotizacionModel>("Select TipoCotizacion.ID, TipoCotizacion, Descripcion, BDCreado.Nombre + ' ' + BDCreado.Apellido  CreadoPor, " +
                "BDActualizado.Nombre + ' ' + BDActualizado.Apellido  EditadoPor, TipoCotizacion.CreadoEn, TipoCotizacion.EditadoEn " +
                "from TipoCotizacion inner join PersonalServicios as BDCreado on TipoCotizacion.CreadoPor = BDCreado.IDEmpleado " +
                "inner join PersonalServicios as BDActualizado on TipoCotizacion.EditadoPor = BDActualizado.IDEmpleado " +
                "where TipoCotizacion.ID = @ID", new TipoCotizacionModel {ID = id });
        }
    }
}
