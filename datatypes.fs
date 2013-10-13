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
