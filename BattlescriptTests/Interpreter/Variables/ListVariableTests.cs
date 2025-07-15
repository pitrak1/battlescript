using Battlescript;

namespace BattlescriptTests.InterpreterTests.VariablesTests;

[TestFixture]
public static class ListVariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void HandlesSingleIndex()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false)
            ]);
            var index = new NumericInstruction(1);
            var result = listVariable.GetItem(Runner.Run(""), new ArrayInstruction([index], separator: "["));
            Assertions.AssertVariablesEqual(result, new StringVariable("a"));
        }
        
        [Test]
        public void HandlesRangeIndexWithBothEnds()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 1), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 2)]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new StringVariable("a"),
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithoutStartIndex()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([null, BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 2)]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithoutEndIndex()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 1), null]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithoutEitherIndex()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([null, null]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithPositiveStep()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 1), 
                null, 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 2)]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new StringVariable("a"),
                new NumericVariable(8)
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStep()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 3), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 0), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", -2)]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new NumericVariable(8),
                new StringVariable("a"),
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStepAndNoStart()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([
                null, 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 1), 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", -1)]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new NumericVariable(8),
                new ConstantVariable(false),
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStepAndNoEnd()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 2), 
                null, 
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", -1)]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new ConstantVariable(false),
                new StringVariable("a"),
                new NumericVariable(5),
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStepAndNoStartOrEnd()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index = new ListVariable([null, null, BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", -1)]);
            var memory = Runner.Run("");
            var result = listVariable.GetRangeIndex(memory, index);
            var expected = new ListVariable([
                new NumericVariable(8),
                new ConstantVariable(false),
                new StringVariable("a"),
                new NumericVariable(5),
            ]);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void HandlesHandlesStackedSingleIndices()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ListVariable([new NumericVariable(6), new StringVariable("b")]),
                new ConstantVariable(false),
                new NumericVariable(8)
            ]);
            var index1 = new NumericInstruction(2);
            var index2 = new NumericInstruction(1);
            var indexInstruction = new ArrayInstruction(
                [index1], 
                Consts.SquareBrackets,
                next: new ArrayInstruction([index2], Consts.SquareBrackets));
            var result = listVariable.GetItem(Runner.Run(""), indexInstruction);
            var expected = new StringVariable("b");
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        // [Test]
        // public void HandlesHandlesStackedRangeIndices()
        // {
        //     var listVariable = new ListVariable([
        //         new NumericVariable(5),
        //         new StringVariable("a"),
        //         new ConstantVariable(false),
        //         new NumericVariable(8)
        //     ]);
        //     var index1 = new KeyValuePairInstruction(new NumericInstruction(1), null);
        //     var index2 = new KeyValuePairInstruction(null, new NumericInstruction(1));
        //     var indexInstruction = new SquareBracketsInstruction([index1], new SquareBracketsInstruction([index2]));
        //     var result = listVariable.GetItem(Runner.Run(""), indexInstruction);
        //     var expected = new ListVariable([
        //         new StringVariable("a"),
        //         new ConstantVariable(false)
        //     ]);
        //     Assertions.AssertVariablesEqual(result, expected);
        // }
    }
    
    [TestFixture]
    public class SetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false)
            ]);
            var index = new NumericInstruction(1);
            var value = new StringVariable("b");
            listVariable.SetItem(Runner.Run(""), value, new ArrayInstruction([index], separator: "["));
            
            Assertions.AssertVariablesEqual(listVariable.Values[1], value);
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var listVariable = new ListVariable([
                new NumericVariable(5),
                new StringVariable("a"),
                new ListVariable([
                    new NumericVariable(6),
                    new StringVariable("b")
                ]),
                new ConstantVariable(false)
            ]);
            var index1 = new NumericInstruction(2);
            var index2 = new NumericInstruction(1);
            var value = new StringVariable("c");
            var indexInstruction = new ArrayInstruction(
                [index1],
                Consts.SquareBrackets,
                next: new ArrayInstruction([index2], Consts.SquareBrackets));
            listVariable.SetItem(Runner.Run(""), value, indexInstruction);
            
            Assert.That(listVariable.Values[2], Is.TypeOf<ListVariable>());
            var innerListVariable = (ListVariable)listVariable.Values[2];
            Assertions.AssertVariablesEqual(innerListVariable.Values[1], value);
        }
    }

    [TestFixture]
    public class Methods
    {
        [Test]
        public void Append()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a")]);
            listVariable.Append([new ConstantVariable(true)]);
            var expected = new ListVariable([new NumericVariable(5), new StringVariable("a"), new ConstantVariable(true)]);
            Assertions.AssertVariablesEqual(listVariable, expected);
        }
        
        [Test]
        public void Extend()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a")]);
            listVariable.Extend([
                new ListVariable([new ConstantVariable(true), new StringVariable("b")])
            ]);
            var expected = new ListVariable([
                new NumericVariable(5), 
                new StringVariable("a"), 
                new ConstantVariable(true),
                new StringVariable("b")]);
            Assertions.AssertVariablesEqual(listVariable, expected);
        }
        
        [Test]
        public void InsertAtStart()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a")]);
            listVariable.Insert([new NumericVariable(0), new StringVariable("b")]);
            var expected = new ListVariable([
                new StringVariable("b"),
                new NumericVariable(5), 
                new StringVariable("a")]);
            Assertions.AssertVariablesEqual(listVariable, expected);
        }
        
        [Test]
        public void InsertAtEnd()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a")]);
            listVariable.Insert([new NumericVariable(2), new StringVariable("b")]);
            var expected = new ListVariable([
                new NumericVariable(5), 
                new StringVariable("a"),
                new StringVariable("b")]);
            Assertions.AssertVariablesEqual(listVariable, expected);
        }

        [Test]
        public void Remove()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a")]);
            listVariable.Remove([new StringVariable("a")]);
            var expected = new ListVariable([new NumericVariable(5)]);
            Assertions.AssertVariablesEqual(listVariable, expected);
        }

        [Test]
        public void PopInMiddle()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a"), new StringVariable("b")]);
            var result = listVariable.Pop([new NumericVariable(1)]);
            var expected = new ListVariable([new NumericVariable(5), new StringVariable("b")]);
            Assertions.AssertVariablesEqual(listVariable, expected);
            Assertions.AssertVariablesEqual(result, new StringVariable("a"));
        }
        
        [Test]
        public void PopAtEnd()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a"), new StringVariable("b")]);
            var result = listVariable.Pop([]);
            var expected = new ListVariable([new NumericVariable(5), new StringVariable("a")]);
            Assertions.AssertVariablesEqual(listVariable, expected);
            Assertions.AssertVariablesEqual(result, new StringVariable("b"));
        }
        
        [Test]
        public void Clear()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a"), new StringVariable("b")]);
            listVariable.Clear([]);
            var expected = new ListVariable();
            Assertions.AssertVariablesEqual(listVariable, expected);
        }
        
        [Test]
        public void Count()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("b"), new StringVariable("b")]);
            var result = listVariable.Count([new StringVariable("b")]);
            var expected = new NumericVariable(2);
            Assertions.AssertVariablesEqual(result, expected);
        }
        
        [Test]
        public void Reverse()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a"), new StringVariable("b")]);
            listVariable.Reverse([]);
            var expected = new ListVariable([new StringVariable("b"), new StringVariable("a"), new NumericVariable(5)]);
            Assertions.AssertVariablesEqual(listVariable, expected);
        }
        
        [Test]
        public void Copy()
        {
            var listVariable = new ListVariable([new NumericVariable(5), new StringVariable("a"), new StringVariable("b")]);
            var copy = listVariable.Copy([]);
            Assertions.AssertVariablesEqual(listVariable, copy);
        }
    }
}