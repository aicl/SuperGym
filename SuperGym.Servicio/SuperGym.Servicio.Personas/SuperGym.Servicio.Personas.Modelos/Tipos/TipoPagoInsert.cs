
using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="TiposPago.Insertar")]
	public class TipoPagoInsert:TipoPago, IRequireAuthentication
	{
		public TipoPagoInsert ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
	}
}

