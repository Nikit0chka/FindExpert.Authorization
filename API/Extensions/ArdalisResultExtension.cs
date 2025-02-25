using Ardalis.Result;
using FastEndpoints;
using FluentValidation.Results;
using IResult = Ardalis.Result.IResult;

namespace API.Extensions;

internal static class ResultsExtensions
{
    internal static Task SendResponse<TResult, TResponse>(
        this IEndpoint ep,
        TResult result,
        Func<TResult, TResponse> mapper) where TResult : IResult
    {
        switch (result.Status)
        {
            case ResultStatus.Ok:
                return ep.HttpContext.Response.SendAsync(mapper(result));

            case ResultStatus.Created:
                return ep.HttpContext.Response.SendCreatedAsync(mapper(result));

            case ResultStatus.NoContent:
                return ep.HttpContext.Response.SendNoContentAsync();

            case ResultStatus.NotFound:
                return ep.HttpContext.Response.SendNotFoundAsync(result.Errors.FirstOrDefault() ?? "Resource not found.");

            case ResultStatus.Invalid:
                foreach (var error in result.ValidationErrors)
                {
                    ep.ValidationFailures.Add(new(error.Identifier, error.ErrorMessage));
                }

                return ep.HttpContext.Response.SendErrorsAsync(ep.ValidationFailures);

            case ResultStatus.Forbidden:
                return ep.HttpContext.Response.SendForbiddenAsync();

            case ResultStatus.Unauthorized:
                return ep.HttpContext.Response.SendUnauthorizedAsync();

            case ResultStatus.Conflict:
                return ep.HttpContext.Response.SendConflictAsync();

            case ResultStatus.CriticalError:
                return ep.HttpContext.Response.SendCriticalErrorAsync();

            case ResultStatus.Unavailable:
                return ep.HttpContext.Response.SendUnavailableAsync();

            case ResultStatus.Error:
                return ep.HttpContext.Response.SendErrorAsync(result.Errors.FirstOrDefault() ?? "An error occurred while processing the request.");
            default:
                return ep.HttpContext.Response.SendErrorAsync("An unexpected result status was encountered.");
        }
    }

    private static Task SendCreatedAsync<TResponse>(this HttpResponse response, TResponse data)
    {
        response.StatusCode = StatusCodes.Status201Created;
        return response.WriteAsJsonAsync(data);
    }

    private static Task SendNoContentAsync(this HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status204NoContent;
        return response.WriteAsync(string.Empty);
    }

    private static Task SendNotFoundAsync(this HttpResponse response, string error)
    {
        response.StatusCode = StatusCodes.Status404NotFound;
        return response.WriteAsync(error);
    }

    private static Task SendForbiddenAsync(this HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status403Forbidden;
        return response.WriteAsync("Forbidden.");
    }

    private static Task SendUnauthorizedAsync(this HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status401Unauthorized;
        return response.WriteAsync("Unauthorized.");
    }

    private static Task SendConflictAsync(this HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status409Conflict;
        return response.WriteAsync("Conflict occurred.");
    }

    private static Task SendCriticalErrorAsync(this HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status500InternalServerError;
        return response.WriteAsync("A critical error occurred.");
    }

    private static Task SendUnavailableAsync(this HttpResponse response)
    {
        response.StatusCode = StatusCodes.Status503ServiceUnavailable;
        return response.WriteAsync("Service is unavailable.");
    }

    private static Task SendErrorAsync(this HttpResponse response, string errorMessage)
    {
        response.StatusCode = StatusCodes.Status500InternalServerError;
        return response.WriteAsync(errorMessage);
    }

    private static Task SendErrorsAsync(this HttpResponse response, IEnumerable<ValidationFailure> validationFailures)
    {
        response.StatusCode = StatusCodes.Status400BadRequest;

        var errors = validationFailures
            .Select(static validationFailure => new { Property = validationFailure.PropertyName, Message = validationFailure.ErrorMessage })
            .ToList();

        return response.WriteAsJsonAsync(errors);
    }
}