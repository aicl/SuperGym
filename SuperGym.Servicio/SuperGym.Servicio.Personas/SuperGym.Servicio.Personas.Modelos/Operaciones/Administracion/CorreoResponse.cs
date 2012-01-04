

using System;
using System.Collections.Generic;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;


namespace SuperGym.Servicio.Personas.Modelos
{
	public class CorreoResponse:IHasResponseStatus
	{
		public CorreoResponse ()
		{
			ResponseStatus= new ResponseStatus();
			
			ResponseStatus.Message="OK";
			Success=false;
			Correos = new List<SuperGym.Records.Correo>();
			
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
		public bool Success {get;set;}

		public List<SuperGym.Records.Correo> Correos{ get; set;}
		
		public int Total {get { return  Correos.Count;} }
	}
}