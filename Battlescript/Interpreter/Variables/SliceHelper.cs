namespace Battlescript;

public static class SliceHelper
{
    public static (int start, int stop, int step) GetSliceArgs(List<Variable?> argVariable, int length)
    {
        int step = 1;
        int start, stop;

        // Get step first (determines default start/stop)
        if (argVariable.Count >= 3 && argVariable[2] is not null and not NoneVariable)
        {
            step = BtlTypes.GetIntValue(argVariable[2]!);
        }

        if (step > 0)
        {
            start = 0;
            stop = length;
        }
        else
        {
            start = length - 1;
            stop = -1;
        }

        // Override start if provided
        if (argVariable.Count >= 1 && argVariable[0] is not null and not NoneVariable)
        {
            start = BtlTypes.GetIntValue(argVariable[0]!);
            if (start < 0) start = Math.Max(0, length + start);
            if (step > 0) start = Math.Min(start, length);
        }

        // Override stop if provided
        if (argVariable.Count >= 2 && argVariable[1] is not null and not NoneVariable)
        {
            stop = BtlTypes.GetIntValue(argVariable[1]!);
            if (stop < 0) stop = Math.Max(step > 0 ? 0 : -1, length + stop);
            if (step > 0) stop = Math.Min(stop, length);
        }

        return (start, stop, step);
    }

    public static List<int> GetSliceIndices(List<Variable?> argVariable, int length)
    {
        var (start, stop, step) = GetSliceArgs(argVariable, length);

        if (step == 0)
        {
            throw new InternalRaiseException(BtlTypes.Types.ValueError, "slice step cannot be zero");
        }

        List<int> indices = [];
        if (step > 0)
        {
            for (var i = start; i < stop; i += step)
            {
                indices.Add(i);
            }
        }
        else
        {
            for (var i = start; i > stop; i += step)
            {
                indices.Add(i);
            }
        }

        return indices;
    }
}
