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
        SPRITE_TYPE m_sprite;
        const int m_spriteTypeCount = 5;
        public LevelManager(Game game, LEVEL_TYPE currentLevel) : base(game)
        {
            m_currentLevel = currentLevel;
        }
        public void SetLevelType(LEVEL_TYPE currentLevel) { m_currentLevel = currentLevel; }
        public override void Update(GameTime gameTime)
        {
            switch(m_currentLevel)
            {
                case LEVEL_TYPE.LEVEL0:
                    {
                        break;
                    }
                case LEVEL_TYPE.LEVEL1:
                    {
                        break;
                    }
                case LEVEL_TYPE.LEVEL2:
                    {
                        break;
                    }
                case LEVEL_TYPE.LEVELE:
                    {
                        SelectSprite();
                        if (KeyMouseReader.LeftClickHover())
                        {
                            Point mp = KeyMouseReader.mouseState.Position;
                            Point p = new Point(mp.X, mp.Y);
                            GameManager.ModTileWithRes(ref p);
                            if (GameManager.IsWithinWindowBounds(new Rectangle(mp, new Point(0, 0))))
                            {
                                foreach (GameObject sprite in ResourceManager.GetResourceManager().GetSprites())
                                {
                                    if (sprite.GetBounds().Contains(p) && sprite.GetIsEditable())
                                    {
                                        return;
                                    }
                                }
                                switch (m_sprite)
                                {
                                    case SPRITE_TYPE.PLATFORM:
                                        {
                                            SPRITE_STRUCT data = ResourceManager.GetResourceManager().GetSpritedata(SPRITE_TYPE.PLATFORM);


                                            data.x = p.X; data.y = p.Y;
                                            int size = GameManager.GetTileSize();
                                            data.height = data.width = size;
                                            GameObject s = new Platform(data);
                                            ResourceManager.GetResourceManager().AddSprite(s);
                                            break;
                                        }
                                    case SPRITE_TYPE.ENEMY:
                                        {
                                            break;
                                        }

                                }
                            }
                        }
                        else if (KeyMouseReader.RightClickHover())
                        {
                            Point mp = KeyMouseReader.mouseState.Position;
                            Point p = new Point(mp.X, mp.Y);
                            GameManager.ModTileWithRes(ref p);
                            if (GameManager.IsWithinWindowBounds(new Rectangle(mp, new Point(0, 0))))
                            {
                                foreach (GameObject sprite in ResourceManager.GetResourceManager().GetSprites())
                                {
                                    if (sprite.GetBounds().Contains(p) && sprite.GetIsEditable())
                                    {
                                        ResourceManager.GetResourceManager().GetSprites().Remove(sprite);
                                        return;
                                    }
                                }

                            }
                        }
                        break;
                    }
            }
            base.Update(gameTime);
        }
        private void SelectSprite()
        {
            if (KeyMouseReader.KeyPressed(Microsoft.Xna.Framework.Input.Keys.E))
            {
                if ((int)m_sprite < m_spriteTypeCount - 1)
                {
                    m_sprite++;

                }
                else
                {
                    m_sprite = 0;
                }
                if ((int)m_sprite == (int)SPRITE_TYPE.PLAYER)
                {
                    SelectSprite();
                }
            }
        }
    }
    
}