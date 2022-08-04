using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace CPM_converter
{
    class texturemanager
    {
        List<(string, int, int)> texlist; //name, texoffset, actual_res)
        public int smallest_res;
        public int height;
        public int weidth;

        public texturemanager()
        {
            texlist = new List<(string, int, int)>();
            smallest_res = 0;
            height = 0;
            weidth = 0;
        }

        public int addText(string texName, int size)
        {
            if (texName.EndsWith("skin.png")) return -1;
            Image img = Image.FromFile(texName);
            if (img == null) return -1;
            int res = img.Height / size;
            if (size == -1)
            {
                int index = texlist.FindIndex(a => a.Item1 == texName);
                if (index != -1)
                {
                    return index;
                }
                res = 1;
            }
            (string, int, int) result = texlist.Find(a => a.Item1 == texName && a.Item3 == res);
            if (result.Item3 != 0) return texlist.IndexOf(result);
            if (res > smallest_res && texlist.Count != 0)
            {
                height *= res / smallest_res;
                weidth *= res / smallest_res;
                for (int i = 0; i < texlist.Count; i++)
                {
                    (string, int, int) tex = texlist[i];
                    texlist[i] = (tex.Item1, tex.Item2 * res / smallest_res, tex.Item3);
                }
            }
            texlist.Add((texName, weidth, res));
            smallest_res = Math.Max(smallest_res, res);
            height = Math.Max(height, img.Height / res * smallest_res);
            weidth += img.Width / res * smallest_res;
            return texlist.Count - 1;
        }

        public int getOffset(int index)
        {
            return texlist[index].Item2/smallest_res;
        }

        public void export_tex(string path)
        {
            Bitmap bmp = new Bitmap(weidth, height);
            Graphics graphics = Graphics.FromImage(bmp);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;
            graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            foreach (var tex in texlist)
            {
                Bitmap bitmap = new Bitmap(tex.Item1);
                graphics.DrawImage(bitmap, tex.Item2, 0, bitmap.Width * smallest_res / tex.Item3, bitmap.Height * smallest_res / tex.Item3);
            }
            graphics.Save();
            bmp.Save(path, System.Drawing.Imaging.ImageFormat.Png);
        }
    }
}
