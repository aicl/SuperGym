using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class ActividadResponse:IHasResponseStatus
	{
		public ActividadResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Actividades = new List<SuperGym.Records.Actividad>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<SuperGym.Records.Actividad> Actividades{ get; set;}
		
		public int Total {get { return  Actividades.Count;} }
	}
}

