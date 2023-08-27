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

namespace BattleScript.Tests;

public class ParserTests
{
    [Test]
    public void Variables()
    {
        string contents = LoadFile("variables.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new NumberInstruction(15)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new StringInstruction("1234")
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new StringInstruction("2345")
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new BooleanInstruction(true)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new VariableInstruction("a")
            )
        };
        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Operators()
    {
        string contents = LoadFile("operators.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new OperationInstruction(
                    "+",
                    new NumberInstruction(5),
                    new NumberInstruction(6)
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new OperationInstruction(
                    "*",
                    new NumberInstruction(7),
                    new NumberInstruction(8)
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new OperationInstruction(
                    "==",
                    new NumberInstruction(3),
                    new NumberInstruction(5)
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new OperationInstruction(
                    ">",
                    new NumberInstruction(4),
                    new NumberInstruction(3)
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new OperationInstruction(
                    "<",
                    new NumberInstruction(5),
                    new NumberInstruction(2)
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Arrays()
    {
        string contents = LoadFile("arrays.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new SquareBracesInstruction(
                    new List<Instruction>() {
                        new NumberInstruction(1),
                        new OperationInstruction(
                            "+",
                            new NumberInstruction(1),
                            new NumberInstruction(1)
                        ),
                        new NumberInstruction(3)
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new SquareBracesInstruction(
                    new List<Instruction>() {
                        new StringInstruction("1234"),
                        new StringInstruction("2345")
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new OperationInstruction(
                                "+",
                                new NumberInstruction(0),
                                new NumberInstruction(2)
                            )
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new NumberInstruction(1)
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new SquareBracesInstruction(
                    new List<Instruction>() {
                        new VariableInstruction("z"),
                        new VariableInstruction("a")
                    }
                )
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new NumberInstruction(1)
                        }
                    )
                ),
                new NumberInstruction(5)
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new VariableInstruction("a")
                        }
                    )
                ),
                new NumberInstruction(6)
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Dictionaries()
    {
        string contents = LoadFile("dictionaries.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new DictionaryInstruction(
                    new List<Instruction>() {
                        new NumberInstruction(1),
                        new StringInstruction("asdf"),
                        new StringInstruction("qwer"),
                        new OperationInstruction(
                            "+",
                            new NumberInstruction(3),
                            new NumberInstruction(2)
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("qwer"),
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("qwer")
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new DictionaryInstruction(
                    new List<Instruction>() {
                        new NumberInstruction(5),
                        new OperationInstruction(
                            "+",
                            new NumberInstruction(4),
                            new NumberInstruction(5)
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new VariableInstruction("y")
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new NumberInstruction(1)
                        }
                    )
                ),
                new StringInstruction("sdfg")
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new VariableInstruction("y")
                        }
                    )
                ),
                new NumberInstruction(10)
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void If()
    {
        string contents = LoadFile("if.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new NumberInstruction(5)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new NumberInstruction(3)
            ),
            new IfInstruction(
                new OperationInstruction(
                    "==",
                    new VariableInstruction("x"),
                    new NumberInstruction(5)
                ),
                new List<Instruction>() {
                    new AssignmentInstruction(
                        new VariableInstruction("y"),
                        new NumberInstruction(6)
                    )
                }
            ),
            new IfInstruction(
                new OperationInstruction(
                    "==",
                    new VariableInstruction("y"),
                    new NumberInstruction(3)
                ),
                new List<Instruction>() {
                    new AssignmentInstruction(
                        new VariableInstruction("x"),
                        new NumberInstruction(1)
                    )
                }
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void IfElse()
    {
        string contents = LoadFile("ifelse.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new NumberInstruction(5)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new NumberInstruction(3)
            ),
            new IfInstruction(
                new OperationInstruction(
                    "==",
                    new VariableInstruction("x"),
                    new NumberInstruction(3)
                ),
                new List<Instruction>() {
                    new AssignmentInstruction(
                        new VariableInstruction("y"),
                        new NumberInstruction(6)
                    )
                },
                new ElseInstruction(
                    Consts.InstructionTypes.Else,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new VariableInstruction("x"),
                            new NumberInstruction(3)
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new NumberInstruction(2)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new NumberInstruction(1)
            ),
            new IfInstruction(
                new OperationInstruction(
                    "==",
                    new VariableInstruction("z"),
                    new NumberInstruction(5)
                ),
                new List<Instruction>() {
                    new AssignmentInstruction(
                        new VariableInstruction("a"),
                        new NumberInstruction(6)
                    )
                },
                new ElseInstruction(
                    new OperationInstruction(
                        "==",
                        new VariableInstruction("z"),
                        new NumberInstruction(2)
                    ),
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new VariableInstruction("a"),
                            new NumberInstruction(5)
                        )
                    },
                    new ElseInstruction(
                        null,
                        new List<Instruction>() {
                            new AssignmentInstruction(
                                new VariableInstruction("a"),
                                new NumberInstruction(4)
                            )
                        }
                    )
                )
            ),
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void While()
    {
        string contents = LoadFile("while.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new NumberInstruction(0)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new NumberInstruction(3)
            ),
            new WhileInstruction(
                new OperationInstruction(
                    "<",
                    new VariableInstruction("z"),
                    new NumberInstruction(8)
                ),
                new List<Instruction>() {
                    new AssignmentInstruction(
                        new VariableInstruction("a"),
                        new OperationInstruction(
                            "+",
                            new VariableInstruction("a"),
                            new NumberInstruction(1)
                        )
                    ),
                    new AssignmentInstruction(
                        new VariableInstruction("z"),
                        new OperationInstruction(
                            "+",
                            new VariableInstruction("z"),
                            new NumberInstruction(1)
                        )
                    )
                }
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Functions()
    {
        string contents = LoadFile("functions.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("my_function"),
                new FunctionInstruction(
                    new List<Instruction>(),
                    new List<Instruction>() {
                        new ReturnInstruction(
                            new NumberInstruction(5)
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new VariableInstruction(
                    "my_function",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("my_other_function"),
                new FunctionInstruction(
                    new List<Instruction>() {
                        new VariableInstruction("my_variable")
                    },
                    new List<Instruction>() {
                        new ReturnInstruction(
                            new OperationInstruction(
                                "+",
                                new VariableInstruction("my_variable"),
                                new NumberInstruction(5)
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new VariableInstruction(
                    "my_other_function",
                    new ParensInstruction(
                        new List<Instruction>() {
                            new NumberInstruction(3)
                        }
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Classes()
    {
        string contents = LoadFile("classes.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    Consts.InstructionTypes.Class,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("a"),
                            new NumberInstruction(5)
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new VariableInstruction(
                    "Class1",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("a")
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("a")
                        }
                    )
                ),
                new NumberInstruction(10)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("a")
                        }
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Methods()
    {
        string contents = LoadFile("methods.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    null,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("a"),
                            new NumberInstruction(5)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new VariableInstruction("a")
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new VariableInstruction(
                    "Class1",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("a")
                        }
                    )
                ),
                new NumberInstruction(10)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("z"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Inheritance()
    {
        string contents = LoadFile("inheritance.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    null,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("b"),
                            new NumberInstruction(6)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("d"),
                            new NumberInstruction(3)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new VariableInstruction("b")
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("Class2"),
                new ClassInstruction(
                    new VariableInstruction("Class1"),
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("c"),
                            new NumberInstruction(9)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("d"),
                            new NumberInstruction(12)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new OperationInstruction(
                                            "+",
                                            new VariableInstruction("b"),
                                            new VariableInstruction("c")
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new VariableInstruction(
                    "Class2",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("c"),
                new VariableInstruction(
                    "b",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_other_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "b",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("b")
                        }
                    )
                ),
                new NumberInstruction(9)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("d"),
                new VariableInstruction(
                    "b",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Self()
    {
        string contents = LoadFile("self.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    null,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("b"),
                            new NumberInstruction(8)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new AssignmentInstruction(
                                        new DeclarationInstruction("b"),
                                        new NumberInstruction(3)
                                    ),
                                    new ReturnInstruction(
                                        new SelfInstruction(
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("b")
                                                }
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("x"),
                new VariableInstruction(
                    "Class1",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("y"),
                new VariableInstruction(
                    "x",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Super()
    {
        string contents = LoadFile("super.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    null,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new NumberInstruction(9)
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("Class2"),
                new ClassInstruction(
                    new VariableInstruction("Class1"),
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new NumberInstruction(4)
                                    )
                                }
                            )
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new SuperInstruction(
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("my_function")
                                                },
                                                new ParensInstruction(new List<Instruction>())
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new VariableInstruction(
                    "Class2",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_other_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void SuperSuper()
    {
        string contents = LoadFile("super_super.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    null,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new (
                                        Consts.InstructionTypes.Return,
                                        new NumberInstruction(9)
                                    )
                                }
                            )
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new NumberInstruction(3)
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("Class2"),
                new ClassInstruction(
                    new VariableInstruction("Class1"),
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new NumberInstruction(4)
                                    )
                                }
                            )
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new SuperInstruction(
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("my_other_function")
                                                },
                                                new ParensInstruction(new List<Instruction>())
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("Class3"),
                new ClassInstruction(
                    new VariableInstruction("Class2"),
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new NumberInstruction(2)
                                    )
                                }
                            )
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new SuperInstruction(
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("my_other_function")
                                                },
                                                new ParensInstruction(new List<Instruction>())
                                            )
                                        )
                                    )
                                }
                            )
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new SuperInstruction(
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("super")
                                                },
                                                new SquareBracesInstruction(
                                                    new List<Instruction>() {
                                                        new StringInstruction("my_function")
                                                    },
                                                    new ParensInstruction(new List<Instruction>())
                                                )
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new VariableInstruction(
                    "Class3",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_other_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("c"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_other_other_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void SelfSuper()
    {
        string contents = LoadFile("self_super.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    null,
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("x"),
                            new NumberInstruction(5)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new SelfInstruction(
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("x")
                                                }
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("Class2"),
                new ClassInstruction(
                    new VariableInstruction("Class1"),
                    new List<Instruction>() {
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new SuperInstruction (
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("my_function")
                                                },
                                                new ParensInstruction(new List<Instruction>())
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new VariableInstruction(
                    "Class2",
                    new ParensInstruction(new List<Instruction>())
                )
            ),
            new AssignmentInstruction(
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("x")
                        }
                    )
                ),
                new NumberInstruction(10)
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_other_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

    [Test]
    public void Constructors()
    {
        string contents = LoadFile("constructors.btl");
        Lexer lexer = new Lexer();
        var tokens = lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        List<Instruction> expected = new List<Instruction>() {
            new AssignmentInstruction(
                new DeclarationInstruction("Class1"),
                new ClassInstruction(
                    null,
                    new List<Instruction>() {
                        new ConstructorInstruction(
                            new List<Instruction>() {
                                new VariableInstruction("value")
                            },
                            new List<Instruction>() {
                                new AssignmentInstruction(
                                    new VariableInstruction("b"),
                                    new OperationInstruction(
                                        "+",
                                        new VariableInstruction("value"),
                                        new NumberInstruction(3)
                                    )
                                )
                            }
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("a"),
                            new NumberInstruction(4)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("b"),
                            new NumberInstruction(7)
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new VariableInstruction("a")
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("Class2"),
                new ClassInstruction(
                    new VariableInstruction("Class1"),
                    new List<Instruction>() {
                        new ConstructorInstruction(
                            new List<Instruction>() {
                                new VariableInstruction("value")
                            },
                            new List<Instruction>() {
                                new SuperInstruction(
                                    new ParensInstruction(
                                        new List<Instruction>() {
                                            new VariableInstruction("value")
                                        }
                                    )
                                ),
                                new AssignmentInstruction(
                                    new VariableInstruction("a"),
                                    new VariableInstruction("value")
                                )
                            }
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new Instruction (Consts.InstructionTypes.Variable, "b")
                                    )
                                }
                            )
                        ),
                        new AssignmentInstruction(
                            new DeclarationInstruction("my_other_function"),
                            new FunctionInstruction(
                                new List<ScopeVariable>(),
                                new List<Instruction>() {
                                    new ReturnInstruction(
                                        new SuperInstruction (
                                            new SquareBracesInstruction(
                                                new List<Instruction>() {
                                                    new StringInstruction("my_function")
                                                },
                                                new ParensInstruction(new List<Instruction>())
                                            )
                                        )
                                    )
                                }
                            )
                        )
                    }
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("a"),
                new VariableInstruction(
                    "Class2",
                    new ParensInstruction(
                        new List<Instruction>() {
                            new NumberInstruction(8)
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("b"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("a")
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("c"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("b")
                        }
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("d"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            ),
            new AssignmentInstruction(
                new DeclarationInstruction("e"),
                new VariableInstruction(
                    "a",
                    new SquareBracesInstruction(
                        new List<Instruction>() {
                            new StringInstruction("my_other_function")
                        },
                        new ParensInstruction(new List<Instruction>())
                    )
                )
            )
        };

        Assertions.AssertInstructions(instructions, expected);
    }

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