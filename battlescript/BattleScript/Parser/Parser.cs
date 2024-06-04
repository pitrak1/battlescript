using BattleScript.Tokens;
using BattleScript.Instructions;

namespace BattleScript.Core;

public class Parser
{
    List<Token> tokens;
    List<Token> tokensForCurrentInstruction;
    List<Instruction> instructions;
    List<List<Instruction>> scopes;

    InstructionParser instructionParser;

    public Parser(List<Token> _tokens)
    {
        tokens = _tokens;
        tokensForCurrentInstruction = new List<Token>();
        instructions = new List<Instruction>();
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
                Instruction parsedInstruction = instructionParser.Run(tokensForCurrentInstruction);
                scopes[^1].Add(parsedInstruction);
                tokensForCurrentInstruction = new List<Token>();
            }
            else if (isStartOfCodeBlock(tokens, tokenIndex))
            {
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
                // property.
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
            else if (isEndOfCodeBlock(tokens, tokenIndex))
            {
                tokensForCurrentInstruction = new List<Token>();
                scopes.RemoveAt(scopes.Count - 1);
            }
            else
            {
                tokensForCurrentInstruction.Add(token);
            }
        }

        return instructions;
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

    private bool isStartOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        return tokens[tokenIndex].Value == "{" &&
            Utilities.BlockContainsSemicolon(tokens, tokenIndex);
    }

    private bool isEndOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        return tokens[tokenIndex].Value == "}" &&
            Utilities.BlockContainsSemicolonReverse(tokens, tokenIndex);
    }
}