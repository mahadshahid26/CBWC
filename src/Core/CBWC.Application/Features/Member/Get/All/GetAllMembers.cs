using CBWC.Application.Common.ResultWrapper;
using CBWC.Application.Features.Member.Get.All.Models;
using CBWC.Application.Interfaces.Repositories;
using CBWC.Domain.Enums;
using MediatR;

namespace CBWC.Application.Features.Member.Get.All;

public class GetAllMembersHandler(
    IMemberRepo memberRepo)
        : IRequestHandler<GetAllMembersQuery, ApiResult<GetAllMemberResponse>>
{
    private readonly IMemberRepo _memberRepo = memberRepo;

    public async Task<ApiResult<GetAllMemberResponse>> Handle(
        GetAllMembersQuery request,
        CancellationToken cancellationToken)
    {
        var result = new ApiResult<GetAllMemberResponse>();

        var member = MemberMapper.MapToDomain(request);

        var (members, totalRecords) = await _memberRepo.GetAllAsync(
            member,
            request.PageNumber,
            request.PageSize
        );

        result.ResultCode = ResultCodes.Success;

        result.Payload = new GetAllMemberResponse
        {
            Members = [.. members.Select(MemberMapper.Map)],
            Pagination = new PaginationDto
            {
                TotalRecords = totalRecords,
                CurrentPage = request.PageNumber,
                PageSize = request.PageSize
            }
        };

        return result;
    }
}
