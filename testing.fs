: assert ( res -- )
    0= if .s cr s" assertion error" exception throw endif ;
