using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Servicios
{
    public class ProgramaModel
    {
        public int ID { get; set; }
        public int IDTarea { get; set; }
        public List<SelectListItem> Tareas { get; set; }
        public string IDEmpleado { get; set; }

        public string Empleado { get; set; }

        public string Especialidad { get; set; }
        public int IDServicio { get; set; }
        public List<SelectListItem> Empleados { get; set; }
        public DateTime FechaInicial { get; set; }
        public DateTime FechaTermino { get; set; }

        public string Responsabilidades { get; set; }
    }
}