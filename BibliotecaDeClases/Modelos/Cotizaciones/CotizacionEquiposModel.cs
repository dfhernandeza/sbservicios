using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos
{
  public  class CotizacionEquiposModel
    {

        public int ID { get; set; }
        public int IDCot { get; set; }
            public string Item { get; set; }
            public string Unidad { get; set; }
            public decimal? Cantidad { get; set; }
           
            public decimal? PUnitario { get; set; }
            
            public decimal? Total

            {

                get { return (Cantidad * PUnitario); }


            }

        public string CreadoPor { get; set; }

        public DateTime CreadoEn { get; set; }

        public string EditadoPor { get; set; }

        public DateTime EditadoEn { get; set; }



    }
   
}
