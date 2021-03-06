s" ./testing.fs" included
s" ./datatypes.fs" included

\ *****************************************
\ Char Extension Tests
\ *****************************************
48 _char-numeric? assert \ ASCII 0
57 _char-numeric? assert \ ASCII 9
54 _char-numeric? assert \ ASCII 6 
47 _char-numeric? invert assert 
58 _char-numeric? invert assert 

\ *****************************************
\ Unsigned Extension Tests
\ *****************************************
2 4 _unsigned-power 16
    = assert

0 4 _unsigned-power 1
    = assert

4 2 _unsigned-power 16
    = assert

\ *****************************************
\ String Tests
\ *****************************************
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

s" 30" make-string
>string-to-number >number-value 30
    = assert

\ *****************************************
\ Number Tests
\ *****************************************
variable a-number
20 make-number a-number !
a-number @ >number-value 20 
    = assert

\ *****************************************
\ List Tests
\ *****************************************
variable a-list
make-list a-list !
a-list @ >list-length 0
    = assert
a-number @ a-list @ >list-append
a-number @ >number-value 20 
    = assert

a-list @ >list-head 
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

make-list a-list !
20 a-list @ >list-append
30 a-list @ >list-append
a-list @ >list-expand
30 =
    assert
20 =
    assert

: map-test-fun1 
    20 + ;
' map-test-fun1 a-list @ >list-map
0 over >list-at 
40 =
    assert
1 over >list-at 
50 =
    assert

\ *****************************************
\ Call Tests
\ *****************************************
: call-test-fun1
    10 + ;

10 ' call-test-fun1 make-1call >is-call? 
assert

10 ' call-test-fun1 make-1call call-execute
20 = 
    assert

: call-test-fun2
    10 + + ;

10 10 ' call-test-fun2 make-2call >is-call? 
assert

10 10 ' call-test-fun2 make-2call call-execute
30 = 
    assert

: call-test-fun3
    10 + + + ;

10 10 10 ' call-test-fun3 make-3call >is-call? 
assert

10 10 10 ' call-test-fun3 make-3call call-execute
40 = 
    assert

: call-test-funn
    + + + ;

30 20 10 5 make-list 
swap over >list-append 
swap over >list-append 
swap over >list-append 
swap over >list-append 
' call-test-funn make-ncall call-execute
65 =
    assert
