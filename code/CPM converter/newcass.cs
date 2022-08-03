using System;
using System.Collections.Generic;
using System.Text;

namespace CPM_converter
{
    class newmodel
    {
        public string name;
        public string version;
        public string[] author;
        public string description;
        public Dictionary<string, object> skeleton;
        public Dictionary<string, object>[] parts;
        public Dictionary<string, float[]> size;
        public Dictionary<string, float[]> eye_height;

        public newmodel(model model, parameters param)
        {
            name = model.modelName.Replace(" ", "_") + "_converted";
            version = model.version;
            author = model.author.Split(',', StringSplitOptions.RemoveEmptyEntries);
            description = "converted model using zerustu's tool";
            skeleton = new Dictionary<string, object>()
            {
            { "type" , "biped" },
            { "param", param }
            };
            parts = new Dictionary<string, object>[1];
            parts[0] = new Dictionary<string, object>() { { "name", name + ".geo" }, { "texture", name + ".png" } };
            size = model.boundingBox;
            if (size != null) size.Remove("dying");
            if (model.eyeHeight != null)
            {
                eye_height = new Dictionary<string, float[]>();
                foreach (KeyValuePair<string, float> item in model.eyeHeight)
                {
                    if (item.Key != "dying") eye_height.Add(item.Key, new float[2] { item.Value, 0 });
                }
            }
        }

    }

    class parameters
    {
        public float leg_length;
        public float leg_interval;
        public float leg_offset;
        public float body_pivot_height;
        public float body_offset;
        public float arm_pivot_height;
        public float arm_interval;
        public float arm_length;
        public float head_pivot_height;
        public float head_offset;

        public parameters(skel skeleton)
        {
            body_pivot_height = convertlist.stringToDoble(skeleton.body_all[1]) + 24;
            body_offset = convertlist.stringToDoble(skeleton.body_all[2]) * -1;
            head_pivot_height = 24 + convertlist.stringToDoble(skeleton.head_all[1]);
            head_offset = convertlist.stringToDoble(skeleton.head_all[2]);
            leg_interval = convertlist.stringToDoble(skeleton.right_leg_all[0]) - convertlist.stringToDoble(skeleton.left_leg_all[0]);
            leg_length = 24 + convertlist.stringToDoble(skeleton.left_leg_all[1]);
            leg_offset = convertlist.stringToDoble(skeleton.left_leg_all[2]);
            arm_interval = convertlist.stringToDoble(skeleton.right_arm_all[0]) - convertlist.stringToDoble(skeleton.left_arm_all[0]);
            arm_pivot_height = 24 + convertlist.stringToDoble(skeleton.left_arm_all[1]);
            arm_length = 12;
        }
    }

    class newcube
    {
        public float[] origin;
        public float[] size;
        public float[] uv;
        public float inflate;
        public bool mirror;
        public newcube(boxe boxe)
        {
            this.origin = new float[3];
            origin[0] = convertlist.stringToDoble(boxe.coordinates[0]);
            origin[1] = convertlist.stringToDoble(boxe.coordinates[1]);
            origin[2] = convertlist.stringToDoble(boxe.coordinates[2]);
            this.size = new float[3];
            size[0] = convertlist.stringToDoble(boxe.coordinates[3]);
            size[1] = convertlist.stringToDoble(boxe.coordinates[4]);
            size[2] = convertlist.stringToDoble(boxe.coordinates[5]);
            this.uv = boxe.textureOffset;
            inflate = boxe.sizeAdd;
            mirror = boxe.mirror;
        }
    }

    class newbone
    {
        public string name;
        public string parent;
        public float[] pivot;
        public float[] rotation;
        public newcube[] cubes;
        static string[] axis = { "X", "Y", "Z" };
        static int[] axisrerout = { 1, 0, 2 };
        private int textureIndex;

        public int TextureIndex { get => textureIndex; set => textureIndex = value; }

        public newbone(Bone bone)
        {
            name = bone.Id.Replace(' ', '_');
            parent = bone.Parent;
            if (parent != null) parent = parent.Replace(' ', '_');
            float value = 0;
            if (bone.Visible != null)
            {
                Program.animation += convertlist.setvar(name + "_visib", bone.Visible);
                Program.animation += $"model.getBone(\"{name}\").setVisible({name}_visib);\n";
            }
            if (bone.Position != null)
            {
                pivot = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    value = convertlist.stringToDoble(bone.Position[i]);
                    pivot[i] = value;
                    if (value.ToString() != bone.Position[i] && value.ToString().Replace(',','.') != bone.Position[i])
                    {
                        Program.animation += convertlist.setvar(name + "P" + axis[i], bone.Position[i]);
                        Program.animation += $"model.getBone(\"{name}\").setPosition{axis[i]}({name + "P" + axis[i] + (i == 0 ? "*-1" : "")});\n";
                    }
                }
            }
            else
            {
                pivot = new float[3] { 0, 0, 0};
            }
            if (bone.Rotation != null)
            {
                rotation = new float[3];
                for (int i = 0; i < 3; i++)
                {
                    int j = axisrerout[i];
                    value = convertlist.stringToDoble(bone.Rotation[i]);
                    rotation[j] = value;
                    if (value.ToString() != bone.Rotation[i] && value.ToString().Replace(',', '.') != bone.Rotation[i])
                    {
                        Program.animation += convertlist.setvar(name+"R"+axis[j], bone.Rotation[i]);
                        Program.animation += $"model.getBone(\"{name}\").setRotation{axis[j]}({name + "R" + axis[j]});\n";
                    }
                }
            }
            else
            {
                rotation = new float[3] { 0, 0, 0 };
            }
            if (bone.Boxes != null)
            {
                cubes = new newcube[bone.Boxes.Length];
                for (int i = 0; i < cubes.Length; i++)
                {
                    cubes[i] = new newcube(bone.Boxes[i]);
                }
            }
            if (bone.Texture != null)
            {
                if (bone.Texture.StartsWith("if"))
                {
                    bone.Texture = bone.Texture.Substring(bone.Texture.IndexOf("tex."));
                    bone.Texture = bone.Texture.Remove(bone.Texture.IndexOf(','));
                }
                int size = -1;
                if (bone.TextureSize != null)
                {
                    size = bone.TextureSize[1];
                }
                textureIndex = Program.texturemanager.addText(Program.path + bone.Texture.Substring(4) + ".png", size);
            }
            else { textureIndex = -1; }
        }

        public newbone(string name, string parent, float[] pivot, float[] rotation, newcube[] cubes)
        {
            this.name = name;
            this.parent = parent;
            this.pivot = pivot;
            this.rotation = rotation;
            this.cubes = cubes;
        }
    }
    class geo
    {
        newbone[] bones;
    }
}
