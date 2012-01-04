using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class DeCajaUpdateResponse:IHasResponseStatus
	{
		public DeCajaUpdateResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
		}
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
	}
}

