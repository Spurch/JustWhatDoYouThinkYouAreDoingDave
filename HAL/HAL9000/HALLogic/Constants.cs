using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static int TOOHIGHTMINWEIGHTNUMBER = 10;
        public static int NIGHVALUETOOPONENTSCARD = 10;
        public static int COUNTTRUMPMORETHANCANGETWITHTRUMP = 2;
        public static int LESSVALUETHATWECANGETWITHTEN = 10;
        public static int TOOLOWVALUEFORWEMAKEANYTHINK = 2;
    }
}
