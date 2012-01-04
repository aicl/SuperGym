using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Conceptos.Consultar")]
	public class ConceptosGet:IRequireAuthentication
	{
		public ConceptosGet ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}

