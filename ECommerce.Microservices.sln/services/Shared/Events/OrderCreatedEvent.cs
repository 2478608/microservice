 
namespace Shared.Events
{
    public record OrderCreatedEvent(
        int ProductId,
        int Quantity,
        decimal TotalAmount
    );
}
    