using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Collections.Generic;

using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Web;

using ServiceStack.OrmSimple;

using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
using SuperGym.Tablas;

using Reportman.Reporting;
using Reportman.Drawing;

using SuperGym.Servicio.Personas.Modelos;

using ServiceStack.Logging;

namespace SuperGym.Servicio.Personas.Interfaz
{

	public class FacturacionDiaGetService:AuthServiceBase<FacturacionDiaGet>{
		
		
		protected override object Run(FacturacionDiaGet request){
			
			return new FacturacionDiaGetResponse(){
				Success=true,
				FacturacionDia= DbFactory.ListaFacturacionDia(request.FechaInicial,
					request.FechaFinal)
			};
			
		}
	}
	
	public class PersonasActivasGetService:AuthServiceBase<PersonasActivasGet>{
		
		protected override object Run( PersonasActivasGet request){
			
			DateTime? corte;
			if (request.FechaCorte!=default(DateTime))
				corte=	 request.FechaCorte;
			else 
				corte = null;
			var pa = DbFactory.PersonasActivas(corte);
			
			return new PersonasActivasGetResponse(){
				Success=true,
				PersonasActivas = pa
			};
		}
	}
	
	public class PersonasInactivasGetService:AuthServiceBase<PersonasInactivasGet>{
		
		protected override object Run( PersonasInactivasGet request){
			
			var pa = DbFactory.PersonasInactivas(request.DiasAusenciaTopeInferior, 
			                                     request.DiasAusenciaTopeSuperior);
			
			return new PersonasInactivasGetResponse(){
				Success=true,
				PersonasInactivas = pa
			};
		}
	}
	
	
	public class CajaConsolidadoGetService:AuthServiceBase<CajaConsolidadoGet>{
		
		public Mailer Mail {get;set;}
		
		protected  override object Run(CajaConsolidadoGet request){
			
			DateTime desde = request.Desde==default(DateTime) ?
				DateTime.Today.AddDays(-8):
					request.Desde;
			
			
			DateTime hasta = request.Hasta==default(DateTime) ?
				DateTime.Today.AddDays(-2):
					request.Hasta;
			
			var consolidado = DbFactory.CajaConsolidado(desde, hasta);
			var detalles = DbFactory.DeCajasClasificacion(desde, hasta, true);
			
			var response = new CajaConsolidadoGetResponse(){
				Success=true,
				HtmlResponse=consolidado.ToHtml(desde, hasta)+"<br/>"+ detalles.PagosToHtml()
				              +"<br/>"+ detalles.AnticiposToHtml()
			};
			
			if(request.SendMail){
				var uc = DbFactory.UsuariosCorreos("EstadoResultados.Consultar").
					Where(r=>!r.Correo.IsNullOrEmpty()).ToList();
				if (uc.Count>0){
				
					foreach(var r in uc){
						Mail.Message.To.Add(r.Correo);
					}
							
					Mail.Message.Subject = string.Format( "Estado de Resultados. Del {0} al {1}",
					                                     desde.ToString("dd.MM.yyyy"),
					                                     hasta.ToString("dd.MM.yyyy"));
					Mail.Message.IsBodyHtml=true;
					Mail.Message.Body = response.HtmlResponse;
					Mail.Send();
					Mail.Message.To.Clear();
				}
			}
			
			return response;		
			//return new HttpResult(response, "text/html");
		}
		
	}
	
	
	public class CajaCierreGetService:AuthServiceBase<CajaCierreGet>{
		
		public Mailer Mail {get;set;}
		
		private static readonly ILog Log = LogManager.GetLogger(typeof(CajaCierreGet));
		
		protected  override object Run(CajaCierreGet request){
			
			Caja caja = DbFactory.Caja(request.Fecha);
			
			if (caja==default(Caja) ){
				
				return new CajaCierreGetResponse(){
					Success=false,
					ResponseStatus= new ResponseStatus(){
						Message="No se encontro caja cerrada para: " + request.Fecha.ToString("dd.MM.yyyy"),
						ErrorCode="NoCajaAsentadaEnFecha"
					}
				};
			}
			
			List<DeCajaClasificacion> decajas = DbFactory.DeCajasClasificacion(caja.Id);
						
			CajaCierreGetResponse response = new CajaCierreGetResponse(){
				Success=true,
				HtmlResponse= decajas.ToHtml(caja.Fecha, caja.SaldoAnterior)
			};
			
			if(request.SendMail){
				
				try{
				
					var uc = DbFactory.UsuariosCorreos("Caja.Asentar").
						Where(r=>!r.Correo.IsNullOrEmpty()).ToList();
					if (uc.Count>0){
					
						foreach(var r in uc){
							Mail.Message.To.Add(r.Correo);
						}
								
						Mail.Message.Subject = string.Format( "Caja Cerrada {0}", caja.Fecha.ToString("dd.MM.yyyy") );
						Mail.Message.IsBodyHtml=true;
						Mail.Message.Body = response.HtmlResponse;
						Mail.Send();
						Mail.Message.To.Clear();
					}
					
					
				}
				catch(Exception e){
					Log.Error(e);
				}
			}
			
			return response;
			
		}
		
	}
	
	
	public class FacturasDiaDetalleGetService:AuthServiceBase<DetalleFacturasDiaGet>{
		
		public Mailer Mail {get;set;}
		
		private static readonly ILog Log = LogManager.GetLogger(typeof(DetalleFacturasDiaGet));
		
		
		protected  override object Run(DetalleFacturasDiaGet request){
			List<FacturaDiaDetalle> facturas = DbFactory.FacturasDiaDetalle(request.Fecha).
				OrderBy(r=>r.Numero).ToList();
			
			var response = new DetalleFacturasDiaGetResponse(){
				HtmlResponse= facturas.ToHtml(request.Fecha),
				Success=true
			};
			
			if(request.SendMail){
				
				try{
				
					var uc = DbFactory.UsuariosCorreos("Informe.FacturacionDia").
						Where(r=>!r.Correo.IsNullOrEmpty()).ToList();
					if (uc.Count>0){
					
						foreach(var r in uc){
							Mail.Message.To.Add(r.Correo);
						}
								
						Mail.Message.Subject = string.Format( "Facturacion del dia: {0}", request.Fecha.ToString("dd.MM.yyyy") );
						Mail.Message.IsBodyHtml=true;
						Mail.Message.Body = response.HtmlResponse;
						Mail.Send();
						Mail.Message.To.Clear();
					}
					
					
				}
				catch(Exception e){
					Log.Error(e);
				}
			}
			
			
			return response;
			
		}
		
	}

}

