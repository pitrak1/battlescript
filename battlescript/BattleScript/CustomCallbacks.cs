using System.Diagnostics;

namespace BattleScript.Core;

public class CustomCallbacks
{
    /*
     * This is where you need to add custom callbacks for your Unity project.  These functions will be called by using
     * the Btl object (ex: Btl.print).
     */

    public Dictionary<string, Delegate> Callbacks = new();

    public CustomCallbacks()
    {
        Callbacks["test"] = new Func<int>(TestFunction);
        Callbacks["test2"] = new Func<string, string>(TestFunction2);
        Callbacks["test3"] = new Action(TestFunction3);
    }

    public object? Run(string key, List<dynamic> args)
    {
        return Callbacks[key].DynamicInvoke(args.ToArray());
    }

    public int TestFunction()
    {
        return 5;
    }

    public string TestFunction2(string stringToReturn)
    {
        return $"{stringToReturn}asdf";
    }

    public void TestFunction3()
    {
        Debug.Print("some message");
    }
}