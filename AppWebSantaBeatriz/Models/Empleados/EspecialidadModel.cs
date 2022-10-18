using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWebSantaBeatriz.Models
{
   public class EspecialidadModel
    {
         public int ID { get; set; }
         public string Especialidad { get; set; }
         public double ValorHH { get; set; }
         public string Descripcion { get; set; }
         public string CreadoPor { get; set; }
         public string EditadoPor { get; set; }
         public DateTime CreadoEn { get; set; }
         public DateTime EditadoEn { get; set; }
    }
}

