using CBWC.Application.Common.ResultWrapper;
using CBWC.Application.Features.Member.Delete.Models;
using CBWC.Application.Interfaces.Repositories;
using CBWC.Domain.Enums;
using MediatR;

namespace CBWC.Application.Features.Member.Delete;

internal class DeleteMemberHandler(
    IMemberRepo memberRepo)
        : IRequestHandler<DeleteMemberRequest, ApiResult<DeleteMemberResponse>>
{
    private readonly IMemberRepo _memberRepo = memberRepo;

    public async Task<ApiResult<DeleteMemberResponse>> Handle(
        DeleteMemberRequest request,
        CancellationToken cancellationToken)
    {
        var member = MemberMapper.Map(request);

        var result = new ApiResult<DeleteMemberResponse>();

        if (!await _memberRepo.ExistsAsync(member.Id))
        {
            result.ResultCode = ResultCodes.NotFound;
            result.Message = "Member not found";
            return result;
        }

        var isDeleted = await _memberRepo.DeleteAsync(member.Id);
        if (!isDeleted)
        {
            result.ResultCode = ResultCodes.NotFound;
            result.Message = "Member could not be deleted";
            return result;
        }

        result.Payload = new DeleteMemberResponse
        {
            Message = "Member deleted successfully"
        };  
        result.ResultCode = ResultCodes.Success;

        return result;
    }
}
