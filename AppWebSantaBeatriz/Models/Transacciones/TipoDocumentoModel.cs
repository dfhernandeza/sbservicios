using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AppWebSantaBeatriz.Models.Transacciones
{
    public class TipoDocumentoModel
    {
        public int ID { get; set; }
        [Display(Name ="Tipo de Documento")]

        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public string CreadoPor { get; set; }
        public DateTime CreadoEn { get; set; }
        public string EditadoPor { get; set; }
        public DateTime EditadoEn { get; set; }
    }
}