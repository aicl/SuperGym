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
using ServiceStack.Logging;

namespace SuperGym.Servicio.Personas.Interfaz
{
	
	public class PersonaFacturasService:AuthServiceBase<PersonaFacturasGet>{
		
		protected override object Run(PersonaFacturasGet request){
			
			if( request.Id== default (Int32)){
				
				if(request.Documento.IsNullOrEmpty()){
					return new PersonaFacturasGetResponse (){
						ResponseStatus= new ResponseStatus(){
							Message="Debe Indicar el numero de Documento",
							ErrorCode="SinDocumento"
						},
						Success=false
					};
				}
			
				Persona persona = DbFactory.Persona(request.Documento);
				if(persona==default(Persona) ){
					return new PersonaFacturasGetResponse (){
						ResponseStatus= new ResponseStatus(){
							Message=string.Format("No existe persona con Documento:'{0}'",
							                      request.Documento),
							ErrorCode="DocumentoNoExiste"
						},
						Success=false
					};
				}
				
				request.Id= persona.Id;
			}
			
			return new PersonaFacturasGetResponse(){
				Facturas= DbFactory.PersonaFacturas(request.Id),
				Success=true,
			};
			
		}
	}
	
	public class PersonaValidarService:AuthServiceBase<PersonaValidar>{
		
		private static readonly ILog Log = LogManager.GetLogger(typeof(PersonaValidarService));
		
		protected override object Run(PersonaValidar request){
			
			if(request.Documento.IsNullOrEmpty()){
				return new PersonaValidarResponse(){
					ResponseStatus= new ResponseStatus(){
						Message="Debe Indicar el numero de Documento",
						ErrorCode="SinDocumento"
					},
					Success=false
				};
			}
			
			Persona persona = DbFactory.Persona(request.Documento);
			if(persona==default(Persona) ){
				return new PersonaValidarResponse (){
					ResponseStatus= new ResponseStatus(){
						Message=string.Format("No existe persona con Documento:'{0}'",
						                      request.Documento),
						ErrorCode="DocumentoNoExiste"
					},
					Success=false
				};
			}
			Pago facturaVigente= DbFactory.FacturaVigente(persona.Id);
			if( facturaVigente== default(Pago)){
				return new PersonaValidarResponse(){
					ResponseStatus= new ResponseStatus(){
						Message="La persona se encuentra Inactiva",
						ErrorCode="NoActivo"
					},
					Success=false,
					Persona= persona
				};
			}
			
			// ver si tiene suspensiones
			
			var suspensiones = DbFactory.Suspensiones(facturaVigente.Id);
			if(suspensiones.Count>0){
				var s = (from r in suspensiones
						where r.Desde<=DateTime.Today && r.Hasta>=DateTime.Today
						orderby r.Hasta descending
						select r).FirstOrDefault();
				
				if( s != default(Suspension)){
					return new PersonaValidarResponse(){
						ResponseStatus= new ResponseStatus(){
							Message=string.Format( "Factura {0}\n Servicio suspendido entre {1} y {2}",
							                      facturaVigente.Numero,
							                      s.Desde.ToString("dd.MM.yyyy"), 
							                      s.Hasta.ToString("dd.MM.yyyy")),
							ErrorCode="Suspendido"
						},
						Success=false,
						Persona= persona
					};
				}
			}
			
			
			//Insertar ingreso, talves devolver ultimIngreso ?
			Ingreso ultimo = DbFactory.UltimoIngreso(persona.Id);
			
			string ultimoIngreso = ultimo==default(Ingreso) 
				? ""
				: ultimo.Fecha.ToString("dd.MM.yyyy HH:mm");
			
			Ingreso ingreso= new Ingreso(){
				IdPersona= persona.Id,
			};
							
			try{
				DbFactory.Exec(dbCmd => 
			    	dbCmd.Insert(ingreso) );
			}
			catch(Exception  e){
				Log.Error("Al insertar", e);
			}
						
			return new PersonaValidarResponse(){
				Success=true,
				Persona= persona,
				Factura= facturaVigente,
				UltimoIngreso = ultimoIngreso
			};
			
		}
	}
	
	public class PersonaGetService:AuthServiceBase<PersonaGet>
	{		
		protected override object Run(PersonaGet request)
		{
			
			if(request.Documento.IsNullOrEmpty()){
				return new PersonaGetResponse(){
					ResponseStatus= new ResponseStatus(){
						Message="Debe Indicar el numero de Documento",
						ErrorCode="SinDocumento"
					},
					Success=false
				};
			}
			
			return new PersonaGetResponse(){
				Persona= DbFactory.Persona(request.Documento),
				Success=true,
			};
		}
		
	}
	
	public class PersonaUpdateService:AuthServiceBase<PersonaUpdate>
	{
		
		protected override object Run(PersonaUpdate request)
		{
			
			request.IdUsuarioRegistra= Int16.Parse( Session.UserId );
			DbFactory.Exec(dbCmd => 
				    dbCmd.Update(request) );
			
			return new PersonaUpdateResponse(){
				ResponseStatus = new ResponseStatus(){
					Message="OK"
				},
				Success=true
			};
		}
		
	}
	
	
	public class PersonaInsertService:AuthServiceBase<PersonaInsert>{
		
		protected override object Run(PersonaInsert request)
		{
			
			request.IdUsuarioRegistra= Int16.Parse( Session.UserId );
			DbFactory.Exec(dbCmd => 
				    dbCmd.Insert(request) );
			
			return new PersonaInsertResponse (){
				Persona= request as Persona,
				Success=true,
			};
		}
	}
		
		
	
	
	public class PersonasGetService:AuthServiceBase<PersonasGet>
	{
		
		protected override object Run(PersonasGet request)
		{
			
			string filter= string.Empty;
			string valor = string.Format("%{0}%",  request.Valor.IsNullOrEmpty()? "":
			                             request.Valor.ToUpper());
			
			if(request.Criterio=="PrimerApellido"){
				filter="upper(PRIMER_APELLIDO)  like {0}";
			}
			else if(request.Criterio=="SegundoApellido")
				filter="upper( SEGUNDO_APELLIDO ) like {0}";
			else if(request.Criterio=="Nombres")
				filter="upper( NOMBRES ) like {0}";
			
						
			return new PersonasGetResponse(){
				Personas= DbFactory.Personas( filter, new object[1]{ valor}),
				Success=true,
				
			};
		}
		
	}
	
	
	
}

