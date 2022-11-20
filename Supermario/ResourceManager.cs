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
        string m_cloudpath;
        string m_coinblockpath = "coinblock";
        string m_backgroundpath = "background";
        string m_platformpath = "block";
        string m_variouspath = "smbvarious";
        string m_marioTinyPath = "marioTiny";
        Texture2D m_coinblockTex;
        Texture2D m_cointex;
        Texture2D m_backrgoundTex;
        Texture2D m_platformtex;
        Texture2D m_marioTinyTex;
        public ResourceManager(Game game) : base(game)
        {
            m_spritedata.texture = m_platformpath;
            m_spritedata.fullsheetsizeX = 1;
            m_spritedata.fullSheetsizeY = 1;
            m_spritedata.usedsheetX = 0;
            m_spritedata.usedSheetY = 0;
            m_spritedata.width = m_spritedata.height = GameManager.GetTileSize();
            m_spritedata.type = SPRITE_TYPE.BLOCK;
            m_objectData.Add(SPRITE_TYPE.BLOCK, m_spritedata);
            m_spritedata.texture = m_coinblockpath;
            m_spritedata.type = SPRITE_TYPE.COINBLOCK;
            m_objectData.Add(SPRITE_TYPE.COINBLOCK, m_spritedata);
            m_spritedata.width = GameManager.GetRes(true);
            m_spritedata.height = GameManager.GetRes(false);
            m_spritedata.texture = m_backgroundpath;
            m_spritedata.fullsheetsizeX = 1;
            m_spritedata.fullSheetsizeY = 1;
            m_spritedata.usedsheetX = 0;
            m_spritedata.usedSheetY = 0;
            m_spritedata.type = SPRITE_TYPE.BACKGROUND;
            m_objectData.Add(SPRITE_TYPE.BACKGROUND, m_spritedata);

            m_spritedata.texture = m_marioTinyPath;
            m_spritedata.fullsheetsizeX = 10;
            m_spritedata.fullSheetsizeY = 2;
            m_spritedata.usedsheetX =6;
            m_spritedata.usedSheetY =0;
            m_spritedata.width = 300;
            m_spritedata.height = 44;
            m_spritedata.type = SPRITE_TYPE.PLAYER;
            m_objectData.Add(SPRITE_TYPE.PLAYER, m_spritedata);
        }

        
        

        protected override void LoadContent()
        {
            m_marioTinyTex = Game.Content.Load<Texture2D>(m_marioTinyPath);
            m_platformtex = Game.Content.Load<Texture2D>(m_platformpath);
            m_coinblockTex = Game.Content.Load<Texture2D>(m_coinblockpath);
            m_backrgoundTex = Game.Content.Load<Texture2D>(m_backgroundpath);
            m_textures.Add(m_platformpath, m_platformtex);
            m_textures.Add(m_coinblockpath, m_coinblockTex);
            m_textures.Add(m_backgroundpath, m_backrgoundTex);
            m_textures.Add(m_marioTinyPath, m_marioTinyTex);
            base.LoadContent();
        }

       
        static List<GameObject> m_objects = new List<GameObject>();
        static Dictionary<SPRITE_TYPE, OBJECT_CONSTRUCTION_DATA> m_objectData = new Dictionary<SPRITE_TYPE, OBJECT_CONSTRUCTION_DATA>();
        static Dictionary<string, Texture2D> m_textures = new Dictionary<string, Texture2D>();
        public static Texture2D GetTexture(string name) { return m_textures[name]; }
        public static OBJECT_CONSTRUCTION_DATA GetSpritedata(SPRITE_TYPE type) { return m_objectData[type]; }
        public static Dictionary<string, Texture2D> GetTexture() { return m_textures; }
        public static void AddTexture(Texture2D tex, string name) { m_textures.Add(name, tex); }
        public static void AddObject(OBJECT_CONSTRUCTION_DATA data, SPRITE_TYPE type) {m_objectData.Add(type, data); }
        public static void SetObjectData(OBJECT_CONSTRUCTION_DATA data, SPRITE_TYPE type)
        {
            m_objectData[type] = data;
        }
        public static void AddObject(GameObject s) { m_objects.Add(s); }
        public static ref List<GameObject> GetObjects() { return ref m_objects; }
    }
}
