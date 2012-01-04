using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class PersonasGetResponse:IHasResponseStatus
	{
		public PersonasGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			Personas= new List<Persona>();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public List<Persona> Personas {get ; set;}
		
		public int Count { get { return Personas.Count;} }
		
		public bool Success {get;set;}
	}
}

