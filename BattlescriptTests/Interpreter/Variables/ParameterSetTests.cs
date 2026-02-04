using Battlescript;

namespace BattlescriptTests.InterpreterTests.Variables;

[TestFixture]
public class ParameterSetTests
{
    // Helper to create *args or **kwargs instructions
    private static VariableInstruction CreateSpecialVariable(string name, int asterisks)
    {
        var prefix = new string('*', asterisks);
        var tokens = new List<Token> { new(Consts.TokenTypes.SpecialVariable, prefix + name) };
        return new VariableInstruction(tokens);
    }

    // Helper to create / or * marker (Positionals-only or keyword-only separator)
    private static OperationInstruction CreateMarker(string marker) => new(marker);

    [TestFixture]
    public class BasicParameters
    {
        [Test]
        public void SimpleParameters()
        {
            // def foo(a, b)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.Positionals, Is.Empty);
            Assert.That(parameters.Keywords, Is.Empty);
            Assert.That(parameters.ArgsName, Is.Null);
            Assert.That(parameters.KwargsName, Is.Null);
        }

        [Test]
        public void ParametersWithDefaults()
        {
            // def foo(a, b=1)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(1))
            ]);

            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.DefaultValues.ContainsKey("b"), Is.True);
            Assert.That(parameters.DefaultValues.ContainsKey("a"), Is.False);
        }

        [Test]
        public void AllDefaults()
        {
            // def foo(a=1, b=2)
            var parameters = new ParameterSet([
                new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(2))
            ]);

            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.DefaultValues.ContainsKey("a"), Is.True);
            Assert.That(parameters.DefaultValues.ContainsKey("b"), Is.True);
        }
    }

    [TestFixture]
    public class PositionalOnlyParameters
    {
        [Test]
        public void SimplePositionalOnly()
        {
            // def foo(a, b, /)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                new VariableInstruction("b"),
                CreateMarker("/")
            ]);

            Assert.That(parameters.Positionals, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.Any, Is.Empty);
            Assert.That(parameters.Keywords, Is.Empty);
        }

        [Test]
        public void PositionalOnlyWithDefaults()
        {
            // def foo(a, b=1, /)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(1)),
                CreateMarker("/")
            ]);

            Assert.That(parameters.Positionals, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.DefaultValues.ContainsKey("b"), Is.True);
        }

        [Test]
        public void PositionalOnlyWithRegularParams()
        {
            // def foo(a, /, b)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                CreateMarker("/"),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.Positionals, Is.EqualTo(new List<string> { "a" }));
            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "b" }));
        }
    }

    [TestFixture]
    public class KeywordOnlyParameters
    {
        [Test]
        public void SimpleKeywordOnly()
        {
            // def foo(*, a, b)
            var parameters = new ParameterSet([
                CreateMarker("*"),
                new VariableInstruction("a"),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.Positionals, Is.Empty);
            Assert.That(parameters.Any, Is.Empty);
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "a", "b" }));
        }

        [Test]
        public void KeywordOnlyWithDefaults()
        {
            // def foo(*, a, b=1)
            var parameters = new ParameterSet([
                CreateMarker("*"),
                new VariableInstruction("a"),
                new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(1))
            ]);

            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.DefaultValues.ContainsKey("b"), Is.True);
        }

        [Test]
        public void RegularAndKeywordOnly()
        {
            // def foo(a, *, b)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                CreateMarker("*"),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "a" }));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "b" }));
        }
    }

    [TestFixture]
    public class ArgsAndKwargs
    {
        [Test]
        public void OnlyArgs()
        {
            // def foo(*args)
            var parameters = new ParameterSet([
                CreateSpecialVariable("args", 1)
            ]);

            Assert.That(parameters.ArgsName, Is.EqualTo("args"));
            Assert.That(parameters.KwargsName, Is.Null);
            Assert.That(parameters.Any, Is.Empty);
        }

        [Test]
        public void OnlyKwargs()
        {
            // def foo(**kwargs)
            var parameters = new ParameterSet([
                CreateSpecialVariable("kwargs", 2)
            ]);

            Assert.That(parameters.ArgsName, Is.Null);
            Assert.That(parameters.KwargsName, Is.EqualTo("kwargs"));
        }

        [Test]
        public void BothArgsAndKwargs()
        {
            // def foo(*args, **kwargs)
            var parameters = new ParameterSet([
                CreateSpecialVariable("args", 1),
                CreateSpecialVariable("kwargs", 2)
            ]);

            Assert.That(parameters.ArgsName, Is.EqualTo("args"));
            Assert.That(parameters.KwargsName, Is.EqualTo("kwargs"));
        }

        [Test]
        public void CustomArgsName()
        {
            // def foo(*items)
            var parameters = new ParameterSet([
                CreateSpecialVariable("items", 1)
            ]);

            Assert.That(parameters.ArgsName, Is.EqualTo("items"));
        }

        [Test]
        public void CustomKwargsName()
        {
            // def foo(**options)
            var parameters = new ParameterSet([
                CreateSpecialVariable("options", 2)
            ]);

            Assert.That(parameters.KwargsName, Is.EqualTo("options"));
        }

        [Test]
        public void ArgsWithKeywordOnlyParams()
        {
            // def foo(*args, a, b)
            var parameters = new ParameterSet([
                CreateSpecialVariable("args", 1),
                new VariableInstruction("a"),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.ArgsName, Is.EqualTo("args"));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "a", "b" }));
        }

        [Test]
        public void RegularParamsWithArgsAndKwargs()
        {
            // def foo(a, b, *args, **kwargs)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                new VariableInstruction("b"),
                CreateSpecialVariable("args", 1),
                CreateSpecialVariable("kwargs", 2)
            ]);

            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.ArgsName, Is.EqualTo("args"));
            Assert.That(parameters.KwargsName, Is.EqualTo("kwargs"));
        }

        [Test]
        public void ArgsKeywordOnlyAndKwargs()
        {
            // def foo(*args, a, b, **kwargs)
            var parameters = new ParameterSet([
                CreateSpecialVariable("args", 1),
                new VariableInstruction("a"),
                new VariableInstruction("b"),
                CreateSpecialVariable("kwargs", 2)
            ]);

            Assert.That(parameters.ArgsName, Is.EqualTo("args"));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.KwargsName, Is.EqualTo("kwargs"));
        }
    }

    [TestFixture]
    public class FullParameterCombinations
    {
        [Test]
        public void PositionalOnlyAndRegular()
        {
            // def foo(a, /, b, c)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                CreateMarker("/"),
                new VariableInstruction("b"),
                new VariableInstruction("c")
            ]);

            Assert.That(parameters.Positionals, Is.EqualTo(new List<string> { "a" }));
            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "b", "c" }));
        }

        [Test]
        public void PositionalOnlyRegularAndKeywordOnly()
        {
            // def foo(a, /, b, *, c)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                CreateMarker("/"),
                new VariableInstruction("b"),
                CreateMarker("*"),
                new VariableInstruction("c")
            ]);

            Assert.That(parameters.Positionals, Is.EqualTo(new List<string> { "a" }));
            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "b" }));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "c" }));
        }

        [Test]
        public void FullSignatureWithArgs()
        {
            // def foo(a, /, b, *args, c, **kwargs)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                CreateMarker("/"),
                new VariableInstruction("b"),
                CreateSpecialVariable("args", 1),
                new VariableInstruction("c"),
                CreateSpecialVariable("kwargs", 2)
            ]);

            Assert.That(parameters.Positionals, Is.EqualTo(new List<string> { "a" }));
            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "b" }));
            Assert.That(parameters.ArgsName, Is.EqualTo("args"));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "c" }));
            Assert.That(parameters.KwargsName, Is.EqualTo("kwargs"));
        }

        [Test]
        public void FullSignatureWithDefaults()
        {
            // def foo(a, b=1, /, c=2, *args, d, e=3, **kwargs)
            var parameters = new ParameterSet([
                new VariableInstruction("a"),
                new AssignmentInstruction("=", new VariableInstruction("b"), new NumericInstruction(1)),
                CreateMarker("/"),
                new AssignmentInstruction("=", new VariableInstruction("c"), new NumericInstruction(2)),
                CreateSpecialVariable("args", 1),
                new VariableInstruction("d"),
                new AssignmentInstruction("=", new VariableInstruction("e"), new NumericInstruction(3)),
                CreateSpecialVariable("kwargs", 2)
            ]);

            Assert.That(parameters.Positionals, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "c" }));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "d", "e" }));
            Assert.That(parameters.DefaultValues.ContainsKey("b"), Is.True);
            Assert.That(parameters.DefaultValues.ContainsKey("c"), Is.True);
            Assert.That(parameters.DefaultValues.ContainsKey("e"), Is.True);
            Assert.That(parameters.ArgsName, Is.EqualTo("args"));
            Assert.That(parameters.KwargsName, Is.EqualTo("kwargs"));
        }
    }

    [TestFixture]
    public class SyntaxErrors
    {
        [Test]
        public void NonDefaultAfterDefault()
        {
            // def foo(a=1, b) - invalid
            Assert.Throws<InternalRaiseException>(() => new ParameterSet([
                new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                new VariableInstruction("b")
            ]));
        }

        [Test]
        public void NonDefaultAfterDefaultAllowedAfterStar()
        {
            // def foo(a=1, *, b) - valid, b is keyword-only
            var parameters = new ParameterSet([
                new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                CreateMarker("*"),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "a" }));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "b" }));
        }

        [Test]
        public void NonDefaultAfterDefaultAllowedAfterArgs()
        {
            // def foo(a=1, *args, b) - valid, b is keyword-only
            var parameters = new ParameterSet([
                new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                CreateSpecialVariable("args", 1),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.Any, Is.EqualTo(new List<string> { "a" }));
            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "b" }));
        }

        [Test]
        public void DefaultBeforeRequiredInKeywordOnly()
        {
            // def foo(*, a=1, b) - valid in Python, Keywords-only params can have defaults before required
            var parameters = new ParameterSet([
                CreateMarker("*"),
                new AssignmentInstruction("=", new VariableInstruction("a"), new NumericInstruction(1)),
                new VariableInstruction("b")
            ]);

            Assert.That(parameters.Keywords, Is.EqualTo(new List<string> { "a", "b" }));
            Assert.That(parameters.DefaultValues.ContainsKey("a"), Is.True);
            Assert.That(parameters.DefaultValues.ContainsKey("b"), Is.False);
        }

        [Test]
        public void StarAndArgsNotAllowed()
        {
            // def foo(*, *args) - invalid
            Assert.Throws<InternalRaiseException>(() => new ParameterSet([
                CreateMarker("*"),
                CreateSpecialVariable("args", 1)
            ]));
        }

        [Test]
        public void KwargsMustBeLast()
        {
            // def foo(**kwargs, a) - invalid
            Assert.Throws<InternalRaiseException>(() => new ParameterSet([
                CreateSpecialVariable("kwargs", 2),
                new VariableInstruction("a")
            ]));
        }

        [Test]
        public void KwargsBeforeArgsNotAllowed()
        {
            // def foo(**kwargs, *args) - invalid
            Assert.Throws<InternalRaiseException>(() => new ParameterSet([
                CreateSpecialVariable("kwargs", 2),
                CreateSpecialVariable("args", 1)
            ]));
        }
    }
}
