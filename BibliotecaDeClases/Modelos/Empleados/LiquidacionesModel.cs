using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Empleados
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
        public string Empleado { get; set; }
        public string ID { get; set; }
        public string IDEmpleado { get; set; }
        public string IDEmpleadoInt { get; set; }
        public string IDUsuario { get; set; }
        public string Mes { get; set; }
        public string Ano { get; set; }
        public HttpPostedFileBase Documento { get; set; }
        public Stream Elemento { get; set; }
        public string MesTexto { get; set; }
        public string Firma { get; set; }
        public DateTime FechaSubida { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public DateTime? FechaFirma { get; set; }
        public string Firmado { get; set; }
        public string TextoPagina { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }
        public List<Mes> Meses { get; set; }
        public List<SelectListItem> MesesSelect { get; set; }
        public List<SelectListItem> Años { get; set; }
    }

    public class Mes
    {
        public string Nombre { get; set; }
        public string Numero { get; set; }
    }
}
