using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
  public  class ProcesadorModelos
    {
        public static async Task insertarModeloAsync(ModeloModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBDAutomotriz.Comando(@"Insert into Modelos (ID, Nombre, IDMarca, Descripcion) Values (NEWID(), @Nombre, @IDMarca, @Descripcion)", modelo);
            });
        }
        public static async Task updateModeloAsync(ModeloModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBDAutomotriz.Comando(@"Update Modelos Set Nombre = @Nombre, Descripcion = @Descripcion where ID = @ID", modelo);
            });
        }
        public static async Task deleteModeloAsync(string id)
        {
            await Task.Run(async () =>
            {
                await AccesoBDAutomotriz.Comando(@"Delete from Modelos where ID = @ID", new { ID = id });
            });
        }
        public static async Task<ModeloModel> selectModeloAsync(string id)
        {
           return await Task.Run(async () =>
            {
               return await AccesoBDAutomotriz.LoadAsync(@"Select ID,Nombre,IDMarca, Descripcion from Modelos where ID = @ID", new ModeloModel { ID = id });
            });

        }
        public static async Task<List<ModeloModel>> getModelosAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<ModeloModel>(@"Select Modelos.ID, Modelos.Nombre, MarcaProducto.Marca, Modelos.IDMarca, Modelos.Descripcion from Modelos 
                                                                            inner join MarcaProducto on Modelos.IDMarca = MarcaPrducto.ID");
            });
                
        }
        public static async Task<List<SelectListItem>> getmodelosListAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select ID Value, Nombre Text from Modelos");
            });

        }
    }
}
