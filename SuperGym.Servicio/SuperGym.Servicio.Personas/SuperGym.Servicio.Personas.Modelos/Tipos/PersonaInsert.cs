using System;

using ServiceStack.DataAnnotations;

using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Personas.Insertar")]
	public class PersonaInsert:Persona, IRequireAuthentication
	{
		public PersonaInsert ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
		
		[Ignore]
		public new DateTime FechaRegistro{get;set;}
		
		[Ignore]
		public new string RutaTemplate{get; set;}     
	}
}

