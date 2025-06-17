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
1. `is` and `is not`
2. `in` and `not in`
3. `()` in precedence
4. Support for unary operators
5. Testing appropriate return types
6. Test falsiness
