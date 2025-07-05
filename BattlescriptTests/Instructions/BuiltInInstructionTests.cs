using Battlescript;

namespace BattlescriptTests.Instructions;

[TestFixture]
public static partial class InstructionTests
{
    [TestFixture]
    public class BuiltInInstructionParse
    {
        [Test]
        public void HandlesNoArguments()
        {
            var lexer = new Lexer("super()");
            var lexerResult = lexer.Run();
            
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: []
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesArguments()
        {
            var lexer = new Lexer("super(x, y)");
            var lexerResult = lexer.Run();
            
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: [new VariableInstruction("x"), new VariableInstruction("y")]
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
        
        [Test]
        public void HandlesTokensAfterArguments()
        {
            var lexer = new Lexer("super(x, y).asdf");
            var lexerResult = lexer.Run();
            
            var expected = new BuiltInInstruction(
                name: "super",
                parameters: [new VariableInstruction("x"), new VariableInstruction("y")],
                next: new ArrayInstruction([new StringInstruction("asdf")])
            );
            
            Assert.That(InstructionFactory.Create(lexerResult), Is.EqualTo(expected));
        }
    }

    [TestFixture]
    public class BuiltInInstructionInterpret
    {
        [TestFixture]
        public class IsInstance
        {
            [Test]
            public void ReturnsTrueIfObjectIsDirectInstanceOfClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        x = asdf()
                                        y = isinstance(x, asdf)
                                        """);
                Assert.That(memory.Scopes.First()["y"], Is.EqualTo(new ConstantVariable(true)));
            }
            
            [Test]
            public void ReturnsTrueIfObjectIsInheritedInstanceOfClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer(asdf):
                                            j = 6
                                            
                                        x = qwer()
                                        y = isinstance(x, asdf)
                                        """);
                Assert.That(memory.Scopes.First()["y"], Is.EqualTo(new ConstantVariable(true)));
            }
            
            [Test]
            public void ReturnsFalseIfObjectIsNotInstanceOfClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer:
                                            j = 6
                                            
                                        x = qwer()
                                        y = isinstance(x, asdf)
                                        """);
                Assert.That(memory.Scopes.First()["y"], Is.EqualTo(new ConstantVariable(false)));
            }
        }
        
        [TestFixture]
        public class IsSubclass
        {
            [Test]
            public void ReturnsTrueIfFirstClassIsEqualToSecondClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        y = issubclass(asdf, asdf)
                                        """);
                Assert.That(memory.Scopes.First()["y"], Is.EqualTo(new ConstantVariable(true)));
            }
            
            [Test]
            public void ReturnsTrueIfFirstClassInheritsFromSecondClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer(asdf):
                                            j = 6
                                            
                                        y = issubclass(qwer, asdf)
                                        """);
                Assert.That(memory.Scopes.First()["y"], Is.EqualTo(new ConstantVariable(true)));
            }
            
            [Test]
            public void ReturnsFalseIfFirstClassDoesNotInheritFromSecondClass()
            {
                var memory = Runner.Run("""
                                        class asdf:
                                            i = 5
                                            
                                        class qwer(asdf):
                                            j = 6
                                            
                                        y = issubclass(asdf, qwer)
                                        """);
                Assert.That(memory.Scopes.First()["y"], Is.EqualTo(new ConstantVariable(false)));
            }
        }
    }
}