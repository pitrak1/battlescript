namespace Battlescript;

public static class Runner
{
    public static (CallStack, Closure) Run(string input)
    {
        var callStack = new CallStack();
        var closure = new Closure();
        
        foreach (var builtin in BsTypes.TypeStrings)
        {
            LoadBuiltin(callStack, closure, builtin);
            BsTypes.PopulateBsTypeReference(callStack, closure, builtin);
        }

        RunAsMain(callStack, closure, input);
        
        return (callStack, closure);
    }

    public static void RunAsMain(CallStack callStack, Closure closure, string input)
    {
        RunPartial(callStack, closure, input);
    }

    public static void RunFilePath(CallStack callStack, Closure closure, string path)
    {
        var input = ReadFile(path);
        RunPartial(callStack, closure, input);
    }

    private static void LoadBuiltin(CallStack callStack, Closure closure, string builtinName)
    {
        var fileName = $"/Users/nickpitrak/Desktop/Battlescript/Battlescript/BuiltIn/{builtinName}.bs";
        string text = ReadFile(fileName);
        RunPartial(callStack, closure, text);
    }

    private static string ReadFile(string path)
    {
        using StreamReader reader = new(path);
        return reader.ReadToEnd();
    }

    public static void RunPartial(CallStack callStack, Closure closure, string input)
    {
        var parserResult = Parse(input);
        var interpreter = new Interpreter(parserResult);
        
        try
        {
            interpreter.Run(callStack, closure);
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