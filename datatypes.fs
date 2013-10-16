: string-symbol 1 ;
: number-symbol 2 ;
: list-symbol 3 ;
: node-symbol 4 ;

: _char-numeric? ( char -- )
    47 over < swap 58 < and ;
: _unsigned-power ( u u - u ) ( power base - power )
    over 0= if
        drop drop
        1 
        exit
    endif
    over 1 = if
        swap drop
        exit
    endif
    dup rot 1 - 0 do
        over *
    loop 
    swap drop ;

: make-string ( addr u -- addr )
    3 cells allocate throw
    dup string-symbol swap !
    swap over 1 cells + ! 
    swap over 2 cells + ! ;
: >is-string? ( a -- b ) @ string-symbol = ;
: >string-length ( a -- a ) 1 cells + @ ;
: >string-content ( a -- n ) 2 cells + @ ;
: >string-type ( a -- ) 
    34 emit 
    dup >string-content swap >string-length type 
    34 emit ;
: >string-at ( n a -- c )
    over over >string-length > if
        s" Index out of range -- >string-at" exception throw
        \ exception
    endif
    >string-content swap chars + C@ ;
: >string-numeric? ( a -- b )
    dup >string-length 0 do
        i over >string-at _char-numeric? invert if
            drop 
            0 
            unloop
            exit
        endif
    loop 
    drop 
    -1 ;

: make-number ( u -- addr )
    2 cells allocate throw
    dup number-symbol swap !
    swap over 1 cells + ! ;
: >is-number? ( a -- b ) @ number-symbol = ;
: >number-value ( a -- n ) 1 cells + @ ;
\ TODO: take care of hex vs. dec.
: >number-type ( a -- ) >number-value . ; 

: >string-to-number ( a -- a )
    dup >string-length 0 do
        i over >string-at 48 - swap
    loop 
    0
    swap >string-length 0 do
        swap i 10 _unsigned-power * + 
    loop
    make-number ;

: >is-node? ( a -- a ) @ node-symbol = ;
: >node-content ( a -- a ) 1 cells + @ ;
: >node-content! ( a list-a  -- ) 1 cells + ! ;
: >node-next-node ( a -- a ) 2 cells + @ ;
: >node-next-node! ( a list-a -- ) 2 cells + ! ;
: make-empty-node ( -- addr )
    3 cells allocate throw
    dup node-symbol swap ! 
    0 over >node-next-node! ;

: make-list ( -- addr )
    2 cells allocate throw
    dup list-symbol swap !
    dup 1 cells + 0 swap ! ;
: >is-list? ( a -- b ) @ list-symbol = ;
: o>is-list? ( a -- a b ) dup @ list-symbol = ;
: >list-head ( a -- a ) 1 cells + @ ; 
: >list-head! ( a -- ) 1 cells + ! ; 
: >list-empty? ( a -- b ) 1 cells + @ 0= ;
: o>list-empty? ( a -- a b ) dup 1 cells + @ 0= ;

: _list-node-at ( n a-list -- a )
    o>list-empty? if
        drop drop
        s" List index out of range - Empty list" exception throw
    else
        >list-head
        over 1 + 0 do
            over i = if
                swap drop
                unloop 
                exit
            else
                >node-next-node dup
                0= if
                    drop cr s" List index: " type .
                    s" List index out of range" exception throw
                endif
            endif
        loop 
    endif ;

: >list-last-node ( a-list -- a )
    o>list-empty? if
        drop 
        s" Empty list" exception throw
    else
        >list-head
        BEGIN
            dup >node-next-node 0<>
        WHILE
            >node-next-node 
        REPEAT 
    endif ;

: >list-at ( n a-list -- a )
    _list-node-at >node-content ;
: >list-at! ( a-item n a-list )
    _list-node-at >node-content! ;
: >list-append ( an-item a-list )
    o>list-empty? if
        make-empty-node
        swap over swap
        >list-head!
        >node-content!
    else
        >list-last-node 
        make-empty-node
        swap over swap
        >node-next-node! 
        >node-content! 
    endif ;

: >list-length ( a-list -- u )
    o>list-empty? if
        drop 0
    else
        >list-head
        1
        BEGIN
            over >node-next-node 0<>
        WHILE
            1 + 
            swap >node-next-node swap
        REPEAT swap drop 
    endif ;

defer >list-type

: >data-typer ( a -- )
    dup >is-number? if
        >number-type
    else dup >is-string? if
        >string-type
    else dup >is-list? if
        >list-type
    else
        s" r" type .
    endif
    endif
    endif ;

:noname ( a-list -- )
    o>list-empty? if
        drop
        s" '()" type
    else
        s" (" type
        dup >list-length 0 do
            i over >list-at >data-typer
            s"  " type
        loop
        drop
        s" )" type 
    endif ;
is >list-type
