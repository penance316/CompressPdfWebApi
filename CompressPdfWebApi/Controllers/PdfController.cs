using System;
using System.IO;
using System.Web.Http;
using System.Web.Http.Results;
using CompressPdfWebApi.Models;
using iTextSharp.text.pdf;
using Newtonsoft.Json;

namespace CompressPdfWebApi.Controllers
{
	/// <summary>
	/// Hmm, well what can be said about such a lovely controller
	/// </summary>
	public class PdfController : ApiController
	{
		/// <summary>
		/// Compress a pdf
		/// </summary>
		/// <param name="base64Pdf">A small model to hold a base64 encoded pdf object { "content" : "somebase64" }</param>
		/// <returns>{ "content" : "smallerBase64" }</returns>
		public IHttpActionResult Post(Base64Pdf base64Pdf)
		{
			try
			{
				if (base64Pdf.data == null)
					return BadRequest("Check supplied pdf model");

				byte[] data = Convert.FromBase64String(base64Pdf.data);

				//Compress
				byte[] compressedData;
				using (var memStream = new MemoryStream())
				{
					var reader = new PdfReader(data);
					var stamper = new PdfStamper(reader, memStream, PdfWriter.VERSION_1_4);
					var pageNum = reader.NumberOfPages;

					for (var i = 1; i <= pageNum; i++)
						reader.SetPageContent(i, reader.GetPageContent(i));

					stamper.SetFullCompression();
					stamper.Close();
					reader.Close();

					compressedData = memStream.ToArray();
				}
				var compressedBase64 = Convert.ToBase64String(compressedData);

				return Json(new Base64Pdf { data = compressedBase64 });
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
	}
}
