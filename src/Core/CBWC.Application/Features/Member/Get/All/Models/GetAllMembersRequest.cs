using CBWC.Application.Common.ResultWrapper;
using FluentValidation;

using MediatR;

namespace CBWC.Application.Features.Member.Get.All.Models;

public class GetAllMembersQuery : IRequest<ApiResult<GetAllMemberResponse>>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? CNIC { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
public class GetAllMembersRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? CNIC { get; set; }
    public int? PageNumber { get; set; }
    public int? PageSize { get; set; }
}

public class GetAllMembersQueryValidator : AbstractValidator<GetAllMembersQuery>
{
    public GetAllMembersQueryValidator()
    {

        RuleFor(v => v.FirstName)
            .MaximumLength(100);

        RuleFor(v => v.LastName)
            .MaximumLength(100);

        RuleFor(v => v.MembershipNumber)
            .MaximumLength(50);

        RuleFor(v => v.CNIC)
            .MaximumLength(20);

        RuleFor(v => v.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be a positive integer.");

        RuleFor(v => v.PageSize)
            .GreaterThan(0)
            .WithMessage("Page size must be a positive integer.");
    }
}
