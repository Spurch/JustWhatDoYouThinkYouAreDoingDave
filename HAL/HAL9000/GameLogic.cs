namespace HAL9000
{
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using HALLogic;
    using Extensions;

    public partial class HAL9000
    {
        /// <summary>
        /// Method that checks if we can announce 20 or 40 and returns a queen that we
        /// can use for a possible announce.
        /// </summary>
        /// <param name="context">The current PlayerTurnContext context</param>
        /// <returns>A list of type Card that contains all possible queens to announce - if any, else returns an empty list.</returns>
        private Card CheckForTwentyOrForty(PlayerTurnContext context, ICollection<Card> currentHand)
        {
            foreach (var currentCard in currentHand)
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

        /// <summary>
        /// Returns the current amount of points in our hand.
        /// This method gets into consideration possible announces of 20 or 40.
        /// </summary>
        /// <param name="context">The current PlayerTurnContext</param>
        /// <returns>The current amount of points we are holding.</returns>
        private int CurrentHandPointsForPlayer(PlayerTurnContext context, ICollection<Card> currentHand)
        {
            var currentTrump = context.TrumpCard.Suit;
            var queen = this.CheckForTwentyOrForty(context, currentHand);
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

        private PlayerAction FirstStepState(PlayerTurnContext context, IDictionary<Card, double> weightCards)
        {
            var lowestWeightCard = weightCards.OrderBy(x => x.Value).FirstOrDefault();
            var card = lowestWeightCard.Key;
            var weight = lowestWeightCard.Value;
            Card turnCard = card;
            if (!context.IsFirstPlayerTurn)
            {
                if (Have20Or40(queensFor20Or40) || weight > Constants.TooHighMinimalWeightNumber)
                {
                    var somecard = (from x in this.Cards
                        where
                            x.Suit == oponentCardSuit && x.GetValue() > oponentCardValue
                        orderby x.GetValue()
                        select x).FirstOrDefault();
                    if (somecard != null)
                    {
                        turnCard = somecard;
                    }
                }
            }
            playerHelper.UpdateUsedCardsCollections(turnCard, usedCards);
            return PlayCard(turnCard);
        }
        private PlayerAction MoreThanTwoCardsState(PlayerTurnContext context, IDictionary<Card, double> weightCards)
        {
            var lowestWeightCard = weightCards.OrderBy(x => x.Value).FirstOrDefault();
            var card = lowestWeightCard.Key;
            var weight = lowestWeightCard.Value;
            Card turnCard = card;

            if (context.IsFirstPlayerTurn)
            {
                if (Have20Or40(queensFor20Or40))
                {
                    turnCard = queensFor20Or40;
                }
            }

            if (!context.IsFirstPlayerTurn)
            {
                if (oponentCardValue >= Constants.HighValueOpponentCard)
                {
                    var somecard = (from x in this.Cards
                                    where
                                        x.Suit == oponentCardSuit && x.GetValue() > oponentCardValue
                                    orderby x.GetValue()
                                    select x).FirstOrDefault();
                    if (somecard == null)
                    {
                        if (CardsEvaluation.TrumpsInCurrentHand(this.Cards, context) >= Constants.AmountOfTrumpsToAllowUsToUseThem)
                        {
                            if (playerHelper.HaveCardInHand(CardType.Jack, trumpSuit, possibleCardsToPlay))
                            {
                                var cardTrum = playerHelper.GetCardFromHand(CardType.Jack, trumpSuit, possibleCardsToPlay);
                                turnCard = cardTrum;
                            }
                        }
                    }
                }

                if (Have20Or40(queensFor20Or40) || weight > Constants.TooHighMinimalWeightNumber || this.CloseGame(context, possibleCardsToPlay))
                {
                    var somecard = (from x in this.Cards
                                    where
                                        x.Suit == oponentCardSuit && x.GetValue() > oponentCardValue
                                    orderby x.GetValue()
                                    select x).FirstOrDefault();
                    if (somecard != null)
                    {
                        turnCard = somecard;
                    }
                }

                if (oponentCardValue < Constants.ValueThatWeCanGetWithTen && oponentCardSuit != trumpSuit)

                    if (playerHelper.HaveLonely10FromSuit(oponentCardSuit, possibleCardsToPlay) && oponentCardValue < Constants.ValueThatWeCanGetWithTen && oponentCardSuit != trumpSuit)

                    {
                        turnCard = playerHelper.GetCardFromHand(CardType.Ten, oponentCardSuit, this.possibleCardsToPlay);
                    }
            }
            playerHelper.UpdateUsedCardsCollections(turnCard, this.usedCards);
            return PlayCard(turnCard);
        }

        private PlayerAction TwoCardLeft(PlayerTurnContext context, IDictionary<Card, double> weightCards)
        {
            var lowestWeightCard = weightCards.OrderBy(x => x.Value).FirstOrDefault();
            var card = lowestWeightCard.Key;
            var weight = lowestWeightCard.Value;
            Card turnCard = card;

            if (context.IsFirstPlayerTurn)
            {
                if (Have20Or40(queensFor20Or40))
                {
                    turnCard = queensFor20Or40;
                }
            }

            if (oponentCardValue >= Constants.HighValueOpponentCard && context.TrumpCard.GetValue() < Constants.ValueTooLowToTakeAction)
            {
                var somecard = (from x in this.Cards
                                where
                                    x.Suit == oponentCardSuit && x.GetValue() > oponentCardValue
                                orderby x.GetValue()
                                select x).FirstOrDefault();
                if (somecard == null)
                {
                    if (CardsEvaluation.TrumpsInCurrentHand(this.Cards, context) > Constants.AmountOfTrumpsToAllowUsToUseThem)
                    {
                        if (playerHelper.HaveCardInHand(CardType.Jack, trumpSuit, possibleCardsToPlay))
                        {
                            var cardTrum = playerHelper.GetCardFromHand(CardType.Jack, trumpSuit, possibleCardsToPlay);
                            turnCard = cardTrum;
                        }
                    }
                }
            }
            playerHelper.UpdateUsedCardsCollections(turnCard, usedCards);
            return PlayCard(turnCard);
        }
        private PlayerAction ClosedState(PlayerTurnContext context, IDictionary<Card, double> weightCards)
        {
            var sortedWight = weightCards.OrderBy(x => x.Value);
            var lowestWeightCard = sortedWight.FirstOrDefault();
            var lowestCard = lowestWeightCard.Key;
            var lowestWeight = lowestWeightCard.Value;
            var hightWeightCard = sortedWight.LastOrDefault();
            var hightCard = lowestWeightCard.Key;
            var hightWeight = lowestWeightCard.Value;
            Card turnCard = lowestCard;

            if (context.IsFirstPlayerTurn)
            {
                if (Have20Or40(queensFor20Or40))
                {
                    turnCard = queensFor20Or40;
                }
                else
                {
                    if (CardsEvaluation.HowManyTrumpCardsHasTheOpponent (this.Cards, usedCards, context) == 0)
                    {
                        var somecard = sortedWight.LastOrDefault(x => x.Key.Suit != trumpSuit).Key;
                        turnCard = somecard;
                        if (somecard == null)
                        {
                            turnCard = hightCard;
                        }
                    }
                    if (CardsEvaluation.HowManyTrumpCardsHasTheOpponent(this.Cards, usedCards, context) <= CardsEvaluation.TrumpsInCurrentHand(this.Cards, context))
                    {
                        if (playerHelper.DoWeHaveAMajorTrump(context, possibleCardsToPlay))
                        {
                            var somecard = sortedWight.LastOrDefault(x => x.Key.Suit == trumpSuit).Key;
                            turnCard = somecard;
                            if (somecard == null)
                            {
                                turnCard = hightCard;
                            }
                        }

                        else
                        {
                            var somecard = sortedWight.FirstOrDefault(x => x.Key.Suit == trumpSuit).Key;
                            turnCard = somecard;
                            if (somecard == null)
                            {
                                turnCard = hightCard;
                            }
                        }
                        turnCard = hightCard;
                    }
                    //if (WeightsCalculations.HowManyTrumpCardsHasTheOpponent(this.Cards, usedCards, context) > WeightsCalculations.TrumpsInCurrentHand(this.Cards, context))
                    //{
                    //    if (wehavesuitbig)
                    //    {
                    //        var somecard = sortedWight.FirstOrDefault(x => x.Key.Suit == bigSuit).Key;
                    //        turnCard = somecard;
                    //    }
                    //    turnCard = hightCard;
                    //}
                }
            }
            playerHelper.UpdateUsedCardsCollections(turnCard, usedCards);
            return PlayCard(turnCard);
        }

        private PlayerAction FinalState(PlayerTurnContext context, IDictionary<Card, double> weightCards)
        {

            return null;
        }
    }
}
