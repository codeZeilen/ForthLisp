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
    10 over = over 32 = or swap 9 = or ;

: is-opening-bracket? ( char -- b )
    40 = ;

: is-closing-bracket? ( char -- b )
    41 = ;

: is-bracket? ( char -- b )
    dup is-opening-bracket? swap is-closing-bracket? or ;

: simple-parse-word ( str str-length -- substr substr-length )
    0 swap 0 do
        drop
        dup i chars +               \ Current position
        dup C@ is-whitespace? swap C@ is-bracket? or if        \ Another space
            i leave 
       endif
       i 1 +
    loop 
    here >r 
    swap over
    dup chars allot                 \ Make room for the word 
    r@ swap cmove                   \ Copy string
    r> swap ( Debug ) 2dup type cr ;                       \ Reorder return values

: simple-word-parse ( str str-length -- addr )
    >r                              \ Store str-length for future reference
    make-new-list
    r@ 0 do
        over i chars +              \ Get current position
        dup C@ is-whitespace? invert if                      \ We've got no bracket here
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

: sc-parse-word ( str str-length -- addr str-length )
    simple-parse-word ;

: _sc-parse ( str str-length -- addr parsed-str-length ) 
    s" New Parse" type cr
    >r                              \ Store str-length for future reference
    make-new-list
    0                               \ Dummy i to be dropped at start of loop
    r@ 0 do
        drop
        s" New Loop: " type i . cr
        over i chars +              \ Get current position
        dup C@ is-whitespace? if
            s" Whitespace" type cr
            drop
        else dup C@ is-opening-bracket? if
            s" Opening Bracket" type cr
            
            1 chars +               \ Move pointer forward
            r> r@ swap >r           \ Get str-length
            i -                     \ Adjust string length to substring
            
            recurse 
            dup r> + 1 - >r         \ Jump index forth to end of parsed word
            drop
            s" Lenght of new list: " type dup >list-length . cr
            s" Lenght of old list: " type over >list-length . cr
            over >list-append
        
        else dup C@ is-closing-bracket? if
            s" Closing Bracket" type cr

            drop                    \ Current index isn't relevant anymore
            swap drop               \ String isn't relevant anymore
            s" Lenght of new list: " type dup >list-length . cr
            i
            r> r> swap drop >r      \ Remove overall str-length from rstack
            unloop
            exit
            
        else                        \ Case: Normal Word
            s" Normal Word" type cr
                                    \ Beginning of str is on stack
            r> r@ swap >r          \ Get str-length
            i -                    \ Adjust string length to substring

            sc-parse-word 
            dup r> + 1 - >r        \ Jump index forth to end of parsed word
            make-string
            over >list-append      \ Append word to list
            
        endif
        endif
        endif
        i
    loop
    r> drop ;                       \ Remove str-length from rstack

: sc-parse ( str str-length -- addr )
    _sc-parse drop ; \ TODO check whether the string was completely parsed
    

\ TESTS
: assert ( res -- )
    0= if .s cr s" assertion error" exception throw endif ;

s" hello you!" make-string 
>string-length 10 = assert

create a-number
20 make-number a-number !
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

a-list @ >list-length
2
    = assert

a-list @ >node-next-node >node-content >number-value 
a-number @ >number-value 
    = assert

1 a-list @ >list-at 
a-list @ >node-next-node >node-content
    = assert

30 make-number a-list @ >list-append 
2 a-list @ >list-at >number-value 30 = assert
a-list @ >list-length 3 = assert
30 make-number a-list @ >list-append 
a-list @ >list-length 4 = assert

9 is-whitespace? assert
10 is-whitespace? assert
32 is-whitespace? assert

40 is-opening-bracket? assert
41 is-closing-bracket? assert

40 is-bracket? assert
41 is-bracket? assert

\ s" hallo mein haus" simple-parse-word . .
\ s" hallo" simple-parse-word . .
\ s"  hallo   mein  haus" simple-word-parse . .

s" (+ 20 (+ 3 4) (foobar 2 3 4))" sc-parse 
\ s" hallo mein haus" sc-parse .s
