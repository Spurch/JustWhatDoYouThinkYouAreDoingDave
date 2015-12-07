using System.Collections.Generic;
using HAL9000.HALLogic;

namespace HAL9000.Extensions
{
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

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
        public int TrumpsInCurrentHand(ICollection<Card> cards, PlayerTurnContext context)
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
        public int HowManyTrumpCardsHasTheOpponent(ICollection<Card> cards, IDictionary<CardSuit, List<Card>> usedCards, PlayerTurnContext context, bool isItClosed = true)
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
        public int TrumpCardsInGraveyard(IDictionary<CardSuit, List<Card>> cards, PlayerTurnContext context)
        {
            var trumpsAlreadyPlayed = cards[context.TrumpCard.Suit].Count;

            return trumpsAlreadyPlayed;
        }

        private static void CalculateStatistics()
        {
            
        }
    }
}
