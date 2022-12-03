using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using SuperMario;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;

namespace Supermario
{
    public class Game1 : Game
    {
        const int m_resX = 800;
        const int m_resY = 600;
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        GameManager m_gamemanager;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = m_resX;
            _graphics.PreferredBackBufferHeight = m_resY;
            _graphics.ApplyChanges();
            m_gamemanager = new GameManager(this, m_resX, m_resY);
            Components.Add(m_gamemanager.GetResourceManager());
            Components.Add(m_gamemanager.GetGameObjectManager());
            Components.Add(m_gamemanager.GetLevelManager());
            GameManager.GetSetEditorCamera() = new Camera(GraphicsDevice.Viewport);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            m_gamemanager.LoadMenuObjects();
            
        }

        protected override void Update(GameTime gameTime)
        {
           switch(GameManager.GetState())
            {
                case GAME_STATE.MENU:
                    {
                        if (KeyMouseReader.KeyPressed(Keys.Escape))
                            Exit();
                        break;

                    }
                case GAME_STATE.GAME:
                    {
                        if (GameManager.GetOldLevel() != GameManager.GetCurrentLevel())
                        {
                            m_gamemanager.LoadLevel(GameManager.GetCurrentLevel(), GraphicsDevice.Viewport);
                            GameManager.SetOldLevel(GameManager.GetCurrentLevel());
                        }
                        if (KeyMouseReader.KeyPressed(Keys.Escape))
                        {
                            GameManager.SetLevel(LEVEL_TYPE.NONE);
                            GameManager.SetOldLevel(LEVEL_TYPE.NONE);
                            GameManager.SetState(GAME_STATE.MENU);
                        }
                            
                        break;

                    }
                case GAME_STATE.HIGHSCORE:
                    {
                        if (KeyMouseReader.KeyPressed(Keys.Escape))
                        {
                            GameManager.SetLevel(LEVEL_TYPE.NONE);
                            GameManager.SetOldLevel(LEVEL_TYPE.NONE);
                            GameManager.SetState(GAME_STATE.MENU);
                        }
                        break;

                    }
                case GAME_STATE.EDITOR:
                    {
                        if (GameManager.GetOldLevel() != GameManager.GetCurrentLevel())
                        {
                            m_gamemanager.LoadLevel(GameManager.GetCurrentLevel(), GraphicsDevice.Viewport);
                            GameManager.SetOldLevel(GameManager.GetCurrentLevel());
                        }
                        
                        if (KeyMouseReader.KeyPressed(Keys.Escape))
                        {
                            GameManager.SetLevel(LEVEL_TYPE.NONE);
                            GameManager.SetOldLevel(LEVEL_TYPE.NONE);
                            m_gamemanager.SaveLevel();
                            GameManager.SetState(GAME_STATE.MENU);
                        }

                        

                        break;

                    }
            }
            
            
            if(GameManager.GetState()!=GameManager.GetOldState())
            {
                if (GameManager.GetState() == GAME_STATE.HIGHSCORE)
                {
                    string text1 = "";
                    if (GameManager.GetOldState() != GameManager.GetState())
                    {
                        StreamReader sr = new StreamReader("../../../highscore.txt");
                        while(!sr.EndOfStream)
                        {
                            string line = sr.ReadLine();
                            if (line[0]=='#')
                            {
                                continue;
                            }
                            //level 1 32
                           string[] words = line.Split(' ');
                            if (!GameManager.GetSetHighScore().ContainsKey(int.Parse(words[2])))
                            {

                                GameManager.GetSetHighScore().Add(int.Parse(words[2]), line);
                            }
                        }
                      
                        sr.Close();
                    }
                    GameManager.GetSetHighScore() = GameManager.GetSetHighScore().OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    //GameManager.GetSetHighScore().Reverse();
                    string text = "#highscore\n";
                    foreach (KeyValuePair<int, string> kv in GameManager.GetSetHighScore())
                    {
                        text += kv.Value + "\n";
                    }
                    File.WriteAllText("../../../highscore.txt", text);
                }
                


                GameManager.SetOldState(GameManager.GetState());
            }
            


            KeyMouseReader.Update();
          
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            

            base.Draw(gameTime);
        }
    }
}