namespace Battlescript;

public class Parser
{
    private readonly List<Token> _tokens;
    private readonly List<Token> _currentTokens;
    private readonly List<Instruction> _instructions; 
    // This is to track where the currently parsed instruction goes.  The base scope should always be entry 0, but
    // this will keep references to the set of different code blocks we are in (fors, ifs, functions, whatever)
    private readonly List<List<Instruction>> _scopes;

    public Parser(List<Token> tokens)
    {
        _tokens = tokens;
        _currentTokens = [];
        _instructions = [];
        _scopes = [_instructions];
    }

    public List<Instruction> Run()
    {
        var currentIndentValue = 0;
        for (int tokenIndex = 0; tokenIndex < _tokens.Count; tokenIndex++)
        {
            var token = _tokens[tokenIndex];
            if (token.Type == Consts.TokenTypes.Newline)
            {
                ParseInstructionAndAddToCurrentScope();
                
                var newIndentValue = int.Parse(token.Value);
                var indentDiff = newIndentValue - currentIndentValue;
                // If we are at the start of a code block
                if (indentDiff == 1)
                {
                    if (_scopes[^1].Count == 0 || !Consts.InstructionTypesExpectingIndent.Contains(_scopes[^1][^1].GetType().Name))
                    {
                        throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "unexpected indent");
                    }
                    // Add the last instruction of the current scope to the stack of scopes so that new instructions
                    // within the block are added there
                    _scopes.Add(_scopes[^1][^1].Instructions);
                }
                // If we are at the end of a code block
                else if (indentDiff < 0)
                {
                    // Pop off the last scope so that new instructions are instead added to the next
                    // largest containing scope
                    _scopes.RemoveRange(_scopes.Count + indentDiff, -indentDiff);
                }
                else if (indentDiff > 1)
                {
                    throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "unindent does not match any outer indentation level");
                }
                
                currentIndentValue = newIndentValue;
            }
            else
            {
                _currentTokens.Add(token);
            }
        }

        // This is to handle the case that the file does not end with a newline
        if (_currentTokens.Count != 0)
        {
            ParseInstructionAndAddToCurrentScope();
        }
        
        return _instructions;
    }

    private void ParseInstructionAndAddToCurrentScope()
    {
        if (_currentTokens.Count != 0)
        {
            // Parse the current tokens, add to the current scope, and reset list of current tokens for next instruction
            var instruction = InstructionFactory.Create(_currentTokens);
            _scopes[^1].Add(instruction);
            _currentTokens.Clear();
        }
    }
}