using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class InterpreterTests
{
    [TestFixture]
    public class Assignments
    {
        [Test]
        public void HandlesBasicAssignmentsFromLiteralToVariable()
        {
            var input = "x = 5";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 5) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesBasicAssignmentsFromVariableToVariable()
        {
            var input = "x = 5\ny = x";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "y", new Variable(Consts.VariableTypes.Number, 5) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }

        [Test]
        public void HandlesAdditionAssignment()
        {
            var input = "x = 5\nx += 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 7) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesSubtractionAssignment()
        {
            var input = "x = 5\nx -= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 3) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesMultiplicationAssignment()
        {
            var input = "x = 5\nx *= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 10) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesDivisionAssignment()
        {
            var input = "x = 5\nx /= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 2.5) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesModuloAssignment()
        {
            var input = "x = 5\nx %= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 1) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesFloorDivisionAssignment()
        {
            var input = "x = 5\nx //= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 2) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesPowerAssignment()
        {
            var input = "x = 5\nx **= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 25) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesBitwiseAndAssignment()
        {
            // 00000111 & 00010101 = 00000101
            var input = "x = 7\nx &= 21";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 5) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesBitwiseOrAssignment()
        {
            // 0101 | 0011 = 0111
            var input = "x = 5\nx |= 3";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 7) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesBitwiseXorAssignment()
        {
            // 0101 | 0011 = 0110
            var input = "x = 5\nx ^= 3";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 6) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesBitwiseRightShiftAssignment()
        {
            // 0101 >> 2 = 0001
            var input = "x = 5\nx >>= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 1) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesBitwiseLeftShiftAssignment()
        {
            // 0101 << 2 = 10100
            var input = "x = 5\nx <<= 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 20) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
    }
    
    [TestFixture]
    public class Operations
    {
        [Test]
        public void HandlesEquality()
        {
            var input = "x = 5 == 5\ny = 5 == 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, true) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, false) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesLessThan()
        {
            var input = "x = 5 < 5\ny = 5 < 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, false) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, true) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesGreaterThan()
        {
            var input = "x = 5 > 5\ny = 7 > 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, false) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, true) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesAddition()
        {
            var input = "x = 5 + 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 11) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesMultiplication()
        {
            var input = "x = 5 * 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 30) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
    }
    
    [TestFixture]
    public class Separators
    {
        [Test]
        public void HandlesArrayDefinition()
        {
            var input = "x = [5, '5']";
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
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesTupleDefinition()
        {
            var input = "x = (5, '5')";
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
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesSetDefinition()
        {
            var input = "x = {5, '5'}";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new (
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
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesDictionaryDefinition()
        {
            var input = "x = {'asdf': 5, 'qwer': '5'}";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new (
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
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
    }

    [TestFixture]
    public class If
    {
        [Test]
        public void HandlesTrueIfStatement()
        {
            var input = "x = 5\nif x == 5:\n\tx = 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new (Consts.VariableTypes.Number, 6)
                    },
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesFalseIfStatement()
        {
            var input = "x = 5\nif x == 6:\n\tx = 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new (Consts.VariableTypes.Number, 5)
                    },
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
    }
}