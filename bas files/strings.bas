5 let b$=spc$(2)+spc$(3)
6 print b$
10 let a$=spc$(4)
15 print a$
20 print "aaa",a$,"bbb",b$,"ccc"
30 print "111",spc$(1),"222",spc$(2),"333"
40 print "bin of 19 is ",bin$(19)
50 let c$=chr$(72)
60 print "AAAA_",c$,chr$(73)
70 let d$=hex$(89)
80 print d$,"  ",left$("aaaabbbbc",4)
90 let e$="To be or not to be"
100 let f$=right$(e$,8)
110 print "e-->",e$," f-->",f$
120 print mid$(e$,3,8)
130 print "oct of 190=",oct$(190)
140 print string$(54,87)