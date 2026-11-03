\ ============================================================
\ ANS test suite - 6809 Forth RPN assembler.
\ Tests opcodes and addressing modes.
\ Requires ttester.fs then 00_test_prelude.fs to be loaded first.
\ ============================================================
MARKER M35
TEST-BEGIN

\ Compiling into FORTH vocabulary. 

ONLY FORTH DEFINITIONS
HEX

\ Marks end of byte list.
:  NULL,  0 C, ;

\ Produce a 0 byte on @
CREATE LEAVE-PTR 0 C,

: BOTH@ ( opCodesExpected opCodesActual -- ) 
    C@ SWAP C@ SWAP 
; 
: BOTH+ ( opCodesExpected opCodesActual -- ) 
    1+ SWAP 1+ SWAP
; 

\ Compare two assembled byte streams.
\ E.g.: EXPECTED ACTUAL ==

: == ( opCodesExpected opCodesActual -- TRUE|FALSE)
    2>R 
    TRUE    \ Default result is TRUE.
    2R>
    BEGIN 
        OVER C@ 0<>   \ Terminal byte is 0. 
    WHILE
        2DUP BOTH@ = \ Compare expected and actual bytes.
        IF 
            BOTH+
        ELSE 
            2DROP 
            DROP FALSE \ Replace TRUE with FALSE on stack.
            LEAVE-PTR LEAVE-PTR
        THEN       
    REPEAT
    2DROP \ Leaving result on stack.
;    


\ Implicit opcodes, No operand.

HEX
CREATE EXPECTED
12 C,  3A C,  19 C,  3D C,  3B C,  39 C,  1D C,  13 C, NULL,

\ Need to find & execute ASSEMBLER words.
ALSO ASSEMBLER
CREATE ACTUAL
NOP,  ABX,  DAA,  MUL,  RTI,  RTS,   SEX,  SYNC, 
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

CREATE EXPECTED
48 C,  58 C,  47 C,  57 C,  48 C,  58 C,  44 C,  54 C,  49 C,  59 C,  
46 C,  56 C,  NULL,  

ALSO ASSEMBLER
CREATE ACTUAL
ASLA, ASLB, ASRA, ASRB, LSLA, LSLB, LSRA, LSRB, ROLA, ROLB, RORA, RORB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

CREATE EXPECTED
4F C,  5F C,  43 C,  53 C,  4A C,  5A C,  4C C,  5C C,  40 C,  50 C,  
4D C,  5D C,   NULL, 

ALSO ASSEMBLER
CREATE ACTUAL
CLRA, CLRB, COMA, COMB, DECA, DECB,  INCA, INCB, NEGA, NEGB, TSTA, TSTB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ Immediate only opcodes.

CREATE EXPECTED
34 C,  FF C,     35 C,  FF C,     36 C,  FF C,     37 C,  FF C,
NULL, 

ALSO ASSEMBLER
CREATE ACTUAL
FF # PSHS,    FF # PULS,    FF # PSHU,    FF # PULU,  
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

CREATE EXPECTED
3C C,  A5 C,     1C C,  A5 C,     1A C,  A5 C, 
NULL,
   
ALSO ASSEMBLER
CREATE ACTUAL
A5 # CWAI,    A5 # ANDCC,   A5 # ORCC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ============================================================
\ Part 2: generated from the lwasm listing opcode6809b.lst
\ ============================================================
HEX

\ Compile n NOPs.
ALSO ASSEMBLER
: NOPS,  ( n -- )  0 DO NOP, LOOP ;
PREVIOUS

\ Store byte b, n times.
: BYTES,  ( b n -- )  0 DO DUP C, LOOP DROP ;

\ ------------------------------------------------------------
\ 1a immediate
\ ------------------------------------------------------------
\ 4000 LDA #IMMVALUE .. CMPB #IMMVALUE
CREATE EXPECTED
86 C,  A5 C,  C6 C,  A5 C,  8B C,  A5 C,  CB C,  A5 C,  80 C,  A5 C,
C0 C,  A5 C,  81 C,  A5 C,  C1 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5 # LDA,  0A5 # LDB,  0A5 # ADDA,  0A5 # ADDB,  0A5 # SUBA,
0A5 # SUBB,  0A5 # CMPA,  0A5 # CMPB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4010 ADCA #IMMVALUE .. ORB #IMMVALUE
CREATE EXPECTED
89 C,  A5 C,  C9 C,  A5 C,  82 C,  A5 C,  C2 C,  A5 C,  84 C,  A5 C,
C4 C,  A5 C,  8A C,  A5 C,  CA C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5 # ADCA,  0A5 # ADCB,  0A5 # SBCA,  0A5 # SBCB,  0A5 # ANDA,
0A5 # ANDB,  0A5 # ORA,  0A5 # ORB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4020 EORA #IMMVALUE .. BITB #IMMVALUE
CREATE EXPECTED
88 C,  A5 C,  C8 C,  A5 C,  85 C,  A5 C,  C5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5 # EORA,  0A5 # EORB,  0A5 # BITA,  0A5 # BITB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 1b direct
\ ------------------------------------------------------------
\ 4028 LDA <ADDR8 .. CMPB <ADDR8
CREATE EXPECTED
96 C,  7B C,  D6 C,  7B C,  9B C,  7B C,  DB C,  7B C,  90 C,  7B C,
D0 C,  7B C,  91 C,  7B C,  D1 C,  7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> LDA,  7B <> LDB,  7B <> ADDA,  7B <> ADDB,  7B <> SUBA,
7B <> SUBB,  7B <> CMPA,  7B <> CMPB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4038 ADCA <ADDR8 .. ORB <ADDR8
CREATE EXPECTED
99 C,  7B C,  D9 C,  7B C,  92 C,  7B C,  D2 C,  7B C,  94 C,  7B C,
D4 C,  7B C,  9A C,  7B C,  DA C,  7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> ADCA,  7B <> ADCB,  7B <> SBCA,  7B <> SBCB,  7B <> ANDA,
7B <> ANDB,  7B <> ORA,  7B <> ORB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4048 EORA <ADDR8 .. STB <ADDR8
CREATE EXPECTED
98 C,  7B C,  D8 C,  7B C,  95 C,  7B C,  D5 C,  7B C,  97 C,  7B C,
D7 C,  7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> EORA,  7B <> EORB,  7B <> BITA,  7B <> BITB,  7B <> STA,
7B <> STB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 1c extended
\ ------------------------------------------------------------
\ 4054 LDA >ADDR16 .. CMPB >ADDR16
CREATE EXPECTED
B6 C,  A5 C,  A5 C,  F6 C,  A5 C,  A5 C,  BB C,  A5 C,  A5 C,  FB C,
A5 C,  A5 C,  B0 C,  A5 C,  A5 C,  F0 C,  A5 C,  A5 C,  B1 C,  A5 C,
A5 C,  F1 C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 LDA,  0A5A5 LDB,  0A5A5 ADDA,  0A5A5 ADDB,  0A5A5 SUBA,
0A5A5 SUBB,  0A5A5 CMPA,  0A5A5 CMPB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 406C ADCA >ADDR16 .. ORB >ADDR16
CREATE EXPECTED
B9 C,  A5 C,  A5 C,  F9 C,  A5 C,  A5 C,  B2 C,  A5 C,  A5 C,  F2 C,
A5 C,  A5 C,  B4 C,  A5 C,  A5 C,  F4 C,  A5 C,  A5 C,  BA C,  A5 C,
A5 C,  FA C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 ADCA,  0A5A5 ADCB,  0A5A5 SBCA,  0A5A5 SBCB,  0A5A5 ANDA,
0A5A5 ANDB,  0A5A5 ORA,  0A5A5 ORB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4084 EORA >ADDR16 .. STB >ADDR16
CREATE EXPECTED
B8 C,  A5 C,  A5 C,  F8 C,  A5 C,  A5 C,  B5 C,  A5 C,  A5 C,  F5 C,
A5 C,  A5 C,  B7 C,  A5 C,  A5 C,  F7 C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 EORA,  0A5A5 EORB,  0A5A5 BITA,  0A5A5 BITB,  0A5A5 STA,
0A5A5 STB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 2a immediate
\ ------------------------------------------------------------
\ 4096 LDD #ADDR16 .. CMPY #ADDR16
CREATE EXPECTED
CC C,  A5 C,  A5 C,  C3 C,  A5 C,  A5 C,  83 C,  A5 C,  A5 C,  10 C,
83 C,  A5 C,  A5 C,  8E C,  A5 C,  A5 C,  10 C,  8E C,  A5 C,  A5 C,
8C C,  A5 C,  A5 C,  10 C,  8C C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 # LDD,  0A5A5 # ADDD,  0A5A5 # SUBD,  0A5A5 # CMPD,
0A5A5 # LDX,  0A5A5 # LDY,  0A5A5 # CMPX,  0A5A5 # CMPY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 40B1 LDS #ADDR16 .. CMPU #ADDR16
CREATE EXPECTED
10 C,  CE C,  A5 C,  A5 C,  CE C,  A5 C,  A5 C,  11 C,  8C C,  A5 C,
A5 C,  11 C,  83 C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 # LDS,  0A5A5 # LDU,  0A5A5 # CMPS,  0A5A5 # CMPU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 2b direct
\ ------------------------------------------------------------
\ 40C0 LDD <ADDR8 .. CMPY <ADDR8
CREATE EXPECTED
DC C,  7B C,  D3 C,  7B C,  93 C,  7B C,  10 C,  93 C,  7B C,  9E C,
7B C,  10 C,  9E C,  7B C,  9C C,  7B C,  10 C,  9C C,  7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> LDD,  7B <> ADDD,  7B <> SUBD,  7B <> CMPD,  7B <> LDX,
7B <> LDY,  7B <> CMPX,  7B <> CMPY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 40D3 LDS <ADDR8 .. STS <ADDR8
CREATE EXPECTED
10 C,  DE C,  7B C,  DE C,  7B C,  11 C,  9C C,  7B C,  11 C,  93 C,
7B C,  DD C,  7B C,  9F C,  7B C,  10 C,  9F C,  7B C,  10 C,  DF C,
7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> LDS,  7B <> LDU,  7B <> CMPS,  7B <> CMPU,  7B <> STD,
7B <> STX,  7B <> STY,  7B <> STS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 40E8 STU <ADDR8 .. STU <ADDR8
CREATE EXPECTED
DF C,  7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> STU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 2c extended
\ ------------------------------------------------------------
\ 40EA LDD >ADDR16 .. CMPY >ADDR16
CREATE EXPECTED
FC C,  A5 C,  A5 C,  F3 C,  A5 C,  A5 C,  B3 C,  A5 C,  A5 C,  10 C,
B3 C,  A5 C,  A5 C,  BE C,  A5 C,  A5 C,  10 C,  BE C,  A5 C,  A5 C,
BC C,  A5 C,  A5 C,  10 C,  BC C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 LDD,  0A5A5 ADDD,  0A5A5 SUBD,  0A5A5 CMPD,  0A5A5 LDX,
0A5A5 LDY,  0A5A5 CMPX,  0A5A5 CMPY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4105 LDS >ADDR16 .. STS >ADDR16
CREATE EXPECTED
10 C,  FE C,  A5 C,  A5 C,  FE C,  A5 C,  A5 C,  11 C,  BC C,  A5 C,
A5 C,  11 C,  B3 C,  A5 C,  A5 C,  FD C,  A5 C,  A5 C,  BF C,  A5 C,
A5 C,  10 C,  BF C,  A5 C,  A5 C,  10 C,  FF C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 LDS,  0A5A5 LDU,  0A5A5 CMPS,  0A5A5 CMPU,  0A5A5 STD,
0A5A5 STX,  0A5A5 STY,  0A5A5 STS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4122 STU >ADDR16 .. STU >ADDR16
CREATE EXPECTED
FF C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 STU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 3a direct
\ ------------------------------------------------------------
\ 4125 DEC <ADDR8 .. NEG <ADDR8
CREATE EXPECTED
0A C,  7B C,  0C C,  7B C,  08 C,  7B C,  07 C,  7B C,  08 C,  7B C,
04 C,  7B C,  0F C,  7B C,  00 C,  7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> DEC,  7B <> INC,  7B <> ASL,  7B <> ASR,  7B <> LSL,
7B <> LSR,  7B <> CLR,  7B <> NEG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4135 COM <ADDR8 .. JSR <ADDR8
CREATE EXPECTED
03 C,  7B C,  0D C,  7B C,  09 C,  7B C,  06 C,  7B C,  0E C,  7B C,
9D C,  7B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
7B <> COM,  7B <> TST,  7B <> ROL,  7B <> ROR,  7B <> JMP,
7B <> JSR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 3b extended
\ ------------------------------------------------------------
\ 4141 DEC >ADDR16 .. NEG >ADDR16
CREATE EXPECTED
7A C,  A5 C,  A5 C,  7C C,  A5 C,  A5 C,  78 C,  A5 C,  A5 C,  77 C,
A5 C,  A5 C,  78 C,  A5 C,  A5 C,  74 C,  A5 C,  A5 C,  7F C,  A5 C,
A5 C,  70 C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 DEC,  0A5A5 INC,  0A5A5 ASL,  0A5A5 ASR,  0A5A5 LSL,
0A5A5 LSR,  0A5A5 CLR,  0A5A5 NEG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4159 COM >ADDR16 .. JSR >ADDR16
CREATE EXPECTED
73 C,  A5 C,  A5 C,  7D C,  A5 C,  A5 C,  79 C,  A5 C,  A5 C,  76 C,
A5 C,  A5 C,  7E C,  A5 C,  A5 C,  BD C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 COM,  0A5A5 TST,  0A5A5 ROL,  0A5A5 ROR,  0A5A5 JMP,
0A5A5 JSR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 4a 16-bit pairs
\ ------------------------------------------------------------
\ 416B TFR D,D .. TFR D,PC
CREATE EXPECTED
1F C,  00 C,  1F C,  01 C,  1F C,  02 C,  1F C,  03 C,  1F C,  04 C,
1F C,  05 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
D D TFR,  D X TFR,  D Y TFR,  D U TFR,  D S TFR,  D PC TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4177 TFR X,D .. TFR X,PC
CREATE EXPECTED
1F C,  10 C,  1F C,  11 C,  1F C,  12 C,  1F C,  13 C,  1F C,  14 C,
1F C,  15 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X D TFR,  X X TFR,  X Y TFR,  X U TFR,  X S TFR,  X PC TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4183 TFR Y,D .. TFR Y,PC
CREATE EXPECTED
1F C,  20 C,  1F C,  21 C,  1F C,  22 C,  1F C,  23 C,  1F C,  24 C,
1F C,  25 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y D TFR,  Y X TFR,  Y Y TFR,  Y U TFR,  Y S TFR,  Y PC TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 418F TFR U,D .. TFR U,PC
CREATE EXPECTED
1F C,  30 C,  1F C,  31 C,  1F C,  32 C,  1F C,  33 C,  1F C,  34 C,
1F C,  35 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U D TFR,  U X TFR,  U Y TFR,  U U TFR,  U S TFR,  U PC TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 419B TFR S,D .. TFR S,PC
CREATE EXPECTED
1F C,  40 C,  1F C,  41 C,  1F C,  42 C,  1F C,  43 C,  1F C,  44 C,
1F C,  45 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S D TFR,  S X TFR,  S Y TFR,  S U TFR,  S S TFR,  S PC TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 41A7 TFR PC,D .. TFR PC,PC
CREATE EXPECTED
1F C,  50 C,  1F C,  51 C,  1F C,  52 C,  1F C,  53 C,  1F C,  54 C,
1F C,  55 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
PC D TFR,  PC X TFR,  PC Y TFR,  PC U TFR,  PC S TFR,  PC PC TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 4b 8-bit pairs
\ ------------------------------------------------------------
\ 41B3 TFR A,A .. TFR B,DP
CREATE EXPECTED
1F C,  88 C,  1F C,  89 C,  1F C,  8A C,  1F C,  8B C,  1F C,  98 C,
1F C,  99 C,  1F C,  9A C,  1F C,  9B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
A A TFR,  A B TFR,  A CCR TFR,  A DPR TFR,  B A TFR,  B B TFR,
B CCR TFR,  B DPR TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 41C3 TFR CC,A .. TFR DP,DP
CREATE EXPECTED
1F C,  A8 C,  1F C,  A9 C,  1F C,  AA C,  1F C,  AB C,  1F C,  B8 C,
1F C,  B9 C,  1F C,  BA C,  1F C,  BB C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
CCR A TFR,  CCR B TFR,  CCR CCR TFR,  CCR DPR TFR,  DPR A TFR,
DPR B TFR,  DPR CCR TFR,  DPR DPR TFR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 4c 16-bit pairs
\ ------------------------------------------------------------
\ 41D3 EXG D,D .. EXG D,PC
CREATE EXPECTED
1E C,  00 C,  1E C,  01 C,  1E C,  02 C,  1E C,  03 C,  1E C,  04 C,
1E C,  05 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
D D EXG,  D X EXG,  D Y EXG,  D U EXG,  D S EXG,  D PC EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 41DF EXG X,D .. EXG X,PC
CREATE EXPECTED
1E C,  10 C,  1E C,  11 C,  1E C,  12 C,  1E C,  13 C,  1E C,  14 C,
1E C,  15 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X D EXG,  X X EXG,  X Y EXG,  X U EXG,  X S EXG,  X PC EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 41EB EXG Y,D .. EXG Y,PC
CREATE EXPECTED
1E C,  20 C,  1E C,  21 C,  1E C,  22 C,  1E C,  23 C,  1E C,  24 C,
1E C,  25 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y D EXG,  Y X EXG,  Y Y EXG,  Y U EXG,  Y S EXG,  Y PC EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 41F7 EXG U,D .. EXG U,PC
CREATE EXPECTED
1E C,  30 C,  1E C,  31 C,  1E C,  32 C,  1E C,  33 C,  1E C,  34 C,
1E C,  35 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U D EXG,  U X EXG,  U Y EXG,  U U EXG,  U S EXG,  U PC EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4203 EXG S,D .. EXG S,PC
CREATE EXPECTED
1E C,  40 C,  1E C,  41 C,  1E C,  42 C,  1E C,  43 C,  1E C,  44 C,
1E C,  45 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S D EXG,  S X EXG,  S Y EXG,  S U EXG,  S S EXG,  S PC EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 420F EXG PC,D .. EXG PC,PC
CREATE EXPECTED
1E C,  50 C,  1E C,  51 C,  1E C,  52 C,  1E C,  53 C,  1E C,  54 C,
1E C,  55 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
PC D EXG,  PC X EXG,  PC Y EXG,  PC U EXG,  PC S EXG,  PC PC EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 4d 8-bit pairs
\ ------------------------------------------------------------
\ 421B EXG A,A .. EXG B,DP
CREATE EXPECTED
1E C,  88 C,  1E C,  89 C,  1E C,  8A C,  1E C,  8B C,  1E C,  98 C,
1E C,  99 C,  1E C,  9A C,  1E C,  9B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
A A EXG,  A B EXG,  A CCR EXG,  A DPR EXG,  B A EXG,  B B EXG,
B CCR EXG,  B DPR EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 422B EXG CC,A .. EXG DP,DP
CREATE EXPECTED
1E C,  A8 C,  1E C,  A9 C,  1E C,  AA C,  1E C,  AB C,  1E C,  B8 C,
1E C,  B9 C,  1E C,  BA C,  1E C,  BB C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
CCR A EXG,  CCR B EXG,  CCR CCR EXG,  CCR DPR EXG,  DPR A EXG,
DPR B EXG,  DPR CCR EXG,  DPR DPR EXG,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 5 PULU
\ ------------------------------------------------------------
\ 423B PSHS CC .. PSHS U
CREATE EXPECTED
34 C,  01 C,  34 C,  02 C,  34 C,  04 C,  34 C,  06 C,  34 C,  08 C,
34 C,  10 C,  34 C,  20 C,  34 C,  40 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 CCR +REG # PSHS,  0 A +REG # PSHS,  0 B +REG # PSHS,
0 D +REG # PSHS,  0 DPR +REG # PSHS,  0 X +REG # PSHS,
0 Y +REG # PSHS,  0 U +REG # PSHS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 424B PSHS PC .. PSHS A,B,X,Y,U,DP,PC,CC
CREATE EXPECTED
34 C,  80 C,  34 C,  06 C,  34 C,  30 C,  34 C,  16 C,  34 C,  0F C,
34 C,  70 C,  34 C,  F6 C,  34 C,  FF C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 PC +REG # PSHS,  0 A +REG B +REG # PSHS,  0 X +REG Y +REG # PSHS,
0 A +REG B +REG X +REG # PSHS,
0 CCR +REG A +REG B +REG DPR +REG # PSHS,
0 X +REG Y +REG U +REG # PSHS,
0 D +REG X +REG Y +REG U +REG PC +REG # PSHS,
0 A +REG B +REG X +REG Y +REG U +REG DPR +REG PC +REG CCR +REG # PSHS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 425B PULS CC .. PULS U
CREATE EXPECTED
35 C,  01 C,  35 C,  02 C,  35 C,  04 C,  35 C,  06 C,  35 C,  08 C,
35 C,  10 C,  35 C,  20 C,  35 C,  40 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 CCR +REG # PULS,  0 A +REG # PULS,  0 B +REG # PULS,
0 D +REG # PULS,  0 DPR +REG # PULS,  0 X +REG # PULS,
0 Y +REG # PULS,  0 U +REG # PULS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 426B PULS PC .. PULS A,B,X,Y,U,DP,PC,CC
CREATE EXPECTED
35 C,  80 C,  35 C,  06 C,  35 C,  30 C,  35 C,  16 C,  35 C,  0F C,
35 C,  70 C,  35 C,  F6 C,  35 C,  FF C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 PC +REG # PULS,  0 A +REG B +REG # PULS,  0 X +REG Y +REG # PULS,
0 A +REG B +REG X +REG # PULS,
0 CCR +REG A +REG B +REG DPR +REG # PULS,
0 X +REG Y +REG U +REG # PULS,
0 D +REG X +REG Y +REG U +REG PC +REG # PULS,
0 A +REG B +REG X +REG Y +REG U +REG DPR +REG PC +REG CCR +REG # PULS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 427B PSHU CC .. PSHU S
CREATE EXPECTED
36 C,  01 C,  36 C,  02 C,  36 C,  04 C,  36 C,  06 C,  36 C,  08 C,
36 C,  10 C,  36 C,  20 C,  36 C,  40 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 CCR +REG # PSHU,  0 A +REG # PSHU,  0 B +REG # PSHU,
0 D +REG # PSHU,  0 DPR +REG # PSHU,  0 X +REG # PSHU,
0 Y +REG # PSHU,  0 S +REG # PSHU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 428B PSHU PC .. PSHU A,B,X,Y,S,DP,PC,CC
CREATE EXPECTED
36 C,  80 C,  36 C,  06 C,  36 C,  30 C,  36 C,  16 C,  36 C,  0F C,
36 C,  70 C,  36 C,  F6 C,  36 C,  FF C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 PC +REG # PSHU,  0 A +REG B +REG # PSHU,  0 X +REG Y +REG # PSHU,
0 A +REG B +REG X +REG # PSHU,
0 CCR +REG A +REG B +REG DPR +REG # PSHU,
0 X +REG Y +REG S +REG # PSHU,
0 D +REG X +REG Y +REG S +REG PC +REG # PSHU,
0 A +REG B +REG X +REG Y +REG S +REG DPR +REG PC +REG CCR +REG # PSHU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 429B PULU CC .. PULU S
CREATE EXPECTED
37 C,  01 C,  37 C,  02 C,  37 C,  04 C,  37 C,  06 C,  37 C,  08 C,
37 C,  10 C,  37 C,  20 C,  37 C,  40 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 CCR +REG # PULU,  0 A +REG # PULU,  0 B +REG # PULU,
0 D +REG # PULU,  0 DPR +REG # PULU,  0 X +REG # PULU,
0 Y +REG # PULU,  0 S +REG # PULU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 42AB PULU PC .. PULU A,B,X,Y,S,DP,PC,CC
CREATE EXPECTED
37 C,  80 C,  37 C,  06 C,  37 C,  30 C,  37 C,  16 C,  37 C,  0F C,
37 C,  70 C,  37 C,  F6 C,  37 C,  FF C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0 PC +REG # PULU,  0 A +REG B +REG # PULU,  0 X +REG Y +REG # PULU,
0 A +REG B +REG X +REG # PULU,
0 CCR +REG A +REG B +REG DPR +REG # PULU,
0 X +REG Y +REG S +REG # PULU,
0 D +REG X +REG Y +REG S +REG PC +REG # PULU,
0 A +REG B +REG X +REG Y +REG S +REG DPR +REG PC +REG CCR +REG # PULU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 6 flag combinations
\ ------------------------------------------------------------
\ 42BB ORCC #$01 .. ORCC #$80
CREATE EXPECTED
1A C,  01 C,  1A C,  02 C,  1A C,  04 C,  1A C,  08 C,  1A C,  10 C,
1A C,  20 C,  1A C,  40 C,  1A C,  80 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
01 # ORCC,  02 # ORCC,  04 # ORCC,  08 # ORCC,  10 # ORCC,
20 # ORCC,  40 # ORCC,  80 # ORCC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 42CB ANDCC #$FE .. ANDCC #$7F
CREATE EXPECTED
1C C,  FE C,  1C C,  FD C,  1C C,  FB C,  1C C,  F7 C,  1C C,  EF C,
1C C,  DF C,  1C C,  BF C,  1C C,  7F C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
CY-FLAG INVERT 0FF AND # ANDCC,  OV-FLAG INVERT 0FF AND # ANDCC,
Z-FLAG INVERT 0FF AND # ANDCC,  N-FLAG INVERT 0FF AND # ANDCC,
IRQ-FLAG INVERT 0FF AND # ANDCC,  H-FLAG INVERT 0FF AND # ANDCC,
FIRQ-FLAG INVERT 0FF AND # ANDCC,  E-FLAG INVERT 0FF AND # ANDCC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 42DB CWAI #$FE .. CWAI #$7F
CREATE EXPECTED
3C C,  FE C,  3C C,  FD C,  3C C,  FB C,  3C C,  F7 C,  3C C,  EF C,
3C C,  DF C,  3C C,  BF C,  3C C,  7F C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
CY-FLAG INVERT 0FF AND # CWAI,  OV-FLAG INVERT 0FF AND # CWAI,
Z-FLAG INVERT 0FF AND # CWAI,  N-FLAG INVERT 0FF AND # CWAI,
IRQ-FLAG INVERT 0FF AND # CWAI,  H-FLAG INVERT 0FF AND # CWAI,
FIRQ-FLAG INVERT 0FF AND # CWAI,  E-FLAG INVERT 0FF AND # CWAI,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 42EB ORCC #$05 .. ANDCC #$00
CREATE EXPECTED
1A C,  05 C,  1A C,  50 C,  1C C,  AF C,  1A C,  FF C,  1C C,  00 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
CY-FLAG Z-FLAG + # ORCC,  IRQ-FLAG FIRQ-FLAG + # ORCC,
IRQ-FLAG FIRQ-FLAG + INVERT 0FF AND # ANDCC,  0FF # ORCC,
00 # ANDCC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 7a no offset ,X  (every instruction)
\ ------------------------------------------------------------
\ 42F5 LDA ,X .. CMPB ,X
CREATE EXPECTED
A6 C,  84 C,  E6 C,  84 C,  AB C,  84 C,  EB C,  84 C,  A0 C,  84 C,
E0 C,  84 C,  A1 C,  84 C,  E1 C,  84 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, LDA,  X 0, LDB,  X 0, ADDA,  X 0, ADDB,  X 0, SUBA,  X 0, SUBB,
X 0, CMPA,  X 0, CMPB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4305 ADCA ,X .. ORB ,X
CREATE EXPECTED
A9 C,  84 C,  E9 C,  84 C,  A2 C,  84 C,  E2 C,  84 C,  A4 C,  84 C,
E4 C,  84 C,  AA C,  84 C,  EA C,  84 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, ADCA,  X 0, ADCB,  X 0, SBCA,  X 0, SBCB,  X 0, ANDA,
X 0, ANDB,  X 0, ORA,  X 0, ORB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4315 EORA ,X .. ADDD ,X
CREATE EXPECTED
A8 C,  84 C,  E8 C,  84 C,  A5 C,  84 C,  E5 C,  84 C,  A7 C,  84 C,
E7 C,  84 C,  EC C,  84 C,  E3 C,  84 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, EORA,  X 0, EORB,  X 0, BITA,  X 0, BITB,  X 0, STA,  X 0, STB,
X 0, LDD,  X 0, ADDD,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4325 SUBD ,X .. LDU ,X
CREATE EXPECTED
A3 C,  84 C,  10 C,  A3 C,  84 C,  AE C,  84 C,  10 C,  AE C,  84 C,
AC C,  84 C,  10 C,  AC C,  84 C,  10 C,  EE C,  84 C,  EE C,  84 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, SUBD,  X 0, CMPD,  X 0, LDX,  X 0, LDY,  X 0, CMPX,  X 0, CMPY,
X 0, LDS,  X 0, LDU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4339 CMPS ,X .. DEC ,X
CREATE EXPECTED
11 C,  AC C,  84 C,  11 C,  A3 C,  84 C,  ED C,  84 C,  AF C,  84 C,
10 C,  AF C,  84 C,  10 C,  EF C,  84 C,  EF C,  84 C,  6A C,  84 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, CMPS,  X 0, CMPU,  X 0, STD,  X 0, STX,  X 0, STY,  X 0, STS,
X 0, STU,  X 0, DEC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 434D INC ,X .. COM ,X
CREATE EXPECTED
6C C,  84 C,  68 C,  84 C,  67 C,  84 C,  68 C,  84 C,  64 C,  84 C,
6F C,  84 C,  60 C,  84 C,  63 C,  84 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, INC,  X 0, ASL,  X 0, ASR,  X 0, LSL,  X 0, LSR,  X 0, CLR,
X 0, NEG,  X 0, COM,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 435D TST ,X .. JSR ,X
CREATE EXPECTED
6D C,  84 C,  69 C,  84 C,  66 C,  84 C,  6E C,  84 C,  AD C,  84 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, TST,  X 0, ROL,  X 0, ROR,  X 0, JMP,  X 0, JSR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 7b 8-bit offset OFFP8,Y
\ ------------------------------------------------------------
\ 4367 LDA OFFP8,Y .. CMPB OFFP8,Y
CREATE EXPECTED
A6 C,  A8 C,  5E C,  E6 C,  A8 C,  5E C,  AB C,  A8 C,  5E C,  EB C,
A8 C,  5E C,  A0 C,  A8 C,  5E C,  E0 C,  A8 C,  5E C,  A1 C,  A8 C,
5E C,  E1 C,  A8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , LDA,  Y 5E , LDB,  Y 5E , ADDA,  Y 5E , ADDB,  Y 5E , SUBA,
Y 5E , SUBB,  Y 5E , CMPA,  Y 5E , CMPB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 437F ADCA OFFP8,Y .. ORB OFFP8,Y
CREATE EXPECTED
A9 C,  A8 C,  5E C,  E9 C,  A8 C,  5E C,  A2 C,  A8 C,  5E C,  E2 C,
A8 C,  5E C,  A4 C,  A8 C,  5E C,  E4 C,  A8 C,  5E C,  AA C,  A8 C,
5E C,  EA C,  A8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , ADCA,  Y 5E , ADCB,  Y 5E , SBCA,  Y 5E , SBCB,  Y 5E , ANDA,
Y 5E , ANDB,  Y 5E , ORA,  Y 5E , ORB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4397 EORA OFFP8,Y .. ADDD OFFP8,Y
CREATE EXPECTED
A8 C,  A8 C,  5E C,  E8 C,  A8 C,  5E C,  A5 C,  A8 C,  5E C,  E5 C,
A8 C,  5E C,  A7 C,  A8 C,  5E C,  E7 C,  A8 C,  5E C,  EC C,  A8 C,
5E C,  E3 C,  A8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , EORA,  Y 5E , EORB,  Y 5E , BITA,  Y 5E , BITB,  Y 5E , STA,
Y 5E , STB,  Y 5E , LDD,  Y 5E , ADDD,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 43AF SUBD OFFP8,Y .. LDU OFFP8,Y
CREATE EXPECTED
A3 C,  A8 C,  5E C,  10 C,  A3 C,  A8 C,  5E C,  AE C,  A8 C,  5E C,
10 C,  AE C,  A8 C,  5E C,  AC C,  A8 C,  5E C,  10 C,  AC C,  A8 C,
5E C,  10 C,  EE C,  A8 C,  5E C,  EE C,  A8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , SUBD,  Y 5E , CMPD,  Y 5E , LDX,  Y 5E , LDY,  Y 5E , CMPX,
Y 5E , CMPY,  Y 5E , LDS,  Y 5E , LDU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 43CB CMPS OFFP8,Y .. DEC OFFP8,Y
CREATE EXPECTED
11 C,  AC C,  A8 C,  5E C,  11 C,  A3 C,  A8 C,  5E C,  ED C,  A8 C,
5E C,  AF C,  A8 C,  5E C,  10 C,  AF C,  A8 C,  5E C,  10 C,  EF C,
A8 C,  5E C,  EF C,  A8 C,  5E C,  6A C,  A8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , CMPS,  Y 5E , CMPU,  Y 5E , STD,  Y 5E , STX,  Y 5E , STY,
Y 5E , STS,  Y 5E , STU,  Y 5E , DEC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 43E7 INC OFFP8,Y .. COM OFFP8,Y
CREATE EXPECTED
6C C,  A8 C,  5E C,  68 C,  A8 C,  5E C,  67 C,  A8 C,  5E C,  68 C,
A8 C,  5E C,  64 C,  A8 C,  5E C,  6F C,  A8 C,  5E C,  60 C,  A8 C,
5E C,  63 C,  A8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , INC,  Y 5E , ASL,  Y 5E , ASR,  Y 5E , LSL,  Y 5E , LSR,
Y 5E , CLR,  Y 5E , NEG,  Y 5E , COM,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 43FF TST OFFP8,Y .. JSR OFFP8,Y
CREATE EXPECTED
6D C,  A8 C,  5E C,  69 C,  A8 C,  5E C,  66 C,  A8 C,  5E C,  6E C,
A8 C,  5E C,  AD C,  A8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , TST,  Y 5E , ROL,  Y 5E , ROR,  Y 5E , JMP,  Y 5E , JSR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 7c 16-bit offset OFFN16,U
\ ------------------------------------------------------------
\ 440E LDA OFFN16,U .. CMPB OFFN16,U
CREATE EXPECTED
A6 C,  C9 C,  ED C,  CC C,  E6 C,  C9 C,  ED C,  CC C,  AB C,  C9 C,
ED C,  CC C,  EB C,  C9 C,  ED C,  CC C,  A0 C,  C9 C,  ED C,  CC C,
E0 C,  C9 C,  ED C,  CC C,  A1 C,  C9 C,  ED C,  CC C,  E1 C,  C9 C,
ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -1234 , LDA,  U -1234 , LDB,  U -1234 , ADDA,  U -1234 , ADDB,
U -1234 , SUBA,  U -1234 , SUBB,  U -1234 , CMPA,  U -1234 , CMPB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 442E ADCA OFFN16,U .. ORB OFFN16,U
CREATE EXPECTED
A9 C,  C9 C,  ED C,  CC C,  E9 C,  C9 C,  ED C,  CC C,  A2 C,  C9 C,
ED C,  CC C,  E2 C,  C9 C,  ED C,  CC C,  A4 C,  C9 C,  ED C,  CC C,
E4 C,  C9 C,  ED C,  CC C,  AA C,  C9 C,  ED C,  CC C,  EA C,  C9 C,
ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -1234 , ADCA,  U -1234 , ADCB,  U -1234 , SBCA,  U -1234 , SBCB,
U -1234 , ANDA,  U -1234 , ANDB,  U -1234 , ORA,  U -1234 , ORB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 444E EORA OFFN16,U .. ADDD OFFN16,U
CREATE EXPECTED
A8 C,  C9 C,  ED C,  CC C,  E8 C,  C9 C,  ED C,  CC C,  A5 C,  C9 C,
ED C,  CC C,  E5 C,  C9 C,  ED C,  CC C,  A7 C,  C9 C,  ED C,  CC C,
E7 C,  C9 C,  ED C,  CC C,  EC C,  C9 C,  ED C,  CC C,  E3 C,  C9 C,
ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -1234 , EORA,  U -1234 , EORB,  U -1234 , BITA,  U -1234 , BITB,
U -1234 , STA,  U -1234 , STB,  U -1234 , LDD,  U -1234 , ADDD,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 446E SUBD OFFN16,U .. LDU OFFN16,U
CREATE EXPECTED
A3 C,  C9 C,  ED C,  CC C,  10 C,  A3 C,  C9 C,  ED C,  CC C,  AE C,
C9 C,  ED C,  CC C,  10 C,  AE C,  C9 C,  ED C,  CC C,  AC C,  C9 C,
ED C,  CC C,  10 C,  AC C,  C9 C,  ED C,  CC C,  10 C,  EE C,  C9 C,
ED C,  CC C,  EE C,  C9 C,  ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -1234 , SUBD,  U -1234 , CMPD,  U -1234 , LDX,  U -1234 , LDY,
U -1234 , CMPX,  U -1234 , CMPY,  U -1234 , LDS,  U -1234 , LDU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4492 CMPS OFFN16,U .. DEC OFFN16,U
CREATE EXPECTED
11 C,  AC C,  C9 C,  ED C,  CC C,  11 C,  A3 C,  C9 C,  ED C,  CC C,
ED C,  C9 C,  ED C,  CC C,  AF C,  C9 C,  ED C,  CC C,  10 C,  AF C,
C9 C,  ED C,  CC C,  10 C,  EF C,  C9 C,  ED C,  CC C,  EF C,  C9 C,
ED C,  CC C,  6A C,  C9 C,  ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -1234 , CMPS,  U -1234 , CMPU,  U -1234 , STD,  U -1234 , STX,
U -1234 , STY,  U -1234 , STS,  U -1234 , STU,  U -1234 , DEC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 44B6 INC OFFN16,U .. COM OFFN16,U
CREATE EXPECTED
6C C,  C9 C,  ED C,  CC C,  68 C,  C9 C,  ED C,  CC C,  67 C,  C9 C,
ED C,  CC C,  68 C,  C9 C,  ED C,  CC C,  64 C,  C9 C,  ED C,  CC C,
6F C,  C9 C,  ED C,  CC C,  60 C,  C9 C,  ED C,  CC C,  63 C,  C9 C,
ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -1234 , INC,  U -1234 , ASL,  U -1234 , ASR,  U -1234 , LSL,
U -1234 , LSR,  U -1234 , CLR,  U -1234 , NEG,  U -1234 , COM,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 44D6 TST OFFN16,U .. JSR OFFN16,U
CREATE EXPECTED
6D C,  C9 C,  ED C,  CC C,  69 C,  C9 C,  ED C,  CC C,  66 C,  C9 C,
ED C,  CC C,  6E C,  C9 C,  ED C,  CC C,  AD C,  C9 C,  ED C,  CC C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -1234 , TST,  U -1234 , ROL,  U -1234 , ROR,  U -1234 , JMP,
U -1234 , JSR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 8 register S
\ ------------------------------------------------------------
\ 44EA LDA A,X .. LDA ,X
CREATE EXPECTED
A6 C,  86 C,  A6 C,  85 C,  A6 C,  8B C,  A6 C,  80 C,  A6 C,  81 C,
A6 C,  82 C,  A6 C,  83 C,  A6 C,  84 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X A, LDA,  X B, LDA,  X D, LDA,  X ,+ LDA,  X ,++ LDA,  X -, LDA,
X --, LDA,  X 0, LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 44FA LDA A,Y .. LDA ,Y
CREATE EXPECTED
A6 C,  A6 C,  A6 C,  A5 C,  A6 C,  AB C,  A6 C,  A0 C,  A6 C,  A1 C,
A6 C,  A2 C,  A6 C,  A3 C,  A6 C,  A4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y A, LDA,  Y B, LDA,  Y D, LDA,  Y ,+ LDA,  Y ,++ LDA,  Y -, LDA,
Y --, LDA,  Y 0, LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 450A LDA A,U .. LDA ,U
CREATE EXPECTED
A6 C,  C6 C,  A6 C,  C5 C,  A6 C,  CB C,  A6 C,  C0 C,  A6 C,  C1 C,
A6 C,  C2 C,  A6 C,  C3 C,  A6 C,  C4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U A, LDA,  U B, LDA,  U D, LDA,  U ,+ LDA,  U ,++ LDA,  U -, LDA,
U --, LDA,  U 0, LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 451A LDA A,S .. LDA ,S
CREATE EXPECTED
A6 C,  E6 C,  A6 C,  E5 C,  A6 C,  EB C,  A6 C,  E0 C,  A6 C,  E1 C,
A6 C,  E2 C,  A6 C,  E3 C,  A6 C,  E4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S A, LDA,  S B, LDA,  S D, LDA,  S ,+ LDA,  S ,++ LDA,  S -, LDA,
S --, LDA,  S 0, LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 9a 5-bit offsets, S
\ ------------------------------------------------------------
\ 452A LDA 0,X .. LDA -1,Y
\ LDA 0,X: lwasm gives a 5-bit zero offset; the port emits the no-offset form
\ LDA 0,Y: lwasm gives a 5-bit zero offset; the port emits the no-offset form
CREATE EXPECTED
A6 C,  84 C,  A6 C,  01 C,  A6 C,  1F C,  A6 C,  0A C,  A6 C,  16 C,
A6 C,  A4 C,  A6 C,  21 C,  A6 C,  3F C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 00 , LDA,  X 01 , LDA,  X -01 , LDA,  X 0A , LDA,  X -0A , LDA,
Y 00 , LDA,  Y 01 , LDA,  Y -01 , LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4548 LDA OFFP5,Y .. LDA 0,S
\ LDA 0,S: lwasm gives a 5-bit zero offset; the port emits the no-offset form
\ LDA 0,U: lwasm gives a 5-bit zero offset; the port emits the no-offset form
CREATE EXPECTED
A6 C,  2A C,  A6 C,  36 C,  A6 C,  C4 C,  A6 C,  41 C,  A6 C,  5F C,
A6 C,  4A C,  A6 C,  56 C,  A6 C,  E4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 0A , LDA,  Y -0A , LDA,  U 00 , LDA,  U 01 , LDA,  U -01 , LDA,
U 0A , LDA,  U -0A , LDA,  S 00 , LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4574 LDA 1,S .. LDA OFFN5,S
CREATE EXPECTED
A6 C,  61 C,  A6 C,  7F C,  A6 C,  6A C,  A6 C,  76 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S 01 , LDA,  S -01 , LDA,  S 0A , LDA,  S -0A , LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 9b 8-bit offsets, S
\ ------------------------------------------------------------
\ 4534 LDA OFFP8,X .. LDA OFFN8,S
CREATE EXPECTED
A6 C,  88 C,  5E C,  A6 C,  88 C,  9C C,  A6 C,  A8 C,  5E C,  A6 C,
A8 C,  9C C,  A6 C,  C8 C,  5E C,  A6 C,  C8 C,  9C C,  A6 C,  E8 C,
5E C,  A6 C,  E8 C,  9C C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 5E , LDA,  X -64 , LDA,  Y 5E , LDA,  Y -64 , LDA,  U 5E , LDA,
U -64 , LDA,  S 5E , LDA,  S -64 , LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 9c 16-bit offsets, S
\ ------------------------------------------------------------
\ 453A LDA OFFP16,X .. LDA OFFN16,S
CREATE EXPECTED
A6 C,  89 C,  12 C,  34 C,  A6 C,  89 C,  ED C,  CC C,  A6 C,  A9 C,
12 C,  34 C,  A6 C,  A9 C,  ED C,  CC C,  A6 C,  C9 C,  12 C,  34 C,
A6 C,  C9 C,  ED C,  CC C,  A6 C,  E9 C,  12 C,  34 C,  A6 C,  E9 C,
ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 1234 , LDA,  X -1234 , LDA,  Y 1234 , LDA,  Y -1234 , LDA,
U 1234 , LDA,  U -1234 , LDA,  S 1234 , LDA,  S -1234 , LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 10a 8-bit forward and backward
\ ------------------------------------------------------------
\ 458A LDA *+$20,PCR .. LDB *-$40,PCR
CREATE EXPECTED
A6 C,  8C C,  1D C,  A6 C,  8C C,  DD C,  E6 C,  8C C,  3D C,  E6 C,
8C C,  BD C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 20 + ,PCR LDA,  HERE 20 - ,PCR LDA,  HERE 40 + ,PCR LDB,
HERE 40 - ,PCR LDB,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 10b 16-bit forward and backward
\ ------------------------------------------------------------
\ 4596 LDA *+$200,PCR .. LDD *-$1000,PCR
CREATE EXPECTED
A6 C,  8D C,  01 C,  FC C,  A6 C,  8D C,  FD C,  FC C,  EC C,  8D C,
0F C,  FC C,  EC C,  8D C,  EF C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + ,PCR LDA,  HERE 0200 - ,PCR LDA,  HERE 1000 + ,PCR LDD,
HERE 1000 - ,PCR LDD,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 10c other opcodes
\ ------------------------------------------------------------
\ 45A6 LDY *+$20,PCR .. LEAX *+$200,PCR
CREATE EXPECTED
10 C,  AE C,  8C C,  1C C,  AF C,  8C C,  DD C,  AD C,  8C C,  1D C,
30 C,  8C C,  1D C,  30 C,  8D C,  01 C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 20 + ,PCR LDY,  HERE 20 - ,PCR STX,  HERE 20 + ,PCR JSR,
HERE 20 + ,PCR LEAX,  HERE 0200 + ,PCR LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 11a indirect indexed, S
\ ------------------------------------------------------------
\ 45B7 LDA [,X] .. LDA [1,X]
\ LDA [0,X]: lwasm gives a 5-bit zero offset; the port emits the no-offset form
CREATE EXPECTED
A6 C,  94 C,  A6 C,  96 C,  A6 C,  95 C,  A6 C,  9B C,  A6 C,  91 C,
A6 C,  93 C,  A6 C,  94 C,  A6 C,  98 C,  01 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, [] LDA,  X A, [] LDA,  X B, [] LDA,  X D, [] LDA,
X ,++ [] LDA,  X --, [] LDA,  X 00 , [] LDA,  X 01 , [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 45C9 LDA [OFFP8,X] .. LDA [D,Y]
CREATE EXPECTED
A6 C,  98 C,  5E C,  A6 C,  98 C,  9C C,  A6 C,  99 C,  12 C,  34 C,
A6 C,  99 C,  ED C,  CC C,  A6 C,  B4 C,  A6 C,  B6 C,  A6 C,  B5 C,
A6 C,  BB C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 5E , [] LDA,  X -64 , [] LDA,  X 1234 , [] LDA,  X -1234 , [] LDA,
Y 0, [] LDA,  Y A, [] LDA,  Y B, [] LDA,  Y D, [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 45DF LDA [,Y++] .. LDA [OFFN16,Y]
\ LDA [0,Y]: lwasm gives a 5-bit zero offset; the port emits the no-offset form
CREATE EXPECTED
A6 C,  B1 C,  A6 C,  B3 C,  A6 C,  B4 C,  A6 C,  B8 C,  01 C,  A6 C,
B8 C,  5E C,  A6 C,  B8 C,  9C C,  A6 C,  B9 C,  12 C,  34 C,  A6 C,
B9 C,  ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y ,++ [] LDA,  Y --, [] LDA,  Y 00 , [] LDA,  Y 01 , [] LDA,
Y 5E , [] LDA,  Y -64 , [] LDA,  Y 1234 , [] LDA,  Y -1234 , [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 45F7 LDA [,U] .. LDA [1,U]
\ LDA [0,U]: lwasm gives a 5-bit zero offset; the port emits the no-offset form
CREATE EXPECTED
A6 C,  D4 C,  A6 C,  D6 C,  A6 C,  D5 C,  A6 C,  DB C,  A6 C,  D1 C,
A6 C,  D3 C,  A6 C,  D4 C,  A6 C,  D8 C,  01 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U 0, [] LDA,  U A, [] LDA,  U B, [] LDA,  U D, [] LDA,
U ,++ [] LDA,  U --, [] LDA,  U 00 , [] LDA,  U 01 , [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4609 LDA [OFFP8,U] .. LDA [D,S]
CREATE EXPECTED
A6 C,  D8 C,  5E C,  A6 C,  D8 C,  9C C,  A6 C,  D9 C,  12 C,  34 C,
A6 C,  D9 C,  ED C,  CC C,  A6 C,  F4 C,  A6 C,  F6 C,  A6 C,  F5 C,
A6 C,  FB C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U 5E , [] LDA,  U -64 , [] LDA,  U 1234 , [] LDA,  U -1234 , [] LDA,
S 0, [] LDA,  S A, [] LDA,  S B, [] LDA,  S D, [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 461F LDA [,S++] .. LDA [OFFN16,S]
\ LDA [0,S]: lwasm gives a 5-bit zero offset; the port emits the no-offset form
CREATE EXPECTED
A6 C,  F1 C,  A6 C,  F3 C,  A6 C,  F4 C,  A6 C,  F8 C,  01 C,  A6 C,
F8 C,  5E C,  A6 C,  F8 C,  9C C,  A6 C,  F9 C,  12 C,  34 C,  A6 C,
F9 C,  ED C,  CC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S ,++ [] LDA,  S --, [] LDA,  S 00 , [] LDA,  S 01 , [] LDA,
S 5E , [] LDA,  S -64 , [] LDA,  S 1234 , [] LDA,  S -1234 , [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 11b indirect PC relative
\ ------------------------------------------------------------
\ 4637 LDA [*+$20,PCR] .. LDA [*-$200,PCR]
CREATE EXPECTED
A6 C,  9C C,  1D C,  A6 C,  9C C,  DD C,  A6 C,  9D C,  01 C,  FC C,
A6 C,  9D C,  FD C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 20 + ,PCR [] LDA,  HERE 20 - ,PCR [] LDA,
HERE 0200 + ,PCR [] LDA,  HERE 0200 - ,PCR [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 11c extended indirect
\ ------------------------------------------------------------
\ 4645 LDA [ADDR16] .. CLR [ADDR16]
CREATE EXPECTED
A6 C,  9F C,  A5 C,  A5 C,  E6 C,  9F C,  A5 C,  A5 C,  A7 C,  9F C,
A5 C,  A5 C,  E7 C,  9F C,  A5 C,  A5 C,  AB C,  9F C,  A5 C,  A5 C,
6E C,  9F C,  A5 C,  A5 C,  AD C,  9F C,  A5 C,  A5 C,  6F C,  9F C,
A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 [] LDA,  0A5A5 [] LDB,  0A5A5 [] STA,  0A5A5 [] STB,
0A5A5 [] ADDA,  0A5A5 [] JMP,  0A5A5 [] JSR,  0A5A5 [] CLR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4665 INC [ADDR16] .. LDS [ADDR16]
CREATE EXPECTED
6C C,  9F C,  A5 C,  A5 C,  EC C,  9F C,  A5 C,  A5 C,  ED C,  9F C,
A5 C,  A5 C,  AE C,  9F C,  A5 C,  A5 C,  AF C,  9F C,  A5 C,  A5 C,
10 C,  AE C,  9F C,  A5 C,  A5 C,  10 C,  AF C,  9F C,  A5 C,  A5 C,
10 C,  EE C,  9F C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 [] INC,  0A5A5 [] LDD,  0A5A5 [] STD,  0A5A5 [] LDX,
0A5A5 [] STX,  0A5A5 [] LDY,  0A5A5 [] STY,  0A5A5 [] LDS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4688 STS [ADDR16] .. CMPU [ADDR16]
CREATE EXPECTED
10 C,  EF C,  9F C,  A5 C,  A5 C,  EE C,  9F C,  A5 C,  A5 C,  EF C,
9F C,  A5 C,  A5 C,  10 C,  A3 C,  9F C,  A5 C,  A5 C,  10 C,  AC C,
9F C,  A5 C,  A5 C,  11 C,  AC C,  9F C,  A5 C,  A5 C,  11 C,  A3 C,
9F C,  A5 C,  A5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
0A5A5 [] STS,  0A5A5 [] LDU,  0A5A5 [] STU,  0A5A5 [] CMPD,
0A5A5 [] CMPY,  0A5A5 [] CMPS,  0A5A5 [] CMPU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 11d indirect with other opcodes
\ ------------------------------------------------------------
\ 46A9 STY [OFFP8,X] .. CLR [OFFP8,X]
CREATE EXPECTED
10 C,  AF C,  98 C,  5E C,  10 C,  EE C,  98 C,  5E C,  11 C,  A3 C,
98 C,  5E C,  10 C,  A3 C,  98 C,  5E C,  6E C,  98 C,  5E C,  AD C,
98 C,  5E C,  6D C,  98 C,  5E C,  6F C,  98 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 5E , [] STY,  X 5E , [] LDS,  X 5E , [] CMPU,  X 5E , [] CMPD,
X 5E , [] JMP,  X 5E , [] JSR,  X 5E , [] TST,  X 5E , [] CLR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 12 LEAU
\ ------------------------------------------------------------
\ 46C5 LEAX ,X .. LEAX D,X
CREATE EXPECTED
30 C,  84 C,  30 C,  01 C,  30 C,  0A C,  30 C,  88 C,  9C C,  30 C,
89 C,  12 C,  34 C,  30 C,  86 C,  30 C,  85 C,  30 C,  8B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, LEAX,  X 01 , LEAX,  X 0A , LEAX,  X -64 , LEAX,
X 1234 , LEAX,  X A, LEAX,  X B, LEAX,  X D, LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 46D8 LEAX ,X+ .. LEAX ,Y
CREATE EXPECTED
30 C,  80 C,  30 C,  81 C,  30 C,  82 C,  30 C,  83 C,  30 C,  94 C,
30 C,  98 C,  5E C,  30 C,  91 C,  30 C,  A4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X ,+ LEAX,  X ,++ LEAX,  X -, LEAX,  X --, LEAX,  X 0, [] LEAX,
X 5E , [] LEAX,  X ,++ [] LEAX,  Y 0, LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 46E9 LEAX 1,Y .. LEAX ,Y+
CREATE EXPECTED
30 C,  21 C,  30 C,  2A C,  30 C,  A8 C,  9C C,  30 C,  A9 C,  12 C,
34 C,  30 C,  A6 C,  30 C,  A5 C,  30 C,  AB C,  30 C,  A0 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 01 , LEAX,  Y 0A , LEAX,  Y -64 , LEAX,  Y 1234 , LEAX,
Y A, LEAX,  Y B, LEAX,  Y D, LEAX,  Y ,+ LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 46FC LEAX ,Y++ .. LEAX 1,U
CREATE EXPECTED
30 C,  A1 C,  30 C,  A2 C,  30 C,  A3 C,  30 C,  B4 C,  30 C,  B8 C,
5E C,  30 C,  B1 C,  30 C,  C4 C,  30 C,  41 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y ,++ LEAX,  Y -, LEAX,  Y --, LEAX,  Y 0, [] LEAX,  Y 5E , [] LEAX,
Y ,++ [] LEAX,  U 0, LEAX,  U 01 , LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 470D LEAX OFFP5,U .. LEAX ,U++
CREATE EXPECTED
30 C,  4A C,  30 C,  C8 C,  9C C,  30 C,  C9 C,  12 C,  34 C,  30 C,
C6 C,  30 C,  C5 C,  30 C,  CB C,  30 C,  C0 C,  30 C,  C1 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U 0A , LEAX,  U -64 , LEAX,  U 1234 , LEAX,  U A, LEAX,  U B, LEAX,
U D, LEAX,  U ,+ LEAX,  U ,++ LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4720 LEAX ,-U .. LEAX OFFP5,S
CREATE EXPECTED
30 C,  C2 C,  30 C,  C3 C,  30 C,  D4 C,  30 C,  D8 C,  5E C,  30 C,
D1 C,  30 C,  E4 C,  30 C,  61 C,  30 C,  6A C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -, LEAX,  U --, LEAX,  U 0, [] LEAX,  U 5E , [] LEAX,
U ,++ [] LEAX,  S 0, LEAX,  S 01 , LEAX,  S 0A , LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4731 LEAX OFFN8,S .. LEAX ,-S
CREATE EXPECTED
30 C,  E8 C,  9C C,  30 C,  E9 C,  12 C,  34 C,  30 C,  E6 C,  30 C,
E5 C,  30 C,  EB C,  30 C,  E0 C,  30 C,  E1 C,  30 C,  E2 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S -64 , LEAX,  S 1234 , LEAX,  S A, LEAX,  S B, LEAX,  S D, LEAX,
S ,+ LEAX,  S ,++ LEAX,  S -, LEAX,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4744 LEAX ,--S .. LEAY OFFN8,X
CREATE EXPECTED
30 C,  E3 C,  30 C,  F4 C,  30 C,  F8 C,  5E C,  30 C,  F1 C,  31 C,
84 C,  31 C,  01 C,  31 C,  0A C,  31 C,  88 C,  9C C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S --, LEAX,  S 0, [] LEAX,  S 5E , [] LEAX,  S ,++ [] LEAX,
X 0, LEAY,  X 01 , LEAY,  X 0A , LEAY,  X -64 , LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4756 LEAY OFFP16,X .. LEAY ,--X
CREATE EXPECTED
31 C,  89 C,  12 C,  34 C,  31 C,  86 C,  31 C,  85 C,  31 C,  8B C,
31 C,  80 C,  31 C,  81 C,  31 C,  82 C,  31 C,  83 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 1234 , LEAY,  X A, LEAY,  X B, LEAY,  X D, LEAY,  X ,+ LEAY,
X ,++ LEAY,  X -, LEAY,  X --, LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4768 LEAY [,X] .. LEAY OFFP16,Y
CREATE EXPECTED
31 C,  94 C,  31 C,  98 C,  5E C,  31 C,  91 C,  31 C,  A4 C,  31 C,
21 C,  31 C,  2A C,  31 C,  A8 C,  9C C,  31 C,  A9 C,  12 C,  34 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, [] LEAY,  X 5E , [] LEAY,  X ,++ [] LEAY,  Y 0, LEAY,
Y 01 , LEAY,  Y 0A , LEAY,  Y -64 , LEAY,  Y 1234 , LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 477C LEAY A,Y .. LEAY [,Y]
CREATE EXPECTED
31 C,  A6 C,  31 C,  A5 C,  31 C,  AB C,  31 C,  A0 C,  31 C,  A1 C,
31 C,  A2 C,  31 C,  A3 C,  31 C,  B4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y A, LEAY,  Y B, LEAY,  Y D, LEAY,  Y ,+ LEAY,  Y ,++ LEAY,
Y -, LEAY,  Y --, LEAY,  Y 0, [] LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 478C LEAY [OFFP8,Y] .. LEAY A,U
CREATE EXPECTED
31 C,  B8 C,  5E C,  31 C,  B1 C,  31 C,  C4 C,  31 C,  41 C,  31 C,
4A C,  31 C,  C8 C,  9C C,  31 C,  C9 C,  12 C,  34 C,  31 C,  C6 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , [] LEAY,  Y ,++ [] LEAY,  U 0, LEAY,  U 01 , LEAY,
U 0A , LEAY,  U -64 , LEAY,  U 1234 , LEAY,  U A, LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 47A0 LEAY B,U .. LEAY [OFFP8,U]
CREATE EXPECTED
31 C,  C5 C,  31 C,  CB C,  31 C,  C0 C,  31 C,  C1 C,  31 C,  C2 C,
31 C,  C3 C,  31 C,  D4 C,  31 C,  D8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U B, LEAY,  U D, LEAY,  U ,+ LEAY,  U ,++ LEAY,  U -, LEAY,
U --, LEAY,  U 0, [] LEAY,  U 5E , [] LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 47B1 LEAY [,U++] .. LEAY B,S
CREATE EXPECTED
31 C,  D1 C,  31 C,  E4 C,  31 C,  61 C,  31 C,  6A C,  31 C,  E8 C,
9C C,  31 C,  E9 C,  12 C,  34 C,  31 C,  E6 C,  31 C,  E5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U ,++ [] LEAY,  S 0, LEAY,  S 01 , LEAY,  S 0A , LEAY,
S -64 , LEAY,  S 1234 , LEAY,  S A, LEAY,  S B, LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 47C4 LEAY D,S .. LEAY [,S++]
CREATE EXPECTED
31 C,  EB C,  31 C,  E0 C,  31 C,  E1 C,  31 C,  E2 C,  31 C,  E3 C,
31 C,  F4 C,  31 C,  F8 C,  5E C,  31 C,  F1 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S D, LEAY,  S ,+ LEAY,  S ,++ LEAY,  S -, LEAY,  S --, LEAY,
S 0, [] LEAY,  S 5E , [] LEAY,  S ,++ [] LEAY,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 47D5 LEAS ,X .. LEAS D,X
CREATE EXPECTED
32 C,  84 C,  32 C,  01 C,  32 C,  0A C,  32 C,  88 C,  9C C,  32 C,
89 C,  12 C,  34 C,  32 C,  86 C,  32 C,  85 C,  32 C,  8B C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, LEAS,  X 01 , LEAS,  X 0A , LEAS,  X -64 , LEAS,
X 1234 , LEAS,  X A, LEAS,  X B, LEAS,  X D, LEAS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 47E8 LEAS ,X+ .. LEAS ,Y
CREATE EXPECTED
32 C,  80 C,  32 C,  81 C,  32 C,  82 C,  32 C,  83 C,  32 C,  94 C,
32 C,  98 C,  5E C,  32 C,  91 C,  32 C,  A4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X ,+ LEAS,  X ,++ LEAS,  X -, LEAS,  X --, LEAS,  X 0, [] LEAS,
X 5E , [] LEAS,  X ,++ [] LEAS,  Y 0, LEAS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 47F9 LEAS 1,Y .. LEAS ,Y+
CREATE EXPECTED
32 C,  21 C,  32 C,  2A C,  32 C,  A8 C,  9C C,  32 C,  A9 C,  12 C,
34 C,  32 C,  A6 C,  32 C,  A5 C,  32 C,  AB C,  32 C,  A0 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 01 , LEAS,  Y 0A , LEAS,  Y -64 , LEAS,  Y 1234 , LEAS,
Y A, LEAS,  Y B, LEAS,  Y D, LEAS,  Y ,+ LEAS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 480C LEAS ,Y++ .. LEAS 1,U
CREATE EXPECTED
32 C,  A1 C,  32 C,  A2 C,  32 C,  A3 C,  32 C,  B4 C,  32 C,  B8 C,
5E C,  32 C,  B1 C,  32 C,  C4 C,  32 C,  41 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y ,++ LEAS,  Y -, LEAS,  Y --, LEAS,  Y 0, [] LEAS,  Y 5E , [] LEAS,
Y ,++ [] LEAS,  U 0, LEAS,  U 01 , LEAS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 481D LEAS OFFP5,U .. LEAS ,U++
CREATE EXPECTED
32 C,  4A C,  32 C,  C8 C,  9C C,  32 C,  C9 C,  12 C,  34 C,  32 C,
C6 C,  32 C,  C5 C,  32 C,  CB C,  32 C,  C0 C,  32 C,  C1 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U 0A , LEAS,  U -64 , LEAS,  U 1234 , LEAS,  U A, LEAS,  U B, LEAS,
U D, LEAS,  U ,+ LEAS,  U ,++ LEAS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4830 LEAS ,-U .. LEAS OFFP5,S
CREATE EXPECTED
32 C,  C2 C,  32 C,  C3 C,  32 C,  D4 C,  32 C,  D8 C,  5E C,  32 C,
D1 C,  32 C,  E4 C,  32 C,  61 C,  32 C,  6A C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U -, LEAS,  U --, LEAS,  U 0, [] LEAS,  U 5E , [] LEAS,
U ,++ [] LEAS,  S 0, LEAS,  S 01 , LEAS,  S 0A , LEAS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4841 LEAS OFFN8,S .. LEAS ,-S
CREATE EXPECTED
32 C,  E8 C,  9C C,  32 C,  E9 C,  12 C,  34 C,  32 C,  E6 C,  32 C,
E5 C,  32 C,  EB C,  32 C,  E0 C,  32 C,  E1 C,  32 C,  E2 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S -64 , LEAS,  S 1234 , LEAS,  S A, LEAS,  S B, LEAS,  S D, LEAS,
S ,+ LEAS,  S ,++ LEAS,  S -, LEAS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4854 LEAS ,--S .. LEAU OFFN8,X
CREATE EXPECTED
32 C,  E3 C,  32 C,  F4 C,  32 C,  F8 C,  5E C,  32 C,  F1 C,  33 C,
84 C,  33 C,  01 C,  33 C,  0A C,  33 C,  88 C,  9C C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S --, LEAS,  S 0, [] LEAS,  S 5E , [] LEAS,  S ,++ [] LEAS,
X 0, LEAU,  X 01 , LEAU,  X 0A , LEAU,  X -64 , LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4866 LEAU OFFP16,X .. LEAU ,--X
CREATE EXPECTED
33 C,  89 C,  12 C,  34 C,  33 C,  86 C,  33 C,  85 C,  33 C,  8B C,
33 C,  80 C,  33 C,  81 C,  33 C,  82 C,  33 C,  83 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 1234 , LEAU,  X A, LEAU,  X B, LEAU,  X D, LEAU,  X ,+ LEAU,
X ,++ LEAU,  X -, LEAU,  X --, LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4878 LEAU [,X] .. LEAU OFFP16,Y
CREATE EXPECTED
33 C,  94 C,  33 C,  98 C,  5E C,  33 C,  91 C,  33 C,  A4 C,  33 C,
21 C,  33 C,  2A C,  33 C,  A8 C,  9C C,  33 C,  A9 C,  12 C,  34 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0, [] LEAU,  X 5E , [] LEAU,  X ,++ [] LEAU,  Y 0, LEAU,
Y 01 , LEAU,  Y 0A , LEAU,  Y -64 , LEAU,  Y 1234 , LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 488C LEAU A,Y .. LEAU [,Y]
CREATE EXPECTED
33 C,  A6 C,  33 C,  A5 C,  33 C,  AB C,  33 C,  A0 C,  33 C,  A1 C,
33 C,  A2 C,  33 C,  A3 C,  33 C,  B4 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y A, LEAU,  Y B, LEAU,  Y D, LEAU,  Y ,+ LEAU,  Y ,++ LEAU,
Y -, LEAU,  Y --, LEAU,  Y 0, [] LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 489C LEAU [OFFP8,Y] .. LEAU A,U
CREATE EXPECTED
33 C,  B8 C,  5E C,  33 C,  B1 C,  33 C,  C4 C,  33 C,  41 C,  33 C,
4A C,  33 C,  C8 C,  9C C,  33 C,  C9 C,  12 C,  34 C,  33 C,  C6 C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
Y 5E , [] LEAU,  Y ,++ [] LEAU,  U 0, LEAU,  U 01 , LEAU,
U 0A , LEAU,  U -64 , LEAU,  U 1234 , LEAU,  U A, LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 48B0 LEAU B,U .. LEAU [OFFP8,U]
CREATE EXPECTED
33 C,  C5 C,  33 C,  CB C,  33 C,  C0 C,  33 C,  C1 C,  33 C,  C2 C,
33 C,  C3 C,  33 C,  D4 C,  33 C,  D8 C,  5E C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U B, LEAU,  U D, LEAU,  U ,+ LEAU,  U ,++ LEAU,  U -, LEAU,
U --, LEAU,  U 0, [] LEAU,  U 5E , [] LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 48C1 LEAU [,U++] .. LEAU B,S
CREATE EXPECTED
33 C,  D1 C,  33 C,  E4 C,  33 C,  61 C,  33 C,  6A C,  33 C,  E8 C,
9C C,  33 C,  E9 C,  12 C,  34 C,  33 C,  E6 C,  33 C,  E5 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
U ,++ [] LEAU,  S 0, LEAU,  S 01 , LEAU,  S 0A , LEAU,
S -64 , LEAU,  S 1234 , LEAU,  S A, LEAU,  S B, LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 48D4 LEAU D,S .. LEAU [,S++]
CREATE EXPECTED
33 C,  EB C,  33 C,  E0 C,  33 C,  E1 C,  33 C,  E2 C,  33 C,  E3 C,
33 C,  F4 C,  33 C,  F8 C,  5E C,  33 C,  F1 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
S D, LEAU,  S ,+ LEAU,  S ,++ LEAU,  S -, LEAU,  S --, LEAU,
S 0, [] LEAU,  S 5E , [] LEAU,  S ,++ [] LEAU,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ GROUP 13: short conditional branches, forward
\ ------------------------------------------------------------
\ 48E5 BRA .. BLS
CREATE EXPECTED
20 C,  01 C,  12 C,  21 C,  01 C,  12 C,  22 C,  01 C,  12 C,  23 C,
01 C,  12 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
NVR IF, NOP, ENDIF,  ALW IF, NOP, ENDIF,  LS IF, NOP, ENDIF,
HI IF, NOP, ENDIF,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 48F1 BCC .. BEQ
CREATE EXPECTED
24 C,  01 C,  12 C,  25 C,  01 C,  12 C,  26 C,  01 C,  12 C,  27 C,
01 C,  12 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
CS IF, NOP, ENDIF,  CC IF, NOP, ENDIF,  EQ IF, NOP, ENDIF,
NE IF, NOP, ENDIF,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 48FD BVC .. BMI
CREATE EXPECTED
28 C,  01 C,  12 C,  29 C,  01 C,  12 C,  2A C,  01 C,  12 C,  2B C,
01 C,  12 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
VS IF, NOP, ENDIF,  VC IF, NOP, ENDIF,  MI IF, NOP, ENDIF,
PL IF, NOP, ENDIF,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4909 BGE .. BLE
CREATE EXPECTED
2C C,  01 C,  12 C,  2D C,  01 C,  12 C,  2E C,  01 C,  12 C,  2F C,
01 C,  12 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
LT IF, NOP, ENDIF,  GE IF, NOP, ENDIF,  LE IF, NOP, ENDIF,
GT IF, NOP, ENDIF,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 13b backward branches (loop of one NOP)
\ ------------------------------------------------------------
\ 4915 BRA .. BLS
CREATE EXPECTED
12 C,  20 C,  FD C,  12 C,  21 C,  FD C,  12 C,  22 C,  FD C,  12 C,
23 C,  FD C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN, NOP, NVR UNTIL,  BEGIN, NOP, ALW UNTIL,
BEGIN, NOP, LS UNTIL,  BEGIN, NOP, HI UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4921 BCC .. BEQ
CREATE EXPECTED
12 C,  24 C,  FD C,  12 C,  25 C,  FD C,  12 C,  26 C,  FD C,  12 C,
27 C,  FD C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN, NOP, CS UNTIL,  BEGIN, NOP, CC UNTIL,  BEGIN, NOP, EQ UNTIL,
BEGIN, NOP, NE UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 492D BVC .. BMI
CREATE EXPECTED
12 C,  28 C,  FD C,  12 C,  29 C,  FD C,  12 C,  2A C,  FD C,  12 C,
2B C,  FD C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN, NOP, VS UNTIL,  BEGIN, NOP, VC UNTIL,  BEGIN, NOP, MI UNTIL,
BEGIN, NOP, PL UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4939 BGE .. BLE
CREATE EXPECTED
12 C,  2C C,  FD C,  12 C,  2D C,  FD C,  12 C,  2E C,  FD C,  12 C,
2F C,  FD C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN, NOP, LT UNTIL,  BEGIN, NOP, GE UNTIL,  BEGIN, NOP, LE UNTIL,
BEGIN, NOP, GT UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14a  EQ IF, NOP, THEN,
\ ------------------------------------------------------------
\ 14a  EQ IF, NOP, THEN,
CREATE EXPECTED
26 C,  01 C,  12 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
EQ  IF,  NOP,  THEN,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14b  EQ IF, NOP, ELSE, NOP, THEN,
\ ------------------------------------------------------------
\ 14b  EQ IF, NOP, ELSE, NOP, THEN,
CREATE EXPECTED
26 C,  03 C,  12 C,  20 C,  01 C,  12 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
EQ  IF,  NOP,  ELSE,  NOP,  THEN,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14c  BEGIN, NOP, EQ UNTIL,
\ ------------------------------------------------------------
\ 14c  BEGIN, NOP, EQ UNTIL,
CREATE EXPECTED
12 C,  26 C,  FD C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN,  NOP,  EQ  UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14d  BEGIN, NOP, EQ WHILE, NOP, REPEAT,
\ ------------------------------------------------------------
\ 14d  BEGIN, NOP, EQ WHILE, NOP, REPEAT,
CREATE EXPECTED
12 C,  26 C,  03 C,  12 C,  20 C,  FA C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN,  NOP,  EQ  WHILE,  NOP,  REPEAT,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14e  nested: CS IF, EQ IF, NOP, THEN, NOP, THEN,
\ ------------------------------------------------------------
\ 14e  nested: CS IF, EQ IF, NOP, THEN, NOP, THEN,
CREATE EXPECTED
24 C,  04 C,  26 C,  01 C,  12 C,  12 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
CS  IF,  EQ  IF,  NOP,  THEN,  NOP,  THEN,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14f  nested loop: BEGIN, BEGIN, NOP, EQ UNTIL, NOP, MI UNTIL,
\ ------------------------------------------------------------
\ 14f  nested loop: BEGIN, BEGIN, NOP, EQ UNTIL, NOP, MI UNTIL,
CREATE EXPECTED
12 C,  26 C,  FD C,  12 C,  2A C,  FA C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN,  BEGIN,  NOP,  EQ  UNTIL,  NOP,  MI  UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14g  loop containing IF,: BEGIN, EQ IF, NOP, THEN, CC UNTIL,
\ ------------------------------------------------------------
\ 14g  loop containing IF,: BEGIN, EQ IF, NOP, THEN, CC UNTIL,
CREATE EXPECTED
26 C,  01 C,  12 C,  25 C,  FB C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN,  EQ  IF,  NOP,  THEN,  CC  UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 14h  body of 20 NOPs
\ ------------------------------------------------------------
\ 14h  body of 20 NOPs
CREATE EXPECTED
26 C,  14 C,  12 14 BYTES,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
EQ IF,  14 NOPS,  THEN,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 15a indexed offset limits (LDA ,X)
\ ------------------------------------------------------------
\ 497E LDA 15,X .. LDA -128,X
CREATE EXPECTED
A6 C,  0F C,  A6 C,  88 C,  10 C,  A6 C,  10 C,  A6 C,  88 C,  EF C,
A6 C,  88 C,  7E C,  A6 C,  88 C,  7F C,  A6 C,  89 C,  00 C,  80 C,
A6 C,  88 C,  80 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 0F , LDA,  X 10 , LDA,  X -10 , LDA,  X -11 , LDA,  X 7E , LDA,
X 7F , LDA,  X 80 , LDA,  X -80 , LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ 4995 LDA -129,X .. LDA -32768,X
CREATE EXPECTED
A6 C,  89 C,  FF C,  7F C,  A6 C,  89 C,  7F C,  FF C,  A6 C,  89 C,
80 C,  00 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X -81 , LDA,  X 7FFF , LDA,  X -8000 , LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 15b indirect offset limits
\ ------------------------------------------------------------
\ 49A1 LDA [0,X] .. LDA [-129,X]
\ LDA [0,X]: lwasm gives a 5-bit zero offset; the port emits the no-offset form
CREATE EXPECTED
A6 C,  94 C,  A6 C,  98 C,  0F C,  A6 C,  98 C,  F0 C,  A6 C,  98 C,
7F C,  A6 C,  98 C,  80 C,  A6 C,  99 C,  00 C,  80 C,  A6 C,  99 C,
FF C,  7F C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
X 00 , [] LDA,  X 0F , [] LDA,  X -10 , [] LDA,  X 7F , [] LDA,
X -80 , [] LDA,  X 80 , [] LDA,  X -81 , [] LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 15c PC relative limits.  8-bit offset is measured from the end of
\ ------------------------------------------------------------
\ 49B8 LDA *+129,PCR .. LDA *-127,PCR
CREATE EXPECTED
A6 C,  8C C,  7E C,  A6 C,  8C C,  7F C,  A6 C,  8D C,  00 C,  7F C,
A6 C,  8C C,  81 C,  A6 C,  8C C,  80 C,  A6 C,  8D C,  FF C,  7E C,
A6 C,  8D C,  FF C,  7D C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 81 + ,PCR LDA,  HERE 82 + ,PCR LDA,  HERE 83 + ,PCR LDA,
HERE 7C - ,PCR LDA,  HERE 7D - ,PCR LDA,  HERE 7E - ,PCR LDA,
HERE 7F - ,PCR LDA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 15d branch distance limits, forward (NOP count: 126, 127)
\ ------------------------------------------------------------
\ forward branch over 126 NOPs
CREATE EXPECTED
26 C,  7E C,  12 7E BYTES,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
EQ IF,  7E NOPS,  THEN,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ forward branch over 127 NOPs
CREATE EXPECTED
26 C,  7F C,  12 7F BYTES,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
EQ IF,  7F NOPS,  THEN,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ 15e backward branch limit (128 NOPs then BNE: offset -128)
\ ------------------------------------------------------------
\ backward branch over 126 NOPs (offset -128)
CREATE EXPECTED
12 7E BYTES,  26 C,  80 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
BEGIN,  7E NOPS,  EQ UNTIL,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ ------------------------------------------------------------
\ Error cases: each must THROW 3 and leave the assembler usable.
\ ------------------------------------------------------------
ALSO ASSEMBLER
T{ 0A5 # ' STA, CATCH NIP -> 3 }T
DEFAULT
T{ 0A5 # ' STB, CATCH NIP -> 3 }T
DEFAULT
T{ 0A5 # ' JMP, CATCH NIP -> 3 }T
DEFAULT
T{ 0A5 # ' CLR, CATCH NIP -> 3 }T
DEFAULT
T{ 0A5 # ' STX, CATCH NIP -> 3 }T
DEFAULT
T{ 5 ' PSHS, CATCH NIP -> 3 }T
DEFAULT
T{ 5 <> ' PULU, CATCH NIP -> 3 }T
DEFAULT
T{ 5 ' LEAX, CATCH NIP -> 3 }T
DEFAULT
T{ 5 <> ' LEAU, CATCH NIP -> 3 }T
DEFAULT
T{ X ,+ ' [] CATCH NIP -> 3 }T
DEFAULT
T{ Y -, ' [] CATCH NIP -> 3 }T
DEFAULT
T{ A ' 0, CATCH NIP -> 3 }T
DEFAULT
T{ PC ' A, CATCH NIP -> 3 }T
DEFAULT
T{ D ' ,++ CATCH NIP -> 3 }T
DEFAULT
T{ CCR 5 ' , CATCH NIP NIP -> 3 }T
DEFAULT
T{ EQ IF, 80 NOPS, ' ENDIF, CATCH NIP NIP -> 3 }T
T{ BEGIN, 80 NOPS, EQ ' UNTIL, CATCH NIP NIP NIP -> 3 }T
PREVIOUS

HEX
\ ============================================================
\ Part 3: simple branch instructions, addr Bcc, / addr LBcc,
\ Targets are HERE-relative so the expected bytes are fixed.
\ Short: offset = d - 2.  Long: offset = d - 3 (LBRA, LBSR) or d - 4.
\ ============================================================

\ short: BRA BRN BHI at +10, -10, 0
CREATE EXPECTED
20 C,  0E C,  20 C,  EE C,  20 C,  FE C,  21 C,  0E C,  21 C,  EE C,
21 C,  FE C,  22 C,  0E C,  22 C,  EE C,  22 C,  FE C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 10 + BRA,  HERE 10 - BRA,  HERE BRA,  HERE 10 + BRN,
HERE 10 - BRN,  HERE BRN,  HERE 10 + BHI,  HERE 10 - BHI,  HERE BHI,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short: BLS BCC BCS at +10, -10, 0
CREATE EXPECTED
23 C,  0E C,  23 C,  EE C,  23 C,  FE C,  24 C,  0E C,  24 C,  EE C,
24 C,  FE C,  25 C,  0E C,  25 C,  EE C,  25 C,  FE C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 10 + BLS,  HERE 10 - BLS,  HERE BLS,  HERE 10 + BCC,
HERE 10 - BCC,  HERE BCC,  HERE 10 + BCS,  HERE 10 - BCS,  HERE BCS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short: BHS BLO BNE at +10, -10, 0
CREATE EXPECTED
24 C,  0E C,  24 C,  EE C,  24 C,  FE C,  25 C,  0E C,  25 C,  EE C,
25 C,  FE C,  26 C,  0E C,  26 C,  EE C,  26 C,  FE C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 10 + BHS,  HERE 10 - BHS,  HERE BHS,  HERE 10 + BLO,
HERE 10 - BLO,  HERE BLO,  HERE 10 + BNE,  HERE 10 - BNE,  HERE BNE,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short: BEQ BVC BVS at +10, -10, 0
CREATE EXPECTED
27 C,  0E C,  27 C,  EE C,  27 C,  FE C,  28 C,  0E C,  28 C,  EE C,
28 C,  FE C,  29 C,  0E C,  29 C,  EE C,  29 C,  FE C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 10 + BEQ,  HERE 10 - BEQ,  HERE BEQ,  HERE 10 + BVC,
HERE 10 - BVC,  HERE BVC,  HERE 10 + BVS,  HERE 10 - BVS,  HERE BVS,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short: BPL BMI BGE at +10, -10, 0
CREATE EXPECTED
2A C,  0E C,  2A C,  EE C,  2A C,  FE C,  2B C,  0E C,  2B C,  EE C,
2B C,  FE C,  2C C,  0E C,  2C C,  EE C,  2C C,  FE C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 10 + BPL,  HERE 10 - BPL,  HERE BPL,  HERE 10 + BMI,
HERE 10 - BMI,  HERE BMI,  HERE 10 + BGE,  HERE 10 - BGE,  HERE BGE,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short: BLT BGT BLE at +10, -10, 0
CREATE EXPECTED
2D C,  0E C,  2D C,  EE C,  2D C,  FE C,  2E C,  0E C,  2E C,  EE C,
2E C,  FE C,  2F C,  0E C,  2F C,  EE C,  2F C,  FE C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 10 + BLT,  HERE 10 - BLT,  HERE BLT,  HERE 10 + BGT,
HERE 10 - BGT,  HERE BGT,  HERE 10 + BLE,  HERE 10 - BLE,  HERE BLE,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short: BSR at +10, -10, 0
CREATE EXPECTED
8D C,  0E C,  8D C,  EE C,  8D C,  FE C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 10 + BSR,  HERE 10 - BSR,  HERE BSR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short limits: BRA at +81 (offset 7F) and -7E (offset 80)
CREATE EXPECTED
20 C,  7F C,  20 C,  80 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 81 + BRA,  HERE 7E - BRA,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short limits: BEQ at +81 (offset 7F) and -7E (offset 80)
CREATE EXPECTED
27 C,  7F C,  27 C,  80 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 81 + BEQ,  HERE 7E - BEQ,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ short limits: BSR at +81 (offset 7F) and -7E (offset 80)
CREATE EXPECTED
8D C,  7F C,  8D C,  80 C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 81 + BSR,  HERE 7E - BSR,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ long: LBRA LBSR LBRN at +200, -200, 0
CREATE EXPECTED
16 C,  01 C,  FD C,  16 C,  FD C,  FD C,  16 C,  FF C,  FD C,  17 C,
01 C,  FD C,  17 C,  FD C,  FD C,  17 C,  FF C,  FD C,  10 C,  21 C,
01 C,  FC C,  10 C,  21 C,  FD C,  FC C,  10 C,  21 C,  FF C,  FC C,
NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + LBRA,  HERE 0200 - LBRA,  HERE LBRA,  HERE 0200 + LBSR,
HERE 0200 - LBSR,  HERE LBSR,  HERE 0200 + LBRN,  HERE 0200 - LBRN,
HERE LBRN,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ long: LBHI LBLS LBCC at +200, -200, 0
CREATE EXPECTED
10 C,  22 C,  01 C,  FC C,  10 C,  22 C,  FD C,  FC C,  10 C,  22 C,
FF C,  FC C,  10 C,  23 C,  01 C,  FC C,  10 C,  23 C,  FD C,  FC C,
10 C,  23 C,  FF C,  FC C,  10 C,  24 C,  01 C,  FC C,  10 C,  24 C,
FD C,  FC C,  10 C,  24 C,  FF C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + LBHI,  HERE 0200 - LBHI,  HERE LBHI,  HERE 0200 + LBLS,
HERE 0200 - LBLS,  HERE LBLS,  HERE 0200 + LBCC,  HERE 0200 - LBCC,
HERE LBCC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ long: LBCS LBHS LBLO at +200, -200, 0
CREATE EXPECTED
10 C,  25 C,  01 C,  FC C,  10 C,  25 C,  FD C,  FC C,  10 C,  25 C,
FF C,  FC C,  10 C,  24 C,  01 C,  FC C,  10 C,  24 C,  FD C,  FC C,
10 C,  24 C,  FF C,  FC C,  10 C,  25 C,  01 C,  FC C,  10 C,  25 C,
FD C,  FC C,  10 C,  25 C,  FF C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + LBCS,  HERE 0200 - LBCS,  HERE LBCS,  HERE 0200 + LBHS,
HERE 0200 - LBHS,  HERE LBHS,  HERE 0200 + LBLO,  HERE 0200 - LBLO,
HERE LBLO,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ long: LBNE LBEQ LBVC at +200, -200, 0
CREATE EXPECTED
10 C,  26 C,  01 C,  FC C,  10 C,  26 C,  FD C,  FC C,  10 C,  26 C,
FF C,  FC C,  10 C,  27 C,  01 C,  FC C,  10 C,  27 C,  FD C,  FC C,
10 C,  27 C,  FF C,  FC C,  10 C,  28 C,  01 C,  FC C,  10 C,  28 C,
FD C,  FC C,  10 C,  28 C,  FF C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + LBNE,  HERE 0200 - LBNE,  HERE LBNE,  HERE 0200 + LBEQ,
HERE 0200 - LBEQ,  HERE LBEQ,  HERE 0200 + LBVC,  HERE 0200 - LBVC,
HERE LBVC,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ long: LBVS LBPL LBMI at +200, -200, 0
CREATE EXPECTED
10 C,  29 C,  01 C,  FC C,  10 C,  29 C,  FD C,  FC C,  10 C,  29 C,
FF C,  FC C,  10 C,  2A C,  01 C,  FC C,  10 C,  2A C,  FD C,  FC C,
10 C,  2A C,  FF C,  FC C,  10 C,  2B C,  01 C,  FC C,  10 C,  2B C,
FD C,  FC C,  10 C,  2B C,  FF C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + LBVS,  HERE 0200 - LBVS,  HERE LBVS,  HERE 0200 + LBPL,
HERE 0200 - LBPL,  HERE LBPL,  HERE 0200 + LBMI,  HERE 0200 - LBMI,
HERE LBMI,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ long: LBGE LBLT LBGT at +200, -200, 0
CREATE EXPECTED
10 C,  2C C,  01 C,  FC C,  10 C,  2C C,  FD C,  FC C,  10 C,  2C C,
FF C,  FC C,  10 C,  2D C,  01 C,  FC C,  10 C,  2D C,  FD C,  FC C,
10 C,  2D C,  FF C,  FC C,  10 C,  2E C,  01 C,  FC C,  10 C,  2E C,
FD C,  FC C,  10 C,  2E C,  FF C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + LBGE,  HERE 0200 - LBGE,  HERE LBGE,  HERE 0200 + LBLT,
HERE 0200 - LBLT,  HERE LBLT,  HERE 0200 + LBGT,  HERE 0200 - LBGT,
HERE LBGT,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ long: LBLE at +200, -200, 0
CREATE EXPECTED
10 C,  2F C,  01 C,  FC C,  10 C,  2F C,  FD C,  FC C,  10 C,  2F C,
FF C,  FC C,  NULL,
ALSO ASSEMBLER
CREATE ACTUAL
HERE 0200 + LBLE,  HERE 0200 - LBLE,  HERE LBLE,
PREVIOUS
T{ EXPECTED ACTUAL == -> TRUE }T

\ Out of range short branches THROW 3.
ALSO ASSEMBLER
T{ HERE 82 + ' BRA, CATCH NIP -> 3 }T
T{ HERE 7F - ' BNE, CATCH NIP -> 3 }T
T{ HERE 1000 + ' BSR, CATCH NIP -> 3 }T
PREVIOUS

HEX
TEST-END
M35
