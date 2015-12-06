namespace HAL9000
{
    using Santase.Logic.Players;
    using System.Collections.Generic;
    using Santase.Logic;
    using Santase.Logic.Cards;

    public partial class HAL9000: BasePlayer
    {
        private readonly string name = "HAL9000";


        public override PlayerAction GetTurn(PlayerTurnContext context)
        {
            if (this.PlayerActionValidator.IsValid(PlayerAction.ChangeTrump(), context, this.Cards))
            {
                return this.ChangeTrump(context.TrumpCard);
            }

            if (this.CloseGame(context))
            {
                return this.CloseGame();
            }

            return null;
        }

        public override string Name
        {
            get { return this.name; }
        }

        //TODO: Create closing decision making logic.
        private bool CloseGame(PlayerTurnContext context)
        {
            
            return true;
        }
    }
}
