using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermario
{
    internal class GameObjectManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        SpriteBatch m_spriteBatch;
        SpriteFont m_font;

        List<GameObject> m_editorSprites = new List<GameObject>();
        public GameObjectManager(Game game) : base(game)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            m_spriteBatch.Begin();
            foreach (GameObject sprite in ResourceManager.GetObjects())
            {
                if (sprite.GetShouldDraw())
                {
                    sprite.Draw(m_spriteBatch);
                }
            }

            if(GameManager.GetCurrentLevel()==SuperMario.LEVEL_TYPE.LEVELE)
            {
                m_spriteBatch.DrawString(m_font, "Press E to switch", Vector2.Zero, Color.Black);
                Vector2 vec2 = Vector2.Zero;
                foreach (GameObject s in m_editorSprites)
                {
                    if(s is Player)
                    {
                        Vector2 startPos = Vector2.Zero;
                        startPos.Y += GameManager.GetRes(false)-(GameManager.GetTileSize()*2);
                        s.SetPos(startPos);
                        s.Draw(m_spriteBatch);
                    }
                    if (s.GetSpriteType() == LevelManager.GetSelection()&&s is not Player)
                    {
                        vec2.X = m_font.MeasureString("Press E to switch").X;
                        s.SetPos(vec2);
                        s.Draw(m_spriteBatch);
                        break;
                    }

                }
                vec2.X = 0;
                vec2.Y += m_font.MeasureString("P").Y;
                m_spriteBatch.DrawString(m_font, "Press ENTER to save", vec2, Color.Black);
                vec2.Y += m_font.MeasureString("P").Y;
                m_spriteBatch.DrawString(m_font, "Press ESCAPE to return", vec2, Color.Black);

               
               


            }


            base.Draw(gameTime);
            m_spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (GameObject sprite in ResourceManager.GetObjects())
            {
                if (sprite.GetShouldUpdate())
                {
                    
                    sprite.Update(gameTime);
                }
            }
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            m_font = Game.Content.Load<SpriteFont>("font");
            m_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
       
            //m_editorSprites.Add(new Enemy(ResourceManager.GetSpritedata(SuperMario.SPRITE_TYPE.ENEMY)));
            m_editorSprites.Add(new StaticObject(ResourceManager.GetSpritedata(SuperMario.SPRITE_TYPE.COINBLOCK)));
            m_editorSprites.Add(new StaticObject(ResourceManager.GetSpritedata(SuperMario.SPRITE_TYPE.BLOCK)));
            m_editorSprites.Add(new Player(ResourceManager.GetSpritedata(SuperMario.SPRITE_TYPE.PLAYER)));
            base.LoadContent();
        }
    }
}
