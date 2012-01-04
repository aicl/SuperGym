using System;
using System.IO;
using System.Data;
using System.Linq;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;
using ServiceStack.Common.Extensions;
using ServiceStack.Common.Web;

using ServiceStack.OrmSimple;

using SuperGym.Servicio.Autenticacion.Modelos;
using SuperGym.Records;
using SuperGym.Records.Auxiliares;
using SuperGym.Tablas;

using ServiceStack.ServiceHost;

using SuperGym.Servicio.Personas.Modelos;

namespace SuperGym.Servicio.Personas.Interfaz
{
	public class FotoService:AuthServiceBase<FotoUpload>
	{
		public AppConfig Config { get; set;}
		
		protected override object Run(FotoUpload request)
		{
			
			string message="OK";
			
			if(RequestContext.Files.Count()>0){
				try{
					var uploadedFile =RequestContext.Files[0];
					var newFilePath = Path.Combine(Config.PhotoDirectory, 
				                               request.Id.ToString()+".jpg" );
					uploadedFile.SaveTo(newFilePath);
				}
				catch(Exception e ){
					message=e.Message;
				}
			}
			else{
				message= "Sin Archivo";
			}
			
			
			
			var response= new FotoUploadResponse (){
				ResponseStatus = new ResponseStatus(){
					Message=message
				},
				success= (message=="OK")?true:false,
			};
			
			return  new HttpResult(response, "text/html" );
		}
		
	}
	
	
}

/*
 * ss = true,
            result = this.processResponse(response);
        if (result !== true && !result.success) {
            if (result.errors) {
                form.markInvalid(result.errors);
 */


