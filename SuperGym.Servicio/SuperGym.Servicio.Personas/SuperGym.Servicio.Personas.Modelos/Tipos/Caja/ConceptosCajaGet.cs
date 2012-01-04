using System;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Caja.Consultar")]
	public class ConceptosCajaGet:IRequireAuthentication
	{
		public ConceptosCajaGet ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}

