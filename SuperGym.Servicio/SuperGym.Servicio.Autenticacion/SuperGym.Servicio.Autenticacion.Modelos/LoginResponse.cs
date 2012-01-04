using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.DesignPatterns.Model;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
		
	
	public class LoginResponse: IHasGuidId, IHasResponseStatus
	{
		public LoginResponse ()
		{
			ResponseStatus = new ResponseStatus();
			Grupos= new List<UsuarioGrupo>();
			Actividades = new List<String>();
		}
		
		public Guid Id { get; set; }
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success{get;set;}
		
		public List<UsuarioGrupo> Grupos{ get; set;}
		
		public List<String> Actividades{get; set;}
	}
}
