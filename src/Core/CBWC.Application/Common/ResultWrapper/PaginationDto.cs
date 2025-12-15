namespace CBWC.Application.Common.ResultWrapper;
public readonly record struct PaginationDto
{
    public required int TotalRecords { get; init; }
    public required int CurrentPage { get; init; }
    public required int PageSize { get; init; }
    public int TotalPages => (int)Math.Ceiling((double)TotalRecords / PageSize);
}
