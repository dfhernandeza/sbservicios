using BibliotecaDeClases.Modelos;
using BibliotecaDeClases.Modelos.Cotizaciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica
{
    public class ProcesadorCotizaciones
    {
       public static CotizacionModel CreaCotizacion(string idcotizacion, string servicio, DateTime fecha, int idcliente, int idsupervisor, string detalles, double tiempoejecucion, string sp, 
           string nolicitacion, decimal tasa, int idtipocotizacion, DateTime validezoferta, string creadopor, bool arauco)
        {
            CotizacionModel Cotizacion = new CotizacionModel
            {
                IDCotizacion = idcotizacion,
                Servicio = servicio,
                Fecha = fecha,
                IDCliente = idcliente,
                IDSupervisor = idsupervisor,
                Detalles = detalles,
                TiempoEjecucion = tiempoejecucion,
                IDEstado = 1,
                SolicitudPedido = sp,
                NLicitacion = nolicitacion,                
                Tasa= tasa,
                IDTipoCotizacion = idtipocotizacion,
                ValidezOferta = validezoferta,
                CreadoPor = creadopor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor,
                
                

            };
            string sql;
            if (arauco == true)
            {
                  sql = @"Insert into dbo.CotizacionesServicios (IDCotizacion, Servicio, Fecha, IDCliente, IDSupervisor, Detalles, TiempoEjecucion, IDEstado, 
                        SolicitudPedido, NLicitacion, Tasa, IDTipoCotizacion, ValidezOferta, CreadoPor, CreadoEn, EditadoEn, EditadoPor) 
                        output inserted.ID
                        Values (@IDCotizacion, @Servicio, @Fecha, @IDCliente, @IDSupervisor, @Detalles, @TiempoEjecucion, @IDEstado, @SolicitudPedido, 
                        @NLicitacion, @Tasa, @IDTipoCotizacion, @ValidezOferta, @CreadoPor, @CreadoEn, @EditadoEn, @EditadoPor)";
            }
            else
            {
                 sql = @"Insert into dbo.CotizacionesServicios (IDCotizacion, Servicio, Fecha, IDCliente, IDSupervisor, Detalles, TiempoEjecucion, IDEstado, SolicitudPedido, 
                        CreadoPor, CreadoEn, EditadoEn, EditadoPor) 
                        output inserted.ID
                        Values (@IDCotizacion, @Servicio, @Fecha, @IDCliente, @IDSupervisor, @Detalles, @TiempoEjecucion, @IDEstado, @SolicitudPedido, 
                                @CreadoPor, @CreadoEn, @EditadoEn, @EditadoPor )";
          
            }

         
            
            return AccesoBD.ReturnID(sql, Cotizacion);

        }
        public static async Task<CotizacionModel> LoadCotizacionAsync(int id)
        {
            CotizacionModel data = new CotizacionModel
            {
                ID = id
              
                
            };
            string sql = @"Select CotizacionesServicios.ID, IDCotizacion, Fecha, Servicio, dbo.CostoCotMaterial(CotizacionesServicios.ID) TotalMateriales, dbo.CostoCotPersonal(CotizacionesServicios.ID) TotalPersonal,
                dbo.CostoCotEquipos(CotizacionesServicios.ID) TotalEquipos, dbo.CostoCotSeguridad(CotizacionesServicios.ID) TotalSeguridad, dbo.CostoCotPrincipal(CotizacionesServicios.ID) TotalSubCotizaciones,
                CotizacionesServicios.IDCliente IDCliente, ClientesServicios.RazonSocial Cliente, Contactos.Nombre + ' ' + Contactos.Apellido Supervisor, 
                Detalles, TiempoEjecucion, SolicitudPedido, IDEstado Estado, GastosGenerales from CotizacionesServicios  
                inner join ClientesServicios on CotizacionesServicios.IDCliente = ClientesServicios.ID 
                inner Join Contactos on CotizacionesServicios.IDSupervisor = Contactos.ID 
                where CotizacionesServicios.ID = @ID";

            return await AccesoBD.LoadAsync(sql, data);

        }
        public static async Task<List<SelectListItem>> LoadEstadosAsync()
        {
            string sql = "Select ID, Estado from EstadoCotizaciones";
            var coti = await AccesoBD.LoadDataAsync<EstadoCotizacionModel>(sql);
            List<SelectListItem> Lista = new List<SelectListItem>();
            foreach (var row in coti)
            {
                SelectListItem Cotizacion = new SelectListItem
                {
                    Text = row.Estado,
                    Value = row.ID.ToString()
                };
                Lista.Add(Cotizacion);

            }
            return Lista;
        }
        public static async Task<CotizacionModel> CargarCotizacionAsync(int id)
        {
            CotizacionModel data = new CotizacionModel
            {
                ID = id
            };
            string sql = "Select * from CotizacionesServicios where ID = @ID";

            return await AccesoBD.LoadAsync(sql, data);

        } 
        public static async Task<List<CotizacionModel>> LoadCotizacionesAsync()
        {
            string sql = @"Select CotizacionesServicios.ID, IDCotizacion, Fecha, Servicio, dbo.CostoCot(CotizacionesServicios.ID) Total, 
            Contactos.Nombre + ' ' + Contactos.Apellido Supervisor, ClientesServicios.RazonSocial Cliente, EstadoCotizaciones.Estado Estado 
            from dbo.CotizacionesServicios inner join ClientesServicios on CotizacionesServicios.IDCliente = ClientesServicios.ID inner 
            join Contactos on CotizacionesServicios.IDSupervisor = Contactos.ID inner Join EstadoCotizaciones 
            on CotizacionesServicios.IDEstado = EstadoCotizaciones.ID where ClientesServicios.Arauco = 'false'";

            return await AccesoBD.LoadDataAsync<CotizacionModel>(sql);

        }
        public static async Task<int> EditacotizacionAsync(int id, string idcotizacion, string servicio, DateTime fecha, int idcliente, int idsupervisor, string detalles, double tiempoejecucion, string editadopor)
        {
            CotizacionModel Cotizacion = new CotizacionModel
            {
                ID = id,
                IDCotizacion = idcotizacion,
                Servicio = servicio,
                Fecha = fecha,
                IDCliente = idcliente,
                IDSupervisor = idsupervisor,
                Detalles = detalles,
                TiempoEjecucion = tiempoejecucion,
                EditadoPor = editadopor,
                EditadoEn = DateTime.Now
            };

            string sql = "Update dbo.Cotizaciones set IDCotizacion = @IDCotizacion, servicio = @servicio, fecha = @fecha, IDCliente = @IDCliente, IDSupervisor = @IDSupervisor, detalles = @detalles, tiempoejecucion = @tiempoejecucion where ID = @ID";
            return await AccesoBD.Comando(sql, Cotizacion);

        }
        public static async Task<List<SelectListItem>> ListaCotizacionesAsync()
        {
            string sql = @"Select CotizacionesServicios.ID, CotizacionesServicios.Detalles from CotizacionesServicios inner Join 
            EstadoCotizaciones on CotizacionesServicios.IDEstado = EstadoCotizaciones.ID 
            inner join ClientesServicios on CotizacionesServicios.IDCliente = ClientesServicios.ID
            where ClientesServicios.Arauco = 'true'and EstadoCotizaciones.Estado = 'Aprobada'";
            var coti = await AccesoBD.LoadDataAsync<CotizacionModel>(sql);
            List<SelectListItem> Lista = new List<SelectListItem>();
            foreach(var row in coti)
            {
                SelectListItem Cotizacion = new SelectListItem
                {
                    Text = row.Detalles,
                    Value = row.ID.ToString()
                };
                Lista.Add(Cotizacion);
            
            }
            return Lista;
        }
        public static async Task<List<SelectListItem>> ListaCotizacionesOtrosClientesAsync()
        {
            string sql = @"Select CotizacionesServicios.ID, CotizacionesServicios.Detalles from CotizacionesServicios inner Join 
            EstadoCotizaciones on CotizacionesServicios.IDEstado = EstadoCotizaciones.ID 
            inner join ClientesServicios on CotizacionesServicios.IDCliente = ClientesServicios.ID
            where ClientesServicios.Arauco = 'false'and EstadoCotizaciones.Estado = 'Aprobada'";
            var coti = await AccesoBD.LoadDataAsync<CotizacionModel>(sql);
            List<SelectListItem> Lista = new List<SelectListItem>();
            foreach (var row in coti)
            {
                SelectListItem Cotizacion = new SelectListItem
                {
                    Text = row.Detalles,
                    Value = row.ID.ToString()
                };
                Lista.Add(Cotizacion);

            }
            return Lista;
        }
        public static async Task<int> CambiarEstadoCotizacionAsync(int idcotizacion ,int idestado)
        {
            CotizacionModel modelo = new CotizacionModel
            {
                ID = idcotizacion,
                IDEstado = idestado
            };
            string sql = "Update CotizacionesServicios Set IDEstado = @IDEstado where ID = @ID";
            return await AccesoBD.Comando(sql, modelo);
        
        }
        public static async Task<int> CambiaEstadoCotiAsync(int id, int idestado)
        {
            string sql = "Update CotizacionesServicios Set IDEstado = @IDEstado where ID = @ID";
            CotizacionModel modelo = new CotizacionModel
            {
                ID = id,
                IDEstado = idestado
            };

            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<List<CotizacionModel>> CotizacionesPorClienteAsync(int idcliente)
        {
            string sql = "Select * from CotizacionesServicios where IDCliente = @IDCliente";
            CotizacionModel modelo = new CotizacionModel
            {
                IDCliente = idcliente
            };

            return await AccesoBD.LoadDataAsync(sql, modelo);
        }

        public static async Task<List<SubCotizacionModel>> ListaProductoFabricacionPropiaAsync(int cotizacionprincipal)
        {
            return await AccesoBD.LoadDataAsync("Select ID, Servicio, Unidad, Cantidad, dbo.CostoSubCot(CotizacionesServicios.IDCotizacionPrincipal) Total, Utilidad from CotizacionesServicios where CotizacionesServicios.IDCotizacionPrincipal = @IDCotizacionPrincipal ", new SubCotizacionModel { IDCotizacionPrincipal = cotizacionprincipal });
        }
        public static SubCotizacionModel CreaCotizacionProductoPropio(string producto, string detalles, int idcotprincipal, string creadopor, string unidad, double cantidad)
        {
            return AccesoBD.ReturnID("Insert into CotizacionesServicios (Servicio, Detalles, IDCotizacionPrincipal, Unidad, Cantidad, IDEstado, Fecha, IDTipoCotizacion, CreadoPor, CreadoEn, EditadoPor, EditadoEn) output inserted.ID Values (@Servicio, @Detalles, @IDCotizacionPrincipal, @Unidad, @Cantidad, @IDEstado, @Fecha, @IDTipoCotizacion, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)",
                new SubCotizacionModel
                {
                    Servicio = producto,
                    Detalles = detalles,
                    IDCotizacionPrincipal = idcotprincipal,
                    Unidad = unidad,
                    Cantidad = cantidad,
                    IDEstado = 1,
                    Fecha = DateTime.Today,
                    IDTipoCotizacion = 2,
                    CreadoPor = creadopor,
                    CreadoEn = DateTime.Now,
                    EditadoPor = creadopor,
                    EditadoEn = DateTime.Now
                });
        }

        public static async Task<int> EditaCotizacionProductoPropioAsync(int id, string producto, string detalles, string unidad, double cantidad,  string editadopor)
        {

            return await AccesoBD.Comando("Update CotizacionesServicios Set Servicio = @Servicio , Detalles = @Detalles, Unidad = @Unidad, Cantidad = @Cantidad, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID",
                new SubCotizacionModel {ID = id, Servicio = producto, Detalles = detalles, Unidad = unidad, Cantidad = cantidad, EditadoPor = editadopor, EditadoEn = DateTime.Now });


        }

        public static async Task<SubCotizacionModel> CargaSubCotizacionAsync(int id)
        {
           return await AccesoBD.LoadAsync("Select Sub.ID, dbo.CostoCot(Sub.ID) Total, dbo.CostoCotMaterial(Sub.ID) TotalMateriales, dbo.CostoCotPersonal(Sub.ID) TotalPersonal, dbo.CostoCotSeguridad(Sub.ID) TotalSeguridad, " +
               "dbo.CostoCotEquipos(Sub.ID) TotalEquipos, Principal.Servicio, Sub.Cantidad, Sub.Detalles, Sub.Unidad, Sub.Servicio Producto, Sub.Utilidad, Sub.IDCotizacionPrincipal from CotizacionesServicios as Sub " +
               "inner join CotizacionesServicios as Principal on Sub.IDCotizacionPrincipal = Principal.ID where Sub.ID = @ID", new SubCotizacionModel { ID = id });
        }




        public static async Task<SubCotizacionModel> LoadSubCotizacionAsync(int id)
        {
            return await AccesoBD.LoadAsync("Select Sub.ID, Principal.Servicio CotizacionPrincipal, Sub.Servicio, Sub.Detalles, Sub.Unidad, Sub.Cantidad, Principal.ID IDCotizacionPrincipal from CotizacionesServicios as Sub " +
                "inner join CotizacionesServicios as Principal on Sub.IDCotizacionPrincipal = Principal.ID where Sub.ID = @ID", new SubCotizacionModel { ID = id });
        }

        public static async Task<int> EliminaCotizacionAsync(int id)
        {
            return await AccesoBD.Comando("Delete from CotizacionesServicios where IDCotizacionPrincipal = @ID " +
                "Delete from CotizacionesServicios where ID = @ID", new { ID = id });
        }

       public static async Task<decimal> getUtilidad(int id){
           return await Task.Run(async ()=>{
                var data = await AccesoBD.LoadAsync("Select Utilidad from CotizacionesServicios where ID = @ID", new CotizacionModel {ID = id });
                return data.Utilidad;
           });
       }

        public static async Task<int> CambiaUtilidadAsync(int id, decimal utilidad)
        {
            return await AccesoBD.Comando("Update CotizacionesServicios Set Utilidad = @Utilidad where ID = @ID",new {ID = id, Utilidad = utilidad});
        }
        public static async Task<int> CambiaGGAsync(int id, double gg)
        {
            return await AccesoBD.Comando("Update CotizacionesServicios Set GastosGenerales = @GastosGenerales where ID = @ID", new { ID = id, GastosGenerales = gg });
        }

    }
}
