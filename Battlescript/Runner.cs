namespace Battlescript;

public static class Runner
{
    public static CallStack Run(string input)
    {
        var memory = new CallStack();

        foreach (var builtin in BsTypes.TypeStrings)
        {
            LoadBuiltin(memory, builtin);
            BsTypes.PopulateBsTypeReference(memory, builtin);
        }

        RunAsMain(memory, input);
        
        return memory;
    }

    public static void RunAsMain(CallStack callStack, string input)
    {
        // callStack.AddScope();
        // callStack.CurrentStack.Files.Add("main");
        // callStack.CurrentStack.Functions.Add("<module>");
        RunPartial(callStack, input, "main");
    }

    public static void RunFilePath(CallStack callStack, string path)
    {
        var input = ReadFile(path);
        RunPartial(callStack, input, path);
    }

    private static void LoadBuiltin(CallStack callStack, string builtinName)
    {
        var fileName = $"/Users/nickpitrak/Desktop/Battlescript/Battlescript/BuiltIn/{builtinName}.bs";
        string text = ReadFile(fileName);
        RunPartial(callStack, text, fileName);
    }

    private static string ReadFile(string path)
    {
        using StreamReader reader = new(path);
        return reader.ReadToEnd();
    }

    public static void RunPartial(CallStack callStack, string input, string fileName)
    {
        var parserResult = Parse(input);
        var interpreter = new Interpreter(parserResult);
        
        try
        {
            interpreter.Run(callStack);
        }
        catch (InternalReturnException e)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "'return' outside function");
        }
        catch (InternalBreakException e)
        {
            throw new InternalRaiseException(BsTypes.Types.SyntaxError, "'break' outside loop");
        }
        catch (InternalRaiseException e)
        {
            // callStack.CurrentStack.PrintStacktrace();
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