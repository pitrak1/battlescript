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
        foreach (var token in _tokens)
        {
            if (token.Type == Consts.TokenTypes.Newline)
            {
                ParseInstructionAndAddToCurrentScope();
                
                var newIndentValue = int.Parse(token.Value);
                HandleIndentChange(newIndentValue - currentIndentValue);
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

    private void HandleIndentChange(int indentDiff)
    {
        if (indentDiff == 1)
        {
            if (IsScopeEmpty(_scopes[^1]) || _scopes[^1][^1] is not IBlockInstruction)
            {
                throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "unexpected indent");
            }

            _scopes.Add(_scopes[^1][^1].Instructions);
        }
        else if (indentDiff < 0)
        {
            CloseCodeBlocks(-indentDiff);
        }
        else if (indentDiff > 1)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "unindent does not match any outer indentation level");
        }
    }

    private static bool IsScopeEmpty(List<Instruction> scope) => scope.Count == 0;

    private void CloseCodeBlocks(int count)
    {
        _scopes.RemoveRange(_scopes.Count - count, count);
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