using System;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;

using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
using SuperGym.Tablas;

using SuperGym.Servicio.Personas.Modelos;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public class ClasificacionesService:AuthRestServiceBase<ClasificacionesGet>
	{
	
		protected override object Run(ClasificacionesGet request){	
		
			return new ClasificacionesGetResponse(){
				Clasificaciones= DbFactory.Clasificaciones(),
				Success=true
			};
		}
		
	}
}

