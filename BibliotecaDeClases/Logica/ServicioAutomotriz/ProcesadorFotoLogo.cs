using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using static BibliotecaDeClases.Logica.AccesoBlobs.ProcesadorImagenNeumatico;

namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorFotoLogo
    {
        public static async Task nuevaFotoLogoAsync(FotoLogoModel modelo)
        {
            var id = Guid.NewGuid().ToString();
            modelo.ID = id;
            var url = await SubirFotoLogo(modelo);
            modelo.URL = url;
            await insertarNuevoLogoAsync(modelo);
        }
        public static async Task insertarNuevoLogoAsync(FotoLogoModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando("Insert into FotoLogo (ID, IDMarca, URL) Values(@ID, @IDMarca, @URL)", modelo);
            });
            
        }
        public static async Task<List<FotoLogoModel>> getFotoLogoListAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoLogoModel>(@"Select FotoLogo.ID, FotoLogo.IDMarca, FotoLogo.URL, MarcaProducto.Marca  
                 from FotoLogo inner join MarcaProducto on FotoLogo.IDMarca = MarcaProducto.ID");
            });

        }
        public static async Task<List<SelectListItem>> getMarcasAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBDAutomotriz.LoadDataAsync<SelectListItem>(@"Select MarcaProducto.ID Value, MarcaProducto.Marca Text 
                                                                               from MarcaProducto");
            });

        }

    }
}
