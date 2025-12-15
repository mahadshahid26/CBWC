using CBWC.Application.Common.ResultWrapper;
using CBWC.Application.Features.Member.Update.Models;
using CBWC.Application.Interfaces.Repositories;
using CBWC.Domain.Enums;
using MediatR;

namespace CBWC.Application.Features.Member.Update;

public class UpdateMemberHandler(
    IMemberRepo memberRepo)
        : IRequestHandler<UpdateMemberCommand, ApiResult<UpdateMemberResponse>>
{
    private readonly IMemberRepo _memberRepo = memberRepo;

    public async Task<ApiResult<UpdateMemberResponse>> Handle(
        UpdateMemberCommand request,
        CancellationToken cancellationToken)
    {
        var member = MemberMapper.Map(request);

        var result = new ApiResult<UpdateMemberResponse>();

        var existingMember = await _memberRepo.GetAsync(member.Id);
        if (existingMember == null)
        {
            result.ResultCode = ResultCodes.NotFound;
            result.Message = "Member not found";
            return result;
        }

        var isUpdated = await _memberRepo.UpdateAsync(member.Id, member);
        if (!isUpdated)
        {
            result.ResultCode = ResultCodes.NotFound;
            result.Message = "Member could not be updated";
            return result;
        }

        result.Payload = new UpdateMemberResponse
        {
            Message = "Member updated successfully"
        };
        result.ResultCode = ResultCodes.Success;

        return result;
    }
}
