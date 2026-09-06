### basic
char/charset->char/charset

### count
char/charset-match->count increment
count increment-loop back->char/charset
char/charset-miss match->count end
count end-exit->count increment-judge->next node

### match group/lookaround

left boarder-> match group start              //push stack, add group index
-> match logic                               // recored index 
-> match group end                //pop stack, set group end, build sub sequence










