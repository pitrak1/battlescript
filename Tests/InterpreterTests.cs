namespace BattleScript.Tests; 

public class InterpreterTests {
    [Test]
    public void Variables() {
        string contents = LoadFile("variables.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 15));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, "1234"));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, "2345"));
        expected.Add("a", new ScopeVariable(Consts.VariableTypes.Value, true));
        expected.Add("b", new ScopeVariable(Consts.VariableTypes.Value, true));
        
        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void Operators() {
        string contents = LoadFile("operators.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 11));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, 56));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, false));
        expected.Add("a", new ScopeVariable(Consts.VariableTypes.Value, true));
        expected.Add("b", new ScopeVariable(Consts.VariableTypes.Value, false));
        
        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void Arrays() {
        string contents = LoadFile("arrays.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(
            Consts.VariableTypes.Array, 
            new List<ScopeVariable>() {
                new (Consts.VariableTypes.Value, 1),
                new (Consts.VariableTypes.Value, 5),
                new (Consts.VariableTypes.Value, 6)
            }
        ));
        expected.Add("y", new ScopeVariable(
            Consts.VariableTypes.Array, 
            new List<ScopeVariable>() {
                new (Consts.VariableTypes.Value, "1234"),
                new (Consts.VariableTypes.Value, "2345")
            }
        ));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, 3));
        expected.Add("a", new ScopeVariable(Consts.VariableTypes.Value, 2));
        expected.Add("b", new ScopeVariable(
            Consts.VariableTypes.Array, 
            new List<ScopeVariable>() {
                new (Consts.VariableTypes.Value, 3),
                new (Consts.VariableTypes.Value, 2)
            }
        ));
        
        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void Dictionaries() {
        string contents = LoadFile("dictionaries.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(
            Consts.VariableTypes.Dictionary, 
            new Dictionary<dynamic, ScopeVariable> {
                {
                    1,
                    new ScopeVariable(Consts.VariableTypes.Value, "asdf")
                },
                {
                    "qwer",
                    new ScopeVariable(Consts.VariableTypes.Value, 5)
                }
            }
        ));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, 5));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, 5));
        expected.Add("a", new ScopeVariable(
            Consts.VariableTypes.Dictionary, 
            new Dictionary<dynamic, ScopeVariable> {
                {
                    5,
                    new ScopeVariable(Consts.VariableTypes.Value, 9)
                }
            }
        ));
        expected.Add("b", new ScopeVariable(Consts.VariableTypes.Value, 9));
        
        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void If() {
        string contents = LoadFile("if.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 5));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, 6));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void IfElse() {
        string contents = LoadFile("ifelse.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 3));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, 3));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, 2));
        expected.Add("a", new ScopeVariable(Consts.VariableTypes.Value, 5));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void While() {
        string contents = LoadFile("while.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, 8));
        expected.Add("a", new ScopeVariable(Consts.VariableTypes.Value, 11));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void Functions() {
        string contents = LoadFile("functions.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("my_function", new ScopeVariable(
            Consts.VariableTypes.Function, 
            new List<ScopeVariable>()
        ));
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Value, 5));
        expected.Add("my_other_function", new ScopeVariable(
            Consts.VariableTypes.Function, 
            new List<ScopeVariable>() {
                new ScopeVariable(Consts.VariableTypes.Value, "my_variable")
            }
        ));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, 8));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void Classes() {
        string contents = LoadFile("classes.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);
        
        Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
        class1Scope.Add("a", new ScopeVariable(Consts.VariableTypes.Value, 10));

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class));
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Object, class1Scope));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, 5));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, 10));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    [Test]
    public void Methods() {
        string contents = LoadFile("classes.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
        
        var interpreter = new Interpreter();
        var scopeStack = interpreter.Run(instructions);
        
        Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
        class1Scope.Add("a", new ScopeVariable(Consts.VariableTypes.Value, 10));

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class));
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Object, class1Scope));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Value, 5));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Value, 10));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }
    
    private string LoadFile(string filename) {
        return File.ReadAllText($"/Users/nickpitrak/Desktop/BattleScript/TestFiles/{filename}");
    }
}