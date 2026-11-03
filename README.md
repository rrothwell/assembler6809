# assembler6809

## Introduction

A Forth structured assembler for the MC 6809 microprocessor. 
This assembler is intended to be a developer's utility for [ClaudeForth](https://github.com/rrothwell/ClaudeForth),
targeting the MECB 6809 single board computer (SBC).

This is a port of the RPN assembler by [Brad Rodriguez](https://www.bradrodriguez.com/papers/). 
The original was based on FIG Forth for the 6809,
it has now been ported to ANS Forth.

Additional changes include:
- Conversion to support a subroutine threading (STC) implementation. 
- Improved formatting as a text file, instead of forth SCREENS.
- Inclusion of the code in an ASSEMBLER VOCABULARY.
- Many explanatory comments (my fault if its not quite right).
- Exhaustive unit tests.
- Some additional opcodes/addressing modes were added, just for completion's sake.

Citations:
- [B.Y.O.ASSEMBLER -or- Build Your Own (Cross-) Assembler....in Forth, by Brad Rodriguez](https://www.bradrodriguez.com/papers/tcjassem.txt).
- [B.Y.O.ASSEMBLER, Part 2:  A 6809 Forth Assembler, by Brad Rodriguez](https://www.bradrodriguez.com/papers/6809asm.txt).
- [Original Assembler Source](https://www.bradrodriguez.com/papers/6809asmlisting.txt).
- [Digicool Things: Minimalist Europe Card Bus (MECB) – 6309 PLCC packaged CPU Card](https://digicoolthings.com/minimalist-europe-card-bus-mecb-6309-plcc-packaged-cpu-card/).
- [MECB on GitHub](https://github.com/DigicoolThings/MECB/tree/main/MECB_6309_CPU_PLCC).
- [Minimalist Europe Card Bus (MECB ) – MC6809 (or HD6309) CPU Card](https://digicoolthings.com/minimalist-europe-card-bus-mecb-mc6809-or-hd6309-cpu-card/).
- [ClaudeForth on GitHub](https://github.com/rrothwell/ClaudeForth).

## Development Process

The requirements for writing a 6809 assembler were collected by Google searches and Gemini summaries.
The complexity of the 8609 instruction set is intimidating and the advice was confusing,
so I was drawn to the Rodriguez implementation as an existing solution. The supporting documentation
sets the benchmark for anyone attempting any kind of Forth RPN assembler implementation.

The development gradually discarded initial code and then absorbed the Rodriguez code,
a component at a time, with modifications along the way as it was analysed.

The unit testing scheme was then designed and written, comparing the 
assembly output of the lwasm assembler (the expectation) 
with the output of the ported assembler (the actual).
Once the prototype scheme was functioning for simple opcodes, 
Claude was tasked with the tedious work of adding the tests for the more complex 
opcodes and addressing modes. Of course Claude voluntarily goes further,
as it reviews the assembler code, fixes minor bugs
and completes the tests for the loops/branches. 

## Usage

The file assembler6809.fs contains ANS forth source code.
It can only be used on an SBC with a running forth system.
This can be uploaded to either a 6809-based SBC or a MAME emulation of such a SBC,
via a serial communications interface.

It requires the forth VOCABULARY word to be defined on the target system.
If this is not available the VOCABULARY word can be commented out,
but some overloaded words will also need to be redefined. 

```
Example forth assembly to be added here later.
```

## Testing

Add the testing protocol.
