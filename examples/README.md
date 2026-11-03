# Assembler example: receiving bytes from an MC6821 PIA

This example uses the RPN assembler to write a small device driver as
Forth `CODE` words.

## The scenario

* An MC6821 PIA sits in the `$C000` block. Port A is an 8 bit input.
* An external source puts a byte on port A and touches the CA1 pin.
* The PIA sets bit 7 of its control register A (CRA) when CA1 sees a
  falling edge. Reading the port A data register clears that flag.
* `RECEIVE ( n -- )` waits for `n` bytes and stores each one in a 256
  byte circular buffer. It has two nested loops:
  the outer loop runs once per byte, and the inner loop polls CRA.
* `PIA-INIT ( -- )` sets up the PIA and empties the buffer.

| Address | Register (CRA bit 2 = 1) | Register (CRA bit 2 = 0) |
|---------|--------------------------|--------------------------|
| `$C000` | Port A data              | Data direction register A|
| `$C001` | Control register A       | Control register A       |

The code uses extended addressing for the PIA and the variables, and
indexed addressing for the buffer and the byte count.

## Common definitions

```forth
HEX
C000 CONSTANT PIA-DATA          \ Port A data register / DDRA.
C001 CONSTANT PIA-CRA           \ Port A control register.

CREATE RXBUF  100 ALLOT         \ 256 byte circular buffer.
CREATE HEAD   0 C,              \ Next free slot, 0 to FF.

\ Initialise PIA port A as inputs, CA1 sets flag on a falling edge.
CODE PIA-INIT ( -- )
    PIA-CRA CLR,                \ Select the data direction register.
    PIA-DATA CLR,               \ All eight port A bits are inputs.
    04 # LDA,  PIA-CRA STA,     \ Data register, CA1 falling.
    PIA-DATA LDA,               \ Read data to clear any stale flag.
    HEAD CLR,                   \ Buffer starts empty.
END-CODE
```

The buffer index lives in register B. `INCB,` wraps from `FF` to `00`
by itself, so the buffer needs no wrap-around test and does not have to
be page aligned. `ABX,` adds B to X as an *unsigned* number, which is
why the example does not use `X B, STA,` (the `B,X` form treats B as
signed).

## Version 1: structured loops

```forth
CODE RECEIVE ( n -- )
    U ,++ LDY,                  \ Y = number of bytes to receive.
    BEGIN,                      \ Outer loop: one pass per byte.
        BEGIN,                  \ Inner loop: wait for the strobe.
            PIA-CRA LDA,        \ Bit 7 is the CA1 interrupt flag.
        MI UNTIL,               \ Repeat while bit 7 is clear.
        PIA-DATA LDA,           \ Read the byte, which clears the flag.
        RXBUF # LDX,            \ X = buffer address.
        HEAD LDB,               \ B = index, wraps from FF to 0.
        ABX,                    \ X = X + B, unsigned.
        X 0, STA,               \ Store the byte at ,X.
        INCB,
        HEAD STB,
        Y -1 , LEAY,            \ Y = Y - 1, sets Z when zero.
    EQ UNTIL,                   \ Repeat while Y is not zero.
END-CODE
```

Notes:

* `U ,++ LDY,` is `LDY ,U++`. It pops the count from the data stack,
  which U points to. Write registers right to left: the register comes
  first, then the addressing mode word.
* The condition constants name the condition that ends the loop, and
  assemble the *opposite* branch. `MI UNTIL,` assembles `BPL`, which
  loops back while bit 7 is clear. `EQ UNTIL,` assembles `BNE`.
* The structured branches reach only 127 bytes and cannot be made
  longer.
* `n` must be at least 1. With 0, the count wraps and the loop receives
  65536 bytes.

## Version 2: explicit long branches

The simple branch words take an absolute target address. A backward
branch needs the address of the loop start, so the code saves it with
`HERE` and leaves it on the (host) data stack until the branch.

```forth
CODE RECEIVE-LONG ( n -- )
    U ,++ LDY,
    HERE                        \ Outer loop target.
    HERE                        \ Inner loop target.
        PIA-CRA LDA,
    DUP LBPL,                   \ Loop while bit 7 is clear.
    DROP
        PIA-DATA LDA,
        RXBUF # LDX,
        HEAD LDB,
        ABX,
        X 0, STA,
        INCB,
        HEAD STB,
        Y -1 , LEAY,
    LBNE,                       \ Loop while Y is not zero.
END-CODE
```

Notes:

* Here the branch words use the true condition: `LBPL,` is branch if
  plus, and there is no inversion.
* `DUP` keeps the inner target for `LBPL,` to consume, and the
  following `DROP` discards the copy left under it. `LBNE,` then
  consumes the outer target.
* `HERE` and the branch targets are host stack values. They exist only
  while the `CODE` word is assembled and take no space at run time.
* Long branches have a 16 bit offset, so they can reach anywhere. They
  cost a prefix byte and one more offset byte: this word is 34 bytes
  against 30 bytes for version 1.
* Only backward branches can use this method, because the target must
  be known. Forward branches use `IF,` / `THEN,`, which are limited to
  127 bytes.
* The short forms `BPL,` and `BNE,` use the same style and give the
  same 30 byte code as version 1. They are limited to -128 to +127.

## Using it

```forth
PIA-INIT
100 RECEIVE            \ Receive 256 bytes, then return.
RXBUF 10 DUMP          \ Look at the first 16 bytes.
```

## What the assembler produced

```
RECEIVE (30 bytes)
10 AE C1        LDY   ,U++
B6 C0 01        LDA   $C001
2A FB           BPL   *-3
B6 C0 00        LDA   $C000
8E xx xx        LDX   #RXBUF
F6 xx xx        LDB   HEAD
3A              ABX
A7 84           STA   ,X
5C              INCB
F7 xx xx        STB   HEAD
31 3F           LEAY  -1,Y
26 E7           BNE   outer
39              RTS
```

`RECEIVE-LONG` is identical except that the two branches are
`10 2A FF F9` (`LBPL`) and `10 26 FF E3` (`LBNE`).
