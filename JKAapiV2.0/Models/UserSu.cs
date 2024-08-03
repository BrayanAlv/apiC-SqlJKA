using System.Data;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using SqlCommand = System.Data.SqlClient.SqlCommand;

public class UserSU
{
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public string type { get; set; }


    // Constructor sin parámetros
    public UserSU() { }

    // Constructor con parámetros para inicializar las propiedades
    public UserSU(int userId, string firstName, string lastName, string middleName, string email, string password, string type)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Email = email;
        Password = password;
        type = "admin";

    }
    
    private static string selectAllUsersSU = @"
        SELECT UserId, FirstName, LastName, MiddleName, Email, Password
        FROM UsersSU
        ORDER BY LastName";


    public static List<UserSU> GetAllUsersSU()
    {
        List<UserSU> list = new List<UserSU>();
        using (SqlCommand command = new SqlCommand(selectAllUsersSU))
        {
            using DataTable table = SqlServerConnection.ExecuteQuery(command);

            foreach (DataRow row in table.Rows)
            {
                UserSU userSU = Mapper.ToUserSU(row);
                list.Add(userSU);
            }
        }
        return list;
    }
}