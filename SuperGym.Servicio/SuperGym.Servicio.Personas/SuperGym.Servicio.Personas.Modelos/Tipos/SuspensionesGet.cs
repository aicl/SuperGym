using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Consultar")]
	public class SuspensionesGet:IRequireAuthentication
	{
		public SuspensionesGet ()
		{
			
		}
		
		public Int32 IdPago { get; set; }
		
		public Guid SessionId {get; set;}
		
		
	}
	
}

