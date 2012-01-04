using System;
using System.ComponentModel;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[Description("GET Obtener todas las actividades \n"
				+"GET Obtener activdades por nombre Like {Nombre} \n"
             	+"POST Insertar nuevo  registro   {Nombre}\n"
	            +"PUT  Actualizar registro con id={Id} {Nombre} \n" 
	           	+"DELETE  borrar registro  con id= {Id}"  )]
	
	[RestService("/actividad/sessionid/{SessionId}","get")]
	[RestService("/actividad/sessionid/{SessionId}/nombre/{Nombre}","get,post")]
	[RestService("/actividad/sessionid/{SessionId}/id/{Id}/nombre/{Nombre}","put")]
	[RestService("/actividad/sessionid/{SessionId}/id/{Id}","delete")]
	public class Actividad:SuperGym.Records.Actividad, IRequireAuthentication
	{
		public Actividad ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
	}
}

