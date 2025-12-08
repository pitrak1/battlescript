namespace Battlescript;

public static class Runner
{
    public static (CallStack, Closure) Run(string input, CallStack? initCallStack = null, Closure? initClosure = null)
    {
        var callStack = initCallStack ?? new CallStack();
        var closure = initClosure ?? new Closure();
        
        LoadBuiltIns(callStack, closure);
        
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

    private static void LoadBuiltIns(CallStack callStack, Closure closure)
    {
        for (int i = 0; i < BsTypes.TypeStrings.Length; i++)
        {
            var builtInName = BsTypes.TypeStrings[i];
            var fileName = $"/Users/nickpitrak/Desktop/Battlescript/Battlescript/BuiltIn/{builtInName}.bs";
            var expression = $"import {builtInName} from \"{fileName}\"";
            var importInstruction = new ImportInstruction(fileName, [builtInName], i, expression);
            
            var interpreter = new Interpreter([importInstruction]);
            interpreter.Run(callStack, closure);
            BsTypes.PopulateBsTypeReference(callStack, closure, builtInName);
        }

        BsTypes.PopulateBsTypeConstants(callStack, closure);

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
            callStack.PrintStacktrace();
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
        catch (Exception e)
        {
            callStack.PrintStacktrace();
            Console.WriteLine(e.Message);
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