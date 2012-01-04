using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;
namespace SuperGym.Servicio.Personas.Modelos
{
	public class ProfesionesGetResponse:IHasResponseStatus
	{
		public ProfesionesGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			
			Profesiones = new List<Profesion>();
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<Profesion> Profesiones{ get; set;}
		
		public int Total {get { return Profesiones.Count;} }
	}
}

