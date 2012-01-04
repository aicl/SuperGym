using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;


namespace SuperGym.Servicio.Personas.Modelos
{
	public class GrupoActividadResponse:IHasResponseStatus
	{
		public GrupoActividadResponse ()
		{
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			GruposActividades = new List<SuperGym.Records.GrupoActividad>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<SuperGym.Records.GrupoActividad> GruposActividades{ get; set;}
		
		public int Total {get { return  GruposActividades.Count;} }
	}
}

