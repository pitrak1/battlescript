namespace Battlescript;

public class ImportInstruction : Instruction
{
    public string FilePath { get; set; }
    
    public string FileName { get; set; }
    public List<string> ImportNames { get; set; } = [];
    
    public ImportInstruction(List<Token> tokens)
    {
        if (tokens[0].Value != "from")
        {
            throw new ParserMissingExpectedTokenException(tokens[0], "from");
        }
        
        if (tokens[1].Type != Consts.TokenTypes.String)
        {
            throw new ParserMissingExpectedTokenException(tokens[1], "filePath");
        }
        
        if (tokens[2].Value != "import")
        {
            throw new ParserMissingExpectedTokenException(tokens[2], "import");
        }

        FilePath = tokens[1].Value;
        FileName = Path.GetFileNameWithoutExtension(FilePath);
        InitializeImportNames();
        
        Line = tokens[0].Line;
        Column = tokens[0].Column;
        return;

        void InitializeImportNames()
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
                } else if (instruction is ArrayInstruction arrayInstruction)
                {
                    foreach (var value in arrayInstruction.Values)
                    {
                        if (value is VariableInstruction varInstruction)
                        {
                            ImportNames.Add(varInstruction.Name);
                        }
                    }
                }
            }
        }
    }

    public ImportInstruction(string filePath, List<string> importNames)
    {
        FilePath = filePath;
        FileName = Path.GetFileNameWithoutExtension(FilePath);
        ImportNames = importNames;
    }

    public override Variable Interpret(
        Memory memory, 
        Variable? instructionContext = null,
        ObjectVariable? objectContext = null,
        ClassVariable? lexicalContext = null)
    {
        var importedScope = Runner.RunFilePath(memory, FilePath);
        foreach (var name in ImportNames)
        {
            if (name == "*")
            {
                memory.SetVariable(new VariableInstruction(FileName), new DictionaryVariable(null, importedScope.Variables));
            }
            else if (importedScope.Variables.ContainsKey(name))
            {
                memory.SetVariable(new VariableInstruction(name), importedScope.Variables[name]);
            }
            else
            {
                throw new Exception($"Could not find variable {name} in {FilePath}");
            }
        }

        return new ConstantVariable();
    }
}