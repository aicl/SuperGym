using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class PersonaFacturasGetResponse:IHasResponseStatus
	{
		public PersonaFacturasGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			
			Facturas= new List<Pago>();
		}
						
		public List<Pago> Facturas{ get; set;}
		
		public ResponseStatus ResponseStatus { get; set; }
						
		public bool Success {get;set;}
	}
}

