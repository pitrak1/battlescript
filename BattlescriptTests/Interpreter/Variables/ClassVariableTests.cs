using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public static class ClassVariableTests
{
    [TestFixture]
    public class GetItem
    {
        [Test]
        public void FindsGetItemMethodWhenPresentInClass()
        {
            var getItemFunction = new FunctionVariable([], []);
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"__getitem__", getItemFunction}
            });
            var index = new SquareBracketsInstruction([new StringInstruction("__getitem__")]);
            
            Assert.That(classVariable.GetItem(new Memory(), index), Is.SameAs(getItemFunction));
        }

        [Test]
        public void FindsGetItemMethodWhenPresentInSuperclasses()
        {
            var getItemFunction = new FunctionVariable([], []);
            var superclassVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"__getitem__", getItemFunction}
            });
            var classVariable = new ClassVariable(null, [superclassVariable]);
            var index = new SquareBracketsInstruction([new StringInstruction("__getitem__")]);
            
            Assert.That(classVariable.GetItem(new Memory(), index), Is.SameAs(getItemFunction));
        }
        
        [Test]
        public void FindsGetItemMethodWhenPresentInSupersuperclasses()
        {
            var getItemFunction = new FunctionVariable([], []);
            var superSuperclassVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"__getitem__", getItemFunction}
            });
            var superclassVariable = new ClassVariable(null, [superSuperclassVariable]);
            var classVariable = new ClassVariable(null, [superclassVariable]);
            var index = new SquareBracketsInstruction([new StringInstruction("__getitem__")]);
            
            Assert.That(classVariable.GetItem(new Memory(), index), Is.SameAs(getItemFunction));
        }
        
        [Test]
        public void RunsOverrideIfGetItemMethodExistsAndObjectContextIsGiven()
        {
            // This is effectively creating a class that overrides the [] functionality by always returning 5,
            // and we're showing this by indexing with "x" because it doesn't matter what we index with
            var getItemFunction = new FunctionVariable(
                [new VariableInstruction("self"), new VariableInstruction("index")], 
                [new ReturnInstruction(new NumberInstruction(5.0))]);
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new NumberVariable(7.0)},
                {"__getitem__", getItemFunction}
            });
            var objectVariable = new ObjectVariable(null, classVariable);
            var index = new SquareBracketsInstruction([new StringInstruction("x")]);
            
            Assert.That(
                classVariable.GetItem(new Memory(), index, objectVariable), 
                Is.EqualTo(new NumberVariable(5.0)));
        }
        
        [Test]
        public void DoesNotRunOverrideIfObjectContextIsNotGiven()
        {
            var getItemFunction = new FunctionVariable(
                [new VariableInstruction("self"), new VariableInstruction("index")], 
                [new ReturnInstruction(new NumberInstruction(5.0))]);
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new NumberVariable(7.0)},
                {"__getitem__", getItemFunction}
            });
            var objectVariable = new ObjectVariable(null, classVariable);
            var index = new SquareBracketsInstruction([new StringInstruction("x")]);
            
            Assert.That(
                classVariable.GetItem(new Memory(), index), 
                Is.EqualTo(new NumberVariable(7.0)));
        }

        [Test]
        public void FindsValuePresentInClassIfNoOverridePresentAndIndexIsNotGetItemMethod()
        {
            var classVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new StringVariable("asdf")}
            });
            var index = new SquareBracketsInstruction([new StringInstruction("x")]);
            
            Assert.That(classVariable.GetItem(new Memory(), index), Is.EqualTo(new StringVariable("asdf")));
        }
        
        [Test]
        public void FindsValuePresentInSuperclassIfNoOverridePresentAndIndexIsNotGetItemMethod()
        {
            var superclassVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new StringVariable("asdf")}
            });
            var classVariable = new ClassVariable(null, [superclassVariable]);
            var index = new SquareBracketsInstruction([new StringInstruction("x")]);
            
            Assert.That(classVariable.GetItem(new Memory(), index), Is.EqualTo(new StringVariable("asdf")));
        }
        
        [Test]
        public void FindsValuePresentInSuperSuperclassIfNoOverridePresentAndIndexIsNotGetItemMethod()
        {
            var superSuperclassVariable = new ClassVariable(new Dictionary<string, Variable>()
            {
                {"x", new StringVariable("asdf")}
            });
            var superclassVariable = new ClassVariable(null, [superSuperclassVariable]);
            var classVariable = new ClassVariable(null, [superclassVariable]);
            var index = new SquareBracketsInstruction([new StringInstruction("x")]);
            
            Assert.That(classVariable.GetItem(new Memory(), index), Is.EqualTo(new StringVariable("asdf")));
        }
    }
}