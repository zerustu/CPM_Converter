using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace CPM_converter
{
    class model 
    {
        public string modelId;
        public string modelName;
        public string version;
        public string author;
        public Bone[] bones;
        public skel skeleton;
        public Dictionary<string, string> variables;
        public Dictionary<string, string[]> tickVars;
        public Dictionary<string, float> eyeHeight;
        public Dictionary<string, float[]> boundingBox;

        public model()
        {
            this.modelId = "convertedmodel";
            this.modelName = "converted model";
            this.version = "1.0";
            this.author = "unknown";
            this.variables = new Dictionary<string, string>();
            this.tickVars = new Dictionary<string, string[]>();
        }
    }
    class skel
    {
        public string[] head_all;
        public string[] body_all;
        public string[] left_arm_all;
        public string[] left_leg_all;
        public string[] right_leg_all;
        public string[] right_arm_all;

        public skel()
        {
            head_all = new string[] { "0", "0", "0" };
            body_all = new string[] { "0", "0", "0" };
            left_arm_all = new string[] { "-6", "0", "0" };
            left_leg_all = new string[] { "-2", "-12", "0" };
            right_leg_all = new string[] { "2", "-12", "0" };
            right_arm_all = new string[] { "6", "0", "0" };
        }

        public void fix()
        {
            if (head_all == null)
            {
                head_all = new string[] { "0", "0", "0" };
            }
            if (body_all == null)
            {
                body_all = new string[] { "0", "0", "0" };
            }
            if (left_arm_all == null)
            {
                left_arm_all = new string[] { "-6", "0", "0" };
            }
            if (left_leg_all == null)
            {
                left_leg_all = new string[] { "-2", "-12", "0" };
            }
            if (right_leg_all == null)
            {
                right_leg_all = new string[] { "2", "-12", "0" };
            }
            if (right_arm_all == null)
            {
                right_arm_all = new string[] { "6", "0", "0" };
            }
        }
    }
    class boxe
    {
        public float[] textureOffset;
        public string[] coordinates;
        public float sizeAdd;
        public bool mirror;

    }
    class Bone
    {
        string id;
        string parent;
        string texture;
        string visible;
        string[] position;
        string[] rotation;
        int[] textureSize;
        boxe[] boxes;

        public string Id { get => id; set => id = value; }
        public string Parent { get => parent; set => parent = value; }
        public string Texture { get => texture; set => texture = value; }
        public string[] Position { get => position; set => position = value; }
        public string[] Rotation { get => rotation; set => rotation = value; }
        public boxe[] Boxes { get => boxes; set => boxes = value; }
        public int[] TextureSize { get => textureSize; set => textureSize = value;  }
        public string Visible { get => visible; set => visible = value; }
    }
    class bonetoken
    {
        string id;
        string parent;
        string texture;
        float[] position;
        float[] rotation;
        boxe[] boxes;

        public string Id { get => id; set => id = value; }
        public string Parent { get => parent; set => parent = value; }
        public string Texture { get => texture; set => texture = value; }
        public float[] Position { get => position; set => position = value; }
        public float[] Rotation { get => rotation; set => rotation = value; }
        public boxe[] Boxes { get => boxes; set => boxes = value; }
    }
}
