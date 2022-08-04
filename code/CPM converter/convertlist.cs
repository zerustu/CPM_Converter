using System;
using System.Collections.Generic;
using System.Text;

namespace CPM_converter
{
    class convertlist
    {
        static List<char> number = new List<char>() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.' };
        static List<char> operateur = new List<char>() { '+', '-', '/', '%', '^', '>', '<', '*'};
        static List<string> onenumberfunction = new List<string> { "sin", "cos", "tan", "asin", "acos", "atan", "torad", "todeg", "exp", "abs"};
        static List<string> twonumberfunction = new List<string> { "pow", "min", "max" };
        static List<string> convertedbone = new List<string>() {"head", "body", "left_arm",  "right_arm", "left_leg", "right_leg"};
        static List<newbone> cantconvertyet = new List<newbone>();
        static List<newbone> toconvert;
        static Dictionary<string, bool> isParentActif = new Dictionary<string, bool>()
        {
            { "head", true },
            { "body", true },
            { "left_arm", true },
            { "right_arm", true },
            { "left_leg", true },
            { "right_leg", true },
        };
        static int j;
        static float[] zeros = new float[] { 0, 0, 0 };

        static public float stringToDoble(string value)
        {
            j = 0;
            try
            {
                node expression = readeexpression1(value.Replace(" ", String.Empty));
                return evaluate(expression);
            }
            catch (Exception e)
            {
                Console.WriteLine("an expression could not be solved : " + e.Message);
                return 0;
            }
        }

        static public List<newbone> convertbones(newbone[] list)
        {
            int[] curspos = new int[] { Console.CursorLeft, Console.CursorTop };
            int total = list.Length;
            List<newbone> listnewbone = new List<newbone>();
            newbone parentbone;
            toconvert = new List<newbone>(list);
            int numbertoconvert = toconvert.Count;
            while(numbertoconvert > 0)
            {
                foreach (newbone bone in toconvert)
                {
                    if ((!iszeros(bone.pivot)| bone.cubes !=null) & !iszeros(bone.rotation))
                    {
                        newbone piv_bone = new newbone(bone.name + "_rotation", bone.parent, (float[])zeros.Clone(), bone.rotation, null);
                        Program.animation = Program.animation.Replace($"(\"{bone.name}\").setRotation", $"(\"{bone.name}_rotation\").setRotation");
                        bone.rotation = (float[])zeros.Clone();
                        bone.parent = bone.name + "_rotation";
                        cantconvertyet.Add(piv_bone);
                        numbertoconvert++;
                    }
                    if (convertedbone.Contains(bone.parent)|| bone.parent == null)
                    {
                        if (bone.parent == "head" && isParentActif["head"])
                        {
                            bone.pivot[1] += Program.skeletonparam.head_pivot_height;
                            bone.pivot[2] += Program.skeletonparam.head_offset;
                        }
                        else if (bone.parent == "body" && isParentActif["body"] )
                        {
                            bone.pivot[1] += Program.skeletonparam.body_pivot_height;
                            bone.pivot[2] += Program.skeletonparam.body_offset;
                        }
                        else if (bone.parent == "left_arm" && isParentActif["left_arm"] )
                        {
                            bone.pivot[0] += Program.skeletonparam.arm_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.arm_pivot_height;
                        }
                        else if (bone.parent == "right_arm" && isParentActif["right_arm"])
                        {
                            bone.pivot[0] -= Program.skeletonparam.arm_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.arm_pivot_height;
                        }
                        else if (bone.parent == "left_leg" && isParentActif["left_leg"])
                        {
                            bone.pivot[0] += Program.skeletonparam.leg_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.leg_length;
                        }
                        else if (bone.parent == "right_leg" && isParentActif["right_leg"])
                        {
                            bone.pivot[0] -= Program.skeletonparam.leg_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.leg_length;
                        }
                        else
                        {
                            if (isParentActif.ContainsKey(bone.name) && bone.parent != null) isParentActif[bone.name] = false;
                            bone.pivot[0] *= -1;
                            if (bone.parent != null)
                            {
                                parentbone = listnewbone.Find(x => x.name == bone.parent);
                                bone.pivot[0] += parentbone.pivot[0];
                                bone.pivot[1] += parentbone.pivot[1];
                                bone.pivot[2] += parentbone.pivot[2];
                            }
                            else
                            {
                                bone.pivot[1] = 24 + bone.pivot[1];
                            }
                        }
                        if (bone.cubes != null)
                        {
                            foreach (newcube cube in bone.cubes)
                            {
                                cube.origin[0] *= -1;
                                cube.origin[0] += bone.pivot[0];
                                cube.origin[1] += bone.pivot[1];
                                cube.origin[2] += bone.pivot[2];
                                cube.origin[0] -= cube.size[0];
                                cube.origin[2] -= cube.size[2];
                            }
                        }
                        convertedbone.Add(bone.name);
                        if (bone.getPhysics() != null)
                        {
                            newbone father = listnewbone.Find(a => a.name == bone.parent);
                            (newbone, newbone) anims = create_anim_bones(bone, father);
                            if (anims.Item1 != null)
                            {
                                bone.parent = bone.name + "_physics_reset";
                                listnewbone.Add(anims.Item1);
                                listnewbone.Add(anims.Item2);
                            }
                        }
                        listnewbone.Add(bone);
                        numbertoconvert--;
                        Console.SetCursorPosition(curspos[0], curspos[1]);
                        Console.WriteLine((total - numbertoconvert) + "/" + total);
                    }
                    else
                    {
                        cantconvertyet.Add(bone);
                    }
                }
                toconvert = cantconvertyet;
                cantconvertyet = new List<newbone>();
            }
            return listnewbone;
        }

        static public node readeexpression1(string expression)
        {
            if (j >= expression.Length)
            {
                return new node(0);
            }
            if (expression[j] == '+' || expression[j] == '-')
            {
                bool negativ = expression[j] == '-';
                j++;
                node avlaue = readeexpression3(expression);
                node avalue2;
                if (negativ)
                {
                    avalue2 = new node("*", new node[] { avlaue, new node(-1) });
                }
                else avalue2 = avlaue;
                return readeexpression2(expression, avalue2);
            }
            if (expression[j] == '!')
            {
                j++;
                node value = readeexpression3(expression);
                return readeexpression2(expression, new node("!", new node[] { value }));
            }
            if (j<= expression.Length -4)
            {
                if (expression[j] == 'i' & expression[j + 1] == 'f' & expression[j + 2] == '(')
                {
                    j += 3;
                    node node1 = readeexpression1(expression);
                    j += 1;
                    node node2 = readeexpression1(expression);
                    j += 1;
                    node node3 = readeexpression1(expression);
                    j += 1;
                    return readeexpression2(expression, new node("if", new node[3] { node1, node2, node3 }));
                }
            }
            if (expression[j] == '(')
            {
                j++;
                node avalue = readeexpression1(expression);
                j++;
                return readeexpression2(expression, avalue);
            }
            if (number.Contains(expression[j]))
            {
                bool point = false;
                bool end = false;
                string temp = "";
                while (!end)
                {
                    if (number.Contains(expression[j]) & !(point & expression[j] == '.'))
                    {
                        temp += expression[j];
                        point = (expression[j] == ',');
                        j++;
                        if (j == expression.Length)
                        {
                            end = true;
                        }
                    }
                    else
                    {
                        end = true;
                    }
                }
                float result;
                if (!float.TryParse(temp, out result))
                {
                    result = float.Parse(temp.Replace('.', ','));
                }
                return readeexpression2(expression, new node(result));
            }
            if (expression.ToLower()[j] != expression.ToUpper()[j])
            {
                bool end = false;
                string temp = "";
                while (!end)
                {
                    if (number.Contains(expression[j]) | (expression.ToLower()[j] != expression.ToUpper()[j]) | expression[j] == '_'|expression[j] == '.')
                    {
                        temp += expression[j];
                        j++;
                        if (j == expression.Length)
                        {
                            end = true;
                        }
                    }
                    else
                    {
                        end = true;
                    }
                }
                if (temp == "true")
                {
                    return readeexpression2(expression, new node(1));
                }
                if (temp == "false")
                {
                    return readeexpression2(expression, new node(0));
                }
                if (onenumberfunction.Contains(temp))
                {
                    j++;
                    node value = readeexpression1(expression);
                    j++;
                    return readeexpression2(expression, new node(temp, new node[] { value }));
                }
                if (twonumberfunction.Contains(temp))
                {
                    j++;
                    node value1 = readeexpression1(expression);
                    j++;
                    node value2 = readeexpression1(expression);
                    j++;
                    return readeexpression2(expression, new node(temp, new node[] { value1, value2 }));
                }
                temp = temp.Replace('.', '_');
                if (!Program.variable.ContainsKey(temp))
                {
                    /*Console.WriteLine("a variable was detected : " + temp);
                    Console.WriteLine("in the expression : " + expression);
                    Console.WriteLine("what is its value? (for boolean, 0 for false and 1 for true)");
                    float value;
                    string input = Console.ReadLine();
                    while (!float.TryParse(input, out value))
                    {
                        Console.CursorTop -= 1;
                        Console.Write("invalid value (only number accepted), try again :                                      ");
                        Console.CursorTop -= 1;
                        Console.CursorLeft = 49;
                        input = Console.ReadLine();
                    }
                    Console.CursorTop -= 4;
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.CursorTop -= 4;*/
                    Program.variable.Add(temp, 0);
                }
                return readeexpression2(expression, new node(temp));
            }
            return new node(0);
        }
        static public node readeexpression2(string expression, node last)
        {
            if (j >= expression.Length)
            {
                return last;
            }
            if (expression[j] == ',' | expression[j] == ')')
            {
                return last;
            }
            if (operateur.Contains(expression[j]))
            {
                string oper = expression[j].ToString();
                j++;
                node next = readeexpression3(expression);
                if (oper == "^" |((oper == "*" | oper == "/") & (last.Type == "+" | last.Type == "-")))
                {
                    node value = new node(oper, new node[] { last.Child[1], next });
                    last.Child[1] = value;
                    return readeexpression2(expression, last);
                }
                return readeexpression2(expression, new node(oper, new node[] { last, next }));
            }
            if (j+1 <= expression.Length)
            {
                if (expression[j] == '=' & expression[j+1] == '=')
                {
                    j++;
                    j++;
                    node next = readeexpression3(expression);
                    return readeexpression2(expression, new node("==", new node[] { last, next }));
                }
                if (expression[j] == '*' & expression[j + 1] == '*')
                {
                    j++;
                    j++;
                    node next = readeexpression3(expression);
                    node value = new node("^", new node[] { last.Child[1], next });
                    last.Child[1] = value;
                    return readeexpression2(expression, last);
                }
                if (expression[j] == '&' & expression[j + 1] == '&')
                {
                    j++;
                    j++;
                    node next = readeexpression3(expression);
                    return readeexpression2(expression, new node("&&", new node[] { last, next }));
                }
                if (expression[j] == '|' & expression[j + 1] == '|')
                {
                    j++;
                    j++;
                    node next = readeexpression3(expression);
                    return readeexpression2(expression, new node("||", new node[] { last, next }));
                }
                if (expression[j] == '!' & expression[j + 1] == '=')
                {
                    j++;
                    j++;
                    node next = readeexpression3(expression);
                    return readeexpression2(expression, new node("!=", new node[] { last, next }));
                }
                if (expression[j] == '<' & expression[j + 1] == '=')
                {
                    j++;
                    j++;
                    node next = readeexpression3(expression);
                    return readeexpression2(expression, new node("<=", new node[] { last, next }));
                }
                if (expression[j] == '>' & expression[j + 1] == '=')
                {
                    j++;
                    j++;
                    node next = readeexpression3(expression);
                    return readeexpression2(expression, new node(">=", new node[] { last, next }));
                }
            }
            return last;
        }
        static node readeexpression3(string expression)
        {
            if (j >= expression.Length)
            {
                return new node(0);
            }
            if (expression[j] == '!')
            {
                j++;
                node value = readeexpression3(expression);
                return new node("!", new node[] { value });
            }
            if (j <= expression.Length - 4)
            {
                if (expression[j] == 'i' & expression[j + 1] == 'f' & expression[j + 2] == '(')
                {
                    j += 3;
                    node node1 = readeexpression1(expression);
                    j += 1;
                    node node2 = readeexpression1(expression);
                    j += 1;
                    node node3 = readeexpression1(expression);
                    j += 1;
                    return new node("if", new node[3] { node1, node2, node3 });
                }
            }
            if (expression[j] == '(')
            {
                j++;
                node avalue = readeexpression1(expression);
                j++;
                return avalue;
            }
            if (number.Contains(expression[j]))
            {
                bool point = false;
                bool end = false;
                string temp = "";
                while (!end)
                {
                    if (number.Contains(expression[j]) & !(point & expression[j] == '.'))
                    {
                        temp += expression[j];
                        point = (expression[j] == '.');
                        j++;
                        if (j == expression.Length)
                        {
                            end = true;
                        }
                    }
                    else
                    {
                        end = true;
                    }
                }
                float result;
                if (!float.TryParse(temp, out result))
                {
                    result = float.Parse(temp.Replace('.', ','));
                }
                return new node(result);
            }
            if (expression.ToLower()[j] != expression.ToUpper()[j])
            {
                bool end = false;
                string temp = "";
                while (!end)
                {
                    if (number.Contains(expression[j]) | (expression.ToLower()[j] != expression.ToUpper()[j]) | expression[j] == '_' | expression[j] == '.')
                    {
                        temp += expression[j];
                        j++;
                        if (j == expression.Length)
                        {
                            end = true;
                        }
                    }
                    else
                    {
                        end = true;
                    }
                }
                if (temp == "true")
                {
                    return new node(1);
                }
                if (temp == "false")
                {
                    return new node(0);
                }
                if (onenumberfunction.Contains(temp))
                {
                    j++;
                    node value = readeexpression1(expression);
                    j++;
                    return new node(temp, new node[] { value });
                }
                if (twonumberfunction.Contains(temp))
                {
                    j++;
                    node value1 = readeexpression1(expression);
                    j++;
                    node value2 = readeexpression1(expression);
                    j++;
                    return new node(temp, new node[] { value1, value2 });
                }
                temp = temp.Replace('.', '_');
                if (!Program.variable.ContainsKey(temp))
                {
                    /*Console.WriteLine("a variable was detected : " + temp);
                    Console.WriteLine("in the expression : " + expression);
                    Console.Write("what is its value? (for boolean, 0 for false and 1 for true)");
                    float value;
                    string input = Console.ReadLine();
                    while (!float.TryParse(input, out value))
                    {
                        Console.CursorTop -= 1;
                        Console.WriteLine("invalid value (only number accepted), try again :                                      ");
                        Console.CursorTop -= 1;
                        Console.CursorLeft = 49;
                        input = Console.ReadLine();
                    }
                    Console.CursorTop -= 4;
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.WriteLine(new string(' ', Console.WindowWidth-1));
                    Console.CursorTop -= 4;*/
                    Program.variable.Add(temp, 0);
                }
                return new node(temp);
            }
            return new node(0);
        }

        static float evaluate(node lanode)
        {
            if (lanode.Type == "value")
            {
                return lanode.Value;
            }
            if (lanode.Type == "variable")
            {
                return Program.variable[lanode.VarName];
            }
            float[] lesvaleurs = new float[lanode.Child.Length];
            for (int i = 0; i < lanode.Child.Length; i++)
            {
                lesvaleurs[i] = evaluate(lanode.Child[i]);
            }
            if (lanode.Type == "if")
            {
                return (lesvaleurs[0] == 1) ? lesvaleurs[1] : lesvaleurs[2];
            }
            if (lanode.Type == "+")
            {
                return lesvaleurs[0] + lesvaleurs[1];
            }
            if (lanode.Type == "-")
            {
                return lesvaleurs[0] - lesvaleurs[1];
            }
            if (lanode.Type == "*")
            {
                return lesvaleurs[0] * lesvaleurs[1];
            }
            if (lanode.Type == "/")
            {
                return lesvaleurs[0] / lesvaleurs[1];
            }
            if (lanode.Type == "^" | lanode.Type == "pow")
            {
                return MathF.Pow(lesvaleurs[0],lesvaleurs[1]);
            }
            if (lanode.Type == "min")
            {
                return MathF.Min(lesvaleurs[0], lesvaleurs[1]);
            }
            if (lanode.Type == "max")
            {
                return MathF.Max(lesvaleurs[0], lesvaleurs[1]);
            }
            if (lanode.Type == "sin")
            {
                return MathF.Sin(lesvaleurs[0]);
            }
            if (lanode.Type == "cos")
            {
                return MathF.Cos(lesvaleurs[0]);
            }
            if (lanode.Type == "tan")
            {
                return MathF.Tan(lesvaleurs[0]);
            }
            if (lanode.Type == "asin")
            {
                return MathF.Asin(lesvaleurs[0]);
            }
            if (lanode.Type == "acos")
            {
                return MathF.Acos(lesvaleurs[0]);
            }
            if (lanode.Type == "atan")
            {
                return MathF.Atan(lesvaleurs[0]);
            }
            if (lanode.Type == "torad")
            {
                return lesvaleurs[0] * MathF.PI / 180;
            }
            if (lanode.Type == "todeg")
            {
                return lesvaleurs[0] / MathF.PI * 180;
            }
            if (lanode.Type == "exp")
            {
                return MathF.Exp(lesvaleurs[0]);
            }
            if (lanode.Type == "abs")
            {
                return MathF.Abs(lesvaleurs[0]);
            }
            if (lanode.Type == "!")
            {
                return 1 - lesvaleurs[0];
            }
            if (lanode.Type == "&&")
            {
                return (lesvaleurs[0] == 1 & lesvaleurs[1] == 1) ? 1 : 0;
            }
            if (lanode.Type == "||")
            {
                return (lesvaleurs[0] == 1 | lesvaleurs[1] == 1) ? 1 : 0;
            }
            if (lanode.Type == ">")
            {
                return (lesvaleurs[0] > lesvaleurs[1]) ? 1 : 0;
            }
            if (lanode.Type == ">=")
            {
                return (lesvaleurs[0] >= lesvaleurs[1]) ? 1 : 0;
            }
            if (lanode.Type == "<")
            {
                return (lesvaleurs[0] < lesvaleurs[1]) ? 1 : 0;
            }
            if (lanode.Type == "<=")
            {
                return (lesvaleurs[0] <= lesvaleurs[1]) ? 1 : 0;
            }
            if (lanode.Type == "==")
            {
                return (lesvaleurs[0] == lesvaleurs[1]) ? 1 : 0;
            }
            if (lanode.Type == "!=")
            {
                return (lesvaleurs[0] != lesvaleurs[1]) ? 1 : 0;
            }
            return 0;
        }

        static bool iszeros(float[] value)
        {
            return value[0] == 0 & value[1] == 0 & value[2] == 0;
        }

        static string unnode(node expr)
        {
            if (expr == null) return null;
            if (expr.Type == "value") return expr.Value.ToString();
            if (expr.Type == "variable") return know_name(expr.VarName);
            if (expr.Type == "if") throw new Exception("an if should not be unnode, the animation will be lost");
            if (onenumberfunction.Contains(expr.Type))
            {
                if (expr.Type == "todeg") return $"(180/Math.PI*({unnode(expr.Child[0])}))";
                if (expr.Type == "torad") return $"(Math.PI*({unnode(expr.Child[0])})/180)";
                return $"{expr.Type}({unnode(expr.Child[0])})"; 
            }
            if (twonumberfunction.Contains(expr.Type)) return $"{expr.Type}({unnode(expr.Child[0])},{unnode(expr.Child[1])})";
            if (expr.Type == "!") return $"!{unnode(expr.Child[0])}";
            return $"({unnode(expr.Child[0])}{expr.Type}{unnode(expr.Child[1])})";
        }

        static string node_to_code(node expr, string varName)
        {
            try
            {
                if (expr.Nb_if == 0) return varName + $" = {unnode(expr)};\n";
                if (expr.Type == "value") return $"{varName} += {expr.Value};\n";
                if (expr.Type == "variable") return $"{varName} += " + know_name(expr.VarName) + ";\n";
                if (expr.Type == "if" & expr.Child[0].Nb_if == 0) return $"if({unnode(expr.Child[0])}) " + "{\n" + node_to_code(expr.Child[1], varName) + "}\nelse {\n" + node_to_code(expr.Child[2], varName) + "}\n";
                if (expr.Type == "if") return $"var {varName}_test = 0\n {node_to_code(expr.Child[0], varName + "_test")}if({varName}_test) " + "{\n" + node_to_code(expr.Child[1], varName) + "}\nelse {\n" + node_to_code(expr.Child[2], varName) + "}\n";
                if (onenumberfunction.Contains(expr.Type))
                {
                    if (expr.Type == "todeg") return node_to_code(expr.Child[0], varName) + $"{varName} = (180/Math.PI*({varName}));\n";
                    if (expr.Type == "torad") return node_to_code(expr.Child[0], varName) + $"{varName} = (Math.PI*({varName})/180);\n";
                    return node_to_code(expr.Child[0], varName) + $"{varName} = {expr.Type}({varName});\n";
                }
                if (twonumberfunction.Contains(expr.Type)) return $"{varName}_1 = 0;\n{node_to_code(expr.Child[0], varName + "_1")};\n{varName}_2 = 0;\n{node_to_code(expr.Child[1], varName +"_2")};\n{varName} = {expr.Type}({varName}_1,{varName}_2);\n";
                if (expr.Type == "!") return node_to_code(expr.Child[0], varName) + $"{varName} = !{varName};\n";
                string type_of_one = expr.Child[0].Type;
                if (type_of_one == "value") return node_to_code(expr.Child[1], varName) + $"{varName} = {expr.Child[0].Value} {expr.Type} {varName};\n";
                if (type_of_one == "variable") return node_to_code(expr.Child[1], varName) + $"{varName} = {know_name(expr.Child[0].VarName)} {expr.Type} {varName};\n";
                type_of_one = expr.Child[1].Type;
                if (type_of_one == "value") return node_to_code(expr.Child[0], varName) + $"{varName} = {varName} {expr.Type} {expr.Child[1].Value} ;\n";
                if (type_of_one == "variable") return node_to_code(expr.Child[0], varName) + $"{varName} = {varName} {expr.Type}  {know_name(expr.Child[1].VarName)};\n";
                return $"{varName}_1 = 0;\n{node_to_code(expr.Child[0], varName + "_1")};\n{varName}_2 = 0;\n{node_to_code(expr.Child[1], varName + "_2")};\n{varName} = {varName}_1{expr.Type}{varName}_2;\n"; ;
            }
            catch (Exception)
            {
                return "Console.log(\"something went wrong here, some code has been lost\");\ntemp = 0;\n";
            }
        }

        static public string setvar(string varname, string varvalue)
        {
            string res = $"{varname} = 0;\n";
            j = 0;
            node expr = readeexpression1(varvalue.Replace(" ", string.Empty));
            if (expr.Nb_if != 0)
            {
                res += node_to_code(expr, varname);
            }
            else
            {
                res = varname + " = " + unnode(expr) + ";\n";
            }
            return res;
        }

        static Dictionary<string, string> knowfunctions = new Dictionary<string, string>()
        {
            { "limb_swing", "entity.getAnimPosition()" },
            { "age", "entity.getAge()" },
            { "head_yaw", "entity.getHeadYaw()" },
            { "head_pitch", "entity.getHeadPitch()" },
            { "scale", "entity.getScale()" },
            { "health", "entity.getHealth()" },
            { "food_level", "entity.getFoodLevel()" },
            { "hurt_time", "entity.getHurtTime()" },
            { "pos_x" , "entity.getPosX()" },
            { "pos_y" , "entity.getPosY()" },
            { "pos_z" , "entity.getPosZ()" },
            { "speed_x" , "entity.getMotionX()" },
            { "speed_y" , "entity.getMotionY()" },
            { "speed_z" , "entity.getMotionZ()" },
            { "yaw", "0" },
            { "body_yaw", "entity.getBodyYaw()" },
            { "pitch", "0" },
            { "swing_progress", "entity.getSwingProgress()" },
            { "is_alive", "entity.isAlive()" },
            { "is_burning", "entity.isBurning()" },
            { "is_glowing", "entity.isGlowing()" },
            { "is_hurt", "entity.isHurt()" },
            { "is_in_lava", "entity.isInLava()" },
            { "is_in_water", "entity.isInWater()" },
            { "is_invisible", "entity.isInvisible()" },
            { "is_on_ground", "entity.isOnGround()" },
            { "is_riding", "entity.isRiding()" },
            { "is_sneaking", "entity.isCrouching()" },
            { "is_sprinting", "entity.isSprinting()" },
            { "is_wet", "entity.isWet()" },
            { "is_first_person", "model.isFirstPerson()" },
            { "pi", "Math.PI" },
            { "time", "0" },
        };

        static Dictionary<string, string> knowTerminasion = new Dictionary<string, string>()
        {
            { "_tx", "getPositionX()" },
            { "_ty", "getPositionY()" },
            { "_tz", "getPositionZ()" },
            { "_rx", "getRotationY()*Math.PI/180" },
            { "_ry", "getRotationX()*Math.PI/180" },
            { "_rz", "getRotationZ()*Math.PI/180" },
            { "_sx", "getScaleX()" },
            { "_sy", "getScaleY()" },
            { "_sz", "getScaleZ()" },
        };

        static Dictionary<string, string> inventory = new Dictionary<string, string>()
        {
            { "inv_mainhand", "entity.getMainHandItem().getItem()" },
            { "inv_offhand", "entity.getOffHandItem().getItem()" },
            { "inv_chestplate", "entity.getChestplateItem().getItem()" },
            { "inv_leggings", "entity.getLeggingsItem().getItem()" },
            { "inv_boots", "entity.getBootsItem().getItem()" },
            { "inv_helmet", "entity.getHelmetItem().getItem()" },
            { "current_pose", "entity.getPose()" }
        };

        static string know_name(string oldname)
        {
            if (knowfunctions.ContainsKey(oldname)) return knowfunctions[oldname];
            if (inventory.ContainsKey(oldname)) return inventory[oldname];
            string terminaison = "";
            if (knowTerminasion.TryGetValue(oldname.Substring(oldname.Length - 3), out terminaison)) return $"model.getBone(\"{oldname.Remove(oldname.Length - 3)}\").{terminaison}";
            if (oldname.StartsWith("inv_main"))
            {
                int index = int.Parse(oldname.Substring(8));
                return $"entity.getInventoryItem().getItem({index})";
            }
            if (oldname.StartsWith("item_")) return '"' + oldname.Substring(5) + '"';
            if (oldname.StartsWith("pose_")) return '"' + oldname.Substring(5) + '"';
            return oldname;
        }

        static (newbone, newbone) create_anim_bones(newbone bone, newbone parent)
        {
            float[] posbone = bone.pivot;
            float[] posparent = parent.pivot;
            double x = posbone[0] - posparent[0];
            double y = posbone[1] - posparent[1];
            double z = posbone[2] - posparent[2];

            if (Math.Abs(x) + Math.Abs(z) == 0) return (null, null);

            double rz = z < 0 ? Math.PI : 0;
            double ry = Math.Atan(x / Math.Abs(z));
            double rx = Math.Atan(y / Math.Sqrt(x * x + z * z));
            rx = z < 0 ? Math.PI - rx : -rx;

            float[] rot1 = new float[3] { (float)rx, (float)ry, (float)rz };
            newbone anim1 = new newbone(bone.name + "_physics", parent.name, posbone, rot1, null);
            double rz2 = ry * Math.Sin(rx);

            double x2 = -Math.Cos(rz2) * Math.Sin(ry) + Math.Sin(rz2) * Math.Sin(rx) * Math.Cos(ry);
            double y2 = Math.Cos(rz2) * Math.Sin(rx) * Math.Cos(ry) + Math.Sin(rz2) * Math.Sin(ry);
            double z2 = Math.Cos(rx) * Math.Cos(ry);



            rz2 += z2 < 0 ? Math.PI : 0;
            double ry2 = Math.Atan(x2 / Math.Abs(z2));
            double rx2 = Math.Atan(y2 / Math.Sqrt(x2 * x2 + z2 * z2));
            rx2 = z2 < 0 ? Math.PI - rx2 : -rx2;

            float[] rot2 = new float[3] { (float)rx2, (float)ry2, (float)rz2 };
            newbone anim2 = new newbone(bone.name + "_physics_reset", bone.name + "_physics", posbone, rot2, null);

            Program.bonesPhysics.Add(bone.name + "_physics", bone.getPhysics());

            return (anim1, anim2);
        }
    }
}