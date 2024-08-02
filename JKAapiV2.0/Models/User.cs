using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

public class User
{
    private static string selectAll = @"SELECT UserId, FirstName, LastName, MiddleName, Company, Email, Password, Approval, Status
                                        FROM Users ORDER BY LastName";

    private static string selectOne = @"SELECT UserId, FirstName, LastName, MiddleName, Company, Email, Password, Approval, Status
                                        FROM Users WHERE UserId = @UserId";
    
    private static string selectByEmailPassword = @"SELECT UserId, FirstName, LastName, MiddleName, Company, Email, Password, Approval, Status
                                        FROM Users WHERE Email = @Email AND Password = @Password";

    private static string insertOne = @"INSERT INTO Users (FirstName, LastName, MiddleName, Company, Email, Password, Approval, Status)
                                        VALUES (@FirstName, @LastName, @MiddleName, @Company, @Email, @Password, @Approval, @Status)";
    
    private static string selectNotApproved = @"SELECT UserId, FirstName, LastName, MiddleName, Company, Email, Password, Approval, Status
                                                FROM Users WHERE Approval = 0 ORDER BY LastName";

    private static string updateApprovalStatus = @"UPDATE Users SET Approval = @Approval WHERE UserId = @UserId";

    
    private static string selectUserDetailsByEmail = @"
        SELECT 
            u.UserId as UserId,
            u.FirstName as FirstName,
            u.LastName as LastName,
            u.MiddleName as MiddleName,
            u.Company as Company,
            u.Email as Email,
            u.Approval as Approval,
            u.Status as Status,
            s.SubscriptionId as SubscriptionId,
            s.Folio as Folio,
            s.StartDate as StartDate,
            s.EndDate as EndDate,
            sn.SensorId as SensorId,
            sn.SerialNumber as SerialNumber,
            r.ReadingId as ReadingId,
            r.ReadingDate as ReadingDate,
            r.ReadingTime as ReadingTime,
            r.Temperature as Temperature,
            r.Humidity as Humidity,
            r.Counter as Counter,
            p.PaymentId as PaymentId,
            p.PaymentFolio as PaymentFolio,
            p.TransactionDate as TransactionDate,
            p.Total as Total,
            lh.LocationHistoryId as LocationHistoryId,
            lh.Longitude as Longitude,
            lh.Latitude as Latitude,
            lh.Timestamp as Timestamp
        FROM Users u
        INNER JOIN Subscriptions s ON u.UserId = s.UserId
        INNER JOIN Sensors sn ON s.SubscriptionId = sn.SubscriptionId
        INNER JOIN Readings r ON sn.SensorId = r.SensorId
        INNER JOIN Payments p ON s.SubscriptionId = p.SubscriptionId
        INNER JOIN LocationHistory lh ON sn.SensorId = lh.SensorId
        WHERE u.Email = @Email";
    
    private static string selectAllWithSensors = @"
        SELECT 
            u.UserId, u.FirstName, u.LastName, u.MiddleName, u.Company, u.Email, u.Password, u.Approval, u.Status,
            sn.SensorId, sn.SerialNumber
        FROM Users u
        LEFT JOIN Sensors sn ON u.UserId = sn.UserId
        ORDER BY u.LastName";


    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string MiddleName { get; set; }
    public string Company { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Approval { get; set; }
    public bool Status { get; set; }
    public List<Sensor> Sensors { get; set; }

    public User() { }

    public User(int userId, string firstName, string lastName, string middleName, string company, string email, string password, bool approval, bool status)
    {
        UserId = userId;
        FirstName = firstName;
        LastName = lastName;
        MiddleName = middleName;
        Company = company;
        Email = email;
        Password = password;
        Approval = approval;
        Status = status;
    }

    public static List<User> Get()
    {
        List<User> list = new List<User>();
        SqlCommand command = new SqlCommand(selectAll);
        return Mapper.ToUserList(SqlServerConnection.ExecuteQuery(command));
    }

    public static User Get(int userId)
    {
        using (SqlCommand command = new SqlCommand(selectOne))
        {
            command.Parameters.AddWithValue("@UserId", userId);
            DataTable table = SqlServerConnection.ExecuteQuery(command);

            if (table.Rows.Count > 0)
                return Mapper.ToUser(table.Rows[0]);
            else
                throw new RecordNotFoundException("User", userId.ToString());
        }
    }
    
    public static User GetByEmailPass(PostLogin login)
    {
        using (SqlCommand command = new SqlCommand(selectByEmailPassword))
        {
            command.Parameters.AddWithValue("@Email", login.Email);
            command.Parameters.AddWithValue("@Password", login.Password);
            DataTable table = SqlServerConnection.ExecuteQuery(command);

            if (table.Rows.Count > 0)
                return Mapper.ToUser(table.Rows[0]);
            else
                throw new RecordNotFoundException("User", login.Email);
        }
    }

    public static bool Add(PostUser user)
    {
        using (SqlCommand command = new SqlCommand(insertOne))
        {
            command.Parameters.AddWithValue("@FirstName", user.FirstName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@LastName", user.LastName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@MiddleName", user.MiddleName ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Company", user.Company ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
            command.Parameters.AddWithValue("@Password", user.Password ?? (object)DBNull.Value); 
            command.Parameters.AddWithValue("@Approval", user.Approval);
            command.Parameters.AddWithValue("@Status", user.Status);

            return SqlServerConnection.ExecuteInsert(command);
        }
    }
    
    public static DetailedUser GetDetailedUserByEmail(string email)
    {
        using (SqlCommand command = new SqlCommand(selectUserDetailsByEmail))
        {
            command.Parameters.AddWithValue("@Email", email);
            DataTable table = SqlServerConnection.ExecuteQuery(command);

            return Mapper.ToDetailedUser(table);
        }
    }
    
    public static List<User> GetAllWithSensors()
    {
        List<User> list = new List<User>();
        using (SqlCommand command = new SqlCommand(selectAllWithSensors))
        {
            DataTable table = SqlServerConnection.ExecuteQuery(command);

            Dictionary<int, User> userDictionary = new Dictionary<int, User>();

            foreach (DataRow row in table.Rows)
            {
                int userId = (int)row["UserId"];
                if (!userDictionary.ContainsKey(userId))
                {
                    User user = Mapper.ToUser(row);
                    user.Sensors = new List<Sensor>();
                    userDictionary[userId] = user;
                }

                if (row["SensorId"] != DBNull.Value)
                {
                    Sensor sensor = Mapper.ToSensor(row);
                    userDictionary[userId].Sensors.Add(sensor);
                }
            }

            list.AddRange(userDictionary.Values);
        }
        return list;
    }
    
    public static List<User> GetNotApproved()
    {
        List<User> list = new List<User>();
        SqlCommand command = new SqlCommand(selectNotApproved);
        return Mapper.ToUserList(SqlServerConnection.ExecuteQuery(command));
    }
    public static bool UpdateApprovalStatus(int userId, bool approval)
    {
        using (SqlCommand command = new SqlCommand(updateApprovalStatus))
        {
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@Approval", approval);
            return SqlServerConnection.ExecuteInsert(command);
        }
    }

    
}
