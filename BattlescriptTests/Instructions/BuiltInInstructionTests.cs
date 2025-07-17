using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class BuiltInInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void HandlesNoArguments()
        {
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: []
            );
            Assertions.AssertInputProducesParserOutput("super()", expected);
        }
        
        [Test]
        public void HandlesArguments()
        {
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: [new VariableInstruction("x"), new VariableInstruction("y")]
            );
            Assertions.AssertInputProducesParserOutput("super(x, y)", expected);
        }
        
        [Test]
        public void HandlesTokensAfterArguments()
        {
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: [new VariableInstruction("x"), new VariableInstruction("y")],
                next: new MemberInstruction("asdf")
            );
            Assertions.AssertInputProducesParserOutput("super(x, y).asdf", expected);
        }
    }

    [TestFixture]
    public class Interpret
    {
        [TestFixture]
        public class Range
        {
            [Test]
            public void HandlesSingleArgument()
            {
                var input = "x = range(5)";
                var memory = Runner.Run(input);
                var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>() {
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 0),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 1),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
                });
                Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
            }
            
            [Test]
            public void HandlesTwoArguments()
            {
                var input = "x = range(2, 5)";
                var memory = Runner.Run(input);
                var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>() {
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 3),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
                });
                Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
            }
            
            [Test]
            public void HandlesThreeArguments()
            {
                var input = "x = range(2, 10, 2)";
                var memory = Runner.Run(input);
                var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>() {
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 8),
                });
                Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
            }
            
            [Test]
            public void HandlesCountNotMatchingStep()
            {
                var input = "x = range(2, 5, 2)";
                var memory = Runner.Run(input);
                var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>() {
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 4),
                });
                Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
            }
            
            [Test]
            public void HandlesDecreasingRange()
            {
                var input = "x = range(2, -5, -2)";
                var memory = Runner.Run(input);
                var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>() {
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 2),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 0),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", -2),
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", -4),
                });
                Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
            }
            
            [Test]
            public void ReturnsEmptyListIfGivenInfiniteRange()
            {
                var input = "x = range(2, -5, 2)";
                var memory = Runner.Run(input);
                var expected = BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "list", new List<Variable>());
                
                Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], expected);
            }
        }
    
        [TestFixture]
        public class IsInstance
        {
            [Test]
            public void ReturnsTrueIfObjectIsDirectInstanceOfClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        x = asdf()
                                        y = isinstance(x, asdf)
                                        """);
                Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], new ConstantVariable(true));
            }
            
            [Test]
            public void ReturnsTrueIfObjectIsInheritedInstanceOfClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer(asdf):
                                            j = 6
                                            
                                        x = qwer()
                                        y = isinstance(x, asdf)
                                        """);
                Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], new ConstantVariable(true));
            }
            
            [Test]
            public void ReturnsFalseIfObjectIsNotInstanceOfClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer:
                                            j = 6
                                            
                                        x = qwer()
                                        y = isinstance(x, asdf)
                                        """);
                Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], new ConstantVariable(false));
            }
        }
        
        [TestFixture]
        public class IsSubclass
        {
            [Test]
            public void ReturnsTrueIfFirstClassIsEqualToSecondClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        y = issubclass(asdf, asdf)
                                        """);
                Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], new ConstantVariable(true));
            }
            
            [Test]
            public void ReturnsTrueIfFirstClassInheritsFromSecondClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer(asdf):
                                            j = 6
                                            
                                        y = issubclass(qwer, asdf)
                                        """);
                Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], new ConstantVariable(true));
            }
            
            [Test]
            public void ReturnsFalseIfFirstClassDoesNotInheritFromSecondClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer(asdf):
                                            j = 6
                                            
                                        y = issubclass(asdf, qwer)
                                        """);
                Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], new ConstantVariable(false));
            }
        }
    }
}