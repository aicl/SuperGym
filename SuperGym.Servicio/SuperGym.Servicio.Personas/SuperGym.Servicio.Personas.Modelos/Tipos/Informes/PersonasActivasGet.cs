using System;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Informes.Consultar")]
	public class PersonasActivasGet:IRequireAuthentication
	{
		public PersonasActivasGet ()
		{
		}
		public Guid SessionId {get; set;}
		
		public DateTime FechaCorte { get; set;}
		
		
		
	}
}

