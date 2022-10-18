using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BibliotecaDeClases.Modelos
{
   public class CotizacionPersonalModel
    {
        public int ID { get; set; }
        public int IDCot { get; set; }
      
        public int IDEspecialidad { get; set; }

        public string Especialidad { get; set; }

        public decimal Cantidad { get; set; }
      
        public decimal HH { get; set; }

       
        public decimal TotalHH
        {

            get { return (decimal)(Cantidad * HH); }

        }

     
        public decimal ValorHH { get; set; }

  
        public decimal Total

        {

            get { return (decimal)(TotalHH * ValorHH); }


        }

        public  List<SelectListItem> ListaEspecialidades { get; set; }



        public string CreadoPor { get; set; }

        public DateTime CreadoEn { get; set; }

        public string EditadoPor { get; set; }

        public DateTime EditadoEn { get; set; }

        public decimal Dias { get; set; }

    }
}

