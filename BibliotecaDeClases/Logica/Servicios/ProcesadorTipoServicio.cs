using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.Servicios;

namespace BibliotecaDeClases.Logica.Servicios
{
  public class ProcesadorTipoServicio
    {
        public static async Task<int> NuevoTipoServicioAsync(string nombre, string descipcion, string creadopor)
        {
            return await AccesoBD.Comando("Insert into TipoServicio (TipoServicio,Descripcion,CreadoPor,CreadoEn,EditadoPor,EditadoEn) " +
                "Values (@TipoServicio,@Descripcion,@CreadoPor,@CreadoEn,@EditadoPor,@EditadoEn)", new TipoServicioModel
                {
                    TipoServicio = nombre,
                    Descripcion = descipcion,
                    CreadoPor = creadopor,
                    CreadoEn = DateTime.Now,
                    EditadoEn = DateTime.Now,
                    EditadoPor = creadopor
                });
        }
        public  static async Task<int> EditaTipoServicioAsync(int id, string nombre, string descripcion, string editadopor)
        {
            return await AccesoBD.Comando("Update TipoServicio Set TipoServicio = @TipoServicio,Descripcion = @Descripcion,EditadoPor = @EditadoPor,EditadoEn = @EditadoEn where ID = @ID",
                new TipoServicioModel { ID = id, TipoServicio = nombre, Descripcion = descripcion, EditadoEn = DateTime.Now, EditadoPor = editadopor });
        }
    
        public static async Task<int> EliminaTipoServicioAsync(int id)
        {
            return await AccesoBD.Comando("Delete from TipoServicio where ID = @ID", new { ID = id });
        }
        
        public static async Task<List<SelectListItem>> TiposServiciosAsync()
        {
            var data = await AccesoBD.LoadDataAsync<TipoServicioModel>("Select ID, TipoServicio from TipoServicio");
            List<SelectListItem> lista = new List<SelectListItem>();
            foreach(var row in data)
            {
                lista.Add(new SelectListItem { Text = row.TipoServicio, Value = row.ID.ToString() });
            }
            return lista;
        }
    
        public static async Task<List<TipoServicioModel>> ListaTipoServicioAsync()
        {
            return await AccesoBD.LoadDataAsync<TipoServicioModel>("Select TipoServicio, Descripcion from TipoServicio");
        }
   
         public static async Task<TipoServicioModel> LoadTipoServicioAsync(int id)
        {
            return await AccesoBD.LoadAsync("Select TipoServicio, Descripcion, ID from TipoServicio where ID = @ID", new TipoServicioModel { ID = id });
        }
    }
}
