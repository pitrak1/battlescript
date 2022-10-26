using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BattleScript.Tests; 

public class ParserTests {
    [Test]
    public void Variables() {
        string contents = LoadFile("variables.btl");
        var tokens = Lexer.Run(contents);
        var instructions = Parser.Run(tokens);

        contents = LoadFile("variables.json");
        var expected = JsonConvert.DeserializeObject<List<Instruction>>(contents);
        
        Assertions.AssertInstructions(instructions, expected);
    }
    
    // [Test]
    // public void Operators() {
    //     string contents = LoadFile("operators.btl");
    //     var tokens = Lexer.Run(contents);
    //     var instructions = Parser.Run(tokens);
    //
    //     List<Instruction> expected = new List<Instruction>() {
    //         new Instruction().SetAssignmentValues(
    //             Consts.InstructionTypes.Assignment,
    //             new Instruction().SetVariableValues(Consts.InstructionTypes.Declaration, "x"),
    //             new Instruction().SetVariableValues(Consts.InstructionTypes.Number, 15)
    //         ),
    //         // new Instruction().SetAssignmentValues(
    //         //     Consts.InstructionTypes.Assignment,
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.Declaration, "y"),
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.String, "1234")
    //         // ),
    //         // new Instruction().SetAssignmentValues(
    //         //     Consts.InstructionTypes.Assignment,
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.Declaration, "z"),
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.String, "2345")
    //         // ),
    //         // new Instruction().SetAssignmentValues(
    //         //     Consts.InstructionTypes.Assignment,
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.Declaration, "a"),
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.Boolean, true)
    //         // ),
    //         // new Instruction().SetAssignmentValues(
    //         //     Consts.InstructionTypes.Assignment,
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.Declaration, "b"),
    //         //     new Instruction().SetVariableValues(Consts.InstructionTypes.Variable, "a")
    //         // ),
    //     };
    //     
    //     Assertions.AssertInstructions(instructions, expected);
    // }

    private string LoadFile(string filename) {
        return File.ReadAllText($"/Users/nickpitrak/Desktop/BattleScript/TestFiles/{filename}");
    }
}