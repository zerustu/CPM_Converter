using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

namespace CPM_converter
{
    class Program
    {
        //initailizing global variables 
        public static string texturename = "N/A";
        public static parameters skeletonparam;
        public static Dictionary<string, float> variable = new Dictionary<string, float>()
        {
            { "limb_swing", 0 },
            { "limb_speed", 0 },
            { "age", 0 },
            { "head_yaw", 0 },
            { "head_pitch", 0 },
            { "scale", (float)0.0625 },
            { "health", 20 },
            { "food_level", 20 },
            { "hurt_time", 0 },
            { "pos_x" , 0 },
            { "pos_y" , 0 },
            { "pos_z" , 0 },
            { "speed_x" , 0 },
            { "speed_y" , 0 },
            { "speed_z" , 0 },
            { "yaw", 0 },
            { "body_yaw", 0 },
            { "pitch", 0 },
            { "swing_progress", 0 },
            { "is_alive", 1 },
            { "is_burning", 0 },
            { "is_glowing", 0 },
            { "is_hurt", 0 },
            { "is_in_lava", 0 },
            { "is_in_water", 0 },
            { "is_invisible", 0 },
            { "is_on_ground", 1 },
            { "is_riding", 0 },
            { "is_sneaking", 0 },
            { "is_sprinting", 0 },
            { "is_wet", 0 },
            { "is_first_person", 0 },
            { "pi", (float)Math.PI },
            { "time", 0 },
            { "temp", 0 }
        };
        public static int[] texturesize = null;
        public static string animation = "";

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
            string newpath = path.Remove(path.LastIndexOf('\\') + 1) + "converted";
            Directory.CreateDirectory(newpath);
            string anim_path = newpath + @"\animation.js";
            File.Create(anim_path).Close();

            //deserialize the object
            model themodel = JsonConvert.DeserializeObject<model>(jsonmodel);
            if (themodel.skeleton == null)
            {
                themodel.skeleton = new skel();
            }
            themodel.skeleton.fix();

            Console.WriteLine("loading variables and tickvar.");
            foreach (var item in themodel.tickVars)
            {
                float v = CPM_converter.convertlist.stringToDoble(item.Value[1]);
                variable.Add("tvp_" + item.Key, v);
                variable.Add("tvl_" + item.Key, v);
                variable.Add("tvc_" + item.Key, v);
            }
            foreach (var item in themodel.variables)
            {
                variable.Add("var_" + item.Key, CPM_converter.convertlist.stringToDoble(item.Value));
            }

            //loging all the variables in the animation file
            animation = "var ";
            foreach (var item in variable)
            {
                animation += item.Key + ", ";
            }
            animation = animation.Remove(animation.Length - 2) + "; \n\n function init(entity, model) \n {\n";
            foreach (var item in variable)
            {
                animation += item.Key + " = " + item.Value + ";\n";
            }
            animation = animation + "} \n \n function tick(entity, model) \n{\n";
            foreach (var item in themodel.tickVars)
            {
                animation += $"tvl_{item.Key} = tvc_{item.Key};\n {convertlist.setvar("tvc_" + item.Key, item.Value[2])}";
            }
            foreach (var item in themodel.variables)
            {
                if (item.Value != convertlist.stringToDoble(item.Value).ToString())
                {
                    animation += convertlist.setvar(item.Key, item.Value);
                }
            }
            animation += "}\n \nfunction update(entity, model)\n{\n";
            foreach (var variable in themodel.tickVars)
            {
                animation += $"tvp_{variable.Key} = entity.getPartial()*tvc_{variable.Key} + (1-entity.getPartial())*tvl_{variable.Key};\n";
            }


            Console.WriteLine("loading the skeleton");
            skeletonparam = new parameters(themodel.skeleton);


            Console.WriteLine("loading the bones...");
            newbone[] boneslist = new newbone[themodel.bones.Length];
            for (int i = 0; i < themodel.bones.Length; i++)
            {
                boneslist[i] = new newbone(themodel.bones[i]);//finding a default value for everybone and create the parents link
            }
            newmodel newmod = new newmodel(themodel, skeletonparam);

            Console.WriteLine("calculating new coordinates");
            List<newbone> newbones = convertlist.convertbones(boneslist); //go from relative position to absolute position
            boneslist = newbones.ToArray();

            Console.WriteLine("conversion complete");
            File.WriteAllText(anim_path, animation);
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

            File.WriteAllText(newpath + @"\model.json", newmodeljson);
            File.WriteAllText(newpath + @"\"+newmod.name + ".geo.json", newgeo);
            File.Copy(path.Remove(path.LastIndexOf('\\') + 1) + texturename + ".png", newpath + @"\" + texturename + ".png");
            Console.Clear();
            Console.WriteLine("The new file is ready, bye");
            System.Threading.Thread.Sleep(2000);
            return;
        }
    }
}
