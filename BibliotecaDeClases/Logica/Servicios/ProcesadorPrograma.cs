using BibliotecaDeClases.Modelos.Servicios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Logica.Servicios
{
  public class ProcesadorPrograma
    {
        public static async Task<int> CrearAsignacionAsync(int idtarea, int idempleado, DateTime fechainicial, DateTime fechatermino, string responsabilidades, string creadopor  )
        {
            string sql = "Insert into ProgramaServicios (IDTarea, IDEmpleado, FechaInicial, FechaTermino, Responsabilidades, CreadoPor, CreadoEn, EditadoPor, EditadoEn) Values (@IDTarea, @IDEmpleado, @FechaInicial, @FechaTermino, @Responsabilidades, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)";
            ProgramaModel modelo = new ProgramaModel
            {
                IDTarea = idtarea,
                IDEmpleado = idempleado,
                FechaInicial = fechainicial,
                FechaTermino = fechatermino,
                Responsabilidades = responsabilidades,
                CreadoPor = creadopor,
                CreadoEn = DateTime.Now,
                EditadoEn = DateTime.Now,
                EditadoPor = creadopor
                
            };
            return await AccesoBD.Comando(sql, modelo);
        }
          public static async Task CrearAsignacionAsync(List<ProgramaModel> lista )
        {

            foreach (var item in lista)
            {
                
              await Task.Run(async ()=>{
                        await AccesoBD.Comando(@"Insert into ProgramaServicios (IDTarea, IDEmpleado, FechaInicial, FechaTermino, Responsabilidades, CreadoPor, CreadoEn, EditadoPor, EditadoEn) 
                        Values (@IDTarea, @IDEmpleado, @FechaInicial, @FechaTermino, @Responsabilidades, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn)",item);
                });

            }
         
     
        }
        public static async Task<int> EditarAsignacionAsync(int id, int idempleado, DateTime fechainicial, DateTime fechatermino, string responsabilidades)
        {
            {
                string sql = "Update ProgramaServicios Set  IDEmpleado = @IDEmpleado, FechaInicial = @FechaInicial, FechaTermino = @FechaTermino, Responsabilidades = @Responsabilidades where ID = @ID ";
                ProgramaModel modelo = new ProgramaModel
                {
                    ID = id,                 
                    IDEmpleado = idempleado,
                    FechaInicial = fechainicial,
                    FechaTermino = fechatermino,
                    Responsabilidades = responsabilidades
                };
                return await AccesoBD.Comando(sql, modelo);
            }
        }

        public static async Task<List<ProgramaModel>> LoadAsignacionesAsync(int idtarea)
        {
            string sql = "Select * from ProgramaServicios where IDTarea = @IDTarea";
            ProgramaModel model = new ProgramaModel
            {
                IDTarea = idtarea
            };

            return await AccesoBD.LoadDataAsync(sql, model);

        }

        public static async Task<List<ProgramaModel>> LoadAsignacionAsync(int id)
        {
            string sql = "Select * from ProgramaServicios where ID = @ID";
            ProgramaModel model = new ProgramaModel
            {
                ID = id
            };
            return await AccesoBD.LoadDataAsync(sql, model);
        
        }

        public static async Task<int> BorrarAsignacionAsync(int id)
        {
            string sql = "Delete from ProgramaServicios where ID = @ID";
            ProgramaModel model = new ProgramaModel
            {
                ID = id
            };

           return await AccesoBD.Comando(sql, model);
        }

        public static Task<List<ProgramaModel>> EmpleadosPorServicioAsync(int idservicio)
        {
            return Task.Run(() =>
            {
                return AccesoBD.LoadDataAsync("Select distinct PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, Especialidades.Especialidad Especialidad from ProgramaServicios " +
                    "inner join PersonalServicios on ProgramaServicios.IDEmpleado = PersonalServicios.ID inner join Especialidades on Especialidades.ID = PersonalServicios.IDEspecialidad " +
                    "inner join Tareas on ProgramaServicios.IDTarea = Tareas.ID where Tareas.IDServicio = @IDServicio", new ProgramaModel { IDServicio = idservicio });
            });
                  

        }

        public static async Task<string> DisponibleAsync(int idempleado, DateTime finicial, DateTime ftermino, int idtarea, string origen)
        {
            var datos = await AccesoBD.LoadAsync("Select FechaInicial, FechaFinal from Tareas where ID = @ID", new TareaModel { ID = idtarea });
            if (finicial < datos.FechaInicial || finicial > datos.FechaFinal || ftermino < datos.FechaInicial || ftermino > datos.FechaFinal)
            {
                return "Debe estar dentro del inicio y el termino de la tarea";
            }
            else
            {
                var data = 0;
                if (origen == "Nueva")
                {
                   var dato = await AccesoBD.LoadAsync("select Count(ProgramaServicios.IDEmpleado) as Ocupado from ProgramaServicios " +
                 "where IDEmpleado = @IDEmpleado and ((FechaInicial <= @FechaInicial and FechaTermino >= @FechaInicial) or(FechaInicial <= @FechaTermino and FechaTermino >= @FechaTermino)  " +
                 "or(@FechaInicial <= FechaInicial and @FechaTermino >= FechaTermino)) ",
                  new ProgramaModel { IDEmpleado = idempleado, FechaInicial = finicial, FechaTermino = ftermino});
                    data = dato.Ocupado;
                }
                else
                {
                     var dato  = await AccesoBD.LoadAsync("select Count(ProgramaServicios.IDEmpleado) as Ocupado from ProgramaServicios " +
                   "where IDEmpleado = @IDEmpleado and IDTarea <> @IDTarea and((FechaInicial <= @FechaInicial and FechaTermino >= @FechaInicial) or(FechaInicial <= @FechaTermino and FechaTermino >= @FechaTermino)  " +
                   "or(@FechaInicial <= FechaInicial and @FechaTermino >= FechaTermino)) ",
                    new ProgramaModel { IDEmpleado = idempleado, FechaInicial = finicial, FechaTermino = ftermino, IDTarea = idtarea });
                    data = dato.Ocupado;
                }
               

                if (data >= 1)
                {
                    return "Hay un conflicto con otra tarea";
                }
                else
                {
                    return "true";
                }
            }





        }
    }
}
