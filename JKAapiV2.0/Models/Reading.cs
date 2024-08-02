using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class Reading
{
    private static string selectAll = @"SELECT ReadingId, SensorId, ReadingDate, ReadingTime, Temperature, Humidity, Counter
                                        FROM Readings ORDER BY ReadingDate, ReadingTime";

    private static string selectOne = @"SELECT ReadingId, SensorId, ReadingDate, ReadingTime, Temperature, Humidity, Counter
                                        FROM Readings WHERE ReadingId = @ReadingId";

    private static string insertOne = @"INSERT INTO Readings (SensorId, ReadingDate, ReadingTime, Temperature, Humidity, Counter)
                                        VALUES (@SensorId, @ReadingDate, @ReadingTime, @Temperature, @Humidity, @Counter)";

    public int ReadingId { get; set; }
    public int SensorId { get; set; }
    public DateTime ReadingDate { get; set; }
    public TimeSpan ReadingTime { get; set; }
    public float Temperature { get; set; }
    public float Humidity { get; set; }
    public int Counter { get; set; }

    public static List<Reading> Get()
    {
        SqlCommand command = new SqlCommand(selectAll);
        return Mapper.ToReadingList(SqlServerConnection.ExecuteQuery(command));
    }

    public static Reading Get(int readingId)
    {
        SqlCommand command = new SqlCommand(selectOne);
        command.Parameters.AddWithValue("@ReadingId", readingId);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        if (table.Rows.Count > 0)
            return Mapper.ToReading(table.Rows[0]);
        else
            throw new RecordNotFoundException("Reading", readingId.ToString());
    }

    public static bool Add(Reading reading)
    {
        SqlCommand command = new SqlCommand(insertOne);
        command.Parameters.AddWithValue("@SensorId", reading.SensorId);
        command.Parameters.AddWithValue("@ReadingDate", reading.ReadingDate);
        command.Parameters.AddWithValue("@ReadingTime", reading.ReadingTime);
        command.Parameters.AddWithValue("@Temperature", reading.Temperature);
        command.Parameters.AddWithValue("@Humidity", reading.Humidity);
        command.Parameters.AddWithValue("@Counter", reading.Counter);

        return SqlServerConnection.ExecuteInsert(command);
    }
}
