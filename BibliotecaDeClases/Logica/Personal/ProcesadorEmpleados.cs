using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BibliotecaDeClases;
using BibliotecaDeClases.Modelos;
using BibliotecaDeClases.Modelos.Empleados;
using static BibliotecaDeClases.Logica.Funciones;
using static BibliotecaDeClases.Logica.AccesoBlobs.AccesoBlobs;
using iText.Kernel.Pdf;
using System.IO;
using iText.Kernel.Pdf.Xobject;
using iText.Layout;
using iText.Kernel.Geom;
using iText.Layout.Element;
using iText.Forms;
using iText.Kernel.Pdf.Canvas.Parser.Data;
using iText.Kernel.Pdf.Canvas.Parser.Listener;
using iText.Kernel.Pdf.Canvas.Parser;
using iText.Forms.Fields;
using System.Web;

namespace BibliotecaDeClases
{
   public class ProcesadorEmpleados
    {


        public static async Task<int> CreateEmployeeAsync(EmpleadoModel empleado)
        {
            empleado.Nombre = empleado.Nombre.ToUpperCheckForNull();
            empleado.Apellido = empleado.Apellido.ToUpperCheckForNull();
            empleado.Cargo = empleado.Cargo.ToUpperCheckForNull();
            empleado.Direccion = empleado.Direccion.ToUpperCheckForNull();
            empleado.Email = empleado.Email.ToUpperCheckForNull();
            empleado.OtrosDetalles = empleado.OtrosDetalles.ToUpperCheckForNull();
            empleado.CreadoEn = empleado.EditadoEn = DateTime.Now;
            
            string sql = @"Insert into dbo.PersonalServicios (IDEmpleado, Nombre, Apellido, Email, Fono, Direccion, FechaDeNacimiento, Cargo, 
            IDEspecialidad, IDSupervisor, OtrosDetalles, TallaPantalon, TallaTop, TallaZapatos, ValorHH, CreadoPor, CreadoEn, EditadoPor, EditadoEn, CuentaGastos, CuentaPagos, FechaContratacion, SueldoBase, Cod, Planta, IDEstadoContrato) 
            output Inserted.ID Values (@IDEmpleado, @Nombre, @Apellido, @Email, @Fono, @Direccion, @FechaDeNacimiento,@Cargo, @IDEspecialidad, 
                        @IDSupervisor, @OtrosDetalles, @TallaPantalon, @TallaTop, @TallaZapatos, @ValorHH, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn, @CuentaGastos, @CuentaPagos, @FechaContratacion, @SueldoBase, NEWID(), @Planta, '983CE24B-5785-4CA4-ABA5-F052150037DE')"
;
          var datos = await AccesoBD.LoadAsync(sql, empleado);
            return datos.ID;
        }
        public static async Task<int> BorrarEmpleadoAsync(int id)
        {
            EmpleadoModel data = new EmpleadoModel
            {
                ID = id
            };
            string sql = @"Delete from dbo.PersonalServicios where ID = @ID";
            return await AccesoBD.Comando(sql, data);

        }        
        public static async Task<List<EmpleadoModel>> LoadEmployeesAsync()
        {

            string sql = @"Select PersonalServicios.ID, IDEmpleado, Nombre, Apellido, (select AspNetUsers.UserName from AspNetUsers where Id = PersonalServicios.IdUsuario) Usuario,
            Especialidades.Especialidad Especialidad, Email, FechaCorreoAct, Actualizado from PersonalServicios inner join Especialidades 
            on PersonalServicios.IDEspecialidad = Especialidades.ID;";
            return await AccesoBD.LoadDataAsync<EmpleadoModel>(sql);


        }
        public static async Task<EmpleadoModel> LoadEmployeeAsync(int id)
        {
           
            EmpleadoModel data = new EmpleadoModel
            {
                ID = id
            };
            string sql = @"Select empleado.ID, empleado.IDEmpleado, empleado.Nombre + ' ' + empleado.Apellido NombreCompleto, 
            empleado.Cargo, empleado.IDEspecialidad, empleado.IDSupervisor, empleado.Nombre, 
            empleado.Apellido, empleado.FechaDeNacimiento, empleado.Fono, empleado.Direccion, 
            empleado.Email, empleado.OtrosDetalles, empleado.TallaZapatos, 
            empleado.TallaPantalon, empleado.TallaTop, empleado.IDEspecialidad, empleado.ValorHH, empleado.SueldoBase, 
            empleado.CreadoPor, empleado.CreadoEn, empleado.EditadoPor, empleado.EditadoEn, 
            supervisor.Nombre + supervisor.Apellido Supervisor, Especialidades.Especialidad Especialidad, empleado.FechaContratacion, 
            empleado.IDTipoLicencia, empleado.IDEstadoCivil, empleado.IDNacionalidad
            from PersonalServicios as empleado join PersonalServicios as supervisor 
            on empleado.IDSupervisor = supervisor.ID inner join Especialidades 
            on empleado.IDEspecialidad = Especialidades.ID where empleado.ID = @ID";


            return await AccesoBD.LoadAsync(sql,data);

              

        }
        public static async Task<List<SelectListItem>> SupervisoresAsync()

        {
            string sql = "Select ID Value, Nombre + ' ' + Apellido Text from PersonalServicios";
             var data = await AccesoBD.LoadDataAsync<SelectListItem>(sql);         
            return data;
        }
        public static async Task<List<SelectListItem>> TallaZapatosAsync()

        {
            return await Task.Run(() =>
            {
                string[] tallas = { "37", "37.5", "38", "38.5", "39", "39.5", "40", "40.5", "41", "41.5", "42", "42.5", "43", "43.5", "44", "44.5", "45" };


                List<SelectListItem> Lista = new List<SelectListItem>();

                var query = tallas.Select((r, index) => new SelectListItem { Text = r, Value = r });

                return query.ToList();

            });

        }
        public static async Task<List<SelectListItem>> TallaPantalonAsync()

        {
            return await Task.Run(() =>
            {
                string[] tallas = { "40", "42", "44", "46", "48", "50", "52" };


                List<SelectListItem> Lista = new List<SelectListItem>();

                var query = tallas.Select((r, index) => new SelectListItem { Text = r, Value = r });

                return query.ToList();
            });

        }
        public static async Task<List<SelectListItem>> TallaTop()

        {
            return await Task.Run(() =>
            {
                string[] tallas = { "XS", "S", "M", "L", "XL", "XXL" };


                List<SelectListItem> Lista = new List<SelectListItem>();

                var query = tallas.Select((r, index) => new SelectListItem { Text = r, Value = r });

                return query.ToList();
            });
         


        }
        public static async Task<int> EditarEmpleadoAsync (EmpleadoModel empleado)
        {
            empleado.IDEmpleado = GetRut(empleado.IDEmpleado);
            string sql = @"Update dbo.PersonalServicios Set IDEmpleado = @IDEmpleado, Nombre = @Nombre, Apellido = @Apellido, Email = @Email, Fono = @Fono, Direccion = @Direccion, 
            FechaDeNacimiento = @FechaDeNacimiento, Cargo = @Cargo, IDEspecialidad = @IDEspecialidad, IDSupervisor = @IDSupervisor, OtrosDetalles = @OtrosDetalles, 
            TallaPantalon = @TallaPantalon, TallaTop = @TallaTop, TallaZapatos = @TallaZapatos, EditadoEn = @EditadoEn, EditadoPor = @EditadoPor, ValorHH = @ValorHH, FechaContratacion = @FechaContratacion, SueldoBase = @SueldoBase, Planta = @Planta 
            where ID = @ID";
            return await AccesoBD.Comando(sql, empleado);
        }
        public static async Task<List<EmpleadoModel>> FiltroEmpleadoAsync(string id)
        {
            EmpleadoModel data = new EmpleadoModel
            {
                IDEmpleado = id
            };
            string sql = "Select PersonalServicios.ID, IDEmpleado, Nombre, Apellido , Especialidades.Especialidad Especialidad from PersonalServicios inner join Especialidades on PersonalServicios.IDEspecialidad = Especialidades.ID where IDEmpleado like '%'+ @IDEmpleado + '%' or Nombre like '%'+ @IDEmpleado + '%' or Apellido like '%'+ @IDEmpleado + '%'  ";
            return await AccesoBD.LoadDataAsync(sql, data);

        }
        public static async Task<List<SelectListItem>> ListaEmpleadosAsync()
        {
            List<SelectListItem> Lista = new List<SelectListItem>();
            string sql = "Select ID, Nombre + ' ' + Apellido NombreApellido from PersonalServicios";
            var empleados = await AccesoBD.LoadDataAsync<EmpleadoModel>(sql);
             foreach(var row in empleados)
            {
                SelectListItem item = new SelectListItem
                {
                    Text = row.NombreApellido,
                    Value = row.ID.ToString()
                };

                Lista.Add(item);
            }


            return Lista;


        }
        public static Task<int> ActualizaIDEmpleado(int idempleado, string idusuario)
        {
            return Task.Run(() =>
            {
                return  AccesoBD.Comando("Update PersonalServicios Set IdUsuario = @IDUsuario where ID = @ID", new { IdUsuario = idusuario, ID = idempleado });
            });
            
        }
        public static async Task<string> CorreoHtml(string callbackurl, string usuario)
        {
            return await Task.Run(() =>
            {
                string html = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
            <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" style=""width:100%;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0"">
            <head>
            <meta charset=""UTF-8"">
            <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
            <meta name=""x-apple-disable-message-reformatting"">
            <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
            <meta content=""telephone=no"" name=""format-detection"">
            <title>Nueva plantilla de correo electrónico 2021-04-14</title>
            <!--[if (mso 16)]>
            <style type=""text/css"">
            a {text-decoration: none;}
            </style>
            <![endif]-->
            <!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]-->
            <!--[if gte mso 9]>
            <xml>
            <o:OfficeDocumentSettings>
            <o:AllowPNG></o:AllowPNG>
            <o:PixelsPerInch>96</o:PixelsPerInch>
            </o:OfficeDocumentSettings>
            </xml>
            <![endif]-->
            <!--[if !mso]><!-- -->
            <link href=""https://fonts.googleapis.com/css?family=Open+Sans:400,400i,700,700i"" rel=""stylesheet"">
            <!--<![endif]-->
            <style type=""text/css"">
            #outlook a {
            padding:0;
            }
            .ExternalClass {
            width:100%;
            }
            .ExternalClass,
            .ExternalClass p,
            .ExternalClass span,
            .ExternalClass font,
            .ExternalClass td,
            .ExternalClass div {
            line-height:100%;
            }
            .es-button {
            mso-style-priority:100!important;
            text-decoration:none!important;
            }
            a[x-apple-data-detectors] {
            color:inherit!important;
            text-decoration:none!important;
            font-size:inherit!important;
            font-family:inherit!important;
            font-weight:inherit!important;
            line-height:inherit!important;
            }
            .es-desk-hidden {
            display:none;
            float:left;
            overflow:hidden;
            width:0;
            max-height:0;
            line-height:0;
            mso-hide:all;
            }
            @media only screen and (max-width:600px) {p, ul li, ol li, a { line-height:150%!important } h1 { font-size:32px!important; text-align:center; line-height:120%!important } h2 { font-size:26px!important; text-align:center; line-height:120%!important } h3 { font-size:20px!important; text-align:center; line-height:120%!important } .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a { font-size:32px!important } .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a { font-size:26px!important } .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a { font-size:20px!important } .es-menu td a { font-size:16px!important } .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a { font-size:16px!important } .es-content-body p, .es-content-body ul li, .es-content-body ol li, .es-content-body a { font-size:16px!important } .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a { font-size:16px!important } .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a { font-size:12px!important } *[class=""gmail-fix""] { display:none!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 { text-align:right!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-button-border { display:inline-block!important } a.es-button, button.es-button { font-size:16px!important; display:inline-block!important; border-width:15px 30px 15px 30px!important } .es-btn-fw { border-width:10px 0px!important; text-align:center!important } .es-adaptive table, .es-btn-fw, .es-btn-fw-brdr, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .es-adapt-td { display:block!important; width:100%!important } .adapt-img { width:100%!important; height:auto!important } .es-m-p0 { padding:0px!important } .es-m-p0r { padding-right:0px!important } .es-m-p0l { padding-left:0px!important } .es-m-p0t { padding-top:0px!important } .es-m-p0b { padding-bottom:0!important } .es-m-p20b { padding-bottom:20px!important } .es-mobile-hidden, .es-hidden { display:none!important } tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden { width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } tr.es-desk-hidden { display:table-row!important } table.es-desk-hidden { display:table!important } td.es-desk-menu-hidden { display:table-cell!important } .es-menu td { width:1%!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } table.es-social { display:inline-block!important } table.es-social td { display:inline-block!important } }
            </style>
            </head>
            <body style=""width:100%;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0"">
            <div class=""es-wrapper-color"" style=""background-color:#EEEEEE"">
            <!--[if gte mso 9]>
            <v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t"">
            <v:fill type=""tile"" color=""#eeeeee""></v:fill>
            </v:background>
            <![endif]-->
            <table class=""es-wrapper"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" style=""padding:0;Margin:0"">
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse""></tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" cellspacing=""0"" cellpadding=""0"" align=""center"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-left:10px;padding-right:10px;padding-top:15px;padding-bottom:15px"">
            <!--[if mso]><table style=""width:580px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:282px"" valign=""top""><![endif]-->
            <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;width:282px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td><td style=""width:20px""></td><td style=""width:278px"" valign=""top""><![endif]-->
            <table class=""es-right"" cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;width:278px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td></tr></table><![endif]--></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse""></tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-header-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#044767;width:600px"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#044767"" align=""center"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-top:35px;padding-bottom:35px;padding-left:35px;padding-right:35px"">
            <!--[if mso]><table style=""width:530px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:365px"" valign=""top""><![endif]-->
            <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
            <tr style=""border-collapse:collapse"">
            <td class=""es-m-p0r es-m-p20b"" valign=""top"" align=""center"" style=""padding:0;Margin:0;width:365px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td class=""es-m-txt-c"" align=""left"" style=""padding:0;Margin:0""><h1 style=""Margin:0;line-height:72px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:36px;font-style:normal;font-weight:bold;color:#FFFFFF"">Santa Beatriz</h1></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td><td style=""width:20px""></td><td style=""width:145px"" valign=""top""><![endif]-->
            <table cellspacing=""0"" cellpadding=""0"" align=""right"" class=""es-right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
            <tr class=""es-hidden"" style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;width:145px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;font-size:0px""><img class=""adapt-img"" src=""https://pewulp.stripocdn.email/content/guids/CABINET_0fd4cd8541d8cac1064bd50b6e5f989b/images/41951618448950452.png"" alt style=""display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic"" width=""145""></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td style=""padding:0;Margin:0"">
            <table cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td></tr></table><![endif]--></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;padding-left:35px;padding-right:35px;padding-top:40px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td class=""es-m-txt-l"" align=""left"" style=""padding:0;Margin:0;padding-top:15px""><h3 style=""Margin:0;line-height:22px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:18px;font-style:normal;font-weight:bold;color:#333333"">Hola " + usuario + @",</h3></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:15px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;line-height:24px;color:#777777;font-size:16px"">Su cuenta ha sido creada satisfactoriamente.</p></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;padding-bottom:15px;padding-top:20px;font-size:0"">
            <table width=""100%"" height=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td style=""padding:0;Margin:0;border-bottom:3px solid #EEEEEE;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-top:30px;padding-bottom:35px;padding-left:35px;padding-right:35px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0""><h2 style=""Margin:0;line-height:29px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:24px;font-style:normal;font-weight:bold;color:#333333"">Siguiente Paso</h2></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;padding-top:15px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;line-height:24px;color:#777777;font-size:16px"">Confirme su cuenta pulsando el botón a continuación y siga las instrucciones para crear una nueva cotraseña.</p></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;padding-bottom:15px;padding-top:30px""><span class=""es-button-border"" style=""border-style:solid;border-color:transparent;background:#ED8E20 none repeat scroll 0% 0%;border-width:0px;display:inline-block;border-radius:5px;width:auto""><a href=""" + callbackurl + @""" class=""es-button"" target=""_blank"" style=""mso-style-priority:100 !important;text-decoration:none;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;color:#FFFFFF;font-size:18px;border-style:solid;border-color:#ED8E20;border-width:15px 30px;display:inline-block;background:#ED8E20 none repeat scroll 0% 0%;border-radius:5px;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-weight:normal;font-style:normal;line-height:22px;width:auto;text-align:center"">Confirmar Cuenta</a></span></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table cellpadding=""0"" cellspacing=""0"" class=""es-footer"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-footer-body"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-top:35px;padding-left:35px;padding-right:35px;padding-bottom:40px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;padding-top:15px;padding-left:35px;padding-right:35px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" cellspacing=""0"" cellpadding=""0"" align=""center"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-left:20px;padding-right:20px;padding-top:30px;padding-bottom:30px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            </div>
            </body>
            </html>";
                return html;
            });
      
        }
        public static async Task<string> CorreoHtmlReset(string callbackurl, string usuario)
        {
            return await Task.Run(() =>
            {
                string html = @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
            <html xmlns=""http://www.w3.org/1999/xhtml"" xmlns:o=""urn:schemas-microsoft-com:office:office"" style=""width:100%;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0"">
            <head>
            <meta charset=""UTF-8"">
            <meta content=""width=device-width, initial-scale=1"" name=""viewport"">
            <meta name=""x-apple-disable-message-reformatting"">
            <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
            <meta content=""telephone=no"" name=""format-detection"">
            <title>Nueva plantilla de correo electrónico 2021-04-14</title>
            <!--[if (mso 16)]>
            <style type=""text/css"">
            a {text-decoration: none;}
            </style>
            <![endif]-->
            <!--[if gte mso 9]><style>sup { font-size: 100% !important; }</style><![endif]-->
            <!--[if gte mso 9]>
            <xml>
            <o:OfficeDocumentSettings>
            <o:AllowPNG></o:AllowPNG>
            <o:PixelsPerInch>96</o:PixelsPerInch>
            </o:OfficeDocumentSettings>
            </xml>
            <![endif]-->
            <!--[if !mso]><!-- -->
            <link href=""https://fonts.googleapis.com/css?family=Open+Sans:400,400i,700,700i"" rel=""stylesheet"">
            <!--<![endif]-->
            <style type=""text/css"">
            #outlook a {
            padding:0;
            }
            .ExternalClass {
            width:100%;
            }
            .ExternalClass,
            .ExternalClass p,
            .ExternalClass span,
            .ExternalClass font,
            .ExternalClass td,
            .ExternalClass div {
            line-height:100%;
            }
            .es-button {
            mso-style-priority:100!important;
            text-decoration:none!important;
            }
            a[x-apple-data-detectors] {
            color:inherit!important;
            text-decoration:none!important;
            font-size:inherit!important;
            font-family:inherit!important;
            font-weight:inherit!important;
            line-height:inherit!important;
            }
            .es-desk-hidden {
            display:none;
            float:left;
            overflow:hidden;
            width:0;
            max-height:0;
            line-height:0;
            mso-hide:all;
            }
            @media only screen and (max-width:600px) {p, ul li, ol li, a { line-height:150%!important } h1 { font-size:32px!important; text-align:center; line-height:120%!important } h2 { font-size:26px!important; text-align:center; line-height:120%!important } h3 { font-size:20px!important; text-align:center; line-height:120%!important } .es-header-body h1 a, .es-content-body h1 a, .es-footer-body h1 a { font-size:32px!important } .es-header-body h2 a, .es-content-body h2 a, .es-footer-body h2 a { font-size:26px!important } .es-header-body h3 a, .es-content-body h3 a, .es-footer-body h3 a { font-size:20px!important } .es-menu td a { font-size:16px!important } .es-header-body p, .es-header-body ul li, .es-header-body ol li, .es-header-body a { font-size:16px!important } .es-content-body p, .es-content-body ul li, .es-content-body ol li, .es-content-body a { font-size:16px!important } .es-footer-body p, .es-footer-body ul li, .es-footer-body ol li, .es-footer-body a { font-size:16px!important } .es-infoblock p, .es-infoblock ul li, .es-infoblock ol li, .es-infoblock a { font-size:12px!important } *[class=""gmail-fix""] { display:none!important } .es-m-txt-c, .es-m-txt-c h1, .es-m-txt-c h2, .es-m-txt-c h3 { text-align:center!important } .es-m-txt-r, .es-m-txt-r h1, .es-m-txt-r h2, .es-m-txt-r h3 { text-align:right!important } .es-m-txt-l, .es-m-txt-l h1, .es-m-txt-l h2, .es-m-txt-l h3 { text-align:left!important } .es-m-txt-r img, .es-m-txt-c img, .es-m-txt-l img { display:inline!important } .es-button-border { display:inline-block!important } a.es-button, button.es-button { font-size:16px!important; display:inline-block!important; border-width:15px 30px 15px 30px!important } .es-btn-fw { border-width:10px 0px!important; text-align:center!important } .es-adaptive table, .es-btn-fw, .es-btn-fw-brdr, .es-left, .es-right { width:100%!important } .es-content table, .es-header table, .es-footer table, .es-content, .es-footer, .es-header { width:100%!important; max-width:600px!important } .es-adapt-td { display:block!important; width:100%!important } .adapt-img { width:100%!important; height:auto!important } .es-m-p0 { padding:0px!important } .es-m-p0r { padding-right:0px!important } .es-m-p0l { padding-left:0px!important } .es-m-p0t { padding-top:0px!important } .es-m-p0b { padding-bottom:0!important } .es-m-p20b { padding-bottom:20px!important } .es-mobile-hidden, .es-hidden { display:none!important } tr.es-desk-hidden, td.es-desk-hidden, table.es-desk-hidden { width:auto!important; overflow:visible!important; float:none!important; max-height:inherit!important; line-height:inherit!important } tr.es-desk-hidden { display:table-row!important } table.es-desk-hidden { display:table!important } td.es-desk-menu-hidden { display:table-cell!important } .es-menu td { width:1%!important } table.es-table-not-adapt, .esd-block-html table { width:auto!important } table.es-social { display:inline-block!important } table.es-social td { display:inline-block!important } }
            </style>
            </head>
            <body style=""width:100%;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;-webkit-text-size-adjust:100%;-ms-text-size-adjust:100%;padding:0;Margin:0"">
            <div class=""es-wrapper-color"" style=""background-color:#EEEEEE"">
            <!--[if gte mso 9]>
            <v:background xmlns:v=""urn:schemas-microsoft-com:vml"" fill=""t"">
            <v:fill type=""tile"" color=""#eeeeee""></v:fill>
            </v:background>
            <![endif]-->
            <table class=""es-wrapper"" width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;padding:0;Margin:0;width:100%;height:100%;background-repeat:repeat;background-position:center top"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" style=""padding:0;Margin:0"">
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse""></tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" cellspacing=""0"" cellpadding=""0"" align=""center"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-left:10px;padding-right:10px;padding-top:15px;padding-bottom:15px"">
            <!--[if mso]><table style=""width:580px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:282px"" valign=""top""><![endif]-->
            <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;width:282px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td><td style=""width:20px""></td><td style=""width:278px"" valign=""top""><![endif]-->
            <table class=""es-right"" cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;width:278px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td></tr></table><![endif]--></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse""></tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-header-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#044767;width:600px"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#044767"" align=""center"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-top:35px;padding-bottom:35px;padding-left:35px;padding-right:35px"">
            <!--[if mso]><table style=""width:530px"" cellpadding=""0"" cellspacing=""0""><tr><td style=""width:365px"" valign=""top""><![endif]-->
            <table class=""es-left"" cellspacing=""0"" cellpadding=""0"" align=""left"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:left"">
            <tr style=""border-collapse:collapse"">
            <td class=""es-m-p0r es-m-p20b"" valign=""top"" align=""center"" style=""padding:0;Margin:0;width:365px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td class=""es-m-txt-c"" align=""left"" style=""padding:0;Margin:0""><h1 style=""Margin:0;line-height:72px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:36px;font-style:normal;font-weight:bold;color:#FFFFFF"">Santa Beatriz</h1></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td><td style=""width:20px""></td><td style=""width:145px"" valign=""top""><![endif]-->
            <table cellspacing=""0"" cellpadding=""0"" align=""right"" class=""es-right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;float:right"">
            <tr class=""es-hidden"" style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;width:145px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;font-size:0px""><img class=""adapt-img"" src=""https://pewulp.stripocdn.email/content/guids/CABINET_0fd4cd8541d8cac1064bd50b6e5f989b/images/41951618448950452.png"" alt style=""display:block;border:0;outline:none;text-decoration:none;-ms-interpolation-mode:bicubic"" width=""145""></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td style=""padding:0;Margin:0"">
            <table cellspacing=""0"" cellpadding=""0"" align=""right"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <!--[if mso]></td></tr></table><![endif]--></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;padding-left:35px;padding-right:35px;padding-top:40px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td class=""es-m-txt-l"" align=""left"" style=""padding:0;Margin:0;padding-top:15px""><h3 style=""Margin:0;line-height:22px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:18px;font-style:normal;font-weight:bold;color:#333333"">Hola " + usuario + @",</h3></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;padding-bottom:10px;padding-top:15px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;line-height:24px;color:#777777;font-size:16px"">Ha solicitado restablecer su contraseña.</p></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;padding-bottom:15px;padding-top:20px;font-size:0"">
            <table width=""100%"" height=""100%"" cellspacing=""0"" cellpadding=""0"" border=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td style=""padding:0;Margin:0;border-bottom:3px solid #EEEEEE;background:#FFFFFF none repeat scroll 0% 0%;height:1px;width:100%;margin:0px""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-top:30px;padding-bottom:35px;padding-left:35px;padding-right:35px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" role=""presentation"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0""><h2 style=""Margin:0;line-height:29px;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-size:24px;font-style:normal;font-weight:bold;color:#333333"">Siguiente Paso</h2></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;padding-top:15px""><p style=""Margin:0;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;line-height:24px;color:#777777;font-size:16px"">Restablezca su contraseña pulsando el botón a continuación y siga las instrucciones para crear una nueva cotraseña.</p></td>
            </tr>
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;padding-bottom:15px;padding-top:30px""><span class=""es-button-border"" style=""border-style:solid;border-color:transparent;background:#ED8E20 none repeat scroll 0% 0%;border-width:0px;display:inline-block;border-radius:5px;width:auto""><a href=""" + callbackurl + @""" class=""es-button"" target=""_blank"" style=""mso-style-priority:100 !important;text-decoration:none;-webkit-text-size-adjust:none;-ms-text-size-adjust:none;mso-line-height-rule:exactly;color:#FFFFFF;font-size:18px;border-style:solid;border-color:#ED8E20;border-width:15px 30px;display:inline-block;background:#ED8E20 none repeat scroll 0% 0%;border-radius:5px;font-family:'open sans', 'helvetica neue', helvetica, arial, sans-serif;font-weight:normal;font-style:normal;line-height:22px;width:auto;text-align:center"">Confirmar </a></span></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table cellpadding=""0"" cellspacing=""0"" class=""es-footer"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%;background-color:transparent;background-repeat:repeat;background-position:center top"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-footer-body"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-top:35px;padding-left:35px;padding-right:35px;padding-bottom:40px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" cellspacing=""0"" cellpadding=""0"" bgcolor=""#ffffff"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:#FFFFFF;width:600px"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""padding:0;Margin:0;padding-top:15px;padding-left:35px;padding-right:35px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:530px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            <table class=""es-content"" cellspacing=""0"" cellpadding=""0"" align=""center"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;table-layout:fixed !important;width:100%"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0"">
            <table class=""es-content-body"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px;background-color:transparent;width:600px"" cellspacing=""0"" cellpadding=""0"" align=""center"">
            <tr style=""border-collapse:collapse"">
            <td align=""left"" style=""Margin:0;padding-left:20px;padding-right:20px;padding-top:30px;padding-bottom:30px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td valign=""top"" align=""center"" style=""padding:0;Margin:0;width:560px"">
            <table width=""100%"" cellspacing=""0"" cellpadding=""0"" style=""mso-table-lspace:0pt;mso-table-rspace:0pt;border-collapse:collapse;border-spacing:0px"">
            <tr style=""border-collapse:collapse"">
            <td align=""center"" style=""padding:0;Margin:0;display:none""></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table></td>
            </tr>
            </table>
            </div>
            </body>
            </html>";
                return html;
            });

        }
        public static async Task<List<EmpleadoModel>> getIDRuts()
        {

            return await AccesoBD.LoadDataAsync<EmpleadoModel>("Select ID, IDEmpleado, Nombre, Apellido from PersonalServicios");
        }
        public static  Task<List<SelectListItem>> getMeses()
        {
            return Task.Run(() =>
            {
                List<SelectListItem> meses = new List<SelectListItem>();
                meses.Add(new SelectListItem { Text = "Enero", Value = "1" });
                meses.Add(new SelectListItem { Text = "Febrero", Value = "2" });
                meses.Add(new SelectListItem { Text = "Marzo", Value = "3" });
                meses.Add(new SelectListItem { Text = "Abril", Value = "4" });
                meses.Add(new SelectListItem { Text = "Mayo", Value = "5" });
                meses.Add(new SelectListItem { Text = "Junio", Value = "6" });
                meses.Add(new SelectListItem { Text = "Julio", Value = "7" });
                meses.Add(new SelectListItem { Text = "Agosto", Value = "8" });
                meses.Add(new SelectListItem { Text = "Septiembre", Value = "9" });
                meses.Add(new SelectListItem { Text = "Octubre", Value = "10" });
                meses.Add(new SelectListItem { Text = "Noviembre", Value = "11" });
                meses.Add(new SelectListItem { Text = "Diciembre", Value = "12" });
                return meses;
            });
            
        }
        public static async Task<string> htmlcorreoLiquidacionAsync(string callbackurl, string iddoc, string linkdescarga)
        {
            var empleado = await getempleadoDocAsync(iddoc);
            var mes = await getMesAsync(empleado.Mes);

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
                            
                                <h2>Estimado " + empleado.Empleado + @",</h2>
                                <p>
                                    En este correo podrá encontrar un vínculo para descargar su liquidación de remuneraciones correspondiente al mes de "+ mes +@" del "+ empleado.Ano +@".<br />
                                    Favor haga click en ""Firmar"" para acusar recibo de la misma.<br />
                    
                                    Atentamente,<br />
                                    Administración Santa Beatriz Servicios Industriales<br />
                    
                    
                                </p>
                    
                            <a href="""+ linkdescarga + @""" class=""btna"">Descargar</a>
                            <a href="""+ callbackurl +@""" class=""btnb"">Firmar</a>
                    
                        </div>
                    </body>
                    </html>";
            
        }

        public static async Task<string> htmlcorreoActualizarDatosAsync(string callbackurl, int idempleado)
        {
            var empleado = await LoadEmployeeAsync(idempleado);
            

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
                            
                                <h2>Estimado " + empleado.Nombre + @",</h2>
                                <p>
                                    En este correo podrá encontrar un vínculo hacia un formulario de actualización de datos personales.<br />
                                    Favor dar click en ""Formulario"".<br />
                    
                                    Atentamente,<br />
                                    Administración Santa Beatriz Servicios Industriales<br />
                    
                    
                                </p>
                    
                            <a href=""" + callbackurl + @""" class=""btna"">Formulario</a>
                           
                    
                        </div>
                    </body>
                    </html>";

        }
        public static async Task<string> getUserAsync(int id)
        {

            var data = await AccesoBD.LoadAsync("Select IdUsuario from PersonalServicios where ID = @ID", new EmpleadoModel { ID = id });
            return data.IdUsuario;
        }
        public static async Task<string> getUserAsync(string id)
        {

            var data = await AccesoBD.LoadAsync("Select IdUsuario from PersonalServicios where ID = @IDEmpleado", new EmpleadoModel { IDEmpleado = id });
            return data.IdUsuario;
        }
        public static async Task<string> getIDUsuarioAsync(string rut)
        {

            var data = await AccesoBD.LoadAsync("Select IdUsuario from PersonalServicios where IDEmpleado = @IDEmpleado", new EmpleadoModel { IDEmpleado = rut });
            return data.IdUsuario;
        }
        public static async Task FirmaLiquidacionAsync(FirmaLiquidacionModel model)
        {
            model.Fecha = DateTime.Now;
            await AccesoBD.Comando(@"Update Liquidaciones Set Firma = @Firma, FechaFirma = @Fecha where ID = @ID", model);
        }
        public static async Task<string> getUsuarioAsync(string idusuario)
        {

            var data = await AccesoBD.LoadAsync("Select Nombre, Apellido from PersonalServicios where IdUsuario = @IdUsuario", new EmpleadoModel { IdUsuario = idusuario });

            return data.Nombre + " " + data.Apellido;
        }
        public static async Task<EmpleadoModel> getNombreUsuarioAsync(string idusuario)
        {

            var data = await AccesoBD.LoadAsync("Select Nombre, Apellido from PersonalServicios where IdUsuario = @IdUsuario", new EmpleadoModel { IdUsuario = idusuario });

            return data;
        }
        public static async Task<EmpModel> getempleadoDocAsync(string iddoc)
        {

            return await AccesoBD.LoadAsync(@"Select Nombre + ' ' + Apellido Empleado, Año Ano, Mes from PersonalServicios 
            inner join liquidaciones on PersonalServicios.IdUsuario = Liquidaciones.IDUsuario where Liquidaciones.ID = @ID ", new EmpModel { ID = iddoc });

        }
        public static async Task<string> getMesAsync(string mesint)
        {
           return await Task.Run(() => {
               List<Mes> meses = new List<Mes>();
               meses.Add(new Mes { Nombre = "Enero", Numero = "1" });
               meses.Add(new Mes { Nombre = "Febrero", Numero = "2" });
               meses.Add(new Mes { Nombre = "Marzo", Numero = "3" });
               meses.Add(new Mes { Nombre = "Abril", Numero = "4" });
               meses.Add(new Mes { Nombre = "Mayo", Numero = "5" });
               meses.Add(new Mes { Nombre = "Junio", Numero = "6" });
               meses.Add(new Mes { Nombre = "Julio", Numero = "7" });
               meses.Add(new Mes { Nombre = "Agosto", Numero = "8" });
               meses.Add(new Mes { Nombre = "Septiembre", Numero = "9" });
               meses.Add(new Mes { Nombre = "Octubre", Numero = "10" });
               meses.Add(new Mes { Nombre = "Noviembre", Numero = "11" });
               meses.Add(new Mes { Nombre = "Diciembre", Numero = "12" });
               return meses.Where(x => x.Numero == mesint).FirstOrDefault().Nombre;

           });
           
        }
        public static async Task<List<Mes>> getMesesAsync()
        {
            return await Task.Run(() => {
                List<Mes> meses = new List<Mes>();
                meses.Add(new Mes { Nombre = "Enero", Numero = "1" });
                meses.Add(new Mes { Nombre = "Febrero", Numero = "2" });
                meses.Add(new Mes { Nombre = "Marzo", Numero = "3" });
                meses.Add(new Mes { Nombre = "Abril", Numero = "4" });
                meses.Add(new Mes { Nombre = "Mayo", Numero = "5" });
                meses.Add(new Mes { Nombre = "Junio", Numero = "6" });
                meses.Add(new Mes { Nombre = "Julio", Numero = "7" });
                meses.Add(new Mes { Nombre = "Agosto", Numero = "8" });
                meses.Add(new Mes { Nombre = "Septiembre", Numero = "9" });
                meses.Add(new Mes { Nombre = "Octubre", Numero = "10" });
                meses.Add(new Mes { Nombre = "Noviembre", Numero = "11" });
                meses.Add(new Mes { Nombre = "Diciembre", Numero = "12" });
                return meses;

            });

        }
        public static async Task<List<EmpModel>> getLiquidacionesAsync()
        {

            return await AccesoBD.LoadDataAsync<EmpModel>(@"Select Liquidaciones.ID, Liquidaciones.Mes, Liquidaciones.Año Ano, 
                                                Liquidaciones.FechaEnvio, 
                                                Liquidaciones.Firma, FechaFirma, FechaSubida,
                                                PersonalServicios.Nombre +' '+ PersonalServicios.Apellido Empleado, FechaFirma, PersonalServicios.IdUsuario
                                                from Liquidaciones inner join PersonalServicios on Liquidaciones.IDUsuario = PersonalServicios.IdUsuario order by Mes, Empleado");
        }
        public static async Task<List<EmpModel>> getLiquidacionesMesAsync(string mes, string año)
        {

            return await AccesoBD.LoadDataAsync(@"Select Liquidaciones.Firma from Liquidaciones inner join 
            PersonalServicios on Liquidaciones.IDUsuario = PersonalServicios.IdUsuario 
            Where Mes = @Mes and Año = @Ano and Firma is not null and PersonalServicios.Planta = 'True'", new EmpModel {Mes = mes, Ano = año });
        }
        public static async Task updateFchaEnvioAsync(string iddoc)
        {
            await AccesoBD.Comando("Update Liquidaciones set FechaEnvio = @FechaEnvio where ID = @ID", new { ID = iddoc, FechaEnvio = DateTime.Now });
            
        }
        public static async Task<string> firmadoAsync(string iddoc)
        {

            var data = await AccesoBD.LoadAsync(@"select case when firma is not null then 'true' else 'false' end as Firmado  
            from Liquidaciones where ID = @ID", new EmpModel { ID = iddoc });
            return data.Firmado;
        }
        public static async Task deleteLiquidacionAsync(string iddoc)
        {

            await AccesoBD.Comando("Delete from Liquidaciones where ID = @ID", new { ID = iddoc });
        }
        public static async Task<EmpModel> empleadoLiquidacionAsync(string iddoc)
        {

            return await AccesoBD.LoadAsync(@"select PersonalServicios.Nombre +' '+ PersonalServicios.Apellido Empleado, IDEmpleado, PersonalServicios.ID IDEmpleadoInt,
            Mes, Año Ano, Liquidaciones.ID from PersonalServicios
            inner join Liquidaciones on Liquidaciones.IDUsuario = PersonalServicios.IdUsuario
            where Liquidaciones.ID = @ID", new EmpModel { ID = iddoc });
        }
        public static async Task firmarLiquidacion(string iddoc)
        {

            var empleado = await empleadoLiquidacionAsync(iddoc);
            var src = await CargarLiquidacionAsync(iddoc, "liquidaciones");
            MemoryStream dest = new MemoryStream();
            PdfDocument pdfDoc = new PdfDocument(new PdfReader(src), new PdfWriter(dest));
            PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDoc, true);
            var fields = form.GetFormFields();
            var t = new MyLocationTextExtractionStrategy();           
            var ex = PdfTextExtractor.GetTextFromPage(pdfDoc.GetFirstPage(), t);
            
            var punto = t.myPoints2.Where(x => x.Text == "TrabajadorRecibí").FirstOrDefault();
            if (punto == null)
            {
                punto = t.myPoints2.Where(x => x.Text == "Recibí").FirstOrDefault();
            }
            Document doc = new Document(pdfDoc);           
            PdfFormXObject template = new PdfFormXObject(new Rectangle(punto.Rect.GetX() -180, punto.Rect.GetY() -690, 300, 150));
            Canvas templateCanvas = new Canvas(template, pdfDoc);
            doc.Add(new Image(template));
            String textLine = empleado.Empleado + " " + FormatRutView(empleado.IDEmpleado);
            templateCanvas.Add(new Paragraph(textLine).SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));            
            templateCanvas.Close();
            doc.Close();
            
            await subirLiquidacionFirmadaAsync(dest.ToArray(),empleado);



        }
        public static async Task<List<SelectListItem>> getDistinctMesLiquidaciones()
        {

            return await Task.Run(async () => { return await AccesoBD.LoadDataAsync<SelectListItem>("Select distinct Mes Value from Liquidaciones"); });
        }
        public static async Task<List<SelectListItem>> getDistinctAñoLiquidaciones()
        {

            return await Task.Run(async () => { return await AccesoBD.LoadDataAsync<SelectListItem>("Select distinct Año Text, Año Value from Liquidaciones"); });
        }
        public static async Task<List<EmpleadoModel>> getEmpleadosValorHHCuentaTareaAsync(int idceco)
        {
          return await  Task.Run(async () =>
            {
               return await AccesoBD.LoadDataAsync(@"Select PersonalServicios.ID, PersonalServicios.ValorHH, Cuentas.ID IDCuentaEmpleado, Tareas.IDEncargado IDSupervisor
                from PersonalServicios inner join ProgramaServicios on PersonalServicios.ID = ProgramaServicios.IDEmpleado 
                inner join CECOs on ProgramaServicios.IDTarea = CECOs.IDTarea
                inner join Cuentas on PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido = Cuentas.Nombre
                inner join Tareas on Tareas.ID = ProgramaServicios.IDTarea
                where CECOs.ID = @ID and Cuentas.IDTipo = 4", new EmpleadoModel { ID = idceco });
            });
            
        }
        public static async Task<EmpleadoModel> getCuentaPagosAsync(int idempleado)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadAsync(@"Select CuentaPagos from PersonalServicios where ID = @ID", new EmpleadoModel { ID = idempleado });
            });
        }
        public static async Task<decimal> getEmpleadoValorHHAsync(int idempleado)
        {
            return await Task.Run(async () => { 
                var data = await AccesoBD.LoadAsync(@"Select ValorHH from PersonalServicios where ID = @ID", new EmpleadoModel { ID = idempleado });
                return data.ValorHH;
            });
           
        }
        public static async Task<decimal> ValorHoraExtraAsync(int idempleado)
        {
            var data = await AccesoBD.LoadAsync("Select SueldoBase from PersonalServicios Where ID = @ID", new EmpleadoModel { ID = idempleado });
            var sueldobase = data.SueldoBase;
            var costohoraextra = ((sueldobase / 30) * 28) / 180;
            return costohoraextra;
        }
        public static async Task<List<SelectListItem>> getSupervisoresAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync<SelectListItem>(@"Select PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Text, PersonalServicios.ID Value
            from PersonalServicios
            inner join Especialidades on PersonalServicios.IDEspecialidad = Especialidades.ID
            where Especialidades.Especialidad = 'SUPERVISOR'");
            });
            
        }

        public static async Task<List<SelectListItem>> getPaisesAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync<SelectListItem>("Select ID Value, Pais Text from Paises Order by Orden Asc");
            
            });
            
        }
        public static async Task<List<SelectListItem>> getBancosAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync<SelectListItem>("Select ID Value, Banco Text from Bancos");

            });

        }
        public static async Task<List<SelectListItem>> getEstadosCivilAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync<SelectListItem>("Select ID Value, EstadoCivil Text from EstadosCivil");

            });

        }
        public static async Task<List<SelectListItem>> getTiposLicenciaAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync<SelectListItem>("Select ID Value, TipoLicencia Text from TiposLicencia");

            });

        }
        public static async Task<List<SelectListItem>> getTiposCuentaBancariaAsync()
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync<SelectListItem>("Select ID Value, Tipo Text from TipoCuentaBancaria");

            });

        }
        public static async Task nuevaCuentaBancariaAsync(CuentaBancariaModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando(@"Insert into CuentasBancarias (ID,IDEmpleado,IDBanco,IDTipoCuenta,NumCuenta,CreadoEn) 
                                        Values(NEWID(),@IDEmpleado,@IDBanco,@IDTipoCuenta,@NumCuenta,GETDATE())", modelo);
            
            });
        }
        public static async Task actualizarCuentaBancariaAsync(CuentaBancariaModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando(@"Update CuentasBancarias Set IDBanco = @IDBanco, IDTipoCuenta = @IDTipoCuenta, NumCuenta = @NumCuenta, EditadoEn = GETDATE() where IDEmpleado = @IDEmpleado", modelo);

            });
        }
        public static async Task<bool> cuentaExisteAsync(int idempleado)
        {
            return await Task.Run(async () => {
                var cuenta = await AccesoBD.LoadAsync("Select COUNT(CuentasBancarias.ID) Cuenta from CuentasBancarias where IDEmpleado = @IDEmpleado", new CuentaBancariaModel { IDEmpleado = idempleado });
                bool resultado;
                if (cuenta.Cuenta == 0)
                {
                    resultado = false;
                }
                else
                {
                    resultado = true;
                }
                return resultado;
            });
            
        }
        public static async Task<CuentaBancariaModel> getCuentaEmpleadoAsync(int idempleado)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync("Select ID, IDEmpleado, IDBanco, IDTipoCuenta, NumCuenta from CuentasBancarias where IDEmpleado = @IDEmpleado", new CuentaBancariaModel { IDEmpleado = idempleado });
            });
           
        }
        public static async Task<EmpleadoModel> getCodEmpleadoAsync(int idempleado)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync("Select Cod from PersonalServicios where ID = @ID", new EmpleadoModel {ID = idempleado });
            });
            
        }
        public static async Task actualizarDatosPersonalesAsync(EmpleadoModel modelo)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando(@"Update PersonalServicios Set FechaDeNacimiento = @FechaDeNacimiento, IDEstadoCivil = @IDEstadoCivil, 
            IDNacionalidad = @IDNacionalidad, IDTipoLicencia = @IDTipoLicencia, Email = @Email, Fono = @Fono, Direccion = @Direccion, TallaZapatos = @TallaZapatos,
            TallaPantalon = @TallaPantalon, TallaTop = @TallaTop, EditadoEn = GETDATE() where ID = @ID", modelo);
            
            });
            
        }
        public static async Task onoffActualizadoAsync(EmpleadoModel empleado)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando("Update PersonalServicios Set Actualizado = @Actualizado where ID = @ID",empleado);
            });
            
        }
        public static async Task<EmpleadoModel> getEstadoActualizadoAsync(int idempleado)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync("Select Actualizado from PersonalServicios where ID = @ID", new EmpleadoModel { ID = idempleado });
            });
           
        }
        public static async Task actualizacionFechaCorreoAsync(int idempleado)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando("Update PersonalServicios Set FechaCorreoAct = GETDATE() where ID = @ID", new EmpleadoModel { ID = idempleado });
            });
            
        }

        public static async Task<EmpleadoModel> getEmployeeDataAsync(int idempleado)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadAsync(@"Select empleado.Nombre, empleado.Apellido, empleado.IDEmpleado, empleado.FechaDeNacimiento, empleado.FechaContratacion,
             empleado.Direccion, EstadosCivil.EstadoCivil, Paises.Pais Nacionalidad, TiposLicencia.TipoLicencia, empleado.Fono, empleado.Email, empleado.Cargo,
             Especialidades.Especialidad, supervisor.Nombre + ' ' + supervisor.Apellido Supervisor, Bancos.Banco, TipoCuentaBancaria.Tipo TipoCuentaBancaria, CuentasBancarias.NumCuenta 
            from PersonalServicios as empleado inner join PersonalServicios as supervisor on empleado.IDSupervisor = supervisor.iD
		    inner join	EstadosCivil on empleado.IDEstadoCivil = EstadosCivil.ID 
			inner join Paises on empleado.IDNacionalidad = Paises.ID
			inner join TiposLicencia on empleado.IDTipoLicencia = TiposLicencia.ID
			inner join Especialidades on empleado.IDEspecialidad = Especialidades.ID
			inner join CuentasBancarias on empleado.ID = CuentasBancarias.IDEmpleado
			inner join TipoCuentaBancaria on CuentasBancarias.IDTipoCuenta = TipoCuentaBancaria.ID
			inner join Bancos on Bancos.ID = CuentasBancarias.IDBanco
			where empleado.ID = @ID", new EmpleadoModel { ID = idempleado });
            });
            
        }
        public static Stream getFormActualizacionDatos(EmpleadoModel modelo)
        {

            var formato = HttpContext.Current.Server.MapPath("~/Content/Formatos/FomularioInfoPersonal.pdf");


            byte[] result;
            using (var memoryStream = new MemoryStream())
            {



                //var SourceDocument = new PdfDocument(new PdfReader(formato));
                var pdfWriter = new PdfWriter(memoryStream);
                var pdfDocument = new PdfDocument(new PdfReader(formato), pdfWriter);
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);
                form.SetGenerateAppearance(true);

                //PdfFont font = PdfFontFactory.CreateFont(FontFamily.GenericSerif.Name, PdfEncodings.IDENTITY_H);
                form.GetField("FechaHoy").SetValue(DateTime.Now.ToString("dd-MM-yyyy"));
                form.GetField("Rut").SetValue(FormatRutView(modelo.IDEmpleado));
                form.GetField("FNacimiento").SetValue(modelo.FechadeNacimiento.ToString("dd-MM-yyyy"));
                form.GetField("FContratacion").SetValue(modelo.FechaContratacion.ToString("dd-MM-yyyy"));
                form.GetField("Nombre").SetValue(modelo.Nombre + " " + modelo.Apellido);
                form.GetField("Domicilio").SetValue(modelo.Direccion);
                form.GetField("ECivil").SetValue(modelo.EstadoCivil);
                form.GetField("Nacionalidad").SetValue(modelo.Nacionalidad);
                form.GetField("Licencia").SetValue(modelo.TipoLicencia);
                form.GetField("Fono").SetValue(modelo.Fono);
                form.GetField("Email").SetValue(modelo.Email);
                form.GetField("Cargo").SetValue(modelo.Cargo);
                form.GetField("Especialidad").SetValue(modelo.Especialidad);
                form.GetField("Supervisor").SetValue(modelo.Supervisor);
                form.GetField("Banco").SetValue(modelo.Banco);
                form.GetField("TipoCuenta").SetValue(modelo.TipoCuentaBancaria);
                form.GetField("NCuenta").SetValue(modelo.NumCuenta);

                //memoryStream.Position = 0;
                pdfDocument.Close();

                result = memoryStream.ToArray();



            }
            return new MemoryStream(result);



        }
    }
    public class MyLocationTextExtractionStrategy : LocationTextExtractionStrategy
    {
        //Hold each coordinate
        public List<RectAndText> myPoints = new List<RectAndText>();
        public List<RectAndText> myPoints2 = new List<RectAndText>();
        public string palabra { get; set; }
        //Automatically called for each chunk of text in the PDF
        public override void EventOccurred(IEventData data, EventType type)
        {
            if (!type.Equals(EventType.RENDER_TEXT))
                return;

            var renderInfo = (TextRenderInfo)data;
            var text = renderInfo.GetCharacterRenderInfos();                     
            
            //Get the bounding box for the chunk of text
            var bottomLeft = renderInfo.GetDescentLine().GetStartPoint();
            var topRight = renderInfo.GetAscentLine().GetEndPoint();

            //Create a rectangle from it
            var rect = new Rectangle(bottomLeft.Get(Vector.I1), bottomLeft.Get(Vector.I2),topRight.Get(Vector.I1),topRight.Get(Vector.I2));

            //Add this to our main collection
            this.myPoints.Add(new RectAndText(rect, renderInfo.GetText()));
            if(renderInfo.GetText()!=" ")
            {

                palabra = palabra + renderInfo.GetText();
            }
            else
            {
                this.myPoints2.Add(new RectAndText(rect, palabra));
                palabra = "";
            }
        }
    }
    public class RectAndText
    {
        public Rectangle Rect;
        public String Text;
        public RectAndText(Rectangle rect, String text)
        {
            this.Rect = rect;
            this.Text = text;
        }
    }
}
