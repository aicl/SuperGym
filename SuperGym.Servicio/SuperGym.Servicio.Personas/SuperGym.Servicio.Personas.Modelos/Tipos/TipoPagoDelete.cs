
using System;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="TiposPago.Borrar")]
	public class TipoPagoDelete:IRequireAuthentication
	{
		public TipoPagoDelete ()
		{
			
		}
		
		public Int16 Id { get; set;}
		
		public Guid SessionId {get; set;}
	}
}

