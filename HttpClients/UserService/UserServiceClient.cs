using System.Net.Http.Json;
using System.Text.Json;
using Domain.Utils;
using HttpClients.Base;
using HttpClients.Contracts;
using HttpClients.UserService.Get;
using HttpClients.UserService.Validate;

namespace HttpClients.UserService;

internal sealed class UserServiceClient(HttpClient httpClient):IUserServiceClient
{
    private readonly JsonSerializerOptions _jsonOptions = new()
                                                          {
                                                              PropertyNameCaseInsensitive = true,
                                                              PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                                                          };

    public Task<OperationResult<UserServiceValidateResponse>> ValidateUserAsync(UserServiceValidateRequest request, CancellationToken cancellationToken) => SendRequest<UserServiceValidateRequest, UserServiceValidateResponse>(
         "/api/user/validate",
         HttpMethod.Post,
         request,
         cancellationToken);

    public Task<OperationResult<UserServiceGetResponse>> GetUserAsync(UserServiceGetRequest request, CancellationToken cancellationToken) => SendRequest<UserServiceGetRequest, UserServiceGetResponse>(
         $"/api/user/{request.Id}",
         HttpMethod.Get,
         cancellationToken);

    private async Task<OperationResult<TResponse>> SendRequest<TRequest, TResponse>(
        string endpoint,
        HttpMethod method,
        TRequest request,
        CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(method, endpoint);

        httpRequest.Content = JsonContent.Create(request, options: _jsonOptions);

        using var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse<TResponse>>(
                                                                                        stream,
                                                                                        _jsonOptions,
                                                                                        cancellationToken);

        if (apiResponse is null)
            throw new("Failed to deserialize response");

        if (!response.IsSuccessStatusCode)
            return OperationResult<TResponse>.Error(apiResponse.Error?.ErrorCode ?? "UnknownError");

        if (!apiResponse.IsSuccess && apiResponse.Error!.ErrorCode is null)
            throw new("Api response is not success but error code is null");

        return OperationResult<TResponse>.Success(apiResponse.Data!);
    }

    private async Task<OperationResult<TResponse>> SendRequest<TRequest, TResponse>(
        string endpoint,
        HttpMethod method,
        CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(method, endpoint);

        using var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse<TResponse>>(
                                                                                        stream,
                                                                                        _jsonOptions,
                                                                                        cancellationToken);

        if (apiResponse is null)
            throw new("Failed to deserialize response");

        if (!response.IsSuccessStatusCode)
            return OperationResult<TResponse>.Error(apiResponse.Error?.ErrorCode ?? "UnknownError");

        if (!apiResponse.IsSuccess && apiResponse.Error!.ErrorCode is null)
            throw new("Api response is not success but error code is null");

        return OperationResult<TResponse>.Success(apiResponse.Data!);
    }

    private async Task<OperationResult> SendRequest<TRequest>(
        string endpoint,
        HttpMethod method,
        TRequest request, CancellationToken cancellationToken)
    {
        using var httpRequest = new HttpRequestMessage(method, endpoint);

        httpRequest.Content = JsonContent.Create(request, options: _jsonOptions);

        using var response = await httpClient.SendAsync(httpRequest, cancellationToken);
        await using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);

        var apiResponse = await JsonSerializer.DeserializeAsync<ApiResponse>(
                                                                             stream,
                                                                             _jsonOptions,
                                                                             cancellationToken);

        if (apiResponse is null)
            throw new("Failed to deserialize response");

        if (!response.IsSuccessStatusCode)
            return OperationResult.Error(apiResponse.Error?.ErrorCode ?? "UnknownError");

        if (!apiResponse.IsSuccess && apiResponse.Error!.ErrorCode is null)
            throw new("Api response is not success but error code is null");

        return OperationResult.Success();
    }
}