using BibliotecaDeClases.Modelos.Empleados;
using iText.Forms;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BibliotecaDeClases.Logica.Personal
{
  public class ProcesadorVacaciones
    {
        public static async Task insertarRegistroVacacionesAsync(RegistroVacacionesModel modelo)
        {
            await Task.Run(async () =>
            {
                modelo.Comentarios = modelo.Comentarios.ToUpperCheckForNull();
                await AccesoBD.Comando(@"Insert into RegistroVacaciones (IDEmpleado, FechaInicial, FechaFinal, Dias, Comentarios, CreadoPor, CreadoEn, EditadoPor, EditadoEn, PeriodoInicio, PeriodoTermino, Inhabiles)
                                     Values (@IDEmpleado, @FechaInicial, @FechaFinal, @Dias, @Comentarios, @CreadoPor, @CreadoEn, @EditadoPor, @EditadoEn, @PeriodoInicio, @PeriodoTermino, @Inhabiles)", modelo);
            });            
        }
        public static async Task editarRegistroVacacionesAsync(RegistroVacacionesModel modelo)
        {
            await Task.Run(async () =>
            {
                modelo.Comentarios = modelo.Comentarios.ToUpperCheckForNull();
                await AccesoBD.Comando(@"Update RegistroVacaciones Set FechaInicial = @FechaInicial, FechaFinal = @FechaFinal, Dias = @Dias, Comentarios = @Comentarios, 
                EditadoPor = @EditadoPor, EditadoEn = @EditadoEn Where ID = @ID", modelo);
            });
        }
        public static async Task<RegistroVacacionesModel> getRegistroVacacionesAsync(int id)
        {
           return await Task.Run(async () =>
            {
              return await AccesoBD.LoadAsync(@"Select PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, 
                RegistroVacaciones.IDEmpleado, FechaInicial, 
                FechaFinal, Dias, Comentarios, PeriodoInicio, PeriodoTermino, Inhabiles, 
                (DATEDIFF(month,personalservicios.fechacontratacion,GETDATE())*1.25)-[dbo].[DiasVacaciones] (PersonalServicios.ID) DiasDisponibles  
                from RegistroVacaciones 
                inner join PersonalServicios on RegistroVacaciones.IDEmpleado = PersonalServicios.ID 
                Where RegistroVacaciones.ID = @ID", new RegistroVacacionesModel {ID = id });
            });
        }
        public static async Task<List<RegistroVacacionesModel>> getRegistroVacacionesAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync<RegistroVacacionesModel>(@"Select PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, 
                RegistroVacaciones.IDEmpleado, FechaInicial, FechaFinal, Dias, Comentarios,  
                from RegistroVacaciones inner join PersonalServicios on RegistroVacaciones.IDEmpleado = PersonalServicios.ID Where ID = @ID");
            });
        }
        public static async Task deleteRegistroVacacionesAsync(int id)
        {
            await Task.Run(async () => {
                await AccesoBD.Comando("Delete from RegistroVacaciones where ID = @ID", new { ID = id });
            });
            
        }
        public static async Task<RegistroVacacionesModel> getDiasDisponiblesAsync(int idempleado)
        {
         return await Task.Run(async () => {
             return await AccesoBD.LoadAsync(@"Select (DATEDIFF(month,personalservicios.fechacontratacion,GETDATE())*1.25)-[dbo].[DiasVacaciones] (PersonalServicios.ID) DiasDisponibles, ID IDEmpleado  
            from PersonalServicios where PersonalServicios.ID = @IDEmpleado", new RegistroVacacionesModel { IDEmpleado = idempleado });
         });
        
        }
        public static async Task<List<PeriodosModel>> getPeriodosAsync(int idempleado)
        {
            return await Task.Run(async () => {
                return await AccesoBD.LoadDataAsync(@"
                DECLARE @start_date DATETIME;
                DECLARE @end_date DATETIME ;
                            
                Select @start_date = PersonalServicios.FechaContratacion from PersonalServicios where PersonalServicios.ID = @ID;
                Select @end_date =   DATEFROMPARTS(DATEPART(YEAR,GETDATE()), DATEPART(MONTH,@start_date), DATEPART(DAY,@start_date));    
                WITH AllDays
                          AS(SELECT   @start_date AS[Date], 1 AS[level]
                               UNION ALL
                               SELECT   DATEADD(YEAR, 1, [Date]), [level] + 1
                               FROM     AllDays
                               WHERE[Date] < @end_date)
                     SELECT[Date] PeriodoInicio, DATEADD(YEAR,1,[Date]) PeriodoTermino
                    FROM   AllDays where [Date] < GETDATE() OPTION(MAXRECURSION 0)", new PeriodosModel { ID = idempleado });
            
            });
            
        }
        public static async Task<List<RegistroVacacionesModel>> getDiasDisponiblesAsync()
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync<RegistroVacacionesModel>(@"Select ID IDEmpleado, PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, 
                FechaContratacion, (DATEDIFF(month,personalservicios.fechacontratacion,GETDATE())*1.25)-[dbo].[DiasVacaciones] (PersonalServicios.ID) DiasDisponibles
                from PersonalServicios where FechaContratacion is not null
                Order by PersonalServicios.Nombre");
            });
        }

        public static async Task<List<RegistroVacacionesModel>> getRegistroVacacionesEmpleadoAsync(int idempleado)
        {
            return await Task.Run(async () =>
            {
                return await AccesoBD.LoadDataAsync(@"Select PersonalServicios.Nombre + ' ' + PersonalServicios.Apellido Empleado, RegistroVacaciones.IDEmpleado, FechaInicial, 
                FechaFinal, Dias, Comentarios, RegistroVacaciones.ID ID, RegistroVacaciones.PeriodoInicio, RegistroVacaciones.PeriodoTermino from RegistroVacaciones inner join PersonalServicios on RegistroVacaciones.IDEmpleado = PersonalServicios.ID Where PersonalServicios.ID = @ID", new RegistroVacacionesModel { ID = idempleado });
            });
        }
        //public static readonly String FONT = "../../../resources/font/FreeSans.ttf";
        public static Stream getComprobanteAsync(RegistroVacacionesModel modelo)
        {
            var formato = HttpContext.Current.Server.MapPath("~/Content/Formatos/ComprobanteFeriado.pdf");


            byte[] result;
            using (var memoryStream = new MemoryStream())
            {



                //var SourceDocument = new PdfDocument(new PdfReader(formato));
                var pdfWriter = new PdfWriter(memoryStream);
                var pdfDocument = new PdfDocument(new PdfReader(formato), pdfWriter);
                PdfAcroForm form = PdfAcroForm.GetAcroForm(pdfDocument, true);
                form.SetGenerateAppearance(true);

                //PdfFont font = PdfFontFactory.CreateFont(FontFamily.GenericSerif.Name, PdfEncodings.IDENTITY_H);
                form.GetField("FechaActual").SetValue(DateTime.Now.ToString("dd-MM-yyyy"));
                form.GetField("PContractualInicial").SetValue(modelo.PeriodoInicio.ToString("dd-MM-yyyy"));
                form.GetField("PContractualFinal").SetValue(modelo.PeriodoTermino.ToString("dd-MM-yyyy"));
                form.GetField("Empleado").SetValue(modelo.Empleado);
                form.GetField("FechaInicial").SetValue(modelo.FechaInicial.ToString("dd-MM-yyyy"));
                form.GetField("FechaFinal").SetValue(modelo.FechaFinal.ToString("dd-MM-yyyy"));
                form.GetField("DiasHabiles").SetValue(modelo.Dias.ToString("0.##"));
                form.GetField("DiasInhabiles").SetValue(modelo.Inhabiles.ToString("0.##"));
                form.GetField("Saldo").SetValue(modelo.DiasDisponibles.ToString("0.##"));
                //memoryStream.Position = 0;
                pdfDocument.Close();

                result = memoryStream.ToArray();



            }
            return new MemoryStream(result);

        
           
        }
    
    
    }
}
