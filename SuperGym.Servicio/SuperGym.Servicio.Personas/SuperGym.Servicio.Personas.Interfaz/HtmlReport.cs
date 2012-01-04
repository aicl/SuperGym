using System;
using System.IO;
using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.Text;

using ServiceStack.WebHost.Endpoints;


namespace SuperGym.Servicio.Personas.Interfaz
{
	public class HtmlReport
	{
				
		private const string VCardContentType = "text/x-report";

		public static void Register(IAppHost appHost)
		{
			appHost.ContentTypeFilters.Register(VCardContentType, SerializeToStream, null);

			
		}

		public static void SerializeToStream(IRequestContext requestContext, object response, IHttpResponse stream)
		{
			/*
			var httpReq = requestContext.Get<IHttpRequest>();
			string html = @"<html lang=""es-co"">
				<head>
				<title>Titulo del Informe</title>
				</head>
				<body>
					<h1> veamos esto </h1>
				</body>
				";
			
			var utf8Bytes = html.ToUtf8Bytes();
			stream.OutputStream.Write(utf8Bytes, 0, utf8Bytes.Length);
			*/
		}

		
	}
}

