using BibliotecaDeClases.Modelos;
using BibliotecaDeClases.Modelos.Servicios;
using BibliotecaDeClases.Modelos.Transacciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BibliotecaDeClases.AccesoBD;
namespace BibliotecaDeClases.Logica.Servicios
{
  public class ProcesadorServicios
    {
        public static async Task<int> CrearServicio(ServicioModel modelo)
        {           
            string sql = @"Insert into Servicios (IDEncargado, NombreServicio, Descripcion, IDCotizacion, IDCliente, IDContacto, IDEstado, FechaInicio, 
                          FechaTermino, NumPedido, IDUbicacion, IDTipoServicio, CreadoPor, CreadoEn, EditadoPor, EditadoEn) output Inserted.ID Values (@IDEncargado, 
                          @NombreServicio, @Descripcion, @IDCotizacion, @IDCliente, @IDCOntacto, 1, @FechaInicio, @FechaTermino, @NumPedido, 
                          @IDUbicacion, @IDTipoServicio, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";
            
             
            var data =  await LoadAsync(sql, modelo);
            await Comando("Update CotizacionesServicios Set IDEstado = '6' where ID = @ID", new { ID = modelo.IDCotizacion });
            return data.ID;
        }
        public static async Task<int> EditarServicioAsync(ServicioModel modelo)
        {

         return await  Comando("Update Servicios set  IDEncargado = @IDEncargado, NombreServicio = @NombreServicio, Descripcion = @Descripcion, IDCotizacion = @IDCotizacion,  IDCliente = @IDCliente, IDCOntacto = @IDCOntacto, Fechainicio = @FechaInicio, " +
                "FechaTermino = @FechaTermino, NumPedido = @NumPedido, IDUbicacion = @IDUbicacion, IDTipoServicio = @IDTipoServicio, EditadoEn = @EditadoEn, EditadoPor = @EditadoPor where ID = @ID",modelo);
        

        }
        public static async Task<int> CambiaEstadoServicioAsync(int idservicio, int idestado)
        {
            string sql = "Update Servicios Set IDEstado = @IDEstado where ID= @ID";

            ServicioModel modelo = new ServicioModel
            {
                ID = idservicio,
                IDEstado = idestado
            };

            return await AccesoBD.Comando(sql, modelo);
        }
        public static async Task<List<ServicioModel>> LoadServiciosAsync()
        {
            string sql = @"Select Servicios.ID, FechaInicio, FechaTermino, NombreServicio, EstadoServicio.Estado Estado, 
                         PersonalServicios.Nombre + PersonalServicios.Apellido Encargado, ClientesServicios.RazonSocial Cliente, 
                         Contactos.Nombre + Contactos.Apellido Contacto, EstadoServicio.Estado 
                         from Servicios inner join EstadoServicio on Servicios.IDEstado = EstadoServicio.ID 
                         inner join PersonalServicios on Servicios.IDEncargado = PersonalServicios.ID 
                         inner join ClientesServicios on Servicios.IDCliente = ClientesServicios.ID 
                         inner join Contactos on Servicios.IDContacto = Contactos.ID ";
            
            return await LoadDataAsync<ServicioModel>(sql);




        }
        public static async Task<ServicioModel> LoadServicio(int id)
        {
          return await Task.Run(() =>
            {
             return AccesoBD.LoadAsync(@"Select Servicios.ID, Servicios.Descripcion, NombreServicio, IDCotizacion, EstadoServicio.Estado Estado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Encargado, 
                                        ClientesServicios.RazonSocial Cliente, Contactos.Nombre + Contactos.Apellido Contacto, EstadoServicio.Estado, FechaInicio, FechaTermino, 
                                        Ubicaciones.Alias Ubicacion, NumPedido from Servicios inner join EstadoServicio on Servicios.IDEstado = EstadoServicio.ID inner join PersonalServicios on Servicios.IDEncargado = PersonalServicios.ID 
                                        inner join ClientesServicios on Servicios.IDCliente = ClientesServicios.ID inner join Contactos on Servicios.IDContacto = Contactos.ID 
                                        inner join Ubicaciones on Servicios.IDUbicacion = Ubicaciones.ID where Servicios.ID = @ID", new ServicioModel { ID = id });
            });
          

         

        }
        public static async Task<ServicioModel> CargarServicioAsync(int id)
        {
            ServicioModel modelo = new ServicioModel
            {
                ID = id
            };
            string sql = @"Select Servicios.ID, NombreServicio, IDCotizacion, IDEncargado, IDCliente, IDContacto, 
            IDEstado, FechaInicio, FechaTermino, IDUbicacion, IDTipoServicio, NumPedido, Descripcion 
            from Servicios where ID = @ID";

            return await LoadAsync(sql, modelo);
        }
        public static async Task<int> BorrarServicioAsync(int id)
        {
            ServicioModel servicio = new ServicioModel
            {
                ID = id
            };
            string sql = "Delete from Servicios where ID = @ID ";

            return await Comando(sql, servicio);
        }
        public static async Task<List<ServicioModel>> FiltroServicioAsync(string por, string param)
        {
            string sql;
            ServicioModel servicio = new ServicioModel();
            if (por == "1")
            {
                sql = "Select *, EstadoServicio.Estado Estado, PersonalServicios.Nombre + PersonalServicios.Apellido Encargado, Cliente.RazonSocial, Contactos.Nombre + Contactos.Apellido Contacto from Servicios inner join Estado on Servicios.IDEstado = EstadoServicio.ID inner join PersonalServicios on Servicios.IDEncargado = PersonalServicios.ID inner join ClientesServicios on Servicios.IDCliente = ClientesServicios.ID inner join Contactos on Servicios.IDContacto = Contactos.ID where Servicios.ID = @ID";
                servicio.ID = int.Parse(param);
            }
            else if (por == "2")
            {
                sql = "Select *, EstadoServicio.Estado Estado, PersonalServicios.Nombre + PersonalServicios.Apellido Encargado, Cliente.RazonSocial, Contactos.Nombre + Contactos.Apellido Contacto from Servicios inner join Estado on Servicios.IDEstado = EstadoServicio.ID inner join PersonalServicios on Servicios.IDEncargado = PersonalServicios.ID inner join ClientesServicios on Servicios.IDCliente = ClientesServicios.ID inner join Contactos on Servicios.IDContacto = Contactos.ID where NombreServicio = @NombreServicio";
                servicio.NombreServicio = param;
            }
            else if (por == "3")
            {
                sql = "Select *, EstadoServicio.Estado Estado, PersonalServicios.Nombre + PersonalServicios.Apellido Encargado, Cliente.RazonSocial, Contactos.Nombre + Contactos.Apellido Contacto from Servicios inner join Estado on Servicios.IDEstado = EstadoServicio.ID inner join PersonalServicios on Servicios.IDEncargado = PersonalServicios.ID inner join ClientesServicios on Servicios.IDCliente = ClientesServicios.ID inner join Contactos on Servicios.IDContacto = Contactos.ID where ClientesServicios.RazonSocial = @Cliente";
                servicio.CLiente = param;
            }

            else 
            {
                sql = "Select *, EstadoServicio.Estado Estado, PersonalServicios.Nombre + PersonalServicios.Apellido Encargado, Cliente.RazonSocial, Contactos.Nombre + Contactos.Apellido Contacto from Servicios inner join Estado on Servicios.IDEstado = EstadoServicio.ID inner join PersonalServicios on Servicios.IDEncargado = PersonalServicios.ID inner join ClientesServicios on Servicios.IDCliente = ClientesServicios.ID inner join Contactos on Servicios.IDContacto = Contactos.ID where FechaInicio = @FechaInicio";
                servicio.FechaInicio = Convert.ToDateTime(param);
            }

            return await LoadDataAsync(sql, servicio);


        }
        public static async Task<int> IDServicioAsync()
        {
            
            string sql = "declare @caso1 int select @caso1 = max(ID) from Servicios if @caso1 is null begin Select 1 ID end else begin Select @caso1 + 1 ID end";

            var data = await LoadDataAsync<ServicioModel>(sql);
           ServicioModel servicio = new ServicioModel
           {
               ID = data.FirstOrDefault().ID
           };

            return servicio.ID;
        }                     
        public static Task<ServicioModel> PresupuestoVsCostos(int idcotizacion)
        {
           return Task.Run(() =>
            {
                string sql = @"Declare @GG decimal; 
                            Declare @PptoMaterial decimal; 
                            declare @PptoPersonal decimal;
                            declare @PptoEquipos decimal; 
                            IF EXISTS(Select IDCotizacion from CotizacionesServicios where IDCotizacionPrincipal = @ID) 
                            begin
                            select @PptoMaterial = sum(dbo.CostoSubCot(ID)) + dbo.CostoCotMaterial(@ID) from CotizacionesServicios where IDCotizacionPrincipal = @ID
                            select @PptoPersonal = dbo.CostoCotPersonal(@ID) 
                            Select @PptoEquipos = dbo.CostoCotEquipos(@ID) 
                            Select @GG = GastosGenerales from CotizacionesServicios where ID = @ID Select @PptoMaterial PresupuestoMateriales, dbo.GastoMaterialTotal(Servicios.ID)  CostoMaterial, 
                            @PptoPersonal PresupuestoPersonal, dbo.GastoPersonalTotal(Servicios.ID) CostoPersonal ,@PptoEquipos PresupuestoEquiposServicios, dbo.GastoEquiposServiciosTotal(Servicios.ID) CostoEquiposServicios , 
                            (@PptoMaterial + @PptoEquipos + @PptoPersonal) * @GG / 100 GastosGenerales, DBO.GastosGeneralesTotal(Servicios.ID) CostoGastosGenerales 
                             from CotizacionesServicios inner join Servicios on CotizacionesServicios.ID = Servicios.IDCotizacion where CotizacionesServicios.ID = @ID group by GastosGenerales, Servicios.ID
                             end
                             else
                             begin
                             select @pptomaterial = dbo.CostoCotPrincipal(@ID) + dbo.CostoCotMaterial(@ID);
                             select @pptopersonal = dbo.CostoCotPersonal(@ID);
                             select @pptoequipos = dbo.CostoCotEquipos(@ID)
                             declare @GGtotales decimal;
                             Select @GG = GastosGenerales from CotizacionesServicios where ID = @ID; 
                             Select @pptomaterial as PresupuestoMateriales, dbo.GastoMaterialTotal(Servicios.ID)  CostoMaterial, @pptopersonal PresupuestoPersonal, 
                             dbo.GastoPersonalTotal(Servicios.ID) CostoPersonal, @pptoequipos PresupuestoEquiposServicios, 
                             dbo.GastoEquiposServiciosTotal(Servicios.ID) CostoEquiposServicios ,  
                             (@PptoMaterial + @PptoEquipos + @PptoPersonal) * @GG / 100 GastosGenerales, dbo.GastosGeneralesTotal(Servicios.ID) CostoGastosGenerales  
                             from CotizacionesServicios inner join Servicios on CotizacionesServicios.ID = Servicios.IDCotizacion where CotizacionesServicios.ID = @ID
                            end
                            ";  
                ServicioModel modelo = new ServicioModel
                {
                    ID = idcotizacion
                };

                return LoadAsync(sql, modelo);
            });
          

        }
        public static async Task<ServicioModel> PresupuestoAsync(int id)
        {
           return await Task.Run(async () => {
               return await AccesoBD.LoadAsync(@"Declare @GG decimal; Select @GG = GastosGenerales from CotizacionesServicios where ID = @ID 
               Select sum(dbo.CostoSubCot(ID)) + dbo.CostoCotMaterial(@ID) PresupuestoMateriales, 
               dbo.CostoCotPersonal(@ID) PresupuestoPersonal, dbo.CostoCotEquipos(@ID) PresupuestoEquiposServicios, @GG GastosGenerales 
               from CotizacionesServicios  where IDCotizacionPrincipal = @ID group by GastosGenerales", new ServicioModel {ID = id});
           });
          
        }
        public static async Task<ServicioModel> PresupuestoServicioAsync(int id)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync(@"Declare @GG decimal(18,3); 
               
                Select @GG = GastosGenerales from CotizacionesServicios where ID = @ID 
                
                IF EXISTS(Select IDCotizacion from CotizacionesServicios where IDCotizacionPrincipal = @ID) 
                begin
                Select sum(dbo.CostoSubCot(ID)) + dbo.CostoCotMaterial(@ID) PresupuestoMateriales, 
                               dbo.CostoCotPersonal(@ID) PresupuestoPersonal, dbo.CostoCotEquipos(@ID) PresupuestoEquiposServicios, @GG GastosGenerales 
                               from CotizacionesServicios  where IDCotizacionPrincipal = @ID group by GastosGenerales
                end
                else
                begin

                declare @pptomaterial decimal;
                select @pptomaterial = dbo.CostoCotPrincipal(@ID) + dbo.CostoCotMaterial(@ID);
                declare @pptopersonal decimal;
                select @pptopersonal = dbo.CostoCotPersonal(@ID);
                declare @pptoequipos decimal;
                select @pptoequipos = dbo.CostoCotEquipos(@ID)
                declare @GGtotales decimal;
                Select @GG = GastosGenerales from CotizacionesServicios where ID = @ID 
                
                Select @pptomaterial as PresupuestoMateriales, 
               @pptopersonal PresupuestoPersonal, @pptoequipos PresupuestoEquiposServicios, @GG  GastosGenerales
			   end ", new ServicioModel { ID = id });
            });

        }
        public static async Task<List<ResumenDiaModel>> getResumenDiaServicioAsync(int idservicio)
        {

            return await Task.Run(async () =>
            {

                return await AccesoBD.LoadDataAsync(@"DECLARE @start_date DATETIME;
                DECLARE @end_date DATETIME ;
                            
                            Select @start_date = Servicios.FechaInicio from Servicios where Servicios.ID = @ID;
                            Select @end_date = Servicios.FechaTermino from Servicios where Servicios.ID = @ID;
                            WITH AllDays
                          AS(SELECT   @start_date AS[Date], 1 AS[level]
                               UNION ALL
                               SELECT   DATEADD(DAY, 1, [Date]), [level] + 1
                               FROM     AllDays
                               WHERE[Date] < @end_date)
                     SELECT[Date] Fecha, [dbo].[HorasDiaServicio](@ID, [Date]) HorasTotales, [dbo].[ValorHorasDiaServicio](@ID, [Date]) ValorHH, 
                [dbo].[ValorColacionesDiaServicio](@ID, [Date]) CostoColaciones, [dbo].[ContarColacionesDiaServicio](@ID, [Date]) CantidadColaciones,
                [dbo].[ValorPeajesDiaServicio](@ID, [Date]) CostoPeajes, [dbo].[ValorCombustibleDiaServicio](@ID, [Date]) CostoCombustible,
                [dbo].[ValorMaterialesDiaServicio](@ID, [Date]) CostoMateriales
                    FROM   AllDays OPTION(MAXRECURSION 0)", new ResumenDiaModel { ID = idservicio });

            });
        }

        public static async Task<List<TimeSheetModel>> getCostoHHDiaEmpleadoAsync(int idservicio)
        {
            return await Task.Run(async () => 
            {
                return await AccesoBD.LoadDataAsync(@"Select PersonalServicios.Nombre +' '+ PersonalServicios.Apellido Empleado,
                Case 
                when DATEDIFF(hour,TimeSheet.Entrada,TimeSheet.Salida) > 4 Then  DATEDIFF(hour,TimeSheet.Entrada,TimeSheet.Salida) - 0.5
                else DATEDIFF(hour,TimeSheet.Entrada,TimeSheet.Salida)
                end as HorasTotales,
                Case
                when DATEDIFF(hour,TimeSheet.Entrada,TimeSheet.Salida) > 4 Then  (DATEDIFF(hour,TimeSheet.Entrada,TimeSheet.Salida) - 0.5) * dbo.ValorHHMes(Documentos.FechaDocumento, TimeSheet.IDEmpleado)
                else DATEDIFF(hour,TimeSheet.Entrada,TimeSheet.Salida)* dbo.ValorHHMes(Documentos.FechaDocumento, TimeSheet.IDEmpleado)
                end as ValorHH,
                Documentos.FechaDocumento Fecha
                from TimeSheet
                inner join PersonalServicios on PersonalServicios.ID = TimeSheet.IDEmpleado
                inner join Documentos on Documentos.ID = TimeSheet.IDDocumento
                inner join CECOs on CECOs.ID = TimeSheet.IDCECO
                inner join Tareas on Tareas.ID = CECOs.IDTarea
                where Tareas.IDServicio = @ID", new TimeSheetModel { ID = idservicio });  });
            
        }
    }
}
