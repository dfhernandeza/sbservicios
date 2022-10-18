using BibliotecaDeClases.Modelos.Transacciones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Logica.Transacciones
{
   public class ProcesadorRemuneraciones
    {
        public static async Task igresarRemuneraciones(List<RemuneracionesModel> modelo)
        {
            await Task.Run(async () => { 
                foreach(var item in modelo)
                {
                    await AccesoBD.Comando(@"Insert into Remuneraciones (IDEmpleado, Monto, Mes, Año, CreadoPor, EditadoPor, CreadoEn, EditadoEn)
                    Values (@IDEmpleado, @Monto, @Mes, @Año, @CreadoPor, @EditadoPor, @CreadoEn, @EditadoEn)", item);
                }
     
                
            });
            

        }
    }
}
