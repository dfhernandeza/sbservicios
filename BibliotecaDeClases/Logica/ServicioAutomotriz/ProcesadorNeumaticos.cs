using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.AccesoBlobs.ProcesadorFotoWeb;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorFotos;
using static BibliotecaDeClases.AccesoBDAutomotriz;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorNeumaticos
    {
      
        public static async Task<NeumaticosModel> getMarcayModeloStringAsync(string idmarca, string idmodelo)
        {

            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadAsync(@"Declare @Marca nvarchar(255);
                Declare @Modelo nvarchar(255);
                Select @Marca = Marca from MarcaProducto where ID = @IDMarca;
                Select @Modelo = Nombre from Modelos where ID = @IDModelo;
                Select @Marca Marca, @Modelo Modelo", new NeumaticosModel {IDMarca = idmarca, IDModelo = idmodelo }); 
            });
        }
        public static async Task insertarNeumaticoFotoAsync(List<NeumaticoFotoModel> lista)
        {
            foreach (var item in lista)
            {
               await Task.Run(async () => {
                   await AccesoBDAutomotriz.Comando("Insert into NeumaticoFoto (ID, IDNeumatico, IDFoto) Values (NEWID(), @IDNeumatico, @IDFoto)", item);
                });
            }          
        }
        public static async Task editarNeumaticoFotoAsync(List<NeumaticoFotoModel> lista)
        {
            await AccesoBDAutomotriz.Comando("Delete from NeumaticoFoto where IDNeumatico = @ID", new { ID = lista.FirstOrDefault().IDNeumatico });
            foreach (var item in lista)
            {
                await Task.Run(async () => {
                    await AccesoBDAutomotriz.Comando("Insert into NeumaticoFoto (ID, IDNeumatico, IDFoto) Values (NEWID(), @IDNeumatico, @IDFoto)", item);
                });
            }
        }
        public static async Task nuevoNeumaticoAsync(NeumaticosModel modelo)
        {
            var id = await ingresarNuevoNeumaticoAsync(modelo);
            foreach (var item in modelo.NeumaticoFotoList)
            {
                item.IDNeumatico = id;
            }
            await insertarNeumaticoFotoAsync(modelo.NeumaticoFotoList);
        }
        public static async Task<string> ingresarNuevoNeumaticoAsync(NeumaticosModel modelo)
        {
           return await Task.Run(async () => {
              var data =  await AccesoBDAutomotriz.LoadAsync(@"
            Insert into Neumaticos (ID, IDItem, IDMarca, IDAncho, IDPerfil, IDAro, IDModelo, Precio, Descripcion,VMax, Carga, Origen, PAire, IndiceV)
            OUTPUT Inserted.ID
            Values (NEWID(), @IDItem, @IDMarca, @IDAncho, @IDPerfil, @IDAro, @IDModelo, @Precio, @Descripcion,@VMax, @Carga, @Origen, @PAire, @IndiceV)", modelo);
               return data.ID;
            });
           
        }
        public static async Task editorNeumaticoAsync(NeumaticosModel modelo)
        {
            await editarNeumaticoAsync(modelo);
            foreach (var item in modelo.NeumaticoFotoList)
            {
                item.IDNeumatico = modelo.ID;
            }
            await editarNeumaticoFotoAsync(modelo.NeumaticoFotoList);
        }
        public static async Task editarNeumaticoAsync(NeumaticosModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"
            Update Neumaticos Set IDMarca = @IDMarca, IDAncho = @IDAncho, IDPerfil = @IDPerfil, IDAro = @IDAro, IDModelo = @IDModelo, 
            Descripcion = @Descripcion, VMax = @VMax, Carga = @Carga, 
            Origen = @Origen, PAire = @PAire, IndiceV = @IndiceV where ID = @ID", modelo);

            });

        }
        public static async Task<NeumaticosModel> getNeumaticoAsync(string id)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadAsync("Select * from Neumaticos where ID = @ID", new NeumaticosModel { ID = id });
            });
        }
        public static async Task insertarFotoAsync(FotoNeumaticoModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"
            Insert into FotosNeumaticos (ID, IDMarca, IDModelo, URL)
            Values (@ID, @IDMarca, @IDModelo, @URL)", modelo);

            });
        }
        public static async Task<List<FotoModel>> getListFotosAsync(string idneumatico)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync(@"
            Select Fotos.ID, Fotos.URL, Fotos.Nombre from Fotos inner join NeumaticoFoto on Fotos.ID = NeumaticoFoto.IDFoto where NeumaticoFoto.IDNeumatico = @ID 
                ", new FotoModel {ID = idneumatico });
            });
            
        }
        public static async Task<List<FotoModel>> getListFotosAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoModel>(@"
            Select Fotos.ID, Fotos.URL, Fotos.Nombre 
            from Fotos 
            inner join CategoriaFoto on Fotos.IDCategoria = CategoriaFoto.ID
            inner join Secciones on Fotos.IDSeccion = Secciones.ID        
            where CategoriaFoto.Categoria = 'NEUMÁTICOS' and Secciones.Seccion = 'NEUMÁTICOS'");
            });

        }
        public static async Task<List<NeumaticosModel>> getListNeumaticosAsync()
        {

            return await Task.Run(async () => {
                var neumaticos =  await AccesoBDAutomotriz.LoadDataAsync<NeumaticosModel>(@"
                Select Neumaticos.ID, MarcaProducto.Marca, Anchos.Ancho, IDAncho, Modelos.Nombre Modelo, Inventario.PVENTA Precio, Perfiles.Perfil, IDPerfil, Aros.Aro, IDAro, Modelos.Nombre, IDModelo, Neumaticos.Descripcion, Inventario.STOCK,
            VMax, Carga, Origen, PAire, IndiceV, FotoLogo.URL URLLogo
            from Neumaticos 
            inner join MarcaProducto on Neumaticos.IDMarca = MarcaProducto.ID
            inner join Anchos on Neumaticos.IDAncho = Anchos.ID 
            inner join Perfiles on Neumaticos.IDPerfil = Perfiles.ID
            inner join Aros on Neumaticos.IDAro = Aros.ID
            inner join Modelos on Neumaticos.IDModelo = Modelos.ID
            inner join FotoLogo on Neumaticos.IDMarca = FotoLogo.IDMarca
			inner join Inventario on Neumaticos.IDItem = Inventario.ID
                ");
                foreach (var item in neumaticos)
                {
                    var fotos = await getListFotosAsync(item.ID);
                    item.Fotos = fotos;
                }
                return neumaticos;
            });
        }
        public static async Task<List<SelectListItem>> getAnchosAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select ID Value, Ancho Text from Anchos");
            });
            
        }
        public static async Task<List<SelectListItem>> getArosAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select ID Value, Aro Text from Aros");
            });

        }
        public static async Task<List<SelectListItem>> getMarcasNeumaticosAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select MarcaProducto.ID Value, MarcaProducto.Marca Text 
                                                                               from MarcaProducto inner join CategoriaMarca on MarcaProducto.IDCategoria = CategoriaMarca.ID where CategoriaMarca.Categoria = 'NEUMÁTICOS' ");
            });

        }
        public static async Task<List<SelectListItem>> getModelosAsync(string idmarca)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select ID Value, 
                Nombre Text from Modelos where IDMarca = @Value", new SelectListItem {Value = idmarca });
            });

        }
        public static async Task<List<SelectListItem>> getPerfilesAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select ID Value, Perfil Text from Perfiles Order by Perfil Asc");
            });

        }
        public static async Task<List<NeumaticoFotoModel>> getFotosNeumaticoAsync(string idneumatico)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select ID, IDNeumatico, IDFoto from NeumaticoFoto where IDNeumatico = @ID", new NeumaticoFotoModel { ID = idneumatico });
            });

        }  
    }
}
