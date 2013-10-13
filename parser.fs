s" ./datatypes.fs" included

: is-whitespace? ( char -- b )
    10 over = over 32 = or swap 9 = or ;

: is-opening-bracket? ( char -- b )
    40 = ;

: is-closing-bracket? ( char -- b )
    41 = ;

: is-bracket? ( char -- b )
    dup is-opening-bracket? swap is-closing-bracket? or ;

: sc-parse-word ( str str-length -- addr str-length )
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
    
