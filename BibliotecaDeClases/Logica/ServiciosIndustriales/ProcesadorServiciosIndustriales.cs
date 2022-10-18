using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
using System.Net.Mail;
using System.Net;

namespace BibliotecaDeClases.Logica.ServiciosIndustriales
{
    public class ProcesadorServiciosIndustriales
    {
        public static async Task<List<FotoModel>> getFotosElectricidad()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoModel>("Select ID, URL, Nombre, FechaSubida, Descripcion from Fotos where IDSeccion = '5CE06DB3-1970-446F-AE88-2519A13BE782'");
            });
        }

        public static async Task<List<FotoModel>> getFotosMecanica()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoModel>("Select ID, URL, Nombre, FechaSubida, Descripcion from Fotos where IDSeccion = '1D337FB8-4403-4EFE-891B-C4EF77B372A6'");
            });
        }

        public static async Task<List<FotoModel>> getFotosEstrucuturas()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoModel>("Select ID, URL, Nombre, FechaSubida, Descripcion from Fotos where IDSeccion = '63208B82-946B-46E4-A489-8D92A19F0CFB'");
            });
        }

        public static async Task<List<FotoModel>> getFotosHidraulica()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<FotoModel>("Select ID, URL, Nombre, FechaSubida, Descripcion from Fotos where IDSeccion = 'B121800F-287A-4611-A2E7-1627FC07726F'");
            });
        }
        public static async Task nuevoContactoAsync(ContactoCorreoModel modelo)
        {
          
            await SendCorreoConfirmacionContactoInternoAsync(modelo);
            
        }

        public static async Task SendCorreoConfirmacionContactoInternoAsync(ContactoCorreoModel modelo)
        {
            var body = await bodyCorreoConfirmarContactoInternoAsync(modelo);

            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("mailSettings/smtp_3");
            string to = "contacto@sbservicios.cl";
            string from = "notificaciones@sbservicios.cl";
            string subject = "Solicitud " + modelo.Nombre + " " + modelo.Apellido + "Servicios Industriales";
            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("sbservicios.cl");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("notificaciones@sbservicios.cl", smtpSection.Network.Password);
            client.Send(message);
        }

        public static async Task<string> bodyCorreoConfirmarContactoInternoAsync(ContactoCorreoModel modelo)
        {
            return await Task.Run(() => {
                return @"<!DOCTYPE html>
                    <html>
                    <head>
                        <style>
                            .btna{
                                text-decoration:none !important;
                                background-color:darkslategrey;
                                color:white !important;
                                border:none;
                                border-radius:8px;
                                padding:10px;
                                margin-top:10px
                            }
                            .btnb {
                                text-decoration: none !important;
                                background-color: #171b48;
                                color: white !important;
                                border: none;
                                border-radius: 8px;
                                padding: 10px;
                                margin-top: 10px
                            }
                            .container {
                                background-color: #f7f6f6;
                                padding: 10px;
                                padding-bottom: 30px;
                                padding-left: 20px;
                                width: 55%;
                                border-radius: 8px;
                                box-shadow: inset 0 3px 6px rgba(0,0,0,0.16), 0 4px 6px rgba(0,0,0,0.45)
                            }

                        </style>
                        <meta charset=""utf-8"" />
                        <title></title>
                    </head>
                    <body>
                        <div class=""container"">

                                <h2>Nueva Solicitud</h2>
                                <p>Cliente: " + modelo.Nombre + " " + modelo.Apellido + @"<br/> 
                                     Fecha: " + DateTime.Now + @"<br/>
                                     Email: " + modelo.Email + @"<br/>
                                      Fono: " + modelo.Fono + @" 
                                </p>
                                <h4>Detalles</h2>             
                               <p>Mensaje: " + modelo.Mensaje + @"</p>                                   

                        </div>
                    </body>
                    </html>";


            });


        }
    }
}
