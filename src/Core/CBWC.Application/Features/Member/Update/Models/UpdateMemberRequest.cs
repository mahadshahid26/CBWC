using CBWC.Application.Common.ResultWrapper;
using FluentValidation;

using MediatR;

namespace CBWC.Application.Features.Member.Update.Models;

public class UpdateMemberRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? CNIC { get; set; }
}

public class UpdateMemberCommand : IRequest<ApiResult<UpdateMemberResponse>>
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? CNIC { get; set; }
}

public class UpdateMemberCommandValidator : AbstractValidator<UpdateMemberCommand>
{
    public UpdateMemberCommandValidator()
    {
        RuleFor(v => v.CNIC)
            .NotEmpty()
            .NotNull()
            .MaximumLength(20);

        RuleFor(v => v.FirstName)
            .MaximumLength(100);

        RuleFor(v => v.LastName)
            .MaximumLength(100);

        RuleFor(v => v.MembershipNumber)
            .MaximumLength(50);
    }
}
