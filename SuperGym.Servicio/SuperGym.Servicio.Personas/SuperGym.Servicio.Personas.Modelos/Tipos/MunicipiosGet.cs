using System;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Municipios.Consultar")]	
	public class MunicipiosGet:IRequireAuthentication
	{
		public MunicipiosGet ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}

