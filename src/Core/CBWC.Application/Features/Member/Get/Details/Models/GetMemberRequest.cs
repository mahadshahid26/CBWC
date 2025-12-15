using CBWC.Application.Common.ResultWrapper;
using FluentValidation;

using MediatR;

namespace CBWC.Application.Features.Member.Get.Details.Models;

public class GetMemberRequest : IRequest<ApiResult<GetMemberResponse>>
{
    public int Id { get; set; }
}

public class GetMemberRequestValidator : AbstractValidator<GetMemberRequest>
{
    public GetMemberRequestValidator()
    {
        //RuleFor(v => v.Id)
        //    .NotEmpty;
    }
}
