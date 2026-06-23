using Capa_Datos;
using CapaEntidades;

namespace CapaNegocio
{
    public class ClienteNegocio
    {
        private readonly ClienteDatos _clienteDatos;

        public ClienteNegocio(string connectionString)
        {
            _clienteDatos = new ClienteDatos(connectionString);
        }

        public bool CrearCliente(Cliente cliente) => _clienteDatos.CrearCliente(cliente);

        public bool ActualizarCliente(Cliente cliente) => _clienteDatos.ActualizarCliente(cliente);

        public bool EliminarCliente(int codCliente) => _clienteDatos.EliminarCliente(codCliente);

        public List<Cliente> ObtenerClientePorId(int codCliente) => _clienteDatos.ObtenerClientePorId(codCliente);

        public List<Cliente> ObtenerTodosClientes() => _clienteDatos.ObtenerTodosClientes();
    }
}