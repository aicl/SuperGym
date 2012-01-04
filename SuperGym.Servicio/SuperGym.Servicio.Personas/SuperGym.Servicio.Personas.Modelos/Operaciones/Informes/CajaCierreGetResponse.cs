using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class CajaCierreGetResponse:IHasResponseStatus
	{
		public CajaCierreGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;			
						
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
		
		public string HtmlResponse { get; set;}
	}
}
