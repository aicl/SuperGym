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
	public class ConceptosGetService:AuthServiceBase<ConceptosGet>
	{
		protected  override object Run(ConceptosGet request){
			
			return new ConceptosGetResponse(){
				Conceptos= DbFactory.Conceptos(),
				Success=true
			};
		}
		
	}
	
	public class ConceptoInsertService: AuthServiceBase<ConceptoInsert>{
		
		protected override object Run (ConceptoInsert request)
		{
			DbFactory.Exec(dbCmd=>
			               dbCmd.Insert(request) );
			
			
			return new ConceptoInsertResponse(){
				Concepto= request as Concepto,
				Success=true
			};
		}
	}
	
	public class ConceptoUpdateService: AuthServiceBase<ConceptoUpdate>{
	
		protected override object Run (ConceptoUpdate request)
		{
			DbFactory.Exec(dbCmd=>
			               dbCmd.Update(request) );
			
			return new ConceptoUpdateResponse(){
				Success=true
			};
		}
	}
	
	
	public class ConcetoDeleteService: AuthServiceBase<ConceptoDelete>{
		
		protected override object Run (ConceptoDelete request)
		{
						
			var dc = new Concepto{
				Id= request.Id
			};
			
			DbFactory.Exec(dbCmd=>
			               dbCmd.Delete(dc) );
			
			return new ConceptoDeleteResponse(){
				Success=true
			};
			
			
		}
	}
	
}

