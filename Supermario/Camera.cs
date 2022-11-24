using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace Supermario
{
    internal class Camera
    {
        private Matrix m_transform;  
        private Vector2 m_position;
        private Viewport m_view;
        public Matrix Transform
        {
            get { return m_transform; }
        }
        public Camera(Viewport view)
        {
            m_view = view;
        }
        public void SetPosition(Vector2 position)
        {
            m_position = position;
            float offset = GameManager.GetTileSize()*2;
            m_position.Y -= m_view.Height- offset;
            if (GameManager.IsWithinWindowBounds(new Rectangle(m_view.X+(int)position.X,m_view.Y+(int)m_position.Y
                ,m_view.Width,
                m_view.Height)))
                m_transform = Matrix.CreateTranslation(-m_position.X, -m_position.Y, 0);
            
                   
        }
    }
}
