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
	public class GrupoActividadService:AuthRestServiceBase<GrupoActividad>
	{
		
		public override object OnGet (GrupoActividad request)
		{
			ValidateUserAction("Usuarios.Consultar");
			
			GrupoActividadResponse gur ;
			
			if( request.IdGrupo==default(Int16) && request.IdActividad==default(Int16) ){
				var gu = request as SuperGym.Records.GrupoActividad;
				
				gur= new GrupoActividadResponse(){
					Success=true,
					GruposActividades= gu.Get(DbFactory),
				};
			}
			
			else if( request.IdActividad!=default(Int16) ) {
		
				gur = new GrupoActividadResponse(){
					Success=true,
					GruposActividades= request.GetByIdActividad(DbFactory)
				};
				
				
			}else{
				
				gur = new GrupoActividadResponse(){
					Success=true,
					GruposActividades= request.GetByIdGrupo(DbFactory)
				};
				
			}
			
			return gur;
			
		}
		
		
		public override object OnPost (GrupoActividad request)
		{
			ValidateUserAction("Usuarios.Actualizar");
			request.Post(DbFactory);
			return new GrupoActividadResponse(){
				Success=true,
				GruposActividades= new List<SuperGym.Records.GrupoActividad>(
				            new SuperGym.Records.GrupoActividad[]{ request})
			};
		}
		
		
		public override object OnDelete (GrupoActividad request)
		{
			ValidateUserAction("Usuarios.Actualizar");
			
			request.Delete(DbFactory);
			return new GrupoActividadResponse(){
				Success=true,
			};
			
		}
		
	}
}

