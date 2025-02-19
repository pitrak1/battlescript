using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class InterpreterTests
{
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