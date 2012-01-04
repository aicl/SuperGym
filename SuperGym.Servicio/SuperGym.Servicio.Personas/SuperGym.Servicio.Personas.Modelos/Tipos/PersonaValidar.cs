using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	
	[CustomAuth(Action="Ingresos.Insertar")]
	public class PersonaValidar:PersonaGet, IRequireAuthentication
	{
		public PersonaValidar ()
		{
		}
		
	}
}

