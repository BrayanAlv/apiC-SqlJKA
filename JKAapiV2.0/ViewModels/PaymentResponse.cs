//PaymentResponse.cs
using System;
using System.Collections.Generic;
public class PaymentResponse : JSONResponse
{
    public Payment Payment { get; set; }

    public static PaymentResponse Get(Payment payment)
    {
        return new PaymentResponse
        {
            Status = 0,
            Payment = payment
        };
    }
}