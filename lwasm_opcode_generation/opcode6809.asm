; ============================================================
; 6809 opcodes, with every addressing mode exercised.
; ============================================================

IMMVALUE    EQU   $A5
ADDR16      EQU   $A5A5
ADDR8       EQU   $7B
OFFSET5     EQU   $1A
OFFSETP8    EQU   $5E
OFFSETN8    EQU   $8C
BRNCHP      EQU   $0A
BRNCHN      EQU   $-09
            

            ORG   $0000             ; GLOBALS page (DP = $00)
; Inherent Opcodes
            NOP   
            ABX   
            DAA   
            MUL   
            RTI   
            RTS
            SEX  
            SYNC  
            
            ASLA  
            ASLB  
            ASRA  
            ASRB  
            LSLA  
            LSLB  
            LSRA  
            LSRB  
            ROLA  
            ROLB  
            RORA  
            RORB  
            
            CLRA  
            CLRB  
            COMA  
            COMB  
            DECA  
            DECB
            INCA  
            INCB  
            NEGA  
            NEGB  
            TSTA  
            TSTB
; Immediate required and assumed opcodes.
            PSHS  A,B,X,Y,U,DP,PC,CC
            PULS  A,B,X,Y,U,DP,PC,CC
            PSHU  A,B,X,Y,S,DP,PC,CC
            PULU  A,B,X,Y,S,DP,PC,CC
            CWAI  #IMMVALUE 
            ANDCC #IMMVALUE
            ORCC  #IMMVALUE

