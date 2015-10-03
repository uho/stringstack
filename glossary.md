String Stack Glossary
=====================

("         ( ccc<)> -- )
------------------------
Start a string stack comment.


" ( ccc<"> -- ) (" -- s )
-------------------------

Parse characters form the input stream delimited by ".
Put the parsed sttring on top of the string stack.
The word " is state smart and can be used in interpretation
and in compilation state.


"! (" s -- ) ( addr -- )  
------------------------
Copy the string **s** as a cell counted string to the memory at **adddr**.
Remove **s** from the string stack.


"" ( -- ) (" -- s )
-------------------
Put the empty stack on top of the string stack.


"-roll ( +n -- )  (" sn ... s0 -- s0 sn ... s1 )
------------------------------------------------
Move the top most string on the string stack to the nth position on the string stack.


".  (" s -- )
-------------
Print the topmost string from the string, remove it.


".s  ( -- ) (" -- )
-------------------
Show the contents of the string stack with the topmost string first.


"< ( -- f )  (" s0 s1 -- )
--------------------------
Determine whether or not the second most string **s1** is lexicographically less than the
topmost string **s0**.


"<= ( -- f ) (" s0 s1 -- )
--------------------------
Determine whether or not the second most string **s1** is lexicographically less or equal than the
topmost string **s0**.


"= ( -- f )  (" s0 s1 -- ) 
--------------------------
Determine whether or not the top kost strings on the string stack are equail.


"@ ( addr -- ) (" -- s )
------------------------
Fetch a cell counted string from memory and put it on the string stack.


"allot ( len -- )
-----------------
Allocate a new string stack and put its anchor address in **"stack**.


"append ( c -- ) (" s0 -- s1 )
------------------------------
Append the character *c* at the end of the topmost string on the string stack.


"clear ( -- ) (" s*i -- )
-------------------------
Remove all items from the string stack.


"compare ( -- n ) (" s0 s1 -- )
-------------------------------
Compare the string **s0** and **s1**. If **s0** is lexicographically less than **s1** return -1.
If they are equal return 0. If **s0** is greater than **s1** return 1.

     
"Constant ( name< > -- ) (" s -- )
----------------------------------
Define a new string constant named **name**. When **name** is later
executed it puts the string **s** on the top of string stack.


"count ( -- addr len ) (" s -- s )
----------------------------------
Get the address and length of the topmost string from the string stack.
*addr* ist the address of the forst character in the string.


"delimiter-join (" s1 s2 ... sn delim -- s ) ( n -- )
-----------------------------------------------------
Concatenate the **n** strings **sn**, ... **s2**, **s1**
(**sn** at the beginning of the resulting string **s**)
interspersed with the delimiter string **delim**.
`" ef"  " cd"  " ab" 3  " /"  "delimiter-join` results in `ab/cd/ef.`


"delimiter-split (" s0 delim -- s1 ... sn ) ( -- n )
----------------------------------------------------
Split the text **s0** on occurances of the delimiter string **delim**.
**s1** to **sn** are the resulting parts. **sn** is the closest
to the beginning of **s0**. The parts do no longer contain the delimiter.
**n** is the number of parts. The effect of **"delimiter-split**
can be reversed by calling **"delimiter-join** with the same delimiter.


"depth ( -- +n ) (" sn-1 ... s0 -- sn-1 ... s0 )
------------------------------------------------
Get the number of strings on the string stack.


"drop ( --  ) (" s -- )
-----------------------
Remove the topmost string from the string stack.


"dup ( -- ) (" s -- s s ) 
-------------------------
Put another copy of the top most string on the stack on top of the string stack.


"expect ( +n -- )  (" -- s )
----------------------------
Accept user input from the terminal and put the entered string on
top of the string stack.


"extract ( +n0 +n1 -- )  (" s0 -- s1 )
--------------------------------------
Retrieve the substring of the top most string on the string stack
starting at character position **+n0** and ending at **+n1**. 
`" abcdefghi" 3 5 "extract` results in the string `de`.


"join ( -- ) (" s0 s1 -- s2 )
-----------------------------
Concatenate the characters of the string **s1** to those of **s0** giving the string **s2**.
**s0** and **s1** are removed from the string stack.


"joins ( n -- ) (" s0 s1 .. sn -- s )
-------------------------------------
Remove and concatenate the **n** topmost strings, so that the resulting string **s**
has the characters of **sn**, **sn-1**, ... **s1**, and **s0**.


"length ( -- len ) (" s -- s )
------------------------------
Get the length of the topmost string on the stringstack.


"over ( -- ) (" s0 s1 -- s0 s1 s0 )
-----------------------------------
Put another copy of the second most string on the stack on top of the string stack.


"pick ( +n -- )  (" sn ... s0 -- sn ... s0 sn )
----------------------------------------------
Copy the nth string from the string stack and put it on top of the string stack.


"pop ( -- addr len ) (" s -- )
------------------------------
Remove the topmost string from the stringstack and leave 
its address and lenght on the data stack.


"positions (" s1 s2 -- s1 ) ( -- n1 n2 ... nn n )
-------------------------------------------------
Find all the positions of the string **s2** inside the text **s1**
and put them on the stack. **n** is the number of occurances.


"push  ( addr len -- )  (" -- s )
---------------------------------
Put the string given by *addr* and *len* on top of the string stack.


"roll ( +n -- )  (" sn ... s0 -- sn-1 ... s0 sn )
-------------------------------------------------
Move the nth string from within the string stack to the topmost location.


"rot  ( -- ) (" s0 s1 s2 -- s1 s2 s0 )
--------------------------------------
Move the third most string on the string stack to the top most position.


"search (" s1 s2 -- s1 )  ( -- n f )
-----------------------------------------
Find the string **s2** inside the text **s1**. If **s2** is found leave 
its starting position **n** within **s1** and a true flag on the stack.
If not found put a length of **s1** and a false flag on the stack.
`" abcdefg" " de" "search` results in 3 and true on the stack.


"split ( +n -- ) (" s0 -- s1 s2 )
---------------------------------
Cut the topmost string **s0** at character position **+n** into the
prefix **s2** and the suffix **s1**. 
`" abcdefghi" 3 "split` puts the strings `defghi` (**s1**, second most) 
and `abc` (**s2**, topmost) on the string stack.


"stack  ( -- addr )
-------------------
Put the address of teh variable **"stack** on the stack. **"stack** is the anchor
address of the current string stack. Switching addresses in **"stack**
switches to a another stack. **"allot** allocates a new string stack
and initializes **"stack** to this stack.


"substitute (" s1 s2 s3 -- s1' )  ( -- )
----------------------------------------
Replace the first occurcance of the string **s2** in the text **s1** by the string **s3**
putting the resulting string on top of the string stack.
If **s2** is not found in **s1**, **s1** remains unchanged.


"swap ( -- ) (" s0 s1 -- s1 s0 ) 
--------------------------------
Exchange top most and second most string on the string stack.


"th ( +n -- addr ) (" s -- s )
------------------------------
Retrieve the address of nth string (counting from 0) from the string stack.
The string stack is not modified. *addr* is the address of a cell counted string
(suitable to be processed by **length**).


"Variable ( name< > +n --  )
----------------------------
Define a new strung variable named **name**. Whem **name** is later
executed it puts its adress on the data stack suitable for **"!** and **"@**.

c"push ( char -- )  (" -- s )
-----------------------------
Put the char **char** as a string on top of the string stack.


length ( a-addr1 -- a-addr2 u )
-------------------------------
Get the length *u* and the address **a-addr2** of the first character of
the call counted string at **a-addr1**.