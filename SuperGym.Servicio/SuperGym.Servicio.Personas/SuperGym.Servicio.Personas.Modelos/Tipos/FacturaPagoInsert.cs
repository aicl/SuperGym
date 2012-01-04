using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Insertar")]
	public class FacturaPagoInsert:Pago, IRequireAuthentication
	{
		public FacturaPagoInsert ()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
		
	}
}

