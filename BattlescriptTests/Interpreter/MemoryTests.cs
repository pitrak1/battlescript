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

            Assert.That(memory.Scopes.Count, Is.EqualTo(1));
            Assert.That(memory.Scopes[0], Is.Empty);
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
            Assert.That(memory.Scopes[1], Is.Empty);
        }

        [Test]
        public void AddsExistingScopeIfArgumentProvided()
        {
            var memory = new Memory();
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(5) }
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
                { "x", new IntegerVariable(5) }
            };
            var memory = new Memory([scope]);
            var returnedScope = memory.RemoveScope();
            
            Assert.That(memory.Scopes.Count, Is.EqualTo(0));
            Assert.That(returnedScope, Is.EquivalentTo(scope));
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
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(5) }
            };
            var memory = new Memory([scope]);
            var returnedVariable = memory.GetVariable("x");
            
            Assert.That(returnedVariable is IntegerVariable);
            if (returnedVariable is IntegerVariable IntegerVariable)
            {
                Assert.That(IntegerVariable.Value, Is.EqualTo(5));
            }
        }
        
        [Test]
        public void GetsVariableNotInLastScope()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(5) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "y", new IntegerVariable(8) }
            };
            var memory = new Memory([scope1, scope2]);
            var returnedVariable = memory.GetVariable("x");
            
            Assert.That(returnedVariable is IntegerVariable);
            if (returnedVariable is IntegerVariable IntegerVariable)
            {
                Assert.That(IntegerVariable.Value, Is.EqualTo(5));
            }
        }

        [Test]
        public void PrefersVariablesInLaterScopes()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(5) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(8) }
            };
            var memory = new Memory([scope1, scope2]);
            var returnedVariable = memory.GetVariable("x");
            
            Assert.That(returnedVariable is IntegerVariable);
            if (returnedVariable is IntegerVariable IntegerVariable)
            {
                Assert.That(IntegerVariable.Value, Is.EqualTo(8));
            }
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
            memory.SetVariable(new VariableInstruction("x"), new IntegerVariable(5));
            var scopes = memory.Scopes;
            
            Assert.That(scopes[1]["x"] is IntegerVariable);
            if (scopes[1]["x"] is IntegerVariable IntegerVariable)
            {
                Assert.That(IntegerVariable.Value, Is.EqualTo(5));
            }
        }

        [Test]
        public void AssignsToVariableIfExists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(5) }
            };
            var memory = new Memory([scope]);
            memory.SetVariable(new VariableInstruction("x"), new IntegerVariable(8));
            var scopes = memory.Scopes;
            
            Assert.That(scopes[0]["x"] is IntegerVariable);
            if (scopes[0]["x"] is IntegerVariable IntegerVariable)
            {
                Assert.That(IntegerVariable.Value, Is.EqualTo(8));
            }
        }

        [Test]
        public void AssignsToVariableInLaterScopesIfExistsInMultipleScopes()
        {
            var scope1 = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(5) }
            };
            var scope2 = new Dictionary<string, Variable>()
            {
                { "x", new IntegerVariable(6) }
            };
            var memory = new Memory([scope1, scope2]);
            memory.SetVariable(new VariableInstruction("x"), new IntegerVariable(8));
            var scopes = memory.Scopes;
            
            Assert.That(scopes[0]["x"] is IntegerVariable);
            if (scopes[0]["x"] is IntegerVariable IntegerVariable1)
            {
                Assert.That(IntegerVariable1.Value, Is.EqualTo(5));
            }
            
            Assert.That(scopes[1]["x"] is IntegerVariable);
            if (scopes[1]["x"] is IntegerVariable IntegerVariable2)
            {
                Assert.That(IntegerVariable2.Value, Is.EqualTo(8));
            }
        }

        [Test]
        public void SupportsAssigningToLists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new ListVariable([new IntegerVariable(5), new IntegerVariable(8)]) }
            };
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new SquareBracketsInstruction([new IntegerInstruction(1)]));
            memory.SetVariable(variableInstructionWithIndex, new IntegerVariable(10));
            var scopes = memory.Scopes;
            
            Assert.That(scopes[0]["x"] is ListVariable);
            if (scopes[0]["x"] is ListVariable listVariable)
            {
                Assert.That(listVariable.Values.Count, Is.EqualTo(2));
                Assert.That(listVariable.Values[0] is IntegerVariable);
                if (listVariable.Values[0] is IntegerVariable IntegerVariable1)
                {
                    Assert.That(IntegerVariable1.Value, Is.EqualTo(5));
                }
                
                Assert.That(listVariable.Values[1] is IntegerVariable);
                if (listVariable.Values[1] is IntegerVariable IntegerVariable2)
                {
                    Assert.That(IntegerVariable2.Value, Is.EqualTo(10));
                }
            }
        }

        [Test]
        public void SupportsAssigningToDictionaries()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new DictionaryVariable(new Dictionary<Variable, Variable>()
                    {
                        {new IntegerVariable(5), new IntegerVariable(8)}
                    })}
            };
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new SquareBracketsInstruction([new IntegerInstruction(5)]));
            memory.SetVariable(variableInstructionWithIndex, new IntegerVariable(10));
            var expected = new DictionaryVariable(new Dictionary<Variable, Variable>()
            {
                { new IntegerVariable(5), new IntegerVariable(10) }
            });
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }
        
        [Test]
        public void SupportsAssigningToDictionariesAndLists()
        {
            var scope = new Dictionary<string, Variable>()
            {
                { "x", new ListVariable([
                    new IntegerVariable(5), 
                    new DictionaryVariable(new Dictionary<Variable, Variable>()
                    {
                        {new IntegerVariable(5), new IntegerVariable(8)}
                    })])}
            };
            var memory = new Memory([scope]);
            var variableInstructionWithIndex = new VariableInstruction(
                "x",
                new SquareBracketsInstruction(
                    [new IntegerInstruction(1)], 
                    new SquareBracketsInstruction([new IntegerInstruction(5)])));
            memory.SetVariable(variableInstructionWithIndex, new IntegerVariable(10));
            var expected = new ListVariable([
                new IntegerVariable(5),
                new DictionaryVariable(new Dictionary<Variable, Variable>()
                {
                    { new IntegerVariable(5), new IntegerVariable(10) }
                })
            ]);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
        }

        [Test]
        public void SupportsAssigningToObjects()
        {
            var classValues = new Dictionary<string, Variable>()
            {
                { "y", new IntegerVariable(6) }
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
            memory.SetVariable(variableInstructionWithIndex, new IntegerVariable(10));
            var scopes = memory.Scopes;
            
            Assert.That(scopes[0]["y"] is ObjectVariable);
            if (scopes[0]["y"] is ObjectVariable objectVariable)
            {
                Assert.That(objectVariable.Values.ContainsKey("y"));
                Assert.That(objectVariable.Values["y"] is IntegerVariable);

                if (objectVariable.Values["y"] is IntegerVariable IntegerVariable)
                {
                    Assert.That(IntegerVariable.Value, Is.EqualTo(10));
                }
            }
        }
    }
}