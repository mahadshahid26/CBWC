using CBWC.Application.Features.Member.Create.Models;
using CBWC.Application.Features.Member.Delete.Models;
using CBWC.Application.Features.Member.Get.All.Models;
using CBWC.Application.Features.Member.Update.Models;

namespace CBWC.Application.Features.Member;

internal static class MemberMapper
{
    public static Member Map(Domain.Entities.Member from) =>
        new()
        {
            Id = from.Id,
            FirstName = from.FirstName,
            LastName = from.LastName,
            CNIC = from.CNIC,
            MembershipNumber = from.MembershipNumber,
            Created = from.Created
        };

    public static Domain.Entities.Member MapToDomain(CreateMemberRequest from) =>
        new()
        {
            FirstName = from.FirstName,
            LastName = from.LastName,
            CNIC = from.CNIC,
            MembershipNumber = from.MembershipNumber
        };

    public static Domain.Entities.Member MapToDomain(GetAllMembersQuery from) =>
        new()
        {
            FirstName = from.FirstName,
            LastName = from.LastName,
            CNIC = from.CNIC,
            MembershipNumber = from.MembershipNumber
        };
    public static Domain.Entities.Member Map(UpdateMemberCommand from) =>
        new()
        {
            Id = from.Id,
            FirstName = from.FirstName,
            LastName = from.LastName,
            CNIC = from.CNIC,
            MembershipNumber = from.MembershipNumber
        };
    public static Member Map(DeleteMemberRequest from) =>
        new()
        {
           Id = from.Id
        };
}
