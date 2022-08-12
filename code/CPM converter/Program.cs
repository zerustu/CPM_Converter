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
            { "last_limb_swing", 0 },
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
        };
        public static int[] texturesize = null;
        public static string animation = "";
        public static string path = "";
        public static texturemanager texturemanager;
        public static Dictionary<string, float[]> bonesPhysics = new Dictionary<string, float[]>();

        static void Main(string[] args)
        {
            texturemanager = new texturemanager();
            if (args.Length > 0)
            {
                path = args[0] + @"\model.json";
            }
            while (!File.Exists(path))
            {
                Console.WriteLine("please enter the file :");
                path = Console.ReadLine() + @"\model.json";
                Console.Clear();
                Console.WriteLine("the file was not found.");
            }
            Console.WriteLine("loading the model.");
            string jsonmodel = File.ReadAllText(path);
            path = path.Remove(path.LastIndexOf('\\') + 1);
            string newpath = path + "converted";

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
            int safezone = animation.Length;
            animation = animation.Remove(animation.Length - 2) + "; \n\n function init(entity, model) {\n";
            foreach (var item in variable)
            {
                animation += item.Key + " = " + item.Value + ";\n";
            }
            animation = animation + "} \n \n function tick(entity, model) {\nlimb_speed = entity.getAnimPosition() - last_limb_swing;\n";
            foreach (var item in themodel.tickVars)
            {
                animation += $"tvl_{item.Key} = tvc_{item.Key};\n {convertlist.setvar("tvc_" + item.Key, item.Value[2])}";
            }
            foreach (var item in themodel.variables)
            {
                if (item.Value != convertlist.stringToDoble(item.Value).ToString())
                {
                    animation += convertlist.setvar("var_" + item.Key, item.Value);
                }
            }
            animation += "last_limb_swing = entity.getAnimPosition() ; \n}\n \nfunction update(entity, model) {\n";
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

            Console.WriteLine("remapping texture");
            int[] curspos = new int[] { Console.CursorLeft, Console.CursorTop };
            int total = newbones.Count;
            int numbertoconvert = newbones.Count;
            foreach (var item in newbones)
            {
                if (item.getTextureIndex() == -1 && item.parent != null)
                {
                    item.setTextureIndex(newbones.Find( a => a.name == item.parent ).getTextureIndex());
                }
                if (item.cubes != null && item.getTextureIndex() != -1)
                {
                    int offset = texturemanager.getOffset(item.getTextureIndex());
                    foreach (var cube in item.cubes)
                    {
                        cube.uv[0] += offset;
                    }
                }
                numbertoconvert--;
                Console.SetCursorPosition(curspos[0], curspos[1]);
                Console.WriteLine((total - numbertoconvert) + "/" + total);
            }
            boneslist = newbones.ToArray();

            Console.WriteLine("adding physics in the animation.js");
            string physicsCode = "";
            foreach (var item in bonesPhysics)
            {
                physicsCode += $"model.getBone(\"{item.Key}\").physicalize({item.Value[0]}|{item.Value[1]}|{item.Value[2]}|{item.Value[3]}|{item.Value[4]});\n";
            }
            physicsCode = physicsCode.Replace(',', '.').Replace('|', ',');
            Console.WriteLine("conversion complete");
            animation += "}";
            animation = animation.Remove(safezone) + animation.Substring(safezone).Replace(',', '.').Replace("entity. model", "entity, model");
            animation = animation.Remove(animation.IndexOf("} \n \n function tick(entity,")) + physicsCode + animation.Substring(animation.IndexOf("} \n \n function tick(entity,"));
            Directory.CreateDirectory(newpath);
            string anim_path = newpath + @"\animation.js";
            File.Create(anim_path).Close();
            File.WriteAllText(anim_path, animation.Replace("abs", "Math.abs").Replace("sin", "Math.sin").Replace("cos", "Math.cos").Replace("tan", "Math.tan").Replace("asin", "Math.asin").Replace("acos", "Math.acos").Replace("atan", "Math.atan"));
            
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
            string newmodeljson = JsonConvert.SerializeObject(newmod, Formatting.Indented, settings);
            string newgeo = "{\n\n    " + '"' + "format_version" + '"' + ": " + '"' + "1.16.5" + '"' + ",\n	" + '"' + "minecraft:geometry" + '"' + @": [
        {
                " + '"' + "description" + '"' + @": {
                    " + '"' + "identifier" + '"' + ": " + '"' + "geometry.steve" + '"' + @",
				" + '"' + "texture_width" + '"' + @": " + (texturemanager.weidth / texturemanager.smallest_res) + @",
				" + '"' + "texture_height" + '"' + @": " + (texturemanager.height / texturemanager.smallest_res) + @",
				" + '"' + "visible_bounds_width" + '"' + @": 4,
				" + '"' + "visible_bounds_height" + '"' + @": 3.5,
				" + '"' + "visible_bounds_offset" + '"' + @": [0, 1.25, 0]
			},
			" + '"' + "bones" + '"' + @": " + JsonConvert.SerializeObject(boneslist, settings)+@"}]}";
            newgeo = newgeo.Replace(".0,", ",").Replace(".0]", "]");

            File.WriteAllText(newpath + @"\model.json", newmodeljson);
            File.WriteAllText(newpath + @"\"+newmod.name + ".geo.json", newgeo);
            texturemanager.export_tex(newpath + @"\" + newmod.name +".png");
            Console.Clear();
            Console.WriteLine("The new file is ready, bye");
            System.Threading.Thread.Sleep(2000);
            return;
        }
    }
}
