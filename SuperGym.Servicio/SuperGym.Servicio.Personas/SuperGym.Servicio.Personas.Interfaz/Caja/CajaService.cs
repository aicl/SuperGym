using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using ServiceStack.OrmSimple;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;
using SuperGym.Servicio.Personas.Modelos;
using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
using SuperGym.Tablas;
using ServiceStack.Logging;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public class CajaGetService:AuthServiceBase<CajasGet>
	{
		protected override object Run(CajasGet request)
		{
			return new CajasGetResponse(){
				Success=true,
				Cajas= DbFactory.Cajas(request.Anio, request.Mes).OrderByDescending(r=> r.Fecha).ToList()
			};			
		}
	}
	
	public class CajaInsertService:AuthServiceBase<CajaInsert>
	{
		protected override object Run(CajaInsert request)
		{
		
			Caja caja = new Caja(){
				Fecha= request.Fecha
			};
			
			DbFactory.Exec(dbCmd => 
				    dbCmd.Insert(caja) );
			
			return new CajaInsertResponse (){
				Caja = caja,
				Success=true
				
			};
		}
	}
	
	
	public class CajaDeleteService:AuthServiceBase<CajaDelete>{
		
		protected override object Run(CajaDelete request){
			Caja caja = new Caja(){
				Id= request.Id
			};
			
			DbFactory.Exec( dbCmd =>
			               dbCmd.Delete(caja) );
			
			return new CajaDeleteResponse(){
				Success=true
			};
		}
	}
	
	public class CajaAsentarService:AuthServiceBase<CajaAsentar>{
		
		private static readonly ILog Log = LogManager.GetLogger(typeof(CajaAsentarService));
		
		private Mailer Mail {get;set;}
		
		public AppConfig Config { get; set;}
		
		protected override object Run(CajaAsentar request){
			AsentadorCaja	aCaja = new AsentadorCaja(){
				Id= request.Id,
				AsentadoPor= Int16.Parse( Session.UserId ),
				TrasladarA= request.TrasladarA
			};
			
			DbFactory.Exec( dbCmd =>
			               dbCmd.Update(aCaja));
			
			Caja caja = DbFactory.Caja(request.Id);
			
			try{
				
				var uc = DbFactory.UsuariosCorreos("Caja.Asentar").
					Where(r=>!r.Correo.IsNullOrEmpty()).ToList();
				if (uc.Count>0){
					Mail = new Mailer(Config);
					foreach(var r in uc){
						Mail.Message.To.Add(r.Correo);
					}
					
					Mail.Message.Subject = string.Format( "Caja Cerrada {0}", caja.Fecha.ToString("dd.MM.yyyy") );
					Mail.Message.IsBodyHtml=true;
					List<DeCajaClasificacion> decajas = DbFactory.DeCajasClasificacion(caja.Id);
					Mail.Message.Body = decajas.ToHtml(caja.Fecha, caja.SaldoAnterior);
					Mail.Send();
				}
				
				if( caja.Fecha.DayOfWeek == Config.DiaDeCierre){
					EnviarMailCajaConsolidado(caja.Fecha.AddDays(-6), caja.Fecha);
				}
				
				if (caja.Fecha.Day==  DateTime.DaysInMonth(caja.Fecha.Year, caja.Fecha.Month) ){
					EnviarMailCajaConsolidado(new DateTime(caja.Fecha.Year, caja.Fecha.Month,1),
						caja.Fecha);
				}
				
			}
			catch(Exception e){
				Log.Error(e);
			}
			
			
			return new CajaAsentarResponse(){
				Caja= caja,
				Success=true
			};
		}
		
		private void EnviarMailCajaConsolidado(DateTime desde, DateTime hasta){
			
			var uc = DbFactory.UsuariosCorreos("EstadoResultados.Consultar").
					Where(r=>!r.Correo.IsNullOrEmpty()).ToList();
			
			if(uc.Count>0){	
			
				var consolidado = DbFactory.CajaConsolidado(desde, hasta);
				var detalles = DbFactory.DeCajasClasificacion(desde, hasta, true);

				foreach(var r in uc){
					Mail.Message.To.Add(r.Correo);
				}
							
				Mail.Message.Subject = string.Format( "Estado de Resultados. Del {0} al {1}",
				                                     desde.ToString("dd.MM.yyyy"),
				                                     hasta.ToString("dd.MM.yyyy"));
				Mail.Message.IsBodyHtml=true;
				Mail.Message.Body = consolidado.ToHtml(desde, hasta)+"<br/>"+ detalles.PagosToHtml();
				Mail.Send();
				Mail.Message.To.Clear();
				
			}
			
			
		}
		
	}
	
	public class CajaDesasentarService:AuthServiceBase<CajaDesasentar>{
		
		private static readonly ILog Log = LogManager.GetLogger(typeof(CajaDesasentarService));
		
		public AppConfig Config { get; set;}
		
		private Mailer Mail {get;set;}
		
		protected override object Run(CajaDesasentar request){
			AsentadorCaja	aCaja = new AsentadorCaja(){
				Id= request.Id,
				AsentadoPor= null
			};
			
			DbFactory.Exec( dbCmd =>
			               dbCmd.Update(aCaja));
			
			Caja caja = DbFactory.Caja(request.Id);
			
			
			try{
				
				var uc = DbFactory.UsuariosCorreos("Caja.Desasentar");
				if (uc.Count>0){
					Mail = new Mailer(Config);
					foreach(var r in uc){
						Mail.Message.To.Add(r.Correo);
					}
					Mail.Message.Subject = string.Format( "Caja Abierta {0}", caja.Fecha.ToString("dd.MM.yyyy") );
					Mail.Message.IsBodyHtml=true;
					Mail.Message.Body = "";
					Mail.Send();
				}
			}
			catch(Exception e){
				Log.Error(e);
			}
			
			return new CajaDesasentarResponse(){
				Caja= caja,
				Success=true
			};
			
			
		}
	}
	

	public class ConceptosCajaGetService: AuthServiceBase<ConceptosCajaGet>{
		
		protected override object Run(ConceptosCajaGet request){
			
			var conceptos = DbFactory.Conceptos();
			
			return new ConceptosCajaGetResponse(){
				Success=true,
				Egresos= conceptos.Where(r=>r.Factor==-1).ToList(),
				Ingresos= conceptos.Where(r=>r.Factor==1).ToList()
			};
			
		}
		
	}
}

