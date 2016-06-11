10 rem checkbk.bas                                                    
20 rem a simple check book balancing program                          
30 rem copyright 1987, 1992  s m estvanik                             
40 rem                                                                
50 CLS                                                              
60 PRINT "Check Book Balancing Program"                             
80 INPUT "What is your opening balance",BALANCE                     
90 PRINT " "                 
100 INPUT "Next transaction? (D/eposit, C/heck, Q/uit)",T$                                                    
110 IF T$<> "d" then GOTO 210          
120    INPUT "Amount of deposit",DEPOSIT                            
126 PRINT "DEPOSIT =", DEPOSIT              
130    BALANCE = BALANCE + DEPOSIT                
140    PRINT "New balance is $",BALANCE               
150       GOTO 90                                                   
210 IF T$<> "c" then GOTO 300           
220    INPUT "Amount of check" ,CHECK                                
230    BALANCE = BALANCE - CHECK            
240    PRINT "New balance is $",BALANCE               
250       GOTO 90                                                   
300 IF T$<> "q" then GOTO 90     
400 PRINT " "                 
410 PRINT "Final balance is $",BALANCE                
430 END