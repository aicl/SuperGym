using System;
using System.Data;
using ServiceStack.OrmSimple;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;
using SuperGym.Servicio.Personas.Modelos;
using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Tablas;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public class DeCajasGetService:AuthServiceBase<DeCajasGet>
	{
		protected  override object Run(DeCajasGet request){
			
			return new DeCajasGetResponse(){
				DeCajas= DbFactory.DeCajas(request.IdCaja),
				Success=true
			};
		}
		
	}
	
	
	public class DeCajaInsertService:AuthServiceBase<DeCajaInsert>{
		
		protected override object Run(DeCajaInsert request){
			
			DbFactory.Exec(dbCmd=>
			               dbCmd.Insert(request) );
			
			
			return new DeCajaInsertResponse(){
				DeCaja= request as Decaja,
				Success=true
			};
		}
	}
	
	public class DeCajaUpdateService:AuthServiceBase<DeCajaUpdate>{
		
		protected override object Run(DeCajaUpdate request){
			DbFactory.Exec(dbCmd=>
			               dbCmd.Update(request) );
			
			return new DeCajaUpdateResponse(){
				Success=true
			};
			
		}
	}
	
	public class DeCajaDeleteService:AuthServiceBase<DeCajaDelete>{
		
		protected override object Run(DeCajaDelete request){
			
			var dc = new Decaja(){
				Id= request.Id
			};
			
			DbFactory.Exec(dbCmd=>
			               dbCmd.Delete(dc) );
			
			return new DeCajaDeleteResponse(){
				Success=true
			};
			
		}
	}
	
	
	
	
}

