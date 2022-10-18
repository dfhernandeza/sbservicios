using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models
{
    public class CotizacionPersonalModel

    {
        public int ID { get; set; }
        [Display(Name = "ID Cotización")]
        public int IDCot { get; set; }
        public int IDEspecialidad { get; set; }

        public string Especialidad { get; set; }
        [Display(Name = "Cantidad")]
        public decimal? Cantidad { get; set; }
        [Display(Name = "HH")]
        public decimal? HH { get; set; }

        [Display(Name = "Total HH")]
        
        public decimal? TotalHH
        {

            get { return (Cantidad * HH); }

        }
        [DataType(DataType.Currency)]
        [Display(Name = "Valor HH")]
        public decimal? ValorHH { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Total")]
        public decimal? Total

        {

            get { return (TotalHH * ValorHH); }


        }

        public  List<SelectListItem> ListaEspecialidades { get; set; }

        public string CreadoPor { get; set; }

        public DateTime CreadoEn { get; set; }

        public string EditadoPor { get; set; }

        public DateTime EditadoEn { get; set; }


    }
}