Hermann is a engine without an own graphical user interface. Arena or Winboard
is recommended.

The logos come from Patrick Buchmann, David Dahlem, Alex Schmidt and Graham Banks.

Private users may use and copy Hermann unchanged. Everything else needs the written
permission of the author.

Because Hermann is licensed free of charge, there is no warranty of any kind.

Starting with Hermann 2.2 there is a 64 bit version for windows and Hermann is optimised
for 64 bit. There will still be 32 bit versions but they may be slower and therefor
weaker then Hermann 2.0.


Have fun
Volker Annuss (e-mail: hermann-25@nnuss.de)


Configuration
=============

Hermann supports the UCI- and the xboard/Winboard protocol.

Version 1.3 and later support the UCI protocol's multi principal variation mode.
With a value greater than 1 for the option "MultiPV", multiple principal variations
are sent to the GUI. This requires more computing time, because weaker variations
must be examined more detailed. And in this mode, optimal timing is impossible.
So I recommend to use it for analysis only.

More options are described in the file Hermann.ini. This file is used for the
xboard/Winboard protocol only, but it also contains a description for some
UCI options.



Thanks
======

To my wife Sabine for her patience when I developed this program.

To Christian Bartsch, not only for testing, but also for "listening" to my e-mails
which gave me some good ideas for many years.

To Kurt Utzinger for his very precise Tests.

To Jörg Nowack for testing Chess960.

To all who used Hermann in their Tournaments and have published the results and/or
sent me error reports.

To all who published their Ideas about chess programming.


Book creation
=============

Hermann's book creation is still experimental and will be improved in later versions.
Hermann.exe must be started directly without graphical user interface for book creation.

Set UCI mode for accepting book commands by typing

    uci

An opening book is made from one ore more PGN-files. With

    book add <PGN file> [value <v>] [ELO <e>] [all]

files are added to a book.

An optional value parameter specifies the weight of the games in a file. Default is 1000.

An optional ELO parameter filters games from a file, where both players have at least that
ELO number.

With an optional all parameter, all games from the file are used, even very short ones
and duplicates are not excluded.

A file may be added more than once with different parameters. Each game from the file will
be used only once with its maximum value.


The book is written with

    book write <filename> [width <w>] [threshold <t>]

The parameter width specifies the width of the book. A big value makes Hermann choose from
a bigger set of moves but it may play weaker. Default is 25 which is a relativly wide book.

The parameter threshold is the sum of values (from the value parameters in book add ...)
that a move must have to come into the book. Default is 3000.


Example for book creation:
    book add twic478.pgn value 1000 ELO 2600
    book add twic478.pgn value 500 ELO 2400
    book add GM2001.pgn
    book write MyBook.opn width 10 threshold 1500
    quit
