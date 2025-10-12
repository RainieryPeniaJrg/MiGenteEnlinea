using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using iText.Html2pdf;
using Microsoft.TeamFoundation.WorkItemTracking.Client;

using MiGente_Front.Data;

using Newtonsoft.Json;

namespace MiGente_Front.Services
{
    public class Utilitario
    {
        private string ObtenerImagenComoDataUrl(int id)
        {
            // Ajusta el tipo MIME según el formato de tu imagen, por ejemplo, image/png, image/jpeg, etc.
            string mimeType = "image/jpeg"; // Cambia esto si tu imagen tiene otro formato
            byte[] imageBytes;

            // Conexión a la base de datos
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["miCadenaConexion"].ConnectionString))
            {
                connection.Open();

                // Consulta para obtener los datos binarios de la imagen
                using (SqlCommand command = new SqlCommand("SELECT Imagen FROM MiTabla WHERE ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);

                    // Recupera los datos de la imagen como un arreglo de bytes
                    imageBytes = command.ExecuteScalar() as byte[];
                }
            }

            if (imageBytes != null)
            {
                // Convierte los datos binarios en una cadena base64
                string base64String = Convert.ToBase64String(imageBytes);
                return $"data:{mimeType};base64,{base64String}"; // Devuelve la URL de datos completa
            }
            else
            {
                // Maneja el caso de una imagen no encontrada o establece una imagen por defecto
                return "";
            }
        }

        public byte[] ConvertHtmlToPdf(string htmlContent)
        {
            // Crea un MemoryStream donde se almacenará el PDF
            using (MemoryStream pdfStream = new MemoryStream())
            {
                // Convierte el HTML a PDF y escribe en el MemoryStream
                HtmlConverter.ConvertToPdf(htmlContent, pdfStream);

                // Retorna el PDF en formato de byte[]
                return pdfStream.ToArray();
            }
        }

        
    }
}