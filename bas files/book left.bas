10 dim n$(10)
20 y=1
30 for l=1 to 10
40 input "Name";n$(l)
50 next l
60 cls
70 for l=1 to 10
80 print n$(l)
90 next l
95 print "------- j sorted -------"
100 for l=1 to 10
110 a$=left$(n$(l),1)
120 if a$<>"j" then goto 150
130 print n$(l)
150 next l