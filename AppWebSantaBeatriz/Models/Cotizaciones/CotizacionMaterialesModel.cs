using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Services.Description;

namespace AppWebSantaBeatriz.Models
{
    public class CotizacionMaterialesModel

    {
        public int ID { get; set; }
        [Display(Name="ID Cotización")]
        public int IDCot { get; set; }
        [Display(Name = "Item")]
        public string Item { get; set; }
        [Display(Name = "Unidad")]
        public string Unidad { get; set; }
        [Display(Name = "Cantidad")]
        
        public decimal ? Cantidad { get; set; } 
        [Display(Name = "Precio Unitario")]
        [DataType(DataType.Currency)]
        public decimal ? PUnitario { get; set; }
        [Display(Name = "Total")]
        [DataType(DataType.Currency)]
        public decimal ? VTotal
        {
                      
            get { return PUnitario * Cantidad;}
                                    

        }
        public string Tipo { get; set; }
        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }

        public string EditadoPor { get; set; }

        public DateTime EditadoEn { get; set; }

        public int IDCategoria { get; set; }
    }
}