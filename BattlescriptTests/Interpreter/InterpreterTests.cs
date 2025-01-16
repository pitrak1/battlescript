using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class InterpreterTests
{
    [TestFixture]
    public class InterpreterAssignments
    {
        [Test]
        public void HandlesBasicAssignmentsFromLiteralToVariable()
        {
            var lexer = new Lexer("x = 5");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 5) }
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesBasicAssignmentsFromVariableToVariable()
        {
            var lexer = new Lexer("x = 5\ny = x");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "y", new Variable(Consts.VariableTypes.Number, 5) }
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
    }
    
    [TestFixture]
    public class InterpreterOperations
    {
        [Test]
        public void HandlesEquality()
        {
            var lexer = new Lexer("x = 5 == 5\ny = 5 == 6");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, true) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, false) }
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesLessThan()
        {
            var lexer = new Lexer("x = 5 < 5\ny = 5 < 6");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, false) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, true) }
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesGreaterThan()
        {
            var lexer = new Lexer("x = 5 > 5\ny = 7 > 6");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, false) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, true) }
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesAddition()
        {
            var lexer = new Lexer("x = 5 + 6");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 11) }
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesMultiplication()
        {
            var lexer = new Lexer("x = 5 * 6");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 30) }
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
    }
    
    [TestFixture]
    public class InterpreterSeparators
    {
        [Test]
        public void HandlesArrayDefinition()
        {
            var lexer = new Lexer("x = [5, '5']");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new Variable(
                            Consts.VariableTypes.List, 
                            new List<Variable>
                            {
                                new (Consts.VariableTypes.Number, 5),
                                new (Consts.VariableTypes.String, "5"),
                            }
                        )
                    },
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesTupleDefinition()
        {
            var lexer = new Lexer("x = (5, '5')");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new Variable(
                            Consts.VariableTypes.Tuple, 
                            new List<Variable>
                            {
                                new (Consts.VariableTypes.Number, 5),
                                new (Consts.VariableTypes.String, "5"),
                            }
                        )
                    },
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesSetDefinition()
        {
            var lexer = new Lexer("x = {5, '5'}");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new Variable(
                            Consts.VariableTypes.Set, 
                            new List<Variable>
                            {
                                new (Consts.VariableTypes.Number, 5),
                                new (Consts.VariableTypes.String, "5"),
                            }
                        )
                    },
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
        
        [Test]
        public void HandlesDictionaryDefinition()
        {
            var lexer = new Lexer("x = {'asdf': 5, 'qwer': '5'}");
            var lexerResult = lexer.Run();
            var parser = new Parser(lexerResult);
            var parserResult = parser.Run();
            var interpreter = new Interpreter(parserResult);
            var interpreterResult = interpreter.Run();
    
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new Variable(
                            Consts.VariableTypes.Dictionary, 
                            new Dictionary<string, Variable>
                            {
                                {"asdf", new (Consts.VariableTypes.Number, 5)},
                                {"qwer", new (Consts.VariableTypes.String, "5")}
                            }
                        )
                    },
                }
            };
            
            Assertions.AssertScopeListEqual(interpreterResult, expected);
        }
    }
}