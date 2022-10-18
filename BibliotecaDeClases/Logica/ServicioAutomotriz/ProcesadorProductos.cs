using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;

namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorProductos
    {
        public static async Task nuevoProductoAsync(ProductoModel modelo)
        {
            var idproducto = await insertarProductoAsync(modelo);
            foreach (var item in modelo.FotoProductoList)
            {
                item.IDProducto = idproducto;
            }
            await insertarFotoProducto(modelo.FotoProductoList);
        }
        public static async Task<string> insertarProductoAsync(ProductoModel modelo)
        {
           return await Task.Run(async () => {
               var data = await AccesoBDAutomotriz.LoadAsync(@"Insert into Productos (ID, IDItem, Nombre, Descripcion, FechaCreacion, IDMarca) 
                                               OUTPUT INSERTED.ID   
                                               Values (NEWID(), @IDItem, @Nombre, @Descripcion, GETDATE(), @IDMarca)", modelo);
               return data.ID;
             });
            
        }
        public static async Task<List<InventarioModel>> getInventarioItemsAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<InventarioModel>(@"
                Select ID, Item, PVENTA, PCompra , Categoria, Stock, [dbo].[CheckProduct] (ID,Categoria) Product from Inventario 
                where Categoria <> 'SERVICIOS' 
                and ID <> '20202'
                and ID <> '20206'
                and ID <> '20207'
                and ID <> '20208'
                and ID <> '20209'
                and ID <> '20203'
                and ID <> '20204'
                and ID <> '20205'
                order by Item asc
                ");
            });
            
        }

        public static async Task<List<InventarioModel>> getInventarioNeumaticosAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<InventarioModel>(@"
                Select ID, Item, PVENTA, PCompra , Categoria, Stock, [dbo].[CheckProduct] (ID,Categoria) Product from Inventario 
                where Categoria <> 'SERVICIOS' 
                and ID <> '20202'
                and ID <> '20206'
                and ID <> '20207'
                and ID <> '20208'
                and ID <> '20209'
                and ID <> '20203'
                and ID <> '20204'
                and ID <> '20205'
                and Categoria = 'NEUMATICOS'
                order by Item asc
                ");
            });

        }
        public static async Task<List<InventarioModel>> getItemsAsync(string text)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync(@"
                Select ID, Item, PVENTA, PCompra , Categoria, Stock, [dbo].[CheckProduct] (ID,Inventario.Categoria) Product from Inventario 
                where Categoria <> 'SERVICIOS' 
                and ID <> '20202'
                and ID <> '20206'
                and ID <> '20207'
                and ID <> '20208'
                and ID <> '20209'
                and ID <> '20203'
                and ID <> '20204'
                and ID <> '20205'
                and Item like '%' + @Item + '%'
                order by Item asc
                ", new InventarioModel { Item = text });
            });

        }
        public static async Task<ProductoModel> getItemByIDAsync(string id)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadAsync("Select Item Nombre, ID IDItem, PVENTA Precio, STOCK Stock, PCompra PrecioCompra  from Inventario where ID = @ID", new ProductoModel {ID = id });
            });           
        }
        public static async Task insertarFotoProducto(List<FotoProductoModel> lista)
        {
            foreach (var item in lista)
            {
                await Task.Run(async() => {
                    await AccesoBDAutomotriz.Comando("Insert into FotoProducto (ID, IDFoto, IDProducto) Values (NEWID(), @IDFoto, @IDProducto)", item);
                });
            }
            
        }
        public static async Task<List<ProductoModel>> getProductosAsync()
        {
            return await Task.Run(async () => {
                var products = await AccesoBDAutomotriz.LoadDataAsync<ProductoModel>(@"Select Productos.ID, Productos.Nombre, MarcaProducto.Marca, [dbo].[ContarFotoProducto] (Productos.ID) CantidadFotos, Inventario.PVENTA Precio, Inventario.Categoria, Inventario.STOCK Stock, FotoLogo.URL Logo
                                                                               from Productos
                                                                               inner join MarcaProducto on Productos.IDMarca = MarcaProducto.ID
                                                                               inner join Inventario on Productos.IDItem = Inventario.ID
                                                                               inner join FotoLogo on FotoLogo.IDMarca = Productos.IDMarca");
                foreach (var item in products)
                {
                    var fotos = await getFotosProducto(item.ID);
                    item.Fotos = fotos;
                }
                return products;
            });            
        }
        public static async Task<List<FotoModel>> getFotosProducto(string idproducto)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select Fotos.ID, Fotos.Nombre, Fotos.URL from Fotos 
                                                            inner join FotoProducto on Fotos.ID = FotoProducto.IDFoto
                                                            where FotoProducto.IDProducto = @ID", new FotoModel { ID = idproducto });
            });
            
        }
        public static async Task<List<FotoProductoModel>> getFotosProductoList(string idproducto)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select ID, IDFoto, IDProducto from FotoProducto where IDProducto = @IDProducto ", new FotoProductoModel { IDProducto = idproducto });
            });

        }
        public static async Task<ProductoModel> getProductoAsync(string idproducto)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadAsync(@"
                Select Productos.ID, Productos.Nombre, Productos.Descripcion, Productos.IDMarca, MarcaProducto.Marca, Inventario.PVENTA Precio, Inventario.STOCK Stock, Inventario.PCompra PrecioCompra
                from Productos
                inner join MarcaProducto on Productos.IDMarca = MarcaProducto.ID
                inner join Inventario on Productos.IDItem = Inventario.ID
                where Productos.ID = @ID
                ", new ProductoModel {ID = idproducto });
            });
            
        }
        public static async Task edicionProductoAsync(ProductoModel modelo)
        {
            await updateProductoAsync(modelo);
            await updateFotoProductoAsync(modelo.FotoProductoList);
        }
        public static async Task updateProductoAsync(ProductoModel modelo)
        {

            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando("Update Productos Set Nombre = @Nombre, Descripcion = @Descripcion, IDMarca = @IDMarca where ID = @ID ", modelo );
            });
        }
        public static async Task updateFotoProductoAsync(List<FotoProductoModel> lista)
        {
            await Task.Run(async () => {

                await AccesoBDAutomotriz.Comando("Delete from FotoProducto where IDProducto = @ID", new { ID = lista.FirstOrDefault().IDProducto });
                foreach (var item in lista)
                {
                    await Task.Run(async () => {
                        await AccesoBDAutomotriz.Comando("Insert into FotoProducto (ID, IDFoto, IDProducto) Values (NEWID(), @IDFoto, @IDProducto)", item);
                    });
                }
            });
            
        }
        public static async Task<List<InventarioModel>> getItemsAsync(string text, string categoria)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync(@" Select ID, Item, PVENTA, PCompra , Categoria, Stock, [dbo].[CheckProduct] (ID,Inventario.Categoria) Product from Inventario 
                where Categoria <> 'SERVICIOS' 
                and ID <> '20202'
                and ID <> '20206'
                and ID <> '20207'
                and ID <> '20208'
                and ID <> '20209'
                and ID <> '20203'
                and ID <> '20204'
                and ID <> '20205'
                and Item like '%' + @Item + '%' and Categoria = @Categoria
                order by Item asc", new InventarioModel {Item = text, Categoria = categoria });
            });

        }
        public static async Task<List<InventarioModel>> getItemsCategoriaAsync(string categoria)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync(@" Select ID, Item, PVENTA, PCompra , Categoria, Stock, [dbo].[CheckProduct] (ID,Inventario.Categoria) Product from Inventario 
                where Categoria <> 'SERVICIOS' 
                and ID <> '20202'
                and ID <> '20206'
                and ID <> '20207'
                and ID <> '20208'
                and ID <> '20209'
                and ID <> '20203'
                and ID <> '20204'
                and ID <> '20205'
                and Categoria = @Categoria
                order by Item asc", new InventarioModel { Item = categoria, Categoria = categoria });
            });

        }

        public static async Task<List<FotoModel>> getFotosSeccionOpcionesAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoModel>(@"Select Fotos.Nombre, Fotos.URL from Fotos inner join Secciones on Secciones.ID = Fotos.IDSeccion where Seccion = 'OPCIONES'");
            });

        }
        public static async Task<List<SelectListItem>> selectCategorias()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"select distinct Categoria Text, Categoria Value from Inventario where Categoria <> 'DESCUENTOS' AND Categoria <> 'SERVICIOS'");
            });

        }
    }
}
