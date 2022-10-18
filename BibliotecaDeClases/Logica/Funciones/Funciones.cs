using BibliotecaDeClases.Modelos.Empleados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BibliotecaDeClases.Logica
{
  public static  class Funciones
    {
        public static string GetRut(string texto)
        {

            if (texto.Length == 12)
            {
                string a = texto.Substring(0, 2).ToString();
                string b = texto.Substring(3, 3).ToString();
                string c = texto.Substring(7, 3).ToString();
                string d = texto.Substring(11, 1).ToString();
                return a + b + c + d;
            }
            else if (texto.Length == 11)
            {
                string a = texto.Substring(0, 1).ToString();
                string b = texto.Substring(2, 3).ToString();
                string c = texto.Substring(6, 3).ToString();
                string d = texto.Substring(10, 1).ToString();
                return a + b + c + d;
            }

            else
            {
                return texto;
            }

        }

        public static string FormatRutView(string rut)
        {
            if (rut == null)
            {
                return "";
            }
            if (rut.Length == 8)
            {
                string a = rut.Substring(0, 1);
                string b = rut.Substring(1, 3);
                string c = rut.Substring(4, 3);
                string d = rut.Substring(7, 1);

                return String.Format("{0}.{1}.{2}-{3}", a, b, c, d);
            }

            else if (rut.Length == 9)
            {
                string a = rut.Substring(0, 2);
                string b = rut.Substring(2, 3);
                string c = rut.Substring(5, 3);
                string d = rut.Substring(8, 1);
                return String.Format("{0}.{1}.{2}-{3}", a, b, c, d);

            }

            else
            {
                return rut;
            }

        }


        public static  Task<string> RutNombre(string rut)
        {
            if (rut == "037a1192-aa40-4b0e-bab6-6982a1ac3813") {
                return Task.Run(() =>
                {
                     return "Administrador";
                });
                
            }
            else
            {
                return Task.Run(async () =>
                {
                    var data = await AccesoBD.LoadAsync("Select Nombre from PersonalServicios inner join AspNetUsers on PersonalServicios.IdUsuario = AspNetUsers.Id where PersonalServicios.IdUsuario = @IDEmpleado", new EmpleadoModel { IDEmpleado = rut });
                    return data.Nombre;
                });
            }
            
            

        }

        public static String ToUpperCheckForNull(this string input)
        {

            string retval = input;

            if (!String.IsNullOrEmpty(retval))
            {
                retval = retval.ToUpper();
            }

            return retval;

        }




    }
}
