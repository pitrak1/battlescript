using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class MemoryTests
{
    [TestFixture]
    public class Initialization
    {
        [Test]
        public void CreatesEmptyScopeOnObjectCreation()
        {
            var memory = Runner.Run("");

            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class AddScope
    {
        [Test]
        public void AddsEmptyScopeIfNoArgumentProvided()
        {
            var memory = Runner.Run("");
            memory.AddScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assert.That(memory.Scopes[1], Is.Empty);
        }

        [Test]
        public void AddsExistingScopeIfArgumentProvided()
        {
            var memory = Runner.Run("");
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            memory.AddScope(scope);
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assert.That(memory.Scopes[1], Is.EquivalentTo(scope));
        }
    }

    [TestFixture]
    public class RemoveScope()
    {
        [Test]
        public void RemovesAndReturnsLastScopeInList()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = Runner.Run("");
            memory.AddScope(scope);
            var returnedScope = memory.RemoveScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
            Assert.That(returnedScope, Is.EquivalentTo(scope));
        }
    }
    
    [TestFixture]
    public class RemoveScopes()
    {
        [Test]
        public void RemovesScopeCountGiven()
        {
            var memory = Runner.Run("");
            memory.AddScope();
            memory.AddScope();
            memory.AddScope();
            memory.RemoveScopes(3);
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class GetVariable()
    {
        [Test]
        public void GetsVariableInLastScope()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = Runner.Run("");
            memory.AddScope(scope);
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
        }
        
        [Test]
        public void GetsVariableNotInLastScope()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "y", new NumericVariable(8) }
            };
            var memory = Runner.Run("");
            memory.AddScope(scope1);
            memory.AddScope(scope2);
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
        }

        [Test]
        public void PrefersVariablesInLaterScopes()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(8) }
            };
            var memory = Runner.Run("");
            memory.AddScope(scope1);
            memory.AddScope(scope2);
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(8));
        }
    }

    [TestFixture]
    public class SetVariable
    {
        [Test]
        public void CreatesVariableInLastScopeIfDoesNotExist()
        {
            var memory = Runner.Run("");
            memory.AddScope();
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(5));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(5));
        }

        [Test]
        public void AssignsToVariableIfExists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = Runner.Run("");
            memory.AddScope(scope);
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(8));
        }

        [Test]
        public void AssignsToVariableInLaterScopesIfExistsInMultipleScopes()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(6) }
            };
            var memory = Runner.Run("");
            memory.AddScope(scope1);
            memory.AddScope(scope2);
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1]["x"], new NumericVariable(5));
            
            Assertions.AssertVariablesEqual(scopes[2]["x"], new NumericVariable(8));
        }
        
        // These are currently not passing because our variable comparer is not working correctly for equals. We need to
        // figure out the correct and comprehensive way to use GetHashCode to make it work
        
        [Test]
        public void SupportsAssigningToLists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new ListVariable([new NumericVariable(5), new NumericVariable(8)]) }
            };
            var memory = Runner.Run("");
            memory.AddScope(scope);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new ArrayInstruction([new NumericInstruction(1)], separator: "["));
            memory.SetVariable(variableInstructionWithIndex, new NumericVariable(10));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1]["x"], new ListVariable([new NumericVariable(5), new NumericVariable(10)]));
        }
        
        [Test]
        public void SupportsAssigningToDictionaries()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new DictionaryVariable(new Dictionary<int, Variable>()
                    {
                        {5, new NumericVariable(8)}
                    })}
            };
            var memory = Runner.Run("");
            memory.AddScope(scope);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new ArrayInstruction([new NumericInstruction(5)], separator: "["));
            memory.SetVariable(variableInstructionWithIndex, new NumericVariable(10));
            var expected = new DictionaryVariable(new Dictionary<int, Variable>()
            {
                {5, new NumericVariable(10) }
            });
            Assertions.AssertVariablesEqual(memory.Scopes[1]["x"], expected);
        }
        
        [Test]
        public void SupportsAssigningToDictionariesAndLists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new ListVariable([
                    BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 5), 
                    new DictionaryVariable(new Dictionary<int, Variable>()
                    {
                        {5, new NumericVariable(8)}
                    })])}
            };
            var memory = Runner.Run("");
            memory.AddScope(scope);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new ArrayInstruction(
                    [new NumericInstruction(1)],
                    Consts.SquareBrackets,
                    next: new ArrayInstruction([new NumericInstruction(5)], Consts.SquareBrackets)));
            memory.SetVariable(variableInstructionWithIndex, new NumericVariable(10));
            var expected = new ListVariable([
                BuiltInTypeHelper.CreateBuiltInTypeWithValue(Runner.Run(""), "int", 5),
                new DictionaryVariable(new Dictionary<int, Variable>()
                {
                    { 5, new NumericVariable(10) }
                })
            ]);
            
            Assertions.AssertVariablesEqual(memory.Scopes[1]["x"], expected);
        }
        
        [Test]
        public void SupportsAssigningToObjects()
        {
            var classValues = new Dictionary<string, Variable>()
            {
                { "y", new NumericVariable(6) }
            };
            var classVariable = new ClassVariable("asdf", classValues);
            var scope = new Dictionary<string, Variable>()
            {
                { "x", classVariable },
                { "y", new ObjectVariable(classValues, classVariable) }
            };
                
            var memory = Runner.Run("");
            memory.AddScope(scope);
            var variableInstructionWithIndex = new VariableInstruction(
                "y",
                new ArrayInstruction(
                    [new StringInstruction("y")], separator: "["));
            memory.SetVariable(variableInstructionWithIndex, new NumericVariable(10));
            var scopes = memory.Scopes;
            
            Assert.That(scopes[1]["y"] is ObjectVariable);
            if (scopes[1]["y"] is ObjectVariable objectVariable)
            {
                Assert.That(objectVariable.Values.ContainsKey("y"));
                Assert.That(objectVariable.Values["y"] is NumericVariable);
        
                if (objectVariable.Values["y"] is NumericVariable NumericVariable)
                {
                    Assert.That(NumericVariable.Value, Is.EqualTo(10));
                }
            }
        }
    }
}