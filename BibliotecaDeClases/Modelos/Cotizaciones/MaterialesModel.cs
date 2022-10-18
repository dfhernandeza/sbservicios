using OfficeOpenXml.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Modelos
{
  public  class MaterialesModel
    {
        [EpplusIgnore]
        public int ID { get; set; }
        [EpplusIgnore]
        public int IDCot { get; set; }
       
        public string Item { get; set; }
       
        public string Unidad { get; set; }


        public decimal Cantidad { get; set; }

        public decimal PUnitario { get; set; }
        [EpplusIgnore]
        public decimal VTotal
        {

            get { return PUnitario * Cantidad; }


        }

        [EpplusIgnore]
        public decimal Total { get; set; }
        [EpplusIgnore]
        public string Tipo { get; set; }
        [EpplusIgnore]
        public string CreadoPor { get; set; }
        [EpplusIgnore]

        public DateTime CreadoEn { get; set; }
        [EpplusIgnore]
        public string EditadoPor { get; set; }
        [EpplusIgnore]

        public DateTime EditadoEn { get; set; }

        public string Estado { get; set; }
        public int IDCategoria { get; set; }
    }
    [EpplusTable]
    public class MaterialForExcelModel

    {
        
       
        public int ID { get; set; }       

        public string Item { get; set; }

        public string Unidad { get; set; }


        public decimal Cantidad { get; set; }


        public decimal PUnitario { get; set; }

        public string Tipo { get; set; }
        public int IDCategoria { get; set; }
    }




}
