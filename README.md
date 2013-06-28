World Domination
==
World Domination a name i made up for an old board game we all know. I didn't want to use the orginall 
name because it is probably copyrighted and don't want any problems with it. But if i tell you it is
played on a board with a picture of a world map, you have to place pieces which stand for armies
on countries, dices are use when you attack another player and you win if you conquer the world. You 
can probably guess which game i mean. 

<a name="contents"/>

Contents
--------
1. [Contents](#contents)
2. [Goal](#goal)
3. [Setup](#setup)
4. [Technique used](#technique used)
5. [State] (#state)
6. [Compile] (#compile)

<a name="goal"/>

Goal
----

I wanted to make something like http://aichallenge.org/ . But this time you can write a bot for a player
in the game world domination. 

<a name="setup"/>

Setup
-----

I game up with the following setup:
- A dll knowing the rules of the game : WDGameEngine.dll;
- A website for watching the game progress;
- A commandline application which controls the game : GameRunner.exe;
- A commandline application for hosting the website and the communication between all the other applications : GameServer.exe; 
- Commandline applications which are the bots

<a name="technique used"/>

Technique used
--------------
- Visual Studio Express 2012 for Windows Desktop
- https://code.google.com/p/moq/ for mocking in the unit tests.
- http://nancyfx.org/ for hosting the website inside GameServer.exe
- http://signalr.net/ for pushing the new game state to the browsers viewing the website and
  communicatie between GameRunner and bots.
- http://www.sqlite.org/ for storing the country and continent structure and logging the game.
- https://code.google.com/p/dapper-dot-net/ ORM for reading the SQLite data and converting it to POCO's.
- http://sqlitestudio.one.pl/ for managing the SQLite database.
- http://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki for using SQLite in c#

<a name="state"/>

State
-----
28-6-2013 Created a side project for drawing the map in javascript https://github.com/taeke/JMD and used
          this for creating content in the newly added GameServer project. Also added a Data project for
          getting the drawing data from the sqlite database.
14-4-2013 The dll is around 90% done. All other things only excist in my mind :)

<a name="compile"/>

Compile
-------
Some tips if you download this and want to compile it:
- Not figured out how to force a nuget install for all the dependencies before building automatically so
  you probably first have to do the nuget installs which are summariced in doc/nuget.txt
- First compile the CreateDB solution
- Second compile the WD solution