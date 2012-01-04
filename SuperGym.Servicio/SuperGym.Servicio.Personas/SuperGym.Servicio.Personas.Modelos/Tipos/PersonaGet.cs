using System;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Personas.Consultar")]
	public class PersonaGet:IRequireAuthentication
	{
		public PersonaGet ()
		{
		}
		
		public string Documento{get; set;}
		public Guid SessionId {get; set;}
		
	}
}

