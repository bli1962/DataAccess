
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace mhcb.cs.Shared.DataAccess
{
	public interface IDataExecution
	{
		//new string getDefaultConnectionString();
		//new string getConnectionString(Boolean blnIsLoading);

		object ExecuteScalar(SqlCommand cmd);
		object ExecuteScalar(SqlCommand cmd, SqlConnection conn);

		void ExecuteNonQuery(SqlCommand cmd);
		void ExecuteNonQuery(SqlCommand cmd, SqlConnection conn);

		SqlDataReader ExecuteDataReader(string sqlString);
		SqlDataReader ExecuteDataReader(string sqlString, SqlConnection conn);

		SqlDataReader ExecuteDataReader(SqlCommand cmd);
		SqlDataReader ExecuteDataReader(SqlCommand cmd, SqlConnection conn);

		DataSet ExecuteDataSet(SqlCommand cmd);
		DataSet ExecuteDataSet(SqlCommand cmd, SqlConnection conn);
		DataSet ExecuteDataSet(SqlCommand cmd, string strTable);
		DataSet ExecuteDataSet(SqlCommand cmd, SqlConnection conn, string strTable);

		OleDbDataReader ExecuteDataReader(OleDbCommand cmd);
		OleDbDataReader OleDbExecuteReader(OleDbCommand cmd);
		OleDbDataReader OleDbExecuteReader(string sqlString);
		
		DataSet ExecuteDataSet(OleDbCommand cmd);
		DataSet ExecuteDataSet(OleDbCommand cmd, string strTable);
		void ExecuteNonQuery(OleDbCommand cmd);
		void OleDbExecuteNonQuery (OleDbCommand cmd);

		SqlConnection SetSqlConnection();
		OleDbConnection SetOleDbConnection();
	}
}