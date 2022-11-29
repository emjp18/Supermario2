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

        List<StaticObject> m_pipeList = new List<StaticObject>();
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
        public List<StaticObject> GetPipes() { return m_pipeList; }
        public List<StaticObject> GetBlocks() { return m_blockList; }
        public List<StaticObject> GetCoinBlocks() { return m_coinblockList; }
        public List<Enemy> GetEnemies() { return m_enemyList; }

        public List<StaticObject> GetBackground() { return m_backgroundObjects; }
        public void ReadFromFile(string fileName)
        {
            m_blockList.Clear();
            m_coinblockList.Clear();
            m_enemyList.Clear();
            m_backgroundObjects.Clear();
            m_pipeList.Clear();
            OBJECT_CONSTRUCTION_DATA pipeData = ResourceManager.GetSpritedata(SPRITE_TYPE.PIPE);
            OBJECT_CONSTRUCTION_DATA enemydata0 = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY0);
            OBJECT_CONSTRUCTION_DATA enemydata1 = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY1);
            OBJECT_CONSTRUCTION_DATA enemydata2 = ResourceManager.GetSpritedata(SPRITE_TYPE.ENEMY2);
            OBJECT_CONSTRUCTION_DATA coinblockata = ResourceManager.GetSpritedata(SPRITE_TYPE.COINBLOCK);
            OBJECT_CONSTRUCTION_DATA blockata = ResourceManager.GetSpritedata(SPRITE_TYPE.BLOCK);
            OBJECT_CONSTRUCTION_DATA backgroundData = ResourceManager.GetSpritedata(SPRITE_TYPE.BACKGROUND);
            List<Vector2> enemies = GetPosList(m_directory + fileName, "enemies0");
            foreach (Vector2 pos in enemies)
            {
                enemydata0.x = (int)pos.X;
                enemydata0.y = (int)pos.Y;

                Enemy p = new Enemy(enemydata0);

                m_enemyList.Add(p);
            }
            List<Vector2> enemies1 = GetPosList(m_directory + fileName, "enemies1");
            foreach (Vector2 pos in enemies1)
            {
                enemydata1.x = (int)pos.X;
                enemydata1.y = (int)pos.Y;

                Enemy p = new Enemy(enemydata1);

                m_enemyList.Add(p);
            }
            List<Vector2> enemies2 = GetPosList(m_directory + fileName, "enemies2");
            foreach (Vector2 pos in enemies2)
            {
                enemydata2.x = (int)pos.X;
                enemydata2.y = (int)pos.Y;

                Enemy p = new Enemy(enemydata2);

                m_enemyList.Add(p);
            }
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
            List<Vector2> pipes = GetPosList(m_directory + fileName, "pipes");
            foreach (Vector2 pos in pipes)
            {
                pipeData.x = (int)pos.X;
                pipeData.y = (int)pos.Y;

                StaticObject n = new StaticObject(pipeData);

               
                m_pipeList.Add(n);
            }
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
            JArray pipeArray0 = new JArray();
            JArray enemyArray0 = new JArray();
            JArray enemyArray1 = new JArray();
            JArray enemyArray2 = new JArray();
            JArray blockarray = new JArray();
            JArray coinblockarray = new JArray();
            JArray backgroundarray = new JArray();
            JObject bigobj = new JObject();

            for (int i = 0; i < gList.Count; i++)
            {
                if (gList[i] is Enemy)
                {
                    JObject obj = CreateObject(gList[i]);
                    
                    switch (gList[i].GetSpriteType())
                    {
                        case SPRITE_TYPE.ENEMY0:
                            {
                                enemyArray0.Add(obj);
                                break;
                            }
                        case SPRITE_TYPE.ENEMY1:
                            {
                                enemyArray1.Add(obj);
                                break;
                            }
                        case SPRITE_TYPE.ENEMY2:
                            {
                                enemyArray2.Add(obj);
                                break;
                            }
                    }
                    
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
                        case SPRITE_TYPE.PIPE:
                            {
                                pipeArray0.Add(obj);
                                break;
                            }
                    }
                   



                }
                

            }
          
            bigobj.Add("enemies0", enemyArray0);
            bigobj.Add("enemies1", enemyArray1);
            bigobj.Add("enemies2", enemyArray2);
            bigobj.Add("blocks", blockarray);
            bigobj.Add("coinblocks", coinblockarray);
            bigobj.Add("pipes", pipeArray0);
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

