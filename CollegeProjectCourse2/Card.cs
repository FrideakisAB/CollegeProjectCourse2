using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollegeProjectCourse2
{
    public class Card
    {
        public int Id { get; private set; }
        public int HP { get; private set; }
        public int Damage { get; private set; }
        public int Cost { get; private set; }

        public Card(int cId, int cHP, int cDamage, int cCost)
        {
            Id = cId;
            HP = cHP;
            Damage = cDamage;
            Cost = cCost;
        }

        public void Attack(Card card)
        {
            HP -= card.Damage;
            card.HP -= Damage;
        }

        public bool Death()
        {
            return (HP <= 0);
        }

        public Card Clone()
        {
            return new Card(Id, HP, Damage, Cost);
        }
    }
}
