using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Subscription
{
    private static string selectAll = @"SELECT 
	                                        u.FirstName,
	                                        u.LastName,
	                                        u.MiddleName,
	                                        u.Email,
	                                        s.Folio, 
	                                        s.StartDate, 
	                                        s.EndDate 
                                        FROM 
	                                        Subscriptions s
                                        INNER JOIN 
	                                        Users u ON s.UserId = u.UserId
                                        ORDER BY StartDate";

    private static string selectOne = @"SELECT SubscriptionId, UserId, Folio, StartDate, EndDate
                                        FROM Subscriptions WHERE SubscriptionId = @SubscriptionId";

    private static string insertOne = @"INSERT INTO Subscriptions (UserId, EndDate)
                                        VALUES (@UserId, @EndDate)";

    private static string selectSubscriptionsByEmail = @"SELECT 
                                                            u.Email as Email,
                                                            s.Folio as Folio,
                                                            s.EndDate as EndDate
                                                        FROM 
                                                            Subscriptions s
                                                        INNER JOIN 
                                                            Users u ON s.UserId = u.UserId
                                                        WHERE 
                                                            u.Email = @Email";

    public int SubscriptionId { get; set; }
    public int UserId { get; set; }
    public string Folio { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public Subscription(int subscriptionId, int userId, string folio, DateTime startDate, DateTime endDate)
    {
        //SubscriptionId = subscriptionId;
        UserId = userId;
        Folio = folio;
        StartDate = startDate;
        EndDate = endDate;
    }

    public Subscription()
    {
        // Constructor sin parámetros implementado
    }

    public static List<SubscriptionDetail> Get()
    {
        SqlCommand command = new SqlCommand(selectAll);
        DataTable table = SqlServerConnection.ExecuteQuery(command);
        return Mapper.ToSubscriptionDetailList(table);
    }

    public static Subscription Get(int subscriptionId)
    {
        SqlCommand command = new SqlCommand(selectOne);
        command.Parameters.AddWithValue("@SubscriptionId", subscriptionId);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        if (table.Rows.Count > 0)
            return Mapper.ToSubscription(table.Rows[0]);
        else
            throw new RecordNotFoundException("Subscription", subscriptionId.ToString());
    }

    public static bool Add(Subscription subscription)
    {
        SqlCommand command = new SqlCommand(insertOne);
        command.Parameters.AddWithValue("@UserId", subscription.UserId);
        command.Parameters.AddWithValue("@Folio", subscription.Folio);
        command.Parameters.AddWithValue("@StartDate", subscription.StartDate);
        command.Parameters.AddWithValue("@EndDate", subscription.EndDate);

        return SqlServerConnection.ExecuteInsert(command);
    }

    public static List<SubscriptionDetail> GetSubscriptionsByEmail(string email)
    {
        List<SubscriptionDetail> subscriptions = new List<SubscriptionDetail>();

        SqlCommand command = new SqlCommand(selectSubscriptionsByEmail);
        command.Parameters.AddWithValue("@Email", email);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        foreach (DataRow row in table.Rows)
        {
            subscriptions.Add(new SubscriptionDetail
            {
                Email = row.Field<string>("Email"),
                Folio = row.Field<string>("Folio"),
                EndDate = row.Field<DateTime>("EndDate")
            });
        }

        return subscriptions;
    }
}


