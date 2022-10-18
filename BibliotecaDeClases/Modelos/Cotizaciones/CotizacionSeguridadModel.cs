using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos
{
    public class CotizacionSeguridadModel
    {

        public int ID { get; set; }
        public int IDCot { get; set; }
            public string Item { get; set; }
            public double? Cantidad { get; set; }

            public double? PUnitario { get; set; }
      
            public double? Total

            {

                get { return (Cantidad * PUnitario); }


            }

        public string CreadoPor { get; set; }

        public DateTime CreadoEn { get; set; }

        public string EditadoPor { get; set; }

        public DateTime EditadoEn { get; set; }

    }
}
