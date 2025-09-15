using LastLink.Anticipation.Domain.Enums;

namespace LastLink.Anticipation.Domain.Entities;

public class AnticipationRequest
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    public Guid CreatorId { get; private set; }
    public decimal RequestedAmount { get; private set; }
    public DateTime RequestedAt { get; private set; }
    public decimal NetAmount { get; private set; }
    public AnticipationStatus Status { get; private set; } = AnticipationStatus.Pending;

    public const decimal FeeRate = 0.05m;

    private AnticipationRequest() { }

    public AnticipationRequest(Guid creatorId, decimal requestedAmount, DateTime requestedAt)
    {
        if (requestedAmount <= 100m) throw new ArgumentException("Min R$100,00", nameof(requestedAmount));
        CreatorId = creatorId;
        RequestedAmount = decimal.Round(requestedAmount, 2, MidpointRounding.ToEven);
        RequestedAt = requestedAt;
        NetAmount = CalculateNet(RequestedAmount);
        Status = AnticipationStatus.Pending;
    }

    public void Approve()
    {
        if (Status != AnticipationStatus.Pending) throw new InvalidOperationException("Solicitação não está pendente.");
        Status = AnticipationStatus.Approved;
    }

    public void Reject()
    {
        if (Status != AnticipationStatus.Pending) throw new InvalidOperationException("Solicitação não está pendente.");
        Status = AnticipationStatus.Rejected;
    }

    public static decimal CalculateNet(decimal gross) => decimal.Round(gross * (1 - FeeRate), 2, MidpointRounding.ToEven);
}
