namespace Kafka.Models;

public readonly record struct EmailConfirmationCodeMessage(string Address, string Code);