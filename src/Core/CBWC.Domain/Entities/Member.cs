using CBWC.Domain.Enums;

namespace CBWC.Domain.Entities;

public class Member
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? MembershipNumber { get; set; }
    public string? CNIC { get; set; }
    public DateTime Created { get; set; }
}