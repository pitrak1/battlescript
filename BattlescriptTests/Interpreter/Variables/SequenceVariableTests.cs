using Battlescript;

namespace BattlescriptTests.InterpreterTests.Variables;

[TestFixture]
public static class SequenceVariableTests
{
    [TestFixture]
    public class Operate
    {
        private Memory _memory;
        
        [SetUp]
        public void Setup()
        {
            _memory = Runner.Run("");
        }
        
        [Test]
        public void HandlesAdditionOperator()
        {
            var seq = new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3)]);
            var result = seq.Operate(_memory, "+", new SequenceVariable([
                new NumericVariable(4), 
                new NumericVariable(5), 
                new NumericVariable(6)]));
            Assertions.AssertVariablesEqual(result, new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3),
                new NumericVariable(4),
                new NumericVariable(5),
                new NumericVariable(6)]));
        }
        
        [Test]
        public void HandlesMultiplyOperator()
        {
            var seq = new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3)]);
            var result = seq.Operate(_memory, "*", new NumericVariable(3));
            Assertions.AssertVariablesEqual(result, new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3),
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3),
                new NumericVariable(1),
                new NumericVariable(2),
                new NumericVariable(3)]));
        }
        
        [Test]
        public void HandlesTrueEqualityOperator()
        {
            var seq1 = new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3)]);
            var seq2 = new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3)]);
            var result = seq1.Operate(_memory, "==", seq2);
            Assertions.AssertVariablesEqual(result, new NumericVariable(1));
        }
        
        [Test]
        public void HandlesFalseEqualityOperator()
        {
            var seq1 = new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(3)]);
            var seq2 = new SequenceVariable([
                new NumericVariable(1), 
                new NumericVariable(2), 
                new NumericVariable(4)]);
            var result = seq1.Operate(_memory, "==", seq2);
            Assertions.AssertVariablesEqual(result, new NumericVariable(0));
        }
    }
}