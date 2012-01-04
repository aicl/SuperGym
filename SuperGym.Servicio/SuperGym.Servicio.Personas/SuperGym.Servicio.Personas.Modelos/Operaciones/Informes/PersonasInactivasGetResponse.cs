using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records.Auxiliares;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class PersonasInactivasGetResponse
	{
		public PersonasInactivasGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			
			PersonasInactivas= new List<PersonaInactiva>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}
		
		public List<PersonaInactiva> PersonasInactivas{get;set; }
	}
}

