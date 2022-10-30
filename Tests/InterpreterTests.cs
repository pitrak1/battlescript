namespace BattleScript.Tests; 

public class InterpreterTests {
    [Test]
    public void Variables() {
        string contents = LoadFile("variables.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        var scopeStack = Interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 15));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, "1234"));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, "2345"));
        expected.Add("a", new ScopeVariable(Consts.VariableTypes.Value, true));
        expected.Add("b", new ScopeVariable(Consts.VariableTypes.Value, true));
        
        Assertions.AssertScope(scopeStack.CurrentScope().Value, expected);
    }
    
    private string LoadFile(string filename) {
        return File.ReadAllText($"/Users/nickpitrak/Desktop/BattleScript/TestFiles/{filename}");
    }
}