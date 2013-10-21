s" ./datatypes.fs" included 

: >string-sc-string? ( a -- b )
    0 over >string-at 39 =
    over dup >string-length 1 - swap >string-at 39 =
    and ;

: >string-from-sc-string ( a -- a )
    dup >string-content 1 chars +
    over >string-length 2 - dup >r r@ chars allocate throw >r r@ \ Get memory
    swap cmove 
    r> r> make-string ;

: analyze-string? ( a -- b ) ( exp -- b )
   >string-sc-string? ; 

: o-analyze-string? ( a -- a b )
    dup analyze-string? ;

: _string-execution ( a a -- a ) ( evt string -- string )
    swap drop ;

: analyze-string ( a -- a ) ( string -- 1call )
    >string-from-sc-string ['] _string-execution make-1call ;

: analyze-number? ( a -- b ) ( exp -- b )
    >string-numeric? ;

: o-analyze-number? ( a -- a b )
    dup analyze-number? ;

: _number-execution ( a a -- a ) ( evt number -- number )
    swap drop ;

: analyze-number ( a -- a ) ( string -- 1call )
    >string-to-number ['] _number-execution make-1call ;

: _sc-analyze ( a -- a ) ( parsed-list -- analyzed-tree )
    o-analyze-string? if
        analyze-string
    else o-analyze-number? if
        analyze-number
    endif
    endif ;

: sc-analyze ( a -- a ) ( parsed-list -- analyzed tree )
    ;
