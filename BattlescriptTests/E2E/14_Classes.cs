using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static partial class E2ETests
{
    [TestFixture]
    public class Classes
    {
        [Test]
        public void AllowsDefiningClasses()
        {
            var input = "class asdf:\n\ti = 1234";
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    {"i", new NumberVariable(1234)}
                });
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("asdf"));
            Assert.That(result[0]["asdf"], Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsCreatingClassObjects()
        {
            var values = new Dictionary<string, Variable>()
            {
                { "i", new NumberVariable(1234) }
            };
            var input = "class asdf:\n\ti = 1234\nx = asdf()";
            var expected = new ObjectVariable(
                values,
                new ClassVariable(values));
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void IgnoresMethodsWhenCreatingObjects()
        {
            var classValues = new Dictionary<string, Variable>()
            {
                { "i", new NumberVariable(1234) },
                { "j", new FunctionVariable([], [new ReturnInstruction(new NumberInstruction(1234))])}
            };
            var objectValues = new Dictionary<string, Variable>()
            {
                { "i", new NumberVariable(1234) }
            };
            var input = @"
class asdf:
    i = 1234
    def j():
        return 5

x = asdf()";
            var expected = new ObjectVariable(
                objectValues,
                new ClassVariable(classValues));
            var result = Runner.Run(input);
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsSuperclasses()
        {
            var input = "class asdf:\n\ti = 1234\nclass qwer(asdf):\n\tj = 2345";
            var superclass = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    { "i", new NumberVariable(1234) }
                });
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    { "j", new NumberVariable(2345) }
                }, [superclass]);
            var result = Runner.Run(input);
            Assert.That(result[0]["qwer"], Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsAccessingValueMembers()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\ny = x.i";
            var expected = new NumberVariable(1234);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected));
        }
        
        [Test]
        public void ChangingValueMembersDoesNotAlterClass()
        {
            var input = "class asdf:\n\ti = 1234\nx = asdf()\nx.i = 6";
            var expected = new ClassVariable(
                new Dictionary<string, Variable>()
                {
                    { "i", new NumberVariable(1234) }
                }
            );
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("asdf"));
            Assert.That(result[0]["asdf"], Is.EqualTo(expected));
        }
        
        [Test]
        public void AllowsAccessingMethods()
        {
            var input = @"
class asdf:
    i = 1234
    def j():
        i = 2345
x = asdf()
x.j()
y = x.i
";
            var expected = new NumberVariable(2345);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("y"));
            Assert.That(result[0]["y"], Is.EqualTo(expected));
        }
    }
}