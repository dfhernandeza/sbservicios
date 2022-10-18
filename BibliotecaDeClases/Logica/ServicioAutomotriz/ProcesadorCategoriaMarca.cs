using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorCategoriaMarca
    {
        public static async Task insertarNuevaCategoriaMarcaAsync(CategoriaMarcaModel modelo)
        {
            await Task.Run(async () => {
                modelo.Categoria = modelo.Categoria.ToUpper();
                modelo.Descripcion = modelo.Descripcion.ToUpper();
                await AccesoBDAutomotriz.Comando(@"Insert into CategoriaMarca (ID, Categoria,Descripcion) Values (NEWID(),@Categoria,@Descripcion)",modelo);
            });
        }
        public static async Task updateCategoriaMarcaAsync(CategoriaMarcaModel modelo)
        {
            await Task.Run(async () => {
                modelo.Categoria = modelo.Categoria.ToUpper();
                modelo.Descripcion = modelo.Descripcion.ToUpper();
                await AccesoBDAutomotriz.Comando(@"Update CategoriaMarca Set Categoria = @Categoria, Descripcion = @Descripcion where ID = @ID", modelo);
            });
        }
        public static async Task deleteCategoriaMarcaAsync(string id)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"Delete from CategoriaMarca where ID = @ID", new {ID = id });
            });
        }
        public static async Task<List<CategoriaMarcaModel>> getCategoriasMarcaAsync()
        {
           return await Task.Run(async () => {
              return  await AccesoBDAutomotriz.LoadDataAsync<CategoriaMarcaModel>(@"Select ID, Categoria, Descripcion from CategoriaMarca");
            });
        }
        public static async Task<List<SelectListItem>> getCategoriasMarcaListAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select ID Value, Categoria Text from CategoriaMarca");
            });
        }
        public static async Task<CategoriaMarcaModel> getCategoriaMarcaAsync(string id)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadAsync(@"Select ID, Categoria, Descripcion from CategoriaMarca where ID = @ID", new CategoriaMarcaModel {ID = id });
            });
        }
    }
}
