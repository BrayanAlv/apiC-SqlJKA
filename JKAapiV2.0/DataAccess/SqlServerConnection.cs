using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

class SqlServerConnection{
    
    #region variables
    private static string cs = @"
    Data Source = DESKTOP-AU7JV19\MSSQLSERVER01;
    Initial Catalog = JKASensorDataV2;
    Integrated Security = true;";

    private static SqlConnection connection;
    #endregion

    #region methods
    private static bool Open(){
        bool open = false;
        try {
            connection = new SqlConnection(cs);
            if(connection != null){
                connection.Open();
                open = true;
            }
            else{
                Console.WriteLine("The connection cannot be open");
            }
        }catch(SqlException e){
            Console.WriteLine(e.Message);
        }catch(ArgumentException e){
            Console.WriteLine(e.Message);
        }catch(Exception e){
            Console.WriteLine(e.Message);
        }
        return open;
    }

    /// <summary>
    /// Execute a query and returns the resulting data table
    /// </summary>
    /// <param name="command">SQL command</param>
    /// <returns></returns>
    public static DataTable ExecuteQuery(SqlCommand command){
        DataTable table = new DataTable();

        if(Open()){
            command.Connection = connection; // Set SQL Connection
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            try{
                adapter.Fill(table); // Execute query and populate table
            }catch(SqlException e){
                Console.WriteLine(e.Message);
            }
        }

        return table;
    }

    public static bool ExecuteInsert(SqlCommand command)
    {

        bool status = true;

        if (Open())
        {
            try
            {
                command.Connection = connection;
                command.ExecuteNonQuery(); // Execute query and populate table
                status = true;
            }
            catch (SqlException e)
            {
                //Console.WriteLine(e.Message);
                status = false;
            }
        }

        return status;
    }

    public static bool ExecuteNonQuery(SqlCommand command)
    {

        bool success = false;

        if (Open())
        {
            try
            {
                command.Connection = connection;
                command.ExecuteNonQuery(); // Execute query and populate table
                success = true;
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
                //status = false;
            }
        }

        return success;
    }
    #endregion
}