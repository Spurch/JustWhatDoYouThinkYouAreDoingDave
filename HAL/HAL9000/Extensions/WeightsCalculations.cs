namespace HAL9000.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    /// <summary>
    /// 
    /// </summary>
    public class WeightsCalculations
    {
        private static IDictionary<Card, double> cardWeights;
        private static IDictionary<CardSuit, List<Card>> usedCardsInGame;
        private static Card lastOpponentCard;

        private static Stack<Card> spadeStack;
        private static Stack<Card> diamondStack;
        private static Stack<Card> heartStack;
        private static Stack<Card> clubStack;

        /// <summary>
        /// 
        /// </summary>
        static WeightsCalculations()
        {
            cardWeights = new Dictionary<Card, double>();
            foreach (CardSuit suit in Enum.GetValues((typeof(CardSuit))))
            {
                cardWeights[new Card(suit, CardType.Ace)] = 11;
                cardWeights[new Card(suit, CardType.Ten)] = 10;
                cardWeights[new Card(suit, CardType.King)] = 4;
                cardWeights[new Card(suit, CardType.Queen)] = 3;
                cardWeights[new Card(suit, CardType.Jack)] = 2;
                cardWeights[new Card(suit, CardType.Nine)] = 0;
            }

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
                if (card.Type == CardType.Queen && !usedCardsInGame[card.Suit].Select(x => x.Type == CardType.King).Any())
                {
                    weightedPlayerCards[card] += 10;
                }
                if (card.Type == CardType.King && !usedCardsInGame[card.Suit].Select(x => x.Type == CardType.Queen).Any())
                {
                    weightedPlayerCards[card] += 10;
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
            usedCardsInGame = usedCards;
            IDictionary<Card, double> weightedPlayerCards = new Dictionary<Card, double>();

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
            }

            return weightedPlayerCards;
        }

        public static void SetMajorCard()
        {

        }

        private static bool DoWeHaveMajorCardsOfSuit()
        {
            return true;
        }

        private static void InitializeStacks()
        {
            spadeStack = new Stack<Card>();
            diamondStack = new Stack<Card>();
            heartStack = new Stack<Card>();
            clubStack = new Stack<Card>();
        }
    }
}
