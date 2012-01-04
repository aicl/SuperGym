using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("PAGOS")]
	public partial class Pago{

 		public Pago(){}

 		[Alias("IDPAGO")]
		[PrimaryKey]
		[Required]
		[Sequence("GEN_PAGOS_ID")]
		public System.Int32 Id { get; set;} 

		[Alias("IDFORMAPAGO")]
		public System.Int16 IdFormaPago { get; set;} 

		[Alias("IDTIPO_PAGO")]
		[Required]
		public System.Int16 IdTipoPago { get; set;} 

		[Alias("IDPERSONA")]
		[Required]
		public System.Int32 IdPersona { get; set;} 

		[Alias("NUMERO_FACTURA")]
		[Required]
		public System.String Numero { get; set;} 

		[Alias("FECHAPAGO")]
		[Required]
		public System.DateTime FechaPago { get; set;} 

		[Alias("FECHAINICIO")]
		[Required]
		public System.DateTime FechaInicio { get; set;} 

		[Alias("FECHATERMINACION")]
		[Required]
		public System.DateTime FechaTerminacion { get; set;} 

		[Alias("VALOR_UNITARIO")]
		[Required]
		public System.Decimal ValorUnitario { get; set;} 

		[Alias("CANTIDAD")]
		[Required]
		public System.Int16 Cantidad { get; set;} 

		[Alias("VALOR_TOTAL")]
		[Required]
		public System.Decimal ValorTotal { get; set;} 

		[Alias("OBSERVACION")]
		public System.String Observacion { get; set;} 

		[Alias("IDUSUARIO_REGISTRA")]
		[Required]
		public System.Int32 IdUsuarioRegistra { get; set;} 
		
		[Alias("ACTIVA")]
		public Boolean Activa { get; set;} 
		

 	}
 }
