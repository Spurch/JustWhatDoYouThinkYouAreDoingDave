namespace HAL9000.HALLogic
{
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using Extensions;

    /// <summary>
    /// 
    /// </summary>
    public class HelperMethods
    {
        public void UpdateUsedCardsCollections(Card playedCard, IDictionary<CardSuit, List<Card>> usedCards)
        {
            if (usedCards == null)
            {
                usedCards = new Dictionary<CardSuit, List<Card>>();
            }
            if (!usedCards.ContainsKey(playedCard.Suit))
            {
                usedCards.Add(playedCard.Suit, new List<Card>());
            }
            usedCards[playedCard.Suit].Add(playedCard);

            if (CardsEvaluation.MajorCard.Contains(playedCard))
            {
                CardsEvaluation.MajorCard.Remove(playedCard);

                if (playedCard.Type == CardType.Ace || playedCard.Type > (CardType) 9)
                {
                    if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.Ten))
                    {
                        CardsEvaluation.MajorCard.Add(new Card(playedCard.Suit, CardType.Ten));
                    }
                    else if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.King))
                    {
                        CardsEvaluation.MajorCard.Add(new Card(playedCard.Suit, CardType.King));
                    }
                    else if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.Queen))
                    {
                        CardsEvaluation.MajorCard.Add(new Card(playedCard.Suit, CardType.Queen));
                    }
                    else if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.Jack))
                    {
                        CardsEvaluation.MajorCard.Add(new Card(playedCard.Suit, CardType.Jack));
                    }
                }
                else
                {
                    CardsEvaluation.MajorCard.Add(new Card(playedCard.Suit, CardType.Nine));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="currentCard"></param>
        /// <returns></returns>
        public bool CanTryToChangeTrump(PlayerTurnContext context, ICollection<Card> currentCard)
        {
            var hasNineOfTrump = from x in currentCard
                                 where x.Type == CardType.Nine && x.Suit == context.TrumpCard.Suit
                                 select x;
            if (hasNineOfTrump.Any())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="suit"></param>
        /// <param name="currentHand"></param>
        /// <returns></returns>
        public bool HaveCardInHand(CardType type, CardSuit suit, ICollection<Card> currentHand)
        {
            return currentHand.Select(x => x.Type == type && x.Suit == suit).FirstOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="suit"></param>
        /// <param name="currentHand"></param>
        /// <returns></returns>
        public Card GetCardFromHand(CardType type, CardSuit suit, ICollection<Card> currentHand)
        {
            return currentHand.First(x => x.Type == type && x.Suit == suit);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suit"></param>
        /// <returns></returns>
        public bool HaveLonely10FromSuit(CardSuit suit, ICollection<Card> currentHand)
        {
            var has10OfSuit = currentHand.Any(x => x.Type == CardType.Ten && x.Suit == suit);
            var hasOtherCardsOfSuit = currentHand.Count(x => x.Suit == suit);
            if (has10OfSuit && hasOtherCardsOfSuit == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suit"></param>
        /// <returns></returns>
        public Card GetCurrentMajorCardForSuit(CardSuit suit)
        {
            return null;
        }

        /// <summary>
        /// Method that checks if we have a Major trump card.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool DoWeHaveAMajorTrump(PlayerTurnContext context, ICollection<Card> currentHand)
        {
            foreach (var card in currentHand)
            {
                if (card.Suit == context.TrumpCard.Suit)
                {
                    if (CardsEvaluation.MajorCard.Contains(card))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
