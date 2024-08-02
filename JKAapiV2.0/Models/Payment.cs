using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Payment
{
    private static string selectAll = @"SELECT PaymentFolio, SubscriptionFolio, TransactionDate, Total
                                        FROM Payments ORDER BY TransactionDate";

    private static string selectOne = @"SELECT PaymentFolio, SubscriptionFolio, TransactionDate, Total
                                        FROM Payments WHERE PaymentFolio = @PaymentFolio";

    private static string insertOne = @"INSERT INTO Payments (SubscriptionFolio, TransactionDate, Total)
                                        VALUES (@SubscriptionFolio, @TransactionDate, @Total)";

    private static string selectByEmail = @"SELECT p.PaymentFolio, p.SubscriptionFolio, p.TransactionDate, p.Total
                                            FROM Payments p
                                            JOIN Subscriptions s ON p.SubscriptionFolio = s.Folio
                                            JOIN Users u ON s.UserId = u.UserId
                                            WHERE u.Email = @Email
                                            ORDER BY p.TransactionDate DESC";

    public int PaymentFolio { get; set; }
    public int SubscriptionFolio { get; set; }
    public DateTime TransactionDate { get; set; }
    public float Total { get; set; }

    public static List<Payment> Get()
    {
        SqlCommand command = new SqlCommand(selectAll);
        return Mapper.ToPaymentList(SqlServerConnection.ExecuteQuery(command));
    }

    public static Payment Get(int paymentFolio)
    {
        SqlCommand command = new SqlCommand(selectOne);
        command.Parameters.AddWithValue("@PaymentFolio", paymentFolio);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        if (table.Rows.Count > 0)
            return Mapper.ToPayment(table.Rows[0]);
        else
            throw new RecordNotFoundException("Payment", paymentFolio.ToString());
    }

    public static bool Add(Payment payment)
    {
        SqlCommand command = new SqlCommand(insertOne);
        command.Parameters.AddWithValue("@SubscriptionFolio", payment.SubscriptionFolio);
        command.Parameters.AddWithValue("@TransactionDate", payment.TransactionDate);
        command.Parameters.AddWithValue("@Total", payment.Total);

        return SqlServerConnection.ExecuteInsert(command);
    }

    public static List<Payment> GetRecentPaymentsByEmail(string email)
    {
        SqlCommand command = new SqlCommand(selectByEmail);
        command.Parameters.AddWithValue("@Email", email);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        return Mapper.ToPaymentList(table);
    }
}
