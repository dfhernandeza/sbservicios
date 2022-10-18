using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace AppWebSantaBeatriz.Models.Transacciones
{
    public class TimeSheetModel
    {
        public List<EmpleadoModel> Empleados { get; set; }
        public int ID { get; set; }
        public DateTime Fecha { get; set; }

        public int IDCECO { get; set; }

        public string CECO { get; set; }
        public List<CECOsModel> CECOs { get; set; }
        public string IDEmpleado { get; set; }
        public string Empleado { get; set; }
        public DateTime Entrada { get; set; }
        public DateTime Salida { get; set; }
        public string Comentarios { get; set; }


    }
}