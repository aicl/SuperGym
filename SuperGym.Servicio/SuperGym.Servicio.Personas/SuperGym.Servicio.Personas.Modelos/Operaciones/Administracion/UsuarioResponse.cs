using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using SuperGym.Records;

namespace SuperGym.Servicio.Personas.Modelos
{
	public class UsuarioResponse:IHasResponseStatus
	{
		public UsuarioResponse ()
		{
		
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Usuarios=  new List<SuperGym.Records.Usuario>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<SuperGym.Records.Usuario> Usuarios{ get; set;}
		
		public int Total {get { return  Usuarios.Count;} }
	}
}

