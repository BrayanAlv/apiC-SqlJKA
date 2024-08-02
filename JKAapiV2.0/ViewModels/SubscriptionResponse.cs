//SubscriptionResponse.cs
using System;
using System.Collections.Generic;
public class SubscriptionResponse : JSONResponse
{
    public Subscription Subscription { get; set; }

    public static SubscriptionResponse Get(Subscription subscription)
    {
        return new SubscriptionResponse
        {
            Status = 0,
            Subscription = subscription
        };
    }
}