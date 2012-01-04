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
	public class MunicipiosService:AuthServiceBase<MunicipiosGet>
	{
		protected override object Run(MunicipiosGet request)
		{
			
			return new MunicipiosGetResponse(){
				Municipios= DbFactory.Municipios(),
				Success=true,
			};
		}
		
	}
}

