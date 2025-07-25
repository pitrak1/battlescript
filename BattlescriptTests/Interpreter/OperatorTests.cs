using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class OperatorTests
{
    [TestFixture]
    public class CommonOperations
    {
        private Memory _memory;
        
        [SetUp]
        public void SetUp()
        {
            _memory = Runner.Run("");
        }
        
        [Test]
        public void HandlesAndOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "and", 
                BsTypes.Create(_memory, BsTypes.Types.Bool, true), 
                BsTypes.Create(_memory, BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesOrOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "or", 
                BsTypes.Create(_memory, BsTypes.Types.Bool, true), 
                BsTypes.Create(_memory, BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesNotOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "not", 
                null, 
                BsTypes.Create(_memory, BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesTrueIsOperations()
        {
            var value = BsTypes.Create(_memory, BsTypes.Types.Int, 5);
            var result = Operator.Operate(
                _memory, 
                "is", 
                value, 
                value);
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "is", 
                BsTypes.Create(_memory, BsTypes.Types.Int, 5), 
                BsTypes.Create(_memory, BsTypes.Types.Int, 5));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueIsNotOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "is not", 
                BsTypes.Create(_memory, BsTypes.Types.Int, 5), 
                BsTypes.Create(_memory, BsTypes.Types.Int, 5));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsNotOperations()
        {
            var value = BsTypes.Create(_memory, BsTypes.Types.Int, 5);
            var result = Operator.Operate(
                _memory, 
                "is not", 
                value, 
                value);
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithString()
        {
            var result = Operator.Operate(
                _memory, 
                "in", 
                new StringVariable("sd"), 
                new StringVariable("asdf"));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithString()
        {
            var result = Operator.Operate(
                _memory, 
                "in", 
                new StringVariable("fa"), 
                new StringVariable("asdf"));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithString()
        {
            var result = Operator.Operate(
                _memory, 
                "not in", 
                new StringVariable("fa"), 
                new StringVariable("asdf"));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithString()
        {
            var result = Operator.Operate(
                _memory, 
                "not in", 
                new StringVariable("sd"), 
                new StringVariable("asdf"));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "in", 
                new NumericVariable(1), 
                BsTypes.Create(_memory, BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "in", 
                new NumericVariable(3), 
                BsTypes.Create(_memory, BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "not in", 
                new NumericVariable(3), 
                BsTypes.Create(_memory, BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "not in", 
                new NumericVariable(1), 
                BsTypes.Create(_memory, BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                BsTypes.Create(_memory, BsTypes.Types.Int, 1),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                BsTypes.Create(_memory, BsTypes.Types.Int, 3),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                new StringVariable("asdf"),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "qwer", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                new StringVariable("asdf"),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "qwer", new NumericVariable(1) },
                    { "zxcv", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                BsTypes.Create(_memory, BsTypes.Types.Int, 3),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                BsTypes.Create(_memory, BsTypes.Types.Int, 1),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                new StringVariable("zxcv"),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "qwer", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                new StringVariable("asdf"),
                BsTypes.Create(_memory, BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "zxcv", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(_memory, BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
    }

    [TestFixture]
    public class ObjectOperations
    {
        [Test]
        public void HandlesObjectOperationsIfOverrideIsPresentOnLeftObject()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        def __add__(self, other):
                                            return 5
                                            
                                    x = asdf()
                                    """);
            var x = memory.GetVariable("x");
            var result = Operator.Operate(
                memory,
                "+",
                x,
                new StringVariable("asdf"));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 5));
        }
        
        [Test]
        public void HandlesObjectOperationsIfOverrideIsPresentOnRightObject()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        def __add__(self, other):
                                            return 5
                                            
                                    x = asdf()
                                    """);
            var x = memory.GetVariable("x");
            var result = Operator.Operate(
                memory,
                "+",
                new StringVariable("asdf"),
                x);
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 5));
        }
        
        [Test]
        public void HandlesUnaryObjectOperationsIfOverrideIsPresent()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        def __neg__(self):
                                            return 5
                                            
                                    x = asdf()
                                    """);
            var x = memory.GetVariable("x");
            var result = Operator.Operate(
                memory,
                "-",
                null,
                x);
            Assertions.AssertVariablesEqual(result, BsTypes.Create(memory, BsTypes.Types.Int, 5));
        }
    }

    [TestFixture]
    public class AssignmentOperations
    {
        private Memory _memory;
        
        [SetUp]
        public void SetUp()
        {
            _memory = Runner.Run("");
        }
        
        [Test]
        public void ReturnsRightIfStandardAssignmentOperator()
        {
            var result = Operator.Assign(_memory, "=", null, new NumericInstruction(8));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(_memory, BsTypes.Types.Int, 8));
        }
        
        [Test]
        public void ConductsOperationOfTruncatedOperatorIfNotStandardAssignmentOperator()
        {
            var result = Operator.Assign(
                _memory, 
                "+=", 
                new NumericInstruction(8), 
                new NumericInstruction(5));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(_memory, BsTypes.Types.Int, 13));
        }
    }
}