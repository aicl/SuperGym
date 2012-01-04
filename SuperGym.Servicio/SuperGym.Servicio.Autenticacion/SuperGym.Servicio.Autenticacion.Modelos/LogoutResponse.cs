using System;

using ServiceStack.ServiceInterface.ServiceModel;

namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class LogoutResponse:IHasResponseStatus
	{
		public LogoutResponse ()
		{
			ResponseStatus = new ResponseStatus();
		
		}
		
		public ResponseStatus ResponseStatus { get; set; }
		
	}
}

