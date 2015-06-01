s" ./datatypes.fs" included

: sc-interpret ( a a -- ) ( environment string -- )
    sc-parse sc-analyze sc-execute
