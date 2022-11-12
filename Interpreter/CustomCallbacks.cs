using System.Diagnostics;

namespace BattleScript; 

public class CustomCallbacks {
    /*
     * This is where you need to add custom callbacks for your Unity project.  These functions will be called by using
     * the Btl object (ex: Btl.print).
     */
    
    public Dictionary<string, Delegate> Callbacks = new ();

    public CustomCallbacks() {
        Callbacks["print"] = new Action<string>(PrintFunction);
        Callbacks["test"] = new Func<int>(TestFunction);
    }

    public object? Run(string key, List<dynamic> args) {
        return Callbacks[key].DynamicInvoke(args.ToArray());
    }
    
    public void PrintFunction(string stringToPrint) {
        Debug.Print(stringToPrint);
    }

    public int TestFunction() {
        return 5;
    }
}