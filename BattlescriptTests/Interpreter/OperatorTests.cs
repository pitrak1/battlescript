using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class OperatorTests
{
    [TestFixture]
    public class CommonOperations
    {
        [Test]
        public void HandlesAndOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "and", 
                BsTypes.Create(BsTypes.Types.Bool, true), 
                BsTypes.Create(BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesOrOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "or", 
                BsTypes.Create(BsTypes.Types.Bool, true), 
                BsTypes.Create(BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesNotOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "not", 
                null, 
                BsTypes.Create(BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesTrueIsOperations()
        {
            var memory = Runner.Run("");
            var value = BsTypes.Create(BsTypes.Types.Int, 5);
            var result = Operator.Operate(
                memory, 
                "is", 
                value, 
                value);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "is", 
                BsTypes.Create(BsTypes.Types.Int, 5), 
                BsTypes.Create(BsTypes.Types.Int, 5));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueIsNotOperations()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "is not", 
                BsTypes.Create(BsTypes.Types.Int, 5), 
                BsTypes.Create(BsTypes.Types.Int, 5));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsNotOperations()
        {
            var memory = Runner.Run("");
            var value = BsTypes.Create(BsTypes.Types.Int, 5);
            var result = Operator.Operate(
                memory, 
                "is not", 
                value, 
                value);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithString()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "in",
                BsTypes.Create(BsTypes.Types.String, new StringVariable("sd")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithString()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "in", 
                BsTypes.Create(BsTypes.Types.String, new StringVariable("fa")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithString()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "not in", 
                BsTypes.Create(BsTypes.Types.String, new StringVariable("fa")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithString()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "not in", 
                BsTypes.Create(BsTypes.Types.String, new StringVariable("sd")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithList()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "in", 
                new NumericVariable(1), 
                BsTypes.Create(BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithList()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "in", 
                new NumericVariable(3), 
                BsTypes.Create(BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithList()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "not in", 
                new NumericVariable(3), 
                BsTypes.Create(BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithList()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory, 
                "not in", 
                new NumericVariable(1), 
                BsTypes.Create(BsTypes.Types.List, new SequenceVariable([
                    new StringVariable("asdf"),
                    new NumericVariable(1),
                    new NumericVariable(2)])));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithIntAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "in",
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithIntAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "in",
                BsTypes.Create(BsTypes.Types.Int, 3),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithStringAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "in",
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "qwer", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithStringAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "in",
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "qwer", new NumericVariable(1) },
                    { "zxcv", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithIntAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "not in",
                BsTypes.Create(BsTypes.Types.Int, 3),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithIntAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "not in",
                BsTypes.Create(BsTypes.Types.Int, 1),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(new Dictionary<int, Variable>()
                {
                    { 1, new NumericVariable(1) },
                    { 2, new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithStringAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "not in",
                BsTypes.Create(BsTypes.Types.String, new StringVariable("zxcv")),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "qwer", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithStringAndDictionary()
        {
            var memory = Runner.Run("");
            var result = Operator.Operate(
                memory,
                "not in",
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")),
                BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, new Dictionary<string, Variable>()
                {
                    { "asdf", new NumericVariable(1) },
                    { "zxcv", new NumericVariable(2) }
                })));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
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
            Assertions.AssertVariablesEqual(result, BsTypes.Create(BsTypes.Types.Int, 5));
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
            Assertions.AssertVariablesEqual(result, BsTypes.Create(BsTypes.Types.Int, 5));
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
            Assertions.AssertVariablesEqual(result, BsTypes.Create(BsTypes.Types.Int, 5));
        }
        
        [Test]
        public void UsesReversedMethodIfNonCommutativeOperatorAndRightHasOverride()
        {
            var memory = Runner.Run("""
                                    class asdf:
                                        def __sub__(self, other):
                                            return 5
                                            
                                        def __rsub__(self, other):
                                            return 10
                                            
                                    x = asdf()
                                    """);
            var x = memory.GetVariable("x");
            var result = Operator.Operate(
                memory,
                "-",
                new StringVariable("asdf"),
                x);
            Assertions.AssertVariablesEqual(result, BsTypes.Create(BsTypes.Types.Int, 10));
        }
    }

    [TestFixture]
    public class AssignmentOperations
    {
        [Test]
        public void HandlesStandardAssignmentOperator()
        {
            var memory = Runner.Run("");
            Operator.Assign(memory, "=", new VariableInstruction("x"), new NumericInstruction(8));
            Assertions.AssertVariable(memory, "x", BsTypes.Create(BsTypes.Types.Int, 8));
        }
        
        [Test]
        public void HandlesNonStandardAssignmentOperator()
        {
            var memory = Runner.Run("x = 6");
            Operator.Assign(
                memory, 
                "+=", 
                new VariableInstruction("x"), 
                new NumericInstruction(5));
            Assertions.AssertVariable(memory, "x", BsTypes.Create(BsTypes.Types.Int, 11));
        }
    }

    [TestFixture]
    public class NumericOperations
    {
         [Test]
         public void HandlesNumericOperators()
         {
             var memory = Runner.Run("");
             var result = Operator.Operate(memory, "+", new NumericVariable(5), new NumericVariable(10));
             Assertions.AssertVariablesEqual(result, new NumericVariable(15));
         }
         
         [Test]
         public void HandlesComparisonOperators()
         {
             var memory = Runner.Run("");
             var result = Operator.Operate(memory, ">=", new NumericVariable(5), new NumericVariable(2));
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }
         
         [Test]
         public void HandlesUnaryNumericOperators()
         {
             var memory = Runner.Run("");
             var result = Operator.Operate(memory, "-", null, new NumericVariable(5));
             Assertions.AssertVariablesEqual(result, new NumericVariable(-5));
         }
    }

    [TestFixture]
    public class SequenceOperations
    {
         [Test]
         public void HandlesAdditionOperator()
         {
             var memory = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1), 
                 new NumericVariable(2), 
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(4),
                 new NumericVariable(5),
                 new NumericVariable(6)
             ]);
             var result = Operator.Operate(memory, "+", seq1, seq2);
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
             var memory = Runner.Run("");
             var seq = new SequenceVariable([
                 new NumericVariable(1), 
                 new NumericVariable(2), 
                 new NumericVariable(3)]);
             var result = Operator.Operate(memory, "*", seq, new NumericVariable(3));
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
             var memory = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1), 
                 new NumericVariable(2), 
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1), 
                 new NumericVariable(2), 
                 new NumericVariable(3)]);
             var result = Operator.Operate(memory, "==", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }
         
         [Test]
         public void HandlesFalseEqualityOperator()
         {
             var memory = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1), 
                 new NumericVariable(2), 
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1), 
                 new NumericVariable(2), 
                 new NumericVariable(4)]);
             var result = Operator.Operate(memory, "==", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(0));
         }
    }
    
    [TestFixture]
    public class StringOperations
    {
         [Test]
         public void HandlesAdditionOperator()
         {
             var memory = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("qwer");
             var result = Operator.Operate(memory, "+", string1, string2);
             Assertions.AssertVariablesEqual(result, new StringVariable("asdfqwer"));
         }
         
         [Test]
         public void HandlesMultiplyOperator()
         {
             var memory = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var result = Operator.Operate(memory, "*", string1, new NumericVariable(3));
             Assertions.AssertVariablesEqual(result, new StringVariable("asdfasdfasdf"));
         }
         
         [Test]
         public void HandlesTrueEqualityOperator()
         {
             var memory = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("asdf");
             var result = Operator.Operate(memory, "==", string1, string2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }
         
         [Test]
         public void HandlesFalseEqualityOperator()
         {
             var memory = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("qwer");
             var result = Operator.Operate(memory, "==", string1, string2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(0));
         }

         [Test]
         public void ConvertsNumericToStringForAddition()
         {
             var memory = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var numeric = new NumericVariable(6);
             var result = Operator.Operate(memory, "+", string1, numeric);
             Assertions.AssertVariablesEqual(result, new StringVariable("asdf6"));
         }
    }
}