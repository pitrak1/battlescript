using System.Diagnostics;

namespace BattleScript; 

public class Parser {
    public static List<Instruction> Run(List<Token> tokens) {
        List<Token> currentTokenSet = new List<Token>();
        List<Instruction> instructions = new List<Instruction>();
        
        foreach (Token token in tokens) {
            if (token.Type == Consts.TokenTypes.Semicolon) {
                Instruction parsedInstruction = ParseTokenSet(currentTokenSet);
                instructions.Add(parsedInstruction);
                currentTokenSet = new List<Token>();
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
        if (tokens[0].Value == "[") {
            return HandleSquareBraces(tokens);
        }
        else {
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
        instruction.InstructionListValue = values;
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
            // case "if":
            //     return HandleIf(tokens);
            // case "else":
            //     return HandleElse(tokens);
            // case "while":
            //     return HandleWhile(tokens);
            // case "function":
            //     return HandleFunction(tokens);
            // case "return":
            //     return HandleReturn(tokens);
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
        instruction.StringValue = tokens[1].Value;
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
        instruction.StringValue = tokens[mathematicalOperatorIndex].Value;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleIdentifier(List<Token> tokens) {
        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Variable;
        instruction.StringValue = tokens[0].Value;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }

    private static Instruction HandleNumber(List<Token> tokens) {
        Debug.Assert(tokens.Count == 1);
        Debug.Assert(tokens[0].Type == Consts.TokenTypes.Number);

        Instruction instruction = new Instruction();
        instruction.Type = Consts.InstructionTypes.Number;
        instruction.IntegerValue = int.Parse(tokens[0].Value);
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
        instruction.StringValue = trimmedValue;
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
        instruction.BoolValue = value;
        instruction.Line = tokens[0].Line;
        instruction.Column = tokens[0].Column;
        return instruction;
    }
}