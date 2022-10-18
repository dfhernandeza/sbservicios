using System.Threading.Tasks;
using System;
using BibliotecaDeClases.Modelos.Servicios;
using System.Collections.Generic;
using static BibliotecaDeClases.AccesoBD;
namespace BibliotecaDeClases.Logica.Servicios
{
    public class ProcesadorSolicitudes
    {
        public static async Task<List<SolicitudesModel>> ListaSolicitudes()
        {
            return await Task.Run(async () =>
            {
                return await LoadDataAsync<SolicitudesModel>(@"Select SolicitudItem.ID IDSolicitud, CotizacionMateriales.ID IDCotMaterial, Servicios.NombreServicio, Tareas.Nombre NombreTarea, Entregables.Entregable, CotizacionMateriales.Item, CotizacionMateriales.Cantidad, EstadoSolicitudItem.Estado from CotizacionMateriales 
                                            inner join Entregables on Entregables.IDCotizacion = CotizacionMateriales.IDCot
                                            inner join SolicitudItem on SolicitudItem.IDEntregable = Entregables.ID
                                            inner join EstadoSolicitudItem on EstadoSolicitudItem.ID = SolicitudItem.IDEstado
                                            inner join Tareas on Tareas.ID = entregables.IDTarea
                                            inner join Servicios on servicios.ID = Tareas.IDServicio");
            });
        }


    }
}