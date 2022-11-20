using SharpDX.Direct3D9;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Supermario
{
    internal class ResourceManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public ResourceManager(Game game) : base(game)
        {
        }

        
        

        protected override void LoadContent()
        {
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
