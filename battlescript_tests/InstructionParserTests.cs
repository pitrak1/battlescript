using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.LexerNS;
using BattleScript.ParserNS;
using BattleScript.InterpreterNS;
using System.Diagnostics;

namespace BattleScript.InstructionParserTests;

public class InstructionParserTests
{
    [TestFixture]
    public class DeclarationsAndAssignments
    {
        public Instruction CreateExpectedAssignmentInstruction(
            Consts.InstructionTypes type,
            dynamic? value
        )
        {
            return new Instruction(
                null,
                null,
                Consts.InstructionTypes.Assignment,
                null,
                null,
                new List<Instruction>(),
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.Declaration,
                    "x"
                ),
                new Instruction(
                    null,
                    null,
                    type,
                    value
                )
            );
        }

        [Test]
        public void Numbers()
        {
            Lexer lexer = new Lexer("var x = 15");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedAssignmentInstruction(
                Consts.InstructionTypes.Number,
                15
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void SingleQuoteStrings()
        {
            Lexer lexer = new Lexer("var x = 'asdf'");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedAssignmentInstruction(
                Consts.InstructionTypes.String,
                "asdf"
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void DoubleQuoteStrings()
        {
            Lexer lexer = new Lexer("var x = \"asdf\"");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedAssignmentInstruction(
                Consts.InstructionTypes.String,
                "asdf"
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void Booleans()
        {
            Lexer lexer = new Lexer("var x = true");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedAssignmentInstruction(
                Consts.InstructionTypes.Boolean,
                true
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void Variables()
        {
            Lexer lexer = new Lexer("var x = y");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedAssignmentInstruction(
                Consts.InstructionTypes.Variable,
                "y"
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }

    [TestFixture]
    public class Operators
    {
        public Instruction CreateExpectedOperationInstruction(
            dynamic? value
        )
        {
            return new Instruction(
                null,
                null,
                Consts.InstructionTypes.Operation,
                value,
                null,
                new List<Instruction>(),
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.Number,
                    5
                ),
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.Number,
                    6
                )
            );
        }

        [Test]
        public void Addition()
        {
            Lexer lexer = new Lexer("5 + 6");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedOperationInstruction("+");

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void Multiplication()
        {
            Lexer lexer = new Lexer("5 * 6");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedOperationInstruction("*");

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void Equality()
        {
            Lexer lexer = new Lexer("5 == 6");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedOperationInstruction("==");

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void GreaterThan()
        {
            Lexer lexer = new Lexer("5 > 6");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedOperationInstruction(">");

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void LessThan()
        {
            Lexer lexer = new Lexer("5 < 6");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = CreateExpectedOperationInstruction("<");

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }
}