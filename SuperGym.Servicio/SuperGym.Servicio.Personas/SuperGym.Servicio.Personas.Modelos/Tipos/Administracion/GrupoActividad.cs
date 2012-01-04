using System;
using System.ComponentModel;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[Description("GET Obtener las actividades  del grupo  con id={IdGrupo} \n"
				+"GET Obtener los los grupos de la actvidad  id={IdActividad} \n"
    			+"GET Obtener todos los registros \n"
	           	+ "POST Insertar nuevo  registro   {IdGrupo}/{IdActividad}\n"
	           	+ "DELETE  borrar registro {IdGrupo/IdActividad} "  )]
	[RestService("/grupoactividad/sessionid/{SessionId}/idgrupo/{IdGrupo}","get")]
	[RestService("/grupoactividad/sessionid/{SessionId}/idactividad/{IdActividad}","get")]
	[RestService("/grupoactividad/sessionid/{SessionId}","get")]
	[RestService("/grupoactividad/sessionid/{SessionId}/idgrupo/{IdGrupo}/idactvidad/{IdActividad}","post")]
	[RestService("/grupoactividad/sessionid/{SessionId}/idgrupo/{IdGrupo}/idactvidad/{IdActividad","delete")]
	
	public class GrupoActividad:SuperGym.Records.GrupoActividad, IRequireAuthentication
	{
		public GrupoActividad ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
	}
}

