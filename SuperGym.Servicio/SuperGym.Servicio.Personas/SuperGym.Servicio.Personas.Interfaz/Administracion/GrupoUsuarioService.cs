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
	public class GrupoUsuarioService:AuthRestServiceBase<GrupoUsuario>
	{
		public override object OnGet (GrupoUsuario request)
		{
			ValidateUserAction("Usuarios.Consultar");
			
			GrupoUsuarioResponse gur ;
			
			if( request.IdGrupo==default(Int16) && request.IdUsuario==default(Int16) ){
				var gu = request as SuperGym.Records.GrupoUsuario;
				
				gur= new GrupoUsuarioResponse(){
					Success=true,
					GruposUsuarios= gu.Get(DbFactory),
				};
			}
			
			else if( request.IdUsuario!=default(Int16) ) {
		
				gur = new GrupoUsuarioResponse(){
					Success=true,
					GruposUsuarios= request.GetByIdUsuario(DbFactory)
				};
				
				
			}else{
				
				gur = new GrupoUsuarioResponse(){
					Success=true,
					GruposUsuarios= request.GetByIdGrupo(DbFactory)
				};
				
			}
			
			return gur;
			
		}
		
		
		public override object OnPost (GrupoUsuario request)
		{
			ValidateUserAction("Usuarios.Actualizar");
			request.Post(DbFactory);
			return new GrupoUsuarioResponse(){
				Success=true,
				GruposUsuarios= new List<SuperGym.Records.GrupoUsuario>(
				            new SuperGym.Records.GrupoUsuario[]{ request})
			};
		}
		
		
		public override object OnDelete (GrupoUsuario request)
		{
			ValidateUserAction("Usuarios.Actualizar");
			
			request.Delete(DbFactory);
			return new GrupoUsuarioResponse(){
				Success=true,
			};
			
		}
		
	}
}

