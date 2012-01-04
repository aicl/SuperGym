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
	public class ProfesionesService:AuthServiceBase<ProfesionesGet>
	{
		protected override object Run(ProfesionesGet request)
		{
			return new ProfesionesGetResponse(){
				Profesiones= DbFactory.Profesiones(),
				Success=true,
			};
		}
	}
}

