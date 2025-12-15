using CBWC.Application.Common.ResultWrapper;
using FluentValidation;

using MediatR;

namespace CBWC.Application.Features.Member.Delete.Models;

public class DeleteMemberRequest : IRequest<ApiResult<DeleteMemberResponse>>
{
    public int Id { get; set; }
}

public class DeleteMemberRequestValidator : AbstractValidator<DeleteMemberRequest>
{
    public DeleteMemberRequestValidator()
    {
       
    }
}
