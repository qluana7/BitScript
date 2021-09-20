## Note

Byte size rule

- All data in stack must be store with byte(8bits) type.
I'm considering changing data type to int(32bits) type or short(16bits) type

Bit input Operator

- '[]' must used with three bit operators. In '[]', + means 1, - means 0.

- For example, if operator is [++-], it becomes "0000 0110".

Input Stream Opetator

- @< operator read ascii code from keyboard. Then, it must do mod calculate by 256 to make byte type.

Example:
```cs
var k = (int)Console.ReadKey(true).KeyChar;

Stack.Push((byte)(k % 256));
```
Stack Rules

- All bitwise operatoes pop two values and push them after calculate them.
If there is no value to pop, ignore operator.

Code Analysis Rules

- When analysis code, interpreter must ignore following characters:

- ` (space)`, `\n(newline)`, `\t(tab)`, `\r(CR)`, all non-ascii characters and unknown command.

## Operators

### Stack Operators
- $<[]  			= push
- $>    			= pop

- ;				    = copy last stack bits. 

### Bitwise Operators

- |     			= or
- &     			= and
- ^     			= xor
- ~     			= bitwise complement

- %				    = reverse bits

- <<[]   			= left shift
- \>>[]			  = right shift

### IO Stream Operators

- @<    			= read from input
- @>    			= write to output
