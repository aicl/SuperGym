using System;
using SuperGym.Records;
using SuperGym.Servicio.Autenticacion.Modelos;
namespace SuperGym.Servicio.Personas.Modelos
{
	[CustomAuth(Action="Pagos.Consultar")]
	public class PersonaFacturasGet:PersonaGet, IRequireAuthentication
	{
		public PersonaFacturasGet ():base()
		{
		}
		
		public Int32 Id{ get; set;}
				
	}
}

