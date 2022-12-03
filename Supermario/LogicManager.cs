using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D11;
using SharpDX.Direct3D9;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermario
{
    internal class LogicManager : DrawableGameComponent
    {
        LEVEL_TYPE m_currentLevel;
        static SPRITE_TYPE m_sprite = SPRITE_TYPE.BLOCK;
        const int m_spriteTypeCount = 7;
        const int m_pipedistance = 10;
        
        const double m_time = 45;

        public LogicManager(Game game, LEVEL_TYPE currentLevel) : base(game)
        {
            m_currentLevel = currentLevel;
            
        }
        public void SetLevelType(LEVEL_TYPE currentLevel) { m_currentLevel = currentLevel;
            ResourceManager.GetPlayer().GetSetTime().ResetAndStart(m_time);

            ResourceManager.GetPlayer().SetScore(0);


        }
        public static SPRITE_TYPE GetSelection() { return m_sprite; }
        public override void Update(GameTime gameTime)
        {
            if(GameManager.GetState()==GAME_STATE.MENU)
            {
                if ((ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1] as Button).GetPressed())
                {
                    GameManager.SetLevel(LEVEL_TYPE.LEVEL1);
                    GameManager.SetState(GAME_STATE.GAME);
                    (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL1] as Button).SetPressed(false);
                }
                else if ((ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2] as Button).GetPressed())
                {
                    GameManager.SetLevel(LEVEL_TYPE.LEVEL2);
                    GameManager.SetState(GAME_STATE.GAME);
                    (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL2] as Button).SetPressed(false);
                }
                else if ((ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3] as Button).GetPressed())
                {
                    GameManager.SetLevel(LEVEL_TYPE.LEVEL3);
                    GameManager.SetState(GAME_STATE.GAME);
                    (ResourceManager.GetButtons()[(int)BUTTON_TYPE.LEVEL3] as Button).SetPressed(false);
                }
                else if ((ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR] as Button).GetPressed())
                {
                    GameManager.SetLevel(LEVEL_TYPE.LEVELE);
                    GameManager.SetState(GAME_STATE.EDITOR);
                    (ResourceManager.GetButtons()[(int)BUTTON_TYPE.EDITOR] as Button).SetPressed(false);
                }
                else if ((ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM] as Button).GetPressed())
                {
                    GameManager.SetLevel(LEVEL_TYPE.LEVELE);
                    GameManager.SetState(GAME_STATE.GAME);
                    (ResourceManager.GetButtons()[(int)BUTTON_TYPE.CUSTOM] as Button).SetPressed(false);
                }
                else if ((ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS] as Button).GetPressed())
                {
                    GameManager.SetState(GAME_STATE.HIGHSCORE);
                    (ResourceManager.GetButtons()[(int)BUTTON_TYPE.HS] as Button).SetPressed(false);
                }
            }
            else if (GameManager.GetState()==GAME_STATE.GAME)
            {
                ResourceManager.GetPlayer().GetSetTime().Update(gameTime.ElapsedGameTime.TotalSeconds);
                

               if (ResourceManager.GetPlayer().GetCurrentPos().X >=
                        GameManager.GetWindowSize(true) - GameManager.GetTileSize()*2)
                {
                    ResourceManager.GetPlayer().SetScore(ResourceManager.GetPlayer().GetSCore() + 5*
                        (int)ResourceManager.GetPlayer().GetSetTime().GetTime());

                    string level = "Level 1";
                    if(GameManager.GetCurrentLevel()==LEVEL_TYPE.LEVEL2)
                    {
                        level = "Level 2";
                    }
                    else if(GameManager.GetCurrentLevel() == LEVEL_TYPE.LEVEL3)
                    {
                        level = "Level 3";
                    }
                    else if (GameManager.GetCurrentLevel() == LEVEL_TYPE.LEVELE)
                    {
                        level = "Level C";
                    }
                    if(!GameManager.GetSetHighScore().ContainsKey(ResourceManager.GetPlayer().GetSCore()))
                    {
                        GameManager.GetSetHighScore().Add(ResourceManager.GetPlayer().GetSCore(), level + " " +
                        ResourceManager.GetPlayer().GetSCore().ToString());
                        GameManager.GetSetHighScore() = GameManager.GetSetHighScore().OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
                        
                    }
                    GameManager.SetState(GAME_STATE.HIGHSCORE);
                }
                if (ResourceManager.GetPlayer().GetSetTime().IsDone())
                {
                    string level = "Level 1";
                    if (GameManager.GetCurrentLevel() == LEVEL_TYPE.LEVEL2)
                    {
                        level = "Level 2";
                    }
                    else if (GameManager.GetCurrentLevel() == LEVEL_TYPE.LEVEL3)
                    {
                        level = "Level 3";
                    }
                    else if (GameManager.GetCurrentLevel() == LEVEL_TYPE.LEVELE)
                    {
                        level = "Level C";
                    }

                    GameManager.GetSetHighScore().Add(ResourceManager.GetPlayer().GetSCore(), level + " " +
                        ResourceManager.GetPlayer().GetSCore().ToString());
                    GameManager.GetSetHighScore() = GameManager.GetSetHighScore().OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
                    //GameManager.GetSetHighScore().Reverse();
                    GameManager.SetState(GAME_STATE.HIGHSCORE);
                }

                foreach (Enemy e in ResourceManager.GetEnemies())
                {
                    if (ResourceManager.GetPlayer().CircleIntersects(e))
                    {
                        ResourceManager.GetPlayer().Knocback(e);
                        //do something
                        ResourceManager.GetPlayer().SetScore(ResourceManager.GetPlayer().GetSCore() - 10);
                    }


                }
                foreach (StaticObject e in ResourceManager.GetMushrooms())
                {
                    if (ResourceManager.GetPlayer().CircleIntersects(e))
                    {
                        e.SetShouldDraw(false);
                        //do something
                        ResourceManager.GetPlayer().SetScore(ResourceManager.GetPlayer().GetSCore() + 10);
                    }


                }
                foreach (StaticObject p in ResourceManager.GetPipes())
                {
                    Rectangle rect = p.GetBounds();
                    if (rect.Y > ResourceManager.GetPlayer().GetCurrentPos().Y)
                        rect.Y -= m_pipedistance;
                    else
                        rect.Y += m_pipedistance;
                    Rectangle rect2 = ResourceManager.GetPlayer().GetBounds();
                    if (rect2.Intersects(rect))
                    {
                        Vector2 dir =  ResourceManager.GetPlayer().KnockbackRectangle(ResourceManager.GetPlayer().GetBounds()
                          , p);
                        dir.Normalize();
                        if (dir.Y < 0)
                            dir.Y -= GameManager.GetGravity() * 2;
                        else
                        {
                            
                            break;
                        }
                           
                        Vector2 posCorrection = ResourceManager.GetPlayer().GetCurrentPos();
                        posCorrection.X = p.GetCurrentPos().X;
                        ResourceManager.GetPlayer().SetPos(posCorrection);//prevents shaking if you end up in between blocks.
                        ResourceManager.GetPlayer().AddForce(dir * ResourceManager.GetPlayer().GetSpeed()*4);
                        //play sound effect
                        
                    }
                }
            }
            else if(GameManager.GetState()==GAME_STATE.EDITOR)
            {
                switch (m_currentLevel)
                {
                    
                    case LEVEL_TYPE.LEVELE:
                        {
                            SelectSprite();
                            if (KeyMouseReader.LeftClickHover())
                            {
                                Point mp = KeyMouseReader.mouseState.Position;
                                Point p = new Point(mp.X, mp.Y);
                                p += GameManager.GetSetEditorCamera().Position.ToPoint();
                                //GameManager.GetGridPoint(new Vector2(p.X, p.Y), GameManager.GetRootNode(), ref p);
                                if (!GameManager.ModWithRes(ref p))
                                {
                                    return;
                                }
                                foreach (GameObject sprite in ResourceManager.GetObjects())
                                {
                                    OBJECT_CONSTRUCTION_DATA selectiondata = ResourceManager.GetSpritedata(m_sprite);

                                    Rectangle rect = new Rectangle(p.X, p.Y, selectiondata.width/selectiondata.fullsheetsizeX,
                                        selectiondata.height/selectiondata.fullSheetsizeY);
                                    if (sprite.GetBounds().Intersects(rect) && sprite.GetIsEditable())
                                    {
                                        return;
                                    }
                                }
                                switch (m_sprite)
                                {
                                    case SPRITE_TYPE.COINBLOCK:
                                        {
                                            OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.COINBLOCK);


                                            data.x = p.X; data.y = p.Y;
                                            int size = GameManager.GetTileSize();
                                            data.height = data.width = size;

                                            GameObject s = new StaticObject(data);
                                            ResourceManager.AddObject(s);
                                            break;
                                        }
                                    case SPRITE_TYPE.ENEMY:
                                        {
                                            OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY);


                                            data.x = p.X; data.y = p.Y;



                                            GameObject s = new Enemy(data);
                                            ResourceManager.AddObject(s);
                                            break;
                                        }
                                    
                                    case SPRITE_TYPE.MUSHROOM:
                                        {
                                            OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.MUSHROOM);

                                            data.x = p.X; data.y = p.Y;

                                            GameObject s = new StaticObject(data);
                                            ResourceManager.AddObject(s);
                                            break;
                                        }
                                    case SPRITE_TYPE.BLOCK:
                                        {
                                            OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.BLOCK);

                                            
                                            data.x = p.X; data.y = p.Y;
                                            int size = GameManager.GetTileSize();
                                            data.height = data.width = size;

                                            GameObject s = new StaticObject(data);
                                            ResourceManager.AddObject(s);
                                            break;
                                        }
                                    case SPRITE_TYPE.PIPE:
                                        {
                                            OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.PIPE);


                                            data.x = p.X; data.y = p.Y;


                                            GameObject s = new StaticObject(data);
                                            ResourceManager.AddObject(s);
                                            break;
                                        }

                                }
                            }
                            else if (KeyMouseReader.RightClickHover())
                            {
                                Point mp = KeyMouseReader.mouseState.Position;
                                Point p = new Point(mp.X, mp.Y);
                                p += GameManager.GetSetEditorCamera().Position.ToPoint();
                                foreach (GameObject sprite in ResourceManager.GetObjects())
                                {
                                    if (sprite.GetBounds().Contains(p) && sprite.GetIsEditable())
                                    {
                                        ResourceManager.GetObjects().Remove(sprite);
                                        return;
                                    }
                                }
                            }
                            if(KeyMouseReader.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter))
                            {
                                Player p = ResourceManager.GetPlayer();
                                ResourceManager.GetObjects().Clear();
                                ResourceManager.GetEnemies().Clear();
                                ResourceManager.GetPipes().Clear();
                                ResourceManager.GetMushrooms().Clear();
                                ResourceManager.AddObject(p);

                            }
                            break;
                        }
                }
            }


            
            base.Update(gameTime);
        }
        private void SelectSprite()
        {
            if (KeyMouseReader.KeyPressed(Microsoft.Xna.Framework.Input.Keys.E))
            {
                if ((int)m_sprite < m_spriteTypeCount)
                {
                    m_sprite++;

                }
                else
                {
                    m_sprite = 0;
                }
                if (m_sprite == SPRITE_TYPE.PLAYER)
                {
                    SelectSprite();
                }
            }
        }


    }
    
}
