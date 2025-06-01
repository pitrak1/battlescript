using Battlescript;

namespace BattlescriptTests;

public class DictionaryVariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var dictionary = new DictionaryVariable([
                new KeyValuePairVariable(new NumberVariable(1), new NumberVariable(2)),
                new KeyValuePairVariable(new StringVariable("asdf"), new NumberVariable(4))
            ]);
            var indexInstruction = new SquareBracketsInstruction([new StringInstruction("asdf")]);
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<NumberVariable>());
            Assert.That(((NumberVariable)result).Value, Is.EqualTo(4));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var innerDictionary = new DictionaryVariable([
                new KeyValuePairVariable(new StringVariable("qwer"), new NumberVariable(3)),
                new KeyValuePairVariable(new StringVariable("zxcv"), new NumberVariable(5))
            ]);
            var dictionary = new DictionaryVariable([
                new KeyValuePairVariable(new NumberVariable(1), new NumberVariable(2)),
                new KeyValuePairVariable(new StringVariable("asdf"), innerDictionary)
            ]);
            var indexInstruction = new SquareBracketsInstruction(
                [new StringInstruction("asdf")], 
                new SquareBracketsInstruction([new StringInstruction("zxcv")]));
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<NumberVariable>());
            Assert.That(((NumberVariable)result).Value, Is.EqualTo(5));
        }
    }
    
    [TestFixture]
    public class SetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var dictionary = new DictionaryVariable([
                new KeyValuePairVariable(new NumberVariable(1), new NumberVariable(2)),
                new KeyValuePairVariable(new StringVariable("asdf"), new NumberVariable(4))
            ]);
            var indexInstruction = new SquareBracketsInstruction([new StringInstruction("asdf")]);
            var valueVariable = new NumberVariable(10);
            dictionary.SetItem(new Memory(), valueVariable, indexInstruction);
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<NumberVariable>());
            Assert.That(((NumberVariable)result).Value, Is.EqualTo(10));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var innerDictionary = new DictionaryVariable([
                new KeyValuePairVariable(new StringVariable("qwer"), new NumberVariable(3)),
                new KeyValuePairVariable(new StringVariable("zxcv"), new NumberVariable(5))
            ]);
            var dictionary = new DictionaryVariable([
                new KeyValuePairVariable(new NumberVariable(1), new NumberVariable(2)),
                new KeyValuePairVariable(new StringVariable("asdf"), innerDictionary)
            ]);
            var indexInstruction = new SquareBracketsInstruction(
                [new StringInstruction("asdf")], 
                new SquareBracketsInstruction([new StringInstruction("zxcv")]));
            var valueVariable = new NumberVariable(10);
            dictionary.SetItem(new Memory(), valueVariable, indexInstruction);
            var result = dictionary.GetItem(new Memory(), indexInstruction);
            Assert.That(result, Is.TypeOf<NumberVariable>());
            Assert.That(((NumberVariable)result).Value, Is.EqualTo(10));
        }
    }
}