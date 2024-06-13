namespace BattleScript.Core;

public class CustomCallbacks
{
    /*
     * This is where you need to add custom callbacks for your Godot/Unity project.  These functions will be called by using
     * the Btl object (ex: Btl.print).
     */

    public Dictionary<string, Delegate> Callbacks = new();

    public CustomCallbacks()
    {
        Callbacks["test"] = new Func<double, Variable>(TestFunction);
    }

    public Variable? Run(string key, List<dynamic> args)
    {
        return Callbacks[key].DynamicInvoke(args.ToArray()) as Variable;
    }

    public Variable TestFunction(double x)
    {
        return new Variable(Consts.VariableTypes.Number, x + 5);
    }
}