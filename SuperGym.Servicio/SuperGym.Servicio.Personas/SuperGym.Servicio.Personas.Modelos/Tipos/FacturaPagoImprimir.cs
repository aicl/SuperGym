using System;

using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Consultar")]
	public class FacturaPagoImprimir:IRequireAuthentication
	{
		public FacturaPagoImprimir ()
		{
		}
		
		public Int32 Id { get; set; }
		
		public string Numero { get; set;}
		
		public bool Override { get; set;}
		
		public Guid SessionId {get; set;}
	}
}

