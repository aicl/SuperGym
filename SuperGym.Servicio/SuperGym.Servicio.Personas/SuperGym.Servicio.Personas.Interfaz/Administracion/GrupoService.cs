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
	public class GrupoService:AuthRestServiceBase<Grupo>
	{
		public override object OnGet (Grupo request)
		{
			ValidateUserAction("Grupos.Consultar");
			var grupo = request as SuperGym.Records.Grupo;
			
			return new GrupoResponse(){
				Success=true,
				Grupos = grupo.Get(DbFactory),
			};
			
		}
		
		
		public override object OnPost (Grupo request)
		{
			ValidateUserAction("Grupos.Insertar");
			
			request.Post(DbFactory);
			
			List<SuperGym.Records.Grupo> grupos=
				new List<SuperGym.Records.Grupo>(new SuperGym.Records.Grupo []{request} );
					
			return new GrupoResponse(){
				Success=true,
				Grupos = grupos,
			};
			
		}
		
		public override object OnPut (Grupo request)
		{
			ValidateUserAction("Grupos.Actualizar");
			
			request.Put(DbFactory);
			return new GrupoResponse(){
				Success=true,
			};
		}
		
		public override object OnDelete (Grupo request)
		{
			ValidateUserAction("Grupos.Borrar");
			
			request.Delete(DbFactory);
			return new GrupoResponse(){
				Success=true,
			};
		}
		
	}
	
	
	
}

