using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BibliotecaDeClases.Logica.AccesoBlobs.ProcesadorFotoWeb;
using static BibliotecaDeClases.Logica.AccesoBlobs.AccesoBlobs;

namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
  public class ProcesadorFotos
    {
        public static async Task nuevaFotoAsync(FotoModel modelo)
        {

            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando("Insert into Fotos (ID, URL, FechaSubida, Nombre, IDCategoria, IDSeccion, Descripcion) Values (@ID, @URL, GETDATE(), @Nombre, @IDCategoria, @IDSeccion, @Descripcion)", modelo);
            });
        }
        public static async Task nuevaFotoSubidaAsync(FotoModel modelo)
        {
            modelo.ID = Guid.NewGuid().ToString();
            var uri = await subirNuevaFoto(modelo);
            modelo.URL = uri;
            await nuevaFotoAsync(modelo);
        }
        public static async Task<List<FotoModel>> getFotosAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoModel>(@"Select Fotos.ID, Fotos.URL, Fotos.Nombre, Fotos.FechaSubida, CategoriaFoto.Categoria, Secciones.Seccion
                                                                           from Fotos 
                                                                           inner join CategoriaFoto on Fotos.IDCategoria = CategoriaFoto.ID
                                                                           inner join Secciones on Fotos.IDSeccion = Secciones.ID
                                                                           Order by Nombre asc");
            });
        }
        public static async Task<List<FotoModel>> getFotosSeccionAsync(string seccion)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select Fotos.ID, Fotos.URL, Fotos.Nombre, Fotos.FechaSubida, CategoriaFoto.Categoria, Secciones.Seccion
                                                                           from Fotos 
                                                                           inner join CategoriaFoto on Fotos.IDCategoria = CategoriaFoto.ID
                                                                           inner join Secciones on Fotos.IDSeccion = Secciones.ID
                                                                           where Secciones.Seccion = @Seccion
                                                                           Order by Nombre asc", new FotoModel {Seccion = seccion });
            });
        }
        public static async Task<List<FotoModel>> getFotosAsync(string idcategoria)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync("Select ID, URL, Nombre, FechaSubida from Fotos where IDCategoria = @IDCategoria Order by Nombre asc", new FotoModel {IDCategoria = idcategoria });
            });
        }
        public static async Task<List<FotoModel>> getFotosAsync(string idcategoria, string seccion)
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync(@"Select ID, URL, Nombre, FechaSubida from Fotos 
                                                              inner join Secciones on Fotos.IDSeccion = Secciones.ID
                                                              where IDCategoria = @IDCategoria and Secciones.Seccion = @Seccion Order by Nombre asc", new FotoModel { IDCategoria = idcategoria, Seccion = seccion });
            });
        }
        public static async Task<bool> checkNombreDisponibleAsync(string nombre)
        {
            return await Task.Run(async () => {
                var data = await AccesoBDAutomotriz.LoadAsync(@"Declare @Cuenta int;
                                                            Select @Cuenta = COUNT(ID) from Fotos where Nombre = @Nombre
                                                            if @Cuenta = 0
                                                            begin
                                                            select 1 NombreDisponible
                                                            end
                                                            else
                                                            begin
                                                            select 0 NombreDisponible
                                                            end", new FotoModel { Nombre = nombre });
                return data.NombreDisponible;
            });
        }
        public static async Task eliminarFotoAsync(string id)
        {
            await EliminarArchivo(id, "fotos");
            await AccesoBDAutomotriz.Comando("Delete from fotos where ID = @ID",new {ID = id });


        }
        public static async Task editorFotoAsync(FotoModel modelo)
        {

            if (modelo.Foto == null)
            {
                await editarFotoAsync(modelo);
            }
            else
            {
                await EliminarArchivo(modelo.ID, "fotos");
                var uri = await subirNuevaFoto(modelo);
                modelo.URL = uri;
                await editarFotoURLAsync(modelo);
            }
        }
        public static async Task editarFotoAsync(FotoModel modelo)
        {
             await Task.Run(async () => {
                 await AccesoBDAutomotriz.Comando("Update Fotos Set Nombre = @Nombre, IDCategoria = @IDCategoria, IDSeccion = @IDSeccion, Descripcion = @Descripcion  where ID = @ID", modelo);
            });

        }
        public static async Task editarFotoURLAsync(FotoModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando("Update Fotos Set Nombre = @Nombre, IDCategoria = @IDCategoria, URL = @URL, IDSeccion = @IDSeccion, Descripcion = @Descripcion where ID = @ID", modelo);
            });

        }
        public static async Task<FotoModel> getFotoAsync(string id)
        {
           return await Task.Run(async () => {
               return await AccesoBDAutomotriz.LoadAsync("Select ID, Nombre, IDCategoria, URL, IDSeccion, Descripcion from Fotos where ID = @ID", new FotoModel {ID = id });
            });

        }


    }
}
