using Battlescript;

namespace BattlescriptTests.InterpreterTests.VariablesTests;

[TestFixture]
public static class DictionaryVariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var dictionary = new DictionaryVariable(
                new Dictionary<int, Variable>() { { 1, new NumericVariable(2) } },
                new Dictionary<string, Variable>() { { "asdf", new NumericVariable(4) } });
            
            var indexInstruction = new ArrayInstruction([new StringInstruction("asdf")], separator: "[");
            var result = dictionary.GetItem(Runner.Run(""), indexInstruction);
            Assert.That(result, Is.TypeOf<NumericVariable>());
            Assert.That(((NumericVariable)result).Value, Is.EqualTo(4));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var innerDictionary = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "qwer", new NumericVariable(3) },
                { "zxcv", new NumericVariable(5) }
            });

            var dictionary = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "sdfg", new NumericVariable(2) },
                { "asdf", innerDictionary }
            });
            
            var indexInstruction = new ArrayInstruction(
                [new StringInstruction("asdf")],
                Consts.SquareBrackets,
                next: new ArrayInstruction([new StringInstruction("zxcv")], Consts.SquareBrackets));
            var result = dictionary.GetItem(Runner.Run(""), indexInstruction);
            Assert.That(result, Is.TypeOf<NumericVariable>());
            Assert.That(((NumericVariable)result).Value, Is.EqualTo(5));
        }
    }
    
    [TestFixture]
    public class SetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var dictionary = new DictionaryVariable(
                new Dictionary<int, Variable>() { { 1, new NumericVariable(2) } },
                new Dictionary<string, Variable>() { { "asdf", new NumericVariable(4) } });
            var indexInstruction = new ArrayInstruction([new StringInstruction("asdf")], separator: "[");
            var valueVariable = new NumericVariable(10);
            dictionary.SetItem(Runner.Run(""), valueVariable, indexInstruction);
            var result = dictionary.GetItem(Runner.Run(""), indexInstruction);
            Assert.That(result, Is.TypeOf<NumericVariable>());
            Assert.That(((NumericVariable)result).Value, Is.EqualTo(10));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var innerDictionary = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "qwer", new NumericVariable(3) },
                { "zxcv", new NumericVariable(5) }
            });

            var dictionary = new DictionaryVariable(null, new Dictionary<string, Variable>()
            {
                { "sdfg", new NumericVariable(2) },
                { "asdf", innerDictionary }
            });
            
            var indexInstruction = new ArrayInstruction(
                [new StringInstruction("asdf")],
                Consts.SquareBrackets,
                next: new ArrayInstruction([new StringInstruction("zxcv")], Consts.SquareBrackets));
            var valueVariable = new NumericVariable(10);
            dictionary.SetItem(Runner.Run(""), valueVariable, indexInstruction);
            var result = dictionary.GetItem(Runner.Run(""), indexInstruction);
            Assert.That(result, Is.TypeOf<NumericVariable>());
            Assert.That(((NumericVariable)result).Value, Is.EqualTo(10));
        }
    }
}