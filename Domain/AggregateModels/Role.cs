using Ardalis.SmartEnum;

namespace Domain.AggregateModels;

public sealed class Role(string name, int value):SmartEnum<Role>(name, value)
{
    public readonly static Role Customer = new(nameof(Customer), 1);
    public readonly static Role Master = new(nameof(Master), 2);
}