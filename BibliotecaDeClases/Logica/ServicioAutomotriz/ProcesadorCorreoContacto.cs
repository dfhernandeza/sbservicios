using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Configuration;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;
namespace BibliotecaDeClases.Logica.ServicioAutomotriz
{
   public class ProcesadorCorreoContacto
    {
        public static async Task insertContactoAsync(ContactoCorreoModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBDAutomotriz.Comando(@"Insert into Contacto (ID, Nombre, Apellido, Email, Mensaje, Fecha, Fono, Marca, ModeloAuto, Ano, Cilindrada) 
              Values (NEWID(), @Nombre, @Apellido, @Email, @Mensaje, GETDATE(), @Fono, @Marca, @ModeloAuto, @Ano, @Cilindrada)", modelo);
                
            });
        }
        public static async Task nuevoContactoAsync(ContactoCorreoModel modelo)
        {
           var insertTask = insertContactoAsync(modelo);
            var sendTask = SendCorreoConfirmacionContactoAsync(modelo);
            var sendInternoTask = SendCorreoConfirmacionContactoInternoAsync(modelo);
            await Task.WhenAll(insertTask,sendInternoTask,sendTask);
        }
        public static async Task<string> bodyCorreoConfirmarContactoAsync(ContactoCorreoModel modelo)
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

                                <h2>Estimado " + modelo.Nombre + " " + modelo.Apellido + @",</h2>
                                <p>
                                    Hemos recibido su solicitud y nos comunicaremos con usted lo antes posible.<br />

                                    Atentamente,<br />
                                    Servicio Automotriz Santa Beatriz<br />


                                </p>

                            


                        </div>
                    </body>
                    </html>";


            });

           
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
                                <p>Cliente: "+ modelo.Nombre + " " + modelo.Apellido + @"<br/> 
                                     Fecha: "+ DateTime.Now  + @"<br/>
                                     Email: " + modelo.Email + @"<br/>
                                      Fono: " + modelo.Fono + @" 
                                </p>
                                <h4>Vehículo</h2>             
                               <p> Marca: " + modelo.Marca + @"<br/> 
                                  Modelo: " + modelo.ModeloAuto + @"<br/>
                                     Año: " + modelo.Ano + @"<br/>
                              Cilindrada: " + modelo.Cilindrada + @"<br/>
                                 Mensaje: "+ modelo.Mensaje  +@"
                                </p>
                                    

                        </div>
                    </body>
                    </html>";


            });


        }
        public static async Task SendCorreoConfirmacionContactoAsync(ContactoCorreoModel modelo)
        {
            var body = await bodyCorreoConfirmarContactoAsync(modelo);

            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("mailSettings/smtp_3");
            string to = modelo.Email;
            string from = "notificaciones@sbservicios.cl";
            string subject = "Confirmación Servicio Automotriz Santa Beatriz";
            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("sbservicios.cl");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("notificaciones@sbservicios.cl", smtpSection.Network.Password);
            client.Send(message);
        }
        public static async Task SendCorreoConfirmacionContactoInternoAsync(ContactoCorreoModel modelo)
        {
            var body = await bodyCorreoConfirmarContactoInternoAsync(modelo);

            var smtpSection = (SmtpSection)ConfigurationManager.GetSection("mailSettings/smtp_3");
            string to = "contacto@sbservicios.cl";
            string from = "notificaciones@sbservicios.cl";
            string subject = "Solicitud " + modelo.Nombre +" " + modelo.Apellido;
            MailMessage message = new MailMessage(from, to, subject, body);
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("sbservicios.cl");
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential("notificaciones@sbservicios.cl", smtpSection.Network.Password);
            client.Send(message);
        }
        public static async Task<List<ContactoCorreoModel>> getSolicitudesAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBDAutomotriz.LoadDataAsync<ContactoCorreoModel>("Select * from Contacto");
            });
         
        }
    }
}
