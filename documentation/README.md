# Documentation: assembler6809

## Introduction

Assembler6809 is intended to optimise forth inner loops 
or to provide good performance when interacting with fast hardware.  
It is convenient for developing small code blocks with 
short branches or for writing code that is better expressed without 
complicated forth stack manipulations.
 
The 6809 microprocessor is notable for possessing 
a very rich instructions set, with some complicated addressing modes.
This is a hardware based solution for many programming problems,
if you can figure it out. Figuring it out is helped by the 
examples, the unit test suite and the following 
assembler syntax conversion table. 
 
This table was extracted from the Rodriguez articles included here.
They are also worth reading in full.


## Instruction Set

### Inherent Addressing Group
                            
| Motorola  | Forth  | Motorola | Forth |
| --------  | -----  | -------- | ----- |
| ABX       |  ABX,  | MUL      | MUL,  |
| ASLA      | ASLA,  | NEGA     | NEGA, |
| ASLB      | ASLB,  | NEGB     | NEGB, |
| ASRA      | ASRA,  | NOP      | NOP,  |
| ASRB      | ASRB,  | ORA      | ORA,  |
| CLRA      | CLRA,  | ORB      | ORB,  |
| CLRB      | CLRB,  | ROLA     | ROLA, |
| COMA      | COMA,  | ROLB     | ROLB, |
| COMB      | COMB,  | RORA     | RORA, |
| DAA       |  DAA,  | RORB     | RORB, |
| DECA      | DECA,  | RTI      | RTI,  |
| DECB      | DECB,  | RTS      | RTS,  |
| INCA      | INCA,  | SEX      | SEX,  |
| INCB      | INCB,  | SWI      | SWI,  |
| LSLA      | LSLA,  | SWI2     | SWI2, |
| LSLB      | LSLB,  | SWI3     | SWI3, |
| LSRA      | LSRA,  | SYNC     | SYNC, |
| LSRB      | LSRB,  | TSTA     | TSTA, |
|           |        | TSTB     | TSTB, |


### Register-Register Group

| Motorola  | Forth   | Motorola | Forth    |
| --------  | -----   | -------- | -----    |
| EXG s,d   | s d EXG | TFR s,d  | s d TFR  |


### Immediate-Addressing-Only Group

| Motorola  | Forth      | Motorola  | Forth     |
| --------  | -----      | --------  | -----     |
| ANDCC #n  | n # ANDCC, | PSHS regs | n # PSHS, |
| CWAI #n   | n # CWAI   | PSHU regs | n # PSHU, |
| ORCC #n   | n # ORCC,  | PULS regs | n # PULS, |
|           |            | PULU regs | n # PULU, |

Note: 
Motorola allows the PSH and PUL instructions to
contain a register list.  The Forth assembler requires
the programmer to compute the bit mask for this list
and supply it as an immediate argument.


### Indexed-Addressing-Only Group (with example addressing modes)

| Motorola  | Forth        | Motorola    | Forth          |
| --------  | -----        | --------    | -----          |
| LEAS D,U  | U ,D LEAS,   | LEAX [,S++] | S ,++ [] LEAX, |
| LEAU -5,Y | Y -5 , LEAU, | LEAY [1234] | 1234 [] LEAY,  |

 ### General-Addressing Group (with example addressing modes)

| Motorola     | Forth          | Motorola     | Forth           |
| --------     | -----          | --------     | -----           |
| ADCA #20     | 20 # ADCA,     | LDA #20      | 20 # LDA,       |
| ADCB <30     | 30 <> ADCB,    | LDB <30      | 30 <> LDB,      |
| ADDA 2000    | 2000 ADDA,     | LDD 2000     | 2000 LDD,       |
| ADDB [1030]  | 1030 [] ADDB,  | LDS [1030]   | 1030 [] LDS,    |
| ADDD ,S      | S 0, ADDD,     | LDU ,X       | X 0, LDU,       |
| ANDA 23,U    | U 23 , ANDA,   | LDX 23,Y     | Y 23 , LDX,     |
| ANDB A,X     | X A, ANDB,     | LDY A,S      | S A, LDY,       |
| ASL  B,Y     | Y B, ASL,      | LSL B,U      | U B, LSL,       |
| ASR  D,X     | X D, ASR,      | LSR D,S      | S D, LSR,       |
| BITA ,S+     | S ,+ BITA,     | NEG ,X+      | X ,+ NEG,       |
| BITB ,X++    | X ,++ BITB,    | ORA ,S++     | S ,++ ORA,      |
| CLR  ,Y-     | Y ,- CLR,      | ORB ,U-      | U ,- ORB,       |
| CMPA ,U--    | U ,-- CMPA,    | ROL ,Y--     | Y ,-- ROL,      |
| CMPB -5,PCR  | -5 ,PCR CMPB,  | ROR 12,PCR   | 12 ,PCR ROR,    |
| CMPD [,Y]    | Y 0, [] CMPD,  | SBCA [,U]    | U 0, [] SBCA,   |
| CMPS [7,Y]   | Y 7 , [] CMPS, | SBCB [7,U]   | U 7 , [] SBCB,  |
| CMPU [A,S]   | S A, [] CMPU,  | STA [A,X]    | X A, [] STA,    |
| CMPX [B,U]   | U B, [] CMPX,  | STB [B,Y]    | Y B, [] STB,    |
| CMPY [D,X]   | X D, [] CMPY,  | STD [D,S]    | S D, [] STD,    |
| EORA [,Y+]   | Y ,+ [] EORA,  | STS [,U+]    | U ,+ [] STS,    |
| EORB [,U++]  | U ,++ [] EORB, | STU [,Y++]   | Y ,++ [] STU,   |
| COM  [,S-]   | S ,- [] COM,   | STX [,S-]    | S ,- [] STX,    |
| DEC [,X--]   | X ,-- [] DEC,  | STY [,X--]   | X ,-- [] STY,   |
| INC [5,PCR]  | 5 ,PCR [] INC, | SUBA [3,PCR] | 3 ,PCR [] SUBA, |
| JMP [300]    | 300 [] JMP,    | SUBB [300]   | 300 [] SUBB,    |
| JSR 1234     | 1234 JSR,      | SUBD 1234    | 1234 SUBD,      |
|              |                |  TST #2      |  2 # TST,       |

Note: 
in the Forth assembler,
    #  signifies Immediate addressing, and
    <>  signifies Direct addressing.

Note:
Many instructions do not allow immediate addressing.
Refer to the Motorola data sheet.


### Branch instructions

| Motorola  | Forth     | Motorola  | Forth     |
| --------  | -----     | --------  | -----     |
| BCC label | adrs BCC, | BLT label | adrs BLT, |
| BCS label | adrs BCS, | BMI label | adrs BMI, |
| BEQ label | adrs BEQ, | BNE label | adrs BNE, |
| BGE label | adrs BGE, | BPL label | adrs BPL, |
| BGT label | adrs BGT, | BRA label | adrs BRA, |
| BHI label | adrs BHI, | BRN label | adrs BRN, |
| BHS label | adrs BHS, | BSR label | adrs BSR, |
| BLE label | adrs BLE, | BVC label | adrs BVC, |
| BLO label | adrs BLO, | BVS label | adrs BVS, |
| BLS label | adrs BLS, |           |           |

Note: 
The branch instructions in the Forth assembler expect
an absolute address.  The relative offset is computed,
and the "long branch" form of the instruction is used
if necessary.

 
### Indexed Addressing Modes
 
#### Direct 
 
| Type          | Motorola  | Forth   | Postbyte |
| ------------- | --------- | --------|--------- |
| no offset     | ,r        | r 0,    | 1rri0100 |
| 5 bit offset  | n,r       | r n ,   | defaults</BR>to 8-bit| 0rrnnnnn |
| 8 bit offset  | n,r       | r n ,   | 1rri1000 |
| 16 bit offset | n,r       | r n ,   | 1rri1001 |
|               |           |         |          |
| A-reg offset  | A,r       | r A,    | 1rri0110 |
| B-reg offset  | B,r       | r B,    | 1rri0101 |
| D-reg offset  | D,r       | r D,    | 1rri1011 |
|               |           |         |          |
| incr. by 1    | ,r+       | r ,+    | 1rr00000 |
| incr. by 2    | ,r++      | r ,++   | 1rri0001 |
| decr. by 1    | ,r-       | r ,-    | 1rr00010 |
| decr. by 2    | ,r--      | r ,--   | 1rri0011 |
|               |           |         |          |
| PC ofs. 8 bit | n,PCR     | n ,PCR  | 1xxi1100 |
| PC ofs. 16 bit| n,PCR     | n ,PCR  | 1xxi1101 |
|               |           |         |          |
| 16 bit address| not</BR>allowed | not</BR>allowed | 10011111 |

Note: 
where n = a signed integer value,
      r = X (00), Y (01), U (10), or S (11)
      x = don't care


 #### Indirect 

| Type           | Motorola=  | Forth | Postbyte |
| -------------  | ---------  |---------    |--------- |
| no offset      | [,r]       | r 0, []     | 1rri0100 |
| 5 bit offset   | defaults</BR>to 8-bit | defaults</BR>to 8-bit| 0rrnnnnn |
| 8 bit offset   | [n,r]      | r n , []    | 1rri1000 |
| 16 bit offset  | [n,r]      | r n , []    | 1rri1001 |
|                |            |             |          |
| A-reg offset   | [A,r]      | r A, []     | 1rri0110 |
| B-reg offset   | [B,r]      | r B, []     | 1rri0101 |
| D-reg offset   | [D,r]      | r D, []     | 1rri1011 |
|                |            |             |          |
| incr. by 1     | not</BR>allowed | not</BR>allowed | 1rr00000 |
| incr. by 2     | [,r++]     | r ,++ []    | 1rri0001 |
| decr. by 1     | not</BR>allowed | not</BR>allowed | 1rr00010 |
| decr. by 2     | [,r--]     | r ,-- []    | 1rri0011 |
|                |            |             |          |
| PC ofs. 8 bit  | [n,PCR]    | n ,PCR []   | 1xxi1100 |
| PC ofs. 16 bit | [n,PCR]    | n ,PCR []   | 1xxi1101 |
|                |            |             |          |
| 16 bit address | Not</BR>allowed  | [n]  | n [] | 10011111 |

Note: 
where n = a signed integer value,
      r = X (00), Y (01), U (10), or S (11)
      x = don't care









