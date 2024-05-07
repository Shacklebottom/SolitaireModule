using CustomLogging;
using SolitaireDomain.Objects;

Logger _logger = new Logger();

Player _player = new Player();
StandardDeck _deck = new StandardDeck();
Pile[] _pile = new Pile[7];

for (int i = 0; i < _pile.Length; i++)
{
    _pile[i] = new Pile();
}

Foundation[] _foundations = new Foundation[4];

for (int i = 0; i < _foundations.Length; i++)
{
    _foundations[i] = new Foundation();
}

Game _game = new Game(_deck, _foundations, _pile, _player);




_logger.Log("==>INITIALIZING TURTLECRAFT<==");

while (!_game.GameOver())
{
    PromptPlayer();
}

void PromptPlayer()
{
    var userResponse = "";
    while (true)
    {
        userResponse = Console.ReadLine();
    }
}

for (var i = 0; i < _pile.Length; i++)
{
    Thread.Sleep(500);

    _logger.Log($"Pile {i} has {_pile[i].Cards.Count} cards");

    foreach (var card in _pile[i].Cards)
    {
        _logger.Log($"{card}");
    }
    _logger.Log("\n");
}

_logger.Log($"There are {_deck.Cards.Count} cards left in the deck");