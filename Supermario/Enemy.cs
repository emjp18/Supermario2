﻿using Microsoft.Xna.Framework;
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
        
        public Enemy(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
            m_pathTimer.ResetAndStart(m_resetDelay);
            m_speed = m_random.Next((int)(m_speed * 0.75f), (int)(m_speed * 1.25f));
        }
        
        private bool FindDestination(ref Point current, Point old) //recursively change Y so that there is no obstacle
        {
            
            if (current.Y>old.Y+GameManager.GetTileSize()|| current.Y < old.Y - GameManager.GetTileSize())
            {
                return false;
            }
            else
            {
                if (ResourceManager.PosHasTile(current))
                {
                    old = new Point(current.X, current.Y);
                    current.Y += GameManager.GetTileSize();
                    FindDestination(ref current, old);
                    current.Y -= GameManager.GetTileSize() * 2;
                    FindDestination(ref current, old);
                }
                return true;
            }
           
            
        }
        public override void Update(GameTime gametime)
        {
            m_pathTimer.Update(gametime.ElapsedGameTime.TotalSeconds);
            if (m_pathTimer.IsDone()&&!m_pathFound)
            {
                if (m_destination.X <= GameManager.GetWindowSize(true) * 0.5f)
                    m_destination = new Point(GameManager.GetWindowSize(true), (int)m_position.Y);
                else
                    m_destination = new Point(0, (int)m_position.Y);
                ResetPath();
                AStarSearch();
                m_pathTimer.ResetAndStart(m_resetDelay);
            }
            if (m_pathFound)
            {
                A_STAR_NODE node = m_path[m_pathElement];
                m_destination.X = (int)node.pos.X;
                m_destination.Y = (int)node.pos.Y;
                if (m_destination==new Point((int)m_position.X, (int)m_position.Y))
                {
                    m_pathElement++;
                    if(m_pathElement== m_path.Count)
                    {
                        
                        m_pathElement = 0;
                        if (m_destination.X <= GameManager.GetWindowSize(true) * 0.5f)
                            m_destination = new Point(GameManager.GetWindowSize(true), (int)m_position.Y);
                        else
                            m_destination = new Point(0, (int)m_position.Y);

                        m_pathFound = false;
                    }
                    

                }
               
                
            }
            if (Vector2.Distance(ResourceManager.GetPlayer().GetCurrentPos(), m_position) <= m_minPlayerDistance)
            {
                m_destination = new Point((int)ResourceManager.GetPlayer().GetCurrentPos().X, (int)ResourceManager.GetPlayer().GetCurrentPos().Y);
            }
           
            m_direction = new Vector2(m_destination.X, m_destination.Y) - m_position;
            ClampDirection(ref m_direction, true); //Make sure the direction is either one in x or y axis.

            if(m_direction.X < 0)
            {
                m_effect = SpriteEffects.FlipHorizontally;
            }
            else
            {
                m_effect = SpriteEffects.None;
            }

            AddForce(m_direction * m_speed * (float)gametime.ElapsedGameTime.TotalSeconds, gametime);
            AddForce(new Vector2(0, m_gravity * m_speed * 0.5f * (float)gametime.ElapsedGameTime.TotalSeconds), gametime);
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
