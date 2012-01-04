/*
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace SuperGym.Records
{
	[Alias("USUARIOS")]
	public partial class User{

 		public User(){
			//Grupos = new List<short>();
		}

 		[Alias("IDUSUARIO")]
		[PrimaryKey]
		[Required]
		[Sequence("GEN_USUARIOS_ID")]
		public System.Int16 Id { get; set;} 

		[Alias("NOMBRE")]
		[Required]
		[Index(true)]
		public System.String Nombre { get; set;} 
				
		[Alias("CORREO")]
		[Required]
		public System.String Correo { get; set;} 
		
		//[Ignore]
		//public List<System.Int16> Grupos { get; set;} 
		
 	}
}

*/