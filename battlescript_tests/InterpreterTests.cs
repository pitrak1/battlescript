using BattleScript.Core;
using BattleScript.InterpreterNS;

namespace BattleScript.Tests;

public class InterpreterTests
{
    private Runner? _runner;

    [SetUp]
    public void SetUp()
    {
        _runner = new Runner("/Users/nickpitrak/Desktop/battlescript/battlescript_tests/TestFiles/");
    }

    [Test]
    public void Variables()
    {
        var scopeStack = _runner.Run("variables.btl");

        Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
        expected.Add("x", new ScopeVariable(Consts.VariableTypes.Literal, 15));
        expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, "1234"));
        expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, "2345"));
        expected.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, true));
        expected.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, true));

        Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    }

    // [Test]
    // public void Operators()
    // {
    //     var scopeStack = _runner.Run("operators.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("x", new ScopeVariable(Consts.VariableTypes.Literal, 11));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 56));
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, false));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, true));
    //     expected.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, false));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Arrays()
    // {
    //     var scopeStack = _runner.Run("arrays.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("x", new ScopeVariable(
    //         Consts.VariableTypes.Array,
    //         new List<ScopeVariable>() {
    //             new (Consts.VariableTypes.Literal, 1),
    //             new (Consts.VariableTypes.Literal, 5),
    //             new (Consts.VariableTypes.Literal, 6)
    //         }
    //     ));
    //     expected.Add("y", new ScopeVariable(
    //         Consts.VariableTypes.Array,
    //         new List<ScopeVariable>() {
    //             new (Consts.VariableTypes.Literal, "1234"),
    //             new (Consts.VariableTypes.Literal, "2345")
    //         }
    //     ));
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, 3));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 2));
    //     expected.Add("b", new ScopeVariable(
    //         Consts.VariableTypes.Array,
    //         new List<ScopeVariable>() {
    //             new (Consts.VariableTypes.Literal, 3),
    //             new (Consts.VariableTypes.Literal, 2)
    //         }
    //     ));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Dictionaries()
    // {
    //     var scopeStack = _runner.Run("dictionaries.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("x", new ScopeVariable(
    //         Consts.VariableTypes.Dictionary,
    //         new Dictionary<dynamic, ScopeVariable> {
    //             {
    //                 1,
    //                 new ScopeVariable(Consts.VariableTypes.Literal, "asdf")
    //             },
    //             {
    //                 "qwer",
    //                 new ScopeVariable(Consts.VariableTypes.Literal, 5)
    //             }
    //         }
    //     ));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     expected.Add("a", new ScopeVariable(
    //         Consts.VariableTypes.Dictionary,
    //         new Dictionary<dynamic, ScopeVariable> {
    //             {
    //                 5,
    //                 new ScopeVariable(Consts.VariableTypes.Literal, 9)
    //             }
    //         }
    //     ));
    //     expected.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 9));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void If()
    // {
    //     var scopeStack = _runner.Run("if.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("x", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 6));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void IfElse()
    // {
    //     var scopeStack = _runner.Run("ifelse.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("x", new ScopeVariable(Consts.VariableTypes.Literal, 3));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 3));
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, 2));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 5));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void While()
    // {
    //     var scopeStack = _runner.Run("while.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, 8));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 11));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Functions()
    // {
    //     var scopeStack = _runner.Run("functions.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("my_function", new ScopeVariable(
    //         Consts.VariableTypes.Function,
    //         new List<ScopeVariable>()
    //     ));
    //     expected.Add("x", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     expected.Add("my_other_function", new ScopeVariable(
    //         Consts.VariableTypes.Function,
    //         new List<ScopeVariable>() {
    //             new ScopeVariable(Consts.VariableTypes.Literal, "my_variable")
    //         }
    //     ));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 8));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Classes()
    // {
    //     var scopeStack = _runner.Run("classes.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 10));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("x", new ScopeVariable(Consts.VariableTypes.Object, class1Scope));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, 10));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Methods()
    // {
    //     var scopeStack = _runner.Run("methods.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     class1Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));

    //     Dictionary<string, ScopeVariable> xScope = new Dictionary<string, ScopeVariable>();
    //     xScope.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 10));
    //     xScope.Add("class", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("x", new ScopeVariable(Consts.VariableTypes.Object, xScope));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, 10));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Inheritance()
    // {
    //     var scopeStack = _runner.Run("inheritance.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 6));
    //     class1Scope.Add("d", new ScopeVariable(Consts.VariableTypes.Literal, 3));
    //     class1Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));

    //     Dictionary<string, ScopeVariable> class2Scope = new Dictionary<string, ScopeVariable>();
    //     class2Scope.Add("c", new ScopeVariable(Consts.VariableTypes.Literal, 9));
    //     class2Scope.Add("d", new ScopeVariable(Consts.VariableTypes.Literal, 15));
    //     class2Scope.Add("my_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("super", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> bScope = new Dictionary<string, ScopeVariable>();
    //     bScope.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 9));
    //     bScope.Add("d", new ScopeVariable(Consts.VariableTypes.Literal, 12));
    //     bScope.Add("c", new ScopeVariable(Consts.VariableTypes.Literal, 9));
    //     bScope.Add("class", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("Class2", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));
    //     expected.Add("b", new ScopeVariable(Consts.VariableTypes.Object, bScope));
    //     expected.Add("c", new ScopeVariable(Consts.VariableTypes.Literal, 15));
    //     expected.Add("d", new ScopeVariable(Consts.VariableTypes.Literal, 9));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Self()
    // {
    //     var scopeStack = _runner.Run("self.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 8));
    //     class1Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));

    //     Dictionary<string, ScopeVariable> xScope = new Dictionary<string, ScopeVariable>();
    //     xScope.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 8));
    //     xScope.Add("class", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("x", new ScopeVariable(Consts.VariableTypes.Object, xScope));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 8));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Super()
    // {
    //     var scopeStack = _runner.Run("super.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));

    //     Dictionary<string, ScopeVariable> class2Scope = new Dictionary<string, ScopeVariable>();
    //     class2Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("my_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("super", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> aScope = new Dictionary<string, ScopeVariable>();
    //     aScope.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 9));
    //     aScope.Add("d", new ScopeVariable(Consts.VariableTypes.Literal, 12));
    //     aScope.Add("c", new ScopeVariable(Consts.VariableTypes.Literal, 9));
    //     aScope.Add("class", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("Class2", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Object, aScope));
    //     expected.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 9));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void SuperSuper()
    // {
    //     var scopeStack = _runner.Run("super_super.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class1Scope.Add("my_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));

    //     Dictionary<string, ScopeVariable> class2Scope = new Dictionary<string, ScopeVariable>();
    //     class2Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("my_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("super", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> class3Scope = new Dictionary<string, ScopeVariable>();
    //     class3Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class3Scope.Add("my_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class3Scope.Add("my_other_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class3Scope.Add("super", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> aScope = new Dictionary<string, ScopeVariable>();
    //     aScope.Add("class", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("Class2", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));
    //     expected.Add("Class3", new ScopeVariable(Consts.VariableTypes.Class, class3Scope));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Object, aScope));
    //     expected.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 3));
    //     expected.Add("c", new ScopeVariable(Consts.VariableTypes.Literal, 9));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void SelfSuper()
    // {
    //     var scopeStack = _runner.Run("self_super.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("x", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     class1Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));

    //     Dictionary<string, ScopeVariable> class2Scope = new Dictionary<string, ScopeVariable>();
    //     class2Scope.Add("my_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("super", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> aScope = new Dictionary<string, ScopeVariable>();
    //     aScope.Add("x", new ScopeVariable(Consts.VariableTypes.Literal, 10));
    //     aScope.Add("class", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("Class2", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Object, aScope));
    //     expected.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 10));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void Constructors()
    // {
    //     var scopeStack = _runner.Run("constructors.btl");

    //     Dictionary<string, ScopeVariable> class1Scope = new Dictionary<string, ScopeVariable>();
    //     class1Scope.Add("constructor", new ScopeVariable(
    //         Consts.VariableTypes.Function, new List<ScopeVariable>() {
    //             new (Consts.VariableTypes.Literal, "value")
    //         }
    //     ));
    //     class1Scope.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 4));
    //     class1Scope.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 7));
    //     class1Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));

    //     Dictionary<string, ScopeVariable> class2Scope = new Dictionary<string, ScopeVariable>();
    //     class2Scope.Add("constructor", new ScopeVariable(
    //         Consts.VariableTypes.Function, new List<ScopeVariable>() {
    //             new (Consts.VariableTypes.Literal, "value")
    //         }
    //     ));
    //     class2Scope.Add("my_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("my_other_function", new ScopeVariable(Consts.VariableTypes.Function, new List<ScopeVariable>()));
    //     class2Scope.Add("super", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));

    //     Dictionary<string, ScopeVariable> aScope = new Dictionary<string, ScopeVariable>();
    //     aScope.Add("a", new ScopeVariable(Consts.VariableTypes.Literal, 8));
    //     aScope.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 11));
    //     aScope.Add("class", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("Class1", new ScopeVariable(Consts.VariableTypes.Class, class1Scope));
    //     expected.Add("Class2", new ScopeVariable(Consts.VariableTypes.Class, class2Scope));
    //     expected.Add("a", new ScopeVariable(Consts.VariableTypes.Object, aScope));
    //     expected.Add("b", new ScopeVariable(Consts.VariableTypes.Literal, 8));
    //     expected.Add("c", new ScopeVariable(Consts.VariableTypes.Literal, 11));
    //     expected.Add("d", new ScopeVariable(Consts.VariableTypes.Literal, 11));
    //     expected.Add("e", new ScopeVariable(Consts.VariableTypes.Literal, 8));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }

    // [Test]
    // public void DictionaryAssignment()
    // {
    //     var scopeStack = _runner.Run("dictionary_assignment.btl");

    //     Dictionary<string, ScopeVariable> expected = new Dictionary<string, ScopeVariable>();
    //     expected.Add("x", new ScopeVariable(
    //         Consts.VariableTypes.Dictionary,
    //         new Dictionary<dynamic, ScopeVariable> {
    //             {
    //                 "x",
    //                 new ScopeVariable(Consts.VariableTypes.Literal, 1)
    //             },
    //             {
    //                 "y",
    //                 new ScopeVariable(Consts.VariableTypes.Literal, 2)
    //             },
    //             {
    //                 "z",
    //                 new ScopeVariable(Consts.VariableTypes.Literal, 5)
    //             },
    //             {
    //                 "a",
    //                 new ScopeVariable(Consts.VariableTypes.Literal, 6)
    //             }
    //         }
    //     ));
    //     expected.Add("y", new ScopeVariable(Consts.VariableTypes.Literal, 5));
    //     expected.Add("z", new ScopeVariable(Consts.VariableTypes.Literal, 6));

    //     Assertions.AssertScope(scopeStack.GetCurrentContext().Value, expected);
    // }
}