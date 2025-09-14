# LastLink Anticipation API

Serviço criado encima da proposta da LastLink. O criador solicita é aplicado a taxa de 5% é entrado na fila para aprovação ou não.

## Requisitos
- .NET 8 SDK

## Como rodar
```bash
dotnet restore
dotnet build
dotnet test
dotnet run --project src/LastLink.Anticipation.Api
# http://localhost:5000/swagger
```

## Endpoints (v1)
- `POST /api/v1/anticipations` – cria solicitação
- `GET  /api/v1/anticipations?creator_id={GUID}` – lista por creator
- `POST /api/v1/anticipations/{id}/approve` – aprova
- `POST /api/v1/anticipations/{id}/reject` – recusa
- `GET  /api/v1/anticipations/simulate?valor_solicitado=250` – simula sem criar

### Exemplos rápidos
**Criar**
```bash
curl -X POST http://localhost:5000/api/v1/anticipations   -H "Content-Type: application/json"   -d '{ "creatorId":"00000000-0000-0000-0000-000000000001", "valorSolicitado":250, "dataSolicitacao":"2025-09-09T12:00:00Z" }'
```

**Listar**
```bash
curl "http://localhost:5000/api/v1/anticipations?creator_id=00000000-0000-0000-0000-000000000001"
```

**Aprovar / Recusar**
```bash
curl -X POST http://localhost:5000/api/v1/anticipations/{id}/approve
curl -X POST http://localhost:5000/api/v1/anticipations/{id}/reject
```

**Simular**
```bash
curl "http://localhost:5000/api/v1/anticipations/simulate?valor_solicitado=250"
```

## Regras
- valor mínimo: R$ 100,00
- taxa fixa: 5%
- um creator não pode ter mais de uma pendente
- toda nova solicitação começa como “pendente”

## Notas
- DDD enxuto: domínio, casos de uso, repositório, API.
- Persistência InMemory (EF Core). Dá para trocar por banco depois.
- Versionamento: `/api/v1/...`
- Swagger ligado e `/` redireciona para `/swagger` no localhost.

## Testes
```bash
dotnet test
```
Cobre: mínimo, cálculo do líquido, pendência por creator e transições (aprovar/recusar).

## Estrutura
```
src/
  LastLink.Anticipation.Domain/
  LastLink.Anticipation.Application/
  LastLink.Anticipation.Infrastructure/
  LastLink.Anticipation.Api/
tests/
  LastLink.Anticipation.Tests/
```
