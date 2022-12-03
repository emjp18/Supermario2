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
        public Vector2 Position
        {
            get { return m_position; }
        }
        public Camera(Viewport view)
        {
            m_view = view;
        }
        public void SetPosition(Vector2 position)
        {
            m_position = position;
            float offsety =GameManager.GetWindowSize(false)-GameManager.GetPlayerStart().Y;
            m_position.Y -= m_view.Height - offsety;

            m_position.X -= m_view.Width * 0.5f;
            GameManager.ClampInWindow(ref m_position, new Rectangle((int)m_position.X, (int)m_position.Y
                , m_view.Width,
                m_view.Height));

            m_transform = Matrix.CreateTranslation(-m_position.X, -m_position.Y, 0);



        }
    }
}
