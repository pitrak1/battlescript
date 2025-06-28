using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class DictionaryVariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var dictionary = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new IntegerVariable(1), new IntegerVariable(2) },
                { new StringVariable("asdf"), new IntegerVariable(4) }
            });
            
            var indexInstruction = new SquareBracketsInstruction(new StringInstruction("asdf"));
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(4));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var innerDictionary = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new StringVariable("qwer"), new IntegerVariable(3) },
                { new StringVariable("zxcv"), new IntegerVariable(5) }
            });

            var dictionary = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new IntegerVariable(1), new IntegerVariable(2) },
                { new StringVariable("asdf"), innerDictionary }
            });
            
            var indexInstruction = new SquareBracketsInstruction(
                new StringInstruction("asdf"), 
                new SquareBracketsInstruction(new StringInstruction("zxcv")));
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(5));
        }
    }
    
    [TestFixture]
    public class SetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var dictionary = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new IntegerVariable(1), new IntegerVariable(2) },
                { new StringVariable("asdf"), new IntegerVariable(4) }
            });
            var indexInstruction = new SquareBracketsInstruction(new StringInstruction("asdf"));
            var valueVariable = new IntegerVariable(10);
            dictionary.SetItem(new Memory(), valueVariable, indexInstruction);
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(10));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var innerDictionary = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new StringVariable("qwer"), new IntegerVariable(3) },
                { new StringVariable("zxcv"), new IntegerVariable(5) }
            });

            var dictionary = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new IntegerVariable(1), new IntegerVariable(2) },
                { new StringVariable("asdf"), innerDictionary }
            });
            var indexInstruction = new SquareBracketsInstruction(
                new StringInstruction("asdf"), 
                new SquareBracketsInstruction(new StringInstruction("zxcv")));
            var valueVariable = new IntegerVariable(10);
            dictionary.SetItem(new Memory(), valueVariable, indexInstruction);
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(10));
        }
    }
}