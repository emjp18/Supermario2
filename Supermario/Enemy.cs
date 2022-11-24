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
        float m_minPlayerDistance = 200;
        Random m_random = new Random();
        Point m_gridposition = new Point();
        Point m_destination = new Point();
        const double m_resetDelay = 2.0;
        Timer m_pathTimer = new Timer();
        private A_STAR_NODEComparer m_comp = new A_STAR_NODEComparer();
        private bool m_pathFound = false;
        private A_STAR_NODE m_start;
        private A_STAR_NODE m_end;
        private int m_pathElement = 0;
        private List<A_STAR_NODE> m_open = new List<A_STAR_NODE>();
        private List<A_STAR_NODE> m_closed = new List<A_STAR_NODE>();
        private List<A_STAR_NODE> m_path = new List<A_STAR_NODE>();
        public Enemy(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
            m_pathTimer.ResetAndStart(m_resetDelay);
        }
        int GetDistance(A_STAR_NODE nodeA, A_STAR_NODE nodeB)
        {
            return (int)MathF.Abs(nodeA.pos.X - nodeB.pos.X) + (int)MathF.Abs(nodeA.pos.Y - nodeB.pos.Y);
        }
        void AStarSearch()
        {
            m_gridposition.X = (int)m_position.X;
            m_gridposition.Y = (int)m_position.Y;

            GameManager.ModWithRes(ref m_gridposition);
            m_gridposition.X /= GameManager.GetTileCount(true);
            m_gridposition.Y /= GameManager.GetTileCount(false);
            GameManager.ModWithRes(ref m_destination);
            m_destination.X /= GameManager.GetTileCount(true);
            m_destination.Y /= GameManager.GetTileCount(false);
            m_start = GameManager.GetGrid()[m_gridposition.X, m_gridposition.Y];
            m_end = GameManager.GetGrid()[m_destination.X, m_destination.Y];


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
                if (current.pos.X == m_end.pos.X && current.pos.Y == m_end.pos.Y)     //if current is end, save the correct path and end loop
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
            private void ResetPath()
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
            if (m_pathTimer.IsDone())
            {
                ResetPath();
                AStarSearch();
                m_pathTimer.ResetAndStart(m_resetDelay);
            }
            if (m_pathFound)
            {
                if(m_destination==new Point((int)m_position.X, (int)m_position.Y))
                {
                    m_destination = new Point(m_random.Next(GameManager.GetRes(true)), (int)m_position.Y);

                }
               
                A_STAR_NODE node = m_path[m_pathElement];
                m_destination.X = (int)node.pos.X;
                m_destination.Y = (int)node.pos.Y; 
            }
            if (Vector2.Distance(ResourceManager.GetPlayer().GetCurrentPos(), m_position) <= m_minPlayerDistance)
            {
                m_destination = new Point((int)ResourceManager.GetPlayer().GetCurrentPos().X, (int)ResourceManager.GetPlayer().GetCurrentPos().Y);
            }
            else
            {
                if (m_pathFound)
                    if(m_destination.X <= GameManager.GetRes(true) * 0.5f)
                        m_destination = new Point(GameManager.GetRes(true), (int)m_position.Y);
                    else
                        m_destination = new Point(0, (int)m_position.Y);
            }
            m_direction = new Vector2(m_destination.X, m_destination.Y) - m_position;
            ClampDirection(ref m_direction, true); //Make sure the direction is either one in x or y axis.
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
