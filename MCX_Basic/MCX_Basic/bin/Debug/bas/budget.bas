10 rem budget.bas                                                    
20 rem a simple check book balancing program                          
30 rem copyright 1987, 1992  s m estvanik                             
40 rem                                                                
50 CLS                                                              
60 PRINT "Check Book Balancing Program"                             
80 INPUT "What is your opening balance",BALANCE                     
90 PRINT " "                 
100 INPUT "Next transaction? (D/eposit, C/heck, Q/uit)",T$                                                    
110 IF T$<> "d" then GOTO 210          
120 input "Amount of deposit",deposit                            
126 PRINT "DEPOSIT =", DEPOSIT              
130 balance = balance + deposit                
140 print "New balance is $",balance               
150 goto 90                                                   
210 IF T$<> "c" then GOTO 300           
220 input "Amount of check" ,check                                
230 balance = balance - check            
240 print "New balance is $",balance               
250 goto 90                                                   
300 IF T$<> "q" then GOTO 90     
400 PRINT " "                 
410 PRINT "Final balance is $",BALANCE                
430 END