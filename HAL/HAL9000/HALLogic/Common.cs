namespace HAL9000
{
    using System.Collections.Generic;
    using Santase.Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;

    public partial class HAL9000
    {
        /// <summary>
        /// Method that checks if we can announce 20 or 40 and returns a list of the queens that we
        /// can use for possible announces.
        /// </summary>
        /// <param name="context">The current PlayerTurnContext context</param>
        /// <param name="currentPossibleCardsToPlay">The current possible cards to play.</param>
        /// <returns>A list of type Card that contains all possible queens to announce - if any, else returns an empty list.</returns>
        public List<Card> CheckForTwentyOrForty(PlayerTurnContext context, ICollection<Card> currentPossibleCardsToPlay)
        {
            var queensList = new List<Card>();
            foreach (var currentCard in currentPossibleCardsToPlay)
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
    }
}
