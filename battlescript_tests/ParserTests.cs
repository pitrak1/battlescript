using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using BattleScript.Core;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.InterpreterNS;
using System.Diagnostics;

namespace BattleScript.Tests;

public class ParserTests
{
    // [Test]
    // public void Variables()
    // {
    //     string contents = LoadFile("variables.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("x"),
    //             new NumberInstruction(15)
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("y"),
    //             new StringInstruction("1234")
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("z"),
    //             new StringInstruction("2345")
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("a"),
    //             new BooleanInstruction(true)
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("b"),
    //             new VariableInstruction("a")
    //         )
    //     };
    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void Operators()
    // {
    //     string contents = LoadFile("operators.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("x"),
    //             new OperationInstruction(
    //                 "+",
    //                 new NumberInstruction(5),
    //                 new NumberInstruction(6)
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("y"),
    //             new OperationInstruction(
    //                 "*",
    //                 new NumberInstruction(7),
    //                 new NumberInstruction(8)
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("z"),
    //             new OperationInstruction(
    //                 "==",
    //                 new NumberInstruction(3),
    //                 new NumberInstruction(5)
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("a"),
    //             new OperationInstruction(
    //                 ">",
    //                 new NumberInstruction(4),
    //                 new NumberInstruction(3)
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("b"),
    //             new OperationInstruction(
    //                 "<",
    //                 new NumberInstruction(5),
    //                 new NumberInstruction(2)
    //             )
    //         )
    //     };

    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void Arrays()
    // {
    //     string contents = LoadFile("arrays.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("x"),
    //             new SquareBracesInstruction(
    //                 new List<Instruction>() {
    //                     new NumberInstruction(1),
    //                     new OperationInstruction(
    //                         "+",
    //                         new NumberInstruction(1),
    //                         new NumberInstruction(1)
    //                     ),
    //                     new NumberInstruction(3)
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("y"),
    //             new SquareBracesInstruction(
    //                 new List<Instruction>() {
    //                     new StringInstruction("1234"),
    //                     new StringInstruction("2345")
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("z"),
    //             new VariableInstruction(
    //                 "x",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new OperationInstruction(
    //                             "+",
    //                             new NumberInstruction(0),
    //                             new NumberInstruction(2)
    //                         )
    //                     }
    //                 )
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("a"),
    //             new VariableInstruction(
    //                 "x",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new NumberInstruction(1)
    //                     }
    //                 )
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("b"),
    //             new SquareBracesInstruction(
    //                 new List<Instruction>() {
    //                     new VariableInstruction("z"),
    //                     new VariableInstruction("a")
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new VariableInstruction(
    //                 "x",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new NumberInstruction(1)
    //                     }
    //                 )
    //             ),
    //             new NumberInstruction(5)
    //         ),
    //         new AssignmentInstruction(
    //             new VariableInstruction(
    //                 "x",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new VariableInstruction("a")
    //                     }
    //                 )
    //             ),
    //             new NumberInstruction(6)
    //         )
    //     };

    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void Dictionaries()
    // {
    //     string contents = LoadFile("dictionaries.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("x"),
    //             new DictionaryInstruction(
    //                 new List<Instruction>() {
    //                     new NumberInstruction(1),
    //                     new StringInstruction("asdf"),
    //                     new StringInstruction("qwer"),
    //                     new OperationInstruction(
    //                         "+",
    //                         new NumberInstruction(3),
    //                         new NumberInstruction(2)
    //                     )
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("y"),
    //             new VariableInstruction(
    //                 "x",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new StringInstruction("qwer"),
    //                     }
    //                 )
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("z"),
    //             new VariableInstruction(
    //                 "x",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new StringInstruction("qwer")
    //                     }
    //                 )
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("a"),
    //             new DictionaryInstruction(
    //                 new List<Instruction>() {
    //                     new NumberInstruction(5),
    //                     new OperationInstruction(
    //                         "+",
    //                         new NumberInstruction(4),
    //                         new NumberInstruction(5)
    //                     )
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("b"),
    //             new VariableInstruction(
    //                 "a",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new VariableInstruction("y")
    //                     }
    //                 )
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new VariableInstruction(
    //                 "x",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new NumberInstruction(1)
    //                     }
    //                 )
    //             ),
    //             new StringInstruction("sdfg")
    //         ),
    //         new AssignmentInstruction(
    //             new VariableInstruction(
    //                 "a",
    //                 new SquareBracesInstruction(
    //                     new List<Instruction>() {
    //                         new VariableInstruction("y")
    //                     }
    //                 )
    //             ),
    //             new NumberInstruction(10)
    //         )
    //     };

    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void If()
    // {
    //     string contents = LoadFile("if.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("x"),
    //             new NumberInstruction(5)
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("y"),
    //             new NumberInstruction(3)
    //         ),
    //         new IfInstruction(
    //             new OperationInstruction(
    //                 "==",
    //                 new VariableInstruction("x"),
    //                 new NumberInstruction(5)
    //             ),
    //             new List<Instruction>() {
    //                 new AssignmentInstruction(
    //                     new VariableInstruction("y"),
    //                     new NumberInstruction(6)
    //                 )
    //             }
    //         ),
    //         new IfInstruction(
    //             new OperationInstruction(
    //                 "==",
    //                 new VariableInstruction("y"),
    //                 new NumberInstruction(3)
    //             ),
    //             new List<Instruction>() {
    //                 new AssignmentInstruction(
    //                     new VariableInstruction("x"),
    //                     new NumberInstruction(1)
    //                 )
    //             }
    //         )
    //     };

    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void IfElse()
    // {
    //     string contents = LoadFile("ifelse.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("x"),
    //             new NumberInstruction(5)
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("y"),
    //             new NumberInstruction(3)
    //         ),
    //         new IfInstruction(
    //             new OperationInstruction(
    //                 "==",
    //                 new VariableInstruction("x"),
    //                 new NumberInstruction(3)
    //             ),
    //             new List<Instruction>() {
    //                 new AssignmentInstruction(
    //                     new VariableInstruction("y"),
    //                     new NumberInstruction(6)
    //                 )
    //             },
    //             new ElseInstruction(
    //                 null,
    //                 new List<Instruction>() {
    //                     new AssignmentInstruction(
    //                         new VariableInstruction("x"),
    //                         new NumberInstruction(3)
    //                     )
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("z"),
    //             new NumberInstruction(2)
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("a"),
    //             new NumberInstruction(1)
    //         ),
    //         new IfInstruction(
    //             new OperationInstruction(
    //                 "==",
    //                 new VariableInstruction("z"),
    //                 new NumberInstruction(5)
    //             ),
    //             new List<Instruction>() {
    //                 new AssignmentInstruction(
    //                     new VariableInstruction("a"),
    //                     new NumberInstruction(6)
    //                 )
    //             },
    //             new ElseInstruction(
    //                 new OperationInstruction(
    //                     "==",
    //                     new VariableInstruction("z"),
    //                     new NumberInstruction(2)
    //                 ),
    //                 new List<Instruction>() {
    //                     new AssignmentInstruction(
    //                         new VariableInstruction("a"),
    //                         new NumberInstruction(5)
    //                     )
    //                 },
    //                 new ElseInstruction(
    //                     null,
    //                     new List<Instruction>() {
    //                         new AssignmentInstruction(
    //                             new VariableInstruction("a"),
    //                             new NumberInstruction(4)
    //                         )
    //                     }
    //                 )
    //             )
    //         ),
    //     };

    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void While()
    // {
    //     string contents = LoadFile("while.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("z"),
    //             new NumberInstruction(0)
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("a"),
    //             new NumberInstruction(3)
    //         ),
    //         new WhileInstruction(
    //             new OperationInstruction(
    //                 "<",
    //                 new VariableInstruction("z"),
    //                 new NumberInstruction(8)
    //             ),
    //             new List<Instruction>() {
    //                 new AssignmentInstruction(
    //                     new VariableInstruction("a"),
    //                     new OperationInstruction(
    //                         "+",
    //                         new VariableInstruction("a"),
    //                         new NumberInstruction(1)
    //                     )
    //                 ),
    //                 new AssignmentInstruction(
    //                     new VariableInstruction("z"),
    //                     new OperationInstruction(
    //                         "+",
    //                         new VariableInstruction("z"),
    //                         new NumberInstruction(1)
    //                     )
    //                 )
    //             }
    //         )
    //     };

    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void Functions()
    // {
    //     string contents = LoadFile("functions.btl");

    //     Lexer lexer = new Lexer(contents);
    //     var tokens = lexer.Run();

    //     Parser parser = new Parser(tokens);
    //     var instructions = parser.Run();

    //     List<Instruction> expected = new List<Instruction>() {
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("my_function"),
    //             new FunctionInstruction(
    //                 new List<Instruction>(),
    //                 new List<Instruction>() {
    //                     new ReturnInstruction(
    //                         new NumberInstruction(5)
    //                     )
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("x"),
    //             new VariableInstruction(
    //                 "my_function",
    //                 new ParensInstruction(new List<Instruction>())
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("my_other_function"),
    //             new FunctionInstruction(
    //                 new List<Instruction>() {
    //                     new VariableInstruction("my_variable")
    //                 },
    //                 new List<Instruction>() {
    //                     new ReturnInstruction(
    //                         new OperationInstruction(
    //                             "+",
    //                             new VariableInstruction("my_variable"),
    //                             new NumberInstruction(5)
    //                         )
    //                     )
    //                 }
    //             )
    //         ),
    //         new AssignmentInstruction(
    //             new DeclarationInstruction("y"),
    //             new VariableInstruction(
    //                 "my_other_function",
    //                 new ParensInstruction(
    //                     new List<Instruction>() {
    //                         new NumberInstruction(3)
    //                     }
    //                 )
    //             )
    //         )
    //     };

    //     Assertions.AssertInstructions(instructions, expected);
    // }

    // [Test]
    // public void ConstVariables() {
    //     string contents = LoadFile("const_variables.btl");
    //     var tokens = Lexer.Run(contents);
    //     var instructions = Parser.Run(tokens);
    //
    //     List<Instruction> expected = new List<Instruction>() {
    //         new (
    //             Consts.InstructionTypes.Assignment,
    //             null,
    //             new ConstDeclarationInstruction("x"),
    //             new NumberInstruction(5)
    //         ),
    //         new (
    //             Consts.InstructionTypes.Assignment,
    //             null,
    //             new DeclarationInstruction("y"),
    //             new NumberInstruction(3)
    //         ),
    //         new (
    //             Consts.InstructionTypes.Assignment,
    //             null,
    //             new ConstDeclarationInstruction("z"),
    //             new Instruction(
    //                 Consts.InstructionTypes.Operation,
    //                 "+",
    //                 new VariableInstruction("x"),
    //                 new VariableInstruction("y")
    //             )
    //         )
    //     };
    //     
    //     Assertions.AssertInstructions(instructions, expected);
    // }

    private string LoadFile(string filename)
    {
        return File.ReadAllText($"/Users/nickpitrak/Desktop/battlescript/battlescript_tests/TestFiles/{filename}");
    }
}