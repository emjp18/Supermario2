using Microsoft.Xna.Framework;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
namespace Supermario
{
    internal class Player : DynamicObject
    {
        public Player(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
            m_isEditable = false;
            m_canmove = true;
            
        }
        public override void Update(GameTime gametime)
        {
            m_direction = Vector2.Zero;
            if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Left))
            {
                m_direction = Vector2.Zero;
                m_direction.X = -1;
                m_effect = SpriteEffects.None ;
            }

            if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Right))
            {
                m_direction = Vector2.Zero;
                m_direction.X = 1;
                m_effect = SpriteEffects.FlipHorizontally;
            }

            if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
            {
                m_direction = Vector2.Zero;
                m_direction.Y = -1;
                m_effect = SpriteEffects.None;
            }

            if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Down))
            {
                m_direction = Vector2.Zero;
                m_direction.Y = 1;
                m_effect = SpriteEffects.None;
            }
           
            base.Update(gametime);
        }
    }
}
