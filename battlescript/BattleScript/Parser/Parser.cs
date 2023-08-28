using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;
using Newtonsoft.Json;

namespace BattleScript.ParserNS;

public class Parser
{
    public List<Instruction> Run(List<Token> tokens)
    {
        List<Token> currentTokens = new List<Token>();
        List<Instruction> instructions = new List<Instruction>();
        List<List<Instruction>> scopes = new List<List<Instruction>>() { instructions };
        InstructionParser instructionParser = new InstructionParser();

        for (int tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
        {
            Token token = tokens[tokenIndex];
            if (token.Type == Consts.TokenTypes.Semicolon)
            {
                Instruction parsedInstruction = instructionParser.Run(currentTokens);
                scopes[^1].Add(parsedInstruction);
                currentTokens = new List<Token>();
            }
            else if (isStartOfCodeBlock(tokens, tokenIndex))
            {
                Instruction instruction = instructionParser.Run(currentTokens);

                if (instruction.Type == Consts.InstructionTypes.Else)
                {
                    Instruction mostRecentInstruction = scopes[^1][^1];
                    while (mostRecentInstruction.Next is not null)
                    {
                        mostRecentInstruction = mostRecentInstruction.Next;
                    }
                    mostRecentInstruction.Next = instruction;
                }
                else
                {
                    scopes[^1].Add(instruction);
                }

                if (instruction.Type == Consts.InstructionTypes.Assignment)
                {
                    scopes.Add(instruction.Right.Instructions);
                }
                else
                {
                    scopes.Add(instruction.Instructions);
                }
                currentTokens = new List<Token>();
            }
            else if (isEndOfCodeBlock(tokens, tokenIndex))
            {
                currentTokens = new List<Token>();
                scopes.RemoveAt(scopes.Count - 1);
            }
            else
            {
                currentTokens.Add(token);
            }
        }

        return instructions;
    }

    private bool isStartOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        return tokens[tokenIndex].Value == "{" && ParserUtilities.BlockContainsSemicolon(tokens, tokenIndex);
    }

    private bool isEndOfCodeBlock(List<Token> tokens, int tokenIndex)
    {
        return tokens[tokenIndex].Value == "}" && ParserUtilities.BlockContainsSemicolonReverse(tokens, tokenIndex);
    }
}