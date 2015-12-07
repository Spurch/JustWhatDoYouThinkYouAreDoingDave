namespace HAL9000.HALLogic
{
    public  class Constants
    {
        public static double GetSingleCardProbability = 0.46;
        public static double GetAnnounceOnFirstHand = 0.25;

        public static int TotalNumberOfCardsOfGivenSuit = 6;
        public static int TotalNumberOfCardsOfGivenType = 4;
        public static int TotalNumberOfSuits = 4;
        public static int TotalAmountOfDeck = 24;

        public static bool OpponentHasTrump = true;
        public static bool OpponentHasSpade = true;
        public static bool OpponentHasDiamond = true;
        public static bool OpponentHasHeart = true;
        public static bool OpponentHasClub = true;

        public static bool OpponentHasMajorTrump = false;
        public static bool OpponentHasMajorSpade = false;
        public static bool OpponentHasMajorDiamond = false;
        public static bool OpponentHasMajorHeart = false;
        public static bool OpponentHasMajorClub = false;

        public static int TooHighMinimalWeightNumber = 10;
        public static int HighValueOpponentCard = 10;
        public static int AmountOfTrumpsToAllowUsToUseThem = 2;
        public static int ValueThatWeCanGetWithTen = 10;
        public static int ValueTooLowToTakeAction = 2;
    }
}
