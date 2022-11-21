using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Supermario;
using SharpDX.Direct2D1.Effects;

namespace SuperMario
{
    internal class Button : GameObject
    {
        string m_text;
        bool m_pressed = false;
        float m_increament = 1.3f;
        Color m_oldColor;
       
        public Button(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
        }
       
        public override void Update(GameTime gameTime)
        {

           


            if (GetBounds().Contains(KeyMouseReader.mouseState.Position))
            {
                if (KeyMouseReader.LeftClick())
                {
                    m_pressed = true;
                }
                else
                {
                    m_pressed = false;
                }
                if(m_oldColor!=m_color)
                {
                    m_color *= m_increament;
                    m_oldColor = m_color;
                }
                
                

            }
            else
            {
                if (m_oldColor == m_color)
                {
                    m_color *= 1.0f / m_increament;
                    m_color.A = 255;
                    

                }
               
                

            }
           
        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            m_frameSize.X = (int)font.MeasureString(m_text).X;

            m_frameSize.Y = (int)font.MeasureString(m_text).Y;
            spriteBatch.Draw(m_texture, m_position, new Rectangle((int)m_position.X,
           (int)m_position.Y,
            m_frameSize.X, m_frameSize.Y), m_color);
            spriteBatch.DrawString(font, m_text,m_position ,Color.Black);

           

        }
        public bool GetPressed() { return m_pressed; }
        public void SetPressed(bool value) { m_pressed = value; }
        public void SetText(string text) { m_text = text; }
    }
}
