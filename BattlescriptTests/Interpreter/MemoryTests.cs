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
            var memory = new Memory();

            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
        }
    }

    [TestFixture]
    public class AddScope
    {
        [Test]
        public void AddsEmptyScopeIfNoArgumentProvided()
        {
            var memory = new Memory();
            memory.AddScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assert.That(memory.Scopes[1].Variables, Is.Empty);
        }

        [Test]
        public void AddsExistingScopeIfArgumentProvided()
        {
            var memory = new Memory();
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            memory.AddScope(new MemoryScope(scopeVariables));
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(2));
            Assert.That(memory.Scopes[1].Variables, Is.EquivalentTo(scopeVariables));
        }
    }

    [TestFixture]
    public class RemoveScope()
    {
        [Test]
        public void RemovesAndReturnsLastScopeInList()
        {
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = new Memory();
            memory.AddScope(new MemoryScope(scopeVariables));
            var returnedScope = memory.RemoveScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
            Assert.That(returnedScope.Variables, Is.EquivalentTo(scopeVariables));
        }
    }
    
    [TestFixture]
    public class RemoveScopes()
    {
        [Test]
        public void RemovesScopeCountGiven()
        {
            var memory = new Memory();
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
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = new Memory();
            memory.AddScope(new MemoryScope(scopeVariables));
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
        }
        
        [Test]
        public void GetsVariableNotInLastScope()
        {
            var scopeVariables1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scopeVariables2 = new Dictionary<string, Variable>()
            {
                { "y", new NumericVariable(8) }
            };
            var memory = new Memory();
            memory.AddScope(new MemoryScope(scopeVariables1));
            memory.AddScope(new MemoryScope(scopeVariables2));
            var returnedVariable = memory.GetVariable("x");
            
            Assertions.AssertVariablesEqual(returnedVariable, new NumericVariable(5));
        }

        [Test]
        public void PrefersVariablesInLaterScopes()
        {
            var scopeVariables1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scopeVariables2 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(8) }
            };
            var memory = new Memory();
            memory.AddScope(new MemoryScope(scopeVariables1));
            memory.AddScope(new MemoryScope(scopeVariables2));
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
            var memory = new Memory();
            memory.AddScope();
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(5));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1].Variables["x"], new NumericVariable(5));
        }

        [Test]
        public void AssignsToVariableIfExists()
        {
            var scopeVariables = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var memory = new Memory();
            memory.AddScope(new MemoryScope(scopeVariables));
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1].Variables["x"], new NumericVariable(8));
        }

        [Test]
        public void AssignsToVariableInLaterScopesIfExistsInMultipleScopes()
        {
            var scopeVariables1 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(5) }
            };
            var scopeVariables2 = new Dictionary<string, Variable>()
            {
                { "x", new NumericVariable(6) }
            };
            var memory = new Memory();
            memory.AddScope(new MemoryScope(scopeVariables1));
            memory.AddScope(new MemoryScope(scopeVariables2));
            memory.SetVariable(new VariableInstruction("x"), new NumericVariable(8));
            var scopes = memory.Scopes;
            
            Assertions.AssertVariablesEqual(scopes[1].Variables["x"], new NumericVariable(5));
            
            Assertions.AssertVariablesEqual(scopes[2].Variables["x"], new NumericVariable(8));
        }
    }
}