namespace HAL9000.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using HALLogic;

    /// <summary>
    /// 
    /// </summary>
    public partial class WeightsCalculations
    {
        private static IDictionary<Card, double> cardWeights = new Dictionary<Card, double>();
        private static List<Card> majorCards = new List<Card>();
        private static IDictionary<Card, double> weightedPlayerCards;
        /// <summary>
        /// 
        /// </summary>
        static WeightsCalculations()
        {
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
            weightedPlayerCards = new Dictionary<Card, double>();

            StandartWeightCheck(currentHand, usedCards, context);

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
            weightedPlayerCards = new Dictionary<Card, double>();

            if (majorCards.Contains(opponentPlayedCard))
            {
                majorCards.Remove(opponentPlayedCard);
            }

            StandartWeightCheck(currentHand, usedCards, context);

            foreach (var card in currentHand)
            {
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
        /// <param name="context"></param>
        /// <param name="currentHand"></param>
        /// <param name="usedCards"></param>
        /// <param name="opponentHand"></param>
        /// <returns></returns>
        public static IDictionary<Card, double> EvaluateWeightsInFinalState(PlayerTurnContext context,
            ICollection<Card> currentHand,
            IDictionary<CardSuit, List<Card>> usedCards, List<Card> opponentHand)
        {
            weightedPlayerCards = new Dictionary<Card, double>();

            Constants.OpponentHasClub = opponentHand.Any(x => x.Suit == CardSuit.Club);
            Constants.OpponentHasHeart = opponentHand.Any(x => x.Suit == CardSuit.Heart);
            Constants.OpponentHasDiamond = opponentHand.Any(x => x.Suit == CardSuit.Diamond);
            Constants.OpponentHasSpade = opponentHand.Any(x => x.Suit == CardSuit.Spade);
            Constants.OpponentHasTrump = opponentHand.Any(x => x.Suit == context.TrumpCard.Suit);

            foreach (var card in opponentHand)
            {
                if (MajorCard.Contains(card))
                {
                    switch (card.Suit)
                    {
                        case CardSuit.Club:
                            if (Constants.OpponentHasClub)
                                Constants.OpponentHasMajorClub = true;
                            break;
                        case CardSuit.Heart:
                            if (Constants.OpponentHasHeart)
                                Constants.OpponentHasMajorHeart = true;
                            break;
                        case CardSuit.Diamond:
                            if (Constants.OpponentHasDiamond)
                                Constants.OpponentHasMajorDiamond = true;
                            break;
                        case CardSuit.Spade:
                            if (Constants.OpponentHasSpade)
                                Constants.OpponentHasMajorSpade = true;
                            break;
                    }
                }
            }

            StandartWeightCheck(currentHand, usedCards, context);

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

        private static void StandartWeightCheck(ICollection<Card> currentHand, IDictionary<CardSuit, List<Card>> usedCards, PlayerTurnContext context)
        {
            foreach (var card in currentHand)
            {
                weightedPlayerCards.Add(card, cardWeights[card]);
                if (card.Suit == context.TrumpCard.Suit)
                {
                    weightedPlayerCards[card] += 12;
                }
                if (usedCards.ContainsKey(card.Suit))
                {
                    if (card.Type == CardType.Queen &&
                        !usedCards[card.Suit].Select(x => x.Type == CardType.King).Any())
                    {
                        weightedPlayerCards[card] += 10;
                    }
                    if (card.Type == CardType.King &&
                        !usedCards[card.Suit].Select(x => x.Type == CardType.Queen).Any())
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
        }
    }
}
