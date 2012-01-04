using System;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class PersonaGetResponse:IHasResponseStatus
	{
		public PersonaGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			Persona = new Persona();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public Persona Persona {get ; set;}
		
		public bool Success {get;set;}
	}
}

