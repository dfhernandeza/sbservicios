using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Transacciones
{
   public class CuentaModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public int IDTipo { get; set; }
        public List<SelectListItem> TiposDeCuentas { get; set; }
        public string Tipo { get; set; }
        public DateTime CreadoEn { get; set; }
        public string CreadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
        public string EditadoPor { get; set; }
        public int IDSubTipo { get; set; }
        public List<SelectListItem> SubTiposDeCuentas { get; set; }
        public string SubTipo { get; set; }
        public string Descripcion { get; set; }
        public int IDReporte { get; set; }

    }
}
