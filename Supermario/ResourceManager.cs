using SharpDX.Direct3D9;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1;

namespace Supermario
{
    internal class ResourceManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        OBJECT_CONSTRUCTION_DATA m_spritedata;
        static string m_pipepath = "pipe";
       static string m_coinblockpath = "coinblock";
       static string m_backgroundpath = "background";
       static string m_platformpath = "block";
       static string m_enemiespath = "enemies";
       static string m_marioTinyPath = "marioTiny";
        static string m_timeuppath = "timeup";
        static string m_gameoverpath = "gameover";
        static string m_buttonpath = "button";
        static string m_enemyFire = "enemy";
        static string m_mushroom = "mushroom";
        Texture2D m_coinblockTex;
        Texture2D m_pipeTex;
        Texture2D m_enemiesTex;
        Texture2D m_backrgoundTex;
        Texture2D m_platformtex;
        Texture2D m_marioTinyTex;
        Texture2D m_timeupTex;
        Texture2D m_gameoverTex;
        Texture2D m_buttonTex;
        Texture2D m_enemyFireTex;
        Texture2D m_mushroomTex;
        static List<GameObject> m_buttons = new List<GameObject>();
        static List<GameObject> m_objects = new List<GameObject>();
        static List<StaticObject> m_pipes = new List<StaticObject>();
        static List<StaticObject> m_pickups = new List<StaticObject>();
        static Dictionary<SPRITE_TYPE, OBJECT_CONSTRUCTION_DATA> m_objectData = new Dictionary<SPRITE_TYPE, OBJECT_CONSTRUCTION_DATA>();
        static Dictionary<string, Texture2D> m_textures = new Dictionary<string, Texture2D>();
        static Dictionary<MENU_TYPE, GameObject> m_menuObjects = new Dictionary<MENU_TYPE, GameObject>();
        static Player m_player;
        static List<Enemy> m_enemies = new List<Enemy>();
        public ResourceManager(Game game) : base(game)
        {
            m_spritedata.mass = 1;
            m_spritedata.texture = m_platformpath;
            m_spritedata.fullsheetsizeX = 1;
            m_spritedata.fullSheetsizeY = 1;
            m_spritedata.usedsheetMaxX = 0;
            m_spritedata.usedSheetMaxY = 0;
            m_spritedata.usedsheetMinX = 0;
            m_spritedata.usedSheetMinY = 0;
            m_spritedata.width = m_spritedata.height = 25;
            m_spritedata.type = SPRITE_TYPE.BLOCK;
            m_objectData.Add(SPRITE_TYPE.BLOCK, m_spritedata);
            m_spritedata.texture = m_coinblockpath;
            m_spritedata.type = SPRITE_TYPE.COINBLOCK;
            m_objectData.Add(SPRITE_TYPE.COINBLOCK, m_spritedata);
            m_spritedata.width = 800;
            m_spritedata.height = 600;
            m_spritedata.texture = m_backgroundpath;
            m_spritedata.fullsheetsizeX = 1;
            m_spritedata.fullSheetsizeY = 1;
            m_spritedata.usedsheetMaxX = 0;
            m_spritedata.usedSheetMaxY = 0;
            m_spritedata.usedsheetMinX = 0;
            m_spritedata.usedSheetMinY = 0;
            m_spritedata.type = SPRITE_TYPE.BACKGROUND;
            m_objectData.Add(SPRITE_TYPE.BACKGROUND, m_spritedata);

            
            m_spritedata.texture = m_marioTinyPath;
            m_spritedata.fullsheetsizeX = 10;
            m_spritedata.fullSheetsizeY = 2;
            m_spritedata.usedsheetMaxX = 6;
            m_spritedata.usedSheetMaxY = 0;
            m_spritedata.usedsheetMinX = 0;
            m_spritedata.usedSheetMinY = 0;
            m_spritedata.width = 300;
            m_spritedata.height = 44;
            m_spritedata.speed = 1900;
            m_spritedata.type = SPRITE_TYPE.PLAYER;
            m_spritedata.mass = 10;
            m_objectData.Add(SPRITE_TYPE.PLAYER, m_spritedata);

            m_spritedata.speed = 4400;
            m_spritedata.texture = m_enemyFire;
            m_spritedata.fullsheetsizeX = 1;
            m_spritedata.fullSheetsizeY = 1;
            m_spritedata.usedsheetMaxX = 0;
            m_spritedata.usedSheetMaxY = 0;
            m_spritedata.usedsheetMinX = 0;
            m_spritedata.usedSheetMinY = 0;
            m_spritedata.width = 57;
            m_spritedata.height = 30;
            m_spritedata.mass = 100;
            m_spritedata.type = SPRITE_TYPE.ENEMY;
            m_objectData.Add(SPRITE_TYPE.ENEMY, m_spritedata);

            m_spritedata.texture = m_mushroom;
            m_spritedata.fullsheetsizeX = 1;
            m_spritedata.fullSheetsizeY = 1;
            m_spritedata.usedsheetMaxX = 0;
            m_spritedata.usedSheetMaxY = 0;
            m_spritedata.usedsheetMinX = 0;
            m_spritedata.usedSheetMinY = 0;
            m_spritedata.width = 40;
            m_spritedata.height = 40;
            m_spritedata.type = SPRITE_TYPE.MUSHROOM;
            m_objectData.Add(SPRITE_TYPE.MUSHROOM, m_spritedata);

            

            m_spritedata.texture = m_pipepath;
            m_spritedata.fullsheetsizeX = 1;
            m_spritedata.fullSheetsizeY = 1;
            m_spritedata.usedsheetMaxX = 0;
            m_spritedata.usedSheetMaxY = 0;
            m_spritedata.usedsheetMinX = 0;
            m_spritedata.usedSheetMinY = 0;
            m_spritedata.width = 25;
            m_spritedata.height = 50;
            m_spritedata.mass = 1;
            m_spritedata.type = SPRITE_TYPE.PIPE;
            m_objectData.Add(SPRITE_TYPE.PIPE, m_spritedata);
        }

        
        

        protected override void LoadContent()
        {
            m_buttonTex = Game.Content.Load<Texture2D>(m_buttonpath);
            m_gameoverTex = Game.Content.Load<Texture2D>(m_gameoverpath);
            m_timeupTex = Game.Content.Load<Texture2D>(m_timeuppath);
            m_marioTinyTex = Game.Content.Load<Texture2D>(m_marioTinyPath);
            m_platformtex = Game.Content.Load<Texture2D>(m_platformpath);
            m_coinblockTex = Game.Content.Load<Texture2D>(m_coinblockpath);
            m_backrgoundTex = Game.Content.Load<Texture2D>(m_backgroundpath);
            m_enemyFireTex = Game.Content.Load<Texture2D>(m_enemyFire);
            m_mushroomTex = Game.Content.Load<Texture2D>(m_mushroom);
            m_pipeTex = Game.Content.Load<Texture2D>(m_pipepath);
            m_textures.Add(m_platformpath, m_platformtex);
            m_textures.Add(m_coinblockpath, m_coinblockTex);
            m_textures.Add(m_backgroundpath, m_backrgoundTex);
            m_textures.Add(m_marioTinyPath, m_marioTinyTex);
            m_textures.Add(m_timeuppath, m_timeupTex);
            m_textures.Add(m_gameoverpath, m_gameoverTex);
            m_textures.Add(m_buttonpath, m_buttonTex);
            m_textures.Add(m_enemyFire, m_enemyFireTex);
            m_textures.Add(m_pipepath, m_pipeTex);
            m_textures.Add(m_mushroom, m_mushroomTex);
            base.LoadContent();
        }
        public static ref Player GetPlayer() { return ref m_player; }
     
        public static Texture2D GetTexture(string name) { return m_textures[name]; }
        public static OBJECT_CONSTRUCTION_DATA GetSpritedata(SPRITE_TYPE type) { return m_objectData[type]; }
        public static Dictionary<string, Texture2D> GetTexture() { return m_textures; }
        public static void AddTexture(Texture2D tex, string name) { m_textures.Add(name, tex); }
        public static void AddObject(OBJECT_CONSTRUCTION_DATA data, SPRITE_TYPE type) {m_objectData.Add(type, data); }
        public static void SetObjectData(OBJECT_CONSTRUCTION_DATA data, SPRITE_TYPE type)
        {
            m_objectData[type] = data;
        }
        public static void AddObject(GameObject s) { 
            if(!m_objects.Contains(s))
                m_objects.Add(s);
            if (s is Player)
                m_player = s as Player;
            if (s is Enemy)
                m_enemies.Add(s as Enemy);
            if(s.GetSpriteType()==SPRITE_TYPE.PIPE)
            {
                m_pipes.Add(s as StaticObject);
            }
            else if (s.GetSpriteType() == SPRITE_TYPE.MUSHROOM)
            {
                m_pickups.Add(s as StaticObject);
            }

        }
        public static ref List<StaticObject> GetMushrooms() { return ref m_pickups; }
        public static ref List<StaticObject> GetPipes() { return ref m_pipes; }
        public static ref List<GameObject> GetObjects() { return ref m_objects; }
        public static ref List<Enemy> GetEnemies() { return ref m_enemies; }
        public static ref List<GameObject> GetButtons() { return ref m_buttons; }
        public static void AddMenuObject( GameObject s, MENU_TYPE type) { s.SetIsEditable(false);
            switch (type)
            {
                case MENU_TYPE.TIMEUP:
                    {
                        s.SetTexture(m_timeuppath);
                        m_menuObjects.Add(type, s);
                        break;
                    }
                case MENU_TYPE.GAMEOVER:
                    {
                        s.SetTexture(m_gameoverpath);
                        m_menuObjects.Add(type, s);
                        break;
                    }
                case MENU_TYPE.START:
                    {
                        s.SetTexture(m_backgroundpath);
                        m_menuObjects.Add(type, s);
                        break;
                    }
                case MENU_TYPE.BUTTON:
                    {
                        s.SetTexture(m_buttonpath);
                        s.SetColor(Color.DarkGoldenrod);
                        m_buttons.Add(s);


                        break;
                    }
            }
            
            
        }
        public static ref Dictionary<MENU_TYPE, GameObject> GetMenuObjects() { return ref m_menuObjects; }
        
    }
}
