using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
        public void Knocback(GameObject other, GameTime gametime)
        {
            float DistanceNow = Vector2.Distance(m_position
            , other.GetCurrentPos());
            float DistanceNext = Vector2.Distance(
            m_position + m_velocity / 100
            , other.GetCurrentPos() + other.GetVelocity() / 100);

            if (DistanceNext< DistanceNow)
            {
                Vector2 collisionNormal = Vector2.Normalize(m_position
               - other.GetCurrentPos());
                ////split a1's velocity into parallel and right-angled components
                Vector2 u1Orthogonal = Vector2.Dot(m_velocity, collisionNormal)
                * collisionNormal;
                Vector2 u1Parallel = m_velocity - u1Orthogonal;
                Vector2 u2Orthogonal = Vector2.Dot(other.GetVelocity(), collisionNormal)
                * collisionNormal;
                Vector2 u2Parallel = other.GetVelocity() - u2Orthogonal;
                float elasticity = 1.1f;
                //Update the right-angled components of the asteroids' hastigheterna
                // and add back the parallel components 
                AddForce(((u1Orthogonal + u2Orthogonal
                + elasticity * (u2Orthogonal - u1Orthogonal)) / 2 + u1Parallel)
                , gametime);


                if (other is DynamicObject)
                {
                    other.AddForce(((u1Orthogonal + u2Orthogonal
                + elasticity * (u1Orthogonal - u2Orthogonal)) / 2 + u2Parallel)
                , gametime);

                }
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
           Rectangle textureBoundsA =  new Rectangle(0,0,
            m_frameSize.X,
            m_frameSize.Y);
            Rectangle textureBoundsB = new Rectangle(0, 0,
            other.GetFrameSize().X,
            other.GetFrameSize().Y);
            Rectangle hitBox = GetBounds();
            Rectangle otherHitBox = other.GetBounds();
            Color[] dataA = new Color[m_frameSize.X * m_frameSize.Y];
            m_texture.GetData(0, textureBoundsA, dataA, 0, dataA.Length);
            Color[] dataB = new Color[other.GetFrameSize().X * other.GetFrameSize().Y];
            other.GetTexture().GetData(0, textureBoundsB, dataB, 0, dataB.Length);
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
        protected void ClampDirection(ref Vector2 dir, bool onlyUseX=false)
        {
            if (MathF.Abs(dir.X) > MathF.Abs(dir.Y))
            {
                dir.Y = 0;
                if (dir.X > 0)
                {
                    dir.X = 1;
                }
                else
                {
                    dir.X = -1;
                }
            }
            else if (MathF.Abs(dir.X) == MathF.Abs(dir.Y))
            {
                dir.Y = 0;
                dir.X = 0;
            }
            else
            {
                dir.X = 0;
                if (dir.Y > 0)
                {
                    dir.Y = 1;
                }
                else
                {
                    dir.Y = -1;
                }
            }
            if(onlyUseX)
            {
                dir.Y = 0;
            }
        }
    }
}
