5 print "Any key press! 'e' for exit."
10 a$=inkey$
15 if a$="e" then goto 40
20 print a$;
30 goto 10
40 print "Hello!"
50 input "input number 1-9 ",n
60 print n