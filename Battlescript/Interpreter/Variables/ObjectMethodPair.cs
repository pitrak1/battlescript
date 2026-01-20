namespace Battlescript;

public class ObjectMethodPair(ObjectVariable obj, FunctionVariable method) : FunctionVariable(method), IEquatable<ObjectMethodPair>
{
    public ObjectVariable Object { get; set; } = obj;

    public override Variable RunFunction(CallStack callStack, Closure closure, ArgumentSet arguments, Instruction? inst = null)
    {
        arguments.Positionals.Insert(0, Object);
        return base.RunFunction(callStack, closure, arguments, inst);
    }

    #region Equality

    public override bool Equals(object? obj) => obj is ObjectMethodPair other && Equals(other);

    public bool Equals(ObjectMethodPair? other) =>
        other is not null &&
        Equals(Object, other.Object) &&
        base.Equals(other);

    public override bool Equals(Variable? other) => other is ObjectMethodPair variable && Equals(variable);

    public override int GetHashCode() => HashCode.Combine(Object, base.GetHashCode());

    public static bool operator ==(ObjectMethodPair? left, ObjectMethodPair? right) =>
        left?.Equals(right) ?? right is null;

    public static bool operator !=(ObjectMethodPair? left, ObjectMethodPair? right) => !(left == right);

    #endregion
}
