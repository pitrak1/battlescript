using BattleScript.Core;
using BattleScript.Instructions;

namespace BattleScript.InstructionParserTests;

public class InstructionParserTests
{
    [TestFixture]
    public class VariableTypes
    {
        [Test]
        public void Boolean()
        {
            Lexer lexer = new Lexer("true");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Boolean,
                true
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void IntegerNumber()
        {
            Lexer lexer = new Lexer("15");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                15
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void FloatingPointNumber()
        {
            Lexer lexer = new Lexer("15.45");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                15.45
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void SingleQuoteString()
        {
            Lexer lexer = new Lexer("'asdf'");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.String,
                "asdf"
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void DoubleQuoteString()
        {
            Lexer lexer = new Lexer("\"asdf\"");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.String,
                "asdf"
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }

    [Test]
    public void Declaration()
    {
        Lexer lexer = new Lexer("var x");
        var tokens = lexer.Run();

        InstructionParser instructionParser = new InstructionParser();
        Instruction instruction = instructionParser.Run(tokens);

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Declaration,
            "x"
        );

        Assert.That(instruction, Is.EqualTo(expectedInstruction));
    }

    [Test]
    public void Assignment()
    {
        Lexer lexer = new Lexer("x = 15");
        var tokens = lexer.Run();

        InstructionParser instructionParser = new InstructionParser();
        Instruction instruction = instructionParser.Run(tokens);

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Assignment,
            null,
            null,
            new List<Instruction>(),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Variable,
                "x"
            ),
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.Number,
                15
            )
        );

        Assert.That(instruction, Is.EqualTo(expectedInstruction));
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

    [TestFixture]
    public class IfElse
    {
        [Test]
        public void If()
        {
            Lexer lexer = new Lexer("if (false)");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.If,
                new Instruction(null, null, Consts.InstructionTypes.Boolean, false)
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void ElseIf()
        {
            Lexer lexer = new Lexer("else if (false)");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Else,
                new Instruction(null, null, Consts.InstructionTypes.Boolean, false)
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void Else()
        {
            Lexer lexer = new Lexer("else");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Else
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }

    [Test]
    public void While()
    {
        Lexer lexer = new Lexer("while (false)");
        var tokens = lexer.Run();

        InstructionParser instructionParser = new InstructionParser();
        Instruction instruction = instructionParser.Run(tokens);

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.While,
            new Instruction(null, null, Consts.InstructionTypes.Boolean, false)
        );

        Assert.That(instruction, Is.EqualTo(expectedInstruction));
    }

    [TestFixture]
    public class Function
    {
        [Test]
        public void NoArguments()
        {
            Lexer lexer = new Lexer("function ()");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Function,
                new List<Instruction>()
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithArguments()
        {
            Lexer lexer = new Lexer("function (x, y, z)");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Function,
                new List<Instruction>() {
                    new Instruction(null, null, Consts.InstructionTypes.Variable, "x"),
                    new Instruction(null, null, Consts.InstructionTypes.Variable, "y"),
                    new Instruction(null, null, Consts.InstructionTypes.Variable, "z")
                }
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }

    [TestFixture]
    public class Return
    {
        [Test]
        public void NoValue()
        {
            Lexer lexer = new Lexer("return");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Return
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithValue()
        {
            Lexer lexer = new Lexer("return 5");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Return,
                new Instruction(null, null, Consts.InstructionTypes.Number, 5)
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }

    [Test]
    public void Btl()
    {
        Lexer lexer = new Lexer("Btl");
        var tokens = lexer.Run();

        InstructionParser instructionParser = new InstructionParser();
        Instruction instruction = instructionParser.Run(tokens);

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Btl
        );

        Assert.That(instruction, Is.EqualTo(expectedInstruction));
    }

    [TestFixture]
    public class SquareBraces
    {
        [Test]
        public void WithNoValue()
        {
            Lexer lexer = new Lexer("Btl[]");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Btl,
                null,
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.SquareBraces,
                    new List<Instruction>()
                )
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithOneValue()
        {
            Lexer lexer = new Lexer("Btl[5]");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Btl,
                null,
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.SquareBraces,
                    new List<Instruction>() {
                        new Instruction(null, null, Consts.InstructionTypes.Number, 5)
                    }
                )
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithMultipleValues()
        {
            Lexer lexer = new Lexer("Btl[x, y, z]");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Btl,
                null,
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.SquareBraces,
                    new List<Instruction>() {
                        new Instruction(null, null, Consts.InstructionTypes.Variable, "x"),
                        new Instruction(null, null, Consts.InstructionTypes.Variable, "y"),
                        new Instruction(null, null, Consts.InstructionTypes.Variable, "z")
                    }
                )
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }

    [TestFixture]
    public class Parens
    {
        [Test]
        public void WithNoValue()
        {
            Lexer lexer = new Lexer("x()");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Variable,
                "x",
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.Parens,
                    new List<Instruction>()
                )
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithOneValue()
        {
            Lexer lexer = new Lexer("x(5)");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Variable,
                "x",
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.Parens,
                    new List<Instruction>() {
                        new Instruction(null, null, Consts.InstructionTypes.Number, 5)
                    }
                )
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithMultipleValues()
        {
            Lexer lexer = new Lexer("x(x, y, z)");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Variable,
                "x",
                new Instruction(
                    null,
                    null,
                    Consts.InstructionTypes.Parens,
                    new List<Instruction>() {
                        new Instruction(null, null, Consts.InstructionTypes.Variable, "x"),
                        new Instruction(null, null, Consts.InstructionTypes.Variable, "y"),
                        new Instruction(null, null, Consts.InstructionTypes.Variable, "z")
                    }
                )
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }

    [Test]
    public void Member()
    {
        Lexer lexer = new Lexer("x.asdf");
        var tokens = lexer.Run();

        InstructionParser instructionParser = new InstructionParser();
        Instruction instruction = instructionParser.Run(tokens);

        Instruction expectedInstruction = new Instruction(
            null,
            null,
            Consts.InstructionTypes.Variable,
            "x",
            new Instruction(
                null,
                null,
                Consts.InstructionTypes.SquareBraces,
                new List<Instruction>() {
                    new Instruction(null, null, Consts.InstructionTypes.String, "asdf")
                }
            )
        );

        Assert.That(instruction, Is.EqualTo(expectedInstruction));
    }

    [TestFixture]
    public class CurlyBraces
    {
        [Test]
        public void WithNoEntries()
        {
            Lexer lexer = new Lexer("{}");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Dictionary,
                new List<Instruction>()
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithOneEntry()
        {
            Lexer lexer = new Lexer("{x: 5}");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Dictionary,
                new List<Instruction>() {
                    new Instruction(null, null, Consts.InstructionTypes.Variable, "x"),
                    new Instruction(null, null, Consts.InstructionTypes.Number, 5)
                }
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }

        [Test]
        public void WithMultipleEntries()
        {
            Lexer lexer = new Lexer("{x: 5, 'asdf': 15}");
            var tokens = lexer.Run();

            InstructionParser instructionParser = new InstructionParser();
            Instruction instruction = instructionParser.Run(tokens);

            Instruction expectedInstruction = new Instruction(
                null,
                null,
                Consts.InstructionTypes.Dictionary,
                new List<Instruction>() {
                    new Instruction(null, null, Consts.InstructionTypes.Variable, "x"),
                    new Instruction(null, null, Consts.InstructionTypes.Number, 5),
                    new Instruction(null, null, Consts.InstructionTypes.String, "asdf"),
                    new Instruction(null, null, Consts.InstructionTypes.Number, 15)
                }
            );

            Assert.That(instruction, Is.EqualTo(expectedInstruction));
        }
    }
}