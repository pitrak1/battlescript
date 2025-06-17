using Battlescript;

namespace BattlescriptTests;

public static class VariablesAndOperatorsTests
{
    [TestFixture]
    public class VariableTypes
    {
        [Test]
        public void SupportsStringVariablesUsingSingleQuotes()
        {
            var input = "x = 'asdf'";
            var expected = new StringVariable("asdf");
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsStringVariablesUsingDoubleQuotes()
        {
            var input = "x = \"asdf\"";
            var expected = new StringVariable("asdf");
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsFloats()
        {
            var input = "x = 5.5";
            var expected = new FloatVariable(5.5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsIntegers()
        {
            var input = "x = 5";
            var expected = new IntegerVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }

        [Test]
        public void SupportsBooleans()
        {
            var input = "x = True";
            var expected = new BooleanVariable(true);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsLists()
        {
            var input = "x = []";
            var expected = new ListVariable([]);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsDictionaries()
        {
            var input = "x = {}";
            var expected = new DictionaryVariable([]);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsClasses()
        {
            var input = "class asdf():\n\ty = 3";
            var expected = new ClassVariable(new Dictionary<string, Variable> {{"y", new IntegerVariable(3)}});
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("asdf"));
            Assert.That(result[0]["asdf"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsObjects()
        {
            var input = "class asdf():\n\ty = 3\nx = asdf()";
            var classValues = new Dictionary<string, Variable> { { "y", new IntegerVariable(3) } };
            var expected = new ObjectVariable(classValues, new ClassVariable(classValues));
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
    
    [TestFixture]
    public class OperatorSupport
    {
        [Test]
        public void SupportsUnaryPlus()
        {
            var input = "x = +6";
            var expected = new IntegerVariable(6);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class OperatorPrecedence
    {
        [Test]
        public void ValuesParenthesesOverPower()
        {
            var input = "x = 2 ** (1 + 2)";
            var expected = new IntegerVariable(8);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void ValuesPowerOverDivisionMultiplicationAndModulo()
        {
            var input = "x = 4 * 2 ** 3";
            var expected = new IntegerVariable(32);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }

        [Test]
        public void ValuesDivisionMultiplicationAndModuloOverAdditionAndSubtraction()
        {
            var input = "x = 8 - 16 // 4";
            var expected = new IntegerVariable(4);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void ValuesAdditionAndSubtractionOverComparison()
        {
            var input = "x = 3 <= 8 - 7";
            var expected = new BooleanVariable(false);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        // I would have liked to add test cases here to make sure the precedence order is
        // Comparison > not > and > or, but I can't come up with good test cases. Maybe I'll come back to this.
    }
}