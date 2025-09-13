namespace LastLink.Anticipation.Application.DTOs;
public record CreateAnticipationDto(Guid CreatorId, decimal ValorSolicitado, DateTime DataSolicitacao);
