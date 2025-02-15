using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InterpreterTests
{
    [TestFixture]
    public class Operations
    {
        [Test]
        public void HandlesPower()
        {
            var input = "x = 5 ** 2";
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
        public void HandlesBitwiseNotPower()
        {
            var input = "x = ~5";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, -6) }
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
        
        [Test]
        public void HandlesDivision()
        {
            var input = "x = 5 / 2";
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
        public void HandlesFloorDivision()
        {
            var input = "x = 5 // 2";
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
        public void HandlesModulo()
        {
            var input = "x = 5 % 2";
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
        public void HandlesSubtraction()
        {
            var input = "x = 6 - 5";
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
        public void HandlesLeftShift()
        {
            // 0011 << 2 = 1100
            var input = "x = 3 << 2";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Number, 12) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesRightShift()
        {
            // 0110 >> 2 = 0001
            var input = "x = 6 >> 2";
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
        public void HandlesBitwiseAnd()
        {
            // 0110 & 0011 = 0010
            var input = "x = 6 & 3";
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
        public void HandlesBitwiseXor()
        {
            // 0110 ^ 0011 = 0101
            var input = "x = 6 ^ 3";
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
        public void HandlesBitwiseOr()
        {
            // 0110 | 0011 = 0111
            var input = "x = 6 | 3";
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
        public void HandlesNotEquality()
        {
            var input = "x = 5 != 5\ny = 5 != 6";
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
        public void HandlesGreaterThanOrEqualTo()
        {
            var input = "x = 5 >= 5\ny = 7 >= 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, true) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, true) }
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
        public void HandlesLessThanOrEqualTo()
        {
            var input = "x = 5 <= 5\ny = 5 <= 6";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    { "x", new Variable(Consts.VariableTypes.Boolean, true) },
                    { "y", new Variable(Consts.VariableTypes.Boolean, true) }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesBooleanNot()
        {
            var input = "x = not True\ny = not False";
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
        public void HandlesBooleanAnd()
        {
            var input = "x = True and False\ny = True and True";
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
        public void HandlesBooleanOr()
        {
            var input = "x = True or False\ny = False or False";
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
    }

    [TestFixture]
    public class Arrays
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
        public void HandlesArrayIndex()
        {
            var input = "x = [5, '5']\ny = x[1]";
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
                    {
                        "y",
                        new Variable(Consts.VariableTypes.String, "5")
                    }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesArrayRangeIndex()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:3]";
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
                                new (Consts.VariableTypes.Number, 3),
                                new (Consts.VariableTypes.Number, 2),
                                new (Consts.VariableTypes.String, "5"),
                            }
                        )
                    },
                    {
                        "y",
                        new Variable(
                            Consts.VariableTypes.List, 
                            new List<Variable>
                            {
                                new (Consts.VariableTypes.Number, 3),
                                new (Consts.VariableTypes.Number, 2),
                            }
                        )
                    }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesArrayRangeIndexWithBlankStart()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[:3]";
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
                                new (Consts.VariableTypes.Number, 3),
                                new (Consts.VariableTypes.Number, 2),
                                new (Consts.VariableTypes.String, "5"),
                            }
                        )
                    },
                    {
                        "y",
                        new Variable(
                            Consts.VariableTypes.List, 
                            new List<Variable>
                            {
                                new (Consts.VariableTypes.Number, 5),
                                new (Consts.VariableTypes.Number, 3),
                                new (Consts.VariableTypes.Number, 2),
                            }
                        )
                    }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesArrayRangeIndexWithBlankEnd()
        {
            var input = "x = [5, 3, 2, '5']\ny = x[1:]";
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
                                new (Consts.VariableTypes.Number, 3),
                                new (Consts.VariableTypes.Number, 2),
                                new (Consts.VariableTypes.String, "5"),
                            }
                        )
                    },
                    {
                        "y",
                        new Variable(
                            Consts.VariableTypes.List, 
                            new List<Variable>
                            {
                                new (Consts.VariableTypes.Number, 3),
                                new (Consts.VariableTypes.Number, 2),
                                new (Consts.VariableTypes.String, "5"),
                            }
                        )
                    }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
    }

    [TestFixture]
    public class Tuples
    {
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
        public void HandlesTupleIndex()
        {
            var input = "x = (5, '5')\ny = x[1]";
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
                    {
                        "y",
                        new Variable(Consts.VariableTypes.String, "5")
                    }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
    }

    [TestFixture]
    public class Sets
    {
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
        public void HandlesSetIndex()
        {
            var input = "x = {5, '5'}\ny = x[1]";
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
                    {
                        "y",
                        new Variable(Consts.VariableTypes.String, "5")
                    }
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
    }
    
    [TestFixture]
    public class Dictionaries
    {
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
        
        [Test]
        public void HandlesDictionaryIndex()
        {
            var input = "x = {'asdf': 5, 'qwer': '5'}\ny = x['qwer']";
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
                    {
                        "y", 
                        new Variable(Consts.VariableTypes.String, "5")
                    }
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
    
    [TestFixture]
    public class While
    {
        [Test]
        public void HandlesWhileStatement()
        {
            var input = "x = 5\nwhile x < 10:\n\tx += 1";
            var expected = new List<Dictionary<string, Variable>>
            {
                new ()
                {
                    {
                        "x", 
                        new (Consts.VariableTypes.Number, 10)
                    },
                }
            };
            InterpreterAssertions.AssertInputProducesOutput(input, expected);
        }
        
        [Test]
        public void HandlesFalseWhileStatement()
        {
            var input = "x = 5\nwhile x == 6:\n\tx = 10";
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