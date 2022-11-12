namespace BattleScript.Tests; 

public class E2ETests {
    private Runner? _runner;
    private CustomCallbacks? _callbacks;
    
    [SetUp]
    public void SetUp() {
        _callbacks = new CustomCallbacks();
        _runner = new Runner("/Users/nickpitrak/Desktop/BattleScript/TestFiles/", _callbacks);
    }

    [Test]
    public void RunsCustomCallbacks() {
        var scopeStack = _runner.RunString("var x = Btl.test();");

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 5));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }


    [Test]
    public void RunsCustomCallbacksWithArgs() {
        var scopeStack = _runner.RunString("var x = Btl.test2('asdf');");

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, "asdfasdf"));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
}