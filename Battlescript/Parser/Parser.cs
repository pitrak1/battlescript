namespace Battlescript;

public class Parser
{
    private List<Token> _tokens;
    private List<Token> _currentTokens;
    // This is the base set of instructions we'll be returning
    private List<Instruction> _instructions; 
    // This is to track where the currently parsed instruction goes.  The base scope should always be entry 0, but
    // this will keep references to the set of different code blocks we are in (fors, ifs, functions, whatever)
    private List<List<Instruction>> _scopes;
    private InstructionParser _instructionParser;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _currentTokens = [];
        _instructions = [];
        _scopes = [_instructions];
        _instructionParser = new InstructionParser();
    }

    public List<Instruction> Run()
    {
        var currentIndentValue = 0;
        for (int tokenIndex = 0; tokenIndex < _tokens.Count; tokenIndex++)
        {
            var token = _tokens[tokenIndex];
            if (token.Type == Consts.TokenTypes.Newline)
            {
                var newIndentValue = int.Parse(token.Value);
                // If we are at the start of a code block
                if (newIndentValue > currentIndentValue) {
                    // Add the last instruction of the current scope to the stack of scopes so that new instructions
                    // within the block are added there
                    _scopes.Add(_scopes[^1][^1].Instructions);
                }
                // If we are at the end of a code block
                else if (newIndentValue < currentIndentValue)
                {
                    // Pop off the last scope so that new instructions are instead added to the next
                    // largest containing scope
                    _scopes.RemoveAt(_scopes.Count - 1);
                }

                ParseInstructionAndAddToCurrentScope();
            }
            else
            {
                _currentTokens.Add(token);
            }
        }

        if (_currentTokens.Count != 0)
        {
            ParseInstructionAndAddToCurrentScope();
        }
        
        return _instructions;
    }

    private void ParseInstructionAndAddToCurrentScope()
    {
        // Parse the current tokens, add to the current scope, and reset list of current tokens for next instruction
        var instruction = _instructionParser.Run(_currentTokens);
        _scopes[^1].Add(instruction);
        _currentTokens = [];
    }
}