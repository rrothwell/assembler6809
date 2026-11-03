; ============================================================
; 6809 opcodes, every addressing mode exercised (part 2).
; Companion to opcode6809.asm (inherent + PSH/PUL/CWAI/ANDCC/ORCC).
;
; Assemble:  lwasm -l -f raw -o opcode6809b.bin opcode6809b.asm
; Explicit modes are forced so lwasm cannot choose differently:
;    #   immediate     <   direct     >   extended
;    indexed offsets are left for lwasm to size (5, 8 or 16 bit).
; ============================================================

IMMVALUE EQU   $A5
ADDR16   EQU   $A5A5
ADDR8    EQU   $7B
OFFP8    EQU   $5E               ; +94,  needs 8 bits
OFFN8    EQU   -$64              ; -100, needs 8 bits
OFFP16   EQU   $1234             ; needs 16 bits
OFFN16   EQU   -$1234            ; needs 16 bits
OFFP5    EQU   $0A               ; +10,  fits 5 bits
OFFN5    EQU   -$0A              ; -10,  fits 5 bits

        ORG   $4000

; ------------------------------------------------------------
; GROUP 1: 8-bit accumulator instructions, simple modes
; ------------------------------------------------------------
; 1a immediate
        LDA   #IMMVALUE
        LDB   #IMMVALUE
        ADDA  #IMMVALUE
        ADDB  #IMMVALUE
        SUBA  #IMMVALUE
        SUBB  #IMMVALUE
        CMPA  #IMMVALUE
        CMPB  #IMMVALUE
        ADCA  #IMMVALUE
        ADCB  #IMMVALUE
        SBCA  #IMMVALUE
        SBCB  #IMMVALUE
        ANDA  #IMMVALUE
        ANDB  #IMMVALUE
        ORA   #IMMVALUE
        ORB   #IMMVALUE
        EORA  #IMMVALUE
        EORB  #IMMVALUE
        BITA  #IMMVALUE
        BITB  #IMMVALUE

; 1b direct
        LDA   <ADDR8
        LDB   <ADDR8
        ADDA  <ADDR8
        ADDB  <ADDR8
        SUBA  <ADDR8
        SUBB  <ADDR8
        CMPA  <ADDR8
        CMPB  <ADDR8
        ADCA  <ADDR8
        ADCB  <ADDR8
        SBCA  <ADDR8
        SBCB  <ADDR8
        ANDA  <ADDR8
        ANDB  <ADDR8
        ORA   <ADDR8
        ORB   <ADDR8
        EORA  <ADDR8
        EORB  <ADDR8
        BITA  <ADDR8
        BITB  <ADDR8
        STA   <ADDR8
        STB   <ADDR8

; 1c extended
        LDA   >ADDR16
        LDB   >ADDR16
        ADDA  >ADDR16
        ADDB  >ADDR16
        SUBA  >ADDR16
        SUBB  >ADDR16
        CMPA  >ADDR16
        CMPB  >ADDR16
        ADCA  >ADDR16
        ADCB  >ADDR16
        SBCA  >ADDR16
        SBCB  >ADDR16
        ANDA  >ADDR16
        ANDB  >ADDR16
        ORA   >ADDR16
        ORB   >ADDR16
        EORA  >ADDR16
        EORB  >ADDR16
        BITA  >ADDR16
        BITB  >ADDR16
        STA   >ADDR16
        STB   >ADDR16


; ------------------------------------------------------------
; GROUP 2: 16-bit register instructions, simple modes
; ------------------------------------------------------------
; 2a immediate
        LDD   #ADDR16
        ADDD  #ADDR16
        SUBD  #ADDR16
        CMPD  #ADDR16
        LDX   #ADDR16
        LDY   #ADDR16
        CMPX  #ADDR16
        CMPY  #ADDR16
        LDS   #ADDR16
        LDU   #ADDR16
        CMPS  #ADDR16
        CMPU  #ADDR16

; 2b direct
        LDD   <ADDR8
        ADDD  <ADDR8
        SUBD  <ADDR8
        CMPD  <ADDR8
        LDX   <ADDR8
        LDY   <ADDR8
        CMPX  <ADDR8
        CMPY  <ADDR8
        LDS   <ADDR8
        LDU   <ADDR8
        CMPS  <ADDR8
        CMPU  <ADDR8
        STD   <ADDR8
        STX   <ADDR8
        STY   <ADDR8
        STS   <ADDR8
        STU   <ADDR8

; 2c extended
        LDD   >ADDR16
        ADDD  >ADDR16
        SUBD  >ADDR16
        CMPD  >ADDR16
        LDX   >ADDR16
        LDY   >ADDR16
        CMPX  >ADDR16
        CMPY  >ADDR16
        LDS   >ADDR16
        LDU   >ADDR16
        CMPS  >ADDR16
        CMPU  >ADDR16
        STD   >ADDR16
        STX   >ADDR16
        STY   >ADDR16
        STS   >ADDR16
        STU   >ADDR16


; ------------------------------------------------------------
; GROUP 3: read-modify-write, JMP, JSR (no immediate mode)
; ------------------------------------------------------------
; 3a direct
        DEC   <ADDR8
        INC   <ADDR8
        ASL   <ADDR8
        ASR   <ADDR8
        LSL   <ADDR8
        LSR   <ADDR8
        CLR   <ADDR8
        NEG   <ADDR8
        COM   <ADDR8
        TST   <ADDR8
        ROL   <ADDR8
        ROR   <ADDR8
        JMP   <ADDR8
        JSR   <ADDR8

; 3b extended
        DEC   >ADDR16
        INC   >ADDR16
        ASL   >ADDR16
        ASR   >ADDR16
        LSL   >ADDR16
        LSR   >ADDR16
        CLR   >ADDR16
        NEG   >ADDR16
        COM   >ADDR16
        TST   >ADDR16
        ROL   >ADDR16
        ROR   >ADDR16
        JMP   >ADDR16
        JSR   >ADDR16


; ------------------------------------------------------------
; GROUP 4: register to register, TFR and EXG
; ------------------------------------------------------------
; 4a 16-bit pairs
        TFR   D,D
        TFR   D,X
        TFR   D,Y
        TFR   D,U
        TFR   D,S
        TFR   D,PC
        TFR   X,D
        TFR   X,X
        TFR   X,Y
        TFR   X,U
        TFR   X,S
        TFR   X,PC
        TFR   Y,D
        TFR   Y,X
        TFR   Y,Y
        TFR   Y,U
        TFR   Y,S
        TFR   Y,PC
        TFR   U,D
        TFR   U,X
        TFR   U,Y
        TFR   U,U
        TFR   U,S
        TFR   U,PC
        TFR   S,D
        TFR   S,X
        TFR   S,Y
        TFR   S,U
        TFR   S,S
        TFR   S,PC
        TFR   PC,D
        TFR   PC,X
        TFR   PC,Y
        TFR   PC,U
        TFR   PC,S
        TFR   PC,PC

; 4b 8-bit pairs
        TFR   A,A
        TFR   A,B
        TFR   A,CC
        TFR   A,DP
        TFR   B,A
        TFR   B,B
        TFR   B,CC
        TFR   B,DP
        TFR   CC,A
        TFR   CC,B
        TFR   CC,CC
        TFR   CC,DP
        TFR   DP,A
        TFR   DP,B
        TFR   DP,CC
        TFR   DP,DP

; 4c 16-bit pairs
        EXG   D,D
        EXG   D,X
        EXG   D,Y
        EXG   D,U
        EXG   D,S
        EXG   D,PC
        EXG   X,D
        EXG   X,X
        EXG   X,Y
        EXG   X,U
        EXG   X,S
        EXG   X,PC
        EXG   Y,D
        EXG   Y,X
        EXG   Y,Y
        EXG   Y,U
        EXG   Y,S
        EXG   Y,PC
        EXG   U,D
        EXG   U,X
        EXG   U,Y
        EXG   U,U
        EXG   U,S
        EXG   U,PC
        EXG   S,D
        EXG   S,X
        EXG   S,Y
        EXG   S,U
        EXG   S,S
        EXG   S,PC
        EXG   PC,D
        EXG   PC,X
        EXG   PC,Y
        EXG   PC,U
        EXG   PC,S
        EXG   PC,PC

; 4d 8-bit pairs
        EXG   A,A
        EXG   A,B
        EXG   A,CC
        EXG   A,DP
        EXG   B,A
        EXG   B,B
        EXG   B,CC
        EXG   B,DP
        EXG   CC,A
        EXG   CC,B
        EXG   CC,CC
        EXG   CC,DP
        EXG   DP,A
        EXG   DP,B
        EXG   DP,CC
        EXG   DP,DP


; ------------------------------------------------------------
; GROUP 5: PSHS PULS PSHU PULU, single registers and combinations
; ------------------------------------------------------------
; 5 PSHS
        PSHS  CC
        PSHS  A
        PSHS  B
        PSHS  D
        PSHS  DP
        PSHS  X
        PSHS  Y
        PSHS  U
        PSHS  PC
        PSHS  A,B
        PSHS  X,Y
        PSHS  A,B,X
        PSHS  CC,A,B,DP
        PSHS  X,Y,U
        PSHS  D,X,Y,U,PC
        PSHS  A,B,X,Y,U,DP,PC,CC

; 5 PULS
        PULS  CC
        PULS  A
        PULS  B
        PULS  D
        PULS  DP
        PULS  X
        PULS  Y
        PULS  U
        PULS  PC
        PULS  A,B
        PULS  X,Y
        PULS  A,B,X
        PULS  CC,A,B,DP
        PULS  X,Y,U
        PULS  D,X,Y,U,PC
        PULS  A,B,X,Y,U,DP,PC,CC

; 5 PSHU
        PSHU  CC
        PSHU  A
        PSHU  B
        PSHU  D
        PSHU  DP
        PSHU  X
        PSHU  Y
        PSHU  S
        PSHU  PC
        PSHU  A,B
        PSHU  X,Y
        PSHU  A,B,X
        PSHU  CC,A,B,DP
        PSHU  X,Y,S
        PSHU  D,X,Y,S,PC
        PSHU  A,B,X,Y,S,DP,PC,CC

; 5 PULU
        PULU  CC
        PULU  A
        PULU  B
        PULU  D
        PULU  DP
        PULU  X
        PULU  Y
        PULU  S
        PULU  PC
        PULU  A,B
        PULU  X,Y
        PULU  A,B,X
        PULU  CC,A,B,DP
        PULU  X,Y,S
        PULU  D,X,Y,S,PC
        PULU  A,B,X,Y,S,DP,PC,CC


; ------------------------------------------------------------
; GROUP 6: condition code masks, ANDCC ORCC CWAI
; ------------------------------------------------------------
; 6 ORCC single flags
        ORCC  #$01              ; CY-FLAG
        ORCC  #$02              ; OV-FLAG
        ORCC  #$04              ; Z-FLAG
        ORCC  #$08              ; N-FLAG
        ORCC  #$10              ; IRQ-FLAG
        ORCC  #$20              ; H-FLAG
        ORCC  #$40              ; FIRQ-FLAG
        ORCC  #$80              ; E-FLAG

; 6 ANDCC single flags
        ANDCC #$FE              ; CY-FLAG (complement)
        ANDCC #$FD              ; OV-FLAG (complement)
        ANDCC #$FB              ; Z-FLAG (complement)
        ANDCC #$F7              ; N-FLAG (complement)
        ANDCC #$EF              ; IRQ-FLAG (complement)
        ANDCC #$DF              ; H-FLAG (complement)
        ANDCC #$BF              ; FIRQ-FLAG (complement)
        ANDCC #$7F              ; E-FLAG (complement)

; 6 CWAI single flags
        CWAI  #$FE              ; CY-FLAG (complement)
        CWAI  #$FD              ; OV-FLAG (complement)
        CWAI  #$FB              ; Z-FLAG (complement)
        CWAI  #$F7              ; N-FLAG (complement)
        CWAI  #$EF              ; IRQ-FLAG (complement)
        CWAI  #$DF              ; H-FLAG (complement)
        CWAI  #$BF              ; FIRQ-FLAG (complement)
        CWAI  #$7F              ; E-FLAG (complement)

; 6 flag combinations
        ORCC  #$05              ; CY-FLAG Z-FLAG +
        ORCC  #$50              ; IRQ-FLAG FIRQ-FLAG +
        ANDCC #$AF              ; complement of IRQ+FIRQ
        ORCC  #$FF
        ANDCC #$00


; ------------------------------------------------------------
; GROUP 7: indexed addressing, every instruction, three forms each
; ------------------------------------------------------------
; 7a no offset ,X  (every instruction)
        LDA   ,X
        LDB   ,X
        ADDA  ,X
        ADDB  ,X
        SUBA  ,X
        SUBB  ,X
        CMPA  ,X
        CMPB  ,X
        ADCA  ,X
        ADCB  ,X
        SBCA  ,X
        SBCB  ,X
        ANDA  ,X
        ANDB  ,X
        ORA   ,X
        ORB   ,X
        EORA  ,X
        EORB  ,X
        BITA  ,X
        BITB  ,X
        STA   ,X
        STB   ,X
        LDD   ,X
        ADDD  ,X
        SUBD  ,X
        CMPD  ,X
        LDX   ,X
        LDY   ,X
        CMPX  ,X
        CMPY  ,X
        LDS   ,X
        LDU   ,X
        CMPS  ,X
        CMPU  ,X
        STD   ,X
        STX   ,X
        STY   ,X
        STS   ,X
        STU   ,X
        DEC   ,X
        INC   ,X
        ASL   ,X
        ASR   ,X
        LSL   ,X
        LSR   ,X
        CLR   ,X
        NEG   ,X
        COM   ,X
        TST   ,X
        ROL   ,X
        ROR   ,X
        JMP   ,X
        JSR   ,X

; 7b 8-bit offset OFFP8,Y
        LDA   OFFP8,Y
        LDB   OFFP8,Y
        ADDA  OFFP8,Y
        ADDB  OFFP8,Y
        SUBA  OFFP8,Y
        SUBB  OFFP8,Y
        CMPA  OFFP8,Y
        CMPB  OFFP8,Y
        ADCA  OFFP8,Y
        ADCB  OFFP8,Y
        SBCA  OFFP8,Y
        SBCB  OFFP8,Y
        ANDA  OFFP8,Y
        ANDB  OFFP8,Y
        ORA   OFFP8,Y
        ORB   OFFP8,Y
        EORA  OFFP8,Y
        EORB  OFFP8,Y
        BITA  OFFP8,Y
        BITB  OFFP8,Y
        STA   OFFP8,Y
        STB   OFFP8,Y
        LDD   OFFP8,Y
        ADDD  OFFP8,Y
        SUBD  OFFP8,Y
        CMPD  OFFP8,Y
        LDX   OFFP8,Y
        LDY   OFFP8,Y
        CMPX  OFFP8,Y
        CMPY  OFFP8,Y
        LDS   OFFP8,Y
        LDU   OFFP8,Y
        CMPS  OFFP8,Y
        CMPU  OFFP8,Y
        STD   OFFP8,Y
        STX   OFFP8,Y
        STY   OFFP8,Y
        STS   OFFP8,Y
        STU   OFFP8,Y
        DEC   OFFP8,Y
        INC   OFFP8,Y
        ASL   OFFP8,Y
        ASR   OFFP8,Y
        LSL   OFFP8,Y
        LSR   OFFP8,Y
        CLR   OFFP8,Y
        NEG   OFFP8,Y
        COM   OFFP8,Y
        TST   OFFP8,Y
        ROL   OFFP8,Y
        ROR   OFFP8,Y
        JMP   OFFP8,Y
        JSR   OFFP8,Y

; 7c 16-bit offset OFFN16,U
        LDA   OFFN16,U
        LDB   OFFN16,U
        ADDA  OFFN16,U
        ADDB  OFFN16,U
        SUBA  OFFN16,U
        SUBB  OFFN16,U
        CMPA  OFFN16,U
        CMPB  OFFN16,U
        ADCA  OFFN16,U
        ADCB  OFFN16,U
        SBCA  OFFN16,U
        SBCB  OFFN16,U
        ANDA  OFFN16,U
        ANDB  OFFN16,U
        ORA   OFFN16,U
        ORB   OFFN16,U
        EORA  OFFN16,U
        EORB  OFFN16,U
        BITA  OFFN16,U
        BITB  OFFN16,U
        STA   OFFN16,U
        STB   OFFN16,U
        LDD   OFFN16,U
        ADDD  OFFN16,U
        SUBD  OFFN16,U
        CMPD  OFFN16,U
        LDX   OFFN16,U
        LDY   OFFN16,U
        CMPX  OFFN16,U
        CMPY  OFFN16,U
        LDS   OFFN16,U
        LDU   OFFN16,U
        CMPS  OFFN16,U
        CMPU  OFFN16,U
        STD   OFFN16,U
        STX   OFFN16,U
        STY   OFFN16,U
        STS   OFFN16,U
        STU   OFFN16,U
        DEC   OFFN16,U
        INC   OFFN16,U
        ASL   OFFN16,U
        ASR   OFFN16,U
        LSL   OFFN16,U
        LSR   OFFN16,U
        CLR   OFFN16,U
        NEG   OFFN16,U
        COM   OFFN16,U
        TST   OFFN16,U
        ROL   OFFN16,U
        ROR   OFFN16,U
        JMP   OFFN16,U
        JSR   OFFN16,U


; ------------------------------------------------------------
; GROUP 8: indexed addressing, accumulator and auto increment forms (LDA)
; ------------------------------------------------------------
; 8 register X
        LDA   A,X
        LDA   B,X
        LDA   D,X
        LDA   ,X+
        LDA   ,X++
        LDA   ,-X
        LDA   ,--X
        LDA   ,X

; 8 register Y
        LDA   A,Y
        LDA   B,Y
        LDA   D,Y
        LDA   ,Y+
        LDA   ,Y++
        LDA   ,-Y
        LDA   ,--Y
        LDA   ,Y

; 8 register U
        LDA   A,U
        LDA   B,U
        LDA   D,U
        LDA   ,U+
        LDA   ,U++
        LDA   ,-U
        LDA   ,--U
        LDA   ,U

; 8 register S
        LDA   A,S
        LDA   B,S
        LDA   D,S
        LDA   ,S+
        LDA   ,S++
        LDA   ,-S
        LDA   ,--S
        LDA   ,S


; ------------------------------------------------------------
; GROUP 9: indexed addressing, constant offsets (LDA)
; ------------------------------------------------------------
; 9a 5-bit offsets, X
        LDA   0,X
        LDA   1,X
        LDA   -1,X
        LDA   OFFP5,X
        LDA   OFFN5,X
; 9b 8-bit offsets, X
        LDA   OFFP8,X
        LDA   OFFN8,X
; 9c 16-bit offsets, X
        LDA   OFFP16,X
        LDA   OFFN16,X

; 9a 5-bit offsets, Y
        LDA   0,Y
        LDA   1,Y
        LDA   -1,Y
        LDA   OFFP5,Y
        LDA   OFFN5,Y
; 9b 8-bit offsets, Y
        LDA   OFFP8,Y
        LDA   OFFN8,Y
; 9c 16-bit offsets, Y
        LDA   OFFP16,Y
        LDA   OFFN16,Y

; 9a 5-bit offsets, U
        LDA   0,U
        LDA   1,U
        LDA   -1,U
        LDA   OFFP5,U
        LDA   OFFN5,U
; 9b 8-bit offsets, U
        LDA   OFFP8,U
        LDA   OFFN8,U
; 9c 16-bit offsets, U
        LDA   OFFP16,U
        LDA   OFFN16,U

; 9a 5-bit offsets, S
        LDA   0,S
        LDA   1,S
        LDA   -1,S
        LDA   OFFP5,S
        LDA   OFFN5,S
; 9b 8-bit offsets, S
        LDA   OFFP8,S
        LDA   OFFN8,S
; 9c 16-bit offsets, S
        LDA   OFFP16,S
        LDA   OFFN16,S


; ------------------------------------------------------------
; GROUP 10: indexed addressing, PC relative (LDA).  * is the instruction address
; ------------------------------------------------------------
; 10a 8-bit forward and backward
        LDA   *+$20,PCR
        LDA   *-$20,PCR
        LDB   *+$40,PCR
        LDB   *-$40,PCR
; 10b 16-bit forward and backward
        LDA   *+$200,PCR
        LDA   *-$200,PCR
        LDD   *+$1000,PCR
        LDD   *-$1000,PCR
; 10c other opcodes
        LDY   *+$20,PCR
        STX   *-$20,PCR
        JSR   *+$20,PCR
        LEAX  *+$20,PCR
        LEAX  *+$200,PCR


; ------------------------------------------------------------
; GROUP 11: indirect addressing (LDA)
; ------------------------------------------------------------
; 11a indirect indexed, X
        LDA   [,X]
        LDA   [A,X]
        LDA   [B,X]
        LDA   [D,X]
        LDA   [,X++]
        LDA   [,--X]
        LDA   [0,X]
        LDA   [1,X]
        LDA   [OFFP8,X]
        LDA   [OFFN8,X]
        LDA   [OFFP16,X]
        LDA   [OFFN16,X]

; 11a indirect indexed, Y
        LDA   [,Y]
        LDA   [A,Y]
        LDA   [B,Y]
        LDA   [D,Y]
        LDA   [,Y++]
        LDA   [,--Y]
        LDA   [0,Y]
        LDA   [1,Y]
        LDA   [OFFP8,Y]
        LDA   [OFFN8,Y]
        LDA   [OFFP16,Y]
        LDA   [OFFN16,Y]

; 11a indirect indexed, U
        LDA   [,U]
        LDA   [A,U]
        LDA   [B,U]
        LDA   [D,U]
        LDA   [,U++]
        LDA   [,--U]
        LDA   [0,U]
        LDA   [1,U]
        LDA   [OFFP8,U]
        LDA   [OFFN8,U]
        LDA   [OFFP16,U]
        LDA   [OFFN16,U]

; 11a indirect indexed, S
        LDA   [,S]
        LDA   [A,S]
        LDA   [B,S]
        LDA   [D,S]
        LDA   [,S++]
        LDA   [,--S]
        LDA   [0,S]
        LDA   [1,S]
        LDA   [OFFP8,S]
        LDA   [OFFN8,S]
        LDA   [OFFP16,S]
        LDA   [OFFN16,S]

; 11b indirect PC relative
        LDA   [*+$20,PCR]
        LDA   [*-$20,PCR]
        LDA   [*+$200,PCR]
        LDA   [*-$200,PCR]

; 11c extended indirect
        LDA   [ADDR16]
        LDB   [ADDR16]
        STA   [ADDR16]
        STB   [ADDR16]
        ADDA  [ADDR16]
        JMP   [ADDR16]
        JSR   [ADDR16]
        CLR   [ADDR16]
        INC   [ADDR16]
        LDD   [ADDR16]
        STD   [ADDR16]
        LDX   [ADDR16]
        STX   [ADDR16]
        LDY   [ADDR16]
        STY   [ADDR16]
        LDS   [ADDR16]
        STS   [ADDR16]
        LDU   [ADDR16]
        STU   [ADDR16]
        CMPD  [ADDR16]
        CMPY  [ADDR16]
        CMPS  [ADDR16]
        CMPU  [ADDR16]

; 11d indirect with other opcodes
        STY   [OFFP8,X]
        LDS   [OFFP8,X]
        CMPU  [OFFP8,X]
        CMPD  [OFFP8,X]
        JMP   [OFFP8,X]
        JSR   [OFFP8,X]
        TST   [OFFP8,X]
        CLR   [OFFP8,X]


; ------------------------------------------------------------
; GROUP 12: LEAX LEAY LEAS LEAU
; ------------------------------------------------------------
; 12 LEAX
        LEAX  ,X
        LEAX  1,X
        LEAX  OFFP5,X
        LEAX  OFFN8,X
        LEAX  OFFP16,X
        LEAX  A,X
        LEAX  B,X
        LEAX  D,X
        LEAX  ,X+
        LEAX  ,X++
        LEAX  ,-X
        LEAX  ,--X
        LEAX  [,X]
        LEAX  [OFFP8,X]
        LEAX  [,X++]
        LEAX  ,Y
        LEAX  1,Y
        LEAX  OFFP5,Y
        LEAX  OFFN8,Y
        LEAX  OFFP16,Y
        LEAX  A,Y
        LEAX  B,Y
        LEAX  D,Y
        LEAX  ,Y+
        LEAX  ,Y++
        LEAX  ,-Y
        LEAX  ,--Y
        LEAX  [,Y]
        LEAX  [OFFP8,Y]
        LEAX  [,Y++]
        LEAX  ,U
        LEAX  1,U
        LEAX  OFFP5,U
        LEAX  OFFN8,U
        LEAX  OFFP16,U
        LEAX  A,U
        LEAX  B,U
        LEAX  D,U
        LEAX  ,U+
        LEAX  ,U++
        LEAX  ,-U
        LEAX  ,--U
        LEAX  [,U]
        LEAX  [OFFP8,U]
        LEAX  [,U++]
        LEAX  ,S
        LEAX  1,S
        LEAX  OFFP5,S
        LEAX  OFFN8,S
        LEAX  OFFP16,S
        LEAX  A,S
        LEAX  B,S
        LEAX  D,S
        LEAX  ,S+
        LEAX  ,S++
        LEAX  ,-S
        LEAX  ,--S
        LEAX  [,S]
        LEAX  [OFFP8,S]
        LEAX  [,S++]

; 12 LEAY
        LEAY  ,X
        LEAY  1,X
        LEAY  OFFP5,X
        LEAY  OFFN8,X
        LEAY  OFFP16,X
        LEAY  A,X
        LEAY  B,X
        LEAY  D,X
        LEAY  ,X+
        LEAY  ,X++
        LEAY  ,-X
        LEAY  ,--X
        LEAY  [,X]
        LEAY  [OFFP8,X]
        LEAY  [,X++]
        LEAY  ,Y
        LEAY  1,Y
        LEAY  OFFP5,Y
        LEAY  OFFN8,Y
        LEAY  OFFP16,Y
        LEAY  A,Y
        LEAY  B,Y
        LEAY  D,Y
        LEAY  ,Y+
        LEAY  ,Y++
        LEAY  ,-Y
        LEAY  ,--Y
        LEAY  [,Y]
        LEAY  [OFFP8,Y]
        LEAY  [,Y++]
        LEAY  ,U
        LEAY  1,U
        LEAY  OFFP5,U
        LEAY  OFFN8,U
        LEAY  OFFP16,U
        LEAY  A,U
        LEAY  B,U
        LEAY  D,U
        LEAY  ,U+
        LEAY  ,U++
        LEAY  ,-U
        LEAY  ,--U
        LEAY  [,U]
        LEAY  [OFFP8,U]
        LEAY  [,U++]
        LEAY  ,S
        LEAY  1,S
        LEAY  OFFP5,S
        LEAY  OFFN8,S
        LEAY  OFFP16,S
        LEAY  A,S
        LEAY  B,S
        LEAY  D,S
        LEAY  ,S+
        LEAY  ,S++
        LEAY  ,-S
        LEAY  ,--S
        LEAY  [,S]
        LEAY  [OFFP8,S]
        LEAY  [,S++]

; 12 LEAS
        LEAS  ,X
        LEAS  1,X
        LEAS  OFFP5,X
        LEAS  OFFN8,X
        LEAS  OFFP16,X
        LEAS  A,X
        LEAS  B,X
        LEAS  D,X
        LEAS  ,X+
        LEAS  ,X++
        LEAS  ,-X
        LEAS  ,--X
        LEAS  [,X]
        LEAS  [OFFP8,X]
        LEAS  [,X++]
        LEAS  ,Y
        LEAS  1,Y
        LEAS  OFFP5,Y
        LEAS  OFFN8,Y
        LEAS  OFFP16,Y
        LEAS  A,Y
        LEAS  B,Y
        LEAS  D,Y
        LEAS  ,Y+
        LEAS  ,Y++
        LEAS  ,-Y
        LEAS  ,--Y
        LEAS  [,Y]
        LEAS  [OFFP8,Y]
        LEAS  [,Y++]
        LEAS  ,U
        LEAS  1,U
        LEAS  OFFP5,U
        LEAS  OFFN8,U
        LEAS  OFFP16,U
        LEAS  A,U
        LEAS  B,U
        LEAS  D,U
        LEAS  ,U+
        LEAS  ,U++
        LEAS  ,-U
        LEAS  ,--U
        LEAS  [,U]
        LEAS  [OFFP8,U]
        LEAS  [,U++]
        LEAS  ,S
        LEAS  1,S
        LEAS  OFFP5,S
        LEAS  OFFN8,S
        LEAS  OFFP16,S
        LEAS  A,S
        LEAS  B,S
        LEAS  D,S
        LEAS  ,S+
        LEAS  ,S++
        LEAS  ,-S
        LEAS  ,--S
        LEAS  [,S]
        LEAS  [OFFP8,S]
        LEAS  [,S++]

; 12 LEAU
        LEAU  ,X
        LEAU  1,X
        LEAU  OFFP5,X
        LEAU  OFFN8,X
        LEAU  OFFP16,X
        LEAU  A,X
        LEAU  B,X
        LEAU  D,X
        LEAU  ,X+
        LEAU  ,X++
        LEAU  ,-X
        LEAU  ,--X
        LEAU  [,X]
        LEAU  [OFFP8,X]
        LEAU  [,X++]
        LEAU  ,Y
        LEAU  1,Y
        LEAU  OFFP5,Y
        LEAU  OFFN8,Y
        LEAU  OFFP16,Y
        LEAU  A,Y
        LEAU  B,Y
        LEAU  D,Y
        LEAU  ,Y+
        LEAU  ,Y++
        LEAU  ,-Y
        LEAU  ,--Y
        LEAU  [,Y]
        LEAU  [OFFP8,Y]
        LEAU  [,Y++]
        LEAU  ,U
        LEAU  1,U
        LEAU  OFFP5,U
        LEAU  OFFN8,U
        LEAU  OFFP16,U
        LEAU  A,U
        LEAU  B,U
        LEAU  D,U
        LEAU  ,U+
        LEAU  ,U++
        LEAU  ,-U
        LEAU  ,--U
        LEAU  [,U]
        LEAU  [OFFP8,U]
        LEAU  [,U++]
        LEAU  ,S
        LEAU  1,S
        LEAU  OFFP5,S
        LEAU  OFFN8,S
        LEAU  OFFP16,S
        LEAU  A,S
        LEAU  B,S
        LEAU  D,S
        LEAU  ,S+
        LEAU  ,S++
        LEAU  ,-S
        LEAU  ,--S
        LEAU  [,S]
        LEAU  [OFFP8,S]
        LEAU  [,S++]


; ------------------------------------------------------------
; GROUP 13: conditional branches, short.  Each skips one NOP (*+3).
; ------------------------------------------------------------
; The ClaudeForth condition constant that yields each opcode is shown.
        BRA   *+3               ; $20  NVR
        NOP
        BRN   *+3               ; $21  ALW
        NOP
        BHI   *+3               ; $22  LS
        NOP
        BLS   *+3               ; $23  HI
        NOP
        BCC   *+3               ; $24  CS
        NOP
        BCS   *+3               ; $25  CC
        NOP
        BNE   *+3               ; $26  EQ
        NOP
        BEQ   *+3               ; $27  NE
        NOP
        BVC   *+3               ; $28  VS
        NOP
        BVS   *+3               ; $29  VC
        NOP
        BPL   *+3               ; $2A  MI
        NOP
        BMI   *+3               ; $2B  PL
        NOP
        BGE   *+3               ; $2C  LT
        NOP
        BLT   *+3               ; $2D  GE
        NOP
        BGT   *+3               ; $2E  LE
        NOP
        BLE   *+3               ; $2F  GT
        NOP

; 13b backward branches (loop of one NOP)
BKNVR
        NOP
        BRA   BKNVR             ; $20  NVR
BKALW
        NOP
        BRN   BKALW             ; $21  ALW
BKLS
        NOP
        BHI   BKLS              ; $22  LS
BKHI
        NOP
        BLS   BKHI              ; $23  HI
BKCS
        NOP
        BCC   BKCS              ; $24  CS
BKCC
        NOP
        BCS   BKCC              ; $25  CC
BKEQ
        NOP
        BNE   BKEQ              ; $26  EQ
BKNE
        NOP
        BEQ   BKNE              ; $27  NE
BKVS
        NOP
        BVC   BKVS              ; $28  VS
BKVC
        NOP
        BVS   BKVC              ; $29  VC
BKMI
        NOP
        BPL   BKMI              ; $2A  MI
BKPL
        NOP
        BMI   BKPL              ; $2B  PL
BKLT
        NOP
        BGE   BKLT              ; $2C  LT
BKGE
        NOP
        BLT   BKGE              ; $2D  GE
BKLE
        NOP
        BGT   BKLE              ; $2E  LE
BKGT
        NOP
        BLE   BKGT              ; $2F  GT


; ------------------------------------------------------------
; GROUP 14: structured constructs, as assembled by IF, ELSE, THEN, BEGIN, UNTIL, WHILE, REPEAT
; ------------------------------------------------------------
; 14a  EQ IF, NOP, THEN,
        BNE   *+3
        NOP

; 14b  EQ IF, NOP, ELSE, NOP, THEN,
        BNE   L14B1
        NOP
        BRA   L14B2
L14B1
        NOP
L14B2

; 14c  BEGIN, NOP, EQ UNTIL,
L14C
        NOP
        BNE   L14C

; 14d  BEGIN, NOP, EQ WHILE, NOP, REPEAT,
L14D1
        NOP
        BNE   L14D2
        NOP
        BRA   L14D1
L14D2

; 14e  nested: CS IF, EQ IF, NOP, THEN, NOP, THEN,
        BCC   L14E1
        BNE   L14E2
        NOP
L14E2
        NOP
L14E1

; 14f  nested loop: BEGIN, BEGIN, NOP, EQ UNTIL, NOP, MI UNTIL,
L14F1
L14F2
        NOP
        BNE   L14F2
        NOP
        BPL   L14F1

; 14g  loop containing IF,: BEGIN, EQ IF, NOP, THEN, CC UNTIL,
L14G
        BNE   L14G1
        NOP
L14G1
        BCS   L14G

; 14h  body of 20 NOPs
        BNE   L14H
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
L14H


; ------------------------------------------------------------
; GROUP 15: boundary cases.  lwasm sizes these as the hardware allows;
; ------------------------------------------------------------
; the Forth port may choose a larger encoding at some limits.
; 15a indexed offset limits (LDA ,X)
        LDA   15,X
        LDA   16,X
        LDA   -16,X
        LDA   -17,X
        LDA   126,X
        LDA   127,X
        LDA   128,X
        LDA   -128,X
        LDA   -129,X
        LDA   32767,X
        LDA   -32768,X

; 15b indirect offset limits
        LDA   [0,X]
        LDA   [15,X]
        LDA   [-16,X]
        LDA   [127,X]
        LDA   [-128,X]
        LDA   [128,X]
        LDA   [-129,X]

; 15c PC relative limits.  8-bit offset is measured from the end of
;     the 3 byte instruction, so *+130 is +127 and *-125 is -128.
        LDA   *+129,PCR
        LDA   *+130,PCR
        LDA   *+131,PCR
        LDA   *-124,PCR
        LDA   *-125,PCR
        LDA   *-126,PCR
        LDA   *-127,PCR

; 15d branch distance limits, forward (NOP count: 126, 127)
        BNE   LF126
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
LF126
        BNE   LF127
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
LF127

; 15e backward branch limit (128 NOPs then BNE: offset -128)
LBK128
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        NOP
        BNE   LBK128

; 15f long forms never produced by the Forth port, for reference
        LBRA  *+$200
        LBSR  *+$200
        LBNE  *+$200
        BSR   *+3
        NOP

