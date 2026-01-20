namespace Battlescript;

public class ImportInstruction : Instruction
{
    public string FilePath { get; set; }
    
    public string FileName { get; set; }
    public List<string> ImportNames { get; set; } = [];
    
    public ImportInstruction(List<Token> tokens) : base(tokens)
    {
        CheckTokenValidity(tokens);
        FilePath = tokens[1].Value;
        FileName = Path.GetFileNameWithoutExtension(FilePath);
        InitializeImportNames(tokens);
    }

    private void CheckTokenValidity(List<Token> tokens)
    {
        if (tokens[1].Type != Consts.TokenTypes.String)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "expected file path to be a string");
        }
        
        if (tokens[2].Value != "import")
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "expected 'import' keyword");
        }
    }
    
    private void InitializeImportNames(List<Token> tokens)
    { 
        if (IsModuleExport(tokens))
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
            else if (instruction is ArrayInstruction arrayInstruction)
            {
                ImportNames = ParseImportNamesAsStrings(arrayInstruction);
            }
        }
    }

    private List<string> ParseImportNamesAsStrings(ArrayInstruction imports)
    {
        List<string> importNames = [];
        
        foreach (var value in imports.Values)
        {
            if (value is VariableInstruction varInstruction)
            {
                importNames.Add(varInstruction.Name);
            }
            // I don't love that this asterisk is an operation instruction, but it seems harmless
            else if (value is OperationInstruction)
            {
                importNames.Add("*");
            }
        }
        
        return importNames;
    }

    private bool IsModuleExport(List<Token> tokens)
    {
        return tokens.Count == 4 && tokens[3].Value == "*";
    }

    public ImportInstruction(string filePath, List<string> importNames, int? line = null, string? expression = null) : base(line, expression)
    {
        FilePath = filePath;
        FileName = Path.GetFileNameWithoutExtension(FilePath);
        ImportNames = importNames;
    }

    public override Variable? Interpret(CallStack callStack,
        Closure closure,
        Variable? instructionContext = null)
    {
        var importedScope = InterpretFileInNewClosureScope(callStack, closure);
        
        var starImportConversionType = new MappingVariable(null, importedScope);
        foreach (var name in ImportNames)
        {
            if (name == "*")
            {
                closure.SetVariable(callStack, new VariableInstruction(FileName), BtlTypes.Create(BtlTypes.Types.Dictionary, starImportConversionType));
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

    private Dictionary<string, Variable> InterpretFileInNewClosureScope(CallStack callStack, Closure closure)
    {
        callStack.AddFrame(Line, Expression, "<module>", FilePath);
        var newClosure = new Closure(closure);
        Runner.RunFilePath(callStack, newClosure, FilePath);
        callStack.RemoveFrame();
        return newClosure.Scopes[^1].Values.ToDictionary();
    }
    
    // All the code below is to override equality
    public override bool Equals(object? obj) => Equals(obj as ImportInstruction);
    public bool Equals(ImportInstruction? inst)
    {
        if (inst is null) return false;
        if (ReferenceEquals(this, inst)) return true;
        if (GetType() != inst.GetType()) return false;
        
        var importNamesEqual = ImportNames.SequenceEqual(inst.ImportNames);
        return importNamesEqual && FilePath == inst.FilePath && FileName == inst.FileName;
    }
    
    public override int GetHashCode() => HashCode.Combine(ImportNames, FilePath, FileName);
}