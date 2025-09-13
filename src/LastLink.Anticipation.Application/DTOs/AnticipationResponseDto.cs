using LastLink.Anticipation.Domain.Enums;
namespace LastLink.Anticipation.Application.DTOs;
public record AnticipationResponseDto(Guid Id, Guid CreatorId, decimal ValorSolicitado, decimal ValorLiquido, DateTime DataSolicitacao, AnticipationStatus Status);
