using System;
namespace SuperGym.Servicio.Autenticacion.Modelos
{
	public interface IRequireAuthentication
	{
		Guid SessionId { get; }
		
	}
}

