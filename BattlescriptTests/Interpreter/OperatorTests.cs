using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class OperatorTests
{
    [TestFixture]
    public class BooleanOperate
    {
        [Test]
        public void AndOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "and",
                new ConstantInstruction("True"),
                new ConstantInstruction("False"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void OrOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "or",
                new ConstantInstruction("True"),
                new ConstantInstruction("False"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void NotOperations()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "not",
                null,
                new ConstantInstruction("False"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TrueIsOperationWithSameReference()
        {
            var (callStack, closure) = Runner.Run("");
            var value = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([BtlTypes.Create(BtlTypes.Types.Int, 5)]));
            closure.SetVariable(callStack, new VariableInstruction("x"), value);
            var result = Operator.BooleanOperate(
                callStack, closure,
                "is",
                new VariableInstruction("x"),
                new VariableInstruction("x"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FalseIsOperationWithDifferentReferences()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "is",
                Runner.Parse("[5]")[0],
                Runner.Parse("[5]")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TrueIsNotOperationWithDifferentReferences()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "is not",
                Runner.Parse("[5]")[0],
                Runner.Parse("[5]")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FalseIsNotOperationWithSameReference()
        {
            var (callStack, closure) = Runner.Run("");
            var value = BtlTypes.Create(BtlTypes.Types.List, new SequenceVariable([BtlTypes.Create(BtlTypes.Types.Int, 5)]));
            closure.SetVariable(callStack, new VariableInstruction("x"), value);
            var result = Operator.BooleanOperate(
                callStack, closure,
                "is not",
                new VariableInstruction("x"),
                new VariableInstruction("x"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void TrueInOperationWithSubstringInString()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "in",
                new StringInstruction("sd"),
                new StringInstruction("asdf"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FalseInOperationWithMissingSubstring()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "in",
                new StringInstruction("fa"),
                new StringInstruction("asdf"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void TrueNotInOperationWithMissingSubstring()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "not in",
                new StringInstruction("fa"),
                new StringInstruction("asdf"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test]
        public void FalseNotInOperationWithSubstringInString()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "not in",
                new StringInstruction("sd"),
                new StringInstruction("asdf"));
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void TrueInOperationWithList()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "in",
                new NumericInstruction(1),
                Runner.Parse("['asdf', 1, 2]")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void FalseInOperationWithList()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure, 
                "in", 
                new NumericInstruction(3), 
                Runner.Parse("['asdf', 1, 2]")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void TrueNotInOperationWithList()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure, 
                "not in", 
                new NumericInstruction(3), 
                Runner.Parse("['asdf', 1, 2]")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void FalseNotInOperationWithList()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure, 
                "not in", 
                new NumericInstruction(1), 
                Runner.Parse("['asdf', 1, 2]")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void TrueInOperationWithIntAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "in",
                new NumericInstruction(1),
                Runner.Parse("{1: 1, 2: 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void FalseInOperationWithIntAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "in",
                new NumericInstruction(3),
                Runner.Parse("{1: 1, 2: 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void TrueInOperationWithStringAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "in",
                new StringInstruction("asdf"),
                Runner.Parse("{'asdf': 1, 'qwer': 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void FalseInOperationWithStringAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "in",
                new StringInstruction("asdf"),
                Runner.Parse("{'qwer': 1, 'zxcv': 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void TrueNotInOperationWithIntAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "not in",
                new NumericInstruction(3),
                Runner.Parse("{1: 1, 2: 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void FalseNotInOperationWithIntAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "not in",
                new NumericInstruction(1),
                Runner.Parse("{1: 1, 2: 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void TrueNotInOperationWithStringAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "not in",
                new StringInstruction("zxcv"),
                Runner.Parse("{'asdf': 1, 'qwer': 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, true);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void FalseNotInOperationWithStringAndDictionary()
        {
            var (callStack, closure) = Runner.Run("");
            var result = Operator.BooleanOperate(
                callStack, closure,
                "not in",
                new StringInstruction("asdf"),
                Runner.Parse("{'asdf': 1, 'zxcv': 2}")[0]);
            var expected = BtlTypes.Create(BtlTypes.Types.Bool, false);
            Assert.That(result, Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class ObjectOperations
    {
        [Test]
        public void ObjectOperationsWithOverrideOnLeftObject()
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
            Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 5)));
        }

        [Test]
        public void ObjectOperationsWithOverrideOnRightObject()
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
            Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 5)));
        }

        [Test]
        public void UnaryObjectOperationsWithOverride()
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
            Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 5)));
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
            Assert.That(result, Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 10)));
        }
    }

    [TestFixture]
    public class AssignmentOperations
    {
        [Test]
        public void StandardAssignmentOperator()
        {
            var (callStack, closure) = Runner.Run("");
            Operator.Assign(callStack, closure, "=", new VariableInstruction("x"), new NumericInstruction(8));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 8)));
        }

        [Test]
        public void NonStandardAssignmentOperator()
        {
            var (callStack, closure) = Runner.Run("x = 6");
            Operator.Assign(
                callStack, closure,
                "+=",
                new VariableInstruction("x"),
                new NumericInstruction(5));
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 11)));
        }

        [Test]
        public void TrueDivisionAssignmentConvertsIntToFloat()
        {
            var (callStack, closure) = Runner.Run("x = 10");
            Operator.Assign(
                callStack, closure,
                "/=",
                new VariableInstruction("x"),
                new NumericInstruction(3));
            var result = closure.GetVariable(callStack, "x");
            Assert.That(BtlTypes.Is(BtlTypes.Types.Float, result), Is.True);
            var floatValue = BtlTypes.GetFloatValue(result);
            Assert.That(floatValue, Is.EqualTo(10.0 / 3.0).Within(0.0001));
        }

        [Test]
        public void FloorDivisionAssignmentConvertsFloatToInt()
        {
            var (callStack, closure) = Runner.Run("x = 10.5");
            Operator.Assign(
                callStack, closure,
                "//=",
                new VariableInstruction("x"),
                new NumericInstruction(3));
            var result = closure.GetVariable(callStack, "x");
            Assert.That(BtlTypes.Is(BtlTypes.Types.Int, result), Is.True);
            Assert.That(closure.GetVariable(callStack, "x"), Is.EqualTo(BtlTypes.Create(BtlTypes.Types.Int, 3)));
        }
    }

    [TestFixture]
    public class NumericOperations
    {
         [Test]
         public void NumericOperators()
         {
             var (callStack, closure) = Runner.Run("");
             var result = Operator.Operate(callStack, closure, "+", new NumericVariable(5), new NumericVariable(10));
             Assert.That(result, Is.EqualTo(new NumericVariable(15)));
         }

         [Test]
         public void ComparisonOperators()
         {
             var (callStack, closure) = Runner.Run("");
             var result = Operator.Operate(callStack, closure, ">=", new NumericVariable(5), new NumericVariable(2));
             Assert.That(result, Is.EqualTo(new NumericVariable(1)));
         }

         [Test]
         public void UnaryNumericOperators()
         {
             var (callStack, closure) = Runner.Run("");
             var result = Operator.Operate(callStack, closure, "-", null, new NumericVariable(5));
             Assert.That(result, Is.EqualTo(new NumericVariable(-5)));
         }
    }

    [TestFixture]
    public class SequenceOperations
    {
         [Test]
         public void AdditionOperator()
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
             Assert.That(result, Is.EqualTo(new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3),
                 new NumericVariable(4),
                 new NumericVariable(5),
                 new NumericVariable(6)])));
         }

         [Test]
         public void MultiplyOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var seq = new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)]);
             var result = Operator.Operate(callStack, closure, "*", seq, new NumericVariable(3));
             Assert.That(result, Is.EqualTo(new SequenceVariable([
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3),
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3),
                 new NumericVariable(1),
                 new NumericVariable(2),
                 new NumericVariable(3)])));
         }

         [Test]
         public void TrueEqualityOperator()
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
             Assert.That(result, Is.EqualTo(new NumericVariable(1)));
         }

         [Test]
         public void FalseEqualityOperator()
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
             Assert.That(result, Is.EqualTo(new NumericVariable(0)));
         }

         [Test]
         public void TrueInequalityOperator()
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
             Assert.That(result, Is.EqualTo(new NumericVariable(1)));
         }

         [Test]
         public void FalseInequalityOperator()
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
             Assert.That(result, Is.EqualTo(new NumericVariable(0)));
         }

         [Test]
         public void EqualityWithNullElements()
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
             Assert.That(result, Is.EqualTo(new NumericVariable(1)));
         }

         [Test]
         public void InequalityWithOneNull()
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
             Assert.That(result, Is.EqualTo(new NumericVariable(0)));
         }

         [Test]
         public void InequalityWithNullsInDifferentPositions()
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
             Assert.That(result, Is.EqualTo(new NumericVariable(0)));
         }
    }
    
    [TestFixture]
    public class StringOperations
    {
         [Test]
         public void AdditionOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("qwer");
             var result = Operator.Operate(callStack, closure, "+", string1, string2);
             Assert.That(result, Is.EqualTo(new StringVariable("asdfqwer")));
         }

         [Test]
         public void MultiplyOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var result = Operator.Operate(callStack, closure, "*", string1, new NumericVariable(3));
             Assert.That(result, Is.EqualTo(new StringVariable("asdfasdfasdf")));
         }

         [Test]
         public void TrueEqualityOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("asdf");
             var result = Operator.Operate(callStack, closure, "==", string1, string2);
             Assert.That(result, Is.EqualTo(new NumericVariable(1)));
         }

         [Test]
         public void FalseEqualityOperator()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var string2 = new StringVariable("qwer");
             var result = Operator.Operate(callStack, closure, "==", string1, string2);
             Assert.That(result, Is.EqualTo(new NumericVariable(0)));
         }

         [Test]
         public void ConvertsNumericToStringForAddition()
         {
             var (callStack, closure) = Runner.Run("");
             var string1 = new StringVariable("asdf");
             var numeric = new NumericVariable(6);
             var result = Operator.Operate(callStack, closure, "+", string1, numeric);
             Assert.That(result, Is.EqualTo(new StringVariable("asdf6")));
         }

         [Test]
         public void ConvertsNumericToStringForAdditionReversed()
         {
             var (callStack, closure) = Runner.Run("");
             var numeric = new NumericVariable(5);
             var string1 = new StringVariable("asdf");
             var result = Operator.Operate(callStack, closure, "+", numeric, string1);
             Assert.That(result, Is.EqualTo(new StringVariable("5asdf")));
         }
    }
}