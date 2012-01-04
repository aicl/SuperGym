using System;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Informes.Consultar")]
	public class FacturacionDiaGet:IRequireAuthentication
	{
		public FacturacionDiaGet ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public DateTime FechaInicial { get; set;}
		
		public DateTime FechaFinal { get; set;}
		
		
	}
}

