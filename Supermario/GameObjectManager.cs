﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using SuperMario;
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
            
            if (GameManager.GetState() == GAME_STATE.MENU)
            {
                m_spriteBatch.Begin();
                foreach (KeyValuePair<MENU_TYPE,GameObject> sprite in ResourceManager.GetMenuObjects())
                {
                    if(sprite.Key==MENU_TYPE.START)
                    {
                        if (sprite.Value.GetShouldDraw())
                        {
                            sprite.Value.Draw(m_spriteBatch);
                        }
                        break;
                    }
                    
                }
                
                Vector2 pos = Vector2.Zero;
                pos.X = GameManager.GetRes(true) / 2;
                pos.Y = GameManager.GetRes(false) / 2;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1] as Button).SetText("LEVEL 1");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1] as Button).Draw(m_spriteBatch, m_font);
                Vector2 offset = m_font.MeasureString("LEVEL 1");
                pos.Y += offset.Y;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2] as Button).SetText("LEVEL 2");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2] as Button).Draw(m_spriteBatch, m_font);
                pos.Y += offset.Y;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3] as Button).SetText("LEVEL 3");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3] as Button).Draw(m_spriteBatch, m_font);
                pos.Y += offset.Y;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM] as Button).SetText("Custom Level");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM] as Button).Draw(m_spriteBatch, m_font);
                offset = m_font.MeasureString("Custom Level");
                pos.Y = GameManager.GetRes(true) / 2;
                pos.X += offset.X;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS] as Button).SetText("High Score");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS] as Button).Draw(m_spriteBatch, m_font);
                pos.Y += offset.Y;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR] as Button).SetText("Editor");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR] as Button).Draw(m_spriteBatch, m_font);
              
            }
            else if(GameManager.GetState() == GAME_STATE.EDITOR)
            {
                m_spriteBatch.Begin();
                foreach (KeyValuePair<MENU_TYPE, GameObject> sprite in ResourceManager.GetMenuObjects())
                {
                    if (sprite.Key == MENU_TYPE.START)
                    {
                        if (sprite.Value.GetShouldDraw())
                        {
                            sprite.Value.Draw(m_spriteBatch);
                        }
                        break;
                    }

                }
                foreach (GameObject sprite in ResourceManager.GetObjects())
                {
                    if (sprite.GetShouldDraw())
                    {
                        sprite.Draw(m_spriteBatch);
                        
                    }
                   
                }
                m_spriteBatch.DrawString(m_font, "Press E to switch", Vector2.Zero, Color.Black);
                Vector2 vec2 = Vector2.Zero;
                foreach (GameObject s in m_editorSprites)
                {
                    
                    if (s.GetSpriteType() == LevelManager.GetSelection() && s is not Player)
                    {
                        vec2.X = m_font.MeasureString("Press E to switch sprite").X;
                        s.SetPos(vec2);
                        s.Draw(m_spriteBatch);
                        break;
                    }

                }
                vec2.X = 0;
                vec2.Y += m_font.MeasureString("P").Y;
                m_spriteBatch.DrawString(m_font, "The level is saved when you return", vec2, Color.Black);
                vec2.Y += m_font.MeasureString("P").Y;
                m_spriteBatch.DrawString(m_font, "Press ESCAPE to return", vec2, Color.Black);
                
            }
            else if(GameManager.GetState() == GAME_STATE.GAME)
            {
                if(ResourceManager.GetPlayer()!= null)
                {
                    m_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                    ResourceManager.GetPlayer().GetCamera().Transform);
                }
                else
                {
                    m_spriteBatch.Begin();
                }
                
                if (ResourceManager.GetMenuObjects()[MENU_TYPE.START].GetShouldDraw())
                    ResourceManager.GetMenuObjects()[MENU_TYPE.START].Draw(m_spriteBatch);



                foreach (GameObject sprite in ResourceManager.GetObjects())
                {
                    
                    if (sprite.GetShouldDraw())
                    {
                        sprite.Draw(m_spriteBatch);
                    }
                    
                }

                

            }


            base.Draw(gameTime);
            m_spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            if (GameManager.GetState() == GAME_STATE.MENU)
            {
                foreach (GameObject sprite in ResourceManager.GetButtons())
                {
                    if(sprite.GetShouldUpdate())
                        sprite.Update(gameTime);
                }
                
                
            }
            else if(GameManager.GetState() == GAME_STATE.GAME)
            {
                foreach (GameObject sprite in ResourceManager.GetObjects())
                {
                    if (sprite.GetShouldUpdate())
                    {

                        sprite.Update(gameTime);
                    }
                }
                
                
                
            }
            
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            m_font = Game.Content.Load<SpriteFont>("font");
            m_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
       
            m_editorSprites.Add(new Enemy(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.ENEMY0)));
            m_editorSprites.Add(new Enemy(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.ENEMY1)));
            m_editorSprites.Add(new Enemy(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.ENEMY2)));
            m_editorSprites.Add(new StaticObject(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.COINBLOCK)));
            m_editorSprites.Add(new StaticObject(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.BLOCK)));
            
            




            base.LoadContent();
        }
    }
}
