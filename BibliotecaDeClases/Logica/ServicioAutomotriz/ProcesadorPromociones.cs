using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BibliotecaDeClases.Logica.AccesoBlobs.ProcesadorFotoWeb;
using static BibliotecaDeClases.Logica.ServicioAutomotriz.ProcesadorFotos;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorPromociones
    {
        public static async Task nuevaPromocionAsync(PromocionModel modelo)
        {
            if (modelo.Foto != null)
            {
                var id = Guid.NewGuid().ToString();
                var uri = await subirNuevaFoto(new FotoModel { ID = id, FechaSubida = DateTime.Now, Foto = modelo.Foto });
                await nuevaFotoAsync(new FotoModel { ID = id, URL = uri, Nombre = modelo.Titulo });
                modelo.IDFoto = id;
            }
            
            modelo.Vigente = true;
            await AccesoBDAutomotriz.Comando("Insert into Promociones (Titulo, Descripcion, IDFoto, Precio, Vigente) Values (@Titulo, @Descripcion, @IDFoto, @Precio, @Vigente) ", modelo);

        }
        public static async Task<List<PromocionModel>> getPromociones()
        {

            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<PromocionModel>(@"Select Promociones.ID, Titulo, Promociones.Descripcion, Promociones.Precio, Promociones.Vigente, Fotos.URL 
                                                  from Promociones inner join Fotos on Promociones.IDFoto = Fotos.ID");
            });
        }
    }
}
