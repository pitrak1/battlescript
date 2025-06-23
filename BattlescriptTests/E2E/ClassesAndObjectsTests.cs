using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ClassesAndObjectsTests
{
    [TestFixture]
    public class Constructors
    {
        [Test]
        public void SupportsClassesWithNoConstructors()
        {
            var input = @"
class asdf:
    y = 5
z = asdf()
x = z.y";
            var expected = new IntegerVariable(5);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsConstructorsWithNoParameters()
        {
            var input = @"
class asdf:
    y = 5

    def __init__(self):
        self.y = 6
z = asdf()
x = z.y";
            var expected = new IntegerVariable(6);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsConstructorsWithParameters()
        {
            var input = @"
class asdf:
    y = 5

    def __init__(self, a):
        self.y = a
z = asdf(9)
x = z.y";
            var expected = new IntegerVariable(9);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsConstructorsInSuperclasses()
        {
            var input = @"
class asdf:
    def __init__(self, a):
        self.y = a

class qwer(asdf):
    y = 6

z = qwer(9)
x = z.y";
            var expected = new IntegerVariable(9);
            var result = Runner.Run(input);
            Assert.That(result[0], Contains.Key("x"));
            Assert.That(result[0]["x"], Is.EqualTo(expected));
        }
    }
}