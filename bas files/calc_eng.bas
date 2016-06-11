1 rem calculator
5 cls
10 m=0
11 res=0
12 c$=" "
13 a=0
14 b=0
17 re=0
20 cls
23 print "Memory (m) = "+m
24 print "------------------"
25 print "1 - addition"
26 print "2 - subtraction"
27 print "3 - division"
28 print "4 - multiplication"
30 input "Enter a number of action:", a
40 if a=1 then goto 100
50 if a=2 then goto 200
60 if a=3 then goto 300
70 if a=4 then goto 400 else goto 20
100 rem summing
110 if c$="y" then print "The first addendum = "+res else input "Addition. Enter the first addendum or 'm' to use the memory",a$
115 if a$="m" then a=m else a=val(a$)
117 if c$="y" then a=res
120 input "Addition. Enter the second addendum or 'm' to use the memory",b$
125 if b$="m" then b=m else b=val(b$)
130 re=a+b
140 print "The sum is "+re
145 res=re
150 goto 1000
200 rem subtraction
210 if c$="y" then print "Subtrahend = "+res else  input "Subtraction. Enter a subtrahend or 'm' to use the memory",a$
215 if a$="m" then a=m else a=val(a$)
217 if c$="y" then a=res
220 input "Subtraction. Enter a subtractor or 'm' to use the memory",b$
225 if b$="m" then b=m else b=val(b$)
230 re=a-b
240 print "Difference is "+re
245 res=re
250 goto 1000
300 rem dividing
310 if c$="y" then print "Dividend = "+res else input "Division. Enter a dividend or 'm' to use the memory ",a$
315 if a$="m" then a=m else a=val(a$)
317 if c$="y" then a=res
320 input "Division. Enter a divider or 'm' to use the memory",b$
325 if b$="m" then b=m else b=val(b$)
330 re=a/b
340 print "Часное равно "+re
345 res=re
350 goto 1000
400 rem multiplying
410 if c$="y" then print "Multiplicand = "+res else input "Multiplication. Enter a multiplicand or 'm' to use the memory",a$
415 if a$="m" then a=m else a=val(a$)
417 if c$="y" then a=res
420 input "Multiplication. Enter a multiplier or 'm' to use the memory",b$
425 if b$="m" then b=m else b=val(b$)
430 re=a*b
440 print "Result of multiplication is "+re
445 res=re
450 goto 1000
1000 rem result
1010 input "To remember enter 'm', to continue of a calculation enter 'y' or enter another symbol to exit ",c$
1013 goto 1020
1015 input " to continue of a calculation enter 'y' or enter another symbol to exit",c$
1020 if c$="m" then m=res else goto 1030
1021 goto 1015
1030 if c$="y" then cls else res=0
1040 if c$="y" then print "The previous result is "+res else c$=" "
1050 if c$="y" then goto 13 else b=0