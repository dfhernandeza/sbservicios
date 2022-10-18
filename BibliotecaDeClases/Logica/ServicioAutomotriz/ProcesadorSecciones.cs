using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorSecciones
    {
        public static async Task insertarSeccionAsync(SeccionesModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"Insert into Secciones (ID, Seccion, Descripcion) Values (NEWID(), @Seccion, @Descripcion)", modelo);
            });
        }
        public static async Task editarSeccionAsync(SeccionesModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"Update Secciones Seccion = @Seccion, Descripcion = @Descripcion where ID = @ID)", modelo);
            });
        }
        public static async Task eliminarSeccionAsync(string id)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"Delete from Secciones where ID = @ID)", new {ID = id });
            });
        }
        public static async Task<List<SeccionesModel>> getSeccionesAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<SeccionesModel>("Select ID, Seccion, Descripcion from Secciones");
            });
            
        }
        public static async Task<List<SelectListItem>> getSeccionesListAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>("Select ID Value, Seccion Text from Secciones");
            });

        }
        public static async Task<SeccionesModel> getSeccionAsync(string id)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadAsync("Select ID, Seccion, Descripcion from Secciones where ID = @ID", new SeccionesModel {ID = id });
            });
        }
    }
}
