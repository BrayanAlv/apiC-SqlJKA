//SubscriptionListResponse.cs
using System;
using System.Collections.Generic;

public class SubscriptionListResponse : JSONResponse
{
    public List<Subscription> Subscriptions { get; set; }

    public static SubscriptionListResponse Get(List<Subscription> subscriptions)
    {
        return new SubscriptionListResponse
        {
            Status = 0,
            Subscriptions = subscriptions
        };
    }
}