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
	public class CorreoService:AuthRestServiceBase<Correo>
	{
		public override object OnGet (Correo request)
		{
			ValidateUserAction("Usuarios.Consultar");
			
			CorreoResponse gur ;
			
			if( request.IdActividad==default(Int16) && request.IdUsuario==default(Int16) ){
				var gu = request as SuperGym.Records.Correo;
				
				gur= new CorreoResponse(){
					Success=true,
					Correos= gu.Get(DbFactory),
				};
			}
			
			else if( request.IdUsuario!=default(Int16) ) {
		
				gur = new CorreoResponse(){
					Success=true,
					Correos= request.GetByIdUsuario(DbFactory)
				};
				
				
			}else{
				
				gur = new CorreoResponse(){
					Success=true,
					Correos= request.GetByIdActividad(DbFactory)
				};
				
			}
			
			return gur;
			
		}
		
		
		public override object OnPost (Correo request)
		{
			ValidateUserAction("Usuarios.Actualizar");
			request.Post(DbFactory);
			return new CorreoResponse(){
				Success=true,
				Correos= new List<SuperGym.Records.Correo>(
				            new SuperGym.Records.Correo[]{ request})
			};
		}
		
		
		public override object OnDelete (Correo request)
		{
			ValidateUserAction("Usuarios.Actualizar");
			
			request.Delete(DbFactory);
			return new CorreoResponse(){
				Success=true,
			};
			
		}
		
	}
}

