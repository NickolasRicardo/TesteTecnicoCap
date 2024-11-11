using FluentValidation;
using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests;

public record BuscarSaldoQueryParams : IRequest<BuscarSaldoQueryResponse>
{
    public long NumeroContaCorrente { get; init; }


    public void Validate()
    {
        var validator = new InlineValidator<BuscarSaldoQueryParams>();
        
        validator.RuleFor(x=> x.NumeroContaCorrente).GreaterThan(0).WithMessage("INVALID_VALUE");
    }
}