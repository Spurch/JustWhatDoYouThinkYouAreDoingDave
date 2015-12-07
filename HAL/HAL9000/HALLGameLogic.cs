namespace HAL9000
{
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using HALLogic;
    using Extensions;

    public partial class HAL9000
    {
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
            UpdateUsedCardsCollections(turnCard);
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
                        if (WeightsCalculations.TrumpsInCurrentHand(this.Cards, context) >= Constants.AmountOfTrumpsToAllowUsToUseThem)
                        {
                            if (HaveCardInHand(CardType.Jack, trumpSuit))
                            {
                                var cardTrum = GetCardFromHand(CardType.Jack, trumpSuit);
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

                    if (HaveLonely10FromSuit(oponentCardSuit) && oponentCardValue < Constants.ValueThatWeCanGetWithTen && oponentCardSuit != trumpSuit)

                    {
                        turnCard = GetCardFromHand(CardType.Ten, oponentCardSuit);
                    }
            }
            UpdateUsedCardsCollections(turnCard);
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
                    if (WeightsCalculations.TrumpsInCurrentHand(this.Cards, context) > Constants.AmountOfTrumpsToAllowUsToUseThem)
                    {
                        if (HaveCardInHand(CardType.Jack, trumpSuit))
                        {
                            var cardTrum = GetCardFromHand(CardType.Jack, trumpSuit);
                            turnCard = cardTrum;
                        }
                    }
                }
            }
            UpdateUsedCardsCollections(turnCard);
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
                    if (WeightsCalculations.HowManyTrumpCardsHasTheOpponent (this.Cards, usedCards, context) == 0)
                    {
                        var somecard = sortedWight.LastOrDefault(x => x.Key.Suit != trumpSuit).Key;
                        turnCard = somecard;
                        if (somecard == null)
                        {
                            turnCard = hightCard;
                        }
                    }
                    if (WeightsCalculations.HowManyTrumpCardsHasTheOpponent(this.Cards, usedCards, context) <= WeightsCalculations.TrumpsInCurrentHand(this.Cards, context))
                    {
                        if (DoWeHaveAMajorTrump(context))
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
            UpdateUsedCardsCollections(turnCard);
            return PlayCard(turnCard);
        }

        private PlayerAction FinalState(PlayerTurnContext context, IDictionary<Card, double> weightCards)
        {

            return null;
        }
    }
}
