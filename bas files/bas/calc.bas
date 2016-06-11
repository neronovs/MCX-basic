10 rem Calculator
20 cls
25 print "1 - сложение"
26 print "2 - вычитание"
27 print "3 - деление"
28 print "4 - умножение"
30 input "Введите номер действия", a
40 if a=1 then goto 100
50 if a=2 then goto 200
60 if a=3 then goto 300
70 if a=4 then goto 400 else goto 30
100 rem Summing
104 if c$="y" then a=res
105 if c$="y" then goto 120
110 input "Сложение. Введите первое слогаемое",a
120 input "Сложение. Введите второе слогаемое",b
130 re=a+b
140 print "Сумма равна "+re
145 res=re
150 goto 500
200 rem Minussing
204 if c$="y" then a=res
205 if c$="y" then goto 220
210 input "Вычитание. Введите вычитаемое",a
220 input "Вычитание. Введите вычитатель",b
230 re=a-b
240 print "Разность равна "+re
245 res=re
250 goto 500
300 rem Deviding
304 if c$="y" then a=res
305 if c$="y" then goto 320
310 input "Деление. Введите делимое",a
320 input "Деление. Введите делитель",b
330 re=a/b
340 print "Часное равно "+re
345 res=re
350 goto 500
400 rem Multiplying
404 if c$="y" then a=res
405 if c$="y" then goto 420
410 input "Умножение. Введите множимое",a
420 input "Умножение. Введите множитель",b
430 re=a*b
440 print "Произведение равно "+re
445 res=re
450 goto 500
500 rem Result
510 input "Для продолжения введите 'y', для выхода любой другой символ",c$
520 if c$="y" then cls else res=0
530 if c$="y" then print "Предыдущий результат равен "+res else c$=" "
540 if c$="y" then goto 25 else b=0