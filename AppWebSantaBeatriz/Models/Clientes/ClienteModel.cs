using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models.Clientes
{
    public class ClienteModel
    {
        public int ID { get; set; }
        [Required]
        public string Alias { get; set; }
        public string IDSinFormato { get; set; }
        [Display(Name = "Rut")]
        [Required]
        public string IDCliente { get; set; }
        [Display(Name = "Razón Social")]
        [Required]
        public string RazonSocial { get; set; }
        [Required]
        public string Giro { get; set; }
        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }
        [Required]
        public bool Arauco { get; set; }

        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }

        public List<ContactoModel> Contactos { get; set; }
        public List<UbicacionesModel> Ubicaciones { get; set; }



    }
}