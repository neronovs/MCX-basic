10 dim a(5)
20 dim b(5)
30 for i=0 to 5
40 a(i)=i
50 b(i)=i*i
60 next
70 for i=0 to 5
80 print a(i)+"^2=";
90 print b(i)
100 if a(i)=b(i) then print"equal"
110 if a(i)<b(i) then print"less"
120 if a(i)>=b(i) then print"more or equal"
121 if a(i)><b(i) then print"not equal"
130 next