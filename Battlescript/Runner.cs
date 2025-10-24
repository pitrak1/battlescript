namespace Battlescript;

public static class Runner
{
    public static Memory Run(string input)
    {
        var memory = new Memory();

        foreach (var builtin in Memory.BsTypeStrings)
        {
            LoadBuiltin(memory, builtin);
            memory.PopulateBsTypeReference(builtin);
        }

        RunAsMain(memory, input);
        
        return memory;
    }

    public static void RunAsMain(Memory memory, string input)
    {
        memory.AddScope();
        // memory.CurrentStack.Files.Add("main");
        // memory.CurrentStack.Functions.Add("<module>");
        RunPartial(memory, input, "main");
    }

    public static Dictionary<string, Variable> RunFilePath(Memory memory, string path)
    {
        var input = ReadFile(path);
        RunPartial(memory, input, path);
        return memory.RemoveScope();
    }

    private static void LoadBuiltin(Memory memory, string builtinName)
    {
        var fileName = $"/Users/nickpitrak/Desktop/Battlescript/Battlescript/BuiltIn/{builtinName}.bs";
        string text = ReadFile(fileName);
        RunPartial(memory, text, fileName);
    }

    private static string ReadFile(string path)
    {
        using StreamReader reader = new(path);
        return reader.ReadToEnd();
    }

    public static void RunPartial(Memory memory, string input, string fileName)
    {
        var parserResult = Parse(input);
        var interpreter = new Interpreter(parserResult);
        
        try
        {
            interpreter.Run(memory);
        }
        catch (InternalReturnException e)
        {
            throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "'return' outside function");
        }
        catch (InternalBreakException e)
        {
            throw new InternalRaiseException(Memory.BsTypes.SyntaxError, "'break' outside loop");
        }
        catch (InternalRaiseException e)
        {
            // memory.CurrentStack.PrintStacktrace();
            if (e.Type is not null)
            {
                Console.WriteLine(e.Type + ": " + e.Message);
            }
            else
            {
                Console.WriteLine(e.Message);
            }
            throw;
        }
    }

    public static List<Instruction> Parse(string input)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        Postparser.Run(parserResult);
        return parserResult;
    }
    
    public static List<Token> Tokenize(string input)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        return lexerResult;
    }
}