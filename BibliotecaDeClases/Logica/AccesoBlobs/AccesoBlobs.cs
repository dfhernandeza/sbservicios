using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BibliotecaDeClases.Modelos.Empleados;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Parser;
using System.Text.RegularExpressions;
using static BibliotecaDeClases.ProcesadorEmpleados;
using iText.Kernel.Utils;
using iText.IO.Source;
using iText.Layout;

using static BibliotecaDeClases.Logica.Transacciones.ProcesadorTransacciones;
using BibliotecaDeClases.Modelos.ServicioAutomotriz;

namespace BibliotecaDeClases.Logica.AccesoBlobs
{
    public class AccesoBlobs
    {
        public static List<byte[]> Datos = new List<byte[]>();

        public static string GetConnectionString(string connectionName = "AzureBlobs")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        public static async Task SubirArchivo(string container, EmpModel modelo)
        {

            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), container);

            Dictionary<string, string> tags = new Dictionary<string, string>
          {

            { "IDEmpleado", modelo.IDEmpleado.ToString() },
              { "Mes", modelo.Mes.ToString() },
              { "Ano", modelo.Ano.ToString() }
          };
            string nombre = modelo.IDEmpleado.ToString() + modelo.Ano.ToString() + modelo.Mes.ToString() + ".pdf";
            await contenedor.UploadBlobAsync(nombre, modelo.Documento.InputStream);

            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            appendBlobWithTags.SetTags(tags);



        }
        public static async Task SubirArchivos(string container, EmpModel modelo)
        {

            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), container);
            var id = Guid.NewGuid().ToString();
            Dictionary<string, string> tags = new Dictionary<string, string>
            {
              {"ID",id },
              { "IDEmpleado", modelo.IDEmpleado.ToString() },
              { "Mes", modelo.Mes.ToString() },
              { "Ano", modelo.Ano.ToString() }
            };
            string nombre = modelo.IDEmpleado.ToString() + modelo.Ano.ToString() + modelo.Mes.ToString() + ".pdf";
            await contenedor.UploadBlobAsync(nombre, modelo.Elemento);

           //var output = await CrearDocumento(
           //     new Modelos.Transacciones.DocumentoModel 
           //     { 
           //         IDDocumento = id, 
           //         FechaDocumento = new DateTime(int.Parse(modelo.Ano), int.Parse(modelo.Mes), 5), 
           //         Descripcion = "Liquidación de sueldo", 
           //         IDTipo = 3, 
           //         IDEmisor = "1", 
           //         CreadoPor = modelo.CreadoPor, 
           //         EditadoPor = modelo.EditadoPor, 
           //         CreadoEn = DateTime.Now, 
           //         EditadoEn = DateTime.Now 
           //     });
            
            //await ingresarTransaccionAsync(
            //    new Modelos.Costos.TransaccionModel
            //    {
            //        IDDocumento = output.ID,
            //        Item = "Remuneración " + modelo.Empleado,
            //        Monto = remuneracion,
            //        Cantidad = 1,
            //        Fecha = new DateTime(int.Parse(modelo.Ano), int.Parse(modelo.Mes), 5),
            //        IDCuentaCR = 10,
            //        IDCuentaDB = 9,
            //        CreadoEn = DateTime.Now,
            //        EditadoEn = DateTime.Now,
            //        CreadoPor = modelo.CreadoPor,
            //        EditadoPor = modelo.EditadoPor
            //    });




            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            
            appendBlobWithTags.SetTags(tags);
            var idusuario = await getUserAsync(modelo.IDEmpleado);
            await AccesoBD.Comando("Insert into Liquidaciones (ID,Mes,Año,IDUsuario,FechaSubida) Values(@ID,@Mes,@Año,@IDUsuario,@FechaSubida)",
                new { ID = id, Mes = modelo.Mes, Año = modelo.Ano, IDUsuario = idusuario, FechaSubida = DateTime.Now });


        }
        public static void Subir(string container, EmpModel modelo)
        {

            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), container);

            Dictionary<string, string> tags = new Dictionary<string, string>
          {

            { "IDEmpleado", modelo.IDEmpleado.ToString() },
              { "Mes", modelo.Mes.ToString() },
              { "Ano", modelo.Ano.ToString() }
          };
            string nombre = modelo.IDEmpleado.ToString() + modelo.Ano.ToString() + modelo.Mes.ToString() + ".pdf";
            contenedor.UploadBlob(nombre, modelo.Elemento);

            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            appendBlobWithTags.SetTags(tags);



        }
        public static async Task<Stream> CargarLiquidacionAsync(string iddoc, string container)
        {
            BlobServiceClient serviceClient = new BlobServiceClient(GetConnectionString());
            string id = @"""ID"" = '" + iddoc + "'";

            String query = id;
            var filename = serviceClient.FindBlobsByTags(query).FirstOrDefault().BlobName;
            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), container);
            BlobClient blobClient = contenedor.GetBlobClient(filename);
            BlobDownloadInfo download = await blobClient.DownloadAsync();

            return download.Content;
        }
        public static List<EmpModel> cargaLiquidacionesEmpleado(int idempleado)
        {
            BlobServiceClient serviceClient = new BlobServiceClient(GetConnectionString());
            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), "liquidaciones");
            var blobs = contenedor.GetBlobs(BlobTraits.All).ToList();
            List<EmpModel> lista = new List<EmpModel>();
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
            foreach (var item in blobs)
            {
                var id = item.Tags.Where(x => x.Key == "IDEmpleado").FirstOrDefault().Value;
                var mes = item.Tags.Where(x => x.Key == "Mes").FirstOrDefault().Value;
                var ano = item.Tags.Where(x => x.Key == "Ano").FirstOrDefault().Value;
                var mestxt = meses.Where(x => x.Numero.ToString() == mes).FirstOrDefault();
                if (id == idempleado.ToString())
                {
                    lista.Add(new EmpModel { Ano = ano, IDEmpleado = id, Mes = mes, MesTexto = mestxt.Nombre });
                }
            }

            return lista;
        }
        public static List<EmpModel> CargarLiquidaciones(string container)
        {
            List<EmpModel> lista = new List<EmpModel>();
            //BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), container);
            BlobServiceClient serviceClient = new BlobServiceClient(GetConnectionString());
            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), container);
            var blobs = contenedor.GetBlobs(BlobTraits.All);

            foreach (var row in blobs)
            {

                var idempleado = row.Tags.Where(x => x.Key == "IDEmpleado").FirstOrDefault().Value;
                var mes = row.Tags.Where(x => x.Key == "Mes").FirstOrDefault().Value;
                var ano = row.Tags.Where(x => x.Key == "Ano").FirstOrDefault().Value;
                lista.Add(new EmpModel { IDEmpleado = idempleado, Ano = ano, Mes = mes });
            }




            return lista;
        }
        public static async Task EliminarArchivo(string iddoc, string container)
        {

            BlobServiceClient serviceClient = new BlobServiceClient(GetConnectionString());
            string id = @"""ID"" = '" + iddoc + "'";

            String query = id;
            var filename = serviceClient.FindBlobsByTags(query).FirstOrDefault().BlobName;
            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), container);
            BlobClient blobClient = contenedor.GetBlobClient(filename);
            await blobClient.DeleteAsync();


        }
        public static async Task subirLiquidaciones(EmpModel modelo)
        {

            PdfReader lector = new PdfReader(modelo.Documento.InputStream);
            iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(lector);
            StringBuilder builder = new StringBuilder();
            //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument(modelo.Documento.InputStream);
            ByteArrayPdfSplitter splitter = new ByteArrayPdfSplitter(pdfDoc);
            splitter.SplitByPageCount(1, new ByteArrayPdfSplitter.DocumentReadyListender(splitter));

            for (int i = 1; i <= pdfDoc.GetNumberOfPages(); i++)
            {


                var page = pdfDoc.GetPage(i);
                string text = PdfTextExtractor.GetTextFromPage(page);
                var texto = text.Substring(140);

                var rutcompleto = getBetween(texto, "R.U.T.:", "Sueldo");
                var rutfinal = Regex.Replace(rutcompleto, @"[^0-9a-zA-Z]+", "");
                var rutid = await getIDRuts();

                if (rutid.Any(x => x.IDEmpleado == rutfinal))
                {
                    var empleado = rutid.Where(x => x.IDEmpleado == rutfinal).FirstOrDefault();
                    var id = empleado.ID;




                    using (Stream ms = new MemoryStream(Datos[i - 1]))
                    {
                        ms.Position = 0;
                        EmpModel liquidacion = new EmpModel
                        {
                            Ano = modelo.Ano,
                            Elemento = ms,
                            IDEmpleado = id.ToString(),
                            Mes = modelo.Mes,
                            TextoPagina = texto,
                            Empleado = empleado.Nombre + " " + empleado.Apellido,
                            CreadoPor = modelo.CreadoPor,
                            EditadoPor = modelo.EditadoPor
                        };
                        await SubirArchivos("liquidaciones", liquidacion);
                    }

                }
            }

        }
        private static string getBetween(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }
        public static async Task subirLiquidacionFirmadaAsync(byte[] archivo, EmpModel emp)
        {

            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), "liquidaciones");
            string nombre = emp.IDEmpleadoInt.ToString() + emp.Ano.ToString() + emp.Mes.ToString() + "firmada.pdf";
            var id = Guid.NewGuid().ToString();
            Dictionary<string, string> tags = new Dictionary<string, string>
          {
                {"ID",id },
            { "IDEmpleado", emp.IDEmpleadoInt.ToString() },
              { "Mes", emp.Mes.ToString() },
              { "Ano", emp.Ano.ToString() }
          };

            MemoryStream ms = new MemoryStream(archivo);
            await contenedor.UploadBlobAsync(nombre, ms);

            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            appendBlobWithTags.SetTags(tags);

            await FirmaLiquidacionAsync(new FirmaLiquidacionModel { Firma = id, ID = emp.ID });

        }
        public static async Task<decimal> getTotalPagar(string texto)
        {
          return await Task.Run(() =>
            {
                var sueldo = getBetween(texto, "PAGAR\n", "\nSon:");
                var final = Regex.Replace(sueldo, "[^0-9]", "");
                decimal valor;
                if (final == "")
                {
                    valor = 0;
                }
                else
                {
                    valor = decimal.Parse(final);
                }
                return valor;
            });
            
        }
        public static async Task<Stream> getLiquidacionesMergedMes(string mes, string año)
        {
            var documentos = new List<Stream>();
           var liquidaciones = await getLiquidacionesMesAsync(mes, año);
            foreach (var item in liquidaciones)
            {
                var data = await CargarLiquidacionAsync(item.Firma, "liquidaciones");
                documentos.Add(data);
            }

            var docs = documentos.ToArray();
            var SourceDocument1 = new PdfDocument(new PdfReader(docs[0]));
            var SourceDocument2 = new PdfDocument(new PdfReader(docs[1]));
            byte[] result;
                using (var memoryStream = new MemoryStream())
                {
                    var pdfWriter = new PdfWriter(memoryStream);
                    var pdfDocument = new PdfDocument(pdfWriter);
                    PdfMerger merge = new PdfMerger(pdfDocument);
                    merge.Merge(SourceDocument1, 1, SourceDocument1.GetNumberOfPages())
                        .Merge(SourceDocument2, 1, SourceDocument2.GetNumberOfPages());

                    merge.Close();
                    result = memoryStream.ToArray();
                }

            for (int i = 2; i < docs.Length; i++)
            {
                 SourceDocument1 = new PdfDocument(new PdfReader(new MemoryStream(result) ));
                 SourceDocument2 = new PdfDocument(new PdfReader(docs[i]));

                using (var memoryStream = new MemoryStream())
                {
                    var pdfWriter = new PdfWriter(memoryStream);
                    var pdfDocument = new PdfDocument(pdfWriter);
                    PdfMerger merge = new PdfMerger(pdfDocument);
                    merge.Merge(SourceDocument1, 1, SourceDocument1.GetNumberOfPages())
                        .Merge(SourceDocument2, 1, SourceDocument2.GetNumberOfPages());

                    merge.Close();
                    result = memoryStream.ToArray();
                }
            }
            return new MemoryStream(result);
        }
        public static async Task subirLiquidacion(EmpModel modelo)
        {

            PdfReader lector = new PdfReader(modelo.Documento.InputStream);
            iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(lector);
            StringBuilder builder = new StringBuilder();
            //Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument(modelo.Documento.InputStream);
            //ByteArrayPdfSplitter splitter = new ByteArrayPdfSplitter(pdfDoc);
            //splitter.SplitByPageCount(1, new ByteArrayPdfSplitter.DocumentReadyListender(splitter));

           

                var page = pdfDoc.GetPage(1);
                string text = PdfTextExtractor.GetTextFromPage(page);
                var texto = text.Substring(140);
            pdfDoc.Close();
            var rutcompleto = getBetween(texto, "R.U.T.:", "Sueldo");
                var rutfinal = Regex.Replace(rutcompleto, @"[^0-9a-zA-Z]+", "");
                var rutid = await getIDRuts();
                
                if (rutid.Any(x => x.IDEmpleado == rutfinal))
                {
                    var empleado = rutid.Where(x => x.IDEmpleado == rutfinal).FirstOrDefault();
                    var id = empleado.ID;

                modelo.Documento.InputStream.Position = 0;
                        EmpModel liquidacion = new EmpModel
                        {
                            Ano = modelo.Ano,
                            Elemento = modelo.Documento.InputStream,
                            IDEmpleado = id.ToString(),
                            Mes = modelo.Mes,
                            TextoPagina = texto,
                            Empleado = empleado.Nombre + " " + empleado.Apellido,
                            CreadoPor = modelo.CreadoPor,
                            EditadoPor = modelo.EditadoPor
                        };
                        await SubirArchivos("liquidaciones", liquidacion);
                   

                }
           

        }
    }

    class ByteArrayPdfSplitter : PdfSplitter
    {

        private MemoryStream currentOutputStream;


        public ByteArrayPdfSplitter(iText.Kernel.Pdf.PdfDocument pdfDocument) : base(pdfDocument)
        {
        }

        protected override PdfWriter GetNextPdfWriter(PageRange documentPageRange)
        {
            currentOutputStream = new MemoryStream();
            return new PdfWriter(currentOutputStream);
        }


        public MemoryStream CurrentMemoryStream
        {
            get { return currentOutputStream; }
        }

        public class DocumentReadyListender : IDocumentReadyListener
        {

            private ByteArrayPdfSplitter splitter;

            public DocumentReadyListender(ByteArrayPdfSplitter splitter)
            {
                this.splitter = splitter;
            }

            public void DocumentReady(iText.Kernel.Pdf.PdfDocument pdfDocument, PageRange pageRange)
            {
                pdfDocument.Close();
                var data = splitter.CurrentMemoryStream.ToArray();
                BibliotecaDeClases.Logica.AccesoBlobs.AccesoBlobs.Datos.Add(data);
                String pageNumber = pageRange.ToString();

            }
        }
    }
    public class ProcesadorImagenNeumatico
    {
        public static string GetConnectionString(string connectionName = "AzureBlobs")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        public static async Task<string> SubirFotoNeumatico(NeumaticosModel modelo)
        {

            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), "neumaticos");
            Dictionary<string, string> tags = new Dictionary<string, string>
          {
            { "ID", modelo.IDFoto }
          };

            string search = "/";
            string result = modelo.Foto.ContentType.Substring(modelo.Foto.ContentType.IndexOf(search) + search.Length);
            string nombre = modelo.IDFoto +"." + result;
            await contenedor.UploadBlobAsync(nombre, modelo.Foto.InputStream);
            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            appendBlobWithTags.SetTags(tags);
            return appendBlobWithTags.Uri.ToString();
        }
        public static async Task<string> SubirFotoLogo(FotoLogoModel modelo)
        {

            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), "fotos");
            Dictionary<string, string> tags = new Dictionary<string, string>
          {
            { "ID", modelo.ID }
          };

            string search = "/";
            string result = modelo.Foto.ContentType.Substring(modelo.Foto.ContentType.IndexOf(search) + search.Length);
            string nombre = modelo.ID + "." + result;
            await contenedor.UploadBlobAsync(nombre, modelo.Foto.InputStream);
            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            appendBlobWithTags.SetTags(tags);
            return appendBlobWithTags.Uri.ToString();
        }



    }

    public class ProcesadorFotoWeb
    {
        public static string GetConnectionString(string connectionName = "AzureBlobs")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        public static async Task<string> subirNuevaFoto(FotoModel foto)
        {
            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), "fotos");
            Dictionary<string, string> tags = new Dictionary<string, string>
          {
            { "ID", foto.ID }
          };
            string search = "/";
            string result = foto.Foto.ContentType.Substring(foto.Foto.ContentType.IndexOf(search) + search.Length);
            string nombre = foto.ID + "." + result;
            await contenedor.UploadBlobAsync(nombre, foto.Foto.InputStream);
            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            appendBlobWithTags.SetTags(tags);
            return appendBlobWithTags.Uri.ToString();
        }
    }

    public class ProcesadorFiniquitos
    {
        public static string GetConnectionString(string connectionName = "AzureBlobs")
        {
            return ConfigurationManager.ConnectionStrings[connectionName].ConnectionString;
        }
        public static async Task subirNuevoFiniquito(FiniquitoModel modelo)
        {
            BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), "finiquitos");
            Dictionary<string, string> tags = new Dictionary<string, string>
          {
            { "ID", modelo.ID }
          };
            var nombre = "finiquito" + modelo.Empleado + ".pdf";
           var response = await contenedor.UploadBlobAsync(nombre, modelo.Finiquito.InputStream);            
            AppendBlobClient appendBlobWithTags = contenedor.GetAppendBlobClient(nombre);
            appendBlobWithTags.SetTags(tags);
            
        }
        public static async Task<Stream> descargarFiniquito(string idfiniquito)
        {

                BlobServiceClient serviceClient = new BlobServiceClient(GetConnectionString());
                string id = @"""ID"" = '" + idfiniquito + "'";

                String query = id;
                var filename = serviceClient.FindBlobsByTags(query).FirstOrDefault().BlobName;
                BlobContainerClient contenedor = new BlobContainerClient(GetConnectionString(), "finiquitos");
                BlobClient blobClient = contenedor.GetBlobClient(filename);
                BlobDownloadInfo download = await blobClient.DownloadAsync();

                return download.Content;
           
        }
    }

}
