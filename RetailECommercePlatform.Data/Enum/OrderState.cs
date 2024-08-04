using System.ComponentModel;

namespace RetailECommercePlatform.Data.Enum;

public enum OrderState
{
    [Description("Order received")]
    OrderReceived = 1,
    [Description("Order approved")]
    Approved = 2,
    [Description("Shipping")]
    Shipping = 3,
    [Description("Completed")]
    Completed = 4,
    [Description("Cancelled")]
    Cancelled = 5
}