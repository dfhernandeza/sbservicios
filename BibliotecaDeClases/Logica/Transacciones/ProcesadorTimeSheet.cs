using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.Transacciones;
using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTransacciones;
namespace BibliotecaDeClases.Logica.Transacciones
{
   public class ProcesadorTimeSheet
    {
        public static async Task InsertaNuevosTimeSheetAsync(List<TimeSheetModel> modelo)
        {
           await Task.Run(() => { modelo.ForEach(async x => { 
               await AccesoBD.Comando(@"Insert into TimeSheet (IDEmpleado, IDCECO, IDDocumento, Entrada, Salida, Comentarios, CreadoPor, EditadoPor, CreadoEn, EditadoEn) 
                                         Values (@IDEmpleado, @IDCECO, @IDDocumento, @Entrada, @Salida, @Comentarios, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", x); 
           
           }); 
                
           
           });
        
            

        }
        public static async Task<int> nuevoDocTimeSheet(DocumentoModel documento)
        {

           var data = await AccesoBD.LoadAsync(@"Insert into Documentos (IDDocumento, FechaDocumento, Descripcion, Comentarios, IDTipo, IDSupervisor, CreadoEn, CreadoPor, EditadoEn, EditadoPor) 
               output Inserted.ID
               Values(@IDDocumento, @FechaDocumento, @Descripcion, @Comentarios, 2, @IDSupervisor, @CreadoEn, @CreadoPor, @EditadoEn, @EditadoPor)", documento);
            return data.ID;
        }
        public static async Task eliminarTimeSheets(int iddoc)
        {
            await AccesoBD.Comando("Delete from TimeSheet where IDDocumento = @ID", new { ID = iddoc });
            
        }
        public static async Task editarDocTimeSheet(DocumentoModel documento)
        {
            await AccesoBD.Comando(@"Update Documentos Set FechaDocumento = @FechaDocumento, Descripcion = @Descripcion, 
            Comentarios = @Comentarios, IDSupervisor = @IDSupervisor, EditadoEn = @EditadoEn, 
            EditadoPor = @EditadoPor", documento);
            
        }
        public static async Task<List<TimeSheetModel>> getTimeSheets(int iddoc)
        {
            return await AccesoBD.LoadDataAsync(@"Select ID, IDEmpleado, IDCECO, IDDocumento, Entrada, Salida, 
            Comentarios from TimeSheet where IDDocumento = @ID", new TimeSheetModel { ID = iddoc });
        }
        public static async Task<List<TimeSheetModel>> getDistinctTimeSheets(int iddoc)
        {
            return await AccesoBD.LoadDataAsync(@"Select distinct IDCECO, Entrada, Salida, Comentarios 
            from TimeSheet where IDDocumento = @ID", new TimeSheetModel { ID = iddoc });
        }
        public static async Task<DocumentoModel> cargardocTimeSheetAsync(int iddocumento)
        {
            string sql = @"Select Documentos.ID, FechaDocumento, Documentos.Descripcion, IDSupervisor, Comentarios 
            from Documentos where Documentos.ID = @ID";
            DocumentoModel modelo = new DocumentoModel
            {
                ID = iddocumento
            };

            return await AccesoBD.LoadAsync(sql, modelo);
        }
        public static async Task<List<TimeSheetModel>> getTimeSheetsMes()
        {

            return await AccesoBD.LoadDataAsync<TimeSheetModel>(@"Select PersonalServicios.Nombre +' ' + PersonalServicios.Apellido Empleado, 
            TimeSheet.ID, CECOs.Nombre CECO, Entrada, Salida, TimeSheet.Comentarios, FechaDocumento from TimeSheet 
            inner join CECOs on TimeSheet.IDCECO = CECOs.ID 
            inner join PersonalServicios on PersonalServicios.ID = TimeSheet.IDEmpleado 
            inner join Documentos on Documentos.ID = TimeSheet.IDDocumento");
        }
        public static async Task<List<TimeSheetModel>> getTimeSheetByServicioAsync(int idservicio)
        {

            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, Documentos.FechaDocumento, 
                                        DATEDIFF(HOUR,TimeSheet.Entrada, TimeSheet.Salida) - 0.5 HorasTotales, 
                                        PersonalServicios.ValorHH * (DATEDIFF(HOUR,TimeSheet.Entrada, TimeSheet.Salida)-0.5) ValorHH, PersonalServicios.ID IDEmpleado, TimeSheet.ID ID,
                                        TimeSheet.IDTransaccion IDTransaccion
                                        from PersonalServicios inner join TimeSheet on PersonalServicios.ID = TimeSheet.IDEmpleado
                                        inner join Documentos on Documentos.ID = TimeSheet.IDDocumento
                                        inner join CECOs on TimeSheet.IDCECO = CECOs.ID 
                                        inner join Tareas on Tareas.ID = CECOs.IDTarea
                                        where Tareas.IDServicio = @ID", new TimeSheetModel { ID = idservicio });
            });
        }
        public static async Task<List<TimeSheetModel>> getHHByServicioDiaAsync(int idservicio)
        {

            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select distinct documentos.FechaDocumento, [dbo].[HorasDiaServicio] (Servicios.ID, Documentos.FechaDocumento) HorasTotales, [dbo].[ValorHorasDiaServicio] (Servicios.ID,  Documentos.FechaDocumento) ValorHH
                from Servicios inner join Tareas on Tareas.IDServicio = Servicios.ID
                inner join CECOs on CECOs.IDTarea = Tareas.ID
                inner join TimeSheet on TimeSheet.IDCECO = CECOs.ID
                inner join Documentos on Documentos.ID = TimeSheet.IDDocumento
                where Servicios.ID = @ID", new TimeSheetModel { ID = idservicio });
            });
        }
        public static async Task InsertaNuevoTimeSheetAsync(TimeSheetModel modelo)
        {
            await Task.Run(async () => {
               
                    await AccesoBD.Comando(@"Insert into TimeSheet (IDEmpleado, IDCECO, IDDocumento, Entrada, Salida, Comentarios, IDTransaccion, CreadoPor, EditadoPor, CreadoEn, EditadoEn) 
                                         Values (@IDEmpleado, @IDCECO, @IDDocumento, @Entrada, @Salida, @Comentarios, @IDTransaccion, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", modelo);

               


            });



        }
        public static async Task<int> editarTimeSheetAsync(TimeSheetModel modelo)
        {
           return await Task.Run(async () =>
            {
              var data =  await AccesoBD.LoadAsync(@"Update TimeSheet Set IDCECO = @IDCECO, Entrada = @Entrada, 
                Salida = @Salida, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn
                output inserted.IDTransaccion               
                where ID = @ID", modelo);
                return data.IDTransaccion;
            });
            
        }
        public static async Task<TimeSheetModel> getTimeSheetAsync(int id)
        {
            return await Task.Run(async () => { return await AccesoBD.LoadAsync(@"Select TimeSheet.ID, TimeSheet.IDEmpleado, TimeSheet.IDCECO, TimeSheet.Entrada, 
            TimeSheet.Salida, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado from TimeSheet 
            inner join PersonalServicios on TimeSheet.IDEmpleado = PersonalServicios.ID where TimeSheet.ID = @ID", 
            new TimeSheetModel { ID = id }); });
        }
        public static async Task<List<TimeSheetModel>> filtrarTimeSheetDiaAsync(DateTime fecha)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync(@"Select PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, Documentos.FechaDocumento, 
                                        DATEDIFF(HOUR, TimeSheet.Entrada, TimeSheet.Salida) - 0.5 HorasTotales,
                                        PersonalServicios.ValorHH * (DATEDIFF(HOUR, TimeSheet.Entrada, TimeSheet.Salida) - 0.5) ValorHH, PersonalServicios.ID IDEmpleado, TimeSheet.ID ID,
                                        TimeSheet.IDTransaccion IDTransaccion
                                        from PersonalServicios inner join TimeSheet on PersonalServicios.ID = TimeSheet.IDEmpleado
                                        inner
                                        join Documentos on Documentos.ID = TimeSheet.IDDocumento
                                       where DATEPART(Y,Documentos.FechaDocumento) = DATEPART(Y,@Fecha) and DATEPART(YEAR,Documentos.FechaDocumento) = DATEPART(YEAR,@Fecha)", new TimeSheetModel { Fecha = fecha });
            });
            
        }
        public static async Task<List<TimeSheetModel>> filtrarTimeSheetEmpleadoAsync(string texto, int idservicio)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync(@"Select PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, Documentos.FechaDocumento, 
                                        DATEDIFF(HOUR,TimeSheet.Entrada, TimeSheet.Salida) - 0.5 HorasTotales, 
                                        PersonalServicios.ValorHH * (DATEDIFF(HOUR,TimeSheet.Entrada, TimeSheet.Salida)-0.5) ValorHH, PersonalServicios.ID IDEmpleado, TimeSheet.ID ID,
                                        TimeSheet.IDTransaccion IDTransaccion
                                        from PersonalServicios inner join TimeSheet on PersonalServicios.ID = TimeSheet.IDEmpleado
                                        inner join Documentos on Documentos.ID = TimeSheet.IDDocumento
                                        inner join CECOs on TimeSheet.IDCECO = CECOs.ID 
                                        inner join Tareas on Tareas.ID = CECOs.IDTarea
                                        where Tareas.IDServicio = @ID and (PersonalServicios.Nombre like '%' + @Empleado + '%' or PersonalServicios.Apellido like '%' + @Empleado + '%')", new TimeSheetModel { ID = idservicio, Empleado = texto });
            });

        }
    }
}
