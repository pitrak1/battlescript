using Battlescript;

namespace BattlescriptTests;

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

            Assert.That(memory.GetScopes().Count, Is.EqualTo(1));
            Assert.That(memory.GetScopes()[0], Is.Empty);
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
            
            Assert.That(memory.GetScopes().Count, Is.EqualTo(2));
            Assert.That(memory.GetScopes()[1], Is.Empty);
        }

        [Test]
        public void AddsExistingScopeIfArgumentProvided()
        {
            var memory = new Memory();
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(5.0) }
            };
            memory.AddScope(scope);
            
            Assert.That(memory.GetScopes().Count, Is.EqualTo(2));
            Assert.That(memory.GetScopes()[1], Is.EquivalentTo(scope));
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
                { "x", new NumberVariable(5.0) }
            };
            var memory = new Memory([scope]);
            var returnedScope = memory.RemoveScope();
            
            Assert.That(memory.GetScopes().Count, Is.EqualTo(0));
            Assert.That(returnedScope, Is.EquivalentTo(scope));
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
                { "x", new NumberVariable(5.0) }
            };
            var memory = new Memory([scope]);
            var returnedVariable = memory.GetVariable("x");
            
            Assert.That(returnedVariable is NumberVariable);
            if (returnedVariable is NumberVariable numberVariable)
            {
                Assert.That(numberVariable.Value, Is.EqualTo(5.0));
            }
        }
        
        [Test]
        public void GetsVariableNotInLastScope()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(5.0) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "y", new NumberVariable(8.0) }
            };
            var memory = new Memory([scope1, scope2]);
            var returnedVariable = memory.GetVariable("x");
            
            Assert.That(returnedVariable is NumberVariable);
            if (returnedVariable is NumberVariable numberVariable)
            {
                Assert.That(numberVariable.Value, Is.EqualTo(5.0));
            }
        }

        [Test]
        public void PrefersVariablesInLaterScopes()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(5.0) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(8.0) }
            };
            var memory = new Memory([scope1, scope2]);
            var returnedVariable = memory.GetVariable("x");
            
            Assert.That(returnedVariable is NumberVariable);
            if (returnedVariable is NumberVariable numberVariable)
            {
                Assert.That(numberVariable.Value, Is.EqualTo(8.0));
            }
        }
    }

    [TestFixture]
    public class AssignToVariable
    {
        [Test]
        public void CreatesVariableInLastScopeIfDoesNotExist()
        {
            var memory = new Memory();
            memory.AddScope();
            memory.AssignToVariable(new VariableInstruction("x"), new NumberVariable(5.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[1]["x"] is NumberVariable);
            if (scopes[1]["x"] is NumberVariable numberVariable)
            {
                Assert.That(numberVariable.Value, Is.EqualTo(5.0));
            }
        }

        [Test]
        public void AssignsToVariableIfExists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(5.0) }
            };
            var memory = new Memory([scope]);
            memory.AssignToVariable(new VariableInstruction("x"), new NumberVariable(8.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[0]["x"] is NumberVariable);
            if (scopes[0]["x"] is NumberVariable numberVariable)
            {
                Assert.That(numberVariable.Value, Is.EqualTo(8.0));
            }
        }

        [Test]
        public void AssignsToVariableInLaterScopesIfExistsInMultipleScopes()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(5.0) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "x", new NumberVariable(6.0) }
            };
            var memory = new Memory([scope1, scope2]);
            memory.AssignToVariable(new VariableInstruction("x"), new NumberVariable(8.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[0]["x"] is NumberVariable);
            if (scopes[0]["x"] is NumberVariable numberVariable1)
            {
                Assert.That(numberVariable1.Value, Is.EqualTo(5.0));
            }
            
            Assert.That(scopes[1]["x"] is NumberVariable);
            if (scopes[1]["x"] is NumberVariable numberVariable2)
            {
                Assert.That(numberVariable2.Value, Is.EqualTo(8.0));
            }
        }

        [Test]
        public void SupportsAssigningToLists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new ListVariable([new NumberVariable(5.0), new NumberVariable(8.0)]) }
            };
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new SquareBracketsInstruction([new NumberInstruction(1.0)]));
            memory.AssignToVariable(variableInstructionWithIndex, new NumberVariable(10.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[0]["x"] is ListVariable);
            if (scopes[0]["x"] is ListVariable listVariable)
            {
                Assert.That(listVariable.Values.Count, Is.EqualTo(2));
                Assert.That(listVariable.Values[0] is NumberVariable);
                if (listVariable.Values[0] is NumberVariable numberVariable1)
                {
                    Assert.That(numberVariable1.Value, Is.EqualTo(5.0));
                }
                
                Assert.That(listVariable.Values[1] is NumberVariable);
                if (listVariable.Values[1] is NumberVariable numberVariable2)
                {
                    Assert.That(numberVariable2.Value, Is.EqualTo(10.0));
                }
            }
        }

        [Test]
        public void SupportsAssigningToDictionaries()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new DictionaryVariable([
                    new KeyValuePairVariable(new NumberVariable(5.0), new NumberVariable(8.0))
                ])}
            };
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new SquareBracketsInstruction([new NumberInstruction(5.0)]));
            memory.AssignToVariable(variableInstructionWithIndex, new NumberVariable(10.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[0]["x"] is DictionaryVariable);
            if (scopes[0]["x"] is DictionaryVariable dictionaryVariable)
            {
                Assert.That(dictionaryVariable.Values.Count, Is.EqualTo(1));
                Assert.That(dictionaryVariable.Values[0].Left is NumberVariable);
                if (dictionaryVariable.Values[0].Left is NumberVariable numberVariable1)
                {
                    Assert.That(numberVariable1.Value, Is.EqualTo(5.0));
                }
                
                Assert.That(dictionaryVariable.Values[0].Right is NumberVariable);
                if (dictionaryVariable.Values[0].Right is NumberVariable numberVariable2)
                {
                    Assert.That(numberVariable2.Value, Is.EqualTo(10.0));
                }
            }
        }
        
        [Test]
        public void SupportsAssigningToDictionariesAndArrays()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new ListVariable([
                    new NumberVariable(5.0), 
                    new DictionaryVariable([
                        new KeyValuePairVariable(new NumberVariable(5.0), new NumberVariable(8.0))
                    ])])}
            };
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new SquareBracketsInstruction(
                    [new NumberInstruction(1.0)], 
                    new SquareBracketsInstruction([new NumberInstruction(5.0)])));
            memory.AssignToVariable(variableInstructionWithIndex, new NumberVariable(10.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[0]["x"] is ListVariable);
            if (scopes[0]["x"] is ListVariable listVariable)
            {
                Assert.That(listVariable.Values.Count, Is.EqualTo(2));
                Assert.That(listVariable.Values[0] is NumberVariable);
                if (listVariable.Values[0] is NumberVariable numberVariable1)
                {
                    Assert.That(numberVariable1.Value, Is.EqualTo(5.0));
                }
                
                Assert.That(listVariable.Values[1] is DictionaryVariable);
                if (listVariable.Values[1] is DictionaryVariable dictionaryVariable)
                {
                    Assert.That(dictionaryVariable.Values.Count, Is.EqualTo(1));
                    Assert.That(dictionaryVariable.Values[0].Left is NumberVariable);
                    if (dictionaryVariable.Values[0].Left is NumberVariable numberVariable2)
                    {
                        Assert.That(numberVariable2.Value, Is.EqualTo(5.0));
                    }
                    
                    Assert.That(dictionaryVariable.Values[0].Right is NumberVariable);
                    if (dictionaryVariable.Values[0].Right is NumberVariable numberVariable3)
                    {
                        Assert.That(numberVariable3.Value, Is.EqualTo(10.0));
                    }
                }
            }
        }

        [Test]
        public void SupportsAssigningToClasses()
        {
            var classValues = new Dictionary<string, Variable>()
            {
                { "y", new NumberVariable(6.0) }
            };
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new ClassVariable(classValues)}
            };
                
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new SquareBracketsInstruction(
                    [new StringInstruction("y")]));
            memory.AssignToVariable(variableInstructionWithIndex, new NumberVariable(10.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[0]["x"] is ClassVariable);
            if (scopes[0]["x"] is ClassVariable classVariable)
            {
                Assert.That(classVariable.Values.ContainsKey("y"));
                Assert.That(classVariable.Values["y"] is NumberVariable);

                if (classVariable.Values["y"] is NumberVariable numberVariable)
                {
                    Assert.That(numberVariable.Value, Is.EqualTo(10.0));
                }
            }
        }

        [Test]
        public void SupportsAssigningToObjects()
        {
            var classValues = new Dictionary<string, Variable>()
            {
                { "y", new NumberVariable(6.0) }
            };
            var classVariable = new ClassVariable(classValues);
            var scope = new Dictionary<string, Variable>()
            {
                { "x", classVariable },
                { "y", new ObjectVariable(classValues, classVariable) }
            };
                
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "y",
                new SquareBracketsInstruction(
                    [new StringInstruction("y")]));
            memory.AssignToVariable(variableInstructionWithIndex, new NumberVariable(10.0));
            var scopes = memory.GetScopes();
            
            Assert.That(scopes[0]["y"] is ObjectVariable);
            if (scopes[0]["y"] is ObjectVariable objectVariable)
            {
                Assert.That(objectVariable.Values.ContainsKey("y"));
                Assert.That(objectVariable.Values["y"] is NumberVariable);

                if (objectVariable.Values["y"] is NumberVariable numberVariable)
                {
                    Assert.That(numberVariable.Value, Is.EqualTo(10.0));
                }
            }
        }
    }
}