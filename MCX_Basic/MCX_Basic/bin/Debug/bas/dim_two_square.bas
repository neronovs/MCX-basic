10 s=10
20 dim b(s,s)
30 for x=0 to s
40 for y=0 to s
50 u=y*10+x
60 b(x,y)=u
70 print b(x,y)+" y="+y*10+" x="+x+" summ="+u
80 next y
90 next x
100 for x=0 to s
110 for y=0 to s
120 print b(x,y)+" ";
130 next y
140 print " "
150 next x