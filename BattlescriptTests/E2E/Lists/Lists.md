## Lists

Lists in Battlescript have the same support as lists in Python, with just a few exceptions:
- Tuples and sets are not supported
- Generators are not supported
- Support for overriding slice functionality with `__getitem__` and `__setitem__`

However, all other list functionality should work as in Python.

This includes:
- Getting and setting indices (ex: `x[1] = x[3] + 5`)
- Getting and setting slices with start/end/step values including negatives and empties (ex: `x[::-2] = [4, 5]`)
- Support for the `in`/`not in` operators
- Concatenation (using `+`) and multiplication (using `*`)
- List comprehensions (ex: `y = [i * 2 for i in x if z == 5]`)