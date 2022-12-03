using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;

namespace Supermario
{
    internal class DynamicObject : GameObject
    {
        protected bool m_isjumping = false;
        protected int m_endXLeft;
        protected int m_endXRight;
        protected float m_minPlayerDistance = 200;
        protected Random m_random = new Random();
        protected Point m_gridposition = new Point();
        protected new Point m_destination = new Point();
        protected new Point m_gridDestination = new Point();
        protected const double m_resetDelay = 2.0;
        protected Timer m_pathTimer = new Timer();
        protected A_STAR_NODEComparer m_comp = new A_STAR_NODEComparer();
        protected bool m_pathFound = false;
        protected A_STAR_NODE m_start;
        protected A_STAR_NODE m_end;
        protected int m_pathElement = 0;
        protected List<A_STAR_NODE> m_open = new List<A_STAR_NODE>();
        protected List<A_STAR_NODE> m_closed = new List<A_STAR_NODE>();
        protected List<A_STAR_NODE> m_path = new List<A_STAR_NODE>();
        public void SetIsJumping(bool jumping) { m_isjumping = jumping; }
        public bool GetIsJumping() { return m_isjumping; }
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
        public bool IsGrounded(Rectangle bounds, GameObject other)
        {
            
            Rectangle otherbounds = other.GetBounds();
            int up = Math.Abs(otherbounds.Top - bounds.Bottom);
            int down = Math.Abs(otherbounds.Bottom - bounds.Top);

            return up < down;
        }
        
        public void Knocback(GameObject other)
        {
            float DistanceNow = Vector2.Distance(m_position
            , other.GetCurrentPos());
            float DistanceNext = Vector2.Distance(
            m_position + m_velocity / 100
            , other.GetCurrentPos() + other.GetVelocity() / 100);

            if (DistanceNext< DistanceNow)
            {
                // Vector2 collisionNormal = Vector2.Normalize(m_position
                //- other.GetCurrentPos());
                // ////split a1's velocity into parallel and right-angled components
                // Vector2 u1Orthogonal = Vector2.Dot(m_velocity, collisionNormal)
                // * collisionNormal;
                // Vector2 u1Parallel = m_velocity - u1Orthogonal;
                // Vector2 u2Orthogonal = Vector2.Dot(other.GetVelocity(), collisionNormal)
                // * collisionNormal;
                // Vector2 u2Parallel = other.GetVelocity() - u2Orthogonal;
                // float elasticity = 1.1f;
                // //Update the right-angled components of the asteroids' hastigheterna
                // // and add back the parallel components 

                float e = 1.1f;
                Vector2 WA = (m_mass * m_velocity + other.GetMass() * other.GetVelocity() + e * other.GetMass()
                        * ((other.GetVelocity() - m_velocity))) / (m_mass + other.GetMass());

                Vector2 WB = (m_mass * m_velocity + other.GetMass() * other.GetVelocity() + e
                    * (m_mass * (m_velocity - other.GetVelocity()))) / (m_mass + other.GetMass());

                AddForce(WA*50);
                if (other is DynamicObject)
                    other.AddForce(WB * 50);


                //AddForce(((u1Orthogonal + u2Orthogonal
                //+ elasticity * (u2Orthogonal - u1Orthogonal)) / 2 + u1Parallel)
                //);


                //if (other is DynamicObject)
                //{
                //    other.AddForce(((u1Orthogonal + u2Orthogonal
                //+ elasticity * (u1Orthogonal - u2Orthogonal)) / 2 + u2Parallel)
                //);

                //}
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
        protected int GetDistance(A_STAR_NODE nodeA, A_STAR_NODE nodeB)
        {
            return (int)MathF.Abs(nodeA.pos.X - nodeB.pos.X) + (int)MathF.Abs(nodeA.pos.Y - nodeB.pos.Y);
        }
        protected void AStarSearch()
        {
            m_gridposition.X = (int)m_position.X;
            m_gridposition.Y = (int)m_position.Y;
            
            GameManager.ModWithRes(ref m_gridposition);
            m_gridposition.X /= GameManager.GetTileSize();
            m_gridposition.Y /= GameManager.GetTileSize();
            m_gridDestination = m_destination;
            GameManager.ModWithRes(ref m_gridDestination);
            m_gridDestination.X /= GameManager.GetTileSize();
            m_gridDestination.Y /= GameManager.GetTileSize();
            m_start = GameManager.GetGrid()[m_gridposition.X, m_gridposition.Y];
            m_end = GameManager.GetGrid()[m_gridDestination.X, m_gridDestination.Y];


            


            m_open.Clear();
            m_closed.Clear();
            m_open.Add(m_start);

            GameManager.GetGrid()[(int)m_start.gridpos.X, (int)m_start.gridpos.Y].openSet = true;
            while (m_open.Count > 0 && !m_pathFound)
            {
                if (m_open.Count > 1)
                {
                    m_open.Sort(m_comp);

                }

                A_STAR_NODE current = m_open[0];
                if (current.gridpos.X == m_end.gridpos.X && current.gridpos.Y == m_end.gridpos.Y)     //if current is end, save the correct path and end loop
                {
                    m_pathFound = true;
                    A_STAR_NODE temp = current;
                    GameManager.GetGrid()[(int)temp.gridpos.X, (int)temp.gridpos.Y].correctPath = true;
                    m_path.Add(GameManager.GetGrid()[(int)temp.gridpos.X, (int)temp.gridpos.Y]);
                    while (temp.previous[0].isActive)
                    {
                        temp = temp.previous[0];
                        GameManager.GetGrid()[(int)temp.gridpos.X, (int)temp.gridpos.Y].correctPath = true;
                        m_path.Add(GameManager.GetGrid()[(int)temp.gridpos.X, (int)temp.gridpos.Y]);
                    }
                    m_path.Reverse();
                }
                else
                {
                    m_open.Remove(m_open.First());

                    GameManager.GetGrid()[(int)current.gridpos.X, (int)current.gridpos.Y].openSet = false;                //Add best choice to closedSet
                    m_closed.Add(current);
                    GameManager.GetGrid()[(int)current.gridpos.X, (int)current.gridpos.Y].closedSet = true;

                    for (int i = 0; i < current.neighbours.Count; i++)                             //Check all neighbors to current
                    {
                        if ((!m_closed.Any() || !m_closed.Contains(current.neighbours[i]))
                            && !current.neighbours[i].obstacle)        //if currents neighbor isn't in closedSet, update costs
                        {
                            float tempG = current.g + 1;

                            bool newPath = false;
                            
                            if (m_open.Contains(current.neighbours[i]))
                            {
                                if (tempG < current.neighbours[i].g)
                                {
                                    A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                                    a_STAR_NODE.g = tempG;
                                    current.neighbours[i] = a_STAR_NODE;
                                    newPath = true;
                                }
                            }
                            else
                            {
                                A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                                a_STAR_NODE.g = tempG;
                                current.neighbours[i] = a_STAR_NODE;
                                m_open.Add(current.neighbours[i]);
                                GameManager.GetGrid()[(int)current.neighbours[i].gridpos.X,
                                    (int)current.neighbours[i].gridpos.Y].openSet = true;
                                newPath = true;
                            }
                            if (newPath)
                            {
                                A_STAR_NODE a_STAR_NODE = current.neighbours[i];
                                a_STAR_NODE.h = GetDistance(current.neighbours[i], m_end);
                                a_STAR_NODE.f = current.neighbours[i].g + current.neighbours[i].h;
                                a_STAR_NODE.previous[0] = current;
                                current.neighbours[i] = a_STAR_NODE;

                            }
                        }
                    }
                }
            }


        }
        protected void ResetPath()
        {
            m_pathElement = 0;
            m_pathFound = false;
            m_path.Clear();
            for (int x = 0; x < GameManager.GetTileCount(true); x++)
            {
                for (int y = 0; y < GameManager.GetTileCount(false); y++)
                {

                    GameManager.GetGrid()[x, y].g = 0;
                    GameManager.GetGrid()[x, y].h = 0;
                    GameManager.GetGrid()[x, y].f = 0;
                    GameManager.GetGrid()[x, y].openSet = false;
                    GameManager.GetGrid()[x, y].closedSet = false;
                    GameManager.GetGrid()[x, y].correctPath = false;
                    GameManager.GetGrid()[x, y].isActive = true;
                    GameManager.GetGrid()[x, y].previous[0].isActive = false;
                    for (int z = 0; z < GameManager.GetGrid()[x, y].neighbours.Count; z++)
                    {
                        A_STAR_NODE temp = GameManager.GetGrid()[x, y].neighbours[z];
                        temp.g = 0;
                        temp.h = 0;
                        temp.f = 0;
                        temp.openSet = false;
                        temp.closedSet = false;
                        temp.correctPath = false;
                        temp.isActive = true;
                        temp.previous[0].isActive = false;
                        GameManager.GetGrid()[x, y].neighbours[z] = temp;
                    }

                }
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
            if (MathF.Abs(dir.X) > MathF.Abs(dir.Y)||onlyUseX)
            {
                dir.Y = 0;
                if (dir.X > 0)
                {
                    dir.X = 1;
                }
                else if(dir.X==0)
                {
                    dir.X = 0;
                }
                else if(dir.X<0)
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
                else if (dir.Y==0)
                {
                    dir.Y = 0;
                }
                else if(dir.Y<0)
                {
                    dir.Y = -1;
                }
            }
           
        }
    }
}
