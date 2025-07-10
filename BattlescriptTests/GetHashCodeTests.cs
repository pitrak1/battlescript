using Battlescript;

namespace BattlescriptTests;

[TestFixture]
public class GetHashCodeTests
{
    [Test]
    public void Constants()
    {
        var trueConstant1 = new ConstantVariable(true);
        var trueConstant2 = new ConstantVariable(true);
        var falseConstant1 = new ConstantVariable(false);
        var falseConstant2 = new ConstantVariable(false);
        var noneConstant1 = new ConstantVariable();
        var noneConstant2 = new ConstantVariable();
        
        Assert.That(trueConstant1.GetHashCode(), Is.EqualTo(trueConstant2.GetHashCode()));
        Assert.That(falseConstant1.GetHashCode(), Is.EqualTo(falseConstant2.GetHashCode()));
        Assert.That(noneConstant1.GetHashCode(), Is.EqualTo(noneConstant2.GetHashCode()));
        
        Assert.That(trueConstant1.GetHashCode(), Is.Not.EqualTo(falseConstant1.GetHashCode()));
        Assert.That(falseConstant1.GetHashCode(), Is.Not.EqualTo(noneConstant1.GetHashCode()));
        Assert.That(noneConstant1.GetHashCode(), Is.Not.EqualTo(trueConstant1.GetHashCode()));
    }
    
    [Test]
    public void BuiltInClasses()
    {
        var memory = Runner.Run("");
        var intClass = memory.BuiltInReferences["int"];
        var floatClass = memory.BuiltInReferences["float"];
        var boolClass = memory.BuiltInReferences["bool"];
        
        Assert.That(intClass.GetHashCode(), Is.Not.EqualTo(floatClass.GetHashCode()));
        Assert.That(floatClass.GetHashCode(), Is.Not.EqualTo(boolClass.GetHashCode()));
        Assert.That(boolClass.GetHashCode(), Is.Not.EqualTo(intClass.GetHashCode()));
    }
    
    [Test]
    public void BuiltInObjects()
    {
        var memory = Runner.Run("");
        
        var intClass = memory.BuiltInReferences["int"];
        var intVariable1 = intClass.CreateObject();
        var intVariable2 = intClass.CreateObject();
        
        var floatClass = memory.BuiltInReferences["float"];
        var floatVariable1 = floatClass.CreateObject();
        var floatVariable2 = floatClass.CreateObject();
        
        var boolClass = memory.BuiltInReferences["bool"];
        var boolVariable1 = boolClass.CreateObject();
        var boolVariable2 = boolClass.CreateObject();
        
        Assert.That(intVariable1.Equals(intVariable2), Is.True);
        Assert.That(intVariable1.GetHashCode(), Is.EqualTo(intVariable2.GetHashCode()));
        
        
        Assert.That(floatVariable1.GetHashCode(), Is.EqualTo(floatVariable2.GetHashCode()));
        Assert.That(boolVariable1.GetHashCode(), Is.EqualTo(boolVariable2.GetHashCode()));
    }
}