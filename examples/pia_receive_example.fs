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

\ Version 1: structured loops.
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

\ Version 2: the same word using explicit long branches.
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
