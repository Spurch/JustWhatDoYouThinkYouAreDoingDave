namespace HAL9000
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic.Cards;
    using Santase.Logic.Players;
    using System.Collections.Generic;
    using System.Linq;
    using Santase.Logic.Cards;
    using HALLogic;
    using Extensions;

    public partial class HAL9000
    {
        private PlayerAction FirstStepState(PlayerTurnContext context, Dictionary<Card, double> weightCards)
        {
            var lowestWeightCard = weightCards.OrderBy(x => x.Value).First();
            var card = lowestWeightCard.Key;
            var weight = lowestWeightCard.Value;
            Card turnCard = card;
            if (!context.IsFirstPlayerTurn)
            {
                if (Have20Or40(queensFor20Or40) || weight > Constants.TOOHIGHTMINWEIGHTNUMBER)
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
            return PlayCard(turnCard);
        }
        private PlayerAction MoreThanTwoCardsState(PlayerTurnContext context, Dictionary<Card, double> weightCards)
        {
            var lowestWeightCard = weightCards.OrderBy(x => x.Value).First();
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
                if (oponentCardValue >= Constants.NIGHVALUETOOPONENTSCARD)
                {
                    var somecard = (from x in this.Cards
                                    where
                                        x.Suit == oponentCardSuit && x.GetValue() > oponentCardValue
                                    orderby x.GetValue()
                                    select x).FirstOrDefault();
                    if (somecard == null)
                    {
                        if (GameStatistics.TrumpsInCurrentHand(this.Cards, context) >= Constants.COUNTTRUMPMORETHANCANGETWITHTRUMP)
                        {
                            if (HaveCardInHand(CardType.Jack, trumpSuit))
                            {
                                var cardTrum = GetCardFromHand(CardType.Jack, trumpSuit);
                                turnCard = cardTrum;
                            }
                        }
                    }
                }

                if (Have20Or40(queensFor20Or40) || weight > Constants.TOOHIGHTMINWEIGHTNUMBER || this.CloseGame(context, possibleCardsToPlay))
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

                if (oponentCardValue < Constants.LESSVALUETHATWECANGETWITHTEN && oponentCardSuit != trumpSuit)

                    if (HaveLonely10FromSuit(oponentCardSuit) && oponentCardValue < Constants.LESSVALUETHATWECANGETWITHTEN && oponentCardSuit != trumpSuit)

                    {
                        turnCard = GetCardFromHand(CardType.Ten, oponentCardSuit);
                    }
            }
            return PlayCard(turnCard);
        }

        private PlayerAction TwoCardLeft(PlayerTurnContext context, Dictionary<Card, double> weightCards)
        {
            var lowestWeightCard = weightCards.OrderBy(x => x.Value).First();
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

            if (oponentCardValue >= Constants.NIGHVALUETOOPONENTSCARD && context.TrumpCard.GetValue() < Constants.TOOLOWVALUEFORWEMAKEANYTHINK)
            {
                var somecard = (from x in this.Cards
                                where
                                    x.Suit == oponentCardSuit && x.GetValue() > oponentCardValue
                                orderby x.GetValue()
                                select x).FirstOrDefault();
                if (somecard == null)
                {
                    if (GameStatistics.TrumpsInCurrentHand(this.Cards, context) > Constants.COUNTTRUMPMORETHANCANGETWITHTRUMP)
                    {
                        if (HaveCardInHand(CardType.Jack, trumpSuit))
                        {
                            var cardTrum = GetCardFromHand(CardType.Jack, trumpSuit);
                            turnCard = cardTrum;
                        }
                    }
                }
            }
            return PlayCard(turnCard);
        }
        private PlayerAction ClosedState(PlayerTurnContext context, Dictionary<Card, double> weightCards)
        {
            var sortedWight = weightCards.OrderBy(x => x.Value);
            var lowestWeightCard = sortedWight.First();
            var lowestCard = lowestWeightCard.Key;
            var lowestWeight = lowestWeightCard.Value;
            var hightWeightCard = sortedWight.Last();
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
                    if (GameStatistics.HowManyTrumpCardsHasTheOpponent (this.Cards, this.usedCards, context) == null)
                    {
                        var somecard = sortedWight.Last(x => x.Key.Suit != trumpSuit).Key;
                        turnCard = somecard;
                        if (somecard == null)
                        {
                            turnCard = hightCard;
                        }
                    }
                    if (GameStatistics.HowManyTrumpCardsHasTheOpponent(this.Cards, this.usedCards, context) <= GameStatistics.TrumpsInCurrentHand(this.Cards, context))
                    {
                        if (wehavetrumpbig)
                        {
                            var somecard = sortedWight.Last(x => x.Key.Suit == trumpSuit).Key;
                            turnCard = somecard;
                            if (somecard == null)
                            {
                                turnCard = hightCard;
                            }
                        }

                        if (wehavetrumpsmall)
                        {
                            var somecard = sortedWight.First(x => x.Key.Suit == trumpSuit).Key;
                            turnCard = somecard;
                            if (somecard == null)
                            {
                                turnCard = hightCard;
                            }
                        }

                    }
                    if (GameStatistics.HowManyTrumpCardsHasTheOpponent(this.Cards, this.usedCards, context) > GameStatistics.TrumpsInCurrentHand(this.Cards, context))
                    {
                        if (wehavesuitbig)
                        {
                            var somecard = sortedWight.First(x => x.Key.Suit == bigSuit).Key;
                            turnCard = somecard;
                        }
                    }
                }
            }
            return PlayCard(turnCard);
        }
    }
}
