using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SuperMario;

namespace Supermario
{
    internal abstract class GameObject
    {
        protected SPRITE_TYPE m_type;
        protected Texture2D m_texture;
        protected Vector2 m_F = Vector2.Zero;
        protected bool m_isEditable = true;
        protected bool m_canmove;
        protected SpriteEffects m_effect = SpriteEffects.None;
        protected bool m_jumping = false;
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
        protected Point m_sheetSize;
        protected Point m_fullsheetSize;
        protected int m_timeSinceLastFrame;
        protected int m_millisecondsPerFrame;
        protected float m_speed;
        protected int m_currentFrameX = 0;
        protected int m_currentFrameY = 0;
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
        public bool GetIsJumping() { return m_jumping; }
        public Vector2 GetPreviousPos() { return m_prevPos; }
        public Vector2 GetCurrentPos() { return m_position; }
        public Color GetColor() { return m_color; }
        public bool GetIsColliding() { return m_isColliding; }
        public bool GetIsMoving() { return m_moving; }
        public Point GetUsedSheetSize() { return m_sheetSize; }
        public Point GetFullSheetSize() { return m_fullsheetSize; }
        public Point GetFrameSize() { return m_frameSize; }
        public SpriteEffects GetSpriteEffect() { return m_effect; }
        public bool GetCanMove() { return m_canmove; }
        public void SetIsEditable(bool canbeedited) { m_isEditable = canbeedited; }
        public SPRITE_TYPE GetSpriteType() { return m_type; }
        public void SetPos(Vector2 pos) { m_position = pos; }
        public void SetTexture(string tex) { m_textureName = tex; m_texture = ResourceManager.GetTexture(m_textureName); }
        public  void SetColor(Color c) { m_color = c; }
        protected GameObject(OBJECT_CONSTRUCTION_DATA constructiondata)
        {
            m_fullsheetSize = new Point(constructiondata.fullsheetsizeX, constructiondata.fullSheetsizeY);
            m_textureName = constructiondata.texture;
            m_texture = ResourceManager.GetTexture(m_textureName);
            m_position = new Vector2(constructiondata.x, constructiondata.y);
            m_frameSize.X = constructiondata.width / constructiondata.fullsheetsizeX;
            m_frameSize.Y = constructiondata.height / constructiondata.fullSheetsizeY;
            m_sheetSize = new Point(constructiondata.usedsheetX, constructiondata.usedSheetY);
            m_speed = constructiondata.speed;
            m_millisecondsPerFrame = constructiondata.animationSpeedMSperFrame;
            float a = (MathF.PI * (float)m_frameSize.X * (float)m_frameSize.Y) / 4.0f;
            m_r = MathF.Sqrt(a / MathF.PI); //the circle inside the texture
            m_mass = constructiondata.mass;
            m_currentFrameY = constructiondata.usedSheetY;
            m_prevPos = m_position;
            m_type = constructiondata.type;
        }
        
        public virtual void AddForce(Vector2 force)
        {
            m_F += force;
        }
        public virtual void Update(GameTime gametime)
        {
            m_F += m_direction * m_speed * gametime.ElapsedGameTime.Seconds;
            m_velocity = m_F/m_mass* gametime.ElapsedGameTime.Seconds;
            if (m_canmove)
                m_position += m_velocity*gametime.ElapsedGameTime.Seconds;

            m_F = Vector2.Zero;
        }
        public virtual void Draw(SpriteBatch batch)
        {
            batch.Draw(m_texture,
            m_position,
            new Rectangle(m_currentFrameX * m_frameSize.X,
            m_currentFrameY * m_frameSize.Y,
            m_frameSize.X, m_frameSize.Y),
            m_color, 0, Vector2.Zero,
            1f, m_effect, 0);
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

