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
                _memory.CreateBsType(Memory.BsTypes.Bool, true), 
                _memory.CreateBsType(Memory.BsTypes.Bool, false));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesOrOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "or", 
                _memory.CreateBsType(Memory.BsTypes.Bool, true), 
                _memory.CreateBsType(Memory.BsTypes.Bool, false));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesNotOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "not", 
                null, 
                _memory.CreateBsType(Memory.BsTypes.Bool, false));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesTrueIsOperations()
        {
            var value = _memory.CreateBsType(Memory.BsTypes.Int, 5);
            var result = Operator.Operate(
                _memory, 
                "is", 
                value, 
                value);
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "is", 
                _memory.CreateBsType(Memory.BsTypes.Int, 5), 
                _memory.CreateBsType(Memory.BsTypes.Int, 5));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueIsNotOperations()
        {
            var result = Operator.Operate(
                _memory, 
                "is not", 
                _memory.CreateBsType(Memory.BsTypes.Int, 5), 
                _memory.CreateBsType(Memory.BsTypes.Int, 5));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsNotOperations()
        {
            var value = _memory.CreateBsType(Memory.BsTypes.Int, 5);
            var result = Operator.Operate(
                _memory, 
                "is not", 
                value, 
                value);
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
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
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
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
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
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
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
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
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "in", 
                new NumericVariable(1), 
                _memory.CreateBsType(Memory.BsTypes.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "in", 
                new NumericVariable(3), 
                _memory.CreateBsType(Memory.BsTypes.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "not in", 
                new NumericVariable(3), 
                _memory.CreateBsType(Memory.BsTypes.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithList()
        {
            var result = Operator.Operate(
                _memory, 
                "not in", 
                new NumericVariable(1), 
                _memory.CreateBsType(Memory.BsTypes.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                _memory.CreateBsType(Memory.BsTypes.Int, 1),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                _memory.CreateBsType(Memory.BsTypes.Int, 3),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                new StringVariable("asdf"),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "qwer", new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "in",
                new StringVariable("asdf"),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "qwer", new NumericVariable(1) },
                    { "zxcv", new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                _memory.CreateBsType(Memory.BsTypes.Int, 3),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithIntAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                _memory.CreateBsType(Memory.BsTypes.Int, 1),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                new StringVariable("zxcv"),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "qwer", new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithStringAndDictionary()
        {
            var result = Operator.Operate(
                _memory,
                "not in",
                new StringVariable("asdf"),
                _memory.CreateBsType(Memory.BsTypes.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "zxcv", new NumericVariable(2) }
                })));
            var expected = _memory.CreateBsType(Memory.BsTypes.Bool, false);
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
            Assertions.AssertVariablesEqual(result, memory.CreateBsType(Memory.BsTypes.Int, 5));
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
            Assertions.AssertVariablesEqual(result, memory.CreateBsType(Memory.BsTypes.Int, 5));
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
            Assertions.AssertVariablesEqual(result, memory.CreateBsType(Memory.BsTypes.Int, 5));
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
            Assertions.AssertVariablesEqual(result, _memory.CreateBsType(Memory.BsTypes.Int, 8));
        }
        
        [Test]
        public void ConductsOperationOfTruncatedOperatorIfNotStandardAssignmentOperator()
        {
            var result = Operator.Assign(
                _memory, 
                "+=", 
                new NumericInstruction(8), 
                new NumericInstruction(5));
            Assertions.AssertVariablesEqual(result, _memory.CreateBsType(Memory.BsTypes.Int, 13));
        }
    }
}