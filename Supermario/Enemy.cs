using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermario
{
    internal class Enemy : DynamicObject
    {
        public Enemy(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
        
        }
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
        public override void UpdateAnimation(GameTime gametime)
        {
            //base.UpdateAnimation(gametime);
        }

    }
    
}
