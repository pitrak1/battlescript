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
        for (var i = 0; i < BtlTypes.TypeStrings.Length; i++)
        {
            var builtInName = BtlTypes.TypeStrings[i];
            ImportBuiltInByName(callStack, closure, builtInName, i);
            BtlTypes.PopulateBtlTypeReference(callStack, closure, builtInName);
        }

        BtlTypes.PopulateBtlTypeConstants(callStack, closure);

        // for (var i = 0; i < Consts.BuiltInFunctions.Length; i++)
        // {
        //     var builtInName = Consts.BuiltInFunctions[i];
        //     ImportBuiltInByName(callStack, closure, builtInName, i + BtlTypes.TypeStrings.Length);
        // }
    }

    private static void ImportBuiltInByName(CallStack callStack, Closure closure, string name, int line)
    {
        var fileName = $"/Users/nickpitrak/Desktop/Battlescript/Battlescript/BuiltIn/{name}.bs";
        var expression = $"import {name} from \"{fileName}\"";
        var importInstruction = new ImportInstruction(fileName, [name], line, expression);
        var interpreter = new Interpreter([importInstruction]);
        interpreter.Run(callStack, closure);
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
        catch (InternalReturnException)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "'return' outside function");
        }
        catch (InternalBreakException)
        {
            throw new InternalRaiseException(BtlTypes.Types.SyntaxError, "'break' outside loop");
        }
        catch (InternalRaiseException e)
        {
            callStack.PrintStacktrace();
            if (e.Type is not null)
            {
                Console.WriteLine($"{e.Type}: {e.Message}");
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

    public static List<Instruction> Parse(string input, bool runPostParser = true)
    {
        var lexer = new Lexer(input);
        var lexerResult = lexer.Run();
        Postlexer.Run(lexerResult);
        var parser = new Parser(lexerResult);
        var parserResult = parser.Run();
        if (runPostParser) Postparser.Run(parserResult);
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