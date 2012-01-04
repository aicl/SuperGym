using System;
using System.ComponentModel;
using ServiceStack.ServiceHost;
using ServiceStack.DataAnnotations;
using SuperGym.Servicio.Autenticacion.Modelos;

namespace SuperGym.Servicio.Personas.Modelos
{
	[Description("GET Obtener todos los usuarios\n"
               + "POST Insertar nuevo usuario   {Nombre}/{Clave}/{Correo}{Activo}\n"
	             + "POST Insertar nuevo usuario {Nombre}/{Clave}/{Activo}\n"
               + "PUT  Actualizar informacion del usuario con id={Id} {Nombre}/{Correo}{Activo}\n"
	           + "PUT  Cambiar Clave  del usuario con id={Id} {Clave}/{Accion}\n"
	           + "DELETE  borrar usuario con id={Id} "  )]
	[RestService("/usuario/sessionid/{SessionId}","get")]
	[RestService("/usuario/sessionid/{SessionId}/nombre/{Nombre}/clave/{Clave}/correo/{Correo}/activo/{Activo}","post")]
	[RestService("/usuario/sessionid/{SessionId}/nombre/{Nombre}/clave/{Clave}/activo/{Activo}","post")]
	[RestService("/usuario/sessionid/{SessionId}/id/{Id}/nombre/{Nombre}/correo/{Correo}/activo/{Activo}","put")]
	[RestService("/usuario/sessionid/{SessionId}/id/{Id}/clave/{Clave}/accion/{Accion}","put")]
	[RestService("/usuario/sessionid/{SessionId}/id/{Id}","delete")]
	public class Usuario:SuperGym.Records.Usuario, IRequireAuthentication
	{
		public Usuario ():base()
		{
		}
		
		[Ignore]
		public Guid SessionId {get; set;}
		
		[Ignore]
		public String Accion {get; set;}
	}
}
//http://localhost/servicio/json/syncreply/usuario?SessionId=322c1557f95e4011ae889700dd18d1e2
//http://localhost/servicio/usuario/sessionid/366b632faba644b8a2491b5c592b3be9/
/*
  
executeRestRequest(
{url:'http://localhost/servicio/usuario/SessionId/322c1557f95e4011ae889700dd18d1e2',  
method:'get', success:function(result){console.log(result) }  } );

executeAjaxRequest({url:'http://localhost/servicio/json/syncreply/usuario', 
params: {SessionId:'f7f3d05df4f043449ef37ac0ed7b238c'},  
method:'get', success:function(result){console.log(result) }  } )
  
executeRestRequest(
{url:'http://localhost/servicio/usuario/SessionId/322c1557f95e4011ae889700dd18d1e2/nombre/billy/clave/123/correo/billaro/activo/true',
  method:'post', 
  success:function(result){console.log(result) }  } );

executeAjaxRequest(
{url:'http://localhost/servicio/json/syncreply/usuario', params:{SessionId:'f7f3d05df4f043449ef37ac0ed7b238c',Nombre:'billy',Clave:'123', correo:'billaro', activo:true},
  method:'post', 
  success:function(result){console.log(result) }  } );

executeRestRequest(
{url:'http://localhost/servicio/usuario/SessionId/322c1557f95e4011ae889700dd18d1e2/nombre/billy/clave/123/activo/true',
  method:'post', 
  success:function(result){console.log(result) }  } );


executeAjaxRequest(
{url:'http://localhost/servicio/json/syncreply/usuario', params:{SessionId:'f7f3d05df4f043449ef37ac0ed7b238c',Nombre:'billy',Clave:'123', Activo:true},
  method:'post', 
  success:function(result){console.log(result) }  } );

executeRestRequest(
{url:'http://localhost/servicio/usuario/SessionId/322c1557f95e4011ae889700dd18d1e2/id/18/nombre/billazo/correo/billazo@gmail.com/activo/true',
  method:'put', 
  success:function(result){console.log(result) }  } );
  
executeRestRequest(
{url:'http://localhost/servicio/usuario/SessionId/f7f3d05df4f043449ef37ac0ed7b238c/id/18/clave/nadielasabe/accion/CambiarClave',
  method:'put', 
  success:function(result){console.log(result) }  } );

executeAjaxRequest(
{url:'http://localhost/servicio/json/syncreply/usuario', params:{SessionId:'f7f3d05df4f043449ef37ac0ed7b238c',Id:20, Accion:'CambiarClave', Clave:'nueva..'},
  method:'put', 
  success:function(result){console.log(result) }  } );

executeRestRequest(
{url:'http://localhost/servicio/usuario/SessionId/f7f3d05df4f043449ef37ac0ed7b238c/id/20',
  method:'delete', 
  success:function(result){console.log(result) }  } );
  
executeAjaxRequest(
{ url:'http://localhost/servicio/json/syncreply/usuario', 
  params:{SessionId:'f7f3d05df4f043449ef37ac0ed7b238c', Id:20},
  method:'delete', 
  success:function(result){console.log(result) }  } );    NOOOOOOOOO ????
   

executeAjaxRequest(
{ url:'http://localhost/servicio/json/syncreply/usuario?SessionId=f7f3d05df4f043449ef37ac0ed7b238c&Id=20' ,  
  method:'delete', 
  success:function(result){console.log(result) }  } );       SIIIIIIII  !!!!
   
*/ 



