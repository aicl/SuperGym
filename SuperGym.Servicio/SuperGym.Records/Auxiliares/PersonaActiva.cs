using System;
namespace SuperGym.Records.Auxiliares
{
	public class PersonaActiva
	{
		public PersonaActiva ()
		{
		}
		
		public System.Int32 Id{get;set;}
		public String NombreCompleto{get; set;}
		public String Documento {get; set;}
		public DateTime FechaNacimiento {get; set;}
		public String Sexo {get; set;}
		public String Barrio {get; set;}
		public String Direccion {get; set;}
		public String Telefono {get; set;}
		public String Celular {get; set;}
		public String Email {get; set;}
		public String Empresa {get; set;}
		public System.String UltimaFactura {get; set;}
		public DateTime Inicio {get; set;}
		public DateTime Terminacion {get; set;}
		public Decimal Valor {get; set;}
		public String Observacion {get; set;}
		public DateTime UltimoIngreso {get; set;}
		public Int32 Entradas {get; set;}
		public Int32 DiasAusencia {get; set;}
		public String TipoPago {get; set;}
		
	}
}


