# Battlescript
## A Javascript-like interpreter for Godot/Unity projects

**NOTE: This project requires a lot more testing, exception handling, and feature development to be useful**

The goal of Battlescript was to allow developers to create coding games in Godot/Unity without needing to develop their own language.  This should be extensive enough to support most coding games and customizable enough to allow developers to use it for a wide array of projects.

## Integration

In order to use Battlescript in your project, copy and paste this entire repo into your assets.  In order to build the interaction between your project and Battlescript, you should only have to change the contents of `CustomCallbacks.cs`.  This file has both callbacks for common debugger functionality (breakpoints, continue, logging, ect) and allows integration via callbacks and the `Btl` object.

For example, if you wrote a function in `CustomCallbacks.cs` called `getHeroPosition`, the player could access that function by using `Btl.getHeroPosition()`.  These functions support arguments and return values, and they will be the main bridge between the player and the gameworld when coding Battlescript.  There are a couple of things to keep in mind:

1. You have to register your function by name in the `CustomCallbacks` constructor (ex: `Callbacks["test"] = new Func<double, Variable>(TestFunction);`)
2. In order to properly use return values, you need to wrap the return value in a properly constructed `Variable` object as used by the interpreter.  There will be more details in the `Variable Types` portion of the `Language Features` section.

## Language Features

A lot of this language was written to be like Javascript, so a Javascript syntax highlighter in whatever editor you'd prefer would work very well.

### Variables

To declare a variable in Battlescript, use the `var` keyword:

`var x` or `var x = 5;`

After declaration, the variable can be used just by its name:

`x = 5;`

There are six supported variable types in Battlescript:

1. Three literal types: Number, String, and Boolean.  Numbers represent both integer and floating point
4. Array - value is a List<Variable> representing the elements
5. Dictionary - value is a Dictionary<dynamic, Variable> representing the key/value pairs
6. Function - value is a List<Variable> representing the arguments.  There is also a list of instructions stored with this variable

Falsy values are null, 0, "", and false.

### Operators

Currently supported operators are `+`, `*`, `==`, `<`, and `>`, and they work exactly like you would think.

### If/While

If and While statements work the same as they do in Javascript.

`if (true) { var x = 5; }`
`var x = 1; while (x < 6) { x = x + 1; }`

Battlescript also supports if/else if/else statements.

`if (false) { var x = 5; } else if (true) { var x = 6; } else { var x = 7; }`

### Arrays

Array declaration/getting/setting can be done using square braces.

`var x = [4]; x[0] = 9; var y = x[0];`

TODO: It is not currently possible to create an array of arbitrary size without an initialization list

### Dictionaries

Dictionary declaration can be done using curly braces and get/set using square braces.

`var x = {'asdf': 4, 9: true}; x[13] = 39; var y = x['asdf'];`

### Functions

Functions are first class objects in Battlescript and can be declared using the `function` keyword:

`var x = function() { var y = 5; }`

Arguments and return values are also supported:

`var x = function(y) { return y + 3; } var z = x(3);`

### Debugger

TODO: Is supported but needs to be clarified on use


