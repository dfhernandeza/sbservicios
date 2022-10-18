using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases.Modelos.Clientes;
using BibliotecaDeClases.Modelos.Cotizaciones;

namespace BibliotecaDeClases.Logica.Cotizaciones
{
   public class ProcesadorCotizacionesArauco
    {
        public static async Task<CotizacionAraucoModel> NuevaCotizacion(CotizacionAraucoModel modelo)
        {
            return await Task.Run(() =>
            {
                return AccesoBD.LoadAsync("Insert into CotizacionesServicios (SolicitudPedido,NLicitacion,IDSupervisor,Fecha,ValidezOferta,IDCotizacion,TiempoEjecucion,Detalles,Utilidad,GastosGenerales,IDUbicacion,Notas,Comentarios,CreadoPor,CreadoEn, " +
               "EditadoPor,EditadoEn, IDEstado, IDTipoCotizacion, IDCliente, DATOE) output inserted.ID Values(@SolicitudPedido,@NLicitacion,@IDSupervisor,@Fecha,@ValidezOferta,@IDCotizacion,@TiempoEjecucion,@Detalles,@Utilidad,@GastosGenerales,@IDUbicacion," +
               "@Notas,@Comentarios,@CreadoPor,@CreadoEn,@EditadoPor,@EditadoEn,@IDEstado, @IDTipoCotizacion,1, @DATOE)",modelo);
            });
        }
   
        public static async Task<int> EditaCotizacionAsync(CotizacionAraucoModel modelo)
        {
            return await AccesoBD.Comando("Update CotizacionesServicios Set SolicitudPedido = @SolicitudPedido, NLicitacion = @NLicitacion, IDSupervisor = @IDSupervisor, Fecha = @Fecha," +
                " ValidezOferta = @ValidezOferta, IDCotizacion = @IDCotizacion, TiempoEjecucion = @TiempoEjecucion, Detalles = @Detalles," +
                " Utilidad = @Utilidad, GastosGenerales = @GastosGenerales, Notas = @Notas, Comentarios = @Comentarios, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn, DATOE = @DATOE, NRFPAriba = @NRFPAriba where ID = @ID", modelo);
        }
    
        public static async Task<int> EliminaCotizacionAsync(int id)
        {
            return await AccesoBD.Comando("Delete from CotizacionesServicios where IDCotizacionPrincipal = @ID Delete from CotizacionesServicios where ID = @ID",new CotizacionAraucoModel{ID = id});
        }

        public static async Task<int> EliminaSubCotiAsync(int id)
        {
            return await AccesoBD.Comando("Delete from CotizacionesServicios where ID = @ID", new { ID = id });
        }
        public static async Task<List<CotizacionAraucoModel>> CargarCotizacionesAraucoAsync()
        {
         
            
                string sql = "Select CotizacionesServicios.ID, IDCotizacion, Fecha, Servicio, dbo.CostoCot(CotizacionesServicios.ID) Total, Contactos.Nombre + ' ' + Contactos.Apellido Supervisor," +
                    "ClientesServicios.RazonSocial Cliente, EstadoCotizaciones.Estado Estado from dbo.CotizacionesServicios inner join ClientesServicios " +
                    "on CotizacionesServicios.IDCliente = ClientesServicios.ID inner " +
                    "join Contactos on CotizacionesServicios.IDSupervisor = Contactos.ID inner Join EstadoCotizaciones on CotizacionesServicios.IDEstado = EstadoCotizaciones.ID";

                return await AccesoBD.LoadDataAsync<CotizacionAraucoModel>(sql);

            
        }
           
        
        public static async Task<CotizacionAraucoModel> CargarCotizacionAraucoAsync(int id)
        {
            CotizacionAraucoModel data = new CotizacionAraucoModel
            {
                ID = id


            };
            string sql = " Select CotizacionesServicios.ID, IDCotizacion, Fecha, Servicio, dbo.CostoCotMaterial(CotizacionesServicios.ID) TotalMateriales, dbo.CostoCotPersonal(CotizacionesServicios.ID) TotalPersonal," +
                " dbo.CostoCotEquipos(CotizacionesServicios.ID) TotalEquipos, dbo.CostoCotSeguridad(CotizacionesServicios.ID) TotalSeguridad, dbo.CostoSubCot(CotizacionesServicios.ID) TotalSubCotizaciones," +
                " CotizacionesServicios.IDCliente IDCliente, ClientesServicios.RazonSocial Cliente, Contactos.Nombre + ' ' + Contactos.Apellido Supervisor," +
                " Detalles, TiempoEjecucion, SolicitudPedido, IDEstado Estado, DATOE from CotizacionesServicios" +
                " inner join ClientesServicios on CotizacionesServicios.IDCliente = ClientesServicios.ID" +
                " inner Join Contactos on CotizacionesServicios.IDSupervisor = Contactos.ID where CotizacionesServicios.ID = @ID";

            return await AccesoBD.LoadAsync(sql, data);
        }

        //public static async Task<CotizacionAraucoModel> CargarCotizacionAraucoSimpleAsync(int id)
        //{

        //    CotizacionAraucoModel data = new CotizacionAraucoModel
        //    {
        //        ID = id


        //    };
        //    string sql = "Select SolicitudPedido,NLicitacion,Contactos.Nombre + ' ' + Contactos.Apellido IngenieroContratos, IDSupervisor, Fecha, ValidezOferta, IDCotizacion,TiempoEjecucion, Detalles, CotizacionesServicios.Notas," +
        //        " CotizacionesServicios.ID, CotizacionesServicios.Utilidad, CotizacionesServicios.GastosGenerales, CotizacionesServicios.IDEstado  from CotizacionesServicios inner Join Contactos on CotizacionesServicios.IDSupervisor = Contactos.ID  where CotizacionesServicios.ID = @ID";

        //    return await AccesoBD.LoadAsync(sql, data);
        //}

        public static Task<CotizacionAraucoFoxExcel> CargarCotizacionAraucoSimpleAsync(int id)

        {
            return  Task.Run(() =>
            {
                CotizacionAraucoFoxExcel data = new CotizacionAraucoFoxExcel
                {
                    ID = id


                };
              string sql = "Select SolicitudPedido,NLicitacion,Contactos.Nombre + ' ' + Contactos.Apellido IngenieroContratos, IDSupervisor, Fecha, ValidezOferta, IDCotizacion,TiempoEjecucion, Detalles, CotizacionesServicios.Notas," +
            " CotizacionesServicios.ID, Utilidad, GastosGenerales, DATOE, CotizacionesServicios.IDEstado, NRFPAriba  from CotizacionesServicios inner Join Contactos on CotizacionesServicios.IDSupervisor = Contactos.ID  where CotizacionesServicios.ID = @ID";

            return  AccesoBD.LoadAsync(sql, data);
            });
        }

        public static async Task<SubCotizacionModel> CargaSubCotizacionAraucoAsync(int id)
        {
            return await AccesoBD.LoadAsync("Select Sub.ID, dbo.CostoCot(Sub.ID) Total, dbo.CostoCotMaterial(Sub.ID) TotalMateriales, dbo.CostoCotPersonal(Sub.ID) TotalPersonal, dbo.CostoCotSeguridad(Sub.ID) TotalSeguridad, " +
                "dbo.CostoCotEquipos(Sub.ID) TotalEquipos, Principal.Detalles CotizacionPrincipal, Sub.Cantidad, Sub.Detalles, Sub.Unidad, Sub.Servicio Producto, Sub.Utilidad, Sub.IDCotizacionPrincipal from CotizacionesServicios as Sub " +
                "inner join CotizacionesServicios as Principal on Sub.IDCotizacionPrincipal = Principal.ID where Sub.ID = @ID", new SubCotizacionModel { ID = id });
        }

        public static async Task<List<CotizacionAraucoModel>> CargarCotizacionesAsync()
        {
            return await AccesoBD.LoadDataAsync<CotizacionAraucoModel>(@"Select CotizacionesServicios.ID, IDCotizacion, Fecha, Detalles, 
            dbo.CostoCot(CotizacionesServicios.ID) TotalOferta, Contactos.Nombre + ' ' + Contactos.Apellido IngenieroContratos, 
            ClientesServicios.RazonSocial Cliente, EstadoCotizaciones.Estado Estado from dbo.CotizacionesServicios inner join ClientesServicios 
            on CotizacionesServicios.IDCliente = ClientesServicios.ID inner join Contactos on CotizacionesServicios.IDSupervisor = Contactos.ID 
            inner Join EstadoCotizaciones on CotizacionesServicios.IDEstado = EstadoCotizaciones.ID where ClientesServicios.Arauco = 'true'");
        }

        public static async Task<List<SelectListItem>> UbicacionesAraucoAsync()
        {
            var data = await AccesoBD.LoadDataAsync<UbicacionesModel>("Select Ubicaciones.ID , ClientesServicios.ID IDCliente, Ubicaciones.Alias FROM Ubicaciones " +
                "inner join ClientesServicios on Ubicaciones.IDCliente = ClientesServicios.ID where ClientesServicios.Arauco = 'true'");

            List<SelectListItem> lista = new List<SelectListItem>();
            foreach(var row in data) { lista.Add(new SelectListItem { Value = row.ID.ToString(), Text = row.Alias }); }
            return lista;

        }


    }
}
