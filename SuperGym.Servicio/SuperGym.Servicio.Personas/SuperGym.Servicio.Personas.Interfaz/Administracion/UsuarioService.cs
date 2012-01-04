using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;

using ServiceStack.OrmSimple;

using SuperGym.Servicio.Autenticacion.Modelos;

using SuperGym.Tablas;


using SuperGym.Servicio.Personas.Modelos;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public class UsuarioService:AuthRestServiceBase<Usuario>{
		
		public override object OnGet (Usuario request)
		{
			
			ValidateUserAction("Usuarios.Consultar");
				
			var usuarios= DbFactory.Usuarios();
			foreach(var x in usuarios){
				x.Clave="**********";
			}
			
			return new UsuarioResponse(){
				Usuarios= usuarios,
				Success=true
			};
		}
		
		public override object OnPost (Usuario request)
		{
			ValidateUserAction("Usuarios.Insertar");
			
			DbFactory.Exec(dbCmd => 
				    dbCmd.Insert(request) );
			
			List<SuperGym.Records.Usuario> usuarios = new List<SuperGym.Records.Usuario>();
			usuarios.Add(request);
			
			return new UsuarioResponse(){
				Usuarios = usuarios,
				Success=true,
			};
		}
		
		
		public override object OnDelete (Usuario request)
		{
			ValidateUserAction("Usuarios.Borrar");
			
			DbFactory.Exec(dbCmd => 
				    dbCmd.Delete(request) );
			
			return new UsuarioResponse(){
				Success=true,
			};
		}
		
		
		public override object OnPut (Usuario request)
		{
			if(request.Accion.IsNullOrEmpty() ) request.Accion="Actualizar";
			
			ValidateUserAction("Usuarios."+request.Accion);
			
			
			if(request.Accion=="Actualizar")
				DbFactory.ActualizarUsuario(request);
			else if(request.Accion=="CambiarClave")
				DbFactory.CambiarClaveUsuario(request);
			else
				return new UsuarioResponse(){
				Success=false,
				ResponseStatus= new ResponseStatus(){
					ErrorCode="AccionNoProgramada",
					Message="Accion " + request.Accion + "  No programada"
				}
			};
				
			return new UsuarioResponse(){
				Success=true,
			};
			
		}
		
	}
	
	
}

