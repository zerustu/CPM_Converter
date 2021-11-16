using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace CPM_converter
{
    class Program
    {
        public static string texturename = "N/A";
        public static parameters skeletonparam;
        public static Dictionary<string, float> variable = new Dictionary<string, float>() { { "is_sneaking", 0 }, { "is_first_person", 0 }, { "age", 0 } };
        public static int[] texturesize = null;
        static void Main(string[] args)
        {
            string path = "";
            if (args.Length > 0)
            {
                path = args[0];
            }
            while (!File.Exists(path))
            {
                Console.WriteLine("please enter the file :");
                path = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("the file was not found.");
            }
            Console.Clear();
            Console.WriteLine("loading the model.");
            string jsonmodel = File.ReadAllText(path);
            model themodel = JsonConvert.DeserializeObject<model>(jsonmodel);
            if (themodel.skeleton == null)
            {
                themodel.skeleton = new skel();
            }
            themodel.skeleton.fix();
            skeletonparam = new parameters(themodel.skeleton);
            Console.WriteLine("loading variables and tickvar.");
            foreach (var item in themodel.variables)
            {
                variable.Add("var."+item.Key, CPM_converter.convertlist.stringToDoble(item.Value));
            }
            foreach (var item in themodel.tickVars)
            {
                float v = CPM_converter.convertlist.stringToDoble(item.Value[1]);
                variable.Add("tvp" + item.Key, v);
                variable.Add("tvl" + item.Key, v);
                variable.Add("tvc" + item.Key, v);
            }
            Console.WriteLine("loading the bones...");
            newbone[] boneslist = new newbone[themodel.bones.Length];
            for (int i = 0; i < themodel.bones.Length; i++)
            {
                boneslist[i] = new newbone(themodel.bones[i]);
            }
            newmodel newmod = new newmodel(themodel, skeletonparam);
            Console.WriteLine("calculating new coordinates");
            List<newbone> newbones = convertlist.convertbones(boneslist);
            boneslist = newbones.ToArray();
            Console.WriteLine("conversion complete");
            if (texturesize == null)
            {
                texturesize = new int[2];
                var buff = new byte[32];
                using (var d = File.OpenRead(path.Remove(path.LastIndexOf('\\') + 1) + texturename + ".png"))
                {
                    d.Read(buff, 0, 32);
                }
                const int wOff = 16;
                const int hOff = 20;
                texturesize[0] = BitConverter.ToInt32(new[] { buff[wOff + 3], buff[wOff + 2], buff[wOff + 1], buff[wOff + 0], }, 0);
                texturesize[1] = BitConverter.ToInt32(new[] { buff[hOff + 3], buff[hOff + 2], buff[hOff + 1], buff[hOff + 0], }, 0);
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string newmodeljson = JsonConvert.SerializeObject(newmod, Formatting.Indented, settings);
            string newgeo = "{\n\n    " + '"' + "format_version" + '"' + ": " + '"' + "1.16.5" + '"' + ",\n	" + '"' + "minecraft:geometry" + '"' + @": [
        {
                " + '"' + "description" + '"' + @": {
                    " + '"' + "identifier" + '"' + ": " + '"' + "geometry.steve" + '"' + @",
				" + '"' + "texture_width" + '"' + @": " + texturesize[0] + @",
				" + '"' + "texture_height" + '"' + @": " + texturesize[1] + @",
				" + '"' + "visible_bounds_width" + '"' + @": 4,
				" + '"' + "visible_bounds_height" + '"' + @": 3.5,
				" + '"' + "visible_bounds_offset" + '"' + @": [0, 1.25, 0]
			},
			" + '"' + "bones" + '"' + @": " + JsonConvert.SerializeObject(boneslist, settings)+@"}]}";
            newgeo = newgeo.Replace(".0,", ",").Replace(".0]", "]");
            string newpath = path.Remove(path.LastIndexOf('\\')+1)+newmod.name;
            Directory.CreateDirectory(newpath);
            File.WriteAllText(newpath + @"\model.json", newmodeljson);
            File.WriteAllText(newpath + @"\"+newmod.name + ".geo.json", newgeo);
            File.Copy(path.Remove(path.LastIndexOf('\\') + 1) + texturename + ".png", newpath + @"\" + texturename + ".png");
            Console.Clear();
            Console.WriteLine("The new file is ready, bye");
            Console.ReadKey();
            return;
        }
    }
}
