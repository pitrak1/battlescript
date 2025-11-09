## Operators

Battlescript supports most operators that Python does.

1. Boolean operators (`or`, `and`, and `not`)
2. Identity operators (`is` and `is not`)
3. Membership operators (`in` and `not in`)
4. Binary operators (`**`, `*`, `/`, `//`, `%`, `+`, `-`)
5. Unary operators (`+` and `-`)
6. Comparison operators (`==`, `!=`, `>`, `>=`, `<`, and `<=`)
7. Assignment operators (`=`, `+=`, `-=`, `*=`, `/=`, `//=`, and `**=`)

This notably leaves out two things which are **NOT** included in Battlescript:

1. All bitwise operations (`~`, `<<`, `>>`, `&`, `|`, and their assignment operator counterparts)
2. The "walrus" operator (`:=`)

All built in types can be arguments for identity operators and boolean operators.  Truthiness rules for boolean operations are described below.

### Membership Operators

Membership operations require one of the following type conditions:

1. When looking for a substring, both operands are `str`
2. When looking for a list element, right operand is `list`
3. When looking for a dictionary key, right operand is `dict` and left operand is `str` or `int`

### String Operators

- `+` and `+=` operators for concatenation (ex: `x = 'asdf' + 'qwer'` results in `asdfqwer`)
- `*` and `*=` operators for multiplication (ex: `x = 'qwer' * 2` results in `qwerqwer`)
- `==` and `!=` operators for comparison

### List Operators

- `+` and `+=` operators for concatenation (ex: `x = [1, 2, 3] + [4, 5, 6]` results in `[1, 2, 3, 4, 5, 6]`)
- `*` and `*=` operators for multiplication (ex: `x = [1, 2, 3] * 2` results in `[1, 2, 3, 1, 2, 3]`)
- `==` and `!=` operators for comparison

### Truthiness

The truthiness values in Battlescript are the same as Python.

1. `True`
2. Any nonzero `int`
3. Any nonzero `float`
4. Any nonempty `str`
5. Any nonempty `list`
6. Any nonempty `dict`
7. Any function
8. Any class
9. Any object (unless `__bool__` has been defined, but we'll discuss operator overloading in a different document)
