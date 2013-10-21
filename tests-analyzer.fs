s" ./testing.fs" included
s" ./analyzer.fs" included

s" 'hallo'" make-string
    >string-sc-string? assert

s" 'hallo'" make-string
>string-from-sc-string >string-length 5
    = assert

s" 20" make-string analyze-number?
    assert

s" 'hallo'" make-string analyze-string?
    assert

s" ''" make-string analyze-string?
    assert

s" 20" make-string _sc-analyze 0 swap call-execute >number-value 20 
    = assert

s" 0" make-string _sc-analyze 0 swap call-execute >number-value 0 
    = assert

s" 100000" make-string _sc-analyze 0 swap call-execute >number-value 100000 
    = assert

s" 'hallo'" make-string _sc-analyze 
0 swap call-execute
>string-length 5
    = assert
