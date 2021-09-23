using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;

namespace CPM_converter
{
    class Program
    {
        public static string texturename = "N/A";
        public static parameters skeletonparam;
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
            Console.WriteLine("conversion complete \n please enter the texture file width :");
            int tex_width = 0;
            while (!int.TryParse(Console.ReadLine(), out tex_width))
            {
                Console.CursorTop -= 1;
                Console.Write("invalide value, try again : ");
            }
            Console.WriteLine("please enter the texture file height :");
            int tex_height = 0;
            while (!int.TryParse(Console.ReadLine(), out tex_height))
            {
                Console.CursorTop -= 1;
                Console.Write("invalide value, try again : ");
            }
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string newmodeljson = JsonConvert.SerializeObject(newmod, Formatting.Indented);
            string newgeo = "{\n\n    " + '"' + "format_version" + '"' + ": " + '"' + "1.16.5" + '"' + ",\n	" + '"' + "minecraft:geometry" + '"' + @": [
        {
                " + '"' + "description" + '"' + @": {
                    " + '"' + "identifier" + '"' + ": " + '"' + "geometry.steve" + '"' + @",
				" + '"' + "texture_width" + '"' + @": " + tex_width + @",
				" + '"' + "texture_height" + '"' + @": " + tex_height + @",
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
            Console.Clear();
            Console.WriteLine("The new file is ready, bye");
            Console.ReadKey();
            return;
        }
    }
}
