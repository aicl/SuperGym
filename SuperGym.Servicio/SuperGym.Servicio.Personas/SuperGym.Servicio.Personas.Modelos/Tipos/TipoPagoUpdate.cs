
using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="TiposPago.Actualizar")]
	public class TipoPagoUpdate:TipoPago, IRequireAuthentication
	{
		public TipoPagoUpdate ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
	}
}

