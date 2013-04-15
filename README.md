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

<a name="goal"/>

Goal
----

I wanted to make something like http://aichallenge.org/ . But this time you can write a bot for a player
in the game world domination. 

<a name="setup"/>

Setup
-----

I game up with the following setup:
- A dll knowing the rules of the game : WDGameEngine.dll
- A commandline application which controls the game : contoller.exe
- Commandline applications which are the bots
- A website for watching the game

<a name="technique used"/>

Technique used
--------------
- Visual Studio Express 2012 for Windows Desktop
- http://nancyfx.org/ for hosting the website inside controller.exe
- http://signalr.net/ for pushing the new game state to the browsers viewing the website.
- Named pipes for communication with the bots.

<a name="state"/>

State
-----
14-4-2013 The dll is around 90% done. All other things only excist in my mind :)

