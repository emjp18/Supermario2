using Microsoft.Xna.Framework;
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
    internal class GameManager
    {
        static QUAD_NODE m_root;
        const int m_maxGridPoints = 10;
        static Vector2 m_playerStart;
        static LEVEL_TYPE m_oldLevel;
        static GAME_STATE m_oldState = GAME_STATE.MENU;
        static GAME_STATE m_currentState = GAME_STATE.MENU;
        static LEVEL_TYPE m_currentLevel;
        FileManager m_filemanager;
        GameObjectManager m_gameobjectManager;
        //SoundManager m_soundmanager;
        ResourceManager m_resourcemanager;
        LevelManager m_levelmanager;
        const string m_directory = "../../../";
        static int m_resX;
        static int m_resY;
        static int m_windowSizeX;
        static int m_windowSizeY;
        const int m_tileSize = 25;
        static int m_tileCountX;
        static int m_tileCountY;
        string m_levelEditor = "levelE.json";
        string m_level1 = "level1.json";
        string m_level2 = "level2.json";
        string m_level3 = "level3.json";
        static A_STAR_NODE[,] m_grid;
        Dictionary<LEVEL_TYPE, string> m_levels = new Dictionary<LEVEL_TYPE, string>();
        public GameManager(Game game, int resX, int resY)
        {
            m_resourcemanager = new ResourceManager(game);
            m_filemanager = new FileManager(m_directory);
            //m_soundmanager = new SoundManager(game);
            m_gameobjectManager = new GameObjectManager(game);
            m_levelmanager = new LevelManager(game, LEVEL_TYPE.LEVELE);
            m_levels.Add(LEVEL_TYPE.LEVELE, m_levelEditor);
            m_levels.Add(LEVEL_TYPE.LEVEL1, m_level1);
            m_resX = resX;
            m_resY = resY;
            m_currentLevel = LEVEL_TYPE.NONE;
            m_oldLevel = LEVEL_TYPE.NONE;
            

            m_windowSizeX = (int)(resX * 1.5f);
            m_windowSizeY = (int)(resY * 1.5f);

            m_tileCountX = m_windowSizeX / m_tileSize;
            m_tileCountY = m_windowSizeY / m_tileSize;

            m_root = new QUAD_NODE();
            
            m_root.bounds = new Rectangle(0, 0, m_windowSizeX, m_windowSizeY);
            m_root.leaf = false;

           

        }
        public static QUAD_NODE GetRootNode() { return m_root; }
        public static A_STAR_NODE[,] GetGrid() { return m_grid; }
        public static int GetTileSize() { return m_tileSize; }
        public static int GetTileCount(bool x) { if (x) return m_tileCountX; else return m_tileCountY; }
        public static int GetRes(bool x) { if (x) return m_resX; else return m_resY; }
        public static int GetWindowSize(bool x) { if (x) return m_windowSizeX; else return m_windowSizeY; }
        public GameObjectManager GetGameObjectManager() { return m_gameobjectManager; }
        //public SoundManager GetSoundManager() { return m_soundmanager; }
        public FileManager GetFileManager() { return m_filemanager; }
        public ResourceManager GetResourceManager() { return m_resourcemanager; }
        public LevelManager GetLevelManager() { return m_levelmanager; }
        public static Vector2 GetPlayerStart() { return m_playerStart; }
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
        public void LoadLevel(LEVEL_TYPE level, Viewport viewport)
        {

         
            m_filemanager.ReadFromFile(m_levels[level]);
            ResourceManager.GetObjects().Clear();
            ResourceManager.GetEnemies().Clear();
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
            
            var data = ResourceManager.GetSpritedata(SPRITE_TYPE.PLAYER);
            m_playerStart = new Vector2(0, m_windowSizeY - (m_tileSize+(data.height / data.fullSheetsizeY)));
            data.x = (int)m_playerStart.X;
            data.y = (int)m_playerStart.Y;
            Player p = new Player(data, viewport);
          
            ResourceManager.AddObject(p);
          
            GenerateGrid();
            GenerateQuadTree(ref m_root);
            m_levelmanager.SetLevelType(level);
        }
        
        public static bool IsWithinWindowBounds(Rectangle rect)
        {
            if ((rect.X + rect.Width <= m_windowSizeX) && (rect.Y + rect.Height <= m_windowSizeY)
                && (rect.X >= 0) && (rect.Y >= 0))
                return true;
            else
                return false;
        }
        public static void ModWithRes(ref Point p)
        {
            
            int rest = m_windowSizeX % (p.X+1);
            if (rest != 0)
            {
                p.X = (p.X / m_tileSize) * m_tileSize;
            }
            rest = m_windowSizeY % (p.Y + 1);
            if (rest != 0)
            {
                p.Y = (p.Y / m_tileSize) * m_tileSize;

            }
        }
        public static void ModWithRes(ref Point p, int w, int h)
        {

            int rest = m_windowSizeX % (p.X + 1);
            if (rest != 0)
            {
                p.X = (p.X / w) * w;
            }
            rest = m_windowSizeY % (p.Y + 1);
            if (rest != 0)
            {
                p.Y = (p.Y / h) * h;

            }
        }
       
        public static void GetGridPoint(Vector2 pos, QUAD_NODE node, ref QUAD_NODE outNode)
        {

            if (node.bounds.Contains(pos))
            {
                if (node.leaf)
                {
                    outNode = node;
                    return;
                    
                }
                else
                {
                    if (node.children != null)
                    {
                        for (int i = 0; i < 4; i++)
                        {
                            GetGridPoint(pos, node.children[i], ref outNode);
                        }
                    }

                }
            }
            return;
        }
        public static LEVEL_TYPE GetCurrentLevel() { return m_currentLevel; }
        public static LEVEL_TYPE GetOldLevel() { return m_oldLevel; }
        public static void SetOldLevel(LEVEL_TYPE level) { m_oldLevel = level; }
        public void SaveLevel()
        {

            m_filemanager.WriteToFile("levelE.json", ResourceManager.GetObjects());
        }
        public static GAME_STATE GetState() { return m_currentState; }
        public static GAME_STATE GetOldState() { return m_oldState; }
        public static void SetOldState(GAME_STATE s) { m_oldState = s; }
        public static void SetState(GAME_STATE state) { m_currentState = state; }
        public static void SetLevel(LEVEL_TYPE level) { m_currentLevel = level; }
        private void AddNeighbours(A_STAR_NODE node)
        {
            int x = (int)node.pos.X / m_tileCountX;
            int y = (int)node.pos.Y / m_tileCountY;

            if (x < (m_tileCountX - 1)) //Precomputed since the map doesnt change.
                node.neighbours.Add(GetGrid()[x + 1, y]);
            if (x > 0)
                node.neighbours.Add(GetGrid()[x - 1, y]);
            if (y < (m_tileCountY - 1))
                node.neighbours.Add(GetGrid()[x, y + 1]);
            if (y > 0)
                node.neighbours.Add(GetGrid()[x, y - 1]);
        }
        private void GenerateQuadTree(ref QUAD_NODE node)
        {
            int gpCount = 0;
            node.leaf = false;
 
            for (int i = 0; i < m_tileCountX; i++)
            {
                for (int j = 0; j < m_tileCountY; j++)
                {
                    Point gp = new Point(i * m_tileSize, j * m_tileSize);
                    if(node.bounds.Contains(gp))
                    {
                        gpCount++;
                        if(gpCount > m_maxGridPoints)
                        {
                            node.children = new QUAD_NODE[4];
                            node.children[0] = new QUAD_NODE();
                       
                            node.children[1] = new QUAD_NODE();
                       
                            node.children[2] = new QUAD_NODE();
                      
                            node.children[3] = new QUAD_NODE();
                         
                            
                            node.children[0].bounds = new Rectangle(node.bounds.X, node.bounds.Y,
                                    node.bounds.Width / 2, node.bounds.Height / 2);
                            node.children[1].bounds = new Rectangle(node.bounds.X+ node.bounds.Width / 2, node.bounds.Y,
                                    node.bounds.Width / 2, node.bounds.Height / 2);
                            node.children[2].bounds = new Rectangle(node.bounds.X+ node.bounds.Width / 2, node.bounds.Y+ node.bounds.Height / 2,
                                    node.bounds.Width / 2, node.bounds.Height / 2);
                            node.children[3].bounds = new Rectangle(node.bounds.X, node.bounds.Y+ node.bounds.Height / 2,
                                    node.bounds.Width / 2, node.bounds.Height / 2);

                            GenerateQuadTree(ref node.children[0]);
                            GenerateQuadTree(ref node.children[1]);
                            GenerateQuadTree(ref node.children[2]);
                            GenerateQuadTree(ref node.children[3]);
                            return;
                        }
                    }
                }

            }
            node.leaf = true;
            node.gridpoints = new List<Point>();
            node.tiles = new List<StaticObject>();
            for (int i = 0; i < m_tileCountX; i++)
            {
                for (int j = 0; j < m_tileCountY; j++)
                {
                    Point gp = new Point(i * m_tileSize, j * m_tileSize);
                    if (node.bounds.Contains(gp))
                    {
                        node.gridpoints.Add(gp);
                        
                    }
                    
                }

            }
            foreach (GameObject obj in ResourceManager.GetObjects())
            {
                if (obj is StaticObject)
                {
                    if (node.bounds.Contains(obj.GetCurrentPos()))
                    {
                        node.tiles.Add(obj as StaticObject);
                    }
                }

            }

        }
        private void GenerateGrid()
        {
            
            m_grid = new A_STAR_NODE[m_tileCountX, m_tileCountY];
            for (int i = 0; i < m_tileCountX; i++)
            {
                for (int j = 0; j < m_tileCountY; j++)
                {
                    m_grid[i, j] = new A_STAR_NODE();
                    m_grid[i, j].pos = new Vector2(i * m_tileSize, j * m_tileSize);
                    QUAD_NODE node = new QUAD_NODE();
                    Vector2 pos = new Vector2(i * m_tileSize, j * m_tileSize);
                    GetGridPoint(pos, m_root, ref node);
                    bool poshastile = false;
                    if(node.tiles!=null)
                    {
                        foreach(StaticObject obj in node.tiles)
                        {
                            if(obj.GetBounds().Contains(pos))
                            {
                                poshastile = true;
                                break;
                            }
                        }
                    }
                    m_grid[i, j].obstacle = poshastile;
                   
                    m_grid[i, j].previous = new A_STAR_NODE[1];
                    m_grid[i, j].neighbours = new List<A_STAR_NODE>();
                    m_grid[i, j].isActive = true;
                    m_grid[i, j].previous[0] = new A_STAR_NODE();
                    m_grid[i, j].previous[0].isActive = false;
                    m_grid[i, j].gridpos = new Point(i, j);

                }
            }
            for (int i = 0; i < m_tileCountX; i++)
            {
                for (int j = 0; j < m_tileCountY; j++)
                {
                    AddNeighbours(m_grid[i, j]);
                }
            }
        }
        
    }
}

