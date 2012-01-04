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
	public class ActividadServicio:AuthRestServiceBase<Actividad>
	{
		
		public override object OnGet (Actividad request)
		{
			ValidateUserAction("Actividades.Consultar");
			
			List<SuperGym.Records.Actividad> ar;
			
			if(request.Nombre.IsNullOrEmpty()){
				ar =(request as SuperGym.Records.Actividad).Get(DbFactory);
			}
			else{
				ar = request.GetByName(DbFactory);
			}
			
			return new ActividadResponse(){
				Success=true,
				Actividades =ar.OrderBy(r=>r.Nombre).ToList() ,
			};	
		}
		
		public override object OnPost (Actividad request)
		{
		
			ValidateUserAction("Actividades.Insertar");
			request.Post(DbFactory);
			return new ActividadResponse(){
				Success=true,
				Actividades= new List<SuperGym.Records.Actividad>(
				            new SuperGym.Records.Actividad[]{ request})
			};
		}
		
		public override object OnPut (Actividad request)
		{
			ValidateUserAction("Actividades.Actualizar");
			request.Put(DbFactory);
			return new ActividadResponse(){
				Success=true,
			};
			
		}
		
		public override object OnDelete (Actividad request)
		{
			ValidateUserAction("Actividades.Borrar");
			request.Delete(DbFactory);
			return new ActividadResponse(){
				Success=true,
			};
			
		}
		
		
	}
}

