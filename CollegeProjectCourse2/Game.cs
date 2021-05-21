using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeProjectCourse2
{
    public class Game
    {
        int secD1;
        int maxD1;
        public int SecPlayer1HP { get; private set; }
        int secD2;
        int maxD2;
        public int SecPlayer2HP { get; private set; }
        public CardCollode collode1 = new CardCollode();
        public CardCollode collode2 = new CardCollode();
        public Card[] crads1 = new Card[4];
        public Card[] crads2 = new Card[4];

        public int mana1 = 4;
        public int mana2 = 4;

        const int manaMax = 8;

        public Game(int maxd1, int maxlive1, int maxd2, int maxlive2)
        {
            secD1 = 0;
            maxD1 = maxd1;

            secD2 = 0;
            maxD2 = maxd2;

            SecPlayer1HP = maxlive1;
            SecPlayer2HP = maxlive2;
        }

        public void Player1Damage(int i)
        {
            SecPlayer1HP -= i;
        }

        public void Player2Damage(int i)
        {
            SecPlayer2HP -= i;
        }

        public bool GameIsFinish()
        {
            return (SecPlayer1HP <= 0) || (SecPlayer2HP <= 0) || (secD1 > maxD1) || (secD2 > maxD2);
        }

        public bool UseCard1(Card card)
        {
            if (card.Cost <= mana1 && collode1.cards.Count < 3)
            {
                collode1.AddCard(card);
                mana1 -= card.Cost;
                return true;
            }
            else
                return false;
        }

        public bool UseCard2(Card card)
        {
            if (card.Cost <= mana2 && collode2.cards.Count < 3)
            {
                collode2.AddCard(card);
                mana2 -= card.Cost;
                return true;
            }
            else
                return false;
        }

        public void Step1()
        {
            SecPlayer2HP -= collode2.ApplyDamage(collode1);
            ++secD1;
            mana1 += 2;
            if (mana1 > manaMax)
                mana1 = manaMax;
        }

        public void Step2()
        {
            SecPlayer1HP -= collode1.ApplyDamage(collode2);
            ++secD2;
            mana2 += 2;
            if (mana2 > manaMax)
                mana2 = manaMax;
        }
    }
}
