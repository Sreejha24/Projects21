using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;

namespace Practice21.ADONET
{

    public class ConnectToSQLQuery
    {
        private string connectionstring;
        private SqlConnection conn;
        public ConnectToSQLQuery()
        {
            connectionstring = "Data Source = LAPTOP - D2U4JDN5; Initial Catalog = sprathipati; Integrated Security = True; Persist Security Info = False; Pooling = False; MultipleActiveResultSets = False; Encrypt = False; TrustServerCertificate = False";
            conn = new SqlConnection(connectionstring);
        }
        public void InsertSqlQuery()
        {
            conn.Open();
            Console.WriteLine("Enter PersonID, FirstName, LastName, Email, Mobile, AddressID");
            int PersonID = int.Parse(Console.ReadLine());
            string FirstName = Console.ReadLine();
            string LastName = Console.ReadLine();
            string Email = Console.ReadLine();
            long Mobile = long.Parse(Console.ReadLine());
            int AddressID = int.Parse(Console.ReadLine());
            SqlCommand sqlcmd = new SqlCommand("INSERT INTO Person values (@PersonID,@FirstName,@LastName,@Email,@Mobile,@AddressID", conn);
            sqlcmd.Parameters.AddWithValue("@PersonID", PersonID);
            sqlcmd.Parameters.AddWithValue("@FirstName", FirstName);
            sqlcmd.Parameters.AddWithValue("L@LastName", LastName);
            sqlcmd.Parameters.AddWithValue("@Email", Email);
            sqlcmd.Parameters.AddWithValue("@Mobile", Mobile);
            sqlcmd.Parameters.AddWithValue("@AddressID", AddressID);
            
            sqlcmd.ExecuteNonQuery();
           conn.Close();
        }

    }
}