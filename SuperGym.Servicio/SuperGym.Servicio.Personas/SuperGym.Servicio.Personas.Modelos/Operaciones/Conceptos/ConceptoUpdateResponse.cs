using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class ConceptoUpdateResponse:IHasResponseStatus
	{
		public ConceptoUpdateResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
		}
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

	}
}

