using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ObjectVariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void RunsOverrideWhenGetItemMethodExists()
        {
            // This is effectively creating a class that overrides the [] functionality by always returning 5,
            // and we're showing this by indexing with "x" because it doesn't matter what we index with
            var getItemFunction = new FunctionVariable(
                [new VariableInstruction("self"), new VariableInstruction("index")], 
                [new ReturnInstruction(new NumberInstruction(5.0))]);
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"__getitem__", getItemFunction},
                {"x", new NumberVariable(10.0)}
            });
            var objectVariable = classVariable.CreateObject();
            var index = new SquareBracketsInstruction([new StringInstruction("x")]);
            var result = objectVariable.GetItem(new Memory(), index);
            Assert.That(result, Is.InstanceOf<NumberVariable>());
            Assert.That(((NumberVariable)result).Value, Is.EqualTo(5.0));
        }
        
        [Test]
        public void DoesNotRunOverrideIfObjectContextIsNotGiven()
        {
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new NumberVariable(7.0)},
            });
            var objectVariable = new ObjectVariable(null, classVariable);
            var index = new SquareBracketsInstruction([new StringInstruction("x")]);
            var result = objectVariable.GetItem(new Memory(), index);
            Assert.That(result, Is.InstanceOf<NumberVariable>());
            Assert.That(((NumberVariable)result).Value, Is.EqualTo(7.0));
        }
    }
}