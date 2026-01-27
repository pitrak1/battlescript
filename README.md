# Battlescript: A Python-like Interpreter for Use in Godot/Unity

Battlescript is a Python-like language interpreter that is simple input/output in C#.  When given a string, it can produce whatever output you would like, whether that be some value in Battlescript's memory, a value to be returned, or a change in the game state through Battlescript's customizable interface layer.  This means it can easily be plugged into any Godot or Unity3D project, allowing you to build out how the player enters code and receives output in your game without worrying about interpreting the code itself.

### Explore the features

1. [Variable Types](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/VariableTypes/VariableTypes.md)
2. [Operators](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/Operators/Operators.md)
3. [Loops and Conditionals](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/LoopsAndConditionals/LoopsAndConditionals.md)
4. [Lists](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/Lists/Lists.md)
5. [Dictionaries](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/Dictionaries/Dictionaries.md)
6. [Functions](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/Functions/Functions.md)
7. [Classes](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/Classes/Classes.md)
8. [Memory](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/Memory/Memory.md)
9. [Errors](https://github.com/pitrak1/battlescript/blob/main/BattlescriptTests/E2E/Errors/Errors.md)

### To Be Implemented Before V1

- Implied multiplication with parentheses `5(4 + 3)`
- All keywords (These are listed in [Consts.cs](https://github.com/pitrak1/battlescript/blob/main/Battlescript/Consts.cs) with skips noted)
- All builtins (These are listed in [Consts.cs](https://github.com/pitrak1/battlescript/blob/main/Battlescript/Consts.cs) with skips noted)
- [All list methods](https://docs.python.org/3/tutorial/datastructures.html#more-on-lists)
- [All dict methods](https://docs.python.org/3/tutorial/datastructures.html#dictionaries)
- Decorators

### Post V1

- Tuples `()`
- Sets `set()`
- Complex numbers `5j`
- `match/case`, possibly with V1 without pattern matching
- `*args` and `**kwargs` parameter names for multiple arguments
- `/` and `*` for defining position-only and keyword-only arguments in function definitions
- support for documentation strings and `__doc__`
- annotations of function headers and `__annotations__`



### TODO
- Could use support for built in functions being passed as functions (ex: look at Builtin/ListTests.SortsWithLenAsKey)
    Update: to do this, the builtins have to exist in memory.  We can't pass them around by reference otherwise.  This would mean that if we wrote them in Battlescript, we could do this, but we would possibly need "ConversionFunctions" (much like our "ConversionTypes") to play nice with C# for stuff we can't do in Btl.  We have to start somewhere, and it's hard to start any earlier than things like `len` and `isinstance`.

