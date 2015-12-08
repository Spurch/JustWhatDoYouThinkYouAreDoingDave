namespace HAL9000
{
    using Santase.Logic.Players;
    using System.Collections.Generic;
    using Santase.Logic.Cards;
    using Extensions;
    using HALLogic;

    public partial class HAL9000 : BasePlayer
    {
        private HelperMethods =
        new HelperMethods();
        private readonly string name = "HelperMethods";
        private ICollection<Card> possibleCardsToPlay = new List<Card>();
        private Dictionary<CardSuit, List<Card>> usedCards = new Dictionary<CardSuit, List<Card>>();
        private Card queensFor20Or40;
        private CardSuit trumpSuit;
        private CardSuit oponentCardSuit;
        private int oponentCardValue;

        public override PlayerAction GetTurn(PlayerTurnContext context)
        {
            var currentStateType = context.State.GetType().Name;
            possibleCardsToPlay = this.PlayerActionValidator.GetPossibleCardsToPlay(context, this.Cards);

            trumpSuit = context.TrumpCard.Suit;
            if (context.FirstPlayedCard != null)
            {
                oponentCardSuit = context.FirstPlayedCard.Suit;
                oponentCardValue = context.FirstPlayedCard.GetValue();
            }

            queensFor20Or40 = this.CheckForTwentyOrForty(context);

            if (this.PlayerActionValidator.IsValid(PlayerAction.ChangeTrump(), context, this.Cards))
            {
                return this.ChangeTrump(context.TrumpCard);
            }

            if (this.CloseGame(context, possibleCardsToPlay))
            {
                if (this.PlayerActionValidator.IsValid(PlayerAction.CloseGame(), context, this.Cards))
                {
                    return this.CloseGame();
                }
            }
            if (currentStateType == "StartRoundState")
            {
                var wightCards = CardsEvaluation.EvaluateWeightsInBaseState(context, this.Cards, this.usedCards);
                return FirstStepState(context, wightCards);
            }
            if (currentStateType == "MoreThanTwoCardsLeftRoundState")
            {
                var wightCards = CardsEvaluation.EvaluateWeightsInBaseState(context, possibleCardsToPlay, this.usedCards);
                return MoreThanTwoCardsState(context, wightCards);
            }
            if (currentStateType == "TwoCardsLeftRoundState")
            {
                var wightCards = CardsEvaluation.EvaluateWeightsInBaseState(context, possibleCardsToPlay, this.usedCards);
                return TwoCardLeft(context, wightCards);
            }
            if (currentStateType == "FinalRoundState" && context.CardsLeftInDeck > 0)
            {
                var wightCards = CardsEvaluation.EvaluateWeightsInClosedState(context, possibleCardsToPlay, this.usedCards, context.FirstPlayedCard);
                return ClosedState(context, wightCards);
            }
            if (currentStateType == "FinalRoundState")
            {
                var opponentHand = CardsEvaluation.GetOpponentHand(possibleCardsToPlay, usedCards);
                var wightCards = CardsEvaluation.EvaluateWeightsInClosedState(context, possibleCardsToPlay, this.usedCards, context.FirstPlayedCard);
                //var wightCards = WeightsCalculations.EvaluateWeightsInFinalState(context, possibleCardsToPlay, this.usedCards, opponentHand);
                return ClosedState(context, wightCards);
            }

            var wightCardsMore = CardsEvaluation.EvaluateWeightsInBaseState(context, possibleCardsToPlay, usedCards);
            return FirstStepState(context, wightCardsMore);
        }

        public override string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public override void EndTurn(PlayerTurnContext context)
        {
            this.UpdateUsedCardsCollections(context.FirstPlayedCard);
            if (context.SecondPlayedCard != null)
            {
                this.UpdateUsedCardsCollections(context.SecondPlayedCard);
            }
        }

        //TODO: Add stats and weight logic to the closing game logic.
        private bool CloseGame(PlayerTurnContext context, ICollection<Card> currentPossibleCardsToPlay)
        {
            if (this.PlayerActionValidator.IsValid(PlayerAction.CloseGame(), context, this.Cards))
            {
                if (CardsEvaluation.TrumpsInCurrentHand(currentPossibleCardsToPlay, context) >= 2 && (CurrentHandPoints(context) >= 50))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
