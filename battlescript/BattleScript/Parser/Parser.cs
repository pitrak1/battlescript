using System.Diagnostics;
using BattleScript.Tokens;
using BattleScript.Instructions;
using BattleScript.Core;
using Newtonsoft.Json;

namespace BattleScript.ParserNS;

public class Parser
{
    List<Token> tokensForCurrentInstruction = new List<Token>();
    List<Instruction> instructions = new List<Instruction>();
    List<List<Instruction>> scopes = new List<List<Instruction>>();
    InstructionParser instructionParser = new InstructionParser();

    public List<Instruction> Run(List<Token> tokens)
    {
        tokensForCurrentInstruction = new List<Token>();
        instructions = new List<Instruction>();
        scopes = new List<List<Instruction>>() { instructions };
        instructionParser = new InstructionParser();

        for (int tokenIndex = 0; tokenIndex < tokens.Count; tokenIndex++)
        {
            Token token = tokens[tokenIndex];
            if (token.Type == Consts.TokenTypes.Semicolon)
            {
                Instruction parsedInstruction = instructionParser.Run(tokensForCurrentInstruction);
                addToCurrentScope(parsedInstruction);
                clearTokensForCurrentInstruction();
            }
            else if (isStartOfCodeBlock(tokens, tokenIndex))
            {
                Instruction instruction = instructionParser.Run(tokensForCurrentInstruction);

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
                    addToCurrentScope(instruction);
                }

                if (instruction.Type == Consts.InstructionTypes.Assignment)
                {
                    addToScopes(instruction.Right.Instructions);
                }
                else
                {
                    addToScopes(instruction.Instructions);
                }
                clearTokensForCurrentInstruction();
            }
            else if (isEndOfCodeBlock(tokens, tokenIndex))
            {
                clearTokensForCurrentInstruction();
                popScopes();
            }
            else
            {
                tokensForCurrentInstruction.Add(token);
            }
        }

        return instructions;
    }

    private void clearTokensForCurrentInstruction()
    {
        tokensForCurrentInstruction = new List<Token>();
    }

    private void addToScopes(List<Instruction> value)
    {
        scopes.Add(value);
    }

    private void addToCurrentScope(Instruction value)
    {
        scopes[^1].Add(value);
    }

    private void popScopes()
    {
        scopes.RemoveAt(scopes.Count - 1);
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