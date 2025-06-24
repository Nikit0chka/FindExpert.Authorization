using System.Net.Mail;
using Ardalis.GuardClauses;

namespace Domain.Extensions;

internal static class GuardAgainstExtensions
{
    internal static void StringLengthOutOfRange(string input,
                                                int minLength,
                                                int maxLength,
                                                string parameterName,
                                                string? message)
    {
        Guard.Against.Negative(maxLength - minLength,
                               "min or max length",
                               "Min length must be equal or less than max length.");

        Guard.Against.StringTooShort(input, minLength, parameterName, message);
        Guard.Against.StringTooLong(input, maxLength, parameterName, message);
    }

    public static void InvalidEmail(string input, string parameterName, string message)
    {
        Guard.Against.NullOrWhiteSpace(input, parameterName);

        try
        {
            _ = new MailAddress(input);
        }
        catch (FormatException)
        {
            throw new ArgumentException(message, parameterName);
        }
    }
}