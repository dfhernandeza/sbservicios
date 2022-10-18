using BibliotecaDeClases.Modelos.Transacciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Logica.Transacciones
{
   public class ProcesadorHorasExtra
    {
        public static async Task insertarHorasExtraAsync(HorasExtraModel modelo)
        {
            await Task.Run(async () =>
            {              
             await AccesoBD.Comando(@"Insert into HorasExtra (Fecha, IDEmpleado, Horas, Factor, IDCECO, IDTransaccion, IDDocumento, Comentarios, 
                                  CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                                  Values (@Fecha, @IDEmpleado, @Horas, @Factor, @IDCECO, @IDTransaccion, @IDDocumento, @Comentarios, 
                                  @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)", modelo);
               
            });
          
        }
        public static async Task<List<HorasExtraModel>> getHorasExtraAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync<HorasExtraModel>(@"
            Select distinct Documentos.ID, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, 
            DATEPART(MONTH,HorasExtra.Fecha) Mes, DATEPART(YEAR,HorasExtra.Fecha) Año,
            sum(HorasExtra.Horas) Horas
            from PersonalServicios 
            inner join HorasExtra on PersonalServicios.ID = HorasExtra.IDEmpleado
            inner join Documentos on Documentos.ID = HorasExtra.IDDocumento
            group by Documentos.ID, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido, DATEPART(MONTH,HorasExtra.Fecha), DATEPART(YEAR,HorasExtra.Fecha)");
            });
 
        }
        public static async Task<List<HorasExtraModel>> getHHExtraDoc(int iddoc)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync(@"
            Select HorasExtra.ID, Fecha, Horas, Factor, CECOs.Nombre CECO, HorasExtra.Comentarios, DATEPART(MONTH,Documentos.FechaDocumento) Mes from HorasExtra 
            inner join CECOs on CECOs.ID = HorasExtra.IDCECO 
            inner join Documentos on HorasExtra.IDDocumento = Documentos.ID
            where horasextra.IDDocumento = @ID", new HorasExtraModel { ID = iddoc });
            });
        }
        public static async Task<DocumentoModel> getDocHHExtraAsync(int iddoc)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync(@"
           Select distinct Documentos.FechaDocumento, Supervisor.Nombre + Supervisor.Apellido Supervisor, Empleado.Nombre + ' ' + Empleado.Apellido Empleado 
           from Documentos  
           inner join PersonalServicios as Supervisor on Documentos.IDSupervisor = Supervisor.ID
           inner join HorasExtra on HorasExtra.IDDocumento = Documentos.ID
           inner join PersonalServicios as Empleado on HorasExtra.IDEmpleado = Empleado.ID
           where Documentos.ID = @ID", new DocumentoModel { ID = iddoc });
            });
        }
        public static async Task editarHoraExtraAsync(HorasExtraModel modelo)
        {
            await Task.Run(async () =>
            {
                await AccesoBD.Comando(@"Update HorasExtra Set Fecha = @Fecha, Horas = @Horas, Factor = @Factor, IDCECO = @IDCECO, 
                Comentarios = @Comentarios, EditadoPor = @EditadoPor, EditadoEn = @EditadoEn where ID = @ID", modelo);
            });
        
        }
        public static async Task eliminarHoraExtraAsync(int id)
        {
            await Task.Run(async () => { await AccesoBD.Comando("Delete from HorasExtra where ID = @ID", new { ID = id }); });
            
        }
        public static async Task<HorasExtraModel> getHoraExtraAsync(int id)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync("Select * from HorasExtra where ID = @ID", new HorasExtraModel {ID = id });
            });
            
        }
    }
}
