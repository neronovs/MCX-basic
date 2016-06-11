10 dim a(3,3)
12 a(0,0)=10
20 a(1,1)=11
30 a(2,2)=12
33 a(3,3)=33
34 x=2
35 y=2
40 print a(1,1)
50 print a(2,2)
55 print a(x,y)
60 s=10
70 dim b(s,s)
80 for x=0 to s
90 for y=0 to s
100 b(x,y)=y*10+x
101 print b(x,y)+" y="+y*10+" x="+x+" summ="+y*10+x
110 next y
120 next x
130 for x=0 to s
140 for y=0 to s
150 print b(x,y)+" ";
160 next y
170 print " "
180 next x