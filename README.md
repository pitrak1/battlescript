# Battlescript: A Python-like Interpreter for Use in Godot/Unity

## Features
### Variables and Operators

8 base variable types: integer, float, string, boolean, list, dictionary, class, object

Not yet supported types:
- tuples `()`
- sets `set()`
- complex numbers `5j`

Supported Operators:
- Arithmetic:  `**`, `*`, `/`, `//`, `%`, unary `+`, binary `+`, unary `-`, binary `-`
- Comparison:  `==`, `!=`, `>`, `>=`, `<`, `<=`
- Logical: `not`, `and`, `or`
- Identity: `is`, `is not`
- Membership: `in`, `not in`
- Assignment: `=`, `+=`, `-=`, `*=`, `/=`, `//=`, `%=`, `**=`

Not Yet Supported Operators:
- Bitwise: `&`, `|`, `^`, `<<`, `>>`
- Assignment: `&=`, `|=`, `^=`, `>>=`, `<<=`, `:=`

Operator precedence respected in descending order: 
1. `()` 
2. `**`
3. unary `+`, `-`, and `~`
4. `%`, `/`, `//`, and `*`
5. `+` and `-`
6. `<<` and `>>`
7. `&`
8. `^`
9. `|`
10. `<`, `<=`, `>`, `>=`, `==`, `!=`, `is`, `is not`, `in`, `not in`
11. `not`
12. `and`
13. `or`

Falsy Values:
- Empty lists `[]`
- Empty tuples `()`
- Empty dictionaries `{}`
- Empty sets `set()`
- Empty strings `""`
- Empty ranges `range(0)`
- 0 in any numeric form
- False
- None

TODOs before v1:
- Maybe tuples and sets?
- Implied multiplication with parentheses? (ex: 5(4 + 3))
- Thorough combthrough of errors against Python errors

### Control Flow

Support for `if/elif/else`, `while`, `for`, `range`, `break`, and `continue`

Not Yet Supported:
- `pass` keyword
- `match/case` statements

TODOs before v1:
- Thorough combthrough of errors against Python errors

### Functions

Supports default and keyword arguments

Not Yet Supported: 
- `global` and `nonlocal` keywords
- `*args` and `**kwargs` parameter names for multiple arguments
- `/` and `*` for defining position-only and keyword-only arguments in function definitions
- support for documentation strings and `__doc__`
- annotations of function headers and `__annotations__`

TODOs before v1:
- Thorough combthrough of errors against Python errors
- Possibly add some stuff from the not yet supported list (probably `*args` and `**kwargs` first)

### Lists (LATER)

Supports lists and list functionality

TODOs before v1:
- Add support for all list methods listed [here](https://docs.python.org/3/tutorial/datastructures.html#more-on-lists)
- Add support for list comprehensions

** Most of the above stuff is tackled, but implementing lists properly may involve fleshing out operator overriding and constructors to make bootstrapped classes for lists/dicts/etc., so taht's probably next **

### Classes and Objects

Support for constructors

TODO:
- Reconsider adding bool to battlescript code, may be better grouped with None in c# code COnstantVariable (UPDATE: given that bools act as integers in almost eveyr context, it's probably better to keep it in bs, we'll find more uses for ConstantVariable and ConstantInstruction as time goes on)
- Need to bring in sequence, should consider if it's just lists/sets/tuples or also strings
- Need to consider how to bring in mappings/dictionaries.  may or may not be worth it
- Assert
- Need to go through and test things once everything that's likely going to be moved to battlescript i smoved
- Need to figure out try/catch and exceptions in bs