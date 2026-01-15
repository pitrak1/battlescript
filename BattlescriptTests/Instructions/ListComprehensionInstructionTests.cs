using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static class ListComprehensionInstructionTests
{
    [TestFixture]
    public class Parse
    {
        [Test]
        public void ForLoop()
        {
            var loopInstructions = Runner.Parse("""
                                                lstcmp = []
                                                for x in y:
                                                    lstcmp.append(x * 2)
                                                """);
            var compInstructions = Runner.Parse("[x * 2 for x in y]");
            Assert.That(loopInstructions, Is.EquivalentTo(compInstructions.First().Instructions));
        }
        
        [Test]
        public void ForAndIf()
        {
            var loopInstructions = Runner.Parse("""
                                                lstcmp = []
                                                for x in y:
                                                    if x == 3:
                                                        lstcmp.append(x * 2)
                                                """);
            var compInstructions = Runner.Parse("[x * 2 for x in y if x == 3]");
            Assert.That(loopInstructions, Is.EquivalentTo(compInstructions.First().Instructions));
        }
        
        [Test]
        public void MultipleFors()
        {
            var loopInstructions = Runner.Parse("""
                                                lstcmp = []
                                                for x in y:
                                                    for y in z:
                                                        lstcmp.append(x * 2)
                                                """);
            var compInstructions = Runner.Parse("[x * 2 for x in y for y in z]");
            Assert.That(loopInstructions, Is.EquivalentTo(compInstructions.First().Instructions));
        }
        
        [Test]
        public void MultipleForsAndIfs()
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
            Assert.That(loopInstructions, Is.EquivalentTo(compInstructions.First().Instructions));
        }
    }
}