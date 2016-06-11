10 gosub 110
20 print "return from first gosub"
30 print "Line number 30"
40 print "Line number 40"
50 print "lets go to 2nd gosub!"
60 gosub 150
70 print "return from second gosub"
80 print "Line number 80"
90 print "Line number 90"
100 end
110 print "first gosub"
120 print "Line number 120"
130 print "Line number 130"
140 return
150 print "It is second gosub"
160 return