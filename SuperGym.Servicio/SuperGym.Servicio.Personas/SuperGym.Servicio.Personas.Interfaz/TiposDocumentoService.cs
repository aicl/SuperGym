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
	public class TiposDocumentoService:AuthRestServiceBase<TiposDocumentoGet>
	{
	
		protected override object Run(TiposDocumentoGet request){	
			
			return new TiposDocumentoGetResponse(){
				TiposDocumento= DbFactory.TiposDocumento(),
				Success=true,
			};
		}
		
		
	}
}

