using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorCategoriaFoto
    {
        public static async Task insertarCategoriaFotoAsync(CategoriaFotoModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"Insert into CategoriaFoto (ID, Categoria, Descripcion) Values (NEWID(), @Categoria, @Descripcion)",modelo);
            });
          
        }
        public static async Task<List<CategoriaFotoModel>> getCategoriasFotoAsync()
        {
           return await Task.Run(async () => {
              return  await AccesoBDAutomotriz.LoadDataAsync<CategoriaFotoModel>(@"Select ID, Categoria, Descripcion from CategoriaFoto");
            });

        }
        public static async Task<List<SelectListItem>> getListCategoriasFotoAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select ID Value, Categoria Text from CategoriaFoto");
            });

        }
    }
}
