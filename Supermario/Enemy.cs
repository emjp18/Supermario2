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
        AI_STATE m_aistate = AI_STATE.PATROL;
        
        public Enemy(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
            m_pathTimer.ResetAndStart(m_resetDelay);
            m_speed = m_random.Next((int)(m_speed * 0.75f), (int)(m_speed * 1.25f));
            
            m_endXLeft = GameManager.GetTileSize();
            m_endXRight = GameManager.GetWindowSize(true)- GameManager.GetTileSize();
            m_destination.X = m_endXLeft;
            m_destination.Y = (int)m_position.Y;
        }
        public void FindPaths()
        {
            Point left = new Point((int)m_position.X, (int)m_position.Y);
            Point right = new Point((int)m_position.X, (int)m_position.Y);
            GameManager.ModWithRes(ref left);
            GameManager.ModWithRes(ref right);
            left.X /= GameManager.GetTileSize();
            right.X /= GameManager.GetTileSize();
            left.Y /= GameManager.GetTileSize();
            right.Y /= GameManager.GetTileSize();
            while (left.X> 0)
            {
               
                left -= new Vector2(1, 0).ToPoint();

             

               
                if (GameManager.GetGrid()[(int)left.X, left.Y+1].obstacle)
                {
                    m_endXLeft = (int)left.X * GameManager.GetTileSize() + GameManager.GetTileSize();
                    break;
                }
                

               
            }
          
            while (right.X <GameManager.GetTileCount(true))
            {
               
                right += new Vector2(1, 0).ToPoint();

               
                
                
                if(GameManager.GetGrid()[(int)right.X, right.Y+1].obstacle)
                {
                    m_endXRight = (int)right.X * GameManager.GetTileSize() - GameManager.GetTileSize();
                    break;
                }
                
              
            }
           
            m_destination.X = m_endXLeft;
            m_destination.Y = (int)m_position.Y;
        }
        
        public override void Update(GameTime gametime)
        {
            if(m_destination.Y!=(int)m_position.Y&&m_grounded)
            {
                FindPaths();
                m_destination.Y = (int)m_position.Y;
            }
           
            m_pathTimer.Update(gametime.ElapsedGameTime.TotalSeconds);
            if (m_pathTimer.IsDone()&&!m_pathFound)
            {
               
                ResetPath();
                AStarSearch();
                m_pathTimer.ResetAndStart(m_resetDelay);
                
            }
            if (m_pathFound)
            {
                A_STAR_NODE node = m_path[m_pathElement];
                m_destination.X = (int)node.pos.X;
                m_destination.Y = (int)m_position.Y;
                if (m_destination== m_position.ToPoint())
                {
                    m_pathElement++;
                    if(m_pathElement== m_path.Count)
                    {
                        
                        m_pathElement = 0;
                        if (m_destination.X == m_endXLeft)
                        {
                            if (m_position.ToPoint().X <= m_destination.X)
                            {
                                m_destination = new Point(m_endXRight, (int)m_position.Y);
                            }

                        }
                        else if (m_destination.X == m_endXRight)
                        {
                            if (m_position.ToPoint().X >= m_destination.X)
                            {
                                m_destination = new Point(m_endXLeft, (int)m_position.Y);

                            }

                        }

                        m_pathFound = false;
                    }
                    

                }
                Vector2 dir = new Vector2(m_destination.X, m_destination.Y) - m_position;
                ClampDirection(ref dir, true); //Make sure the direction is either one in x or y axis.

                if (dir.X < 0)
                {
                    m_effect = SpriteEffects.None;
                }
                else
                {
                    m_effect = SpriteEffects.FlipHorizontally;
                }

                AddForce(dir * m_speed);

            }
            if (m_aistate == AI_STATE.PATROL)
            {
                if (m_destination.X == m_endXLeft)
                {
                    if (m_position.ToPoint().X <= m_destination.X)
                    {
                        m_destination = new Point(m_endXRight, (int)m_position.Y);
                    }

                }
                else if (m_destination.X == m_endXRight)
                {
                    if (m_position.ToPoint().X >= m_destination.X)
                    {
                        m_destination = new Point(m_endXLeft, (int)m_position.Y);

                    }

                }

            }
            if (MathF.Abs(ResourceManager.GetPlayer().GetCurrentPos().X- m_position.X) <= m_minPlayerDistance)
            {
                m_aistate = AI_STATE.CHASE;
                m_destination = new Point((int)ResourceManager.GetPlayer().GetCurrentPos().X, m_destination.Y);
            }
            else
            {
                m_aistate = AI_STATE.PATROL;
            }
            
           

            if(!m_grounded)
                AddForce(new Vector2(0, GameManager.GetGravity() * m_speed));

            //Point gp = GameManager.GetGridPoint(m_position, GameManager.GetRootNode());

            //if (ResourceManager.PosHasTile(gp))
            //{
            //    if(Intersects(ResourceManager.GetTile(gp)))
            //    {
            //        AddForce(KnockbackRectangle(ResourceManager.GetTile(gp)), gametime);
            //    }
            //}


            base.Update(gametime);

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
