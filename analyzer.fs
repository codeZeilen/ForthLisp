s" ./datatypes.fs" included 

defer _sc-analyze

\ *****************************************
\ Strings 
\ *****************************************

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

\ *****************************************
\ Numbers 
\ *****************************************

: analyze-number? ( a -- b ) ( exp -- b )
    >string-numeric? ;

: o-analyze-number? ( a -- a b )
    dup analyze-number? ;

: _number-execution ( a a -- a ) ( evt number -- number )
    swap drop ;

: analyze-number ( a -- a ) ( string -- 1call )
    >string-to-number ['] _number-execution make-1call ;

\ *****************************************
\ Application
\ *****************************************

: analyze-application? ( a -- b ) ( exp -- b )
    >is-list? ;

: o-analyze-application? ( a -- b ) ( exp -- b )
    dup >is-list? ;

: _application-execution ( a a -- a ) ( evt list -- ? )
    \ Execute each element in the list passing
    \ it the environment
    \ Apply operator result list[0] on the operands
   ;

: analyze-application ( a -- a ) ( list -- ncall )
    ['] _sc-analyze >list-map \ Analyze on operator and operands
    _application-execution make-ncall ;

\ *****************************************
\ Analyzer 
\ *****************************************

:noname ( a -- a ) ( parsed-list -- analyzed-tree )
    o-analyze-string? if
        analyze-string
    else o-analyze-number? if
        analyze-number
    \ TODO including environments
    \ else o-analyze-variable? if
    \    analyze-variable
    else o-analyze-application? if
        analyze-application
    else
        >data-typer
        s" Unknown Expression -- Analyzer" exception throw
    endif
    endif
    endif ;
is _sc-analyze

: sc-analyze ( a -- a ) ( parsed-list -- analyzed tree )
    ;
