using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class FotoUploadResponse:IHasResponseStatus
	{
		public FotoUploadResponse ()
		{
			ResponseStatus= new ResponseStatus();
			success=false;
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool success {get;set;}
		public string message {get { return ResponseStatus.Message; }}
	}
}

