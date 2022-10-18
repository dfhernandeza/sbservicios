using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos.Transacciones
{
   public class TransferenciaModel
    {
        public int ID { get; set; }
        public List<SelectListItem> CuentasOrigen { get; set; }
        public List<SelectListItem> CuentasDestino { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Item { get; set; }
        public string Descripcion { get; set; }
    }
}
