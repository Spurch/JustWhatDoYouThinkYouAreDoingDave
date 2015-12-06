

using System.Collections.Generic;
using System.Linq;
using HAL9000.HALLogic;
using Santase.Logic.Cards;

namespace HAL9000
{
    using System;
    using Santase.Logic.Players;

    public partial class HAL9000
    {
        private PlayerAction FirstStepState(PlayerTurnContext context, Dictionary<Card, decimal> weightCards, List<Card> checkForTwentyOrForty)
        {
            var lowestWeightCard = weightCards.OrderByDescending(x => x.Value).First();
            var card = lowestWeightCard.Key;
            var weight = lowestWeightCard.Value;
            Card turnCard = card;
            if (!context.IsFirstPlayerTurn)
            {
                if (Have20Or40(checkForTwentyOrForty))
                {

                }
                if (weight > Constants.TOOHIGHTMINWEIGHTNUMBER)
                {
                    
                }
            }
            
            return PlayCard(turnCard);
        }
    }
}
