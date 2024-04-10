using CustomLogging;
using SolitaireDomain;


var _logger = new Logger();
var _game = new Game(new Player(""));
_logger.Log("===>ENGAGING TURTLES<===");


bool x = _game.Piles.All(p => p.Count(c => c.FaceUp) == 1);


//Assert.IsTrue(_game.Piles.Select((item, index) => new { item, index }).All(x => x.item.Count == x.index + 1));

_logger.Log("===>END OF ENGAGEMENT<===");
//foreach (var card in _deck.Cards)
//{
//    _logger.Log($"{card}");
//}