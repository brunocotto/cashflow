using CashFlow.Communication.Enums;

namespace CashFlow.Communication.Requests;

public class RequestExpanseJson
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public decimal Amount { get; set; }
    public Enums.PaymentType PaymentType { get; set; }
    public IList<Tag> Tags { get; set; } = [];
}
