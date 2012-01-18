using System;

using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Consultar")]
	public class FacturaPagoSendMail:IRequireAuthentication
	{
		public FacturaPagoSendMail ()
		{
		}
		
		public Int32 Id { get; set; }
		
		public string Numero { get; set;}
		
		public Guid SessionId {get; set;}
	}
}

