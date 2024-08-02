using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class LocationHistory
{
    private static string selectAll = @"SELECT LocationHistoryId, SensorId, Longitude, Latitude, Timestamp
                                        FROM LocationHistory ORDER BY Timestamp";

    private static string selectOne = @"SELECT LocationHistoryId, SensorId, Longitude, Latitude, Timestamp
                                        FROM LocationHistory WHERE LocationHistoryId = @LocationHistoryId";

    private static string insertOne = @"INSERT INTO LocationHistory (SensorId, Longitude, Latitude, Timestamp)
                                        VALUES (@SensorId, @Longitude, @Latitude, @Timestamp)";

    private static string bySn = @"SELECT lh.LocationHistoryId, lh.SensorId, lh.Longitude, lh.Latitude, lh.Timestamp
                                        FROM LocationHistory lh
                                        INNER JOIN Sensors s ON lh.SensorId = s.SensorId
                                        WHERE s.SerialNumber = @SerialNumber
                                        ORDER BY lh.Timestamp";
    public int LocationHistoryId { get; set; }
    public int SensorId { get; set; }
    public decimal Longitude { get; set; }
    public decimal Latitude { get; set; }
    public DateTime Timestamp { get; set; }

    public static List<LocationHistory> Get()
    {
        SqlCommand command = new SqlCommand(selectAll);
        return Mapper.ToLocationHistoryList(SqlServerConnection.ExecuteQuery(command));
    }

    public static LocationHistory Get(int locationHistoryId)
    {
        SqlCommand command = new SqlCommand(selectOne);
        command.Parameters.AddWithValue("@LocationHistoryId", locationHistoryId);
        DataTable table = SqlServerConnection.ExecuteQuery(command);

        if (table.Rows.Count > 0)
            return Mapper.ToLocationHistory(table.Rows[0]);
        else
            throw new RecordNotFoundException("LocationHistory", locationHistoryId.ToString());
    }

    public static bool Add(LocationHistory locationHistory)
    {
        SqlCommand command = new SqlCommand(insertOne);
        command.Parameters.AddWithValue("@SensorId", locationHistory.SensorId);
        command.Parameters.AddWithValue("@Longitude", locationHistory.Longitude);
        command.Parameters.AddWithValue("@Latitude", locationHistory.Latitude);
        command.Parameters.AddWithValue("@Timestamp", locationHistory.Timestamp);

        return SqlServerConnection.ExecuteInsert(command);
    }
    
    public static List<LocationHistory> GetBySerialNumber(string serialNumber)
    {
        SqlCommand command = new SqlCommand(selectAll);
        command.Parameters.AddWithValue("@SerialNumber", bySn);
        return Mapper.ToLocationHistoryList(SqlServerConnection.ExecuteQuery(command));
    }
}
