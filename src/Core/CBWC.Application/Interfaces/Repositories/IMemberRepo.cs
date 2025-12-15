using CBWC.Domain.Entities;

namespace CBWC.Application.Interfaces.Repositories;

public interface IMemberRepo
{
    Task<Member?> GetAsync(int memberId);
    Task<(IEnumerable<Member> members, int totalCount)> GetAllAsync(
       Member memberFilter,
       int pageNumber,
       int pageSize);
    Task<int> CreateAsync(Member member);
    Task<bool> UpdateAsync(int memberId, Member member);
    Task<bool> DeleteAsync(int memberId);
    Task<bool> ExistsByCnicAsync(string cnic);
    Task<bool> ExistsAsync(int memberId);
}
