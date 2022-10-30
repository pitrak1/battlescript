using System.Diagnostics;

namespace BattleScript; 

public class Parser {
    public static List<Instruction> Run(List<Token> tokens) {
        List<Token> currentTokenSet = new List<Token>();
        List<Instruction> instructions = new List<Instruction>();

        List<List<Instruction>> scopes = new List<List<Instruction>>();
        scopes.Add(instructions);

        for (int i = 0; i < tokens.Count; i++) {
            Token token = tokens[i];
            if (token.Type == Consts.TokenTypes.Semicolon) {
                Instruction parsedInstruction = ParseTokenSet(currentTokenSet);
                scopes[^1].Add(parsedInstruction);
                currentTokenSet = new List<Token>();
            }
            else if (token.Value == "{" && ParserUtilities.BlockContainsSemicolon(tokens, i)) {
                Instruction instruction = ParseTokenSet(currentTokenSet);

                if (instruction.Type == Consts.InstructionTypes.Else) {
                    Instruction mostRecentInstruction = scopes[^1][^1];
                    while (mostRecentInstruction.Next is not null) {
                        mostRecentInstruction = mostRecentInstruction.Next;
                    }
                    mostRecentInstruction.Next = instruction;
                }
                else {
                    scopes[^1].Add(instruction);
                }
                
                // If this is an assignment, it means that it is a dictionary or class.  If it's not, it's an if/else/while.
                if (instruction.Type == Consts.InstructionTypes.Assignment) {
                    scopes.Add(instruction.Right.Instructions);
                }
                else {
                    scopes.Add(instruction.Instructions);
                }
                currentTokenSet = new List<Token>();
            }
            else if (token.Value == "}" && ParserUtilities.BlockContainsSemicolonReverse(tokens, i)) {
                currentTokenSet = new List<Token>();
                scopes.RemoveAt(scopes.Count - 1);
            }
            else {
                currentTokenSet.Add(token);
            }
        }

        return instructions;
    }

    private static Instruction ParseTokenSet(List<Token> currentTokenSet) {
        int assignmentOperatorIndex = ParserUtilities.GetAssignmentOperatorIndex(currentTokenSet);
        int mathematicalOperatorIndex = ParserUtilities.GetMathematicalOperatorIndex(currentTokenSet);

        if (assignmentOperatorIndex != -1) {
            return HandleAssignment(currentTokenSet, assignmentOperatorIndex);
        } 
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Separator) {
            return HandleSeparator(currentTokenSet);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Keyword) {
            return HandleKeyword(currentTokenSet);
        } 
        else if (mathematicalOperatorIndex != -1) {
            return HandleOperation(currentTokenSet, mathematicalOperatorIndex);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Identifier) {
            return HandleIdentifier(currentTokenSet);
        }
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Number) {
            return HandleNumber(currentTokenSet);
        } 
        else if (currentTokenSet[0].Type == Consts.TokenTypes.String) {
            return HandleString(currentTokenSet);
        } 
        else if (currentTokenSet[0].Type == Consts.TokenTypes.Boolean) {
            return HandleBoolean(currentTokenSet);
        } 
        else {
            return new Instruction();
        }
    }

    private static Instruction HandleAssignment(List<Token> tokens, int assignmentOperatorIndex) {
        Instruction left = ParseTokenSet(tokens.GetRange(0, assignmentOperatorIndex));
        Instruction right = ParseTokenSet(tokens.GetRange(assignmentOperatorIndex + 1, tokens.Count - assignmentOperatorIndex - 1));

        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Assignment;
        instruction.Left = left;
        instruction.Right = right;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleSeparator(List<Token> tokens) {
        switch (tokens[0].Value) {
            case "[":
                return HandleSquareBraces(tokens);
            case "{":
                return HandleCurlyBraces(tokens);
            case "(":
                return HandleParens(tokens);
            case ".":
                return HandleMember(tokens);
            default:
                throw new SystemException("Invalid separator found");
        }
    }
    
    private static Instruction HandleSquareBraces(List<Token> tokens) {
        List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>() {","});
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries) {
            values.Add(ParseTokenSet(entry));
        }
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.SquareBraces;
        instruction.Value = values;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleCurlyBraces(List<Token> tokens) {
        List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>() {",", ":"});
        Debug.Assert(entries.Count % 2 == 0);
        
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries) {
            values.Add(ParseTokenSet(entry));
        }
        
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Dictionary;
        instruction.Value = values;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleParens(List<Token> tokens) {
        List<List<Token>> entries = ParserUtilities.ParseUntilMatchingSeparator(tokens, new List<string>() {","});
        List<Instruction> values = new List<Instruction>();
        foreach (List<Token> entry in entries) {
            if (entry.Count > 0) {
                values.Add(ParseTokenSet(entry));
            }
        }
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Parens;
        instruction.Value = values;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleMember(List<Token> tokens) {
        Instruction property = new Instruction();
        property.Type = Consts.InstructionTypes.String;
        property.Value = tokens[1].Value;
        property.Column = tokens[1].Column;
        property.Line = tokens[1].Line;

        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.SquareBraces;
        instruction.Value = new List<Instruction>() {property};
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleKeyword(List<Token> tokens) {
        switch (tokens[0].Value) {
            case "var":
                return HandleVar(tokens);
            // case "export":
            //     return HandleExport(tokens);
            case "if":
                return HandleIf(tokens);
            case "else":
                return HandleElse(tokens);
            case "while":
                return HandleWhile(tokens);
            case "function":
                return HandleFunction(tokens);
            case "return":
                return HandleReturn(tokens);
            // case "import":
            //     return HandleImport(tokens);
            // case "class":
            //     return HandleClass(tokens);
            // case "constructor":
            //     return HandleConstructor(tokens);
            default:
                return new Instruction();
        }
    }

    private static Instruction HandleVar(List<Token> tokens) {
        Debug.Assert(tokens.Count == 2);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Keyword);
        Debug.Assert(tokens[0].Value == "var");
        Debug.Assert(tokens[1].Type == Consts.TokenTypes.Identifier);

        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Declaration;
        instruction.Value = tokens[1].Value;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleIf(List<Token> tokens) {
        // exclude the if itself and the start and ending parens
        Instruction condition = ParseTokenSet(tokens.GetRange(2, tokens.Count - 3));
        
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.If;
        instruction.Value = condition;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleElse(List<Token> tokens) {
        Instruction instruction = new Instruction();
        
        // this is an else if block
        if (tokens.Count > 1) {
            Debug.Assert(tokens[1].Value == "if");
            // exclude the else if and the start and ending parens
            Instruction condition = ParseTokenSet(tokens.GetRange(3, tokens.Count - 4));
            instruction.Value = condition;
        }
        
        instruction.Type = Consts.InstructionTypes.Else;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
    
    private static Instruction HandleWhile(List<Token> tokens) {
        // exclude the while itself and the start and ending parens
        Instruction condition = ParseTokenSet(tokens.GetRange(2, tokens.Count - 3));
        
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.While;
        instruction.Value = condition;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
    
    private static Instruction HandleFunction(List<Token> tokens) {
        // exclude the function itself and the start and ending parens
        List<Token> argTokens = tokens.GetRange(1, tokens.Count - 1);
        List<List<Token>> tokenizedArgs =
            ParserUtilities.ParseUntilMatchingSeparator(argTokens, new List<string>() { "," });

        List<Instruction> instructionArgs = new List<Instruction>();
        foreach (List<Token> arg in tokenizedArgs) {
            if (arg.Count > 0) {
                Instruction instructionArg = ParseTokenSet(arg);
                Debug.Assert(instructionArg.Type == Consts.InstructionTypes.Variable);
                instructionArgs.Add(instructionArg);
            }
        }
        
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Function;
        instruction.Value = instructionArgs;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
    
    private static Instruction HandleReturn(List<Token> tokens) {
        Instruction returnValue = ParseTokenSet(tokens.GetRange(1, tokens.Count - 1));
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Return;
        instruction.Value = returnValue;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
    
    private static Instruction HandleOperation(List<Token> tokens, int mathematicalOperatorIndex) {
        Instruction left = ParseTokenSet(tokens.GetRange(0, mathematicalOperatorIndex));
        Instruction right = ParseTokenSet(tokens.GetRange(mathematicalOperatorIndex + 1, tokens.Count - mathematicalOperatorIndex - 1));

        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Operation;
        instruction.Left = left;
        instruction.Right = right;
        instruction.Value = tokens[mathematicalOperatorIndex].Value;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleIdentifier(List<Token> tokens) {
        Instruction next = null;
        if (tokens.Count > 1) {
            next = ParseTokenSet(tokens.GetRange(1, tokens.Count - 1));
        }
        
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Variable;
        instruction.Value = tokens[0].Value;
        instruction.Next = next;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleNumber(List<Token> tokens) {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Number);

        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Number;
        instruction.Value = int.Parse(tokens[0].Value);
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
    
    private static Instruction HandleString(List<Token> tokens) {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.String);

        string trimmedValue = tokens[0].Value.Substring(1, tokens[0].Value.Length - 2);
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.String;
        instruction.Value = trimmedValue;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
    
    private static Instruction HandleBoolean(List<Token> tokens) {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Boolean);

        bool value = tokens[0].Value == "true";
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Boolean;
        instruction.Value = value;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
}