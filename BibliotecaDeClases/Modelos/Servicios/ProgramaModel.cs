using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Servicios
{
   public class ProgramaModel
    {
        public int ID { get; set; }
        public int IDTarea { get; set; }
        public List<SelectList> Tareas { get; set; }
        public int IDEmpleado { get; set; }

        public string Empleado { get; set; }
        public string Especialidad { get; set; }

        public int IDServicio { get; set; }
        public List<SelectList> Empleados { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaTermino { get; set; }
        public string Responsabilidades { get; set; }

        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string   EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }

        public int Ocupado { get; set; }

        
    }
}
