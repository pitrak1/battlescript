using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class MemoryAndVariableAccessTests
{
    public class Baseline
    {
        [Test]
        public void LoopsConditionalsEtcCanReadAndWriteOuterVariables()
        {
            var input = """
                        x = 5
                        if True:
                            y = x + 3
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 8);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
    
        [TestFixture]
        public class Functions
        {
            [Test]
            public void FunctionsCanReadOuterVariables()
            {
                var input = """
                            x = 5
                            def func():
                                return x + 3
                            y = func()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 8);
                Assertions.AssertVariable(callStack, closure, "y", expected);
            }
        
            [Test]
            public void FunctionsCannotWriteOuterVariables()
            {
                // This is interpreted as creating a new "x" variable in the function's scope
                var input = """
                            x = 5
                            def func():
                                x = x + 3
                            func()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 5);
                Assertions.AssertVariable(callStack, closure, "x", expected);
            }
        
            [Test]
            public void NestedFunctionsCanReadOuterVariables()
            {
                var input = """
                            def outer():
                                a = 10
                                def inner():
                                    return a + 5
                                return inner()
                            x = outer()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 15);
                Assertions.AssertVariable(callStack, closure, "x", expected);
            }
            
            [Test]
            public void NestedFunctionsCannotWriteOuterVariables()
            {
                var input = """
                            def outer():
                                a = 10
                                def inner():
                                    a = a + 5
                                inner()
                                return a
                            x = outer()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 10);
                Assertions.AssertVariable(callStack, closure, "x", expected);
            }
        }
        
        [TestFixture]
        public class Classes
        {
            [Test]
            public void ClassesCanReadOuterVariables()
            {
                var input = """
                            x = 5
                            class my_class():
                                z = x + 3
                            y = my_class()
                            z = y.z
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 8);
                Assertions.AssertVariable(callStack, closure, "z", expected);
            }
        
            [Test]
            public void ClassesCannotWriteOuterVariables()
            {
                // This is interpreted as creating a new member "x" for the class
                var input = """
                            x = 5
                            class my_class():
                                x = x + 3
                            y = my_class()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 5);
                Assertions.AssertVariable(callStack, closure, "x", expected);
            }
        
            [Test]
            public void NestedClassesCanReadVariablesFromEnclosingFunctions()
            {
                var input = """
                            def outer_func():
                                b = 20
                                class InnerClass:
                                    c = b + 5
                                obj = InnerClass()
                                return obj.c
                            x = outer_func()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 25);
                Assertions.AssertVariable(callStack, closure, "x", expected);
            }
            
            [Test]
            public void NestedClassesCannotWriteVariablesFromEnclosingFunctions()
            {
                var input = """
                            def outer_func():
                                b = 20
                                class InnerClass:
                                    b = b + 5
                                obj = InnerClass()
                                return b
                            x = outer_func()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 20);
                Assertions.AssertVariable(callStack, closure, "x", expected);
            }
            
            [Test]
            public void MethodsCannotAccessVariablesFromEnclosingClasses()
            {
                var input = """
                            class OuterClass:
                                d = 30
                                def method(self):
                                    return d
                            obj = OuterClass()
                            obj.method()
                            """;
                var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
                Assert.That(ex.Type, Is.EqualTo("NameError"));
            }
            
            [Test]
            public void NestedClassesCannotAccessVariablesFromEnclosingClasses()
            {
                var input = """
                            class Outer:
                                e = 40
                                class Inner:
                                    f = e + 10
                            obj = Outer().Inner()
                            """;
                var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
                Assert.That(ex.Type, Is.EqualTo("NameError"));
            }
        }

        [TestFixture]
        public class Closures
        {
            [Test]
            public void CanAccessLexicalScope()
            {
                var input = """
                            def first():
                                a = 10
                                def second():
                                    return a + 5
                                return second()
                            x = first()
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expected = BsTypes.Create(BsTypes.Types.Int, 15);
                Assertions.AssertVariable(callStack, closure, "x", expected);
            }
            
            [Test]
            public void CannotAccessRuntimeScope()
            {
                var input = """
                            def second():
                                return a + 5
                            
                            def first():
                                a = 10
                                return second()
                                
                            x = first()
                            """;
                var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
                Assert.That(ex.Type, Is.EqualTo("NameError"));
            }

            [Test]
            public void CreatesClosures()
            {
                var input = """
                            def multiplier(factor):
                                return lambda x: x * factor
                            
                            double = multiplier(2)
                            triple = multiplier(3)
                            
                            x = double(5)
                            y = triple(5)
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expectedX = BsTypes.Create(BsTypes.Types.Int, 10);
                var expectedY = BsTypes.Create(BsTypes.Types.Int, 15);
                Assertions.AssertVariable(callStack, closure, "x", expectedX);
                Assertions.AssertVariable(callStack, closure, "y", expectedY);
            }
            
            [Test]
            public void CreatesClosuresWithSelf()
            {
                var input = """
                            class MyClass():
                                def __init__(self, value):
                                    self.y = value
                            
                                def my_method(self, x):
                                    return x * self.y
                            
                            a = MyClass(5)
                            b = MyClass(10)
                            method1 = a.my_method
                            method2 = b.my_method

                            x = method1(2)
                            y = method2(2)
                            """;
                var (callStack, closure) = Runner.Run(input);
                var expectedX = BsTypes.Create(BsTypes.Types.Int, 10);
                var expectedY = BsTypes.Create(BsTypes.Types.Int, 20);
                Assertions.AssertVariable(callStack, closure, "x", expectedX);
                Assertions.AssertVariable(callStack, closure, "y", expectedY);
            }
        }
    }
    
    public class GlobalKeyword
    {
        [Test]
        public void AllowsAccessInsideFunctions()
        {
            var input = """
                        x = 5
                        def modify_global():
                            global x
                            x = 15
                        modify_global()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 15);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void AllowsAccessInsideNestedFunctions()
        {
            var input = """
                        y = 10
                        def outer_func2():
                            y = 5
                            def inner_func():
                                global y
                                y = 25
                            inner_func()
                        outer_func2()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 25);
            Assertions.AssertVariable(callStack, closure, "y", expected);
        }
        
        [Test]
        public void AllowsAccessInsideClasses()
        {
            var input = """
                        z = 30
                        class MyClass2:
                            global z
                            z = 45
                        obj = MyClass2()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 45);
            Assertions.AssertVariable(callStack, closure, "z", expected);
        }
        
        [Test]
        public void AllowsAccessInsideNestedClasses()
        {
            var input = """
                        w = 60
                        class OuterClass2():
                            w = 30
                            class InnerClass2:
                                global w
                                w = 75
                        obj = OuterClass2().InnerClass2()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 75);
            Assertions.AssertVariable(callStack, closure, "w", expected);
        }
    }
    
    [TestFixture]
    public class NonlocalKeyword
    {
        [Test]
        public void AllowsAccessToEnclosingFunctionVariablesInFunctions()
        {
            var input = """
                        def outer_func3():
                            z = 50
                            def inner_func():
                                nonlocal z
                                z = 75
                            inner_func()
                            return z
                        x = outer_func3()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 75);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void AllowsAccessToMultipleLevelsInFunctions()
        {
            var input = """
                        def outer_func4():
                            a = 100
                            
                            def middle_func():
                                
                                def inner_func():
                                    nonlocal a
                                    a = 150
                                    
                                inner_func()
                            middle_func()
                            return a
                        x = outer_func4()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 150);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void AllowsAccessToEnclosingFunctionVariablesInClasses()
        {
            var input = """
                        def outer_func5():
                            b = 200
                            class InnerClass3:
                                nonlocal b
                                b = 250
                            obj = InnerClass3()
                            return b
                        x = outer_func5()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 250);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void IgnoresClassesWhenConsideringMultipleLevels()
        {
            var input = """
                        def outer_func5():
                            b = 200
                            class InnerClass3:
                                class InnerInnerClass:
                                    nonlocal b
                                    b = 250
                            obj = InnerClass3().InnerInnerClass()
                            return b
                        x = outer_func5()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 250);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void AllowsClassMethodsToAccessEnclosingFunctionVariables()
        {
            var input = """
                        def outer_func6():
                            c = 300
                            class InnerClass4:
                                def method(self):
                                    nonlocal c 
                                    c = 350
                            obj = InnerClass4()
                            obj.method()
                            return c
                        x = outer_func6()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 350);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
        
        [Test]
        public void IgnoresClassesWhenConsideringMultipleLevelsForClassMethods()
        {
            var input = """
                        def outer_func6():
                            c = 300
                            
                            class InnerClass4:
                            
                                class InnerInnerClass:
                                
                                    def method(self):
                                        nonlocal c 
                                        c = 350
                                        
                            obj = InnerClass4.InnerInnerClass()
                            obj.method()
                            return c
                        x = outer_func6()
                        """;
            var (callStack, closure) = Runner.Run(input);
            var expected = BsTypes.Create(BsTypes.Types.Int, 350);
            Assertions.AssertVariable(callStack, closure, "x", expected);
        }
    }
}