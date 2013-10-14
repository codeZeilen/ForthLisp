s" ./testing.fs" included
s" ./datatypes.fs" included

\ Strings
s" hello you!" make-string 
>string-length 10 
    = assert

s" hello you!" make-string
>is-string? assert

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
