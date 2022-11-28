﻿using Microsoft.Xna.Framework;
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
    internal class LevelManager : DrawableGameComponent
    {
        LEVEL_TYPE m_currentLevel;
        static SPRITE_TYPE m_sprite = SPRITE_TYPE.BLOCK;
        const int m_spriteTypeCount = 7;
        public LevelManager(Game game, LEVEL_TYPE currentLevel) : base(game)
        {
            m_currentLevel = currentLevel;
        }
        public void SetLevelType(LEVEL_TYPE currentLevel) { m_currentLevel = currentLevel; }
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
                foreach (Enemy e in ResourceManager.GetEnemies())
                {
                    if (ResourceManager.GetPlayer().PixelIntersects(e))
                    {
                        ResourceManager.GetPlayer().Knocback(e, gameTime);
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
                                //GameManager.GetGridPoint(new Vector2(p.X, p.Y), GameManager.GetRootNode(), ref p);
                                GameManager.ModWithRes(ref p);
                                if (GameManager.IsWithinWindowBounds(new Rectangle(p, new Point(1, 1))))
                                {
                                    foreach (GameObject sprite in ResourceManager.GetObjects())
                                    {
                                        if (sprite.GetBounds().Contains(p) && sprite.GetIsEditable())
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
                                        case SPRITE_TYPE.ENEMY0:
                                            {
                                                OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY0);

                                                GameManager.ModWithRes(ref p, data.width / data.fullsheetsizeX, data.height / data.fullSheetsizeY);
                                                data.x = p.X; data.y = p.Y;


                                                
                                                GameObject s = new Enemy(data);
                                                ResourceManager.AddObject(s);
                                                break;
                                            }
                                        case SPRITE_TYPE.ENEMY1:
                                            {
                                                OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY1);
                                                GameManager.ModWithRes(ref p, data.width/data.fullsheetsizeX, data.height/ data.fullSheetsizeY);

                                                data.x = p.X; data.y = p.Y;
                                               
                                                GameObject s = new Enemy(data);
                                                ResourceManager.AddObject(s);
                                                break;
                                            }
                                        case SPRITE_TYPE.ENEMY2:
                                            {
                                                OBJECT_CONSTRUCTION_DATA data = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY2);
                                                GameManager.ModWithRes(ref p, data.width / data.fullsheetsizeX, data.height / data.fullSheetsizeY);

                                                data.x = p.X; data.y = p.Y;
                                              
                                                GameObject s = new Enemy(data);
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

                                    }
                                }
                            }
                            else if (KeyMouseReader.RightClickHover())
                            {
                                Point mp = KeyMouseReader.mouseState.Position;
                                Point p = new Point(mp.X, mp.Y);
                                GameManager.ModWithRes(ref p);
                                if (GameManager.IsWithinWindowBounds(new Rectangle(mp, new Point(0, 0))))
                                {
                                    foreach (GameObject sprite in ResourceManager.GetObjects())
                                    {
                                        if (sprite.GetBounds().Contains(p) && sprite.GetIsEditable())
                                        {
                                            ResourceManager.GetObjects().Remove(sprite);
                                            return;
                                        }
                                    }

                                }
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
