using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;


namespace SuperGym.Servicio.Personas.Modelos
{
	public class GrupoResponse:IHasResponseStatus
	{
		public GrupoResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Grupos = new List<SuperGym.Records.Grupo>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<SuperGym.Records.Grupo> Grupos{ get; set;}
		
		public int Total {get { return  Grupos.Count;} }
	}
}

