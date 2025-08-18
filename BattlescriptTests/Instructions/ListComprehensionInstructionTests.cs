using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ListComprehensionInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ParsesForLoops()
        {
            var loopInstructions = Runner.Parse("""
                                                lstcmp = []
                                                for x in y:
                                                    lstcmp.append(x * 2)
                                                """);
            var compInstructions = Runner.Parse("[x * 2 for x in y]");
            Assertions.AssertInstructionListsEqual(loopInstructions, compInstructions.First().Instructions);
        }
        
        [Test]
        public void HandlesForWithIf()
        {
            var loopInstructions = Runner.Parse("""
                                                lstcmp = []
                                                for x in y:
                                                    if x == 3:
                                                        lstcmp.append(x * 2)
                                                """);
            var compInstructions = Runner.Parse("[x * 2 for x in y if x == 3]");
            Assertions.AssertInstructionListsEqual(loopInstructions, compInstructions.First().Instructions);
        }
        
        [Test]
        public void HandlesMultipleFors()
        {
            var loopInstructions = Runner.Parse("""
                                                lstcmp = []
                                                for x in y:
                                                    for y in z:
                                                        lstcmp.append(x * 2)
                                                """);
            var compInstructions = Runner.Parse("[x * 2 for x in y for y in z]");
            Assertions.AssertInstructionListsEqual(loopInstructions, compInstructions.First().Instructions);
        }
        
        [Test]
        public void HandlesMultipleForsAndIfs()
        {
            var loopInstructions = Runner.Parse("""
                                                lstcmp = []
                                                for x in y:
                                                    if x == 3:
                                                        for y in z:
                                                            if y == 3:
                                                                lstcmp.append(x * 2)
                                                """);
            var compInstructions = Runner.Parse("[x * 2 for x in y if x == 3 for y in z if y == 3]");
            Assertions.AssertInstructionListsEqual(loopInstructions, compInstructions.First().Instructions);
        }
    }
}