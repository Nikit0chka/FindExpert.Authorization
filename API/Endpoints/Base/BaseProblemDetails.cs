using API.Contracts;

namespace API.Endpoints.Base;

public sealed class BaseProblemDetails(
    string? errorCode,
    IErrorMapper mapper,
    HttpContext context)
{
    public string Type { get; private set; } = $"https://api.yourdomain.com/errors/{errorCode}";
    public string Title { get; private set; } = mapper.GetTitle(errorCode);
    public int Status { get; private set; } = mapper.GetStatusCode(errorCode);
    public string Detail { get; private set; } = mapper.GetDetail(errorCode);
    public string Instance { get; private set; } = context.Request.Path;
    public string? ErrorCode { get; private set; } = errorCode;
    public Dictionary<string, object> Extensions { get; set; } = new();
}