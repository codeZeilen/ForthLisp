: string-symbol 0 ;
: number-symbol 1 ;
: list-symbol 2 ;

: make-string ( addr u -- addr )
    here 3 cells allot
    dup string-symbol swap !
    swap over 1 cells + ! 
    swap over 2 cells + ! ;

: >is-string? ( a -- b ) @ string-symbol = ;
: >string-length ( a -- a ) 1 cells + @ ;
: >string-content ( a -- n ) 2 cells + @ ;

: make-number ( u -- addr )
    here 2 cells allot
    dup number-symbol swap !
    swap over 1 cells + ! ;
: >is-number? ( a -- b ) @ number-symbol = ;
: >number-value ( a -- n ) 1 cells + @ ;

: make-empty-node ( -- addr )
    here 3 cells allot
    dup list-symbol swap ! ;
: >is-list? ( a -- b ) @ list-symbol = ;
: >node-content ( a -- a ) 1 cells + @ ;
: >node-content! ( a list-a  -- ) 1 cells + ! ;
: >node-next-node ( a -- a ) 2 cells + @ ;
: >node-next-node! ( a list-a -- ) 2 cells + ! ;

: make-new-list ( -- addr )
    make-empty-node
    0 over >node-next-node! ;

: _list-node-at ( n a-list -- a )
    over 1 + 0 do
        over i = if
            swap drop
            unloop 
            exit
        else
            >node-next-node dup
            0= if
                s" Debug: " type .s cr
                drop drop
                s" List index out of range" exception throw
            endif
        endif
    loop ;

: >list-last-node ( a-list -- a )
    BEGIN
        dup >node-next-node 0<>
    WHILE
        >node-next-node 
    REPEAT ;

: >list-at ( n a-list -- a )
    _list-node-at >node-content ;
: >list-at! ( a-item n a-list )
    _list-node-at >node-content! ;
: >list-append ( an-item a-list )
    >list-last-node 
    make-new-list 
    swap over swap
    >node-next-node! 
    >node-content! ;

: >list-length ( a-list -- u )
    1
    BEGIN
        over >node-next-node 0<>
    WHILE
        1 + 
        swap >node-next-node swap
    REPEAT swap drop ;

: is-whitespace? ( char -- b )
    10 over = over 32 = or over 9 = or ;

: sc-parse ( str str-length -- addr )
;

: word-parse ( str str-length -- addr )
;

: simple-parse-word ( str str-length -- substr substr-length )
    0 swap 0 do
        drop
        dup i chars +               \ Current position
        C@ is-whitespace? if                  \ Another space
            i leave 
       endif
       i 1 +
    loop 
    here >r 
    swap over
    dup chars allot                 \ Make room for the word 
    r@ swap cmove                   \ Copy string
    r> swap ;                       \ Reorder return values

: simple-word-parse ( str str-length -- addr )
    >r                              \ Store str-length for future reference
    make-new-list
    r@ 0 do
        over i chars +              \ Get current character
        dup C@ is-whitespace? invert if             \ We've got no space here
                                    \ Beginning of str is on stack
                r> r@ swap >r       \ Get str-length
                i -                 \ Adjust string length to substring

                simple-parse-word
                dup r> + 1 - >r     \ Jump index forth to end of parsed word
                make-string
                over >list-append   \ Append word to list
        else
            drop
        endif
    loop 
    r> drop ; \ Remove str-length from rstack

\ TESTS
: assert ( res -- )
    0= if .s cr s" assertion error" exception throw endif ;

s" hello you!" make-string 
dup >string-content
swap >string-length = 10 assert

create a-number
20 make-number a-number !
a-number @ >number-value
a-number @ >number-value 20 = assert

create a-list
make-new-list a-list !
a-number @ a-list @ >list-append
a-number @ >number-value 20 = assert

a-list @ >node-next-node 
    0<> assert

a-list @ >node-next-node >node-content 
a-number @ 
    = assert

a-list @ >node-next-node >node-content >number-value 
a-number @ >number-value 
    = assert

1 a-list @ >list-at 
a-list @ >node-next-node >node-content
    = assert

30 make-number a-list @ >list-append 
2 a-list @ >list-at >number-value 30 = assert

9 is-whitespace? assert
10 is-whitespace? assert
32 is-whitespace? assert

\ s" (+ 20 (+ 3 4) (foobar 2 3 4))" sc-parse .s
\ s" hallo mein haus" sc-parse .s
s" hallo mein haus" simple-parse-word
s" hallo" simple-parse-word
s"  hallo   mein  haus" simple-word-parse
