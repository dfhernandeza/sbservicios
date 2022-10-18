using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Empleados
{
    public class LiquidacionesModel
    {
        public string ID { get; set; }
        public string IDEmpleado { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public byte[] Documento { get; set; }
    }

    public class EmpModel
    {
        public string ID { get; set; }
        public string IDEmpleado { get; set; }
        public List<SelectListItem> Empleados { get; set; }
        public List<SelectListItem> Meses { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public HttpPostedFileBase Documento { get; set; }
        public string Multi { get; set; }
    }
}