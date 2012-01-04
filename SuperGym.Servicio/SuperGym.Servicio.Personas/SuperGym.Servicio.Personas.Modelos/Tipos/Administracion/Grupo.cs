using System;
using System.ComponentModel;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[Description("GET Obtener todos los grupos\n"
               + "POST Insertar nuevo grupo   {Nombre}/{Descripcion}/{Directorio}{Menu}\n"
	           + "PUT  Actualizar informacion del grupo con id={Id} {Nombre}/{Descripcion}/{Directorio}{Menu}\n"
	           + "DELETE  borrar grupo con id={Id} "  )]
	[RestService("/grupo/sessionid/{SessionId}","get")]
	[RestService("/grupo/sessionid/{SessionId}/nombre/{Nombre}/descripcion/{Descripcion}/directorio/{Directorio}/menu/{Menu}","post")]
	[RestService("/grupo/sessionid/{SessionId}/id/{Id}/nombre/{Nombre}/descripcion/{Descripcion}/directorio/{Directorio}/menu/{Menu}","put")]
	[RestService("/grupo/sessionid/{SessionId}/id/{Id}","delete")]
	public class Grupo:SuperGym.Records.Grupo, IRequireAuthentication
	{
		public Grupo ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
	}
}

