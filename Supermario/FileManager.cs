using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using SuperMario;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace Supermario
{
    internal class FileManager
    {

        Player m_player;
        List<StaticObject> m_blockList = new List<StaticObject>();
        List<StaticObject> m_coinblockList = new List<StaticObject>();
        List<Enemy> m_enemyList = new List<Enemy>();
        List<StaticObject> m_backgroundObjects = new List<StaticObject>();
        JObject m_wholeObj;
        string m_fileName;
        string m_directory;

        public FileManager(string directory)
        {
            m_directory = directory;
        }
        public Player GetPlayer() { return m_player; }
        public List<StaticObject> GetBlocks() { return m_blockList; }
        public List<StaticObject> GetCoinBlocks() { return m_coinblockList; }
        public List<Enemy> GetEnemies() { return m_enemyList; }

        public List<StaticObject> GetBackground() { return m_backgroundObjects; }
        public void ReadFromFile(string fileName)
        {
            //OBJECT_CONSTRUCTION_DATA playerdata = ResourceManager.GetSpritedata(SPRITE_TYPE.PLAYER);
            //OBJECT_CONSTRUCTION_DATA enemydata = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY);
            OBJECT_CONSTRUCTION_DATA coinblockata = ResourceManager.GetSpritedata(SPRITE_TYPE.COINBLOCK);
            OBJECT_CONSTRUCTION_DATA blockata = ResourceManager.GetSpritedata(SPRITE_TYPE.BLOCK);
            OBJECT_CONSTRUCTION_DATA backgroundData = ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND);
            //List<Vector2> enemies = GetPosList(m_directory + fileName, "enemies");
            //foreach (Vector2 pos in enemies)
            //{
            //    enemydata.x = (int)pos.X;
            //    enemydata.y = (int)pos.Y;

            //    Enemy p = new Enemy(enemydata);

            //    m_enemyList.Add(p);
            //}
            List<Vector2> coinblocks = GetPosList(m_directory + fileName, "coinblocks");
            foreach (Vector2 pos in coinblocks)
            {
                coinblockata.x = (int)pos.X;
                coinblockata.y = (int)pos.Y;

                StaticObject p = new StaticObject(coinblockata);

                m_blockList.Add(p);
            }
            List<Vector2> blocks = GetPosList(m_directory + fileName, "blocks");
            foreach (Vector2 pos in blocks)
            {
                blockata.x = (int)pos.X;
                blockata.y = (int)pos.Y;

                StaticObject p = new StaticObject(blockata);
               
                m_blockList.Add(p);
            }
            List<Vector2> background = GetPosList(m_directory + fileName, "backgrounds");
            foreach (Vector2 pos in background)
            {
                backgroundData.x = (int)pos.X;
                backgroundData.y = (int)pos.Y;

                StaticObject n = new StaticObject(backgroundData);
           
                n.SetIsEditable(false);
                m_backgroundObjects.Add(n);
            }
            //Vector2 playerpos = GetPos(m_directory + fileName, "player");
            //playerdata.x = (int)playerpos.X;
            //playerdata.y = (int)playerpos.Y;
            //m_player = new Player(playerdata);
        }
        public void WriteToFile(string fileName, List<GameObject> gameObjectList)
        {


            WriteJsonToFile(m_directory + fileName, gameObjectList);

        }
        private void GetJObjectFromFile(string fileName)
        {
            m_fileName = fileName;
            StreamReader file = File.OpenText(fileName);
            JsonTextReader reader = new JsonTextReader(file);
            m_wholeObj = JObject.Load(reader);
            file.Close();
        }
        private Vector2 GetPos(string fileName, string
        propertyName)
        {
            if (m_wholeObj == null || m_fileName == null ||
            m_fileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }
            JObject obj = (JObject)m_wholeObj.GetValue(propertyName);
            return GetPos(obj);
        }
        private List<Vector2> GetPosList(string fileName, string propertyName)
        {
            if (m_wholeObj == null || m_fileName == null ||
            m_fileName != fileName)
            {
                GetJObjectFromFile(fileName);
            }

            List<Vector2> spritelist = new List<Vector2>();
            JArray arrayObj = (JArray)m_wholeObj.GetValue(propertyName);
            if (arrayObj != null)
            {
                for (int i = 0; i < arrayObj.Count; i++)
                {
                    JObject obj = (JObject)arrayObj[i];
                    Vector2 sprite = GetPos(obj);
                    spritelist.Add(sprite);
                }
            }

            return spritelist;
        }
        private Vector2 GetPos(JObject obj)
        {
            Vector2 position;
            position.X = Convert.ToInt32(obj.GetValue("positionX"));
            position.Y = Convert.ToInt32(obj.GetValue("positionY"));


            return position;
        }
        private void WriteJsonToFile(string filename,
        List<GameObject> gList)
        {
            JArray enemyArray = new JArray();
            JArray blockarray = new JArray();
            JArray coinblockarray = new JArray();
            JArray backgroundarray = new JArray();
            JObject bigobj = new JObject();

            for (int i = 0; i < gList.Count; i++)
            {
                if (gList[i] is Enemy)
                {
                    JObject obj = CreateObject(gList[i]);
                    enemyArray.Add(obj);
                }
                else if (gList[i] is StaticObject)
                {
                    JObject obj = CreateObject(gList[i]);
                    switch(gList[i].GetSpriteType())
                    {
                        case SPRITE_TYPE.BLOCK:
                            {
                                blockarray.Add(obj);
                                break;
                            }
                        case SPRITE_TYPE.BACKGROUND:
                            {
                                backgroundarray.Add(obj);
                                break;
                            }
                        case SPRITE_TYPE.COINBLOCK:
                            {
                                coinblockarray.Add(obj);
                                break;
                            }
                    }
                   



                }
                else if (gList[i] is Player)
                {
                    JObject obj = CreateObject(gList[i]);
                    bigobj.Add("player", obj);
                }

            }
            bigobj.Add("enemies", enemyArray);
            bigobj.Add("blocks", blockarray);
            bigobj.Add("coinblocks", coinblockarray);
            bigobj.Add("backgrounds", backgroundarray);
            //System.Diagnostics.Debug.WriteLine(bigobj.ToString());
            File.WriteAllText(filename, bigobj.ToString());
        }
        private JObject CreateObject(GameObject sprite)
        {
            JObject obj = new JObject();
            obj.Add("positionX", sprite.GetCurrentPos().X);
            obj.Add("positionY", sprite.GetCurrentPos().Y);

            return obj;
        }
    }
}

