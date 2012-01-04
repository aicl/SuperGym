using System;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Informes.Consultar")]
	public class CajaConsolidadoGet:IRequireAuthentication
	{
		public CajaConsolidadoGet ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public DateTime Desde { get; set;}
		
		public DateTime Hasta { get; set;}
		
		public bool SendMail { get; set;}
		
		
	}
}

