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
1. Maybe tuples and sets?
2. Thorough combthrough of errors against Python errors

### Control Flow

Support for `if/elif/else`, `while`, `for`, `range`, `break`, `continue`, `pass`, and `match/case`

TODOs before v1:
1. Make sure `if/elif/else` works
2. Add `range` and `for` support
3. Add `break` and `continue` support
4. Add `pass` support
5. Add `match/case` support

### Functions

Supports default and keyword arguments

Not Yet Supported: 
- `global` and `nonlocal` keywords
- `*args` and `**kwargs` parameter names for multiple arguments
- `/` and `*` for defining position-only and keyword-only arguments in function definitions
- support for documentation strings and `__doc__`
- annotations of function headers and `__annotations__`

TODOs before v1:
- Shore up default and keyword arguments
- Add support for lambda functions
- Possibly add some stuff from the not yet supported list (probably `*args` and `**kwargs` first)

### Data Structures

Supports lists and list functionality

TODOs before v1:
- Add support for all list methods listed [here](https://docs.python.org/3/tutorial/datastructures.html#more-on-lists)
- Add support for list comprehensions

** Lots to do around lists/data structures, so let's tackle the above things first **