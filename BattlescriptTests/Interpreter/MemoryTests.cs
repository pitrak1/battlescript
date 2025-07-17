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
    }
}