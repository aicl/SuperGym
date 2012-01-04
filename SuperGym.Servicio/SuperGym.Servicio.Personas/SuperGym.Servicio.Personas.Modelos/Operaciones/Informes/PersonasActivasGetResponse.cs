using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records.Auxiliares;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class PersonasActivasGetResponse:IHasResponseStatus
	{
		public PersonasActivasGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			
			PersonasActivas = new List<PersonaActiva>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
		
		public List<PersonaActiva> PersonasActivas{get;set; }
	
	}
}

