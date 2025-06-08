using Battlescript;

namespace BattlescriptTests;

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
                new BooleanVariable(false)
            ]);
            var index = new IntegerInstruction(1);
            var result = listVariable.GetItem(new Memory(), new SquareBracketsInstruction([index]));
            
            Assert.That(result, Is.TypeOf<StringVariable>());
            Assert.That(((StringVariable)result).Value, Is.EqualTo("a"));
        }
        
        [Test]
        public void HandlesHandlesRangeIndexWithBothEnds()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new BooleanVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new KeyValuePairInstruction(new IntegerInstruction(1), new IntegerInstruction(2));
            var result = listVariable.GetItem(new Memory(), new SquareBracketsInstruction([index]));
            var expected = new ListVariable([
                new StringVariable("a"),
                new BooleanVariable(false)
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesHandlesRangeIndexWithoutStartIndex()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new BooleanVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new KeyValuePairInstruction(null, new IntegerInstruction(2));
            var result = listVariable.GetItem(new Memory(), new SquareBracketsInstruction([index]));
            var expected = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new BooleanVariable(false)
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesHandlesRangeIndexWithoutEndIndex()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new BooleanVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new KeyValuePairInstruction(new IntegerInstruction(1), null);
            var result = listVariable.GetItem(new Memory(), new SquareBracketsInstruction([index]));
            var expected = new ListVariable([
                new StringVariable("a"),
                new BooleanVariable(false),
                new IntegerVariable(8)
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesHandlesRangeIndexWithoutEitherIndex()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new BooleanVariable(false),
                new IntegerVariable(8)
            ]);
            var index = new KeyValuePairInstruction(null, null);
            var result = listVariable.GetItem(new Memory(), new SquareBracketsInstruction([index]));
            var expected = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new BooleanVariable(false),
                new IntegerVariable(8)
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
                new BooleanVariable(false),
                new IntegerVariable(8)
            ]);
            var index1 = new IntegerInstruction(2);
            var index2 = new IntegerInstruction(1);
            var indexInstruction = new SquareBracketsInstruction([index1], new SquareBracketsInstruction([index2]));
            var result = listVariable.GetItem(new Memory(), indexInstruction);
            var expected = new StringVariable("b");
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesHandlesStackedRangeIndices()
        {
            var listVariable = new ListVariable([
                new IntegerVariable(5),
                new StringVariable("a"),
                new BooleanVariable(false),
                new IntegerVariable(8)
            ]);
            var index1 = new KeyValuePairInstruction(new IntegerInstruction(1), null);
            var index2 = new KeyValuePairInstruction(null, new IntegerInstruction(1));
            var indexInstruction = new SquareBracketsInstruction([index1], new SquareBracketsInstruction([index2]));
            var result = listVariable.GetItem(new Memory(), indexInstruction);
            var expected = new ListVariable([
                new StringVariable("a"),
                new BooleanVariable(false)
            ]);
            Assert.That(result, Is.EqualTo(expected));
        }
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
                new BooleanVariable(false)
            ]);
            var index = new IntegerInstruction(1);
            var value = new StringVariable("b");
            listVariable.SetItem(new Memory(), value, new SquareBracketsInstruction([index]));
            
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
                new BooleanVariable(false)
            ]);
            var index1 = new IntegerInstruction(2);
            var index2 = new IntegerInstruction(1);
            var value = new StringVariable("c");
            var indexInstruction = new SquareBracketsInstruction([index1], new SquareBracketsInstruction([index2]));
            listVariable.SetItem(new Memory(), value, indexInstruction);
            
            Assert.That(listVariable.Values[2], Is.TypeOf<ListVariable>());
            var innerListVariable = (ListVariable)listVariable.Values[2];
            Assert.That(innerListVariable.Values[1], Is.TypeOf<StringVariable>());
            Assert.That(((StringVariable)innerListVariable.Values[1]).Value, Is.EqualTo("c"));
        }
    }
}