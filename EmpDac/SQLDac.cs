using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace EmpDac
{
    public class SQLDac :IDac
    {
        private string connString;

        private SQLDac() { }
        public SQLDac(string sConn)
        {
            connString = sConn;
        }
        public object GetEmp(string fn, string ln)
        {
            if (string.IsNullOrEmpty(connString))
                throw new Exception("Error in GetEmp: Connection string is empty");
            DataTable dt = null;
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = connString;
                conn.Open();

                SqlCommand command = new SqlCommand();
                command = GetEmpCommandParameters(ref command, fn, ln);
                command.Connection = conn;
                string sql = "select *  from CBREmp where firstName = @fnm and lastName = @lnm";
                command.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception("Error In SQLDac (SQLException): Transaction Failure - GetEmp()", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error In SQLDac: GetEmp()", ex);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public object GetEmps()
        {
            if (string.IsNullOrEmpty(connString))
                throw new Exception("Error in GetEmps: Connection string is empty");
            DataTable dt = null;
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = connString;
                conn.Open();

                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                string sql = "select *  from CBREmp";
                command.CommandText = sql;

                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = command;
                dt = new DataTable();
                da.Fill(dt);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception("Error In SQLDac (SQLException): Transaction Failure - GetEmps()", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error In SQLDac: GetEmps()", ex);
            }
            finally
            {
                conn.Close();
            }
            return dt;
        }

        public bool InsertEmp(string ln, string fn, string man)
        {
            if (string.IsNullOrEmpty(connString))
                throw new Exception("Error in GetEmp: Connection string is empty");
            int n = 0;
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = connString;
                conn.Open();

                SqlCommand command = new SqlCommand();
                command = GetEmpCommandParameters(ref command, fn, ln, man);
                command.Connection = conn;
                string sql = "insert into CBREmp values(@fnm, @lnm, @man)";
                command.CommandText = sql;

                n = command.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception("Error In SQLDac (SQLException): Transaction Failure - InsertEmp()", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error In SQLDac: InsertEmp()", ex);
            }
            finally
            {
                conn.Close();
            }
            return n == 1;
        }

        public bool UpdateEmp(string ln, string fn, string man)
        {
            if (string.IsNullOrEmpty(connString))
                throw new Exception("Error in GetEmp: Connection string is empty");
            int n = 0;
            SqlConnection conn = new SqlConnection();
            try
            {
                conn.ConnectionString = connString;
                conn.Open();

                SqlCommand command = new SqlCommand();
                command = GetEmpCommandParameters(ref command, fn, ln, man);
                command.Connection = conn;
                string sql = "update CBREmp set Manager = @man where FirstName = @fnm and LastName = @lnm";
                command.CommandText = sql;

                n = command.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                throw new Exception("Error In SQLDac (SQLException): Transaction Failure - UpdateEmp()", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error In SQLDac: UpdateEmp()", ex);
            }
            finally
            {
                conn.Close();
            }
            return n == 1;
        }

        private SqlCommand GetEmpCommandParameters(ref SqlCommand command, string sFn, string sLn)
        {
            command.Parameters.Add("@fnm", SqlDbType.VarChar, 100);
            command.Parameters["@fnm"].Value = (sFn == null ? String.Empty : sFn);

            command.Parameters.Add("@lnm", SqlDbType.VarChar, 100);
            command.Parameters["@lnm"].Value = (sLn == null ? String.Empty : sLn);
            return command;
        }

        private SqlCommand GetEmpCommandParameters(ref SqlCommand command, string sFn, string sLn, string man)
        {
            command.Parameters.Add("@fnm", SqlDbType.VarChar, 100);
            command.Parameters["@fnm"].Value = (sFn == null ? String.Empty : sFn);

            command.Parameters.Add("@lnm", SqlDbType.VarChar, 100);
            command.Parameters["@lnm"].Value = (sLn == null ? String.Empty : sLn);

            command.Parameters.Add("@man", SqlDbType.VarChar, 100);
            command.Parameters["@man"].Value = (man == null ? String.Empty : man);

            return command;
        }
    }

    public class DacFactory
    {
        private string myDBType, connStr;

        public string MyDBType
        {
            get
            {
                return myDBType;
            }
            set
            {
                myDBType = value;
            }
        }

        public string ConnStr
        {
            get
            {
                return connStr;
            }
            set
            {
                connStr = value;
            }
        }

        private DacFactory() { }

        public DacFactory(string dbType, string conn)
        {
            MyDBType = dbType;
            ConnStr = conn;
        }

        public IDac GetDac(string myDBType, string connStr)
        {
            IDac dac = null;
            if (string.IsNullOrEmpty(myDBType))
                throw new Exception("Database Server type rquired."); // No database server type provided, cannot continue;
            if (string.IsNullOrEmpty(connStr))
                throw new Exception("Connection string is rquired."); // No connection string provided, cannot continue;
            if (myDBType.ToLower() == "sqlserver")
            {
                dac = new SQLDac(connStr) as IDac;
                return dac;
            }
            else
                throw new Exception("Data Access to be implemented."); // No dac defined yet, cannot continue;
        }
    }
}
