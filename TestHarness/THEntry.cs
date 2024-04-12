using CustomLogging;
using SolitaireDomain;
using static SolitaireDomain.EnumCardRank;
using static SolitaireDomain.EnumCardSuit;


var _logger = new Logger();
var _player = new Player("TurtleBot3000");
var _game = new Game(_player);

var x = _game.Piles[0].Last().Color;

_logger.Log("===>ENGAGING TURTLES<===");

_logger.Log($"{x}");


_logger.Log("===>END OF ENGAGEMENT<===");