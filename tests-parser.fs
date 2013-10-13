s" ./testing.fs" included
s" ./parser.fs" included

9 is-whitespace? assert
10 is-whitespace? assert
32 is-whitespace? assert

40 is-opening-bracket? assert
41 is-closing-bracket? assert

40 is-bracket? assert
41 is-bracket? assert

\ s" hallo mein haus" simple-parse-word . .
\ s" hallo" simple-parse-word . .
\ s"  hallo   mein  haus" simple-word-parse . .

s" (+ 20 (+ 3 4) (foobar 2 3 4))" sc-parse 
s" hallo mein haus" sc-parse .s
