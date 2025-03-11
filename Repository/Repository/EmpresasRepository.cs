using Aplication.Interfaces;
using Dapper;
using Domain.Models;
using Infra.Connection;

namespace Infra.Repository
{
    public class EmpresasRepository : IEmpresasRepository
    {
        private readonly DatabaseConnection _connection;

        public EmpresasRepository()
        {
            _connection = new DatabaseConnection();
        }

        public IEnumerable<Empresa> ObterEmpresas()
        {
            string query = "SELECT * FROM empresasdfe";

            using (var connection = _connection.OpenConnection())
            {
                return connection.Query<Empresa>(query);
            }
        }
    }
}
