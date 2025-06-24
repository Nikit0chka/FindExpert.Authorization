namespace API.Contracts;

public interface IErrorMapper
{
    int GetStatusCode(string? error);
    string GetTitle(string? error);
    string GetDetail(string? error);
}