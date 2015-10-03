\ Stingstack                                           uh 2015-10-02

here

: have ( <spaces>ccc<spaces> -- flag )
    BL WORD FIND NIP ;

have under 0= [IF] : under ( n0 n1 -- n1 n0 n1 ) SWAP OVER ;  [THEN]
have CELL- 0= [IF] : cell- ( addr1 -- addr2 ) 1 CELLS - ;  [THEN]

: lplace ( a-addr len a-addr' -- )  
   \ Put the string given by C-ADDR LEN as long counted string at 
   \ address C-ADDR'
   ( 2DUP >R >R  CELL+  SWAP CHARS MOVE  R> R> ! ; )
   OVER >R  ROT OVER CELL+ R> CHARS MOVE  ! ;

: length ( a-addr1 -- a-addr2 u )  DUP CELL+ SWAP @ ;

: (" \ ( ccc<)> -- )    \ for string stack comments
    POSTPONE ( ; IMMEDIATE
    
VARIABLE "stack

: "ptr ( -- addr ) "stack @ ;
: "top ( -- addr ) "ptr @ ;

: "allot ( len -- )
      HERE SWAP ALLOT  HERE "stack !  HERE ,  , ;
      
BASE @ DECIMAL   1000 CHARS "allot   BASE !

: "clear ( -- )  "ptr DUP ! ;

: "skip ( addr0 -- addr1 )   length CHARS + ;

: ?overflow ( addr -- addr )
    DUP "ptr CELL+ @  U<  ABORT" overflow" ;
    
: "push  ( addr len -- )  (" -- s )
    "top OVER - CELL- ?overflow  DUP "ptr !  lplace ;
    
: ?empty ( addr -- addr )  DUP "ptr = ABORT" empty" ;
     
: "th ( +n -- addr ) (" s -- s )
   "top  BEGIN  ?empty SWAP ?DUP WHILE 1- SWAP "skip REPEAT ;
   
: "count ( -- addr len ) (" s -- s ) 0 "th length ;

: "length ( -- len ) 0 "th @ ;

: "@ ( addr -- ) (" -- s )  length "push ;

: "drop ( --  ) (" s -- )   "count + "ptr ! ;

: "pop ( -- addr len ) (" s -- )  "count "drop ;

: "pick ( +n -- )  (" sn ... s0 -- sn ... s0 sn ) "th "@ ;

: "roll ( +n -- )  (" sn ... s0 -- sn-1 ... s0 sn )  
    "th  DUP "@  "top under -   "pop + SWAP CMOVE> ;
    
: "dup ( -- ) (" s -- s s )  0 "pick ;
: "over ( -- ) (" s0 s1 -- s0 s1 s0 )  1 "pick ;

: "-roll ( +n -- )  (" sn ... s0 -- s0 sn ... s1 )
     "th  "skip DUP   "count + under -   "top SWAP
     "dup CMOVE  "pop  ROT OVER - CELL- lplace ;

: "swap ( -- ) (" s0 s1 -- s1 s0 )  1 "roll ;
: "rot  ( -- ) (" s0 s1 s2 -- s1 s2 s0 )  2 "roll ;

: "depth ( -- +n ) (" sn-1 ... s0 -- sn-1 ... s0 )
    0  "top BEGIN DUP "ptr - WHILE  "skip SWAP 1+ SWAP REPEAT DROP ;
    
: "join ( -- ) (" s0 s1 -- s2 )
    "top  DUP >R  "pop  dup "length + R> !
    OVER "ptr !  CELL+ CMOVE> ;

: "joins ( n -- ) (" s0 s1 .. sn -- s )
    0 ?DO  "join  LOOP ;

: "extract ( +n0 +n1 -- )  (" s0 -- s1 )
    "pop  ROT UMIN  ROT OVER UMIN
    ROT OVER +  -ROT  -  "push ;
    
: "split ( +n -- ) (" s0 -- s1 s2 )
    "dup   0 SWAP  DUP "length  "extract  "swap "extract ;
    
: "" ( -- ) (" -- s )  0 0 "push ;

: c"push ( char -- )  (" -- s)
    HERE 1 "push  "count DROP C! ;
    
: " ( ccc<"> -- ) (" -- s )
    STATE @ IF POSTPONE S"  POSTPONE "push  EXIT THEN
    [CHAR] " WORD COUNT "push ; IMMEDIATE

: "expect ( +n -- )  (" -- s )
    HERE OVER "push   "count ACCEPT  "pop DROP SWAP "push ;

: ".  (" s -- )  "pop TYPE ;

: ".s  ( -- ) (" -- )
      "top BEGIN  "depth WHILE CR ". REPEAT  "ptr ! ;

: "Constant (" s -- )
    CREATE  "pop HERE OVER CHARS CELL+ ALLOT  lplace
    DOES> ( -- addr )  "@ ;
    
: "Variable ( +n --  )
    CREATE  DUP ,   0 ,  ALLOT
    DOES>  ( -- addr )  CELL+ ;
    
: ?fits ( addr -- addr )
    DUP CELL- @ "length U< ABORT" too long" ;
    
: "! ( addr -- )  (" s -- )
     ?fits  "pop ROT lplace ;
     
: "compare ( -- n ) (" s0 s1 -- )
     "pop "pop compare negate ;
     
: "= ( -- f )  (" s0 s1 -- )  "compare 0= ;
: "< ( -- f )  (" s0 s1 -- )  "compare 0< ;
: "<= ( -- f ) (" s0 s1 -- )  "compare  DUP 0<  SWAP 0= OR ;

: "append ( c -- ) (" s0 -- s1 ) c"push "swap "join ;

\ high-level functions

\ substring-search

: n"search (" s1 s2 -- s1 s2 )  ( n0 -- n1 tf )
     1 "th length rot /string   0 "th length  SEARCH  ROT DROP  DUP >R IF 1 "th @ SWAP - THEN R> ;
     
: "search (" s1 s2 -- s1 )  ( -- n1 tf )  0 n"search  "drop ;

\ split at delimiter

: "positions (" s1 s2 -- s1 ) ( -- n1 n2 ... nn n )
      0 0
      BEGIN (" s1 s2 ) ( n1 ... nn n )
         SWAP n"search 
      WHILE (" s1 s2 ) ( n1 ... n nn )
         SWAP OVER 1+ SWAP 1+
      REPEAT
      DROP
      "drop ;

: "delimiter-split (" s0 delim -- s1 ... sn ) ( -- n )
    "length >R
    "positions 
    R> OVER >R SWAP
    BEGIN ( n1 n2 ... ni delim-len i )
       DUP
    WHILE ( n1 n2 ... ni delim-len i )
      ROT "split 
      OVER "swap "split "drop "swap
      1-
    REPEAT ( delim-len i )
    2DROP R> 1+ ;

\ join with delimiter

: "delimiter-join (" s1 s2 ... sn delim -- s ) ( n -- )
    BEGIN  
       DUP 1-
    WHILE
       "swap "over "swap "join 2 "roll "swap "join "swap
       1-
    REPEAT
    "drop
    DROP ;

: "substitute (" s1 s2 s3 -- s1' )  ( -- )
     2 "roll 2 "roll  
     "length  "search  IF  ( l p )
        "split "swap  "split  "drop
        2 "roll 2 "roll "join "join
     EXIT THEN
     "swap "drop
     DROP ;

\ --------- testing -------------
marker *test*

: empty-stack ( i*x -- )
   BEGIN depth ?dup WHILE
      0< IF 0 ELSE drop THEN   
   REPEAT ;

: error ( c-addr u -- ) cr type  source type cr empty-stack ;

Variable context-depth
Variable actual-depth

base @ hex 
  20 Constant #stack
base !

Create actual-results #stack cells allot
: >actual-results ( i -- addr  )  cells actual-results + ;
: %depth ( -- u )  depth context-depth @ - ;

: {  ( -- ) 
   depth context-depth ! ;

: -> ( i*x -- ) \ record depth and stack content
   %depth dup actual-depth !
   dup #stack > Abort" Test-Stack size exceeded - increase #stack"
   ?dup 0= IF exit THEN 
   0 DO  I >actual-results ! LOOP ;

: }	( i*x -- ) \ compare stack (expected) contents with saved actual content
   %depth actual-depth @ = 
   IF %depth ?dup 
      IF  0 DO  I >actual-results @ - 
                IF S" *** Incorrect result: " error LEAVE THEN
            LOOP
      THEN
   ELSE
      S" *** Wrong number of results: " error 
   THEN ;

{ 3 4 + -> 7 }

{ " a" "depth "drop -> 1 }
{ " a" "dup "depth "clear -> 2 }
{ " a" "dup "= -> -1 }
{ " a" " b" "join " ba" "= -> -1 }
{ : txt " str" ;  " str" txt "=  -> -1 }
{ " abcdef" 1 3 "extract " bc" "= -> -1 }
{ CHAR z " a" "append " az" "= -> -1 }
{ " abc" " aaa" "< -> 0 }
{ " abc" " abc" "<= -> -1 }
{ " first" " second" "join " secondfirst" "=  ->  -1 }
{ " third" " second" " first" " , "  3 "delimiter-join  " first, second, third" "=  ->  -1 }
{ " ab,cd" " ," "search "drop -> 2 -1 }
{ " ab,cd" " x" "search "drop -> 5 0 }
{ " ab,cd,ef,gh" " ," "positions "drop -> 2 5 8 3 }
{ " ab,cd,ef,gh" " ," "delimiter-split   " -" "delimiter-join  " ab-cd-ef-gh" "=  ->  -1 }
{ " aaa$bbb" " $" " xxx" "substitute  " aaaxxxbbb" "=  ->  -1 }

*test*
\ --------- done testing -------------

cr .( Stringstack successfully loaded. )  here swap - . .( Bytes)