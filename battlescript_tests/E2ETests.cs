namespace BattleScript.Tests;

using BattleScript.Core;

public class E2ETests
{
    private Runner? _runner;

    [SetUp]
    public void SetUp()
    {
        _runner = new Runner("/Users/nickpitrak/Desktop/battlescript/battlescript_tests/TestFiles/");
    }

    [Test]
    public void RunsCustomCallbacks()
    {
        var scopeStack = _runner.RunString("var x = Btl.test();");

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 5));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }


    [Test]
    public void RunsCustomCallbacksWithArgs()
    {
        var scopeStack = _runner.RunString("var x = Btl.test2('asdf');");

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, "asdfasdf"));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }

    [Test]
    public void RunsCustomCallbacksWithNoReturnValue()
    {
        var scopeStack = _runner.RunString("var x = Btl.test3();");

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, null));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }

    [Test]
    public void AllowsContextToBePassedBetweenRuns()
    {
        _runner.RunString("Btl.context.x = 5;");
        var scopeStack = _runner.RunString("var x = Btl.context.x;");

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, null));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
}