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
        param.TypeName = "dbo.DetalleVentaType";

        conn.Open();

        var result = cmd.ExecuteScalar();

        return Convert.ToInt32(result);
    }

    public List<VentaListado> ListarVentas()
    {
        var ventas = new List<VentaListado>();

        using SqlConnection conn = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("sp_ListarVentas", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        conn.Open();
        using SqlDataReader dr = cmd.ExecuteReader();

        // Resultado 1: ventas
        while (dr.Read())
        {
            ventas.Add(new VentaListado
            {
                CodVenta = dr.GetInt32(dr.GetOrdinal("CodVenta")),
                CodCliente = dr.IsDBNull(dr.GetOrdinal("CodCliente")) ? null : dr.GetInt32(dr.GetOrdinal("CodCliente")),
                CodUsuario = dr.IsDBNull(dr.GetOrdinal("CodUsuario")) ? null : dr.GetInt32(dr.GetOrdinal("CodUsuario")),
                FechaVenta = dr.IsDBNull(dr.GetOrdinal("FechaVenta")) ? null : dr.GetDateTime(dr.GetOrdinal("FechaVenta")),
                Total = dr.GetDecimal(dr.GetOrdinal("Total"))
            });
        }

        // Resultado 2: detalles
        if (dr.NextResult())
        {
            var porVenta = ventas.ToDictionary(v => v.CodVenta);

            while (dr.Read())
            {
                var detalle = new DetalleVentaListado
                {
                    CodDetalleVenta = dr.GetInt32(dr.GetOrdinal("CodDetalleVenta")),
                    CodVenta = dr.GetInt32(dr.GetOrdinal("CodVenta")),
                    CodigoProducto = dr.IsDBNull(dr.GetOrdinal("CodigoProducto")) ? null : dr.GetInt32(dr.GetOrdinal("CodigoProducto")),
                    Cantidad = dr.GetInt32(dr.GetOrdinal("Cantidad")),
                    PrecioUnitario = dr.GetDecimal(dr.GetOrdinal("PrecioUnitario")),
                    Descuento = dr.GetDecimal(dr.GetOrdinal("Descuento")),
                    Subtotal = dr.GetDecimal(dr.GetOrdinal("Subtotal"))
                };

                if (porVenta.TryGetValue(detalle.CodVenta, out var venta))
                    venta.Detalles.Add(detalle);
            }
        }

        return ventas;
    }
}