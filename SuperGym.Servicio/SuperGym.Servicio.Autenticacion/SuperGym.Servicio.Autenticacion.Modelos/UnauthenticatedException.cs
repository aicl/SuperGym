using System;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public class UnauthenticatedException:Exception
	{
		public UnauthenticatedException(string message):base(message)
		{
		}
	}

}