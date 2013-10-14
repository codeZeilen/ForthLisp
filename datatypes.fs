: string-symbol 1 ;
: number-symbol 2 ;
: list-symbol 3 ;
: node-symbol 4 ;

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

: >is-node? ( a -- a ) @ node-symbol = ;
: >node-content ( a -- a ) 1 cells + @ ;
: >node-content! ( a list-a  -- ) 1 cells + ! ;
: >node-next-node ( a -- a ) 2 cells + @ ;
: >node-next-node! ( a list-a -- ) 2 cells + ! ;
: make-empty-node ( -- addr )
    here 3 cells allot
    dup node-symbol swap ! 
    0 over >node-next-node! ;

: make-list ( -- addr )
    here 2 cells allot
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

