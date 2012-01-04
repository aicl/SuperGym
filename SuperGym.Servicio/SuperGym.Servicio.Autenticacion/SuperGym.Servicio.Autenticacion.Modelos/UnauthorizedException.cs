using System;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class UnauthorizedException:Exception
	{
		public UnauthorizedException (string message):base(message)
		{
		}
	}
}