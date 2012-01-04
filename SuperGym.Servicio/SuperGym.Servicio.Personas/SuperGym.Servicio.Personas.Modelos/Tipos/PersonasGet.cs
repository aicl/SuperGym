using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Personas.Consultar")]
	public class PersonasGet:IRequireAuthentication
	{
		public PersonasGet ()
		{
		}
		
		public string Criterio{get; set;}
		public string Valor{ get; set;}
		
		public Guid SessionId {get; set;}
	}
}

