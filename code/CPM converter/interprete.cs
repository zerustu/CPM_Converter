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
        int nb_if;

        public string Type { get => type; }
        public float Value { get => type == "value" ? value : throw new ArgumentException("acessing value of a non value node"); }
        public node[] Child { get => type != "value" && type != "variable" ? child : throw new ArgumentException("acessing child of a node without child") ; }
        public string VarName { get => type == "variable" ? var_name : throw new ArgumentException("acessing varname of a non variable node"); }
        public int Nb_if { get => nb_if; }
        public node(string operation, node[] nodes)
        {
            type = operation;
            child = nodes;
            nb_if = type == "if" ? 1 : 0;
            foreach (var item in child)
            {
                nb_if += item.Nb_if;
            }
        }
        public node(float number)
        {
            type = "value";
            value = number;
            nb_if = 0;
        }
        public node(string varname)
        {
            type = "variable";
            var_name = varname;
            nb_if = 0;
        }
    }
}
