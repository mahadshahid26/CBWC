using CBWC.Application.Common.ResultWrapper;
using CBWC.Application.Features.Member.Create.Models;
using CBWC.Application.Interfaces.Repositories;
using CBWC.Domain.Enums;
using MediatR;

namespace CBWC.Application.Features.Member.Create;

internal sealed class CreateMemberHandler(
    IMemberRepo memberRepo)
        : IRequestHandler<CreateMemberRequest, ApiResult<CreateMemberResponse>>
{
    private readonly IMemberRepo _memberRepo = memberRepo;

    public async Task<ApiResult<CreateMemberResponse>> Handle(
        CreateMemberRequest request,
        CancellationToken cancellationToken)
    {

        var member = MemberMapper.MapToDomain(request);


        var result = new ApiResult<CreateMemberResponse>();

        if (await _memberRepo.ExistsByCnicAsync(member.CNIC!))
        {
            result.ResultCode = ResultCodes.AlreadyExists;
            result.Message = "Member already created";
            return result;
        }

        member.Created = DateTime.UtcNow;

        member.Id = await _memberRepo.CreateAsync(member);

        result.Payload = new CreateMemberResponse
        {
            Id = member.Id
        };

        result.ResultCode = ResultCodes.Success;

        return result;
    }
}
