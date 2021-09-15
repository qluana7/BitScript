### Note

- Byte size rule

All data in stack must be store with byte(8bits) type.

I'm considering changing data type to int(32bits) type or short(16bits) type

- Bit input operation

'[]' must used with three bit operation. In '[]', + means 1, - means 0.

For example, if operation is [++-], it becomes "0000 0110".

- Input Stream Operation

@< operation read ascii code from keyboard. Then, it must do mod calculate by 256 to make byte type.

Like this
```cs
var k = (int)Console.ReadKey(true).KeyChar;

Stack.Push((byte)(k % 256));
```

### Stack Operation
- $<[]  			= push
- $>    			= pop

- ;				    = copy last stack bits. 

### Bitwise Operation

- |     			= or
- &     			= and
- ^     			= xor
- ~     			= bitwise complement

- %				    = reverse bits

- <<[]   			= left shift
- \>>[]			  = right shift

### IO Stream Operation

- @<    			= read from input
- @>    			= write to output
