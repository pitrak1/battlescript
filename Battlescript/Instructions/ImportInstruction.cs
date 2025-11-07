namespace Battlescript;

public class ImportInstruction : Instruction
{
    public string FilePath { get; set; }
    
    public string FileName { get; set; }
    public List<string> ImportNames { get; set; } = [];
    
    public ImportInstruction(List<Token> tokens) : base(tokens)
    {
        if (tokens[1].Type != Consts.TokenTypes.String)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "expected file path to be a string");
        }
        
        if (tokens[2].Value != "import")
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "expected 'import' keyword");
        }

        FilePath = tokens[1].Value;
        FileName = Path.GetFileNameWithoutExtension(FilePath);
        InitializeImportNames(tokens);
    }
    
    private void InitializeImportNames(List<Token> tokens)
    { 
        if (tokens.Count == 4 && tokens[3].Value == "*")
        {
            ImportNames = ["*"];
        }
        else
        {
            var importNamesTokens = tokens.GetRange(3, tokens.Count - 3);
            var instruction = InstructionFactory.Create(importNamesTokens);
            
            if (instruction is VariableInstruction variableInstruction)
            {
                ImportNames = [variableInstruction.Name];
            }
            else if (instruction is ConstantInstruction)
            {
                ImportNames = ["*"];
            }
            else if (instruction is ArrayInstruction arrayInstruction)
            {
                foreach (var value in arrayInstruction.Values)
                {
                    if (value is VariableInstruction varInstruction)
                    {
                        ImportNames.Add(varInstruction.Name);
                    }
                    else if (value is ConstantInstruction)
                    {
                        ImportNames.Add("*");
                    }
                }
            }
        }
    }

    public ImportInstruction(string filePath, List<string> importNames) : base([])
    {
        FilePath = filePath;
        FileName = Path.GetFileNameWithoutExtension(FilePath);
        ImportNames = importNames;
    }

    public override Variable? Interpret(
        CallStack callStack,
        Closure closure,
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        callStack.AddFrame(Line, Expression, "<module>", FilePath);
        var newClosure = new Closure(closure);
        Runner.RunFilePath(callStack, newClosure, FilePath);
        callStack.RemoveFrame();
        var importedScope = newClosure.GetLastScope();
        
        foreach (var name in ImportNames)
        {
            if (name == "*")
            {
                closure.SetVariable(callStack, new VariableInstruction(FileName), BsTypes.Create(BsTypes.Types.Dictionary, new MappingVariable(null, importedScope)));
            }
            else if (importedScope.TryGetValue(name, out var value))
            {
                closure.SetVariable(callStack, new VariableInstruction(name), value);
            }
            else
            {
                throw new Exception($"Could not find variable {name} in {FilePath}");
            }
        }

        return null;
    }
}