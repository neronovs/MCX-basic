10 f$=""
20 s$=""
30 print "Calc ver. 2.0"
40 print "Input first number and +"
50 a$=inkey$
60 print a$;
70 if a$="+" then goto 100
80 f$=f$+a$
90 goto 50
100 print "->"+f$
110 print "Input second number and ="
120 b$=inkey$
130 print b$;
140 if b$="=" then goto 170
150 s$=s$+b$
160 goto 120
170 x=val(f$)
180 y=val(s$)
190 print "->"+s$
200 print "=",x+y