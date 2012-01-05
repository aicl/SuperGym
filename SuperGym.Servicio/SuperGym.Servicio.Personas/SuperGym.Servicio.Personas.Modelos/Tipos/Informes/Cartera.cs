using System;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Informes.Consultar")]
	public class Cartera:IRequireAuthentication
	{
		public Cartera ()
		{
		}
		
		public Guid SessionId {get; set;}
	}
}
