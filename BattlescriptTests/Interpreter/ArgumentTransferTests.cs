using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public class ArgumentTransferTests
{
    [TestFixture]
    public class GetVariableDictionary
    {
        [TestFixture]
        public class BasicPositionalAndKeyword
        {
            [Test]
            public void PositionalArguments()
            {
                // def foo(a, b): pass
                // foo(1234, "asdf")
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1234),
                    new StringInstruction("asdf")
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new VariableInstruction("b")
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                    { "b", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void KeywordArguments()
            {
                // def foo(a, b): pass
                // foo(a=1234, b="asdf")
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1234)),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new StringInstruction("asdf"))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new VariableInstruction("b")
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                    { "b", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void MixedArguments()
            {
                // def foo(a, b): pass
                // foo(1234, b="asdf")
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1234),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new StringInstruction("asdf"))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new VariableInstruction("b")
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                    { "b", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void RespectsDefaultValues()
            {
                // def foo(a, b="asdf"): pass
                // foo(1234)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1234)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new StringInstruction("asdf"))
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                    { "b", BtlTypes.Create(BtlTypes.Types.String, "asdf") }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void PrioritizesGivenValuesOverDefaultValues()
            {
                // def foo(a, b="asdf"): pass
                // foo(1234, 5678)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1234),
                    new NumericInstruction(5678)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new StringInstruction("asdf"))
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1234) },
                    { "b", BtlTypes.Create(BtlTypes.Types.Int, 5678) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ThrowsErrorIfTooManyArguments()
            {
                // def foo(a, b): pass
                // foo(1234, 5678, 9012)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1234),
                    new NumericInstruction(5678),
                    new NumericInstruction(9012)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new VariableInstruction("b")
                });
                Assert.Throws<InternalRaiseException>(() =>
                    ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters));
            }

            [Test]
            public void ThrowsErrorIfUnknownKeywordArgument()
            {
                // def foo(a, b="asdf"): pass
                // foo(1234, unknown="asdf")
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1234),
                    new AssignmentInstruction("=", new VariableInstruction("unknown"), new StringInstruction("asdf"))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new StringInstruction("asdf"))
                });
                Assert.Throws<InternalRaiseException>(() =>
                    ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters));
            }

            [Test]
            public void ThrowsErrorIfNotAllParametersHaveValues()
            {
                // def foo(a, b, c): pass
                // foo(1234)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1234)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new VariableInstruction("b"),
                    new VariableInstruction("c")
                });
                Assert.Throws<InternalRaiseException>(() =>
                    ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters));
            }
        }

        [TestFixture]
        public class Varargs
        {

            [Test]
            public void ArgsCollectsExtraPositionalArguments()
            {
                // def foo(a, *args): pass
                // foo(1, 2, 3, 4)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new NumericInstruction(3),
                    new NumericInstruction(4)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    {
                        "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                            BtlTypes.Create(BtlTypes.Types.Int, 2),
                            BtlTypes.Create(BtlTypes.Types.Int, 3),
                            BtlTypes.Create(BtlTypes.Types.Int, 4)
                        ]))
                    }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ArgsIsEmptyListWhenNoExtraPositionalArguments()
            {
                // def foo(a, *args): pass
                // foo(1)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    { "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable()) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ArgsOnlyWithNoRegularParameters()
            {
                // def foo(*args): pass
                // foo(1, 2, 3)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new NumericInstruction(3)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    {
                        "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                            BtlTypes.Create(BtlTypes.Types.Int, 1),
                            BtlTypes.Create(BtlTypes.Types.Int, 2),
                            BtlTypes.Create(BtlTypes.Types.Int, 3)
                        ]))
                    }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ArgsOnlyWithNoArguments()
            {
                // def foo(*args): pass
                // foo()
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>());
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable()) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }
        }

        [TestFixture]
        public class Kwargs
        {
            [Test]
            public void KwargsCollectsExtraKeywordArguments()
            {
                // def foo(a, **kwargs): pass
                // foo(a=1, b=2, c=3)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(2)),
                    new AssignmentInstruction("=", new VariableInstruction("c"), new NumericInstruction(3))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);

                var expectedKwargsMapping = new MappingVariable();
                expectedKwargsMapping.StringValues["b"] = BtlTypes.Create(BtlTypes.Types.Int, 2);
                expectedKwargsMapping.StringValues["c"] = BtlTypes.Create(BtlTypes.Types.Int, 3);

                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, expectedKwargsMapping) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void KwargsIsEmptyDictWhenNoExtraKeywordArguments()
            {
                // def foo(a, **kwargs): pass
                // foo(a=1)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable()) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void KwargsOnlyWithNoRegularParameters()
            {
                // def foo(**kwargs): pass
                // foo(a=1, b=2)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(2))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);

                var expectedKwargsMapping = new MappingVariable();
                expectedKwargsMapping.StringValues["a"] = BtlTypes.Create(BtlTypes.Types.Int, 1);
                expectedKwargsMapping.StringValues["b"] = BtlTypes.Create(BtlTypes.Types.Int, 2);

                var expected = new Dictionary<string, Variable>()
                {
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, expectedKwargsMapping) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void KwargsOnlyWithNoArguments()
            {
                // def foo(**kwargs): pass
                // foo()
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>());
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable()) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }
        }

        [TestFixture]
        public class VarargsAndKwargs
        {
            [Test]
            public void ArgsAndKwargsTogether()
            {
                // def foo(a, *args, **kwargs): pass
                // foo(1, 2, 3, b=4, c=5)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new NumericInstruction(3),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(4)),
                    new AssignmentInstruction("=", new VariableInstruction("c"), new NumericInstruction(5))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")]),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);

                var expectedKwargsMapping = new MappingVariable();
                expectedKwargsMapping.StringValues["b"] = BtlTypes.Create(BtlTypes.Types.Int, 4);
                expectedKwargsMapping.StringValues["c"] = BtlTypes.Create(BtlTypes.Types.Int, 5);

                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    {
                        "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                            BtlTypes.Create(BtlTypes.Types.Int, 2),
                            BtlTypes.Create(BtlTypes.Types.Int, 3)
                        ]))
                    },
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, expectedKwargsMapping) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ArgsAndKwargsWithNoExtras()
            {
                // def foo(a, *args, **kwargs): pass
                // foo(1)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")]),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    { "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable()) },
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable()) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ArgsAndKwargsOnlyWithNoRegularParameters()
            {
                // def foo(*args, **kwargs): pass
                // foo(1, 2, a=3, b=4)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(3)),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(4))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")]),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);

                var expectedKwargsMapping = new MappingVariable();
                expectedKwargsMapping.StringValues["a"] = BtlTypes.Create(BtlTypes.Types.Int, 3);
                expectedKwargsMapping.StringValues["b"] = BtlTypes.Create(BtlTypes.Types.Int, 4);

                var expected = new Dictionary<string, Variable>()
                {
                    {
                        "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                            BtlTypes.Create(BtlTypes.Types.Int, 1),
                            BtlTypes.Create(BtlTypes.Types.Int, 2)
                        ]))
                    },
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, expectedKwargsMapping) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ArgsAndKwargsOnlyWithNoArguments()
            {
                // def foo(*args, **kwargs): pass
                // foo()
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>());
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")]),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**kwargs")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable()) },
                    { "kwargs", BtlTypes.Create(BtlTypes.Types.Dictionary, new MappingVariable()) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void KeywordOnlyParametersAfterArgs()
            {
                // def foo(a, *args, b): pass
                // foo(1, 2, 3, b=4)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new NumericInstruction(3),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(4))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")]),
                    new VariableInstruction("b")
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    {
                        "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                            BtlTypes.Create(BtlTypes.Types.Int, 2),
                            BtlTypes.Create(BtlTypes.Types.Int, 3)
                        ]))
                    },
                    { "b", BtlTypes.Create(BtlTypes.Types.Int, 4) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void KeywordOnlyParametersAfterArgsWithDefault()
            {
                // def foo(a, *args, b=10): pass
                // foo(1, 2, 3)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new NumericInstruction(3)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")]),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(10))
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    { "a", BtlTypes.Create(BtlTypes.Types.Int, 1) },
                    {
                        "args", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                            BtlTypes.Create(BtlTypes.Types.Int, 2),
                            BtlTypes.Create(BtlTypes.Types.Int, 3)
                        ]))
                    },
                    { "b", BtlTypes.Create(BtlTypes.Types.Int, 10) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void ThrowsIfKeywordOnlyParameterMissing()
            {
                // def foo(a, *args, b): pass
                // foo(1, 2, 3)  # missing b
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new NumericInstruction(3)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new VariableInstruction("a"),
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*args")]),
                    new VariableInstruction("b")
                });
                Assert.Throws<InternalRaiseException>(() =>
                    ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters));
            }

            // Custom names for *args and **kwargs

            [Test]
            public void CustomArgsName()
            {
                // def foo(*items): pass
                // foo(1, 2, 3)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new NumericInstruction(1),
                    new NumericInstruction(2),
                    new NumericInstruction(3)
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "*items")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);
                var expected = new Dictionary<string, Variable>()
                {
                    {
                        "items", BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([
                            BtlTypes.Create(BtlTypes.Types.Int, 1),
                            BtlTypes.Create(BtlTypes.Types.Int, 2),
                            BtlTypes.Create(BtlTypes.Types.Int, 3)
                        ]))
                    }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }

            [Test]
            public void CustomKwargsName()
            {
                // def foo(**options): pass
                // foo(a=1, b=2)
                var (callStack, closure) = Runner.Run("");
                var arguments = new ArgumentSet(callStack, closure, new List<Instruction>()
                {
                    new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                    new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(2))
                });
                var parameters = new ParameterSet(new List<Instruction>()
                {
                    new SpecialVariableInstruction([new Token(Consts.TokenTypes.SpecialVariable, "**options")])
                });
                var result = ArgumentTransfer.GetVariableTransferDictionary(callStack, closure, arguments, parameters);

                var expectedKwargsMapping = new MappingVariable();
                expectedKwargsMapping.StringValues["a"] = BtlTypes.Create(BtlTypes.Types.Int, 1);
                expectedKwargsMapping.StringValues["b"] = BtlTypes.Create(BtlTypes.Types.Int, 2);

                var expected = new Dictionary<string, Variable>()
                {
                    { "options", BtlTypes.Create(BtlTypes.Types.Dictionary, expectedKwargsMapping) }
                };
                Assert.That(result, Is.EquivalentTo(expected));
            }
        }
    }
}
