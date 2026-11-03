# Unit Tests

## Test Development

The file opcode6809.asm (short) and opcode6809.asm (long) are assembled 
with the lwasm assembler to generate an authoritative list of expected 
forth assembly results, like so:
```bash
lwasm --6809 --format=raw \
   --output=opcode6809b.bin --list=opcode6809b.lst \
   opcode6809b.asm
```
The file opcode6809b.lst is then processed with a text editor to create
the unit test *expectation* arrays and the corresponding forth assembly
mnemonics that will generate the *actual* array.

The forth word "==" then performs the comparison of the 2 arrays
looking for a mismatch and a failed test.

## Usage
The unit tests are run after the file assembler6809.fs is uploaded.

The unit_tests_framework.fs file can be loaded before or after.
The 6809_opcode_tests.fs is loaded last to run the tests.
