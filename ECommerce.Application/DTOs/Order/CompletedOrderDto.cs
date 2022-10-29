namespace ECommerce.Application.DTOs.Order;

public sealed class CompletedOrderDto
{
    public string OrderCode { get; set; }
    public string UserName { get; set; }
    public DateTime OrderDate { get; set; }
    public string EmailAddress { get; set; }
}