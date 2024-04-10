using CustomLogging;
using SolitaireDomain;


var _logger = new Logger();
var _game = new Game(new Player(""));
_logger.Log("===>ENGAGING TURTLES<===");


_logger.Log("===>END OF ENGAGEMENT<===");
//foreach (var card in _deck.Cards)
//{
//    _logger.Log($"{card}");
//}