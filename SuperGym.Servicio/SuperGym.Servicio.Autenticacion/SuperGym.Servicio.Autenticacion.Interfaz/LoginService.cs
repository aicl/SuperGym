using System;
using System.Linq;
using ServiceStack.ServiceInterface.ServiceModel;

using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
using SuperGym.Tablas;
namespace SuperGym.Servicio.Autenticacion.Interfaz
{
	public class LoginService:AuthServiceBase<LoginData>
	{
		
		protected override object Run(LoginData request)
		{

			ResponseStatus r =null;
			
			if (string.IsNullOrEmpty( request.Password) )
				r= new ResponseStatus(){
					ErrorCode="ArgumentNullException",
					Message= "La clave no puede estar vacia"
				};
			
			if (string.IsNullOrEmpty( request.UserName) )
				r= new ResponseStatus(){
					ErrorCode="ArgumentNullException",
					Message= "el nombre de usuario no puede estar vacio"
				};

			if( r != null) 
				return new LoginResponse(){
					ResponseStatus= r
				};
					
			
			Session =null;
			try{
				Session = AuthProvider.CreateSession(request, base.RequestContext.IpAddress, 
			                                                  base.RequestContext.GetHeader("User-Agent"));
			}
			catch(UnauthenticatedException e){
				r= new ResponseStatus(){
					ErrorCode="UnauthenticatedException",
					Message= e.Message
				};
			}
			catch (Exception e){
		
				r= new ResponseStatus(){
					ErrorCode="Exception",
					Message= e.Message
				};
			}

			
			if( r != null) 
				return new LoginResponse(){
					ResponseStatus= r
				};
			
			
			Permisos permisos = DbFactory.Permisos(Int16.Parse( Session.UserId) );
			
			
			Session.Add<DatosUsuario>(new DatosUsuario(){
				Actividades=permisos.Actividades,
				Id= Int16.Parse( Session.UserId) 
			});
			
			
			return new LoginResponse(){
				Id= base.Session.Id,
				ResponseStatus= new ResponseStatus(){
					Message="Bienvenido"
				},
				Success=true,
				Grupos= permisos.Grupos.Where(rg=> rg.Menu).ToList(),
				Actividades= 
					(from ra in permisos.Actividades
						select ra.Nombre).Distinct().ToList()
				
			};
			
		}
		
	}	
	
}

