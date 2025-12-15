using CBWC.Application.Common.ResultWrapper;
using FluentValidation;

using MediatR;

namespace CBWC.Application.Features.Member.Create.Models;

public class CreateMemberRequest : IRequest<ApiResult<CreateMemberResponse>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? CNIC { get; set; }
}

public class CreateMemberRequestValidator : AbstractValidator<CreateMemberRequest>
{
    public CreateMemberRequestValidator()
    {
        RuleFor(v => v.FirstName)
            .MaximumLength(100);

        RuleFor(v => v.LastName)
            .MaximumLength(100);

        RuleFor(v => v.CNIC)
            .MaximumLength(20);

        RuleFor(v => v.MembershipNumber)
            .MaximumLength(50);

    }
}
