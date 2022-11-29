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
        Camera m_camera;
        float m_jumpforce = 2;
        public Player(OBJECT_CONSTRUCTION_DATA constructiondata, Viewport viewport) : base(constructiondata)
        {
            m_isEditable = false;
            m_canmove = true;
            m_camera = new Camera(viewport);
            m_camera.SetPosition(m_position);
        }
        public Camera GetCamera() { return m_camera; }
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
            

            if (m_grounded)
            {
                
                m_camera.SetPosition(m_position);
                if (KeyMouseReader.KeyPressed(Keys.Space))
                {
                    SwapDirection();
                     m_direction.Y = 1;
                      m_direction.Normalize();
                    AddForce(m_direction*-GameManager.GetGravity() * 2*m_speed*m_jumpforce);
                    m_isjumping = true;
                }
                AddForce(m_direction * m_speed);
            }
            else
            {
                if(m_isjumping)
                    AddForce(new Vector2(0, GameManager.GetGravity() * m_speed*0.5f));
                else
                    AddForce(new Vector2(0, GameManager.GetGravity() * m_speed));
            }

            
            





            base.Update(gametime);
        }
    }
}
