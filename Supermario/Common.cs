using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
namespace SuperMario
{
    public enum GAME_STATE { GAME, MENU, HIGHSCORE};
    public enum LEVEL_TYPE { LEVEL0, LEVEL1, LEVEL2, LEVELE };
    public enum SPRITE_TYPE { PLAYER, BLOCK, ENEMY, BACKGROUND, COINBLOCK };
    public enum DO_ONCE { DO, DONT };

    public struct OBJECT_CONSTRUCTION_DATA
    {
        public int x;
        public int y;
        public int height;
        public int width;
        public string texture;
        public int usedsheetX;
        public int usedSheetY;
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