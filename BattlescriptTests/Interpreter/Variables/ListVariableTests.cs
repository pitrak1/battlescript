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
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false)
            ]);
            var index = new IntegerInstruction(1);
            var result = listVariable.GetItem(new Memory(), new ArrayInstruction([index], separator: "["));
            
            Assert.That(result, Is.TypeOf<StringVariable>());
            Assert.That(((StringVariable)result).Value, Is.EqualTo("a"));
        }
        
        [Test]
        public void HandlesRangeIndexWithBothEnds()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([new IntegerVariable(1), new IntegerVariable(2)]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new StringVariable("a"),
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithoutStartIndex()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([null, new IntegerVariable(2)]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithoutEndIndex()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([new IntegerVariable(1), null]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithoutEitherIndex()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([null, null]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithPositiveStep()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([new IntegerVariable(1), null, new IntegerVariable(2)]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new StringVariable("a"),
                new IntegerVariable(8)
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStep()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([new IntegerVariable(3), new IntegerVariable(0), new IntegerVariable(-2)]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new IntegerVariable(8),
                new StringVariable("a"),
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStepAndNoStart()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([null, new IntegerVariable(1), new IntegerVariable(-1)]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new IntegerVariable(8),
                new ConstantVariable(false),
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStepAndNoEnd()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([new IntegerVariable(2), null, new IntegerVariable(-1)]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new ConstantVariable(false),
                new StringVariable("a"),
                new IntegerVariable(5),
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesRangeIndexWithNegativeStepAndNoStartOrEnd()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new ListVariable([null, null, new IntegerVariable(-1)]);
            var result = listVariable.GetRangeIndex(index);
            var expected = new ListVariable([
                new IntegerVariable(8),
                new ConstantVariable(false),
                new StringVariable("a"),
                new IntegerVariable(5),
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesHandlesStackedSingleIndices()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ListVariable([new IntegerVariable(6), new StringVariable("b")]),
                new ConstantVariable(false),
                new IntegerVariable(8)
            ]);
            var index1 = new IntegerInstruction(2);
            var index2 = new IntegerInstruction(1);
            var indexInstruction = new ArrayInstruction(
                [index1], 
                Consts.SquareBrackets,
                next: new ArrayInstruction([index2], Consts.SquareBrackets));
            var result = listVariable.GetItem(new Memory(), indexInstruction);
            var expected = new StringVariable("b");
            Assert.That(result, Is.EqualTo(expected));
        }
        
        // [Test]
        // public void HandlesHandlesStackedRangeIndices()
        // {
        //     var listVariable = new ListVariable([
        //         new IntegerVariable(5),
        //         new StringVariable("a"),
        //         new ConstantVariable(false),
        //         new IntegerVariable(8)
        //     ]);
        //     var index1 = new KeyValuePairInstruction(new IntegerInstruction(1), null);
        //     var index2 = new KeyValuePairInstruction(null, new IntegerInstruction(1));
        //     var indexInstruction = new SquareBracketsInstruction([index1], new SquareBracketsInstruction([index2]));
        //     var result = listVariable.GetItem(new Memory(), indexInstruction);
        //     var expected = new ListVariable([
        //         new StringVariable("a"),
        //         new ConstantVariable(false)
        //     ]);
        //     Assert.That(result, Is.EqualTo(expected));
        // }
    }
    
    [TestFixture]
    public class SetItem
    {
        [Test]
        public void HandlesIndex()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ConstantVariable(false)
            ]);
            var index = new IntegerInstruction(1);
            var value = new StringVariable("b");
            listVariable.SetItem(new Memory(), value, new ArrayInstruction([index], separator: "["));
            
            Assert.That(listVariable.Values[1], Is.TypeOf<StringVariable>());
            Assert.That(((StringVariable)listVariable.Values[1]).Value, Is.EqualTo("b"));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new ListVariable([
                    new IntegerVariable(6),
                    new StringVariable("b")
                ]),
                new ConstantVariable(false)
            ]);
            var index1 = new IntegerInstruction(2);
            var index2 = new IntegerInstruction(1);
            var value = new StringVariable("c");
            var indexInstruction = new ArrayInstruction(
                [index1],
                Consts.SquareBrackets,
                next: new ArrayInstruction([index2], Consts.SquareBrackets));
            listVariable.SetItem(new Memory(), value, indexInstruction);
            
            Assert.That(listVariable.Values[2], Is.TypeOf<ListVariable>());
            var innerListVariable = (ListVariable)listVariable.Values[2];
            Assert.That(innerListVariable.Values[1], Is.TypeOf<StringVariable>());
            Assert.That(((StringVariable)innerListVariable.Values[1]).Value, Is.EqualTo("c"));
        }
    }

    [TestFixture]
    public class Methods
    {
        [Test]
        public void Append()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a")]);
            listVariable.Append([new ConstantVariable(true)]);
            var expected = new ListVariable([new IntegerVariable(5), new StringVariable("a"), new ConstantVariable(true)]);
            Assert.That(listVariable, Is.EqualTo(expected));
        }
        
        [Test]
        public void Extend()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a")]);
            listVariable.Extend([
                new ListVariable([new ConstantVariable(true), new StringVariable("b")])
            ]);
            var expected = new ListVariable([
                new IntegerVariable(5), 
                new StringVariable("a"), 
                new ConstantVariable(true),
                new StringVariable("b")]);
            Assert.That(listVariable, Is.EqualTo(expected));
        }
        
        [Test]
        public void InsertAtStart()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a")]);
            listVariable.Insert([new IntegerVariable(0), new StringVariable("b")]);
            var expected = new ListVariable([
                new StringVariable("b"),
                new IntegerVariable(5), 
                new StringVariable("a")]);
            Assert.That(listVariable, Is.EqualTo(expected));
        }
        
        [Test]
        public void InsertAtEnd()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a")]);
            listVariable.Insert([new IntegerVariable(2), new StringVariable("b")]);
            var expected = new ListVariable([
                new IntegerVariable(5), 
                new StringVariable("a"),
                new StringVariable("b")]);
            Assert.That(listVariable, Is.EqualTo(expected));
        }

        [Test]
        public void Remove()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a")]);
            listVariable.Remove([new StringVariable("a")]);
            var expected = new ListVariable([new IntegerVariable(5)]);
            Assert.That(listVariable, Is.EqualTo(expected));
        }

        [Test]
        public void PopInMiddle()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a"), new StringVariable("b")]);
            var result = listVariable.Pop([new IntegerVariable(1)]);
            var expected = new ListVariable([new IntegerVariable(5), new StringVariable("b")]);
            Assert.That(listVariable, Is.EqualTo(expected));
            Assert.That(result, Is.EqualTo(new StringVariable("a")));
        }
        
        [Test]
        public void PopAtEnd()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a"), new StringVariable("b")]);
            var result = listVariable.Pop([]);
            var expected = new ListVariable([new IntegerVariable(5), new StringVariable("a")]);
            Assert.That(listVariable, Is.EqualTo(expected));
            Assert.That(result, Is.EqualTo(new StringVariable("b")));
        }
        
        [Test]
        public void Clear()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a"), new StringVariable("b")]);
            listVariable.Clear([]);
            var expected = new ListVariable();
            Assert.That(listVariable, Is.EqualTo(expected));
        }
        
        [Test]
        public void Count()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("b"), new StringVariable("b")]);
            var result = listVariable.Count([new StringVariable("b")]);
            var expected = new IntegerVariable(2);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void Reverse()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a"), new StringVariable("b")]);
            listVariable.Reverse([]);
            var expected = new ListVariable([new StringVariable("b"), new StringVariable("a"), new IntegerVariable(5)]);
            Assert.That(listVariable, Is.EqualTo(expected));
        }
        
        [Test]
        public void Copy()
        {
            var listVariable = new ListVariable([new IntegerVariable(5), new StringVariable("a"), new StringVariable("b")]);
            var copy = listVariable.Copy([]);
            Assert.That(listVariable, Is.EqualTo(copy));
        }
    }
}