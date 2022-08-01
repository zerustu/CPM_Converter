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
        string var_name;

        public string Type { get => type; }
        public float Value { get => type == "value" ? value : throw new ArgumentException("acessing value of a non value node"); }
        public node[] Child { get => type != "value" && type != "variable" ? child : throw new ArgumentException("acessing child of a node without child") ; }
        public string VarName { get => type == "variable" ? var_name : throw new ArgumentException("acessing varname of a non variable node"); }
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
        public node(string varname)
        {
            type = "variable";
            var_name = varname;
        }
    }
}
