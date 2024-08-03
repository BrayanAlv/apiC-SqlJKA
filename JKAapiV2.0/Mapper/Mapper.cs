using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using JKAapiV2._0.DTOs;

public static class Mapper
{
    // Converts a DataRow to a User
    public static User ToUser(DataRow r)
    {
        return new User
        {
            UserId = r["UserId"] != DBNull.Value ? Convert.ToInt32(r["UserId"]) : 0,
            FirstName = r["FirstName"] != DBNull.Value ? r["FirstName"].ToString() : string.Empty,
            LastName = r["LastName"] != DBNull.Value ? r["LastName"].ToString() : string.Empty,
            MiddleName = r["MiddleName"] != DBNull.Value ? r["MiddleName"].ToString() : null,
            Company = r["Company"] != DBNull.Value ? r["Company"].ToString() : null,
            Password = r["Password"] != DBNull.Value ? r["Password"].ToString() : null,
            Email = r["Email"] != DBNull.Value ? r["Email"].ToString() : string.Empty,
            Approval = r["Approval"] != DBNull.Value ? Convert.ToBoolean(r["Approval"]) : false,
            Status = r["Status"] != DBNull.Value ? Convert.ToBoolean(r["Status"]) : false,
            type = "user"
        };
    }

    // Converts a DataTable to a List of User
    public static List<User> ToUserList(DataTable table)
    {
        return table.AsEnumerable().Select(ToUser).ToList();
    }

    // Converts a DataRow to a Subscription
    public static Subscription ToSubscription(DataRow r)
    {
        return new Subscription
        {
            UserId = r["UserId"] != DBNull.Value ? Convert.ToInt32(r["UserId"]) : 0,
            Folio = r["Folio"] != DBNull.Value ? r["Folio"].ToString() : string.Empty,
            StartDate = r["StartDate"] != DBNull.Value ? Convert.ToDateTime(r["StartDate"]) : DateTime.MinValue,
            EndDate = r["EndDate"] != DBNull.Value ? Convert.ToDateTime(r["EndDate"]) : DateTime.MinValue
        };
    }

    // Converts a DataTable to a List of Subscription
    public static List<Subscription> ToSubscriptionList(DataTable table)
    {
        return table.AsEnumerable().Select(ToSubscription).ToList();
    }

    // Converts a DataRow to a Sensor
    public static Sensor ToSensor(DataRow r)
    {
        return new Sensor
        {
            SensorId = r["SensorId"] != DBNull.Value ? Convert.ToInt32(r["SensorId"]) : 0,
            SerialNumber = r["SerialNumber"] != DBNull.Value ? r["SerialNumber"].ToString() : string.Empty,
            UserId = r["UserId"] != DBNull.Value ? Convert.ToInt32(r["UserId"]) : 0
        };
    }

    // Converts a DataTable to a List of Sensor
    public static List<Sensor> ToSensorList(DataTable table)
    {
        return table.AsEnumerable().Select(ToSensor).ToList();
    }

    // Converts a DataRow to a Reading
    public static Reading ToReading(DataRow r)
    {
        return new Reading
        {
            ReadingId = r["ReadingId"] != DBNull.Value ? Convert.ToInt32(r["ReadingId"]) : 0,
            SensorId = r["SensorId"] != DBNull.Value ? Convert.ToInt32(r["SensorId"]) : 0,
            ReadingDate = r["ReadingDate"] != DBNull.Value ? Convert.ToDateTime(r["ReadingDate"]) : DateTime.MinValue,
            ReadingTime = r["ReadingTime"] != DBNull.Value ? TimeSpan.Parse(r["ReadingTime"].ToString()) : TimeSpan.Zero,
            Temperature = r["Temperature"] != DBNull.Value ? Convert.ToSingle(r["Temperature"]) : 0f,
            Humidity = r["Humidity"] != DBNull.Value ? Convert.ToSingle(r["Humidity"]) : 0f,
            Counter = r["Counter"] != DBNull.Value ? Convert.ToInt32(r["Counter"]) : 0
        };
    }

    // Converts a DataTable to a List of Reading
    public static List<Reading> ToReadingList(DataTable table)
    {
        return table.AsEnumerable().Select(ToReading).ToList();
    }

    // Converts a DataRow to a Payment
    public static Payment ToPayment(DataRow r)
    {
        return new Payment
        {
            PaymentFolio = r["PaymentFolio"] != DBNull.Value ? Convert.ToInt32(r["PaymentFolio"]) : 0,
            SubscriptionFolio = r["SubscriptionFolio"] != DBNull.Value ? Convert.ToInt32(r["SubscriptionFolio"]) : 0,
            //PaymentFolio = r["PaymentFolio"] != DBNull.Value ? r["PaymentFolio"].ToString() : string.Empty,
            TransactionDate = r["TransactionDate"] != DBNull.Value ? Convert.ToDateTime(r["TransactionDate"]) : DateTime.MinValue,
            Total = r["Total"] != DBNull.Value ? Convert.ToSingle(r["Total"]) : 0f
        };
    }

    // Converts a DataTable to a List of Payment
    public static List<Payment> ToPaymentList(DataTable table)
    {
        return table.AsEnumerable().Select(ToPayment).ToList();
    }

    // Converts a DataRow to a LocationHistory
    public static LocationHistory ToLocationHistory(DataRow r)
    {
        return new LocationHistory
        {
            LocationHistoryId = r["LocationHistoryId"] != DBNull.Value ? Convert.ToInt32(r["LocationHistoryId"]) : 0,
            SensorId = r["SensorId"] != DBNull.Value ? Convert.ToInt32(r["SensorId"]) : 0,
            Longitude = r["Longitude"] != DBNull.Value ? Convert.ToDecimal(r["Longitude"]) : 0m,
            Latitude = r["Latitude"] != DBNull.Value ? Convert.ToDecimal(r["Latitude"]) : 0m,
            Timestamp = r["Timestamp"] != DBNull.Value ? Convert.ToDateTime(r["Timestamp"]) : DateTime.MinValue
        };
    }

    // Converts a DataTable to a List of LocationHistory
    public static List<LocationHistory> ToLocationHistoryList(DataTable table)
    {
        return table.AsEnumerable().Select(ToLocationHistory).ToList();
    }

    // Converts a DataTable to a DetailedUser
    public static DetailedUser ToDetailedUser(DataTable table)
    {
        var detailedUser = new DetailedUser
        {
            User = null,
            Subscriptions = new List<Subscription>(),
            Sensors = new List<Sensor>(),
            Readings = new List<Reading>(),
            Payments = new List<Payment>(),
            LocationHistories = new List<LocationHistory>()
        };

        foreach (DataRow row in table.Rows)
        {
            if (detailedUser.User == null)
            {
                detailedUser.User = ToUser(row);
            }

            if (row["SubscriptionId"] != DBNull.Value)
            {
                var subscription = ToSubscription(row);
                if (!detailedUser.Subscriptions.Any(s => s.Folio == subscription.Folio && s.StartDate == subscription.StartDate))
                {
                    detailedUser.Subscriptions.Add(subscription);
                }
            }

            if (row["SensorId"] != DBNull.Value)
            {
                var sensor = ToSensor(row);
                if (!detailedUser.Sensors.Any(s => s.SensorId == sensor.SensorId))
                {
                    detailedUser.Sensors.Add(sensor);
                }
            }

            if (row["ReadingId"] != DBNull.Value)
            {
                var reading = ToReading(row);
                if (!detailedUser.Readings.Any(r => r.ReadingId == reading.ReadingId))
                {
                    detailedUser.Readings.Add(reading);
                }
            }

            if (row["PaymentId"] != DBNull.Value)
            {
                var payment = ToPayment(row);
                if (!detailedUser.Payments.Any(p => p.PaymentFolio == payment.PaymentFolio))
                {
                    detailedUser.Payments.Add(payment);
                }
            }

            if (row["LocationHistoryId"] != DBNull.Value)
            {
                var locationHistory = ToLocationHistory(row);
                if (!detailedUser.LocationHistories.Any(lh => lh.LocationHistoryId == locationHistory.LocationHistoryId))
                {
                    detailedUser.LocationHistories.Add(locationHistory);
                }
            }
        }

        return detailedUser;
    }

    // Converts a DataTable to a List of SensorDetail
    public static List<SensorDetail> ToSensorDetailList(DataTable table)
    {
        var sensorDict = new Dictionary<int, SensorDetail>();

        foreach (DataRow row in table.Rows)
        {
            int sensorId = row["SensorId"] != DBNull.Value ? Convert.ToInt32(row["SensorId"]) : 0;

            if (!sensorDict.ContainsKey(sensorId))
            {
                sensorDict[sensorId] = new SensorDetail
                {
                    SensorId = sensorId,
                    SerialNumber = row["SerialNumber"] != DBNull.Value ? row["SerialNumber"].ToString() : string.Empty,
                    Readings = new List<Reading>()
                };
            }

            var reading = ToReading(row);
            sensorDict[sensorId].Readings.Add(reading);
        }

        return sensorDict.Values.ToList();
    }

    // Converts a DataRow to a SubscriptionDetail
    public static SubscriptionDetail ToSubscriptionDetail(DataRow row)
    {
        return new SubscriptionDetail
        {
            FirstName = row.Field<string>("FirstName"),
            LastName = row.Field<string>("LastName"),
            MiddleName = row.Field<string>("MiddleName"),
            Email = row.Field<string>("Email"),
            Folio = row.Field<string>("Folio"),
            StartDate = row.Field<DateTime>("StartDate"),
            EndDate = row.Field<DateTime>("EndDate")
        };
    }
    
    public static UserSU ToUserSU(DataRow row)
    {
        return new UserSU
        {
            UserId = row.Field<int>("UserId"),
            FirstName = row.Field<string>("FirstName"),
            LastName = row.Field<string>("LastName"),
            MiddleName = row.Field<string>("MiddleName"),
            Email = row.Field<string>("Email"),
            Password = row.Field<string>("Password"),
            type = "admin"

        };
    }

    // Converts a DataTable to a List of SubscriptionDetail
    public static List<SubscriptionDetail> ToSubscriptionDetailList(DataTable table)
    {
        return table.AsEnumerable().Select(ToSubscriptionDetail).ToList();
    }
    
    
}
