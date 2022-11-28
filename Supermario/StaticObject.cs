using Microsoft.Xna.Framework.Graphics;
using SuperMario;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Supermario
{
    public  class StaticObject : GameObject
    {
        public StaticObject(OBJECT_CONSTRUCTION_DATA constructiondata) : base(constructiondata)
        {
            m_update = false;
        }
    }
}
