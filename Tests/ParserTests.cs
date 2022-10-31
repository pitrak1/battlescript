using System.Runtime.CompilerServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BattleScript.Tests; 

public class ParserTests {
    [Test]
    public void Variables() {
        string contents = LoadFile("variables.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "x"),
                new Instruction(Consts.InstructionTypes.Number, 15)
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "y"),
                new Instruction(Consts.InstructionTypes.String, "1234")
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "z"),
                new Instruction(Consts.InstructionTypes.String, "2345")
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "a"),
                new Instruction(Consts.InstructionTypes.Boolean, true)
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "b"),
                new Instruction(Consts.InstructionTypes.Variable, "a")
            )
        };
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void Operators() {
        string contents = LoadFile("operators.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
    
        List<Instruction> expected = new List<Instruction>() {
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "x"),
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "+",
                    new Instruction(Consts.InstructionTypes.Number, 5),
                    new Instruction(Consts.InstructionTypes.Number, 6)
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "y"),
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "*",
                    new Instruction(Consts.InstructionTypes.Number, 7),
                    new Instruction(Consts.InstructionTypes.Number, 8)
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "z"),
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "==",
                    new Instruction(Consts.InstructionTypes.Number, 3),
                    new Instruction(Consts.InstructionTypes.Number, 5)
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "a"),
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    ">",
                    new Instruction(Consts.InstructionTypes.Number, 4),
                    new Instruction(Consts.InstructionTypes.Number, 3)
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "b"),
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "<",
                    new Instruction(Consts.InstructionTypes.Number, 5),
                    new Instruction(Consts.InstructionTypes.Number, 2)
                )
            )
        };
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void Arrays() {
        string contents = LoadFile("arrays.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
    
        List<Instruction> expected = new List<Instruction>() {
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "x"),
                new Instruction(
                    Consts.InstructionTypes.SquareBraces, 
                    new List<Instruction>() {
                        new (Consts.InstructionTypes.Number, 1),
                        new (
                            Consts.InstructionTypes.Operation, 
                            "+",
                            new Instruction(Consts.InstructionTypes.Number, 1),
                            new Instruction(Consts.InstructionTypes.Number, 1)
                        ),
                        new (Consts.InstructionTypes.Number, 3)
                    }
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "y"),
                new Instruction(
                    Consts.InstructionTypes.SquareBraces,
                    new List<Instruction>() {
                        new (Consts.InstructionTypes.String, "1234"),
                        new (Consts.InstructionTypes.String, "2345")
                    }
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "z"),
                new Instruction(
                    Consts.InstructionTypes.Variable,
                    "x",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces, 
                        new List<Instruction>() {
                            new (
                                Consts.InstructionTypes.Operation, 
                                "+",
                                new Instruction(Consts.InstructionTypes.Number, 0),
                                new Instruction(Consts.InstructionTypes.Number, 2)
                            )
                        }
                    )
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "a"),
                new Instruction(
                    Consts.InstructionTypes.Variable,
                    "x",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces, 
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.Number, 1)
                        }
                    )
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "b"),
                new Instruction(
                    Consts.InstructionTypes.SquareBraces,
                    new List<Instruction>() {
                        new (Consts.InstructionTypes.Variable, "z"),
                        new (Consts.InstructionTypes.Variable, "a")
                    }
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(
                    Consts.InstructionTypes.Variable, 
                    "x",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces,
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.Number, 1)
                        }
                    )
                ),
                new Instruction(Consts.InstructionTypes.Number, 5)
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(
                    Consts.InstructionTypes.Variable, 
                    "x",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces,
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.Variable, "a")
                        }
                    )
                ),
                new Instruction(Consts.InstructionTypes.Number, 6)
            )
        };
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void Dictionaries() {
        string contents = LoadFile("dictionaries.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
    
        List<Instruction> expected = new List<Instruction>() {
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "x"),
                new Instruction(
                    Consts.InstructionTypes.Dictionary, 
                    new List<Instruction>() {
                        new (Consts.InstructionTypes.Number, 1),
                        new (Consts.InstructionTypes.String, "asdf"),
                        new (Consts.InstructionTypes.String, "qwer"),
                        new (Consts.InstructionTypes.Operation,
                            "+",
                            new (Consts.InstructionTypes.Number, 3),
                            new (Consts.InstructionTypes.Number, 2)
                        )
                    }
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "y"),
                new Instruction(
                    Consts.InstructionTypes.Variable,
                    "x",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces,
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.String, "qwer"),
                        }
                    )
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "z"),
                new Instruction(
                    Consts.InstructionTypes.Variable,
                    "x",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces, 
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.String, "qwer")
                        }
                    )
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "a"),
                new Instruction(
                    Consts.InstructionTypes.Dictionary,
                    new List<Instruction>() {
                        new (Consts.InstructionTypes.Number, 5),
                        new (
                            Consts.InstructionTypes.Operation,
                            "+",
                            new (Consts.InstructionTypes.Number, 4),
                            new (Consts.InstructionTypes.Number, 5)
                        )
                    }
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "b"),
                new Instruction(
                    Consts.InstructionTypes.Variable,
                    "a",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces,
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.Variable, "y")
                        }
                    )
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(
                    Consts.InstructionTypes.Variable, 
                    "x",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces,
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.Number, 1)
                        }
                    )
                ),
                new Instruction(Consts.InstructionTypes.String, "sdfg")
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(
                    Consts.InstructionTypes.Variable, 
                    "a",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.SquareBraces,
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.Variable, "y")
                        }
                    )
                ),
                new Instruction(Consts.InstructionTypes.Number, 10)
            )
        };
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void If() {
        string contents = LoadFile("if.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "x"),
                new Instruction (Consts.InstructionTypes.Number, 5)
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "y"),
                new Instruction (Consts.InstructionTypes.Number, 3)
            ),
            new (
                Consts.InstructionTypes.If,
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "==",
                    new Instruction(Consts.InstructionTypes.Variable, "x"),
                    new Instruction(Consts.InstructionTypes.Number, 5)
                ),
                null,
                null,
                null,
                new List<Instruction>() {
                    new (
                        Consts.InstructionTypes.Assignment,
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "y"),
                        new Instruction(Consts.InstructionTypes.Number, 6)
                    )
                }
            ),
            new (
                Consts.InstructionTypes.If,
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "==",
                    new Instruction(Consts.InstructionTypes.Variable, "y"),
                    new Instruction(Consts.InstructionTypes.Number, 3)
                ),
                null,
                null,
                null,
                new List<Instruction>() {
                    new (
                        Consts.InstructionTypes.Assignment,
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "x"),
                        new Instruction(Consts.InstructionTypes.Number, 1)
                    )
                }
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void IfElse() {
        string contents = LoadFile("ifelse.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new(
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "x"),
                new Instruction(Consts.InstructionTypes.Number, 5)
            ),
            new(
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "y"),
                new Instruction(Consts.InstructionTypes.Number, 3)
            ),
            new(
                Consts.InstructionTypes.If,
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "==",
                    new Instruction(Consts.InstructionTypes.Variable, "x"),
                    new Instruction(Consts.InstructionTypes.Number, 3)
                ),
                null,
                null,
                new Instruction(
                    Consts.InstructionTypes.Else,
                    null,
                    null,
                    null,
                    null,
                    new List<Instruction>() {
                        new Instruction(
                            Consts.InstructionTypes.Assignment,
                            null,
                            new Instruction(Consts.InstructionTypes.Variable, "x"),
                            new Instruction(Consts.InstructionTypes.Number, 3)
                        )
                    }
                ),
                new List<Instruction>() {
                    new(
                        Consts.InstructionTypes.Assignment,
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "y"),
                        new Instruction(Consts.InstructionTypes.Number, 6)
                    )
                }
            ),
            new Instruction(
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "z"),
                new Instruction(Consts.InstructionTypes.Number, 2)
            ),
            new Instruction(
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "a"),
                new Instruction(Consts.InstructionTypes.Number, 1)
            ),
            new(
                Consts.InstructionTypes.If,
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "==",
                    new Instruction(Consts.InstructionTypes.Variable, "z"),
                    new Instruction(Consts.InstructionTypes.Number, 5)
                ),
                null,
                null,
                new Instruction(
                    Consts.InstructionTypes.Else,
                    new Instruction(
                        Consts.InstructionTypes.Operation,
                        "==",
                        new Instruction(Consts.InstructionTypes.Variable, "z"),
                        new Instruction(Consts.InstructionTypes.Number, 2)
                    ),
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.Else,
                        null,
                        null,
                        null,
                        null,
                        new List<Instruction>() {
                            new Instruction(
                                Consts.InstructionTypes.Assignment,
                                null,
                                new Instruction(Consts.InstructionTypes.Variable, "a"),
                                new Instruction(Consts.InstructionTypes.Number, 4)
                            )
                        }
                    ),
                    new List<Instruction>() {
                        new Instruction(
                            Consts.InstructionTypes.Assignment,
                            null,
                            new Instruction(Consts.InstructionTypes.Variable, "a"),
                            new Instruction(Consts.InstructionTypes.Number, 5)
                        )
                    }
                ),
                new List<Instruction>() {
                    new(
                        Consts.InstructionTypes.Assignment,
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "a"),
                        new Instruction(Consts.InstructionTypes.Number, 6)
                    )
                }
            ),
        };
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void While() {
        string contents = LoadFile("while.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "z"),
                new Instruction(Consts.InstructionTypes.Number, 0)
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "a"),
                new Instruction(Consts.InstructionTypes.Number, 3)
            ),
            new (
                Consts.InstructionTypes.While,
                new Instruction(
                    Consts.InstructionTypes.Operation,
                    "<",
                    new Instruction(Consts.InstructionTypes.Variable, "z"),
                    new Instruction(Consts.InstructionTypes.Number, 8)
                ),
                null,
                null,
                null,
                new List<Instruction>() {
                    new (
                        Consts.InstructionTypes.Assignment,
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "a"),
                        new Instruction(
                            Consts.InstructionTypes.Operation,
                            "+",
                            new Instruction(Consts.InstructionTypes.Variable, "a"),
                            new Instruction(Consts.InstructionTypes.Number, 1)
                        )
                    ),
                    new (
                        Consts.InstructionTypes.Assignment,
                        null,
                        new Instruction(Consts.InstructionTypes.Variable, "z"),
                        new Instruction(
                            Consts.InstructionTypes.Operation,
                            "+",
                            new Instruction(Consts.InstructionTypes.Variable, "z"),
                            new Instruction(Consts.InstructionTypes.Number, 1)
                        )
                    )
                }
            )
        };
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void Functions() {
        string contents = LoadFile("functions.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "my_function"),
                new Instruction(
                    Consts.InstructionTypes.Function,
                    new List<Instruction>(),
                    null,
                    null,
                    null,
                    new List<Instruction>() {
                        new (
                            Consts.InstructionTypes.Return,
                            new Instruction(Consts.InstructionTypes.Number, 5)
                        )
                    }
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "x"),
                new Instruction(
                    Consts.InstructionTypes.Variable,
                    "my_function",
                    null,
                    null,
                    new Instruction(Consts.InstructionTypes.Parens, new List<Instruction>())
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "my_other_function"),
                new Instruction(
                    Consts.InstructionTypes.Function,
                    new List<Instruction>() {
                        new (Consts.InstructionTypes.Variable, "my_variable")
                    },
                    null,
                    null,
                    null,
                    new List<Instruction>() {
                        new (
                            Consts.InstructionTypes.Return,
                            new Instruction(
                                Consts.InstructionTypes.Operation,
                                "+",
                                new Instruction(Consts.InstructionTypes.Variable, "my_variable"),
                                new Instruction(Consts.InstructionTypes.Number, 5)
                            )
                        )
                    }
                )
            ),
            new (
                Consts.InstructionTypes.Assignment,
                null,
                new Instruction(Consts.InstructionTypes.Declaration, "y"),
                new Instruction(
                    Consts.InstructionTypes.Variable,
                    "my_other_function",
                    null,
                    null,
                    new Instruction(
                        Consts.InstructionTypes.Parens,
                        new List<Instruction>() {
                            new (Consts.InstructionTypes.Number, 3)
                        }
                    )
                )
            )
        };
        
        Assertions.AssertInstructions(instructions, expected);
    }

    private string LoadFile(string filename) {
        return File.ReadAllText($"/Users/nickpitrak/Desktop/BattleScript/TestFiles/{filename}");
    }
}