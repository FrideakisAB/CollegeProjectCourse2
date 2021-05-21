using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeProjectCourse2
{
    public class CardCollode
    {
        public List<Card> cards = new List<Card>();

        public void AddCard(Card card)
        {
            cards.Add(card);
        }

        public int ApplyDamage(CardCollode collode)
        {
            int playerDamage = 0;

            for (int i = 0; i < collode.cards.Count; ++i)
            {
                for (int j = 0; j < cards.Count; ++j)
                {
                    if (!cards[j].Death())
                    {
                        collode.cards[i].Attack(cards[j]);

                        if (collode.cards[i].Death())
                            break;
                    }
                }

                if (!collode.cards[i].Death())
                    playerDamage += collode.cards[i].Damage;
            }

            removeDeath();
            collode.removeDeath();

            return playerDamage;
        }

        void removeDeath()
        {
            bool nextFind = true;
            while (nextFind)
            {
                nextFind = false;
                for (int i = 0; i < cards.Count; ++i)
                {
                    if (cards[i].Death())
                    {
                        cards.Remove(cards[i]);
                        nextFind = true;
                        break;
                    }
                }
            }
        }
    }
}
