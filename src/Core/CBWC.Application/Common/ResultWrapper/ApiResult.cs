using CBWC.Domain.Enums;

namespace CBWC.Application.Common.ResultWrapper;

public class ApiResult<T>
{
    public ResultCodes ResultCode { get; set; }
    public string? Message { get; set; }
    public T? Payload { get; set; }
}
