namespace HAL9000
{
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    public partial class HAL9000
    {
        private List<Card> usedSpades = new List<Card>();
        private List<Card> usedDiamonds = new List<Card>();
        private List<Card> usedHearts = new List<Card>();
        private List<Card> usedClubs = new List<Card>();

        private void UpdateUsedCardsCollections(Card firstPlayerCard, Card secondPlayerCard)
        {
            switch (firstPlayerCard.Suit)
            {
                case CardSuit.Spade:
                    usedSpades.Add(firstPlayerCard);
                    break;
                case CardSuit.Diamond:
                    usedDiamonds.Add(firstPlayerCard);
                    break;
                case CardSuit.Heart:
                    usedHearts.Add(firstPlayerCard);
                    break;
                case CardSuit.Club:
                    usedClubs.Add(firstPlayerCard);
                    break;
            }
            switch (secondPlayerCard.Suit)
            {
                case CardSuit.Spade:
                    usedSpades.Add(secondPlayerCard);
                    break;
                case CardSuit.Diamond:
                    usedDiamonds.Add(secondPlayerCard);
                    break;
                case CardSuit.Heart:
                    usedHearts.Add(secondPlayerCard);
                    break;
                case CardSuit.Club:
                    usedClubs.Add(secondPlayerCard);
                    break;
            }
        }

        /// <summary>
        /// Returns the amount of trump cards in your current hand.
        /// </summary>
        /// <param name="context">Current PlayerTurnContext</param>
        /// <returns>Returns the current number of trumps or 0 if none.</returns>
        private int TrumpsInCurrentHand(PlayerTurnContext context)
        {
            return this.Cards.Select(x => x.Suit == context.TrumpCard.Suit).Count();
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
            var queens = this.CheckForTwentyOrForty(context);
            int result = 0;
            if (queens.Count > 0)
            {
                foreach (var queen in queens)
                {
                    if (queen.Suit == currentTrump)
                    {
                        result += 40;
                    }
                    else
                    {
                        result += 20;
                    }
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
        /// Method that checks if we can announce 20 or 40 and returns a list of the queens that we
        /// can use for possible announces.
        /// </summary>
        /// <param name="context">The current PlayerTurnContext context</param>
        /// <param name="currentPossibleCardsToPlay">The current possible cards to play.</param>
        /// <returns>A list of type Card that contains all possible queens to announce - if any, else returns an empty list.</returns>
        private List<Card> CheckForTwentyOrForty(PlayerTurnContext context)
        {
            var queensList = new List<Card>();
            foreach (var currentCard in this.Cards)
            {
                if (currentCard.Type == CardType.Queen
                    && (this.AnnounceValidator.GetPossibleAnnounce(this.Cards, currentCard, context.TrumpCard) == Announce.Twenty
                    || this.AnnounceValidator.GetPossibleAnnounce(this.Cards, currentCard, context.TrumpCard) == Announce.Forty))
                {
                    queensList.Add(currentCard);
                }
            }
            return queensList;
        }

        private bool Have20Or40(ICollection<Card> listOfQueens)
        {
            if (listOfQueens.Count == 0)
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
            if (hasNineOfTrump.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
