using System;
using System.Data;
using System.Linq;

using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;

using ServiceStack.OrmSimple;

using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
using SuperGym.Tablas;

using SuperGym.Servicio.Personas.Modelos;

namespace SuperGym.Servicio.Personas.Interfaz
{
	
	public class TipoPagoInsertService:AuthServiceBase<TipoPagoInsert>{
		
		protected override object Run(TipoPagoInsert request){
			try{
				DbFactory.Exec(
				    dbCmd => 
					dbCmd.Insert<TipoPago>( request )
				);
			}
			catch(Exception e){
				return new TipoPagoInsertResponse(){
					ResponseStatus= new ResponseStatus(){
						Message=e.Message,
						ErrorCode="ErrorAlInsertar"
					},
					Success=false,
				};
			}
			
			return new TipoPagoInsertResponse(){
				Success=false,
				TipoPago= request as TipoPago
			};	
		}
	}
	
	public class TipoPagoUpdateService:AuthServiceBase<TipoPagoUpdate>{
		
		protected override object Run(TipoPagoUpdate request){
			try{
				DbFactory.Exec(
				    dbCmd => 
					dbCmd.Update<TipoPago>( request )
				);
			}
			catch(Exception e){
				return new TipoPagoUpdateResponse(){
					ResponseStatus= new ResponseStatus(){
						Message=e.Message,
						ErrorCode="ErrorAlActualizar"
					},
					Success=false,
				};
			}
			
			return new TipoPagoUpdateResponse(){
				Success=false
			};	
		}
	}
	
	
	public class TipoPagoService:AuthServiceBase<TiposPagoGet>
	{
		protected override object Run(TiposPagoGet request){
						
			return new TiposPagoGetResponse(){
				TiposPago= DbFactory.TiposPago(),
				Success=true,
			};
		}
	}
	
	public class TipoPagoDeleteService:AuthServiceBase<TipoPagoDelete>{
		
		protected override object Run(TipoPagoDelete request){
			try{
				DbFactory.Exec(
				    dbCmd => 
					dbCmd.Delete<TipoPago>( new TipoPago(){
						Id= request.Id
					})
				);
			}
			catch(Exception e){
				return new TipoPagoDeleteResponse(){
					ResponseStatus= new ResponseStatus(){
						Message=e.Message,
						ErrorCode="ErrorAlBorrar"
					},
					Success=false,
				};
			}
			
			return new TipoPagoDeleteResponse(){
				Success=false,
			};
			
		}
	}
	
}

