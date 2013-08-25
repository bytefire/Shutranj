Shutranj
========

This is a chess engine written is C# 5.0.It's pretty stable now and beats Chess Titans, which comes with Windows 7, at level 10. Assuming that Chess Titans level 10 plays at < 1700 ELO, we can say that Shutranj plays at > 1700 ELO. It has a basic UCI support and has been tested to work with Arena GUI. It has some missing functionality and much room for improvement, as listed under 'Work To Do' section.

Features
--------
- Bitboards for board representation
- NegaMax search with Alpha-Beta Pruning
- Iterative deepening
- Parallelised search using lockless transposition tables
- Killer heuristic
- Aspiration windows
- Checking capture moves 
- Basic time controls

Work To Do
----------
This is divided into two parts: Missing Functionality and Improvements.

**Missing Functionality**
- At the moment, pawn promotions only result in queens. The engine should promote a pawn to whatever piece is indicated in the long algebraic move notation.
- Cannot detect draw by repetition.
- Doesn't acknowledge 50-move rule.

**Improvements**
- Null move heuristic 
- Opening book
- Ending tablebases
- Better time controls
- Enhanced UCI support
