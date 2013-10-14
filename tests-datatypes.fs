s" ./testing.fs" included
s" ./datatypes.fs" included

\ Char extension

48 _char-numeric? assert \ ASCII 0
57 _char-numeric? assert \ ASCII 9
54 _char-numeric? assert \ ASCII 6 
47 _char-numeric? invert assert 
58 _char-numeric? invert assert 

\ unsigned extension
2 4 _unsigned-power 16
    = assert

0 4 _unsigned-power 1
    = assert

4 2 _unsigned-power 16
    = assert

\ Strings
s" hello you!" make-string 
>string-length 10 
    = assert

s" hello you!" make-string
>is-string? assert

s" 20" make-string
0 swap >string-at 50
    = assert

s" hallo" make-string
4 swap >string-at 111
    = assert

s" 20" make-string
>string-to-number >is-number?
    assert

s" 20" make-string
>string-to-number >number-value 20
    = assert

\ Numbers
create a-number
20 make-number a-number !
a-number @ >number-value 20 
    = assert

\ Lists
create a-list
make-list a-list !
a-list @ >list-length 0
    = assert
a-number @ a-list @ >list-append
a-number @ >number-value 20 
    = assert

a-list @ >node-next-node 
    0<> assert

a-list @ >list-head >node-content 
a-number @ 
    = assert

a-list @ >list-length
1
    = assert

a-list @ >list-head >node-content >number-value 
a-number @ >number-value 
    = assert

0 a-list @ >list-at 
a-list @ >list-head >node-content
    = assert

30 make-number a-list @ >list-append 
1 a-list @ >list-at >number-value 30 
    = assert
a-list @ >list-length 2 
    = assert

30 make-number a-list @ >list-append 
a-list @ >list-length 3 
    = assert
