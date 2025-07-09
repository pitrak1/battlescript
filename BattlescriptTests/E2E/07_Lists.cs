using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class Lists
{
    // [Test]
    // public void SupportsListCreation()
    // {
    //     var input = "x = [5, '5']";
    //     var expected = new ListVariable(
    //         new List<Variable>
    //         {
    //             new NumericVariable(5),
    //             new StringVariable("5"),
    //         }
    //     );
    //     var memory = Runner.Run(input);
    //     Assert.That(memory.Scopes[0], Contains.Key("x"));
    //     Assert.That(memory.Scopes[0]["x"], Is.EqualTo(expected));
    // }
    //
    // [Test]
    // public void SupportsListIndexing()
    // {
    //     var input = "x = [5, '5']\ny = x[1]";
    //     var expected = new StringVariable("5");
    //     var memory = Runner.Run(input);
    //     Assert.That(memory.Scopes[0], Contains.Key("y"));
    //     Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
    // }

    // [Test]
    // public void SupportsListRangeIndexing()
    // {
    //     var input = "x = [5, 3, 2, '5']\ny = x[1:2]";
    //     var expected = new ListVariable(
    //         new List<Variable>
    //         {
    //             new NumericVariable(3),
    //             new NumericVariable(2),
    //         }
    //     );
    //     var memory = Runner.Run(input);
    //     Assert.That(memory.Scopes[0], Contains.Key("y"));
    //     Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
    // }
    //
    // [Test]
    // public void SupportsListRangeIndexingWithNullStart()
    // {
    //     var input = "x = [5, 3, 2, '5']\ny = x[:2]";
    //     var expected = new ListVariable(
    //         new List<Variable>
    //         {
    //             new NumericVariable(5),
    //             new NumericVariable(3),
    //             new NumericVariable(2),
    //         }
    //     );
    //     var memory = Runner.Run(input);
    //     Assert.That(memory.Scopes[0], Contains.Key("y"));
    //     Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
    // }
    //
    // [Test]
    // public void SupportsListRangeIndexingWithNullEnd()
    // {
    //     var input = "x = [5, 3, 2, '5']\ny = x[1:]";
    //     var expected = new ListVariable(
    //         new List<Variable>
    //         {
    //             new NumericVariable(3),
    //             new NumericVariable(2),
    //             new StringVariable("5"),
    //         }
    //     );
    //     var memory = Runner.Run(input);
    //     Assert.That(memory.Scopes[0], Contains.Key("y"));
    //     Assert.That(memory.Scopes[0]["y"], Is.EqualTo(expected));
    // }
}