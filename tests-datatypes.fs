s" ./testing.fs" included
s" ./datatypes.fs" included

s" hello you!" make-string 
>string-length 10 
    = assert

create a-number
20 make-number a-number !
a-number @ >number-value 20 
    = assert

create a-list
make-new-list a-list !
a-number @ a-list @ >list-append
a-number @ >number-value 20 
    = assert

a-list @ >node-next-node 
    0<> assert

a-list @ >node-next-node >node-content 
a-number @ 
    = assert

a-list @ >list-length
2
    = assert

a-list @ >node-next-node >node-content >number-value 
a-number @ >number-value 
    = assert

1 a-list @ >list-at 
a-list @ >node-next-node >node-content
    = assert

30 make-number a-list @ >list-append 
2 a-list @ >list-at >number-value 30 
    = assert
a-list @ >list-length 3 
    = assert

30 make-number a-list @ >list-append 
a-list @ >list-length 4 
    = assert
