using Domain.Models;

namespace Aplication.Interfaces
{
    public interface IEmpresasRepository
    {
        IEnumerable<Empresa> ObterEmpresas();
    }
}
