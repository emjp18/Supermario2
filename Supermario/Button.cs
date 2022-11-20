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
        public Button(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
        }
        public override void Update(GameTime gameTime)
        {
            if(KeyMouseReader.LeftClick())
            {
                Color color = m_color;
                if (GetBounds().Contains(KeyMouseReader.mouseState.Position))
                {
                    m_pressed = true;
                    m_color *= 1.3f;
                }
                else
                {
                    m_pressed = false;
                    m_color = color;
                }
            }
            
            

        }
        public void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.Draw(m_texture, m_position, new Rectangle(m_currentFrameX * m_frameSize.X,
            m_currentFrameY * m_frameSize.Y,
            (int)font.MeasureString(m_text).X, (int)font.MeasureString(m_text).Y), m_color);
            spriteBatch.DrawString(font, m_text,m_position ,Color.Black);


        }
        public bool GetPressed() { return m_pressed; }
        public void SetPressed(bool value) { m_pressed = value; }
    }
}
