using CBWC.Application.Common.ResultWrapper;

namespace CBWC.Application.Features.Member.Get.All.Models;

public class GetAllMemberResponse
{
    public IEnumerable<Member> Members { get; set; } = [];
    public PaginationDto Pagination { get; set; }
}


