using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class GrupoUsuarioResponse:IHasResponseStatus
	{
		public GrupoUsuarioResponse ()
		{
			
			ResponseStatus= new ResponseStatus();
			ResponseStatus.Message="OK";
			Success=false;
			GruposUsuarios = new List<SuperGym.Records.GrupoUsuario>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<SuperGym.Records.GrupoUsuario> GruposUsuarios{ get; set;}
		
		public int Total {get { return  GruposUsuarios.Count;} }
	}
}

