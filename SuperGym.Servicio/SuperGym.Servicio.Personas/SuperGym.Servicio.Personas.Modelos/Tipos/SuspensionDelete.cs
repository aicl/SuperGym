using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Actualizar")]
	public class SuspensionDelete:IRequireAuthentication
	{
		public SuspensionDelete ()
		{
		}
		
		public Int32 Id { get; set; }
		
		public Guid SessionId {get; set;}
	}
}

