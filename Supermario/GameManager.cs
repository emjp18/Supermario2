using Microsoft.Xna.Framework;
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
        DO_ONCE m_doOnce = DO_ONCE.DO;
        FileManager m_filemanager;
        GameObjectManager m_gameobjectManager;
        //SoundManager m_soundmanager;
        LevelManager m_levelmanager;
        const string m_directory = "../../../";
        const int m_resX = 800;
        const int m_resY = 600;
        const int m_tileSize = 25;
        string m_levelEditor = "levelE.json";
        string m_level0 = "level0.json";
        string m_level1 = "level1.json";
        Dictionary<LEVEL_TYPE, string> m_levels = new Dictionary<LEVEL_TYPE, string>();
        public GameManager(Game game)
        {
            m_filemanager = new FileManager(m_directory);
            //m_soundmanager = new SoundManager(game);
            m_gameobjectManager = new GameObjectManager(game);
            m_levelmanager = new LevelManager(game, LEVEL_TYPE.LEVELE);
            m_levels.Add(LEVEL_TYPE.LEVELE, m_levelEditor);
            m_levels.Add(LEVEL_TYPE.LEVEL0, m_level0);
            m_levels.Add(LEVEL_TYPE.LEVEL1, m_level1);
            LoadLevel(LEVEL_TYPE.LEVELE);

        }
        public static int GetTileSize() { return m_tileSize; }
        public static int GetRes(bool x) { if (x) return m_resX; else return m_resY; }
        public GameObjectManager GetGameObjectManager() { return m_gameobjectManager; }
        //public SoundManager GetSoundManager() { return m_soundmanager; }
        public FileManager GetFileManager() { return m_filemanager; }
        public LevelManager GetLevelManager() { return m_levelmanager; }
        public void LoadLevel(LEVEL_TYPE level)
        {
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
            ResourceManager.AddObject(m_filemanager.GetPlayer());
            m_levelmanager.SetLevelType(level);
        }
        public void UpdateGameState()
        {
            if (m_doOnce == DO_ONCE.DO)
            {

                m_doOnce = DO_ONCE.DONT;
            }
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
        public void SaveLevel()
        {

            m_filemanager.WriteToFile("levelE.json", ResourceManager.GetObjects());
        }
    }
}

