namespace BattlescriptTests.E2ETests;

public class ErrorHandlingTests
{
    
    // Will comment these out and fix when stacktrace stuff is tested
    // [TestFixture]
    // public class Interpret
    // {
    //     [Test]
    //     public void RunsElseIfTryThrowsException()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 try:
    //                                     raise 1
    //                                 else:
    //                                     x = 'asdf'
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "asdf"));
    //     }
    //     
    //     [Test]
    //     public void RunsExceptIfTryThrowsExceptionThatMatches()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 try:
    //                                     raise Exception('asdf')
    //                                 except Exception:
    //                                     x = 'qwer'
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "qwer"));
    //     }
    //     
    //     [Test]
    //     public void DoesNotRunElseIfTryThrowsExceptionThatMatches()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 try:
    //                                     raise Exception('asdf')
    //                                 except Exception:
    //                                     x = 'qwer'
    //                                 else:
    //                                     x = 'asdf'
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "qwer"));
    //     }
    //     
    //     [Test]
    //     public void DoesNotRunExceptIfTryThrowsExceptionThatDoesNotMatch()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 try:
    //                                     raise Exception('asdf')
    //                                 except list:
    //                                     x = 'qwer'
    //                                 else:
    //                                     x = 'asdf'
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "asdf"));
    //     }
    //     
    //     [Test]
    //     public void RunsMatchingExceptIfTryThrowsException()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 try:
    //                                     raise Exception('asdf')
    //                                 except list:
    //                                     x = 'qwer'
    //                                 except Exception:
    //                                     x = 'zxcv'
    //                                 else:
    //                                     x = 'asdf'
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.String, "zxcv"));
    //     }
    //     
    //     [Test]
    //     public void RunsFinallyAfterTryBlockIfTryDoesNotThrowException()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 try:
    //                                     x = 2
    //                                 finally:
    //                                     x = 3
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 3));
    //     }
    //     
    //     [Test]
    //     public void RunsFinallyAfterExceptBlockIfTryThrowsMatchingException()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 y = 1
    //                                 try:
    //                                     raise Exception('asdf')
    //                                 except Exception:
    //                                     y = 2
    //                                 finally:
    //                                     x = 2
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 2));
    //         Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, 2));
    //     }
    //     
    //     [Test]
    //     public void RunsFinallyAfterElseBlockIfTryThrowsException()
    //     {
    //         var memory = Runner.Run("""
    //                                 x = 1
    //                                 y = 1
    //                                 try:
    //                                     raise Exception('asdf')
    //                                 else:
    //                                     y = 2
    //                                 finally:
    //                                     x = 2
    //                                 """);
    //         Assertions.AssertVariable(memory, "x", memory.Create(Memory.BsTypes.Int, 2));
    //         Assertions.AssertVariable(memory, "y", memory.Create(Memory.BsTypes.Int, 2));
    //     }
    //     
    //     // We'll have to write some additional tests here for running finally block if exceptions are raised in
    //     // except or else blocks
    // }
}