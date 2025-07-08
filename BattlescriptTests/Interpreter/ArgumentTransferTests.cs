using Battlescript;

namespace BattlescriptTests.InterpreterTests;

[TestFixture]
public static class ArgumentTransferTests
{
    [TestFixture]
    public class WithInstructionArguments
    {
        [Test]
        public void SupportsPositionalArguments()
        {
            // Function: (x, y)     Call: (5, 6)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>() { new NumericInstruction(5), new NumericInstruction(6) };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6)));
        }
        
        [Test]
        public void SupportsKeywordArguments()
        {
            // Function: (x, y)     Call: (y=5, x=6)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("y"), new NumericInstruction(5)), 
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(6)), 
            };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6)));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
        }
        
        [Test]
        public void SupportsCombinationOfPositionalAndKeywordArguments()
        {
            // Function: (x, y)     Call: (5, y=6)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5), 
                new AssignmentInstruction("=", new VariableInstruction("y"), new NumericInstruction(6)), 
            };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6)));
        }
        
        [Test]
        public void ThrowsErrorIfKeywordArgumentIsBeforePositionalArgument()
        {
            // Function: (x, y)     Call: (x=5, 6)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(5)), 
                new NumericInstruction(6), 
            };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            Assert.Throws<InterpreterKeywordArgBeforePositionalArgException>(() => ArgumentTransfer.RunAndApply(memory, arguments, parameters));
        }
        
        [Test]
        public void ThrowsErrorIfKeywordArgumentAndPositionalArgumentAddressSameVariable()
        {
            // Function: (x, y)     Call: (5, 6, x=6)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5), 
                new NumericInstruction(6), 
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(6))
            };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            Assert.Throws<InterpreterMultipleArgumentsForParameterException>(() => ArgumentTransfer.RunAndApply(memory, arguments, parameters));
        }
        
        [Test]
        public void ThrowsErrorIfRequiredArgumentIsMissing()
        {
            // Function: (x, y)     Call: (5)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5)
            };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            Assert.Throws<InterpreterMissingRequiredArgumentException>(() => ArgumentTransfer.RunAndApply(memory, arguments, parameters));
        }
        
        [Test]
        public void AllowsDefaultValues()
        {
            // Function: (x, y=6)     Call: (5)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5)
            };
            var parameters = new List<Instruction>()
            {
                new VariableInstruction("x"), 
                new AssignmentInstruction("=", new VariableInstruction("y"), new NumericInstruction(6))
            };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6)));
        }
        
        [Test]
        public void IgnoresDefaultValueIfPositionalArgumentGiven()
        {
            // Function: (x, y=6)     Call: (5, 9)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5),
                new NumericInstruction(9),
            };
            var parameters = new List<Instruction>()
            {
                new VariableInstruction("x"), 
                new AssignmentInstruction("=", new VariableInstruction("y"), new NumericInstruction(6))
            };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9)));
        }
        
        [Test]
        public void IgnoresDefaultValueIfKeywordArgumentGiven()
        {
            // Function: (x, y=6)     Call: (5, y=9)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5),
                new AssignmentInstruction("=", new VariableInstruction("y"), new NumericInstruction(9))
            };
            var parameters = new List<Instruction>()
            {
                new VariableInstruction("x"), 
                new AssignmentInstruction("=", new VariableInstruction("y"), new NumericInstruction(6))
            };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 9)));
        }
        
        [Test]
        public void ThrowsErrorIfRequiredParamAfterDefaultParam()
        {
            // Function: (x=5, y)     Call: (5, y=9)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5),
                new AssignmentInstruction("=", new VariableInstruction("y"), new NumericInstruction(9))
            };
            var parameters = new List<Instruction>()
            {
                new AssignmentInstruction("=", new VariableInstruction("x"), new NumericInstruction(5)),
                new VariableInstruction("y"), 
            };
            Assert.Throws<InterpreterRequiredParamFollowsDefaultParamException>(() => ArgumentTransfer.RunAndApply(memory, arguments, parameters));
        }
        
        [Test]
        public void ObjectContextBecomesFirstPositionalArgumentIfGiven()
        {
            // Function: (x, y)     Call: (5)
            var memory = Runner.Run("");
            var arguments = new List<Instruction>()
            {
                new NumericInstruction(5),
            };
            var parameters = new List<Instruction>()
            {
                new VariableInstruction("x"),                
                new VariableInstruction("y"), 
            };
            var classValues = new Dictionary<string, Variable>()
            {
                { "i", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5) },
                { "j", BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 6) }
            };
            var objectContext = new ObjectVariable(classValues, new ClassVariable(classValues));
                
            ArgumentTransfer.RunAndApply(memory, arguments, parameters, objectContext);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(objectContext));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(BuiltInTypeHelper.CreateBuiltInTypeWithValue(memory, "int", 5)));        
        }
    }

    [TestFixture]
    public class WithVariableArguments
    {
        [Test]
        public void TreatsAllVariableArgumentsAsPositionalArguments()
        {
            // Function: (x, y)     Call: (5, 6)
            var memory = Runner.Run("");
            var arguments = new List<Variable>() { new NumericVariable(5), new NumericVariable(6) };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(new NumericVariable(5)));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(new NumericVariable(6)));
        }
        
        [Test]
        public void ObjectContextBecomesFirstPositionalArgumentIfGiven()
        {
            // Function: (x, y)     Call: (5)
            var memory = Runner.Run("");
            var arguments = new List<Variable>() { new NumericVariable(5) };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            var classValues = new Dictionary<string, Variable>()
            {
                { "i", new NumericVariable(5) },
                { "j", new NumericVariable(6) }
            };
            var objectContext = new ObjectVariable(classValues, new ClassVariable(classValues));
            ArgumentTransfer.RunAndApply(memory, arguments, parameters, objectContext);
            
            Assert.That(memory.Scopes[0], Contains.Key("x"));
            Assert.That(memory.Scopes[0]["x"], Is.EqualTo(objectContext));
            
            Assert.That(memory.Scopes[0], Contains.Key("y"));
            Assert.That(memory.Scopes[0]["y"], Is.EqualTo(new NumericVariable(5)));
        }
    }
}