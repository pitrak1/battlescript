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
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 6));
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
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], BsTypes.Create(memory, BsTypes.Types.Int, 6));
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
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
            
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 6));
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
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 6));
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
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 9));
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
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 9));
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
                { "i", BsTypes.Create(memory, BsTypes.Types.Int, 5) },
                { "j", BsTypes.Create(memory, BsTypes.Types.Int, 6) }
            };
            var objectContext = new ObjectVariable(classValues, new ClassVariable("asdf", classValues));
                
            ArgumentTransfer.RunAndApply(memory, arguments, parameters, objectContext);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], objectContext);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
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
            var arguments = new List<Variable>() { BsTypes.Create(memory, BsTypes.Types.Int, 5), BsTypes.Create(memory, BsTypes.Types.Int, 6) };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            ArgumentTransfer.RunAndApply(memory, arguments, parameters);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 6));
        }
        
        [Test]
        public void ObjectContextBecomesFirstPositionalArgumentIfGiven()
        {
            // Function: (x, y)     Call: (5)
            var memory = Runner.Run("");
            var arguments = new List<Variable>() { BsTypes.Create(memory, BsTypes.Types.Int, 5) };
            var parameters = new List<Instruction>() { new VariableInstruction("x"), new VariableInstruction("y") };
            var classValues = new Dictionary<string, Variable>()
            {
                { "i", BsTypes.Create(memory, BsTypes.Types.Int, 5) },
                { "j", BsTypes.Create(memory, BsTypes.Types.Int, 6) }
            };
            var objectContext = new ObjectVariable(classValues, new ClassVariable("asdf", classValues));
            ArgumentTransfer.RunAndApply(memory, arguments, parameters, objectContext);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["x"], objectContext);
            Assertions.AssertVariablesEqual(memory.Scopes.First()["y"], BsTypes.Create(memory, BsTypes.Types.Int, 5));
        }
    }
}