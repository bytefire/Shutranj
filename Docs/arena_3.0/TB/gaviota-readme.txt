.........|.........|.........|.........|.........|.........|.........|.........|

Gaviota Chess Engine
Copyright (c) 2000-2010 Miguel A. Ballicora
-------------------------------------------------------------------------------

Gaviota (Spanish word for Seagull) is a chess engine available for both Windows 
and Linux. The code has been written as portable as possible, so Gaviota could 
support other OS in the future.

Gaviota is only an engine, so it needs to be plugged to a proper chess GUI
(Graphic user interface). Current version fully supports the latest Winboard/
Xboard with all its new features. Gaviota also supports the UCI (Universal 
Chess Interface) protocol. Gaviota can be used with some free interfaces such 
as Arena (Windows), Winboard/Xboard, ChessGUI, and Scid and any commercial 
interface that supports these protocols.

Please follow the instructions of your favorite GUI on how to connect engines.

Gaviota is one of the chess engines with more features. It has its own book, 
learning, own endgame table bases, and can use up to sixteen processors or 
cores (SMP), and as mentioned above, supports Windows, Linux and the all the 
features of Winboard II and most of the features of UCI. MultiPV is still not 
supported, but it will be in the next versions.

---- License ------------------------------------------------------------------

Copyright (C) 2000-2010, Miguel A. Ballicora
 
This software is provided 'as-is', without any express or implied warranty of 
any kind. In no event or circumstance will the author be held liable for any 
damages arising from the use of this software. Anyone is permitted to use this
software, but its distribution is not allowed without the consent of the
author. The origin of this software must not be misrepresented and you must
not claim that you wrote the original software. Modification of the software
is not permitted. (mails to the gmail account: mballicora)

---- Usage --------------------------------------------------------------------

Gaviota can also be executed from the console to be used as an analysis tool,
generate tablebases, or opening books. If from console it is executed
(do not forget to precede it with ./ if you are running in Linux)

gaviota -v

You will obtain the version number

gaviota

will run as a winboard or UCI engine.

gaviota -x

will be forced to be run as winboard engine

gaviota -s

will be more friendly if you want to use it as a analysis tool from console.
type 'help', and you will get all the available commands. For instance, you
will get

Gaviota v0.75.7
Copyright (c) 2000-2010 Miguel A. Ballicora
There is NO WARRANTY of any kind
# mode = winboard/xboard
# Type 'screen' for a better output in console mode
# Type 'help' for a list of useful commands

help
use:
help <command>
for more information

?           accepted    adjudicate  analyze     bk          black       
book        bookpgn     bookpgnwb   clearbrain  clearlearn  cores       
d           display     easy        egtbpath    egtpath     epdtest     
evaltest    exclude     fen         force       fritz       go          
hard        hasha       hashb       hashl       hashp       hashr       
hinfo       hint        include     kibitz      learnf      level       
log         memory      name        new         nopost      nowait      
nps         option      otim        pause       perft       perftdiv    
ping        playother   post        protover    quit        random      
rejected    remove      reset       result      score       screen      
script      sd          see         setboard    sn          st          
tb          tbgen       tbtest      tbuse       tbval       tbvali      
testbail    time        traceroot   uci         undo        usermove    
wait        white       xboard 

---- Book building and special help -------------------------------------------

Then, if you need more help about a specific command you type help <command>
For instance, to know more about bookpgnwb, which is one of the tools to
build books, you type

help bookpgnwb

bookpgnwb <ply_limit> <white_input> <black_input> <bookfile_ouput>
creates a book with the name bookfile_output from the files
white_input (white repertoire) and black_input (black repertoire).
These two input files should be in pgn format. The maximum length
the variations is limited by the number ply_limit
For example:
bookpgnwb 30 white.pgn black.pgn book.bin
Aftewards, the file book.bin should be registered in the ini file

---- Configuration-------------------------------------------------------------

As many native Winboard/Xboard engines, Gaviota can be configured by
modifying a special text file. In this case, it is "gaviota.ini.txt"
The options in this file are self-explanatory. In addition, if you use 
Winboard or Xboard, you can configure the engine options directly from the 
graphical interface. 

If you use Gaviota as a UCI engine, most likely the GUI will provide a menu for
you to change the options.

---- Acknowledgments ----------------------------------------------------------

You can find an updated acknowledgement note at

http://sites.google.com/site/gaviotachessengine/Home/acknowledgments

Enjoy Gaviota, and you may find more support at

http://sites.google.com/site/gaviotachessengine/

Miguel A. Ballicora






 


