using System;
using System.Collections.Generic;
using System.Text;

namespace CPM_converter
{
    class texturemanager
    {
        Dictionary<string, (int, int, int)> texlist;
        int nextoffset;

        public texturemanager()
        {
            nextoffset = 0;
            texlist = new Dictionary<string, (int, int, int)>();
        }

        public void addText(string texName, int res)
        {
            if (texlist.ContainsKey(texName)) return;
            texlist.Add(texName, (nextoffset, res, res));
        }

        public int getOffset(string texName)
        {
            return texlist.ContainsKey(texName) ? texlist[texName].Item1 : throw new Exception("a texture appeard mid conversion. what?");
        }
    }
}
