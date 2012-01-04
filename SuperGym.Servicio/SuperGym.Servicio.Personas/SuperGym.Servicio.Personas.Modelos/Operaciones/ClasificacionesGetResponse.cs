using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class ClasificacionesGetResponse:IHasResponseStatus
	{
		public ClasificacionesGetResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Clasificaciones= new List<Clasificacion>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<Clasificacion> Clasificaciones{ get; set;}
		
		public int Total {get { return  Clasificaciones.Count;} }
		
	}
}

