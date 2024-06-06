using BattleScript.Tokens;
using BattleScript.Instructions;

namespace BattleScript.Core;

public class Parser
{
    List<Token> tokens;
    List<Token> tokensForCurrentInstruction;
    List<Instruction> instructions; // This is the base set of instructions we'll be returning
    List<List<Instruction>> scopes; // This is the stack of scopes to know where to add instructions

    InstructionParser instructionParser;

    public Parser(List<Token> _tokens)
    {
        tokens = _tokens;
        tokensForCurrentInstruction = new List<Token>();
        instructions = new List<Instruction>();
        // The scope stack starts with a reference to the base instruction object because that's
        // where we want to start adding instructions
        scopes = new List<List<Instruction>>() { instructions };
        instructionParser = new InstructionParser();
    }

    public List<Instruction> Run()
    {
        for (int tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
        {
            Token token = tokens[tokenIndex];
            if (token.Type == Consts.TokenTypes.Semicolon)
            {
                handleEndOfSemicolonInstruction(tokens, tokenIndex);
            }
            else if (Utilities.AtStartOfCodeBlock(tokens, tokenIndex))
            {
                handleStartOfCodeBlock(tokens, tokenIndex);
            }
            else if (Utilities.AtEndOfCodeBlock(tokens, tokenIndex))
            {
                handleEndOfCodeBlock(tokens, tokenIndex);
            }
            else
            {
                tokensForCurrentInstruction.Add(token);
            }
        }

        return instructions;
    }

    private void handleEndOfSemicolonInstruction(List<Token> tokens, int tokenIndex)
    {
        // Add instruction to the most recent scope and clear the tokens for the current instruction
        Instruction parsedInstruction = instructionParser.Run(tokensForCurrentInstruction);
        scopes[^1].Add(parsedInstruction);
        tokensForCurrentInstruction = new List<Token>();
    }

    private void handleStartOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        // Parse the instruction using the code block (function, if, while, etc.)
        Instruction instruction = instructionParser.Run(tokensForCurrentInstruction);

        if (instruction.Type == Consts.InstructionTypes.Else)
        {
            attachToEndOfNextChainOfMostRecentInstruction(instruction);
        }
        else
        {
            scopes[^1].Add(instruction);
        }

        // This is so that when we start parsing instructions in the block, they will
        // be added to the appropriate scope. In most cases, it's just the list of
        // Instructions, but for assignment, we specifically want it to be the Right
        // property. The most common case for this is function definitions.
        if (instruction.Type == Consts.InstructionTypes.Assignment)
        {
            scopes.Add(instruction.Right.Instructions);
        }
        else
        {
            scopes.Add(instruction.Instructions);
        }
        tokensForCurrentInstruction = new List<Token>();
    }

    private void handleEndOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        // Clear tokens for current instruction and pop most renet scope
        tokensForCurrentInstruction = new List<Token>();
        scopes.RemoveAt(scopes.Count - 1);
    }

    // It's kind of unfortunate that this has to be done this way, but the problem is that an
    // else or elseif instruction after an if instruction looks exactly the same as a whole new
    // instruction. This isn't really built to be able to determine whether an instruction with a code
    // block after an instruction with a code block is a new instruction entirely or an attachment
    // to the previous instruction.  I bet when I do try/catch, this same thing will come up.
    private void attachToEndOfNextChainOfMostRecentInstruction(Instruction instruction)
    {
        Instruction mostRecentInstruction = scopes[^1][^1];
        while (mostRecentInstruction.Next is not null)
        {
            mostRecentInstruction = mostRecentInstruction.Next;
        }
        mostRecentInstruction.Next = instruction;
    }
}