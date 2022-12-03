using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrayNotify;

namespace Supermario
{
    internal class RenderManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        float m_bgspeed = 20;
        SpriteBatch m_spriteBatch;
        SpriteFont m_font;
        Vector2 m_positionEditorCamera = Vector2.Zero;
        List<GameObject> m_editorSprites = new List<GameObject>();
        public RenderManager(Game game) : base(game)
        {
            m_positionEditorCamera = GameManager.GetPlayerStart();
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
                m_spriteBatch.DrawString(m_font, "Run to the end of the map to win.", pos, Color.Black);
                pos.X = GameManager.GetRes(true) / 3;
                pos.Y = GameManager.GetRes(false) / 4;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1] as Button).SetText("LEVEL 1");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1] as Button).Draw(m_spriteBatch, m_font);
                Vector2 offset = m_font.MeasureString("LEVEL 1");
                pos.Y += offset.Y*2;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2] as Button).SetText("LEVEL 2");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2] as Button).Draw(m_spriteBatch, m_font);
                pos.Y += offset.Y*2;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3] as Button).SetText("LEVEL 3");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3] as Button).Draw(m_spriteBatch, m_font);
                pos.Y += offset.Y*2;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM] as Button).SetText("Custom Level");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM] as Button).Draw(m_spriteBatch, m_font);
                offset = m_font.MeasureString("Custom Level");
                pos.Y = GameManager.GetRes(false) / 4;
                pos.X += offset.X*2;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS] as Button).SetText("High Score");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS] as Button).Draw(m_spriteBatch, m_font);
                pos.Y += offset.Y * 2;
                ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR].SetPos(pos);
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR] as Button).SetText("Editor");
                (ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR] as Button).Draw(m_spriteBatch, m_font);
                m_spriteBatch.End();
            }
            else if(GameManager.GetState() == GAME_STATE.EDITOR)
            {
                
                m_spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null,
                  GameManager.GetSetEditorCamera().Transform);
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
                Vector2 vec2 = Vector2.Zero+ GameManager.GetSetEditorCamera().Position;
                foreach (GameObject s in m_editorSprites)
                {
                    
                    if (s.GetSpriteType() == LogicManager.GetSelection() && s is not Player)
                    {
                        vec2.X = m_font.MeasureString("Press E to switch sprite").X;
                        s.SetPos(vec2);
                        s.Draw(m_spriteBatch);
                        break;
                    }

                }
                vec2.X = GameManager.GetSetEditorCamera().Position.X;
                vec2.Y += m_font.MeasureString("P").Y;
                m_spriteBatch.DrawString(m_font, "The level is saved when you return", vec2, Color.Black);
                vec2.Y += m_font.MeasureString("P").Y;
                m_spriteBatch.DrawString(m_font, "Press ESCAPE to return", vec2, Color.Black);
                vec2.Y += m_font.MeasureString("P").Y;
                m_spriteBatch.DrawString(m_font, "Press ENTER to remove all tiles", vec2, Color.Black);
                m_spriteBatch.End();
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
                //if (GameManager.GetGrid() != null)
                //{
                //    for (int i = 0; i < GameManager.GetTileCount(true); i++)
                //    {
                //        for (int j = 0; j < GameManager.GetTileCount(false); j++)
                //        {

                //            if (GameManager.GetGrid()[i, j].obstacle)
                //            {
                //                m_spriteBatch.Draw(ResourceManager.GetTexture("background"), new Rectangle(i *
                //                    25, j *
                //                    25, 25, 25)
                //                    , Color.Black);
                //            }

                //        }
                //    }
                //}


                //for (int i = 0; i < GameManager.GetRes(true) / 10 * 25; i++)
                //{
                //    for (int j = 0; j < (int)Math.Ceiling((double)GameManager.GetRes(false) / 10 * 25); j++)
                //    {
                //        QUAD_NODE node = new QUAD_NODE();
                //        GameManager.GetGridPoint(new Vector2(i * 75, j * 19), GameManager.GetRootNode(),
                //            ref node);
                //        //m_spriteBatch.Draw(ResourceManager.GetTexture("background"), node.bounds, Color.Black);
                //        if(node.tiles!=null)
                //        {
                //            foreach (StaticObject s in node.tiles)
                //            {
                //                s.Draw(m_spriteBatch);
                //            }
                //        }


                //    }
                //}

                m_spriteBatch.End();
            }


            base.Draw(gameTime);
           
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
            else if(GameManager.GetState() == GAME_STATE.EDITOR)
            {
                if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Left))
                {
                    m_positionEditorCamera.X += -10;
                }

                if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Right))
                {
                    m_positionEditorCamera.X += 10;
                }
                if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Up))
                {
                    m_positionEditorCamera.Y += -10;
                }

                if (KeyMouseReader.KeyHeld(Microsoft.Xna.Framework.Input.Keys.Down))
                {
                    m_positionEditorCamera.Y += 10;
                }
                
                GameManager.GetSetEditorCamera().SetPosition(m_positionEditorCamera);
            }
            
            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            m_font = Game.Content.Load<SpriteFont>("font");
            m_spriteBatch = new SpriteBatch(Game.GraphicsDevice);
       
            m_editorSprites.Add(new Enemy(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.ENEMY)));
            m_editorSprites.Add(new Enemy(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.MUSHROOM)));
          
            m_editorSprites.Add(new StaticObject(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.COINBLOCK)));
            m_editorSprites.Add(new StaticObject(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.BLOCK)));
            m_editorSprites.Add(new StaticObject(ResourceManager.GetSpritedata(Supermario.SPRITE_TYPE.PIPE)));





            base.LoadContent();
        }
    }
}
