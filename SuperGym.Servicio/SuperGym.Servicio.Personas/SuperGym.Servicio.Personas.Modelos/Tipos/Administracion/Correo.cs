using System;
using System.ComponentModel;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[Description("GET Obtener las activivades con correo del usuario con id={IdUsuario} \n"
	             +"GET Obtener todos los usuarios con correo de la actividad con id={IdActividad} \n"
				+"GET Obtener todos los registros \n"
	           	+ "POST Insertar nuevo  registro   {IdUsuario}/IdActividad\n"
	           	+ "DELETE  borrar registro con id={Id} "  )]
	[RestService("/correo/sessionid/{SessionId}/idusuario/{IdUsuario}","get")]
	[RestService("/correo/sessionid/{SessionId}/idactividad{IdActividad}","get")]
	[RestService("/correo/sessionid/{SessionId}","get")]
	[RestService("/correo/sessionid/{SessionId}/idusuario/{IdUsuario}/idactividad/{IdActividad}","post")]
	[RestService("/correo/sessionid/{SessionId}/id/{Id}", "delete")]
	public class Correo:SuperGym.Records.Correo,IRequireAuthentication
	{
		public Correo ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
		
	}
}

