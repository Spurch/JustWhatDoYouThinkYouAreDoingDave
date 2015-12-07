namespace HAL9000
{
    using Santase.Logic.Players;
    using System.Collections.Generic;
    using Santase.Logic;
    using Santase.Logic.Cards;
    using Santase.Logic.RoundStates;
    using Extensions;

    public partial class HAL9000: BasePlayer
    {
        private readonly string name = "HAL9000";
        private ICollection<Card> possibleCardsToPlay = new List<Card>();
        private Card queensFor20Or40;
        private CardSuit trumpSuit;
        private CardSuit oponentCardSuit;
        private int oponentCardValue;

        public override PlayerAction GetTurn(PlayerTurnContext context)
        {
            var typeState = context.State.GetType();
            possibleCardsToPlay = this.PlayerActionValidator.GetPossibleCardsToPlay(context, this.Cards);

            trumpSuit = context.TrumpCard.Suit;
            oponentCardSuit = context.FirstPlayedCard.Suit;
            oponentCardValue = context.FirstPlayedCard.GetValue();

            queensFor20Or40 = this.CheckForTwentyOrForty(context);

            if (this.PlayerActionValidator.IsValid(PlayerAction.ChangeTrump(), context, this.Cards))
            {
                return this.ChangeTrump(context.TrumpCard);
            }

            if (this.CloseGame(context, possibleCardsToPlay))
            {
                return this.CloseGame();
            }
            if (typeState == typeof(StartRoundState))
            {
                var wightCards = WeightsCalculations.EvaluateWeightsInBaseState(context, this.Cards, this.usedCards);
                return FirstStepState(context, wightCards);
            }
            if (typeState == typeof(MoreThanTwoCardsLeftRoundState))
            {
                var wightCards = WeightsCalculations.EvaluateWeightsInBaseState(context, this.Cards, this.usedCards);
                return MoreThanTwoCardsState(context, wightCards);
            }
            if (typeState == typeof(TwoCardsLeftRoundState))
            {
                var wightCards = WeightsCalculations.EvaluateWeightsInBaseState(context, this.Cards, this.usedCards);
                return TwoCardLeft(context, wightCards);
            }
            if (typeState == typeof (FinalRoundState) && context.CardsLeftInDeck>0)
            {
                var wightCards = WeightsCalculations.EvaluateWeightsInClosedState(context, this.Cards, this.usedCards, context.FirstPlayedCard);
                return ClosedState(context, wightCards);
            }
            if (typeState == typeof(FinalRoundState))
            {
                var wightCards = WeightsCalculations.EvaluateWeightsInClosedState(context, this.Cards, this.usedCards, context.FirstPlayedCard);
                return ClosedState(context, wightCards);
            }

            var wightCardsMore = WeightsCalculations.EvaluateWeightsInBaseState(context, this.Cards, this.usedCards);
            return MoreThanTwoCardsState(context, wightCardsMore);
        }

        public override string Name
        {
            get { return this.name; }
        }

        //TODO: Add stats and weight logic to the closing game logic.
        private bool CloseGame(PlayerTurnContext context, ICollection<Card> currentPossibleCardsToPlay)
        {
            if (this.PlayerActionValidator.IsValid(PlayerAction.CloseGame(), context, this.Cards))
            {
                if (WeightsCalculations.TrumpsInCurrentHand(currentPossibleCardsToPlay, context) >= 2 && (CurrentHandPoints(context) >=50))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void EndTurn(PlayerTurnContext context)
        {
            this.UpdateUsedCardsCollections(context.FirstPlayedCard);
            this.UpdateUsedCardsCollections(context.SecondPlayedCard);
        }
    }
}
