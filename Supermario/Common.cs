using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SuperMario
{
    public enum GAME_STATE { GAME, MENU,EDITOR, HIGHSCORE};
    public enum LEVEL_TYPE { LEVEL1, LEVEL2, LEVEL3, LEVELE, NONE };
    public enum SPRITE_TYPE { PLAYER, BLOCK, ENEMY0, ENEMY1, ENEMY2, BACKGROUND, COINBLOCK };
    public enum MENU_TYPE { TIMEUP, GAMEOVER, START, BUTTON};
    public enum BUTTON_TYPE { LEVEL1= 0, LEVEL2 = 1, LEVEL3 = 2, CUSTOM = 3,HS = 4, EDITOR = 5}

    public struct OBJECT_CONSTRUCTION_DATA
    {
        public int x;
        public int y;
        public int height;
        public int width;
        public string texture;
        public int usedsheetMaxX;
        public int usedSheetMaxY;
        public int usedsheetMinX;
        public int usedSheetMinY;
        public int fullsheetsizeX;
        public int fullSheetsizeY;
        public int animationSpeedMSperFrame;
        public float speed;
        public float mass;
        public SPRITE_TYPE type;
    }
    public struct A_STAR_NODE
    {
        public bool isActive;
        public A_STAR_NODE[] previous;
        public Vector2 pos;
        public float g;
        public float h;
        public float f;
        public bool obstacle;
        public bool openSet;
        public bool closedSet;
        public bool correctPath;
        public List<A_STAR_NODE> neighbours;


        public override bool Equals(object obj)
        {
            var b = (A_STAR_NODE)obj;
            return pos == b.pos;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 1430287;

                hash = hash * 7302013 ^ pos.X.GetHashCode();
                hash = hash * 7302013 ^ pos.Y.GetHashCode();
                return hash;
            }
        }
    }
    public class A_STAR_NODEComparer : IComparer<A_STAR_NODE>
    {
        public int Compare(A_STAR_NODE x, A_STAR_NODE y)
        {

            return x.f.CompareTo(y.f);
        }
    }
}