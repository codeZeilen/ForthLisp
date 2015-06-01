s" ./testing.fs" included

the-empty-environment s"(+ 1 2)" sc-interpret
>number-value 3
    = assert

