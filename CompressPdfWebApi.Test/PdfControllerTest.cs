using System.Net.Http;
using System.Web.Http.Results;
using CompressPdfWebApi.Controllers;
using CompressPdfWebApi.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using Shouldly;

namespace CompressPdfWebApi.Test
{
	[TestFixture]
	public class ReportControllerTest
	{
		private PdfController _controller;
		private FakePdfGenerator _fakePdfGenerator;

		[SetUp]
		public void Context()
		{
			_controller = new PdfController();
			_fakePdfGenerator = new FakePdfGenerator();
		}

		[Test]
		public void CompressPdf_ShouldSucceed()
		{
			var actionResult = _controller.Post(_fakePdfGenerator.GenerateSimplePdf());
			actionResult.ShouldNotBe(null);

			var negResult = actionResult.ShouldBeOfType<JsonResult<Base64Pdf>>();
			negResult.Content.ShouldNotBeNull();
		}

		[Test]
		public void CompressEmptyPdf_ShouldReturnError()
		{
			var actionResult = _controller.Post(_fakePdfGenerator.GenerateEmptyPdf());
			actionResult.ShouldNotBe(null);
			actionResult.ShouldBeOfType<BadRequestErrorMessageResult>();
		}

		[Test]
		public void CompressInvalidPdf_ShouldReturnInternalError()
		{
			var actionResult = _controller.Post(_fakePdfGenerator.GenerateNotValidPdf());
			actionResult.ShouldNotBe(null);
			actionResult.ShouldBeOfType<ExceptionResult>();
		}
	}
}