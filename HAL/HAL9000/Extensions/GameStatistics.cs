namespace HAL9000.Extensions
{
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using HALLogic;
    using System.Collections.Generic;

    public partial class WeightsCalculations
    {
        private static void InitializeStatistics()
        {
            foreach (var card in cardWeights)
            {
                cardWeights[card.Key] += Constants.GetSingleCardProbability;
                if (card.Key.Type == CardType.Queen || card.Key.Type == CardType.King)
                {
                    cardWeights[card.Key] += Constants.GetAnnounceOnFirstHand;
                }
            }
        }

        /// <summary>
        /// Returns the amount of trump cards in your current hand.
        /// </summary>
        /// <param name="context">Current PlayerTurnContext</param>
        /// <returns>Returns the current number of trumps or 0 if none.</returns>
        public static int TrumpsInCurrentHand(ICollection<Card> cards, PlayerTurnContext context)
        {
            return cards.Select(x => x.Suit == context.TrumpCard.Suit).Count();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="usedCards"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int HowManyTrumpCardsHasTheOpponent(ICollection<Card> cards, IDictionary<CardSuit, List<Card>> usedCards, PlayerTurnContext context, bool isItClosed = true)
        {
            var ourTrumpCards = TrumpsInCurrentHand(cards, context);
            var playedTrumps = TrumpCardsInGraveyard(usedCards, context);
            if (isItClosed)
            {
                playedTrumps += 1;
            }
            return Constants.TotalNumberOfCardsOfGivenSuit - ourTrumpCards - playedTrumps;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cards"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static int TrumpCardsInGraveyard(IDictionary<CardSuit, List<Card>> cards, PlayerTurnContext context)
        {
            int trumpsAlreadyPlayed = 0;
            if (cards.ContainsKey(context.TrumpCard.Suit))
            {
                trumpsAlreadyPlayed = cards[context.TrumpCard.Suit].Count;
            }


            return trumpsAlreadyPlayed;
        }

        private static bool GetOpponentSuitState(CardSuit suit)
        {
            switch (suit)
            {
                case CardSuit.Club:
                    return Constants.OpponentHasClub;// && Constants.OpponentHasMajorClub;
                    break;

                case CardSuit.Heart:
                    return Constants.OpponentHasHeart;// && Constants.OpponentHasMajorHeart;
                    break;
                case CardSuit.Diamond:
                    return Constants.OpponentHasDiamond;// && Constants.OpponentHasDiamond;
                    break;
                case CardSuit.Spade:
                    return Constants.OpponentHasSpade;// && Constants.OpponentHasMajorSpade;
                    break;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Method to calculate the cards left in the opponents hand.
        /// </summary>
        /// <param name="possibleCardsToPlay"></param>
        /// <param name="usedCards"></param>
        /// <returns></returns>
        public static List<Card> GetOpponentHand(ICollection<Card> possibleCardsToPlay, Dictionary<CardSuit, List<Card>> usedCards)
        {
            List<Card> opponentHand = new List<Card>();
            foreach (var suit in usedCards)
            {
                foreach (var innerCard in suit.Value)
                {
                    var result = cardWeights.Keys.FirstOrDefault(y => !y.Equals(innerCard));
                    opponentHand.Add(result);
                }
            }
            foreach (var card in possibleCardsToPlay)
            {
                if (opponentHand.Contains(card))
                {
                    opponentHand.Remove(card);
                }
            }

            return opponentHand;
        } 

        private static void CalculateStatistics()
        {

        }
    }
}
