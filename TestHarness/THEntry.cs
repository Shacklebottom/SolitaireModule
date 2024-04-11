using CustomLogging;
using SolitaireDomain;
using static SolitaireDomain.CardEnum;


var _logger = new Logger();
var _player = new Player("TurtleBot3000");
var _game = new Game(_player);



_logger.Log("===>ENGAGING TURTLES<===");




_logger.Log("===>END OF ENGAGEMENT<===");