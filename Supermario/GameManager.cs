using Microsoft.Xna.Framework;
using SharpDX.Direct3D9;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermario
{
    internal class GameManager
    {
        Vector2 m_playerStart;
        LEVEL_TYPE m_oldLevel;
        GAME_STATE m_oldState = GAME_STATE.MENU;
        static GAME_STATE m_currentState = GAME_STATE.MENU;
        static LEVEL_TYPE m_currentLevel;
        FileManager m_filemanager;
        GameObjectManager m_gameobjectManager;
        //SoundManager m_soundmanager;
        ResourceManager m_resourcemanager;
        LevelManager m_levelmanager;
        const string m_directory = "../../../";
        const int m_resX = 800;
        const int m_resY = 600;
        const int m_tileSize = 25;
        string m_levelEditor = "levelE.json";
        string m_level0 = "level1.json";
        string m_level1 = "level2.json";
        Dictionary<LEVEL_TYPE, string> m_levels = new Dictionary<LEVEL_TYPE, string>();
        public GameManager(Game game)
        {
            m_resourcemanager = new ResourceManager(game);
            m_filemanager = new FileManager(m_directory);
            //m_soundmanager = new SoundManager(game);
            m_gameobjectManager = new GameObjectManager(game);
            m_levelmanager = new LevelManager(game, LEVEL_TYPE.LEVELE);
            m_levels.Add(LEVEL_TYPE.LEVELE, m_levelEditor);
            m_levels.Add(LEVEL_TYPE.LEVEL1, m_level0);
            m_levels.Add(LEVEL_TYPE.LEVEL2, m_level1);
           
            m_currentLevel = LEVEL_TYPE.NONE;
            m_oldLevel = LEVEL_TYPE.NONE;
        }
        public static int GetTileSize() { return m_tileSize; }
        public static int GetRes(bool x) { if (x) return m_resX; else return m_resY; }
        public GameObjectManager GetGameObjectManager() { return m_gameobjectManager; }
        //public SoundManager GetSoundManager() { return m_soundmanager; }
        public FileManager GetFileManager() { return m_filemanager; }
        public ResourceManager GetResourceManager() { return m_resourcemanager; }
        public LevelManager GetLevelManager() { return m_levelmanager; }
        public void LoadMenuObjects()
        {
           
            ResourceManager.GetMenuObjects().Clear();            
            ResourceManager.AddMenuObject(new StaticObject(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.GAMEOVER);
            ResourceManager.AddMenuObject(new StaticObject(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.TIMEUP);
            ResourceManager.AddMenuObject(new StaticObject(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.START);
            ResourceManager.AddMenuObject(new Button(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.BUTTON);
            ResourceManager.AddMenuObject(new Button(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.BUTTON);
            ResourceManager.AddMenuObject(new Button(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.BUTTON);
            ResourceManager.AddMenuObject(new Button(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.BUTTON);
            ResourceManager.AddMenuObject(new Button(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.BUTTON);
            ResourceManager.AddMenuObject(new Button(ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND)), MENU_TYPE.BUTTON);
        }
        public void LoadLevel(LEVEL_TYPE level)
        {
            
            m_currentLevel = level;
            m_filemanager.ReadFromFile(m_levels[level]);
            ResourceManager.GetObjects().Clear();
            foreach (GameObject s in m_filemanager.GetBackground())
            {
                ResourceManager.AddObject(s);

            }
            foreach (GameObject s in m_filemanager.GetBlocks())
            {
                ResourceManager.AddObject(s);

            }
            foreach (GameObject s in m_filemanager.GetCoinBlocks())
            {
                ResourceManager.AddObject(s);

            }
            foreach (GameObject s in m_filemanager.GetEnemies())
            {
                ResourceManager.AddObject(s);

            }
            m_playerStart = new Vector2(0, m_resY - (m_tileSize * 2));
            Player p = m_filemanager.GetPlayer();
            p.SetPos(m_playerStart);
            ResourceManager.AddObject(p);
            m_levelmanager.SetLevelType(level);
        }
        
        public static bool IsWithinWindowBounds(Rectangle rect)
        {
            if ((rect.X + rect.Width <= m_resX) && (rect.Y + rect.Height <= m_resY)
                && (rect.X > 0) && (rect.Y > 0))
                return true;
            else
                return false;
        }
        public static void ModTileWithRes(ref Point p)
        {


            int rest = m_resX % p.X;
            if (rest != 0)
            {
                p.X = (p.X / m_tileSize) * m_tileSize;
            }
            rest = m_resY % p.Y;
            if (rest != 0)
            {
                p.Y = (p.Y / m_tileSize) * m_tileSize;

            }
        }
        public static LEVEL_TYPE GetCurrentLevel() { return m_currentLevel; }
        public void SaveLevel()
        {

            m_filemanager.WriteToFile("levelE.json", ResourceManager.GetObjects());
        }
        public static GAME_STATE GetState() { return m_currentState; }
        public static void SetState(GAME_STATE state) { m_currentState = state; }
        public static void SetLevel(LEVEL_TYPE level) { m_currentLevel = level; }
        public void Update()
        {
            
            if(m_currentState == GAME_STATE.GAME||m_currentState == GAME_STATE.EDITOR)
            {
                if (m_oldLevel != m_currentLevel)
                {
                    LoadLevel(m_currentLevel);
                    m_oldLevel = m_currentLevel;
                }
                if(m_currentState == GAME_STATE.EDITOR)
                {
                    foreach (Player obj in ResourceManager.GetObjects())
                    {
                        obj.SetShouldUpdate(false);
                        break;
                    }
                }
                else
                {
                    foreach (Player obj in ResourceManager.GetObjects())
                    {
                        obj.SetShouldUpdate(true);
                        break;
                    }

                }
            }
           
            if(m_oldState != m_currentState)
            {
                m_oldState = m_currentState;
            }
           
        }
    }
}

