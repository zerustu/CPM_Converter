using System;
using System.Collections.Generic;
using System.Text;

namespace CPM_converter
{
    class node
    {
        string type;
        float value;
        node[] child;

        public string Type { get => type; }
        public float Value { get => value; }
        public node[] Child { get => child; }
        public node(string operation, node[] nodes)
        {
            type = operation;
            child = nodes;
        }
        public node(float number)
        {
            type = "value";
            value = number;
        }
    }
}
