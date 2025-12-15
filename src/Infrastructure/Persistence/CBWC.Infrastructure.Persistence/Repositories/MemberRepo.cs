using CBWC.Application.Interfaces.Repositories;
using CBWC.Domain.Enums;
using CBWC.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace CBWC.Infrastructure.Persistence.Repositories;

public sealed class MemberRepo(
    ICoreDbContextFactory contextFactory) : IMemberRepo
{
    public async Task<Domain.Entities.Member?> GetAsync(int memberId)
    {
        await using var context = await contextFactory.CreateWithTrackingAsync();

        var entity = await context.Members
            .FirstOrDefaultAsync(x =>
                x.Id == memberId &&
                x.RecordStatus == (int)RecordStatus.Active);

        return entity == null
            ? null
            : MemberMapper.MapToDomain(entity);
    }

    public async Task<Domain.Entities.Member?> GetByCnicAsync(string cnic)
    {
        await using var context = await contextFactory.CreateWithTrackingAsync();

        var entity = await context.Members
            .FirstOrDefaultAsync(x =>
                x.CNIC != null && x.CNIC.Equals(cnic) &&
                x.RecordStatus == (int)RecordStatus.Active);

        return entity == null
            ? null
            : MemberMapper.MapToDomain(entity);
    }

    public async Task<(IEnumerable<Domain.Entities.Member> members, int totalCount)> GetAllAsync(
        Domain.Entities.Member memberFilter,
        int pageNumber,
        int pageSize)
    {
        await using var context = await contextFactory.CreateAsync();

        var query = context.Members
            .Where(x => x.RecordStatus == (int)RecordStatus.Active)
            .AsQueryable();

        // SQL Server string filters
        if (!string.IsNullOrEmpty(memberFilter.FirstName))
            query = query.Where(x => x.FirstName != null && x.FirstName.Contains(memberFilter.FirstName));

        if (!string.IsNullOrEmpty(memberFilter.LastName))
            query = query.Where(x => x.LastName != null && x.LastName.Contains(memberFilter.LastName));

        if (!string.IsNullOrEmpty(memberFilter.MembershipNumber))
            query = query.Where(x => x.MembershipNumber != null && x.MembershipNumber.Contains(memberFilter.MembershipNumber));

        if (!string.IsNullOrEmpty(memberFilter.CNIC))
            query = query.Where(x => x.CNIC != null && x.CNIC.Contains(memberFilter.CNIC));

        if (memberFilter.Created != default)
            query = query.Where(x => x.Created.Date == memberFilter.Created.Date);

        var totalCount = await query.CountAsync();

        var memberIds = await query
            .OrderBy(x => x.Id) // stable pagination
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(x => x.Id)
            .ToListAsync();

        var members = await context.Members
            .Where(x => memberIds.Contains(x.Id))
            .ToListAsync();

        return (members.Select(MemberMapper.MapToDomain), totalCount);
    }

    public async Task<int> CreateAsync(Domain.Entities.Member member)
    {
        await using var context = await contextFactory.CreateWithTrackingAsync();

        var entity = MemberMapper.MapToEntity(member);
        entity.Created = DateTime.UtcNow;

        context.Members.Add(entity);
        await context.SaveChangesAsync();

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int memberId, Domain.Entities.Member member)
    {
        await using var context = await contextFactory.CreateWithTrackingAsync();

        var existing = await context.Members
            .FirstOrDefaultAsync(x =>
                x.Id == memberId &&
                x.RecordStatus == (int)RecordStatus.Active);

        if (existing == null)
            return false;

        var updated = MemberMapper.MapToEntity(member);
        MemberMapper.MapToEntityForUpdate(updated, existing);

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int memberId)
    {
        await using var context = await contextFactory.CreateWithTrackingAsync();

        var entity = await context.Members
            .FirstOrDefaultAsync(x =>
                x.Id == memberId &&
                x.RecordStatus == (int)RecordStatus.Active);

        if (entity == null)
            return false;

        entity.RecordStatus = (int)RecordStatus.Deleted;
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsByCnicAsync(string cnic)
    {
        await using var context = await contextFactory.CreateAsync();

        return await context.Members
            .AnyAsync(x =>
                x.CNIC == cnic &&
                x.RecordStatus == (int)RecordStatus.Active);
    }

    public async Task<bool> ExistsAsync(int memberId)
    {
        await using var context = await contextFactory.CreateAsync();

        return await context.Members
            .AnyAsync(x =>
                x.Id == memberId &&
                x.RecordStatus == (int)RecordStatus.Active);
    }
}

