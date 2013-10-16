s" ./testing.fs" included
s" ./analyzer.fs" included

s" 'hallo'" make-string
>string-sc-string? assert

s" 20" make-string analyze-number?
    assert

s" 'hallo'" make-string analyze-string?
    assert

s" ''" make-string analyze-string?
    assert
