using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AppWebSantaBeatriz.Models
{
    public class EmpleadoModel
    {
        public int ID { get; set; }
        [Display(Name ="Rut")]
    
        public string IDEmpleado { get; set; }
        public string IDSinFormato { get; set; }
        public string Cargo { get; set; }
        public string Especialidad { get; set; }

        public int IDEspecialidad { get; set; }

        public int IDSupervisor { get; set; }
        public string Supervisor { get; set; }

        [Required(ErrorMessage = "Ingresar Nombre.")]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        public string NombreCompleto { get; set; }

        [Required(ErrorMessage = "Ingresar Apellidos.")]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Fecha de Nacimiento")]
        public DateTime FechadeNacimiento { get; set; }
        

        public string Direccion { get; set; }


        [Required(ErrorMessage = "Ingresar un correo electrónico.")]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }



        [Required(ErrorMessage = "Debe ingresar un número de teléfono.")]
        [Display(Name = "Fono")]
        public string Fono { get; set; }


        public string OtrosDetalles { get; set; }

 


        public List<SelectListItem> ListaEspecialidades { get; set; }



        public List<SelectListItem> ListaSupervisores { get; set; }

        public string TallaZapatos { get; set; }
        public List<SelectListItem> ListaTallasZapatos { get; set; }
        public string TallaPantalon { get; set; }
        public List<SelectListItem> ListaTallaPantalones { get; set; }

        public List<SelectListItem> ListaTallasTop { get; set; }
        public string TallaTop { get; set; }
        public string NombreApellido { get; set; }

        public string CreadoPor { get; set; }
        public string EditadoPor { get; set; }

        public DateTime CreadoEn { get; set; }
        public DateTime EditadoEn { get; set; }

    }
}