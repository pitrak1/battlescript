using Battlescript;

namespace BattlescriptTests.InterpreterTests.VariablesTests;

[TestFixture]
public static class ClassVariableTests
{
    // [TestFixture]
    // public class GetItem
    // {
    //     [Test]
    //     public void FindsGetItemMethodWhenPresentInClass()
    //     {
    //         var getItemFunction = new FunctionVariable([], []);
    //         var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"__getitem__", getItemFunction}
    //         });
    //         var index = new ArrayInstruction([new StringInstruction("__getitem__")], separator: "[");
    //         Assertions.AssertVariablesEqual(classVariable.GetItem(Runner.Run(""), index), getItemFunction);
    //     }
    //
    //     [Test]
    //     public void FindsGetItemMethodWhenPresentInSuperclasses()
    //     {
    //         var getItemFunction = new FunctionVariable([], []);
    //         var superclassVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"__getitem__", getItemFunction}
    //         });
    //         var classVariable = new ClassVariable("asdf", null, [superclassVariable]);
    //         var index = new ArrayInstruction([new StringInstruction("__getitem__")], separator: "[");
    //         
    //         Assertions.AssertVariablesEqual(classVariable.GetItem(Runner.Run(""), index), getItemFunction);
    //     }
    //     
    //     [Test]
    //     public void FindsGetItemMethodWhenPresentInSupersuperclasses()
    //     {
    //         var getItemFunction = new FunctionVariable([], []);
    //         var superSuperclassVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"__getitem__", getItemFunction}
    //         });
    //         var superclassVariable = new ClassVariable("asdf", null, [superSuperclassVariable]);
    //         var classVariable = new ClassVariable("asdf", null, [superclassVariable]);
    //         var index = new ArrayInstruction([new StringInstruction("__getitem__")], separator: "[");
    //         
    //         Assertions.AssertVariablesEqual(classVariable.GetItem(Runner.Run(""), index), getItemFunction);
    //     }
    //     
    //     [Test]
    //     public void RunsOverrideIfGetItemMethodExistsAndObjectContextIsGiven()
    //     {
    //         // This is effectively creating a class that overrides the [] functionality by always returning 5,
    //         // and we're showing this by indexing with "x" because it doesn't matter what we index with
    //         var getItemFunction = new FunctionVariable(
    //             [new VariableInstruction("self"), new VariableInstruction("index")], 
    //             [new ReturnInstruction(new NumericInstruction(5))]);
    //         var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"x", new NumericVariable(7)},
    //             {"__getitem__", getItemFunction}
    //         });
    //         var objectVariable = new ObjectVariable(null, classVariable);
    //         var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
    //         
    //         Assertions.AssertVariablesEqual(
    //             classVariable.GetItem(Runner.Run(""), index, objectVariable), 
    //             BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 5.0));
    //     }
    //     
    //     [Test]
    //     public void DoesNotRunOverrideIfObjectContextIsNotGiven()
    //     {
    //         var getItemFunction = new FunctionVariable(
    //             [new VariableInstruction("self"), new VariableInstruction("index")], 
    //             [new ReturnInstruction(new NumericInstruction(5))]);
    //         var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"x", new NumericVariable(7)},
    //             {"__getitem__", getItemFunction}
    //         });
    //         var objectVariable = new ObjectVariable(null, classVariable);
    //         var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
    //         
    //         Assertions.AssertVariablesEqual(
    //             classVariable.GetItem(Runner.Run(""), index), 
    //             new NumericVariable(7));
    //     }
    //
    //     [Test]
    //     public void FindsValuePresentInClassIfNoOverridePresentAndIndexIsNotGetItemMethod()
    //     {
    //         var classVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"x", new StringVariable("asdf")}
    //         });
    //         var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
    //         
    //         Assertions.AssertVariablesEqual(classVariable.GetItem(Runner.Run(""), index), new StringVariable("asdf"));
    //     }
    //     
    //     [Test]
    //     public void FindsValuePresentInSuperclassIfNoOverridePresentAndIndexIsNotGetItemMethod()
    //     {
    //         var superclassVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"x", new StringVariable("asdf")}
    //         });
    //         var classVariable = new ClassVariable("asdf", null, [superclassVariable]);
    //         var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
    //         
    //         Assertions.AssertVariablesEqual(classVariable.GetItem(Runner.Run(""), index), new StringVariable("asdf"));
    //     }
    //     
    //     [Test]
    //     public void FindsValuePresentInSuperSuperclassIfNoOverridePresentAndIndexIsNotGetItemMethod()
    //     {
    //         var superSuperclassVariable = new ClassVariable("asdf", new Dictionary<string, Variable>()
    //         {
    //             {"x", new StringVariable("asdf")}
    //         });
    //         var superclassVariable = new ClassVariable("asdf", null, [superSuperclassVariable]);
    //         var classVariable = new ClassVariable("asdf", null, [superclassVariable]);
    //         var index = new ArrayInstruction([new StringInstruction("x")], separator: "[");
    //         
    //         Assertions.AssertVariablesEqual(classVariable.GetItem(Runner.Run(""), index), new StringVariable("asdf"));
    //     }
    // }
}