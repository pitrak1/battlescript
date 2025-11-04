using Battlescript;

namespace BattlescriptTests.E2ETests;

[TestFixture]
public class ErrorHandlingTests
{
    [TestFixture]
    public class Raise
    {
        [Test]
        public void CanRaiseBuiltInException()
        {
            var input = "raise TypeError('asdf')";
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }
        
        [Test]
        public void CanRaiseCustomException()
        {
            var input = """
                        class MyException(Exception):
                            pass
                            
                        raise MyException('asdf')
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("MyException"));
        }
    }

    [TestFixture]
    public class TryExcept
    {
        [Test]
        public void RunsMatchingBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except TypeError:
                            x = 12
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 12));
        }
        
        [Test]
        public void DoesNotRunNonMatchingBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except AssertionError:
                            x = 12
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }

        [Test]
        public void AllowsMultipleExceptBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except AssertionError:
                            x = 9
                        except TypeError:
                            x = 12
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 12));
        }
        
        [Test]
        public void OnlyRunsFirstMatchingExceptBlock()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except TypeError:
                            x = 9
                        except Exception:
                            x = 12
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 9));
        }
        
        [Test]
        public void RunsElseIfAnyErrorIsRaised()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        else:
                            x = 9
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 9));
        }
        
        [Test]
        public void RunsElseIfNoMatchingExceptBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except AssertionError:
                            x = 12
                        else:
                            x = 9
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 9));
        }
    }

    [TestFixture]
    public class As
    {
        [Test]
        public void SupportsAsKeywordInExceptBlocks()
        {
            var input = """
                        x = 5
                        try:
                            raise TypeError('asdf')
                        except TypeError as e:
                            x = e.message
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.String, "asdf"));
        }
    }

    [TestFixture]
    public class Finally
    {
        [Test]
        public void RunsFinallyBlockIfNoErrorsAreRaised()
        {
            var input = """
                        x = 5
                        y = 3
                        try:
                            x = 9
                        except TypeError as e:
                            x = 4
                        finally:
                            y = 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 9));
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 6));
        }
        
        [Test]
        public void RunsFinallyBlockIfErrorsAreRaised()
        {
            var input = """
                        x = 5
                        y = 3
                        try:
                            raise TypeError('asdf')
                        except TypeError:
                            x = 4
                        finally:
                            y = 6
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 4));
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 6));
        }
    }

    [TestFixture]
    public class Nesting
    {
        [Test]
        public void OnlyRunsFirstMatchingExceptBlock()
        {
            var input = """
                        x = 5
                        try:
                            try:
                                raise TypeError('asdf')
                            except TypeError:
                                x = 8
                        except TypeError:
                            x = 4
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 8));
        }
        
        [Test]
        public void RunsOuterMatchingExceptBlockIfNoInnerMatch()
        {
            var input = """
                        x = 5
                        try:
                            try:
                                raise TypeError('asdf')
                            except AssertionError:
                                x = 8
                        except TypeError:
                            x = 4
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 4));
        }
        
        [Test]
        public void RunsNeitherExceptBlockIfNeitherMatches()
        {
            var input = """
                        x = 5
                        try:
                            try:
                                raise ValueError('asdf')
                            except AssertionError:
                                x = 8
                        except TypeError:
                            x = 4
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("ValueError"));
        }
        
        [Test]
        public void OnlyRunsInnerElseBlockWhenMatches()
        {
            var input = """
                        x = 5
                        try:
                            try:
                                raise TypeError('asdf')
                            else:
                                x = 8
                        except TypeError:
                            x = 4
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 8));
        }
        
        [Test]
        public void RunsOuterElseBlockIfNoInnerMatch()
        {
            var input = """
                        x = 5
                        try:
                            try:
                                raise TypeError('asdf')
                            except AssertionError:
                                x = 8
                        else:
                            x = 4
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 4));
        }
        
        [Test]
        public void RunsOuterMatchingExceptBlock()
        {
            var input = """
                        x = 5
                        y = 6
                        try:
                            try:
                                raise AssertionError('asdf')
                            except AssertionError:
                                y = 8
                                raise TypeError('asdf')
                        except TypeError:
                            x = 4
                        """;
            var (callStack, closure) = Runner.Run(input);
            Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 4));
            Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 8));
        }
        
        [Test]
        public void DoesNotRunOuterExceptBlockIfNoMatch()
        {
            var input = """
                        x = 5
                        y = 6
                        try:
                            try:
                                raise AssertionError('asdf')
                            except AssertionError:
                                y = 8
                                raise TypeError('asdf')
                        except ValueError:
                            x = 4
                        """;
            var ex = Assert.Throws<InternalRaiseException>(() => Runner.Run(input));
            Assert.That(ex.Message, Is.EqualTo("asdf"));
            Assert.That(ex.Type, Is.EqualTo("TypeError"));
        }
    }

    [TestFixture]
    public class FinallyWithNesting
    {
        [TestFixture]
        public class WhenCaughtByInnerTryCatch
        {
            [Test]
            public void RunsMatchingExceptAndFinallyBlocks()
            {
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    raise AssertionError("asdf")
                                except AssertionError:
                                    x = 8
                                finally:
                                    y = 9
                            except TypeError:
                                x = 10
                            finally:
                                z = 3
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 8));
                Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 9));
                Assertions.AssertVariable(callStack, closure, "z", BsTypes.Create(BsTypes.Types.Int, 3));
            }
            
            [Test]
            public void RunsExceptBlockBeforeInnerFinallyBlock()
            {
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    raise AssertionError("asdf")
                                except AssertionError:
                                    x = 8
                                finally:
                                    x = 9
                            except TypeError:
                                x = 10
                            finally:
                                z = 3
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 9));
            }
            
            [Test]
            public void RunsInnerFinallyBlockBeforeOuterFinallyBlock()
            {
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    raise AssertionError("asdf")
                                except AssertionError:
                                    x = 8
                                finally:
                                    x = 9
                            except TypeError:
                                x = 10
                            finally:
                                x = 3
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 3));
            }
        }
        
        [TestFixture]
        public class WhenCaughtByOuterTryCatch
        {
            [Test]
            public void RunsMatchingExceptAndFinallyBlocks()
            {
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    raise TypeError("asdf")
                                except AssertionError:
                                    x = 8
                                finally:
                                    y = 9
                            except TypeError:
                                x = 10
                            finally:
                                z = 3
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 10));
                Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 9));
                Assertions.AssertVariable(callStack, closure, "z", BsTypes.Create(BsTypes.Types.Int, 3));
            }
            
            [Test]
            public void RunsInnerFinallyBlockBeforeExceptBlock()
            {
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    raise TypeError("asdf")
                                except AssertionError:
                                    x = 8
                                finally:
                                    x = 9
                            except TypeError:
                                x = 10
                            finally:
                                z = 3
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 10));
            }
            
            [Test]
            public void RunsExceptBlockBeforeOuterFinallyBlock()
            {
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    raise TypeError("asdf")
                                except AssertionError:
                                    x = 8
                                finally:
                                    y = 9
                            except TypeError:
                                x = 10
                            finally:
                                x = 3
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "x", BsTypes.Create(BsTypes.Types.Int, 3));
            }
        }
        
        [TestFixture]
        public class WhenNotCaught
        {
            [Test]
            public void RunsFinallyBlocks()
            {
                // Introducing the outermost try/catch with else is just to be able to catch the error and check the
                // callStack state instead of only getting an exception in the test
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    try:
                                        raise ValueError("asdf")
                                    except AssertionError:
                                        x = 8
                                    finally:
                                        y = 9
                                except TypeError:
                                    x = 10
                                finally:
                                    z = 3
                            else:
                                pass
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 9));
                Assertions.AssertVariable(callStack, closure, "z", BsTypes.Create(BsTypes.Types.Int, 3));
            }
            
            [Test]
            public void RunsInnerFinallyBlockBeforeOuterFinallyBlocks()
            {
                var input = """
                            x = 5
                            y = 9
                            z = 7
                            try:
                                try:
                                    try:
                                        raise ValueError("asdf")
                                    except AssertionError:
                                        x = 8
                                    finally:
                                        y = 9
                                except TypeError:
                                    x = 10
                                finally:
                                    y = 3
                            else:
                                pass
                            """;
                var (callStack, closure) = Runner.Run(input);
                Assertions.AssertVariable(callStack, closure, "y", BsTypes.Create(BsTypes.Types.Int, 3));
            }
        }
    }
}