using System;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Clasificacion.Consultar")]
	public class ClasificacionesGet:IRequireAuthentication
	{
		public ClasificacionesGet ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}

