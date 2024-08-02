public class DetailedUser
{
    public User User { get; set; }
    public List<Subscription> Subscriptions { get; set; } = new List<Subscription>();
    public List<Sensor> Sensors { get; set; } = new List<Sensor>();
    public List<Reading> Readings { get; set; } = new List<Reading>();
    public List<Payment> Payments { get; set; } = new List<Payment>();
    public List<LocationHistory> LocationHistories { get; set; } = new List<LocationHistory>();
}

