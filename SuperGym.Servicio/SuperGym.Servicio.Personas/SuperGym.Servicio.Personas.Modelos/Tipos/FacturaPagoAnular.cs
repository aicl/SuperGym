using System;
using ServiceStack.DataAnnotations;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pago.Anular")]
	public class FacturaPagoAnular: IRequireAuthentication
	{
		public FacturaPagoAnular ()
		{
		}
		
		public Int32 Id { get; set; }
		public string Numero { get; set;}
		
		
		public Guid SessionId {get; set;}
		
	}
}

