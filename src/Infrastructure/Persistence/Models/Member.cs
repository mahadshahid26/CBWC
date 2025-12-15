using CBWC.Domain.Enums;

namespace CBWC.Infrastructure.Persistence.Models;

public class Member
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? CNIC { get; set; }
    public required int RecordStatus { get; set; }
    public DateTime Created { get; set; }
}

public static class MemberMapper
{
    public static Domain.Entities.Member MapToDomain(Member entity)
    {
        return new()
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            CNIC = entity.CNIC,
            MembershipNumber = entity.MembershipNumber,
            Created = entity.Created
        };
    }

    public static Member MapToEntity(Domain.Entities.Member from) =>
        new()
        {
            Id = from.Id,
            FirstName = from.FirstName,
            LastName = from.LastName,
            MembershipNumber = from.MembershipNumber,
            CNIC = from.CNIC,
            RecordStatus = (int)RecordStatus.Active
        };

    internal static void MapToEntityForUpdate(Member from, Member to)
    {
        to.FirstName = from.FirstName;
        to.LastName = from.LastName;
        to.MembershipNumber = from.MembershipNumber;
        to.CNIC = from.CNIC;
    }
}