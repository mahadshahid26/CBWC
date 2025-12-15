using CBWC.Application.Common.ResultWrapper;
using CBWC.Application.Features.Member.Get.Details.Models;
using CBWC.Application.Interfaces.Repositories;
using CBWC.Domain.Enums;
using MediatR;

namespace CBWC.Application.Features.Member.Get.Details;

public class GetMemberHandler(
    IMemberRepo memberRepo)
        : IRequestHandler<GetMemberRequest, ApiResult<GetMemberResponse>>
{
    private readonly IMemberRepo _memberRepo = memberRepo;

    public async Task<ApiResult<GetMemberResponse>> Handle(
        GetMemberRequest request,
        CancellationToken cancellationToken)
    {
        var result = new ApiResult<GetMemberResponse>();

        var member = await _memberRepo.GetAsync(request.Id);
        if (member == null)
        {
            result.ResultCode = ResultCodes.NotFound;
            return result;
        }

        result.ResultCode = ResultCodes.Success;

        result.Payload = new GetMemberResponse
        {
            Member = MemberMapper.Map(member),
        };

        return result;
    }
}
