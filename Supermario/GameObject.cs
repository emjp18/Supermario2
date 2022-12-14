using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using SuperMario;
using static System.Net.Mime.MediaTypeNames;

namespace Supermario
{
     public abstract class GameObject
    {
        protected float m_mingrounddistance =13.0f;
        protected Point m_groundpos = new Point(-1, -1);
        protected bool m_grounded = false;
        protected SPRITE_TYPE m_type;
        protected Texture2D m_texture;
        protected Vector2 m_F = Vector2.Zero;
        protected bool m_isEditable = true;
        protected bool m_canmove;
        protected SpriteEffects m_effect = SpriteEffects.None;
        protected Vector2 m_prevPos;
        protected Color m_color = Color.White;
        protected Vector2 m_tempDir = Vector2.Zero;
        protected bool m_useCollision = true;
        protected bool m_isColliding = false;
        protected bool m_hitWall = false;
        protected bool m_moving = false;
        protected Vector2 m_destination;
        protected Vector2 m_direction = Vector2.Zero;
        protected Vector2 m_velocity = Vector2.Zero;
        protected float m_r;
        protected float m_mass = 1.0f;
        protected bool m_draw = true;
        protected bool m_update = true;
        protected string m_textureName;
        protected Vector2 m_position;
        protected Point m_frameSize;
        protected Point m_sheetSizeMax;
        protected Point m_sheetSizeMin;
        protected Point m_fullsheetSize;
        protected int m_timeSinceLastFrame;
        protected int m_millisecondsPerFrame;
        protected float m_speed;
        protected int m_currentFrameX = 0;
        protected int m_currentFrameY = 0;
        public Texture2D GetTexture() { return m_texture; }
        public bool GetIsEditable() { return m_isEditable; }
        public int GetCurrentFrameX() { return m_currentFrameX; }
        public int GetCurrentFrameY() { return m_currentFrameY; }
        public float GetSpeed() { return m_speed; }
        public bool GetShouldDraw() { return m_draw; }
        public string GetTextureName() { return m_textureName; }
        public bool GetShouldUpdate() { return m_update; }
        public int GetAnimationSpeed() { return m_millisecondsPerFrame; }
        public bool GetUseCollision() { return m_useCollision; }
        public float GetMass() { return m_mass; }
        public float GetRadius() { return m_r; }
        public Vector2 GetVelocity() { return m_velocity; }
        public Vector2 GetDirection() { return m_direction; }
        public Vector2 GetDestination() { return m_destination; }
        
        public Vector2 GetPreviousPos() { return m_prevPos; }
        public Vector2 GetCurrentPos() { return m_position; }
        public Color GetColor() { return m_color; }
        public bool GetIsColliding() { return m_isColliding; }
        public bool GetIsMoving() { return m_moving; }
        public Point GetUsedSheetSizeMax() { return m_sheetSizeMax; }
        public Point GetUsedSheetSizeMin() { return m_sheetSizeMin; }
        public Point GetFullSheetSize() { return m_fullsheetSize; }
        public Point GetFrameSize() { return m_frameSize; }
        public SpriteEffects GetSpriteEffect() { return m_effect; }
        public bool GetCanMove() { return m_canmove; }
        public void SetIsEditable(bool canbeedited) { m_isEditable = canbeedited; }
        public SPRITE_TYPE GetSpriteType() { return m_type; }
        public void SetPos(Vector2 pos) { m_position = pos; }
        public void SetTexture(string tex) { m_textureName = tex; m_texture = ResourceManager.GetTexture(m_textureName); }
        public  void SetColor(Color c) { m_color = c; }
        public void SetShouldUpdate(bool update) { m_update = update; }
        public void SwapDirection() { m_direction *= -1; }
        public void SetShouldDraw(bool draw) { m_draw = draw; }
        public GameObject(OBJECT_CONSTRUCTION_DATA constructiondata)
        {
            m_fullsheetSize = new Point(constructiondata.fullsheetsizeX, constructiondata.fullSheetsizeY);
            m_textureName = constructiondata.texture;
            m_texture = ResourceManager.GetTexture(m_textureName);
            m_position = new Vector2(constructiondata.x, constructiondata.y);
            m_frameSize.X = constructiondata.width / constructiondata.fullsheetsizeX;
            m_frameSize.Y = constructiondata.height / constructiondata.fullSheetsizeY;
            m_sheetSizeMax = new Point(constructiondata.usedsheetMaxX, constructiondata.usedSheetMaxY);
            m_sheetSizeMin = new Point(constructiondata.usedsheetMinX, constructiondata.usedSheetMinY);
            m_speed = constructiondata.speed;
            m_millisecondsPerFrame = constructiondata.animationSpeedMSperFrame;
            float a = (MathF.PI * (float)m_frameSize.X * (float)m_frameSize.Y) / 4.0f;
            m_r = MathF.Sqrt(a / MathF.PI); //the circle inside the texture
            m_mass = constructiondata.mass;
            m_currentFrameY = m_sheetSizeMin.Y;
            m_currentFrameX = m_sheetSizeMin.X;
            m_prevPos = m_position;
            m_type = constructiondata.type;
        }
        
        public Vector2 KnockbackRectangle(Rectangle bounds, GameObject other)
        {
            
            Rectangle otherbounds = other.GetBounds();

            
            float up = (new Vector2(0, otherbounds.Top) - new Vector2(0, bounds.Bottom)).Length();
            float down = (new Vector2(0, otherbounds.Bottom) - new Vector2(0, bounds.Top)).Length();
            float left = (new Vector2(0, otherbounds.Left) - new Vector2(0, bounds.Right)).Length();
            float right = (new Vector2(0, otherbounds.Right) - new Vector2(0, bounds.Left)).Length();
            Vector2 dir = m_velocity;
            dir.Normalize();
            dir *= -1;
            if (MathF.Abs(m_velocity.Y)>MathF.Abs(m_velocity.X))
            {
                if(m_velocity.Y>0)
                {
                    
                    return dir*up;
                }
                else
                {
                    return dir * down;
                }
            }
            else
            {
                if (m_velocity.X > 0)
                {
                    return dir * left;
                }
                else
                {
                    return dir * right;
                }
            }

        }
        public virtual void AddForce(Vector2 force)
        {
            m_F += force;


        }
        public virtual void UpdateAnimation(GameTime gametime)
        {
           

            m_timeSinceLastFrame += gametime.ElapsedGameTime.Milliseconds;
            if (m_timeSinceLastFrame > m_millisecondsPerFrame)
            {
                m_timeSinceLastFrame = 0;
                m_currentFrameX++;
                if (m_currentFrameX >= m_sheetSizeMax.X)
                {
                    m_currentFrameX = m_sheetSizeMin.X;
                    m_currentFrameY++;
                    if (m_currentFrameY >= m_sheetSizeMax.Y)
                        m_currentFrameY = m_sheetSizeMin.Y;
                }
            }
        }
        protected Point GetGridPoint(Vector2 pos)
        {
            float pX = pos.X;
            float pY = pos.Y;
            if (m_direction.X >= 0)
            {
                pX = pos.X + m_frameSize.X * 0.5f;
            }
            else if (m_direction.X < 0)
            {
                pX = pos.X;
            }
            if (m_direction.Y >= 0)
            {
                pY = pos.Y + m_frameSize.Y * 0.5f;
            }
            else if (m_direction.Y < 0)
            {
                pY = pos.Y;
            }

            int x = (int)MathF.Round((pX / GameManager.GetTileSize()));
            int y = (int)MathF.Round((pY / GameManager.GetTileSize()));
            return new Point(x, y);
        }
        public void LimitMovement(ref Vector2 pos)
        {
            
            Vector2 futurePos = pos + m_position;

           
            Rectangle bounds = GetBounds();
            bounds.X = (int)futurePos.X;
            bounds.Y = (int)futurePos.Y;

            if (!GameManager.IsWithinWindowBounds(bounds))
            {

                GameManager.ClampInWindow(ref pos, bounds);
            }
            //int steps = (int)pos.Length();
            //Vector2 dir = pos;
            //dir.Normalize();
            QUAD_NODE node = new QUAD_NODE();
            GameManager.GetGridPoint(bounds, GameManager.GetRootNode(), ref node);
            if (node.tiles != null)
            {
                if (node.tiles.Count > 0)
                {
                    //pos = (bounds.Location.ToVector2() - m_position);
                    foreach (StaticObject obj in node.tiles)
                    {

                        if (obj.GetBounds().Intersects(bounds))
                        {
                            pos = Vector2.Zero;
                            return;
                            Vector2 collisionVector = KnockbackRectangle(bounds, obj);

                            pos -= collisionVector;

                        }


                    }

                }


            }
            //for (int i=0; i<steps; i++)
            //{
               
                
            //    //bounds.Location -= dir.ToPoint();
            //    //if(bounds.Location.X -m_position.X<=GameManager.GetLeafNodeBoundsSize().X
            //    //    || bounds.Location.Y - m_position.Y <= GameManager.GetLeafNodeBoundsSize().Y)
            //    //{
            //    //    break;
            //    //}
            //}

           















        }
        public bool IsColliding()
        {
            Rectangle bounds = GetBounds();
            
            QUAD_NODE node = new QUAD_NODE();
            GameManager.GetGridPoint(bounds, GameManager.GetRootNode(), ref node);
            if (node.tiles != null)
            {
                if (node.tiles.Count > 0)
                {

                    foreach (StaticObject obj in node.tiles)
                    {

                        if (obj.GetBounds().Intersects(bounds))
                        {

                            return true;

                        }


                    }

                }


            }
            return false;
        }
        public void Collision()
        {
            Rectangle bounds = GetBounds();
            
            if (!GameManager.IsWithinWindowBounds(bounds))
            {

                GameManager.ClampInWindow(ref m_position, bounds);
            }
            QUAD_NODE node = new QUAD_NODE();
            GameManager.GetGridPoint(bounds, GameManager.GetRootNode(), ref node);
            if (node.tiles != null)
            {
                if (node.tiles.Count > 0)
                {
                    
                    foreach (StaticObject obj in node.tiles)
                    {

                        if (obj.GetBounds().Intersects(bounds))
                        {

                            m_position += KnockbackRectangle(bounds, obj);
                            //while (IsColliding())
                            //{
                            //    m_position += KnockbackRectangle(bounds, obj);
                            //}
                        }


                    }

                }


            }



        }
        public void SetGroundPos(Vector2 pos)
        {
            //pos.Y += 1;
            if (pos.Y>=GameManager.GetWindowSize(false))
            {
                m_groundpos = new Point(int.MaxValue, int.MaxValue);
                return;
            }
            
            Rectangle bounds = GetBounds();
            bounds.X = (int)pos.X;
            bounds.Y = (int)pos.Y;
            QUAD_NODE node = new QUAD_NODE();
            GameManager.GetGridPoint(bounds, GameManager.GetRootNode(), ref node);
            if (node.tiles != null)
            {
                if (node.tiles.Count > 0)
                {
                    bool r = false;
                    foreach (StaticObject obj in node.tiles)
                    {

                        if (obj.GetBounds().Intersects(bounds))
                        {
                            m_groundpos = new Point(obj.GetBounds().Left, obj.GetBounds().Top);
                            r = true;
                            

                        }
                        
                    }
                    if(r)
                        return;
                    SetGroundPos(pos + new Vector2(0, GameManager.GetTileSize()/2));
                }
                else
                {
                    SetGroundPos(pos + new Vector2(0, GameManager.GetTileSize()/2));
                }
            }
            else
            {
                SetGroundPos(pos + new Vector2(0, GameManager.GetTileSize() / 2));
            }
        }
       
        public virtual void Update(GameTime gametime)
        {
           m_position.Round();
            
            m_velocity = m_F / m_mass;
           
            Vector2 movement = m_velocity * (float)gametime.ElapsedGameTime.TotalSeconds;
            movement.X = (int)movement.X;
            movement.Y = (int)movement.Y;
            //LimitMovement(ref movement);
            
            m_position += movement;
            Collision();
            SetGroundPos(m_position);
            if (m_groundpos.Y != -1)
            {
                float length = (m_position + m_frameSize.ToVector2()).Y - m_groundpos.ToVector2().Y;
                if (length < 0)
                    length *= -1;
                m_grounded = length <= m_mingrounddistance;
            }



            m_F = Vector2.Zero;
        }
        
        public virtual void Draw(SpriteBatch batch)
        {
            if(m_type == SPRITE_TYPE.BACKGROUND)
            {
                batch.Draw(m_texture,
            m_position,
            new Rectangle(0, 0,
            GameManager.GetWindowSize(true), GameManager.GetWindowSize(false)),
            m_color, 0, Vector2.Zero,
            1f, m_effect, 0);
             
            }
            else
            {
                batch.Draw(m_texture,
            m_position,
            new Rectangle(m_currentFrameX * m_frameSize.X,
            m_currentFrameY * m_frameSize.Y,
            m_frameSize.X, m_frameSize.Y),
            m_color, 0, Vector2.Zero,
            1f, m_effect, 0);
            }
            
        }
        public  Rectangle GetBounds()
        {
            return new Rectangle(
            (int)m_position.X,
             (int)m_position.Y,
            m_frameSize.X,
            m_frameSize.Y);

            
        }

     
    }
}

