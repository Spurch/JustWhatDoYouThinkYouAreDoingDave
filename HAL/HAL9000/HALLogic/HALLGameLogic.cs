

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography.X509Certificates;
using HAL9000.HALLogic;
using Santase.Logic.Cards;

namespace HAL9000
{
    using System;
    using Santase.Logic.Players;

    public partial class HAL9000
    {
        private PlayerAction FirstStepState(PlayerTurnContext context, Dictionary<Card, decimal> weightCards)
        {
            var lowestWeightCard = weightCards.OrderByDescending(x => x.Value).First();
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
        private PlayerAction MoreThanTwoCardsState(PlayerTurnContext context, Dictionary<Card, decimal> weightCards)
        {
            var lowestWeightCard = weightCards.OrderByDescending(x => x.Value).First();
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
                        if (TrumpsInCurrentHand(context) >= Constants.COUNTTRUMPMORETHANCANGETWITHTRUMP)
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
                if (HaveLonely10FromSuit(oponentCardSuit) && oponentCardValue < Constants.LESSVALUETHATWECANGETWITHTEN && oponentCardSuit != trumpSuit)
                {
                    turnCard = GetCardFromHand(CardType.Ten, oponentCardSuit);
                }
            }
            return PlayCard(turnCard);
        }
    }
}
