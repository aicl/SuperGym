using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("GRUPOS")]
	public partial class Grupo{

 		public Grupo(){}

 		[Alias("IDGRUPO")]
		[PrimaryKey]
		[Sequence("GEN_GRUPOS_ID")]
		[Required]
		public System.Int16 Id { get; set;} 

		[Alias("DESCRIPCION")]
		[Required]
		public System.String Descripcion { get; set;} 

		[Alias("NOMBRE")]
		[Required]
		[Index(true)]
		public System.String Nombre { get; set;} 
		
		[Alias("DIRECTORIO")]
		[Required]
		public System.String Directorio { get; set;} 
		
		[Alias("MENU")]
		[Required]
		public System.Boolean Menu { get; set;} 
		

 	}
 }
