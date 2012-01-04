using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("TIPO_DOCUMENTO")]
	public partial class TipoDocumento{

 		public TipoDocumento(){}

 		[Alias("IDTIPO_DOCUMENTO")]
		[PrimaryKey]
		[Required]
		public System.Int16 IdtipoDocumento { get; set;} 

		[Alias("DESCRIPCION_TIPO_DOCUMENTO")]
		[Required]
		public System.String DescripcionTipoDocumento { get; set;} 

 	}
 }
