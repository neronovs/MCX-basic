10 dim n$(4)
20 dim t$(4)
30 for l=0 to 4
40 read n$(l),t$(l)
50 next l
60 for l=0 to 4
70 print n$(l),t$(l)
80 next l
81 r=10
82 s=140
90 restore r+s
100 read a$
110 print a$
120 end
130 data peter,"111-2222"
140 data paul,"222-3333"
150 data mary,333-4444
160 data tom,444-5555
170 data susie,555-6666