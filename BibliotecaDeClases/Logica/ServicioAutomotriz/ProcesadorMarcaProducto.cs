using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorMarcaProducto
    {
        public static async Task insertarMarcaPrductoAsync(MarcaProductoModel modelo)
        {
            await Task.Run(async () => {
                modelo.Marca = modelo.Marca.ToUpper();
                modelo.Descripcion = modelo.Descripcion.ToUpper();
                await AccesoBDAutomotriz.Comando("Insert into MarcaProducto (ID, Marca, IDCategoria, Descripcion) Values (NEWID(), @Marca, @IDCategoria, @Descripcion)", modelo);
            });
            
        }
        public static async Task updateMarcaProductoAsync(MarcaProductoModel modelo)
        {
            await Task.Run(async () => {
                modelo.Marca = modelo.Marca.ToUpper();
                modelo.Descripcion = modelo.Descripcion.ToUpper();
                await AccesoBDAutomotriz.Comando("Update MarcaProducto Set Marca = @Marca, IDCategoria = @IDCategoria, Descripcion = @Descripcion Where ID = @ID", modelo);
            });
        }
        public static async Task<List<MarcaProductoModel>> getMarcasProductos()
        {
          return  await Task.Run(async () => {
              return  await AccesoBDAutomotriz.LoadDataAsync<MarcaProductoModel>(@"Select MarcaProducto.ID, MarcaProducto.Marca, MarcaProducto.IDCategoria, 
            MarcaProducto.Descripcion, CategoriaMarca.Categoria from MarcaProducto inner join CategoriaMarca on MarcaProducto.IDCategoria = CategoriaMarca.ID order by CategoriaMarca.Categoria  ");
            });

        }
        public static async Task<MarcaProductoModel> getMarcaProducto(string id)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadAsync("Select ID, Marca, IDCategoria, Descripcion from MarcaProducto where ID = @ID", new MarcaProductoModel { ID = id});
            });

        }
        public static async Task<List<SelectListItem>> getListaMarcasAsync()
        {

            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>("Select ID Value, Marca Text from MarcaProducto");
            });
        }
        public static async Task<List<ModeloModel>> getModelosMarcaAsync(string idmarca)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select ID, Nombre, Descripcion from Modelos where IDMarca = @IDMarca",new ModeloModel {IDMarca = idmarca });
            });

        }
    }
}
