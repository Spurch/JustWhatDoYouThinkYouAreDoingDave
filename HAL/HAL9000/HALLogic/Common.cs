namespace HAL9000
{
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using Extensions;

    public partial class HAL9000
    {
        private void UpdateUsedCardsCollections(Card playedCard)
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

            if (WeightsCalculations.MajorCard.Contains(playedCard))
            {
                WeightsCalculations.MajorCard.Remove(playedCard);

                if (playedCard.Type == CardType.Ace || playedCard.Type > (CardType) 9)
                {
                    if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.Ten))
                    {
                        WeightsCalculations.MajorCard.Add(new Card(playedCard.Suit, CardType.Ten));
                    }
                    else if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.King))
                    {
                        WeightsCalculations.MajorCard.Add(new Card(playedCard.Suit, CardType.King));
                    }
                    else if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.Queen))
                    {
                        WeightsCalculations.MajorCard.Add(new Card(playedCard.Suit, CardType.Queen));
                    }
                    else if (usedCards[playedCard.Suit].Any(x => x.Type != CardType.Jack))
                    {
                        WeightsCalculations.MajorCard.Add(new Card(playedCard.Suit, CardType.Jack));
                    }
                }
                else
                {
                    WeightsCalculations.MajorCard.Add(new Card(playedCard.Suit, CardType.Nine));
                }
            }
        }

        /// <summary>
        /// Returns the current amount of points in our hand.
        /// This method gets into consideration possible announces of 20 or 40.
        /// </summary>
        /// <param name="context">The current PlayerTurnContext</param>
        /// <returns>The current amount of points we are holding.</returns>
        private int CurrentHandPoints(PlayerTurnContext context)
        {
            var currentTrump = context.TrumpCard.Suit;
            var queen = this.CheckForTwentyOrForty(context);
            int result = 0;
            if (queen != null)
            {
                if (queen.Suit == currentTrump)
                {
                    result += 40;
                }
                else
                {
                    result += 20;
                }

                result +=
                    this.Cards.Where(x => x.Type != CardType.King && x.Type != CardType.Queen)
                        .Select(y => y.Type)
                        .Cast<int>()
                        .Sum();
            }
            else
            {
                result = this.Cards.Select(y => y.Type)
                        .Cast<int>()
                        .Sum();
            }

            return result;
        }

        /// <summary>
        /// Method that checks if we can announce 20 or 40 and returns a queen that we
        /// can use for a possible announce.
        /// </summary>
        /// <param name="context">The current PlayerTurnContext context</param>
        /// <returns>A list of type Card that contains all possible queens to announce - if any, else returns an empty list.</returns>
        private Card CheckForTwentyOrForty(PlayerTurnContext context)
        {
            foreach (var currentCard in this.Cards)
            {
                if (currentCard.Type == CardType.Queen
                    && this.AnnounceValidator.GetPossibleAnnounce(
                        this.Cards, currentCard, context.TrumpCard) == Announce.Forty)
                {
                    return currentCard;
                }
                if (currentCard.Type == CardType.Queen
                         &&
                         this.AnnounceValidator.GetPossibleAnnounce(
                             this.Cards, currentCard, context.TrumpCard) == Announce.Twenty)
                {
                    return currentCard;
                }
            }
            return null;
        }

        private bool Have20Or40(Card listOfQueens)
        {
            if (listOfQueens == null)
            {
                return false;
            }
            return true;
        }

        private bool CanTryToChangeTrump(PlayerTurnContext context)
        {
            var hasNineOfTrump = from x in this.Cards
                                 where x.Type == CardType.Nine && x.Suit == context.TrumpCard.Suit
                                 select x;
            if (hasNineOfTrump.Any())
            {
                return true;
            }
            return false;
        }

        private bool HaveCardInHand(CardType type, CardSuit suit)
        {
            return this.Cards.Select(x => x.Type == type && x.Suit == suit).FirstOrDefault();
        }

        private Card GetCardFromHand(CardType type, CardSuit suit)
        {
            return this.Cards.First(x => x.Type == type && x.Suit == suit);
        }

        private bool HaveLonely10FromSuit(CardSuit suit)
        {
            var has10OfSuit = this.Cards.Any(x => x.Type == CardType.Ten && x.Suit == suit);
            var hasOtherCardsOfSuit = this.Cards.Count(x => x.Suit == suit);
            if (has10OfSuit && hasOtherCardsOfSuit == 1)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Method that checks if we have a Major trump card.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public bool DoWeHaveAMajorTrump(PlayerTurnContext context)
        {
            foreach (var card in this.Cards)
            {
                if (card.Suit == context.TrumpCard.Suit)
                {
                    if (WeightsCalculations.MajorCard.Contains(card))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
