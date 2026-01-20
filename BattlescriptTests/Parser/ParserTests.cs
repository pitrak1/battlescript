using Battlescript;

namespace BattlescriptTests.ParserTests;

[TestFixture]
public class ParserTests
{
    [TestFixture]
    public class GenericNesting
    {
        [Test]
        public void SingleInstruction()
        {
            var expected = new List<Instruction>
            {
                new AssignmentInstruction(
                    operation: "=", 
                    left: new VariableInstruction("x"),
                    right: new NumericInstruction(5)
                )
            };
            Assertions.AssertInputProducesParserOutput("x = 5", expected);
        }

        [Test]
        public void ConditionalInstructionBlocks()
        {
            var input = """
                        if 5 < 6:
                            x = 5
                        """;
            var expected = new List<Instruction>
            {
                new IfInstruction(
                    condition: new OperationInstruction(
                       operation: "<",
                       left: new NumericInstruction(5), 
                       right: new NumericInstruction(6)
                    ), 
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ]
                )
            };
            
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void InstructionsBeforeConditionalInstructionBlock()
        {
            var input = """
                        y = 7
                        if 5 < 6:
                            x = 5
                        """;
            var expected = new List<Instruction>
            {
                new AssignmentInstruction(
                    operation: "=",
                    left: new VariableInstruction("y"),
                    right: new NumericInstruction(7)
                ),
                new IfInstruction(
                    condition: new OperationInstruction(
                        operation: "<",
                        left: new NumericInstruction(5), 
                        right: new NumericInstruction(6)
                    ), 
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ]
                )
            };
            
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void InstructionsAfterConditionalInstructionBlock()
        {
            var input = """
                        if 5 < 6:
                            x = 5
                        y = 7
                        """;
            var expected = new List<Instruction>
            {
                new IfInstruction(
                    condition: new OperationInstruction(
                        operation: "<",
                        left: new NumericInstruction(5), 
                        right: new NumericInstruction(6)
                    ),
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ]
                ),
                new AssignmentInstruction(
                    operation: "=",
                    left: new VariableInstruction("y"),
                    right: new NumericInstruction(7)
                )
            };
            
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void MultipleLevelReductions()
        {
            var input = """
                        if 5 < 6:
                            if 5 < 6:
                                x = 6
                        y = 7
                        """;
            var expected = new List<Instruction>
            {
                new IfInstruction(
                    condition: new OperationInstruction(
                        operation: "<",
                        left: new NumericInstruction(5), 
                        right: new NumericInstruction(6)
                    ),
                    instructions: [
                        new IfInstruction(
                            condition: new OperationInstruction(
                                operation: "<",
                                left: new NumericInstruction(5), 
                                right: new NumericInstruction(6)
                            ),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(6)
                                )
                            ]
                        )
                    ]
                ),
                new AssignmentInstruction(
                    operation: "=",
                    left: new VariableInstruction("y"),
                    right: new NumericInstruction(7)
                )
            };
            
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
    }

    [TestFixture]
    public class MissingIndents
    {
        [Test]
        public void DetectsMissingIndents()
        {
            var input = """
                        if 5 < 6:
                        x = 5
                        """;
            Assert.Throws<InternalRaiseException>(() => { Runner.Parse(input); });
        }
    }

    [TestFixture]
    public class IfElse
    {
        [Test]
        public void JoinsIfAndElseInstructions()
        {
            var input = """
                        if 5 < 6:
                            x = 5
                        else:
                            x = 7
                        """;
            var expected = new List<Instruction>
            {
                new IfInstruction(
                    condition: new OperationInstruction(
                        operation: "<",
                        left: new NumericInstruction(5), 
                        right: new NumericInstruction(6)
                    ),
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ],
                    next: new ElseInstruction(
                        instructions: [
                            new AssignmentInstruction(
                                operation: "=",
                                left: new VariableInstruction("x"),
                                right: new NumericInstruction(7)
                            )
                        ]
                    )
                )
            };
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void JoinsElifsInstructions()
        {
            var input = """
                        if 5 < 6:
                            x = 5
                        elif 6 < 7:
                            x = 6
                        else:
                            x = 7
                        """;
            var expected = new List<Instruction>
            {
                new IfInstruction(
                    condition: new OperationInstruction(
                        operation: "<",
                        left: new NumericInstruction(5), 
                        right: new NumericInstruction(6)
                    ),
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ],
                    next: new ElseInstruction(
                        condition: new OperationInstruction(
                            operation: "<",
                            left: new NumericInstruction(6), 
                            right: new NumericInstruction(7)
                        ),
                        instructions: [
                            new AssignmentInstruction(
                                operation: "=",
                                left: new VariableInstruction("x"),
                                right: new NumericInstruction(6)
                            )
                        ],
                        next: new ElseInstruction(
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(7)
                                )
                            ]
                        )
                    )
                )
            };
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
    }
    
    [TestFixture]
    public class TryExceptElseFinally
    {
        [Test]
        public void JoinsTryAndExceptInstructions()
        {
            var input = """
                        try:
                            x = 5
                        except TypeError:
                            x = 6
                        """;
            var expected = new List<Instruction>
            {
                new TryInstruction(
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ],
                    excepts: [
                        new ExceptInstruction(
                            exceptionType: new VariableInstruction("TypeError"),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(6)
                                )
                            ]
                        )
                    ]
                )
            };
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void SupportsExceptVariable()
        {
            var input = """
                        try:
                            x = 5
                        except TypeError as y:
                            x = 6
                        """;
            var expected = new List<Instruction>
            {
                new TryInstruction(
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ],
                    excepts: [
                        new ExceptInstruction(
                            exceptionType: new VariableInstruction("TypeError"),
                            exceptionVariable: new VariableInstruction("y"),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(6)
                                )
                            ]
                        )
                    ]
                )
            };
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void SupportsSeveralExcepts()
        {
            var input = """
                        try:
                            x = 5
                        except TypeError:
                            x = 6
                        except ValueError:
                            x = 7
                        except AssertionError:
                            x = 8
                        """;
            var expected = new List<Instruction>
            {
                new TryInstruction(
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ],
                    excepts: [
                        new ExceptInstruction(
                            exceptionType: new VariableInstruction("TypeError"),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(6)
                                )
                            ]
                        ),
                        new ExceptInstruction(
                            exceptionType: new VariableInstruction("ValueError"),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(7)
                                )
                            ]
                        ),
                        new ExceptInstruction(
                            exceptionType: new VariableInstruction("AssertionError"),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(8)
                                )
                            ]
                        )
                    ]
                )
            };
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void SupportsElseBlock()
        {
            var input = """
                        try:
                            x = 5
                        except TypeError:
                            x = 6
                        else:
                            x = 7
                        """;
            var expected = new List<Instruction>
            {
                new TryInstruction(
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ],
                    excepts: [
                        new ExceptInstruction(
                            exceptionType: new VariableInstruction("TypeError"),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(6)
                                )
                            ]
                        )
                    ],
                    elseInstruction: new ElseInstruction(
                        instructions: [
                            new AssignmentInstruction(
                                operation: "=",
                                left: new VariableInstruction("x"),
                                right: new NumericInstruction(7)
                            )
                        ]        
                    )
                )
            };
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
        
        [Test]
        public void SupportsFinallyBlock()
        {
            var input = """
                        try:
                            x = 5
                        except TypeError:
                            x = 6
                        else:
                            x = 7
                        finally:
                            x = 8
                        """;
            var expected = new List<Instruction>
            {
                new TryInstruction(
                    instructions: [
                        new AssignmentInstruction(
                            operation: "=",
                            left: new VariableInstruction("x"),
                            right: new NumericInstruction(5)
                        )
                    ],
                    excepts: [
                        new ExceptInstruction(
                            exceptionType: new VariableInstruction("TypeError"),
                            instructions: [
                                new AssignmentInstruction(
                                    operation: "=",
                                    left: new VariableInstruction("x"),
                                    right: new NumericInstruction(6)
                                )
                            ]
                        )
                    ],
                    elseInstruction: new ElseInstruction(
                        instructions: [
                            new AssignmentInstruction(
                                operation: "=",
                                left: new VariableInstruction("x"),
                                right: new NumericInstruction(7)
                            )
                        ]        
                    ),
                    finallyInstruction: new FinallyInstruction(
                        instructions: [
                            new AssignmentInstruction(
                                operation: "=",
                                left: new VariableInstruction("x"),
                                right: new NumericInstruction(8)
                            )
                        ]
                    )
                )
            };
            Assertions.AssertInputProducesParserOutput(input, expected);
        }
    }
}