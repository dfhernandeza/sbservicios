using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorTareas;
using static BibliotecaDeClases.Logica.Servicios.ProcesadorServicios;
using BibliotecaDeClases.Modelos.Servicios;

namespace BibliotecaDeClases.Logica.Servicios
{
    public class ProcesadorInforme
    {
        


        public static async Task<ResumenPresupuestoModel> getresumenPresupuestoServicio(int idcot)
        {

           return await Task.Run(async () => {
               return await AccesoBD.LoadAsync(@"                
                Declare @GG decimal; 
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
                             Select Servicios.NombreServicio, Servicios.FechaInicio, ClientesServicios.RazonSocial Cliente, @pptomaterial as PresupuestoMateriales, dbo.GastoMaterialTotal(Servicios.ID)  CostoMaterial, @pptopersonal PresupuestoPersonal, 
                             dbo.GastoPersonalTotal(Servicios.ID) CostoPersonal, @pptoequipos PresupuestoEquiposServicios, 
                             dbo.GastoEquiposServiciosTotal(Servicios.ID) CostoEquiposServicios ,  
                             (@PptoMaterial + @PptoEquipos + @PptoPersonal) * @GG / 100 GastosGenerales, dbo.GastosGeneralesTotal(Servicios.ID) CostoGastosGenerales  
                             from CotizacionesServicios inner join Servicios on CotizacionesServicios.ID = Servicios.IDCotizacion
                              inner join ClientesServicios on ClientesServicios.ID = Servicios.IDCliente where CotizacionesServicios.ID = @ID
                            end", new ResumenPresupuestoModel { ID = idcot });
                
           });
        }




        public static async Task<List<ResumenDiaModel>> getResumenAcumuladoServicioAsync(int idservicio)
        {

            return await Task.Run(async () =>
            {

                return await AccesoBD.LoadDataAsync(@"
                DECLARE @start_date DATETIME;
                DECLARE @end_date DATETIME ;                 
                Declare @IDCot int; Select @IDCot =  IDCotizacion from Servicios where ID = @ID
                Select @start_date = Servicios.FechaInicio from Servicios where Servicios.ID = @ID;
                Select @end_date = Servicios.FechaTermino from Servicios where Servicios.ID = @ID;
                WITH AllDays
                AS(SELECT   @start_date AS[Date], 1 AS[level]
                UNION ALL
                SELECT   DATEADD(DAY, 2, [Date]), [level] + 1
                FROM     AllDays
                WHERE[Date] < @end_date)
                SELECT[Date] Fecha, [dbo].[HorasDiaServicio](@ID, [Date]) HorasTotales, [dbo].[ValorHorasDiaServicio](@ID, [Date]) ValorHH, dbo.ValorHorasDiaServicioAcumulado(@ID,[Date]) ValorHHAcumulado,
                [dbo].[ValorColacionesDiaServicio](@ID, [Date]) CostoColaciones, dbo.ValorColacionesDiaServicioAcumulado(@ID,[Date]) CostoColacionesAcumulado , [dbo].[ContarColacionesDiaServicio](@ID, [Date]) CantidadColaciones,
                [dbo].[ValorPeajesDiaServicio](@ID, [Date]) CostoPeajes, dbo.ValorPeajesDiaServicioAcumulado(@ID,[Date]) CostoPeajesAcumulado, [dbo].[ValorCombustibleDiaServicio](@ID, [Date]) CostoCombustible, dbo.ValorCombustibleDiaServicioAcumulado(@ID,[Date]) CostoCombustibleAcumulado,
                [dbo].[ValorMaterialesDiaServicio](@ID, [Date]) CostoMateriales, dbo.ValorMaterialesDiaServicioAcumulado(@ID,[Date]) CostoMaterialesAcumulado, dbo.CostoCotPersonal(@IDCot) PresupuestoPersonal
                FROM   AllDays OPTION(MAXRECURSION 0)", new ResumenDiaModel { ID = idservicio });

            });
        }




    }
}
