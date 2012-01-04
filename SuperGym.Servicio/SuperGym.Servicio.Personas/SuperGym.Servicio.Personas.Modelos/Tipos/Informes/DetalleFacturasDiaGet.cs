using System;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Informes.Consultar")]
	public class DetalleFacturasDiaGet:IRequireAuthentication
	{
		public DetalleFacturasDiaGet ()
		{
		}
		
		public Guid SessionId {get; set;}
		
		public DateTime Fecha { get; set;}				
		
		public bool SendMail{get; set;}
	}
}

