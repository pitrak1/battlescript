## Variable Types

Battlescript supports nine basic variable types:

1. Integers (class `int`)
2. Floats (class `float`)
3. Strings (class `str`)
4. Booleans (class `bool`)
5. Lists (class `list`)
6. Dictionaries (class `dict`)
7. Functions
8. Classes
9. Objects

Although I have the classes listed here, you usually do not need to reference the classes directly.  Floats are created by decimal numbers like `1.6`, bools are made with values like `True` or `False`, etc.  However, these classes do exist in Battlescript so users can inherit from them directly if they want to enhance any of the base types.  They also can be used for type conversions.  If you wanted to cast an `int` to a `str`, you could type `str(x)` where `x` is your integer value.

Strings can be created using either single quotes or double quotes, and can also be prefixed with the character `f` to allow for interpolation.  For example, `f"asdf{x}qwer"` would insert a string representation of the value `x` into the string.  If `x` were to equal `5`, then the result would be `asdf5qwer`.

The function, class, and object types will be discussed in detail in other documents.  You can find them from the [Main README](https://github.com/pitrak1/battlescript)


### Design Note

The classes listed in parentheses above are actually implemented in Battlescript.  When creating the language, there had to be some bridge between Battlescript and the C# code behind the scenes.  The options were:

1. I could make the listed classes (`int`, `float`, etc) be interpreted directly to C# classes and then create all the methods within those C# classes.  For example, doing `5 + 6` in Battlescript would create two objects in C# that are operated on directly.
2. I could make the listed classes lean on internally recognized keywords which are interpreted to C# classes.  While this would create another abstraction layer between Battlescript and C#, I could then write most of the methods within the listed classes in Battlescript and keep the C# classes as small as possible.  For example, doing `5 + 6` in Battlescript now creates two `int` objects.  Each contain a `__btl_value` property that holds a `NumericVariable` object.

I chose to do the second to try to write as much of the built in code in Battlescript as possible.  This does make things more complicated because we need to check whether we're working with the Battlescript classes or with the C# classes directly when doing operations, but it allows our C# classes to be as small as possible.  For example, the `list` class has several methods in Battlescript (`append`, `extend`, `insert`, `pop`, etc).  If we were to not have this abstraction layer, each of these methods would need to be implemented in C#.  Instead, we can just implement a few of the really important things, like getting length and adding/removing values, in C# and write the rest of these methods in Battlescript.  

Every Battlescript class listed above contains an object of a C# class in its `__btl_value` property.

- `int`, `float`, and `bool` classes contain a `NumericVariable`
- `str` contains a `StringVariable`
- `list` contains a `SequenceVariable`
- `dict` contains a `MappingVariable`

Looking at the built in classes in the [BuiltIn](https://github.com/pitrak1/battlescript/tree/main/Battlescript/BuiltIn) folder and the corresponding C# classes in [Interpreter/Variables](https://github.com/pitrak1/battlescript/tree/main/Battlescript/Interpreter/Variables) may make this more clear.