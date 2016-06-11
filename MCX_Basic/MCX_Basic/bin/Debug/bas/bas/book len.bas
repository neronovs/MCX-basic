5 rem a@rhb
10 cls
20 input "Any letters";a$
30 n=len(a$)
40 for l=1 to n
50 b$=mid$(a$,l,1)
60 x=asc(b$)
70 c$=chr$(x+1)
80 print c$;
90 next l
100 end