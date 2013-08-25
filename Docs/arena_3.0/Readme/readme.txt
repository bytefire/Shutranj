This is Version 3.0 of the Chess GUI Arena!
----------------------------------------------

http://www.playwitharena.com

Version History:
-----------------------------------------

Arena 3.0, 2010-12-24

   * UCIFilter fixed+improved
   * Removing engines from memory after aborting tournament mode works now
   * It is now possible to make an engine weaker in ICS mode 
   * Mainlines can be saved as variations in PGN (optional)
   * Nodes/sec now works for Winboard engines
   * New option for Knodes/second instead of nodes/second
   * Sound at start of Arena now hearable
   * Autoplayer repaired
   * Wine tournament bug fixed: engines can restart now
   * Wine font bug fixed: If fonts Arial or Courier New not available, choose appropriate one, not random one
   * Wine helpfile reader added as separate download
   * Wine does not show Chess Merida Arena's mask. Mask disabled on wine, so graphics is not spoilled 
   * If no font "Wingdings" available (common on Wine), show simple '+','=' and '-' characters, not wrong ones
   * Inactive buttons in tournament mode visible, not invisible, but inactive as they should
   * Auto-Analysis: Comprehensive output in report starts with 1
   * Auto-Analysis: Bug removed where positions were not analyzed
   * Auto-Analysis: New option for adding parameter info in report file
   * In PGN output duplicate game result in braces removed, e.g.: {1-0 Arena adjudication} 1-0
   * Superflous "..." after last move in PGN removed
   * Winboard options "memory", "cores" and "egtpath" now globally setable
   * Bug in combo boxes in Winboard options removed
   * Option "Large Logos" works again


Arena 2.5 beta, 2010-11-18

   * Public Beta
   * Helpfiles updated to new version
   * Bugfixing for large fonts
   * Bugfixing for restart
   * Adjustments for 64 bit compatibility
   
   * Most important changes compared to last public version 2.0.1:
     * Maximum 8 engine can run at the same time instead of 3 
	 * Tablebases
	 * Better compatibility to Linux/WINE
	 * extended options to mark the last move, e.g. with arrow
	 * Appearance adjusts to Windows theme
	 
   * Known bugs/limitations:
	* Option "Large Logos" does not work
	* Operating system Windows XP or Wine 1.0 or higher necessary
	* NAGs above Nr. 24 don't work

Arena 2.4.0, 2010-11-02
   * Internal release for test group
   Arena 2.4.0 problems fixed/ new functions:
   * Appearance set by Windows theme (if file "Arena.exe.manifest" exists)
   * Antialiasing for arrows and marks
   * Lots of new options to mark the current move
   * New: Mark next move
   * Font and colour for diaolgues settable
   * You can set when the 50 moves rule applies
   * Background of the engine name in the analysis area can be set as the arrow colour(under "Configure analysis lines")
   * Some new icons
   * Two colums in the analysis area (pop-up menu of engines -> view -> two columns)
   * Aggregated view of engine output (pop-up menu of engines -> aggregated view)
   * Support for Gaviota Tablebases: some errors removed
   * Change: Gaviota engine now must be called gaviota.tb (not gaviota.exe), in order to avoid starting the same engine twice
   * "Shelf" renamed to "Temp"
   * Size of clocks scales to size of the area automatically
   * Size of captured pieces scales to size of the area automatically
   * Additional delay during engine start phase adjustable
   * TLCV compatible logging under Engines -> Log-Window
   * ICS time management works better
   * EPD analysis error removed
   * Sliding pieces under Linux/Wine
   * Some useful commands added in the main menu uncer "Extras" and "Help"

Arena 2.0.6, 2010-07-02
   * Internal release for test group
   Arena 2.0.6 problems fixed/ new functions:
   * Support for Gaviota Tablebases
   * Engines in ICS work
   * EPD-List some Bugs removed

Arena 2.0.5, 2010-06-21
   * Internal release for test group
   Arena 2.0.5 problems fixed/ new functions:

   Appearance:
   Mix-Tab-Width now saved
   Frame around movelist thinner
   In Maim Menu now 8 Engines configurable
   HTML-Editor setable in Options -> Appearrance -> Other settings

   Analysis: 
   Analyse starts "on the fly", if Analysis-Configuration is changed.

   Demo:
   Multi-Demo while right-click on demo button now works

   Tournament:
   CPU-usage lowered
   Abort Tounrnament confirmation dialogue:  now in tournament window
   gauntlet tournament:auto-rotate board option
   reght mouse on scrollbar -> Scroll here etc popup menu doesn't appear
   Early Draw: settable
   HTML-Editor invokable from tournament dialog -> save

   ICS:
   Setting ICS port after server now works, e.g.: server.org:9000
   If with engine: "set 1 This is a computer account" now correctly sent
   After ending ICS mode no empty engine areas shown

   Engine debug window:
   Options/ Logging new design
   Logging: Configuration simpler
   Logging: Writing to file arena.debug also possible after several lines instead of always after one
   Logging: Real date/time can be logged also

   Configuration:
   PVListbox no longer saved in arenagui.cfg 

   
Arena 2.0.1, 2009-01-18

 * Bestmove-statistics status remembered also for engine one
 * Better displaying quality for TrueType pieces, if Windows has FontSmoothing off
 * Color changes in the analysis lines are now applied at once also for engine two and three
 * Material difference is now switched according to the board orientaion
 * Analysis lines from Crafty shown again
 * Faint shadows during moving white TrueType-pieces now gone
 * English translation for Russian codepage added
 * Missing English translation added in ICS-options: Use time safety buffer for search (seconds) 
 * EPD-list drag+drop can be disabled now (better for Wine)
    

Arena 2.0, 2008-12-22

    * Support for chess server (ICS) simplified and improved
    * Colourschemes now saveable
    * New speed test
    * Time on clocks is now adjustable
    
Arena 1.99beta5,  2008-02-09

    * You can use a different font now for printing as for the screen
    * PGN tags can now be set write-protected
    * You can choose, which PGN tags are saved
    * Option: Load last PGN when starting Arena
    * Confirmation dialogue, if a game, that is not saved, would be lost
    * New dialogue for loading engines
    * simple beep sounds by the PC speaker example beep: 520:100 (Hz:milliseconds),
	  set instead of sound file
    * %ArenaDrive% now stands for the Arena drive in Arena's configuration files,
      e.g. "C:" Therefore ArenaENG.cfg and ArenaGUI.cfg generated with this version
	  cannot be used with older versions of Arena

Arena 1.99beta4, 2007-03-31

    * Automatic Analysis extended and improved, e.g. fixed ply depth possible
    * Pro Deo can now analyse Arena-logfiles of the automatic analysis, see Ed Schröder's page
    * Fixed move format for the movelist
    * Printing, also support of several more fonts for the chessboard than former versions had
    * Support for Novag Citrine
    * Tournament filenames settable individually
    * A few fonts adjustable for the tournament tabs
    * UCI-Protocol: Superflous stop-command removed
    * Analysis Lines improved: Adjustable height if 2 or more engines, the same mainlines are removed except first and last (faster for MTDf-Engines), horizontal colorization

Arena 1.99beta3, 2006-11-08

    * New Helpfile-Interface (english helpfile is available above)
    * 2 new functions for saving game to and reloading game from PGN file
    * Order of buttons on the toolbar can now be changed


Arena 1.99beta2, 2006-01-20

    * Tournaments should run correct now
    * DGT and Autoplayer repaired
    * Tournament table in HTML format improved
    * No other changes to the Beta 1, no additional important functions

Arena 1.99beta1, 2005-12-23

    * New chessboard with a lot of new functions
    * New movelist that supports variations
    * Display of attacked pieces
    * Functions for managing PGN files like copy, delete etc. (please be careful, beta version!)
    * Extended configuration options
    * Colour schemes
    * Analysing now possible with three engines at the same time
    * Use of configuration files, not of the registry


Version 1.1, 2004-12-17

    * completely new look of the analysis lines.
    * material difference can be shown.
    * UCI-2 support, so there is the possibility to limit the playing strength, calculate refutations etc., if the engine supports this
    * new views of the movelists.
    * for Winboard engines the possibilities to save the supported features and show them in a table.
    * extentions in the board set-up.
    * "am"-tag now supported in EPD analysis feature
    * the option to save the Arena configuration.
    * transpositions in the opening book.
    * support for the DGT-XL clock.
    * special support for Prodeo (set-up position and current move).
    * special support for CraftySE in UCI-mode (display of the personality).
    * a lot of additional details added/imporoved.


Suggestions, proposals:
-------------------------
..are always welcome!

Website: www.playwitharena.com. Here you can find recent buglists, help on
common problems etc..

I'm grateful for every help, especially on the following fields:
---------------------------------------------------------------------
Testers are welcome! Please contact us via the contact form on our website.

If anybody likes to translate the program, please let me know! Translations
are already available in a lot of languages.

And now, have fun with Arena!

Martin Blume
Hamm, Germany


