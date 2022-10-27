using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BattleScript.Tests; 

public class ParserTests {
    [Test]
    public void Variables() {
        string contents = LoadFile("variables.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        contents = LoadFile("variables_parser.json");
        var expected = JsonConvert.DeserializeObject<List<Instruction>>(contents);
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void Operators() {
        string contents = LoadFile("operators.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
    
        contents = LoadFile("operators_parser.json");
        var expected = JsonConvert.DeserializeObject<List<Instruction>>(contents);
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void Arrays() {
        string contents = LoadFile("arrays.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
    
        contents = LoadFile("arrays_parser.json");
        var expected = JsonConvert.DeserializeObject<List<Instruction>>(contents);
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    [Test]
    public void Dictionaries() {
        string contents = LoadFile("dictionaries.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);
    
        contents = LoadFile("dictionaries_parser.json");
        var expected = JsonConvert.DeserializeObject<List<Instruction>>(contents);
        
        Assertions.AssertInstructions(instructions, expected);
    }

    private string LoadFile(string filename) {
        return File.ReadAllText($"/Users/nickpitrak/Desktop/BattleScript/TestFiles/{filename}");
    }
}