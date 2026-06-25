using CapaEntidades;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
public class VentaDAL
{
    private readonly string _connectionString;

    public VentaDAL(string connectionString)
    {
        _connectionString = connectionString;
    }

    public int RegistrarVenta(Ventas venta)
    {
        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("sp_RegistrarVenta", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@CodCliente", venta.CodCliente);
        cmd.Parameters.AddWithValue("@CodUsuario", venta.CodUsuario);

        DataTable dt = new DataTable();
        dt.Columns.Add("CodigoProducto", typeof(int));
        dt.Columns.Add("Cantidad", typeof(int));
        dt.Columns.Add("PrecioUnitario", typeof(decimal));
        dt.Columns.Add("Descuento", typeof(decimal));

        foreach (var item in venta.Detalles)
        {
            dt.Rows.Add(
                item.CodigoProducto,
                item.Cantidad,
                item.PrecioUnitario,
                item.Descuento
            );
        }

        var param = cmd.Parameters.AddWithValue("@Detalles", dt);
        param.SqlDbType = SqlDbType.Structured;

        conn.Open();

        var result = cmd.ExecuteScalar();

        return Convert.ToInt32(result);
    }
}