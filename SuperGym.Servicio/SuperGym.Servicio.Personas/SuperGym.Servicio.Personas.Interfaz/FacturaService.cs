using System;
using System.IO;
using System.Data;
using System.Linq;

using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;

using ServiceStack.OrmSimple;

using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
using SuperGym.Tablas;

using Reportman.Reporting;
using Reportman.Drawing;

using SuperGym.Servicio.Personas.Modelos;

namespace SuperGym.Servicio.Personas.Interfaz
{	
	public class FacturaPagoInsertService:AuthServiceBase<FacturaPagoInsert>{
		
		public AppConfig Config { get; set;}
		
		protected override object Run(FacturaPagoInsert request){
						
			var tp = DbFactory.TipoPago(request.IdTipoPago);
			
			request.FechaPago= DateTime.Today;
			request.FechaTerminacion = 
				request.FechaInicio.FechaTerminacion(request.Cantidad*tp.Dias);
							
			request.IdUsuarioRegistra= Int16.Parse( Session.UserId );
			
			request.IdFormaPago= (request.IdFormaPago==default(Int16) )
				? (System.Int16)1  
				:request.IdFormaPago;
			
			request.ValorUnitario= tp.Valor;
			request.ValorTotal= tp.Valor* request.Cantidad;
			request.Activa=true;
			
			DbFactory.Exec(dbCmd => 
				    dbCmd.Insert(request) );
			
			
			// imprimir 
			string  printMessage;
			bool printSuccess;
			
			try{
				Report r = new Report();
				r.LoadFromFile( Path.Combine( Config.MetaFilesDirectory, "factura.rep") );
				foreach(DataInfo di in r.DataInfo){
					r.DataInfo[di.Alias].GetDbItem().ConnectionString=  Config.ReportConnection;
				}
				r.Params["IDPAGO"].Value= request.Id.ToString();
				
				PrintOutPDF  pdfFile = new PrintOutPDF();
				pdfFile.FileName= Path.Combine(Config.PrintOutDirectory, request.Id.ToString() +".pdf");
				pdfFile.Print(r.MetaFile );
				printMessage="OK";
				printSuccess= true;
			}
			catch(Exception e){
				printMessage=e.Message;
				printSuccess= false;
			}
			
			return new FacturaPagoInsertResponse (){
				Factura = request as Pago,
				Success=true,
				PrintSuccess= printSuccess,
				PrintMessage= printMessage
			};
		}
	}
	
	public class FacturaTipoPagoGetService:AuthServiceBase<FacturaTipoPagoGet>{
		
		protected override object Run(FacturaTipoPagoGet request)
		{
			return new FacturaTipoPagoGetResponse(){
				Success= true,
				TiposPago= DbFactory.TiposPago()
					.Where( r=> r.ValidoDesde<=DateTime.Today 
					        && r.ValidoHasta >= DateTime.Today).ToList()
			};
		}
	}
	
	public class FacturaSiguienteNumeroGetService:AuthServiceBase<FacturaSiguienteNumeroGet>
	{
		protected override object Run(FacturaSiguienteNumeroGet request)
		{
			var n = DbFactory.UltimaFactura().Numero;
			
			return new FacturaSiguienteNumeroGetResponse(){
				Success=true,
				Numero= n.SiguienteNumero()
		
			};
			
		}	
	}
	
	public class FacturaPagoAnularService:AuthServiceBase<FacturaPagoAnular>{
		
		protected override object Run(FacturaPagoAnular request){
			
			string msg=string.Empty;
			Pago factura = new Pago();
			
			if(request.Id != default(Int32)) {
				factura= DbFactory.Factura(request.Id);
			}
			else
			{ 
				if( request.Numero.IsNullOrEmpty() ){
					msg="Debe Indicar la factura para anular";
				}
				else{
					factura= DbFactory.Factura(request.Numero);
				}
				
			}
			
			if( !msg.IsNullOrEmpty()){
				return new FacturaPagoAnularResponse(){
					Success=false,
					ResponseStatus = new ResponseStatus(){
						ErrorCode="FacturaNoIndicada",
						Message=msg
					}
				};
			}
			
			if(factura.Activa){
				factura.Activa=false;
				DbFactory.Exec(dbCmd => 
					dbCmd.Update(factura) );
				factura.Cantidad=0;
				factura.ValorTotal=0;
				factura.ValorUnitario=0;
			}
			
			
			return new FacturaPagoAnularResponse(){
					Success=true,
					Factura= factura
				};
			
			
		}
	}
	
	
	public class SuspensionInsertService:AuthServiceBase<SuspensionInsert>{
		
		protected override object Run(SuspensionInsert request){
			
			string msg=string.Empty;
			Pago factura = new Pago();
			
			if(request.IdPago != default(Int32)) {
				factura= DbFactory.Factura(request.IdPago);
			}
			else
			{ 
				if( request.Numero.IsNullOrEmpty() ){
					msg="Debe Indicar la factura para suspender";
				}
				else{
					factura= DbFactory.Factura(request.Numero);
				}
				
			}
			
			if( !msg.IsNullOrEmpty()){
				return new SuspensionInsertResponse(){
					Success=false,
					ResponseStatus = new ResponseStatus(){
						ErrorCode="FacturaNoIndicada",
						Message=msg
					}
				};
			}
			
			if(! factura.Activa){
				return new SuspensionInsertResponse(){
					Success=false,
					ResponseStatus = new ResponseStatus(){
						ErrorCode="FacturaAnulada",
						Message="La factura se encuentra anulada"
					}
				};
			}
			
			if(request.Desde< DateTime.Today){
				return new SuspensionInsertResponse(){
					Success=false,
					ResponseStatus = new ResponseStatus(){
						ErrorCode="InicioSuspensionNoValido<Hoy",
						Message=string.Format(@"La fecha de inicio de la suspension {0} debe ser mayor-igual al
dia de hoy {1} ",
						                      request.Desde.ToString("dd.MM.yyyy"),
						                      DateTime.Today.ToString("dd.MM.yyyy")
						                      )
					}
				};	
			}
			
			if(request.Desde< factura.FechaInicio || request.Desde >factura.FechaTerminacion){
				return new SuspensionInsertResponse(){
					Success=false,
					ResponseStatus = new ResponseStatus(){
						ErrorCode="InicioSuspensionNoValido",
						Message=string.Format(@"La fecha de inicio de la suspension {0} debe ser mayor-igual a la fecha
de Inicio de la factura {1} y Menor-igual a la fecha de terminacion de la factura {2}",
						                      request.Desde.ToString("dd.MM.yyyy"),
						                      factura.FechaInicio.ToString("dd.MM.yyyy"),
						                      factura.FechaTerminacion.ToString("dd.MM.yyyy"))
					}
				};
			}
						
			
			if(request.Hasta< factura.FechaInicio || request.Hasta >factura.FechaTerminacion){
				return new SuspensionInsertResponse(){
					Success=false,
					ResponseStatus = new ResponseStatus(){
						ErrorCode="FinSuspensionNoValido",
						Message=string.Format(@"La fecha de terminacion de la suspension  {0} debe ser mayor-igual a la fecha
de Inicio de la factura {1} y Menor-igual a la fecha de terminacion de la factura {2}",
						                      request.Hasta.ToString("dd.MM.yyyy"),
						                      factura.FechaInicio.ToString("dd.MM.yyyy"),
						                      factura.FechaTerminacion.ToString("dd.MM.yyyy"))
					}
				};
			}
			
			
			Suspension sus = new Suspension(){
				FechaTerminacion= factura.FechaTerminacion,
				NuevaFecha= (factura.FechaTerminacion+ ( request.Hasta- request.Desde )).AddDays(1),
				Desde= request.Desde,
				Hasta= request.Hasta,
				IdPago= factura.Id,
				Fecha= DateTime.Today,
			};
						
			
			DbFactory.Exec(dbCmd => 
				    dbCmd.Insert(sus) );
				
			return new SuspensionInsertResponse(){
				Success=true,
				Factura= DbFactory.Factura(factura.Id),
				Suspension= sus
			};
			
		}	
	
	}
	
	public class SuspensionesGetService:AuthServiceBase<SuspensionesGet>{
		
		protected override object Run(SuspensionesGet request){
			
			return new SuspensionesGetResponse(){
				Suspensiones= DbFactory.Suspensiones(request.IdPago).
					OrderByDescending(r=> r.Fecha).ToList(),
				Success=true,
			};
			
		}
	}
	
	
	public class SuspensionDeleteService:AuthServiceBase<SuspensionDelete>{
		
		protected override object Run(SuspensionDelete request){
			
			var idPago = DbFactory.Suspension(request.Id).IdPago;
			if( idPago == default(Int32) ){
				return new SuspensionDeleteResponse(){
					Success=false,
					ResponseStatus = new ResponseStatus(){
						ErrorCode="NoExisteSuspension",
						Message= string.Format("No existe la suspesion indicada para borrar. Id:'{0}'",
						                       request.Id)
					}
				};				
			}
			
			Suspension sus = new Suspension(){
				Id= request.Id	
			};
			
			DbFactory.Exec(dbCmd => 
					dbCmd.Delete( sus) );
				
						
			return new SuspensionDeleteResponse(){
				Success=true,
				Factura= DbFactory.Factura(idPago)
			};
			
		}
			
		
	}
	
}

