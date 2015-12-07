namespace HAL9000.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    /// <summary>
    /// 
    /// </summary>
    public partial class WeightsCalculations
    {
        private static IDictionary<Card, double> cardWeights;
        private static IDictionary<CardSuit, List<Card>> usedCardsInGame;
        private static Card lastOpponentCard;
        private static List<Card> majorCards;

        /// <summary>
        /// 
        /// </summary>
        static WeightsCalculations()
        {
            majorCards = new List<Card>();
            cardWeights = new Dictionary<Card, double>();

            for (int i = 0; i < 4; i++)
            {
                var card = new Card((CardSuit)i, CardType.Ace);
                MajorCard.Add(card);

                cardWeights[card] = 11;
                cardWeights[new Card((CardSuit)i, CardType.Ten)] = 10;
                cardWeights[new Card((CardSuit)i, CardType.King)] = 4;
                cardWeights[new Card((CardSuit)i, CardType.Queen)] = 3;
                cardWeights[new Card((CardSuit)i, CardType.Jack)] = 2;
                cardWeights[new Card((CardSuit)i, CardType.Nine)] = 0;
            }

            //InitializeStatistics();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentHand"></param>
        /// <param name="usedCards"></param>
        /// <returns></returns>
        public static IDictionary<Card, double> EvaluateWeightsInBaseState(PlayerTurnContext context,
            ICollection<Card> currentHand, IDictionary<CardSuit, List<Card>> usedCards)
        {
            usedCardsInGame = usedCards;
            IDictionary<Card, double> weightedPlayerCards = new Dictionary<Card, double>();

            foreach (var card in currentHand)
            {
                weightedPlayerCards.Add(card, cardWeights[card]);
                if (card.Suit == context.TrumpCard.Suit)
                {
                    weightedPlayerCards[card] += 12;
                }
                if (usedCardsInGame.ContainsKey(card.Suit))
                {
                    if (card.Type == CardType.Queen &&
                        !usedCardsInGame[card.Suit].Select(x => x.Type == CardType.King).Any())
                    {
                        weightedPlayerCards[card] += 10;
                    }
                    if (card.Type == CardType.King &&
                        !usedCardsInGame[card.Suit].Select(x => x.Type == CardType.Queen).Any())
                    {
                        weightedPlayerCards[card] += 10;
                    }
                }
                else
                {
                    if (card.Type == CardType.Queen || card.Type == CardType.King)
                    {
                        weightedPlayerCards[card] += 10;
                    }
                }
                
            }

            return weightedPlayerCards;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentHand"></param>
        /// <param name="usedCards"></param>
        /// <param name="opponentPlayedCard"></param>
        /// <returns></returns>
        public static IDictionary<Card, double> EvaluateWeightsInClosedState(PlayerTurnContext context, ICollection<Card> currentHand,
            IDictionary<CardSuit, List<Card>> usedCards, Card opponentPlayedCard)
        {
            lastOpponentCard = opponentPlayedCard;
            usedCardsInGame = usedCards;
            IDictionary<Card, double> weightedPlayerCards = new Dictionary<Card, double>();

            if (majorCards.Contains(lastOpponentCard))
            {
                majorCards.Remove(lastOpponentCard);
            }

            foreach (var card in currentHand)
            {
                weightedPlayerCards.Add(card, cardWeights[card]);
                if (card.Suit == context.TrumpCard.Suit)
                {
                    weightedPlayerCards[card] += 12;
                }
                if (card.Type == CardType.Queen && !usedCardsInGame[card.Suit].Select(x => x.Type == CardType.King).Any())
                {
                    weightedPlayerCards[card] += 10;
                }
                if (card.Type == CardType.King && !usedCardsInGame[card.Suit].Select(x => x.Type == CardType.Queen).Any())
                {
                    weightedPlayerCards[card] += 10;
                }
                if (majorCards.Contains(card))
                {
                    weightedPlayerCards[card] += 10;
                }
            }

            return weightedPlayerCards;
        }

        /// <summary>
        ///     
        /// </summary>
        public static List<Card> MajorCard 
        {
            get
            {
                return majorCards;
            }
        }
    }
}
