using System;
using System.ComponentModel.DataAnnotations;
using ServiceStack.Common;
using ServiceStack.DataAnnotations;
using ServiceStack.OrmSimple;

namespace  SuperGym.Records  
{
 	[Alias("PAGOS")]
	public partial class Pagos{

 		public Pagos(){}

 		[Alias("IDPAGO")]
		[PrimaryKey]
		[Required]
		public System.Int32 Idpago { get; set;} 

		[Alias("IDFORMAPAGO")]
		public System.Int16? Idformapago { get; set;} 

		[Alias("IDTIPO_PAGO")]
		[Required]
		public System.Int16 IdtipoPago { get; set;} 

		[Alias("IDPERSONA")]
		[Required]
		public System.Int32 Idpersona { get; set;} 

		[Alias("NUMERO_FACTURA")]
		[Required]
		public System.String NumeroFactura { get; set;} 

		[Alias("FECHAPAGO")]
		[Required]
		public System.DateTime Fechapago { get; set;} 

		[Alias("FECHAINICIO")]
		[Required]
		public System.DateTime Fechainicio { get; set;} 

		[Alias("FECHATERMINACION")]
		[Required]
		public System.DateTime Fechaterminacion { get; set;} 

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
		public System.Int32 IdusuarioRegistra { get; set;} 

 	}
 }
