using POSG_V1.ConexionDb;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace POSG_V1.DataAccess
{
    public class NotasClass
    {
        private readonly ConexionDB _dbConnection;

        public NotasClass()
        {
            _dbConnection = new ConexionDB();
        }

        // Método para obtener los periodos académicos desde la tabla SIG_PERIODOS
        public DataTable ObtenerPeriodosAcademicos()
        {
            string query = "SELECT DISTINCT strNombre_per FROM SIG_PERIODOS";
            using (SqlConnection connection = _dbConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        // Actualizando el método para obtener los nombres de maestría desde la tabla UB_CARRERAS
        public DataTable ObtenerNombreMaestria()
        {
            string query = "SELECT DISTINCT strNombre_Car FROM UB_CARRERAS";
            using (SqlConnection connection = _dbConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }


        public DataTable ObtenerNotas(string periodoAcademico, string nombreMaestria)
        {
            using (SqlConnection connection = _dbConnection.GetConnection())
            {
                using (SqlCommand command = new SqlCommand("POSG_GetNotas", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Comodin", "byPeriodo");
                    command.Parameters.AddWithValue("@FILTRO1", string.IsNullOrEmpty(periodoAcademico) ? DBNull.Value : (object)periodoAcademico);
                    command.Parameters.AddWithValue("@FILTRO2", string.IsNullOrEmpty(nombreMaestria) ? DBNull.Value : (object)nombreMaestria);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}
