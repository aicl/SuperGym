using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class DeCajaDeleteResponse:IHasResponseStatus
	{
		public DeCajaDeleteResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
						
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
	}
}

