s" ./datatypes.fs" included 

: >string-sc-string? ( a -- b )
    0 over >string-at 39 =
    over dup >string-length 1 - swap >string-at 39 =
    and ;

: analyze-string? ( a -- b ) ( exp -- b )
   >string-sc-string? ; 

: analyze-number? ( a -- b ) ( exp -- b )
    >string-numeric? ;

: analyze-number
    ;

: _sc-analyze ( a -- a ) ( parsed-list -- analyzed-tree )
    o-analyze-string? if

    else o-analyze-number? if

    endif
    endif ;

: sc-analyze ( a -- a ) ( parsed-list -- analyzed tree )
