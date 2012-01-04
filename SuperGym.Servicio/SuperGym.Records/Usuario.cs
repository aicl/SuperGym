using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("USUARIOS")]
	public partial class Usuario{

 		public Usuario(){}

 		[Alias("IDUSUARIO")]
		[PrimaryKey]
		[Required]
		[Sequence("GEN_USUARIOS_ID")]
		public System.Int16 Id { get; set;} 

		[Alias("NOMBRE")]
		[Required]
		[Index(true)]
		public System.String Nombre { get; set;} 

		[Alias("CLAVE")]
		[Required]
		public System.String Clave { get; set;} 

		[Alias("IDPERSONA")]
		public System.Int32? IdPersona { get; set;} 
		
		[Alias("CORREO")]
		[Required]
		public System.String Correo { get; set;} 
		
		[Alias("ACTIVO")]
		public bool Activo { get; set;} 
				
		
 	}
}



