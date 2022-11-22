using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Supermario
{
    internal class DynamicObject : GameObject
    {
        protected float m_gravity = 9.8f;
        public DynamicObject(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
          
        }
        public override void Update(GameTime gametime)
        {
            base.Update(gametime);
        }
        public bool Intersects(GameObject other)
        {
            return GetBounds().Intersects(other.GetBounds());
            
        }
        public void Knocback(GameObject other)
        {
            int e = 1;
            Vector2 WA = m_mass * m_direction + other.GetMass() * other.GetDirection()
                + e * other.GetMass() * (other.GetDirection() - m_direction) / (m_mass + other.GetMass());

            AddForce(WA);
            if (other is DynamicObject)
            {
                Vector2 WB = m_mass * m_direction + other.GetMass() * other.GetDirection()
               + e * m_mass * (m_direction - other.GetDirection()) / (m_mass + other.GetMass());
                other.AddForce(WB);
            }
        }
        public bool CircleIntersects(GameObject other)
        {
            
            Vector2 directionV = m_position - other.GetCurrentPos();
            float d = MathF.Sqrt(MathF.Pow(directionV.X, 2) + MathF.Pow(directionV.Y, 2));

            if ((m_r + other.GetRadius()) >= d) 
            {
                Vector2 dirV = m_direction - other.GetDirection();
                if (Vector2.Dot(dirV, directionV) > 0)
                {
                    return false;
                }
                else
                {
                    

                    return true;
                }
            }
            else
            {
                return false;
            }
        }
        public bool PixelIntersects(GameObject other)
        {
            Rectangle hitBox = GetBounds();
            Rectangle otherHitBox = other.GetBounds();
            Color[] dataA = new Color[m_frameSize.X * m_frameSize.Y];
            m_texture.GetData(dataA);
            Color[] dataB = new Color[other.GetFrameSize().X * other.GetFrameSize().Y];
            other.GetTexture().GetData(dataB);
            int top = Math.Max(hitBox.Top, otherHitBox.Top);
            int bottom = Math.Min(hitBox.Bottom, otherHitBox.Bottom);
            int left = Math.Max(hitBox.Left, otherHitBox.Left);
            int right = Math.Min(hitBox.Right, otherHitBox.Right);

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    Color colorA = dataA[(x - hitBox.Left) + (y - hitBox.Top) * hitBox.Width];
                    Color colorB = dataB[(x - otherHitBox.Left) + (y - otherHitBox.Top) * otherHitBox.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
