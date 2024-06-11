using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.Tests;
using BattleScript.Instructions;

namespace BattleScript.InterpreterTests;

public class InterpreterTests
{
    [TestFixture]
    public class AssignmentAndLiterals
    {
        [Test]
        public void Number()
        {
            Lexer lexer = new Lexer("var x = 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 5)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void String()
        {
            Lexer lexer = new Lexer("var x = 'asdf';");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.String, "asdf")}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Boolean()
        {
            Lexer lexer = new Lexer("var x = true;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Boolean, true)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [Test]
    public void GettingAndSettingVariables()
    {
        Lexer lexer = new Lexer("var x = 5; var y = x;");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Interpreter interpreter = new Interpreter(instructions);
        var result = interpreter.Run();

        var expectedResult = new Dictionary<string, Variable>() {
            {"x", new Variable(Consts.VariableTypes.Number, 5)},
            {"y", new Variable(Consts.VariableTypes.Number, 5)}
        };
        Assert.That(result, Is.EqualTo(expectedResult));
    }

    [TestFixture]
    public class Operators
    {
        [Test]
        public void EqualityTrue()
        {
            Lexer lexer = new Lexer("var x = 5 == 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Boolean, true)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        public void EqualityFalse()
        {
            Lexer lexer = new Lexer("var x = 5 == 6;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Boolean, false)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void LessThanTrue()
        {
            Lexer lexer = new Lexer("var x = 3 < 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Boolean, true)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void LessThanFalse()
        {
            Lexer lexer = new Lexer("var x = 9 < 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Boolean, false)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GreaterThanTrue()
        {
            Lexer lexer = new Lexer("var x = 9 > 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Boolean, true)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GreaterThanFalse()
        {
            Lexer lexer = new Lexer("var x = 3 > 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Boolean, false)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Addition()
        {
            Lexer lexer = new Lexer("var x = 9 + 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 14)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Multiplication()
        {
            Lexer lexer = new Lexer("var x = 9 * 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 45)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class If
    {
        [Test]
        public void TrueCondition()
        {
            Lexer lexer = new Lexer(@"
                var x = 5; 
                if (true) { x = 10; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 10)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void FalseCondition()
        {
            Lexer lexer = new Lexer(@"
                var x = 5; 
                if (false) { x = 10; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 5)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ExpressionCondition()
        {
            Lexer lexer = new Lexer(@"
                var x = 5; 
                if (x == 5) { x = 10; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 10)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IfElseTrueCondition()
        {
            Lexer lexer = new Lexer(@"
                var x = 5; 
                if (x == 5) { x = 10; } 
                else { x = 6; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 10)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IfElseFalseCondition()
        {
            Lexer lexer = new Lexer(@"
                var x = 5; 
                if (x == 10) { x = 10; } 
                else { x = 6; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 6)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IfElseIfElseFirstConditionIsTrue()
        {
            Lexer lexer = new Lexer(@"
                var x = 5; 
                if (x == 5) { x = 10; } 
                else if (x == 10) { x = 15; } 
                else { x = 6; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 10)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IfElseIfElseSecondConditionIsTrue()
        {
            Lexer lexer = new Lexer(@"
                var x = 10; 
                if (x == 5) { x = 10; } 
                else if (x == 10) { x = 15; } 
                else { x = 6; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 15)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void IfElseIfElseNoConditionIsTrue()
        {
            Lexer lexer = new Lexer(@"
                var x = 8; 
                if (x == 5) { x = 10; } 
                else if (x == 10) { x = 15; } 
                else { x = 6; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 6)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class While
    {
        [Test]
        public void TrueCondition()
        {
            Lexer lexer = new Lexer(@"
                var x = 5;
                while (x < 10) { x = x + 1; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 10)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void FalseCondition()
        {
            Lexer lexer = new Lexer(@"
                var x = 5;
                while (x < 2) { x = x + 1; }
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"x", new Variable(Consts.VariableTypes.Number, 5)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class Arrays
    {
        [Test]
        public void CreateArray()
        {
            Lexer lexer = new Lexer("var x = [1, 2, 3];");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Array,
                        new List<Variable>() {
                            new Variable(Consts.VariableTypes.Number, 1),
                            new Variable(Consts.VariableTypes.Number, 2),
                            new Variable(Consts.VariableTypes.Number, 3)
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetElement()
        {
            Lexer lexer = new Lexer("var x = [1, 2, 3]; var y = x[1];");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Array,
                        new List<Variable>() {
                            new Variable(Consts.VariableTypes.Number, 1),
                            new Variable(Consts.VariableTypes.Number, 2),
                            new Variable(Consts.VariableTypes.Number, 3)
                        }
                    )
                },
                {"y", new Variable(Consts.VariableTypes.Number, 2)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void SetElement()
        {
            Lexer lexer = new Lexer("var x = [1, 2, 3]; x[1] = 5;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Array,
                        new List<Variable>() {
                            new Variable(Consts.VariableTypes.Number, 1),
                            new Variable(Consts.VariableTypes.Number, 5),
                            new Variable(Consts.VariableTypes.Number, 3)
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class Dictionaries
    {
        [Test]
        public void CreateDictionary()
        {
            Lexer lexer = new Lexer("var x = {};");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>()
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CreateDictionaryWithNumbers()
        {
            Lexer lexer = new Lexer("var x = {5: 6};");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {5, new Variable(Consts.VariableTypes.Number, 6)}
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GettingFromDictionaryWithNumbers()
        {
            Lexer lexer = new Lexer("var x = {5: 6}; var y = x[5];");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {5, new Variable(Consts.VariableTypes.Number, 6)}
                        }
                    )
                },
                {"y", new Variable(Consts.VariableTypes.Number, 6)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void SettingDictionaryWithNumbers()
        {
            Lexer lexer = new Lexer("var x = {5: 6}; x[5] = 10;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {5, new Variable(Consts.VariableTypes.Number, 10)}
                        }
                    )
                },
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CreateDictionaryWithStrings()
        {
            Lexer lexer = new Lexer("var x = {'asdf': true};");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {"asdf", new Variable(Consts.VariableTypes.Boolean, true)}
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GettingFromDictionaryWithStrings()
        {
            Lexer lexer = new Lexer("var x = {'asdf': true}; var y = x['asdf'];");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {"asdf", new Variable(Consts.VariableTypes.Boolean, true)}
                        }
                    )
                },
                {"y", new Variable(Consts.VariableTypes.Boolean, true)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void SettingDictionaryWithStrings()
        {
            Lexer lexer = new Lexer("var x = {'asdf': true}; x['asdf'] = false;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {"asdf", new Variable(Consts.VariableTypes.Boolean, false)}
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CreateDictionaryWithVariables()
        {
            Lexer lexer = new Lexer("var y = 6; var x = {y: true};");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"y", new Variable(Consts.VariableTypes.Number, 6)},
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {6, new Variable(Consts.VariableTypes.Boolean, true)}
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GettingFromDictionaryWithVariables()
        {
            Lexer lexer = new Lexer("var y = 6; var x = {y: true}; var z = x[y]; var a = x[6];");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"y", new Variable(Consts.VariableTypes.Number, 6)},
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {6, new Variable(Consts.VariableTypes.Boolean, true)}
                        }
                    )
                },
                {"z", new Variable(Consts.VariableTypes.Boolean, true)},
                {"a", new Variable(Consts.VariableTypes.Boolean, true)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void SettingDictionaryWithVariables()
        {
            Lexer lexer = new Lexer(@"var y = 6; var x = {y: true}; x[y] = 8;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"y", new Variable(Consts.VariableTypes.Number, 6)},
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {6, new Variable(Consts.VariableTypes.Number, 8)}
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CreateDictionaryWithExpressions()
        {
            Lexer lexer = new Lexer("var x = {1 + 2: 6 + 3};");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {3, new Variable(Consts.VariableTypes.Number, 9)}
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GettingFromDictionaryWithExpressions()
        {
            Lexer lexer = new Lexer("var x = {1 + 2: 6 + 3}; var y = x[2 + 1];");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {3, new Variable(Consts.VariableTypes.Number, 9)}
                        }
                    )
                },
                {"y", new Variable(Consts.VariableTypes.Number, 9)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void SettingDictionaryWithExpressions()
        {
            Lexer lexer = new Lexer("var x = {1 + 2: 6 + 3}; x[2 + 1] = 12;");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Dictionary,
                        new Dictionary<dynamic, Variable>() {
                            {3, new Variable(Consts.VariableTypes.Number, 12)}
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class Functions
    {
        [Test]
        public void CreateFunction()
        {
            Lexer lexer = new Lexer("var x = function() { var y = 4; }");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>()
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CallFunction()
        {
            Lexer lexer = new Lexer("var x = function() { var y = 4; } x();");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>()
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReturnValue()
        {
            Lexer lexer = new Lexer("var x = function() { return 4; } var y = x();");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>()
                    )
                },
                {"y", new Variable(Consts.VariableTypes.Number, 4)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CreateFunctionWithArguments()
        {
            Lexer lexer = new Lexer("var x = function(y, z) { var a = 4; }");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>() {
                            new Variable(null, "y"),
                            new Variable(null, "z")
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void CallFunctionWithArgumentsAndReturn()
        {
            Lexer lexer = new Lexer("var x = function(y, z) { return y + z; } var a = x(1, 2);");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>() {
                            new Variable(null, "y"),
                            new Variable(null, "z")
                        }
                    )
                },
                {"a", new Variable(Consts.VariableTypes.Number, 3)}
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ValueArgsAreNotModified()
        {
            Lexer lexer = new Lexer(@"
                var y = 5;
                var x = function(z) { z = z + 3; } x(y);");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"y", new Variable(Consts.VariableTypes.Number, 5)},
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>() {
                            new Variable(null, "z")
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void ReferenceArgsAreModified()
        {
            Lexer lexer = new Lexer(@"
                var y = [5];
                var x = function(z) { z[0] = 9; } x(y);");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {
                    "y",
                    new Variable(
                        Consts.VariableTypes.Array,
                        new List<Variable>() {
                            new Variable(Consts.VariableTypes.Number, 9)
                        }
                    )
                },
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>() {
                            new Variable(null, "z")
                        }
                    )
                }
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class ScopeResolution
    {
        [Test]
        public void InnerScopeIsPreferred()
        {
            Lexer lexer = new Lexer(@"
                var y = 5;
                var x = function() { 
                    var y = 8;
                    return y;
                } 
                var z = x();
            ");
            var tokens = lexer.Run();

            Parser parser = new Parser(tokens);
            var instructions = parser.Run();

            Interpreter interpreter = new Interpreter(instructions);
            var result = interpreter.Run();

            var expectedResult = new Dictionary<string, Variable>() {
                {"y", new Variable(Consts.VariableTypes.Number, 5)},
                {
                    "x",
                    new Variable(
                        Consts.VariableTypes.Function,
                        new List<Variable>()
                    )
                },
                {"z", new Variable(Consts.VariableTypes.Number, 8)},
            };
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }

    [TestFixture]
    public class Btl
    {

    }
}