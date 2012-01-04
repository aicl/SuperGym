using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Consultar")]
	public class FacturaTipoPagoGet:IRequireAuthentication
	{
		public FacturaTipoPagoGet ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}

