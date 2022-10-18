using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.Empleados;
using static BibliotecaDeClases.Logica.AccesoBlobs.ProcesadorFiniquitos;
namespace BibliotecaDeClases.Logica.Personal
{
   public class ProcesadorFiniquitos
    {
        public static async Task insertarFiniquito(FiniquitoModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando("Insert into Finiquitos (ID, IDEmpleado, Fecha) Values (@ID, @IDEmpleado, @Fecha)", modelo);
            });
            
        }
        public static async Task actualizarEstadoContratoAsync(EmpleadoModel modelo)
        {

            await Task.Run(async () => {
                await AccesoBD.Comando("Update PersonalServicios Set IDEstadoContrato = '795E659A-733B-4B97-87C8-B83462505694' where ID = @ID", modelo);
            });
        }
        public static async Task nuevoFiniquitoAsync(FiniquitoModel modelo)
        {
            modelo.ID = Guid.NewGuid().ToString();
            await subirNuevoFiniquito(modelo);
            await insertarFiniquito(modelo);
            await actualizarEstadoContratoAsync(new EmpleadoModel {ID = modelo.IDEmpleado });
        }
        public static async Task<FiniquitoModel> obtenerFiniquitoAsync(int idempleado)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync("Select ID, IDEmpleado, Fecha from Finiquitos where IDEmpleado = @IDEmpleado", new FiniquitoModel { IDEmpleado = idempleado }); 
            });
        }
    }
}
