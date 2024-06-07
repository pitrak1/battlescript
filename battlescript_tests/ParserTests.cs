using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.InterpreterNS;
using System.Diagnostics;

namespace BattleScript.ParserTests;

public class ParserTests
{
    [Test]
    public void BasicInstruction()
    {
        Lexer lexer = new Lexer("var x = 5;");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Declaration,
                "x"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                5
            )
        );

        Assert.That(instructions.Count, Is.EqualTo(1));
        Assert.That(instructions[0], Is.EqualTo(expectedInstruction));
    }

    [Test]
    public void CurlyBracesWithoutSemicolons()
    {
        Lexer lexer = new Lexer("var x = {'asdf': 5};");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Declaration,
                "x"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Dictionary,
                new List<Instruction>() {
                    new Instruction(null, null, Consts.InstructionTypes.String, "asdf"),
                    new Instruction(null, null, Consts.InstructionTypes.Number, 5)
                }
            )
        );

        Assert.That(instructions.Count, Is.EqualTo(1));
        Assert.That(instructions[0], Is.EqualTo(expectedInstruction));
    }

    [Test]
    public void CurlyBracesWithSemicolons()
    {
        Lexer lexer = new Lexer("if (true) { var x = 5; }");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Instruction blockInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Declaration,
                "x"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                5
            )
        );

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.If,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Boolean,
                true
            ),
            null,
            new List<Instruction>() { blockInstruction }
        );

        Assert.That(instructions.Count, Is.EqualTo(1));
        Assert.That(instructions[0], Is.EqualTo(expectedInstruction));
    }

    [Test]
    public void CurlyBracesWithAssignment()
    {
        Lexer lexer = new Lexer("var y = function() { var x = 5; }");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Instruction blockInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Declaration,
                "x"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                5
            )
        );

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Declaration,
                "y"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Function,
                new List<Instruction>(),
                null,
                new List<Instruction>() { blockInstruction }
            )
        );

        Assert.That(instructions.Count, Is.EqualTo(1));
        Assert.That(instructions[0], Is.EqualTo(expectedInstruction));
    }

    [Test]
    public void ChainedBlocks()
    {
        Lexer lexer = new Lexer("if (true) { var x = 5; } else { var y = 6; }");
        var tokens = lexer.Run();

        Parser parser = new Parser(tokens);
        var instructions = parser.Run();

        Instruction firstBlockInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Declaration,
                "x"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                5
            )
        );

        Instruction secondBlockInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            null,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Declaration,
                "y"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                6
            )
        );

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.If,
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Boolean,
                true
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Else,
                null,
                null,
                new List<Instruction>() { secondBlockInstruction }
            ),
            new List<Instruction>() { firstBlockInstruction }
        );

        Assert.That(instructions.Count, Is.EqualTo(1));
        Assert.That(instructions[0], Is.EqualTo(expectedInstruction));
    }
}