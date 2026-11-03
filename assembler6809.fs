\ ============================================================
\ ANS Forth RPN Assembler for MC6809 Microprocessor.

\ Original by Brad Rodriguez (copyrighted 1985)
\ Adapted for ANS Forth by Richard Rothwell (2026)

\ Simplified by assuming 
\ 1. Subroutine threaded (STC) 6809 
\ 2. No forth screens.
\ 3. No byte alignment required.
\ 4 ANS Forth words already exist.
\ ============================================================

\ Create the ASSEMBLER vocabulary and
\ declare it IMMEDIATE so that it can be 
\ executed inside high-level colon definitions 
\ to switch the system's search context on the fly.

ONLY FORTH DEFINITIONS
VOCABULARY ASSEMBLER

\ ------------------------------------------------------------
\ Stack consistency check.
\ Record DEPTH before compilation.
\ Check DEPTH is the same after compilation.
\ TODO: Candidates for addition to FORTH.

HEX
\ Cell location assigned to 
\ saved current stack pointer.
\ 21 CONSTANT CSP is the location used by forth6809.

\ Not exposed in ANS Forth.
VARIABLE CSP

: !CSP ( -- )
  DEPTH CSP ! ;

\ Checks if the current stack pointer matches the saved checkpoint
: ?CSP ( -- )
  DEPTH CSP @ <> ABORT" Definition not finished / Stack mismatch!" ;

\ Check that structured-branch markers match.
: ?PAIRS ( n1 n2 -- )
  <> ABORT" Mismatched structure" ;

\ ------------------------------------------------------------
\ Define CODE and END-CODE

\ CODE is defined in the FORTH vocabulary, 
\ not the ASSEMBLER vocabulary as it is a defining word.

\ Create a CODE word that will package 6809 assembler.
\   Record current stack depth.
\   Define the dictionary header for the word definition
\   Prioritise ASSEMBLER dictionary words.

: CODE ( "name" -- )
    !CSP
    -1 HEADER
    ALSO ASSEMBLER
;


\ Assembler definitions from now on.
\ Beware redefinitions like: <> , etc..
ONLY FORTH ALSO ASSEMBLER DEFINITIONS

\ ------------------------------------------------------------
\ The inner forth interpreter.

\ The "inner interpreter" is a return from subroutine
\ in an STC forth.
HEX
: NEXT, 39 C, ;

\ ------------------------------------------------------------
\ Close a CODE word definition.
\   NEXT, aka RTS, in a subroutine threaded forth.
\   Assembler words are no longer a priority.
\   Final check for conditional branch consistency.
\   This word can be found by name from now on.

: END-CODE ( -- )
    NEXT,
    SMUDGE       
    PREVIOUS
    ?CSP
; IMMEDIATE

\ ------------------------------------------------------------
\ 6809 register codes

HEX
5 CONSTANT PC   
1 CONSTANT X        2 CONSTANT Y    
3 CONSTANT U        4 CONSTANT S    
8 CONSTANT A        9 CONSTANT B    0 CONSTANT D    
0A CONSTANT CCR    0B CONSTANT DPR

: INDEXREG? ( regCode -- )
    CASE
        X OF TRUE ENDOF 
        Y OF TRUE ENDOF 
        U OF TRUE ENDOF 
        S OF TRUE ENDOF 
        FALSE SWAP
    ENDCASE 
;

\ Forth aliases for register codes.
U CONSTANT SP   S CONSTANT RP

\ ------------------------------------------------------------
\ Utilities

\ Alias COMMA as it will be redefined as an addressing mode word later.

: (,) , ;

\ ------------------------------------------------------------
\ Simple addressing modes

\ Addressing mode enumeration: 
\     0=immediate, 10=direct, 20=indexed, 30=extended (default)
VARIABLE MODE

HEX
0  CONSTANT CON#
10 CONSTANT CON<
20 CONSTANT CON,
30 CONSTANT CON>

\ Set immediate addressing mode.
: #   CON# ( 0)  MODE ! ;

\ Set direct addressing mode.
\ Ensures one byte can 
\ address bytes within a page of memory
\ pointed to by the DP register.
\ By default this is page zero.
: <>  CON< ( 10) MODE ! ;

\ Set extended addressing mode.
\     The default addressing mode 
\     used by the opcode when it is not set 
\     to something else beforehand.
: DEFAULT   CON> ( 30) MODE ! ;

\ Set the index addressing mode.
: INDEXED! CON, ( 20) MODE ! ;

\ The initial mode is extended, as it is after each instruction.
DEFAULT

\ Is indexed addressing mode active?
: INDEXED?   MODE @  CON, ( 20) = ;

\ Is immediate addressing mode active?
: IMMEDIATE? MODE @  CON# (  0) = ;

\ Set indexed addressing mode.
\ Ensure that only index registers are valid input.
\ These are X, Y, U, or S register codes with values defined above. 

HEX
: INDEXREG  ( regCode partialPostByte -- postByte)
    INDEXED! 
    SWAP       \ regCode
    DUP INDEXREG? 0= IF 3 THROW THEN 
    1-         \ regCode - 1
    20 * OR    \ Combine regCode - 1 with postbyte.
;  

\ Simple indexed addressing modes
\ Examples: 
\    X 0, = ,X
\    Y B, = B,X
\    X ,++ = ,X++

: XMODE
    CREATE (,) ( partialPostByte "name" -- )
    DOES>  ( indexReg -- )
    @ INDEXREG \ leaves postByte on stack
;  

HEX
84 XMODE 0,    86 XMODE A,    85 XMODE B,    8B XMODE D,
80 XMODE ,+    81 XMODE ,++   82 XMODE -,    83 XMODE --,

\ Constant offset indexed addressing mode.
\ Redefine COMMA as an addressing mode word.
\ Example: Y 16 , STA,

: , ( regCode n -- n postbyte)
    SWAP 89 INDEXREG 
; 

\ Constant offset indexed addressing mode,
\ relative to the program counter (PC) contents.
\ Example: 4C ,PCR LDA,

HEX
: ,PCR ( n -- n postbyte )
    INDEXED!
    8D 
; 

\ Indirect addressing mode.

HEX
: [] ( Indexed: postbyte -- postbyte, Extended: n -- n postbyte )
    INDEXED?     
    IF
        DUP 9D AND
        80 = IF 3 THROW THEN
        10 +      \ Indexed Indirect
    ELSE
        INDEXED!
        9F       \ Extended Indirect
    THEN ;            

\ ------------------------------------------------------------
\ Define the 6809 Opcodes.
\ Defining words are used, specialised to each opcode group.
\ ------------------------------------------------------------

\ Utility words


\ Place opcode word as hi-byte, lo-byte.

HEX
: W,
    DUP FF AND
    SWAP 8 RSHIFT 
    C, C, 
;      

\ Store opcode with prefix (if any).

HEX
: OPCODE,                  
    DUP 
    FF00 AND 
    IF 
        W, 
    ELSE 
        C, 
    THEN  
;

\ ------------------------------------------------------------
\ Inherent Opcodes, No Operands.

\ NOP, SYNC, DAA, SEX,, ABX, MUL,
\ RTS, RTI, SWI, SWI2, SWI3,
\ NEGA, COMA, LSRA, RORA, ASRA, ASLA, ROLA, DECA, INCA, TSTA, CLRA,
\ NEGB, COMB, LSRB, RORB, ASRB, ASLB, ROLB, DECB, INCB, TSTB, CLRB,

\ Define inherent opcodes (INHOP)

: INHOP ( opcode "name" -- )
    CREATE (,)
    DOES> ( -- )
    @ OPCODE,     \ Place opcode with or without pre-byte.
    DEFAULT 
;

\ Byte data generates a simple instruction.
\ Word data has a $10 or $11 pre-byte, 
\ expanding the instruction set.

HEX
12 INHOP NOP,   
3A INHOP ABX,   
19 INHOP DAA,   
3D INHOP MUL,   
3B INHOP RTI,   
39 INHOP RTS,
1D INHOP SEX,  
13 INHOP SYNC,  

48 INHOP ASLA,  58 INHOP ASLB,  47 INHOP ASRA,  57 INHOP ASRB,  
48 INHOP LSLA,  58 INHOP LSLB,  44 INHOP LSRA,  54 INHOP LSRB,  
49 INHOP ROLA,  59 INHOP ROLB,  46 INHOP RORA,  56 INHOP RORB,  

4F INHOP CLRA,  5F INHOP CLRB,  
43 INHOP COMA,  53 INHOP COMB,  
4A INHOP DECA,  5A INHOP DECB,
4C INHOP INCA,  5C INHOP INCB,  
40 INHOP NEGA,  50 INHOP NEGB,  
4D INHOP TSTA,  5D INHOP TSTB,

\ $10 and $11 are pre-bytes for the $3F opcode.
3F INHOP SWI,  103F INHOP SWI2,  113F INHOP SWI3,

\ ------------------------------------------------------------
\ Immediate/Direct/Extended Instructions 
\ One Operand e.g.: ADDA, SUBA, ANDB, ORA,
\ ------------------------------------------------------------

\ Immediate required and assumed (IMMOP).

\ ANDCC, ORCC, CWAI,     
\ PULS, PULU, 
\ PSHS, PSHU, 

: IMMOP   ( opcode "name" -- )
    CREATE (,)    
    DOES> ( #n -- )
      IMMEDIATE? 0= IF 3 THROW THEN
      @ C, C,     \ Opcode + byte value from stack.
        DEFAULT 
;

\ These take an immediate value 
\ which is a mask 
\ representing a list of registers.

34 IMMOP PSHS,  35 IMMOP PULS,
36 IMMOP PSHU,  37 IMMOP PULU,  


\ These take an immediate value 
\ which represents a mask 
\ applied to the condition code register.

3C IMMOP CWAI,  
1C IMMOP ANDCC, 1A IMMOP ORCC,


\ How to construct a multiple registers mask.
\ The register codes come from the list above.
\ Example: 0 A +REG B +REG X +REG PSHS,

: +REG ( mask register-id -- new-mask )
    CASE
        00 OF 06 OR ENDOF \ D register pushes both A and B ($02 + $04)
        01 OF 10 OR ENDOF \ X
        02 OF 20 OR ENDOF \ Y
        03 OF 40 OR ENDOF \ U
        04 OF 40 OR ENDOF \ S
        05 OF 80 OR ENDOF \ PC
        08 OF 02 OR ENDOF \ A
        09 OF 04 OR ENDOF \ B
        0A OF 01 OR ENDOF \ CC
        0B OF 08 OR ENDOF \ DP
    ENDCASE ;


\ How to construct a condition code mask.
\ Example: CY-FLAG Z-FLAG +  ORCC, 
HEX
01 CONSTANT CY-FLAG 
02 CONSTANT OV-FLAG
04 CONSTANT Z-FLAG
08 CONSTANT N-FLAG
10 CONSTANT IRQ-FLAG
20 CONSTANT H-FLAG
40 CONSTANT FIRQ-FLAG
80 CONSTANT E-FLAG

\ ------------------------------------------------------------
\ Register to register copies (RROP).
\ Only two registers involved. 

\ TFR, EXG,  
\ See register codes above.

 \ 2 register codes packed into a byte.
 
: PACK-2REGS ( regCode0 regCode1 -- packedByte) 
    SWAP 4 LSHIFT + 
;

: RROP
    CREATE (,) ( opcode "name" --)
    DOES> ( Reg0 Reg1 -- )
        @ C,               \ Opcode.
        PACK-2REGS C,      \ Register source & destination.
        DEFAULT 
; 

1E RROP EXG,
1F RROP TFR,

\ ------------------------------------------------------------
\ All modes relevant (GENOP). 
\ Immediate is allowed for this group.

\ SUBA, SUBB, SUBD, 
\ ADDA, ADDB, ADDD, 
\ CMPA, CMPB, CMPD, 
\ LDA, LDB, LDD, 
\ SBCA, SBCB, 
\ ANDA, ANDB,
\ BITA, BITB,
\ CMPX, CMPY,
\ LDX, LDY,
\ EORA, EORB,
\ ADCA, ADCB, 
\ ORA, ORB, 
\ CMPU, CMPS,
\ LDU, LDS,

\ ------------------
\ Modify opcode per mode.
: +MODE  ( opcode -- opcode ) 
    MODE @ +  
    DUP 0F0 AND
    50 = 
    IF 
        0F AND \ Change 5x to 0x
    THEN ; 

\ ------------------
\ Immediate values.

\ Immediate is disallowed for the following group.
\ NEG, COM, LSR, ROL, DEC, INC, STA, STB, STD, JSR, JMP,
\ ROR, ASR, ASL,
\ TST, CLR,
\ STX, STY, STU, STS,

\ Place immediate operands.
: IMMED   ( operand opcode-pfa -- ) 
    2+ @  DUP  
    0= IF 3 THROW THEN  \  An immedSize of 0 is disallowed.
    1- 
    IF 
        W, \ Place 2 byte immediate operand.
    ELSE 
        C, \ Place 1 byte immediate operand.
    THEN ;  

\ ------------------
\ Indexed addressing.

: 8BIT?   -80  80  WITHIN ;
: 5BIT?   -10 10 WITHIN ;
: NOTINDIR?  10 AND 0= ;  \ postbyte -- f | test for indirect

\ Place constant offset.
: COFSET  ( operand postbyte -- )
    OVER 
    0= 
    IF  
        0F0 AND 4 OR C, DROP     \ no offset
    ELSE 
        OVER 5BIT? OVER 
        NOTINDIR? AND 
        IF
            60 AND SWAP 1F AND OR C,  \ 5 bit offset
        ELSE 
            OVER 8BIT? 
            IF  
                0FE AND C, C,         \ 8 bit offset
            ELSE  
                C, W,                 \ 16 bit offset
            THEN
        THEN 
    THEN 
;        

\ Place PC relative.
: PCREL  ( operand postbyte -- )
    SWAP  HERE 2+ -  DUP 
    8BIT?                   \ try 8 bit relative offset
    IF 
        SWAP 0FE AND C, C,  \ it fits...lay postbyte, offset
    ELSE
        1- SWAP C, W,       \ no good...use 16 bit relative
    THEN ; 

\ Place extended indirect.
: EXTIND    ( operand postbyte -- )
   C, W, ;  \ Place postbyte and operand

\ Place indexed operands.
: INDEXED   ( operand? postbyte -- )
    DUP 8F AND 
    CASE                       \ check postbyte for modes w/ operands
        89 OF COFSET   ENDOF   \ const.offset
        8D OF PCREL    ENDOF   \ PC relative
        8F OF EXTIND   ENDOF   \ extended indir
        SWAP C,                \ simple modes, postbyte only
    ENDCASE                     
; 

\ ------------------
\ Combined handling of the addressing modes.

: GENOP   ( immedSize opcode "name" -- )
    CREATE (,) (,)    
    DOES> ( #n|addr -- )
      DUP @ +MODE OPCODE, \ Place the mode modified opcode.
      MODE @ 
    CASE \ Based on the mode again 
      CON#  OF IMMED          ENDOF   \ Immediate: opcode + byte value
      CON<  OF DROP C,        ENDOF   \ Direct: opcode + byte address
      CON,  OF DROP INDEXED   ENDOF   \ Indexed: opcode + postbyte
      CON>  OF DROP W,        ENDOF   \ Extended: opcode + word address
    ENDCASE
        DEFAULT 
;

\ Byte data generates a simple instruction.
\ Word data has a 10 or 11 pre-byte, 
\ expanding the instruction set.

\ Stack action of general addressing instructions
\ (1) immediate, direct, extended:                 operand --
\ (2) all indexed except (3):                     postbyte --
\ (3) const.offset, PCR, extended indir:  operand postbyte --

1   86 GENOP LDA,    1   C6 GENOP LDB,    
0   87 GENOP STA,    0   C7 GENOP STB,
1   8B GENOP ADDA,   1   CB GENOP ADDB,      
1   80 GENOP SUBA,   1   C0 GENOP SUBB,
1   81 GENOP CMPA,   1   C1 GENOP CMPB,   
1   89 GENOP ADCA,   1   C9 GENOP ADCB,   
1   82 GENOP SBCA,   1   C2 GENOP SBCB,
1   84 GENOP ANDA,   1   C4 GENOP ANDB,   
1   8A GENOP ORA,    1   CA GENOP ORB,
1   88 GENOP EORA,   1   C8 GENOP EORB,
1   85 GENOP BITA,   1   C5 GENOP BITB,

2   CC GENOP LDD,
0   CD GENOP STD,
2   C3 GENOP ADDD,
2   83 GENOP SUBD,
2 1083 GENOP CMPD,

0   4A GENOP DEC,    0   4C GENOP INC,
0   48 GENOP ASL,    0   47 GENOP ASR,    
0   48 GENOP LSL,    0   44 GENOP LSR,
0   4F GENOP CLR,
0   40 GENOP NEG,
0   43 GENOP COM,    
0   4D GENOP TST,
0   49 GENOP ROL,    
0   46 GENOP ROR,

2   8E GENOP LDX,    2 108E GENOP LDY,    
0   8F GENOP STX,    0 108F GENOP STY,
2   8C GENOP CMPX,   2 108C GENOP CMPY,   
2 10CE GENOP LDS,    2   CE GENOP LDU,    
0 10CF GENOP STS,    0   CF GENOP STU,    
2 118C GENOP CMPS,   2 1183 GENOP CMPU,   
0   4E GENOP JMP,    0   8D GENOP JSR,

\ ------------------------------------------------------------
\ Indexed addressing required.

\ LEAX, LEAY, 
\ LEAU, LEAS,

\ Indexed Only.
: INXOP ( opcode -- )
    CREATE (,)
    DOES> 
        INDEXED? 0= IF 3 THROW THEN
        @ OPCODE, 
        INDEXED 
        DEFAULT ;

30 INXOP LEAX,  31 INXOP LEAY,
32 INXOP LEAS,  33 INXOP LEAU,  

\ ------------------------------------------------------------
\ Opcodes for branches.

HEX

20 CONSTANT NVR \ never (NOP) - BRA 20, 16
21 CONSTANT ALW \ always - BRN 21, 10 21

22 CONSTANT LS  \ lower or same - BHI 22, 10 22
23 CONSTANT HI  \ higher - BLS 23, 10 23            

24 CONSTANT CS  \ carry set - BCC/BHS 24, 10 24         
25 CONSTANT CC  \ carry clear - BCS/BLO 25, 10 25       

\ Same as CS
24 CONSTANT LO \ lower - BCC/BHS  24, 10 24
\ Same as CC
25 CONSTANT HS \ higher or same  - BCS/BLO 25, 10 25

26 CONSTANT EQ  \ equal/zero - BNE 26, 10 26        
27 CONSTANT NE  \ not equal/not zero - BEQ 27, 10 27

28 CONSTANT VS  \ overflow set - BVC 28, 10 28
29 CONSTANT VC  \ overflow clear - BVS 29, 10 29

2A CONSTANT MI  \ minus - BPL 2A, 10 2A
2B CONSTANT PL  \ plus - BMI 2B, 10 2B

2C CONSTANT LT  \ less than - BGE 2C, 10 2C
2D CONSTANT GE  \ greater or equal - BLT 2D, 10 2D   

2E CONSTANT LE  \ less than or equal - BGT 2E, 10 2E
2F CONSTANT GT  \ greater - BLE 2F, 10 2F            

\ ------------------------------------------------------------
\ Simple branch instructions.
\ Each takes the absolute target address: addr BNE,
\ Short branches reach -128..+127 bytes from the end of the
\ instruction, otherwise THROW 3.  Long branches reach any
\ address.  Use the structured words for forward references.

\ Relative offset to target from the end of a short branch.
\ Call after the opcode byte has been placed.
: SHORT-REL ( target -- rel )
    HERE 1+ -
    DUP 8BIT? 0= IF 3 THROW THEN ;

: BROP ( opcode "name" -- )
    CREATE (,)
    DOES> ( target -- )
        @ C,  SHORT-REL C,
        DEFAULT ;

\ Relative offset to target from the end of a long branch.
\ Call after the opcode bytes have been placed.
: LBROP ( opcode "name" -- )
    CREATE (,)
    DOES> ( target -- )
        @ OPCODE,  HERE 2+ -  W,
        DEFAULT ;

HEX
20 BROP BRA,    21 BROP BRN,
22 BROP BHI,    23 BROP BLS,
24 BROP BCC,    25 BROP BCS,
24 BROP BHS,    25 BROP BLO,
26 BROP BNE,    27 BROP BEQ,
28 BROP BVC,    29 BROP BVS,
2A BROP BPL,    2B BROP BMI,
2C BROP BGE,    2D BROP BLT,
2E BROP BGT,    2F BROP BLE,
8D BROP BSR,

16 LBROP LBRA,  17 LBROP LBSR,
1021 LBROP LBRN,
1022 LBROP LBHI,  1023 LBROP LBLS,
1024 LBROP LBCC,  1025 LBROP LBCS,
1024 LBROP LBHS,  1025 LBROP LBLO,
1026 LBROP LBNE,  1027 LBROP LBEQ,
1028 LBROP LBVC,  1029 LBROP LBVS,
102A LBROP LBPL,  102B LBROP LBMI,
102C LBROP LBGE,  102D LBROP LBLT,
102E LBROP LBGT,  102F LBROP LBLE,

\ ------------------------------------------------------------
\ Structured conditional branches and loops.

\ Begin a potential forward branch to ELSE, or THEN,.
\    Remove opcode on the stack and insert into code space at HERE,
\    Create an empty space for the branch to ELSE, or THEN,.
\    Put HERE on the stack, ready for ELSE, or THEN, to patch prefix & address.
\    Put CSP marker on the stack to ensure nesting will checkout.

: IF, ( br.opcode -- adr.next.instr 2 )
    C,      \ opcode
    0  C,   \ empty space for branch relative address.
    HERE    \ target for patching of branch later.
    2       \ CSP check value.
;


\ Target for forward branch from IF, or ELSE,.
\    Verify that correct CSP marker is present on stack.
\    Use HERE value on stack to patch branch at IF, or ELSE,.
\    Only byte-size relative branches are supported.

\ Patch the forward reference
: ENDIF,  ( adr.instr.after.br 2 -- )
    2 ?PAIRS            \ Nesting OK?
    HERE OVER -  DUP  \ Relative branch size.
    8BIT? 0= IF 3 THROW THEN
    SWAP 1- C!          \ Adjust relative branch size and patch target.
;

\ Common alias of ENDIF,
: THEN, ENDIF, ;

\ Target for forward branch from IF,.
\ Begin a forward branch to THEN,.
\    Verify that correct CSP marker is present on stack.
\    Insert BRA into code space at HERE,
\    Use HERE value on stack to patch branch at IF,.
\    Put CSP marker on the stack to ensure nesting will checkout.

: ELSE,  ( adr.after.br 2 -- adr.after.this.br 2 )
   2 ?PAIRS            \ nesting OK?
   NVR C,              \ branch to ENDIF, after completing code block.
   0 C,                \ empty space for branch relative address.
   HERE SWAP           \ Retrieve IF, address, then run ENDIF,
   2 ENDIF,            \ to adjust relative branch size and patch target.
   2 ;                 \ restore CSP check value.

\ Target for reverse branch from UNTIL,

: BEGIN,  ( -- dest.adr 1 )
    HERE               \ target address.
    1                  \ CSP check value.
;

\ Reverse looping construct.
\    Finish a reverse branch to BEGIN,
\    Only byte-size relative branches are supported.

: UNTIL,  ( dest.adr 1 br.opcode -- )
    SWAP 1 ?PAIRS      \ nesting OK?
    C,                 \ opcode.
    HERE 1+ -          \ relative branch size.
    DUP 8BIT? 0= 
    IF 3 THROW THEN    \ Check branch size.
    C, ;               \ Place branch.

\ Common alias of UNTIL,

: END,  UNTIL, ;

\ Begin forward looping construct.

: WHILE, ( dest.adr 1 br.opcod -- adr.after.this 2 dest.adr 1)
    IF, 2SWAP          \ Same behavior as IF,
;

\ Terminus of forward looping construct.
\     Place unconditional branch to BEGIN,
\     Resolve branch from WHILE,
: REPEAT, \ adr.after.while 2 dest.adr.of.begin 1 --
    NVR UNTIL,         \ Same behavior as unconditional UNTIL, 
    ENDIF, ;           \ Same behavior as ENDIF.

\ ------------------------------------------------------------
\ Restore normal compilation state.

ONLY FORTH DEFINITIONS
