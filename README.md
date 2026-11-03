# assembler6809

## Introduction

A Forth structured assembler for the MC 6809 microprocessor. 
The target is the MECB 6809 single board computer (SBC).

This is a port of the RPN assembler () by [Brad Rodriguez](https://www.bradrodriguez.com/papers/). 
The original was based on FIG Forth for the 6809,
it has now been ported to ANS Forth.

Additional changes include:
- Conversion to a subroutine threading (STC) implementation. 
- Improved formatting as a text file, instead of forth SCREENS.
- Inclusion of the code in an ASSEMBLER VOCABULARY.
- More explanatory comments.
- Exhaustive unit tests.
- Some additional opcodes/addressing modes were added, just for completion's sake.

Citations:
- [B.Y.O.ASSEMBLER -or- Build Your Own (Cross-) Assembler....in Forth, by Brad Rodriguez](https://www.bradrodriguez.com/papers/tcjassem.txt)
- [B.Y.O.ASSEMBLER, Part 2:  A 6809 Forth Assembler, by Brad Rodriguez](https://www.bradrodriguez.com/papers/6809asm.txt)
- [Original Assembler Source](https://www.bradrodriguez.com/papers/6809asmlisting.txt)
- [Digicool Things: Minimalist Europe Card Bus (MECB) – 6309 PLCC packaged CPU Card](https://digicoolthings.com/minimalist-europe-card-bus-mecb-6309-plcc-packaged-cpu-card/)
- [MECB on GitHub](https://github.com/DigicoolThings/MECB/tree/main/MECB_6309_CPU_PLCC)
- [Minimalist Europe Card Bus (MECB ) – MC6809 (or HD6309) CPU Card](https://digicoolthings.com/minimalist-europe-card-bus-mecb-mc6809-or-hd6309-cpu-card/)
- [ClaudForth on GitHub]()
