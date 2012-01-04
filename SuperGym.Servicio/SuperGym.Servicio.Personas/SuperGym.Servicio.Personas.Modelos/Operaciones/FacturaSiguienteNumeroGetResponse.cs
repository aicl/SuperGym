using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class FacturaSiguienteNumeroGetResponse:IHasResponseStatus
	{
		public FacturaSiguienteNumeroGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
		
		public string Numero {get; set;}
		
	}
}

