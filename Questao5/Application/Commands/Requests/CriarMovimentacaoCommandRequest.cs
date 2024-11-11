using System.Windows.Input;
using FluentValidation;
using MediatR;
using Questao5.Application.Commands.Response;
using Questao5.Domain.Enumerators;
using Questao5.Domain.Extensions;
using ValidationException = System.ComponentModel.DataAnnotations.ValidationException;

namespace Questao5.Application.Commands.Requests;

public record CriarMovimentacaoCommandRequest : IRequest<CriarMovimentacaoCommandResponse>
{
    public string RequestId { get; init; }
    public long NumeroContaCorrente { get; init; }
    public double Valor { get; init; }
    public string TipoMovimento { get; init; }
    
    
    public void Validate()
    {
        var validator = new InlineValidator<CriarMovimentacaoCommandRequest>();
        validator.RuleFor(x => x.Valor)
            .GreaterThan(0).WithMessage("INVALID_VALUE");
        
        validator.RuleFor(x => x.TipoMovimento)
            .Must(value => Enum.GetValues(typeof(TipoMovimentoEnum))
                .Cast<TipoMovimentoEnum>()
                .Any(e => e.GetDescription() == value))
            .WithMessage("INVALID_TYPE");

        var result = validator.Validate(this);
        if (!result.IsValid)
        {
            var error = string.Join(", ", result.Errors.Select(e => $"{e.PropertyName}: {e.ErrorMessage}"));
            throw new ValidationException(error);
        }
    }
    
    
}