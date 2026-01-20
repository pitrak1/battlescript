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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "and", 
                BsTypes.Create(BsTypes.Types.Bool, true), 
                BsTypes.Create(BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesOrOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "or", 
                BsTypes.Create(BsTypes.Types.Bool, true), 
                BsTypes.Create(BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesNotOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "not", 
                null, 
                BsTypes.Create(BsTypes.Types.Bool, false));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesTrueIsOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var value = BsTypes.Create(BsTypes.Types.Int, 5);
            var result = Operator.Operate(
                callStack, closure, 
                "is", 
                value, 
                value);
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "is", 
                BsTypes.Create(BsTypes.Types.Int, 5), 
                BsTypes.Create(BsTypes.Types.Int, 5));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueIsNotOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "is not", 
                BsTypes.Create(BsTypes.Types.Int, 5), 
                BsTypes.Create(BsTypes.Types.Int, 5));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }

        [Test]
        public void HandlesFalseIsNotOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var value = BsTypes.Create(BsTypes.Types.Int, 5);
            var result = Operator.Operate(
                callStack, closure, 
                "is not", 
                value, 
                value);
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithString()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
                "in",
                BsTypes.Create(BsTypes.Types.String, new StringVariable("sd")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseInOperationWithString()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "in", 
                BsTypes.Create(BsTypes.Types.String, new StringVariable("fa")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueNotInOperationWithString()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "not in", 
                BsTypes.Create(BsTypes.Types.String, new StringVariable("fa")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, true);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesFalseNotInOperationWithString()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
                "not in", 
                BsTypes.Create(BsTypes.Types.String, new StringVariable("sd")),
                BsTypes.Create(BsTypes.Types.String, new StringVariable("asdf")));
            var expected = BsTypes.Create(BsTypes.Types.Bool, false);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesTrueInOperationWithList()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure, 
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("");
            var result = Operator.Operate(
                callStack, closure,
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
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        def __add__(self, other):
                                            return 5

                                    x = asdf()
                                    """);
            var x = closure.GetVariable(callStack, "x");
            var result = Operator.Operate(
                callStack, closure,
                "+",
                x,
                new StringVariable("asdf"));
            Assertions.AssertVariablesEqual(result, BsTypes.Create(BsTypes.Types.Int, 5));
        }

        [Test]
        public void HandlesObjectOperationsIfOverrideIsPresentOnRightObject()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        def __add__(self, other):
                                            return 5

                                    x = asdf()
                                    """);
            var x = closure.GetVariable(callStack, "x");
            var result = Operator.Operate(
                callStack, closure,
                "+",
                new StringVariable("asdf"),
                x);
            Assertions.AssertVariablesEqual(result, BsTypes.Create(BsTypes.Types.Int, 5));
        }

        [Test]
        public void HandlesUnaryObjectOperationsIfOverrideIsPresent()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        def __neg__(self):
                                            return 5

                                    x = asdf()
                                    """);
            var x = closure.GetVariable(callStack, "x");
            var result = Operator.Operate(
                callStack, closure,
                "-",
                null,
                x);
            Assertions.AssertVariablesEqual(result, BsTypes.Create(BsTypes.Types.Int, 5));
        }

        [Test]
        public void UsesReversedMethodIfNonCommutativeOperatorAndRightHasOverride()
        {
            var (callStack, closure) = Runner.Run("""
                                    class asdf:
                                        def __sub__(self, other):
                                            return 5

                                        def __rsub__(self, other):
                                            return 10

                                    x = asdf()
                                    """);
            var x = closure.GetVariable(callStack, "x");
            var result = Operator.Operate(
                callStack,
                closure,
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
            var (callStack, closure) = Runner.Run("");
            Operator.Assign(callStack, closure, "=", new VariableInstruction("x"), new NumericInstruction(8));
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 8));
        }

        [Test]
        public void HandlesNonStandardAssignmentOperator()
        {
            var (callStack, closure) = Runner.Run("x = 6");
            Operator.Assign(
                callStack, closure,
                "+=",
                new VariableInstruction("x"),
                new NumericInstruction(5));
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 11));
        }

        [Test]
        public void HandlesTrueDivisionAssignmentConvertsIntToFloat()
        {
            var (callStack, closure) = Runner.Run("x = 10");
            Operator.Assign(
                callStack, closure,
                "/=",
                new VariableInstruction("x"),
                new NumericInstruction(3));
            var result = closure.GetVariable(callStack, "x");
            Assert.That(BsTypes.Is(BsTypes.Types.Float, result), Is.True);
            var floatValue = BsTypes.GetFloatValue(result);
            Assert.That(floatValue, Is.EqualTo(10.0 / 3.0).Within(0.0001));
        }

        [Test]
        public void HandlesFloorDivisionAssignmentConvertsFloatToInt()
        {
            var (callStack, closure) = Runner.Run("x = 10.5");
            Operator.Assign(
                callStack, closure,
                "//=",
                new VariableInstruction("x"),
                new NumericInstruction(3));
            var result = closure.GetVariable(callStack, "x");
            Assert.That(BsTypes.Is(BsTypes.Types.Int, result), Is.True);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 3));
        }
    }

    [TestFixture]
    public class NumericOperations
    {
         [Test]
         public void HandlesNumericOperators()
         {
             var (callStack, closure) = Runner.Run("");
             var result = Operator.Operate(callStack, closure, "+", new NumericVariable(5), new NumericVariable(10));
             Assertions.AssertVariablesEqual(result, new NumericVariable(15));
         }

         [Test]
         public void HandlesComparisonOperators()
         {
             var (callStack, closure) = Runner.Run("");
             var result = Operator.Operate(callStack, closure, ">=", new NumericVariable(5), new NumericVariable(2));
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }

         [Test]
         public void HandlesUnaryNumericOperators()
         {
             var (callStack, closure) = Runner.Run("");
             var result = Operator.Operate(callStack, closure, "-", null, new NumericVariable(5));
             Assertions.AssertVariablesEqual(result, new NumericVariable(-5));
         }
    }

    [TestFixture]
    public class SequenceOperations
    {
         [Test]
         public void HandlesAdditionOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(4),
                 new NumericVariable(5),
                 new NumericVariable(6)
             ]);
             var result = Operator.Operate(callStack, closure, "+", seq1, seq2);
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
             var (callStack, closure) = Runner.Run("");
             var seq = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var result = Operator.Operate(callStack, closure, "*", seq, new NumericVariable(3));
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
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var result = Operator.Operate(callStack, closure, "==", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }

         [Test]
         public void HandlesFalseEqualityOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(4)]);
             var result = Operator.Operate(callStack, closure, "==", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(0));
         }

         [Test]
         public void HandlesTrueInequalityOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(4)]);
             var result = Operator.Operate(callStack, closure, "!=", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }

         [Test]
         public void HandlesFalseInequalityOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var result = Operator.Operate(callStack, closure, "!=", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(0));
         }

         [Test]
         public void HandlesEqualityWithNullElements()
         {
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1),
                 null,
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1),
                 null,
                 new NumericVariable(3)]);
             var result = Operator.Operate(callStack, closure, "==", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }

         [Test]
         public void HandlesInequalityWithOneNull()
         {
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 new NumericVariable(1),
                 null,
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var result = Operator.Operate(callStack, closure, "==", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(0));
         }

         [Test]
         public void HandlesInequalityWithNullsInDifferentPositions()
         {
             var (callStack, closure) = Runner.Run("");
             var seq1 = new SequenceVariable([
                 null,
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var seq2 = new SequenceVariable([
                 new NumericVariable(1),
                 null,
                 new NumericVariable(3)]);
             var result = Operator.Operate(callStack, closure, "==", seq1, seq2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(0));
         }
    }
    
    [TestFixture]
    public class StringOperations
    {
         [Test]
         public void HandlesAdditionOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("qwer");
             var result = Operator.Operate(callStack, closure, "+", string1, string2);
             Assertions.AssertVariablesEqual(result, new StringVariable("asdfqwer"));
         }

         [Test]
         public void HandlesMultiplyOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var result = Operator.Operate(callStack, closure, "*", string1, new NumericVariable(3));
             Assertions.AssertVariablesEqual(result, new StringVariable("asdfasdfasdf"));
         }

         [Test]
         public void HandlesTrueEqualityOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("asdf");
             var result = Operator.Operate(callStack, closure, "==", string1, string2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(1));
         }

         [Test]
         public void HandlesFalseEqualityOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("qwer");
             var result = Operator.Operate(callStack, closure, "==", string1, string2);
             Assertions.AssertVariablesEqual(result, new NumericVariable(0));
         }

         [Test]
         public void ConvertsNumericToStringForAddition()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var numeric = new NumericVariable(6);
             var result = Operator.Operate(callStack, closure, "+", string1, numeric);
             Assertions.AssertVariablesEqual(result, new StringVariable("asdf6"));
         }

         [Test]
         public void ConvertsNumericToStringForAdditionReversed()
         {
             var (callStack, closure) = Runner.Run("");
             var numeric = new NumericVariable(5);
             var string1 = new StringVariable("asdf");
             var result = Operator.Operate(callStack, closure, "+", numeric, string1);
             Assertions.AssertVariablesEqual(result, new StringVariable("5asdf"));
         }
    }
}