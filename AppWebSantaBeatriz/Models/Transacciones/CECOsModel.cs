using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Transacciones
{
    public class CECOsModel
    {
        public int IDCECOMain { get; set; }
        public string Nombre { get; set; }
        public double Gasto { get; set; }
        public DateTime CECOCreadoEn { get; set; }
        public string DescripcionCECO { get; set; }
        public string CECOCreadoPor { get; set; }
        public string CECOEditadoPor { get; set; }
        public DateTime CECOEditadoEn { get; set; }
        public int IDEstadoCECO { get; set; }

        public List<SelectListItem> Estados {get; set;}

        public string EstadoCECO { get; set; }

        public int IDEncargadoCECO { get; set; }
        public string EncargadoCECO { get; set; }

        public int IDTipoCECO { get; set; }
        public string TipoCECO { get; set; }

        public List<SelectListItem> EncargadosCECO { get; set; }
    }
}