## Dictionaries

Dictionaries in Battlescript have the same support as dictionaries in Python with the following exceptions:
- Cannot be constructed with dictionary comprehensions (ex: `{x: x**2 for x in (2, 4, 6)}`)
- Use of the `dict` constructor with a list of key-value pairs (ex: `dict([('sape', 4139), ('guido', 4127), ('jack', 4098)])`)
- Use of the `dict` constructor with keyword arguments (ex: `dict(sape=4139, guido=4127, jack=4098)`)

However, all other dictionary functionality should work as in Python.

This includes:
- Using integers and strings as keys
- Getting and setting keys (ex: `x[1] = x[3] + 5`)
- Support for the `in`/`not in` operators