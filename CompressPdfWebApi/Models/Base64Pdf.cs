using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CompressPdfWebApi.Models
{
	/// <summary>
	/// A shell class for the base64 encoded pdf object
	/// </summary>
	public class Base64Pdf
	{
		/// <summary>
		/// A base 64 encoded pdf file
		/// </summary>
		public string data { get; set; }
	}
}
