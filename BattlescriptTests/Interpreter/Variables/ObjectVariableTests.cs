using Battlescript;

namespace BattlescriptTests.InterpreterTests.VariablesTests;

[TestFixture]
public static class ObjectVariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void RunsOverrideWhenGetItemMethodExists()
        {
            // This is effectively creating a class that overrides the [] functionality by always returning 5,
            // and we're showing this by indexing with "x" because it doesn't matter what we index with
            var getItemFunction = new FunctionVariable(
                [new VariableInstruction("self"), new VariableInstruction("index")], 
                [new ReturnInstruction(new IntegerInstruction(5))]);
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"__getitem__", getItemFunction},
                {"x", new IntegerVariable(10)}
            });
            var objectVariable = classVariable.CreateObject();
            var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
            var result = objectVariable.GetItem(new Memory(), index);
            Assert.That(result, Is.InstanceOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(5.0));
        }
        
        [Test]
        public void DoesNotRunOverrideIfObjectContextIsNotGiven()
        {
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new IntegerVariable(7)},
            });
            var objectVariable = classVariable.CreateObject();
            var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
            var result = objectVariable.GetItem(new Memory(), index);
            Assert.That(result, Is.InstanceOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(7));
        }

        [Test]
        public void HandlesStackedIndices()
        {
            var innerClassVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "y", new IntegerVariable(6) }
            });
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new IntegerVariable(7)},
                {"asdf", innerClassVariable.CreateObject()}
            });
            var objectVariable = classVariable.CreateObject();
            var index = new ArrayInstruction(
                [new StringInstruction("asdf")],
                new ArrayInstruction([new StringInstruction("y")], separator: "["),
                separator: "[");
            var result = objectVariable.GetItem(new Memory(), index);
            Assert.That(result, Is.InstanceOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(6));
        }
    }
    
    [TestFixture]
    public class SetItem
    {
        // [Test]
        // public void RunsOverrideWhenSetItemMethodExists()
        // {
        //     // The reason we get caught in an infinite loop in this test is because a __setitem__ function will usually
        //     // call self[key] = something.  This will invoke the same override again because we're setting a value in the
        //     // same object.  I think ultimately the solution will involve creating lexical contexts (which I already
        //     // was going to ahve to do for the super keyword anyway), and we will probably have to ignore overrides
        //     // if the code is in a class lexical scope.
        //     
        //     // Thinking about it more, I wonder if you do self.x = whatever in a python method, but have an override.
        //     // Is the override called even if the code is within the calss scope?  *ANSWER* it does.  Just tested.
        //     // This returned 7 twice.  If it exempted class code, it would have been 8 the second time.
        //     
        //     // class asdf(list):
        //     //      def __init__(self):
        //     //          self.append(5)
        //     //          self.append(6)
        //     //          self.append(8)
        //     //
        //     //      def __getitem__(self, key):
        //     //          return 7
        //     //
        //     //      def othermethod(self):
        //     //          return self[2]
        //     //
        //     // y = asdf()
        //     // print(y[0])
        //     // print(y.othermethod())
        //     
        //     // This means unfortunately that we'll have to have a special flag for overriding specific methods so when
        //     // we call an override, we can pass through the flag to ignore looking for itself. :(
        //     
        //     // This is actually incorrect because this code is not overriding __getitem__ correctly.  If you do something
        //     // like `return self[key]` in __getitem__, it will be a stackoverflow like our test.  The proper way is to
        //     // use `super().__getitem__(blahblahblah)`.  This could be a little tricky for our in-interpreter list vs 
        //     // in-code list I've been thinking about, but still good to know.
        //     
        //     // This is effectively creating a class that overrides the [] set functionality by setting the value to 5
        //     var setItemFunction = new FunctionVariable(
        //         [new VariableInstruction("self"), new VariableInstruction("key"), new VariableInstruction("value")], 
        //         [
        //             new AssignmentInstruction(
        //                 "=", 
        //                 new VariableInstruction("self", new SquareBracketsInstruction([new VariableInstruction("key")])),
        //                 new IntegerInstruction(5)
        //                 ),
        //         ]);
        //     var classVariable = new ClassVariable(new Dictionary<string, Variable>()
        //     {
        //         {"__setitem__", setItemFunction},
        //         {"x", new IntegerVariable(10.0)}
        //     });
        //     var objectVariable = classVariable.CreateObject();
        //     var index = new SquareBracketsInstruction([new StringInstruction("x")]);
        //     objectVariable.SetItem(new Memory(), new IntegerVariable(8.0), index, objectVariable);
        //     var result = objectVariable.GetItem(new Memory(), index);
        //     Assert.That(result, Is.InstanceOf<IntegerVariable>());
        //     Assert.That(((IntegerVariable)result).Value, Is.EqualTo(5.0));
        // }
        
        [Test]
        public void DoesNotRunOverrideIfObjectContextIsNotGiven()
        {
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new IntegerVariable(7)},
            });
            var objectVariable = classVariable.CreateObject();
            var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
            objectVariable.SetItem(new Memory(), new IntegerVariable(10), index);
            var result = objectVariable.GetItem(new Memory(), index);
            Assert.That(result, Is.InstanceOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(10));
        }
        
        [Test]
        public void HandlesStackedIndices()
        {
            var innerClassVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                { "y", new IntegerVariable(6) }
            });
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new IntegerVariable(7)},
                {"asdf", innerClassVariable.CreateObject()}
            });
            var objectVariable = classVariable.CreateObject();
            var index = new ArrayInstruction(
                [new StringInstruction("asdf")],
                new ArrayInstruction([new StringInstruction("y")], separator: "["),
                separator: "[");
            objectVariable.SetItem(new Memory(), new IntegerVariable(8), index, objectVariable);
            var result = objectVariable.GetItem(new Memory(), index);
            Assert.That(result, Is.InstanceOf<IntegerVariable>());
            Assert.That(((IntegerVariable)result).Value, Is.EqualTo(8));
        }
    }
}