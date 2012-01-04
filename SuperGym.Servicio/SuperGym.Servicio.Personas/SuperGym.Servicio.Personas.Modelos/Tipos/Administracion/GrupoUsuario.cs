using System;
using System.ComponentModel;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[Description("GET Obtener los grupos del  usuario con id={IdUsuario} \n"
				+"GET Obtener los usuarios del grupo con  id={IdGrupo} \n"
    			+"GET Obtener todos los registros \n"
	           	+ "POST Insertar nuevo  registro   {IdGrupo}/{IdUsuario}\n"
	           	+ "DELETE  borrar registro {IdGrupo/IdUsuario} "  )]
	
	[RestService("/grupousuario/sessionid/{SessionId}/idusuario/{IdUsuario}","get")]
	[RestService("/grupousuario/sessionid/{SessionId}/idgrupo/{IdGrupo}","get")]
	[RestService("/grupousuario/sessionid/{SessionId}","get")]
	[RestService("/grupousuario/sessionid/{SessionId}/idgrupo/{IdGrupo}/idusuario/{IdUsuario}","post")]
	[RestService("/grupousuario/sessionid/{SessionId}/idgrupo/{IdGrupo}/idusuario/{IdUsuario}","delete")]
	public class GrupoUsuario:SuperGym.Records.GrupoUsuario,IRequireAuthentication
	{
		public GrupoUsuario ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
		
	}
}

