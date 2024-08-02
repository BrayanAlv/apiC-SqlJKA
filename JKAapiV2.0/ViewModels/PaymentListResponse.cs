//PaymentListResponse.cs
using System;
using System.Collections.Generic;

public class PaymentListResponse : JSONResponse
{
    public List<Payment> Payments { get; set; }

    public static PaymentListResponse Get(List<Payment> payments)
    {
        return new PaymentListResponse
        {
            Status = 0,
            Payments = payments
        };
    }
}