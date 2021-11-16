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
                        bone.rotation = (float[])zeros.Clone();
                        bone.parent = bone.name + "_rotation";
                        cantconvertyet.Add(piv_bone);
                        numbertoconvert++;
                    }
                    if (convertedbone.Contains(bone.parent)|| bone.parent == null)
                    {
                        if (bone.parent == "head")
                        {
                            bone.pivot[1] += Program.skeletonparam.head_pivot_height;
                            bone.pivot[2] += Program.skeletonparam.head_offset;
                        }
                        else if (bone.parent == "body")
                        {
                            bone.pivot[1] += Program.skeletonparam.body_pivot_height;
                            bone.pivot[2] += Program.skeletonparam.body_offset;
                        }
                        else if (bone.parent == "left_arm")
                        {
                            bone.pivot[0] += Program.skeletonparam.arm_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.arm_pivot_height;
                        }
                        else if (bone.parent == "right_arm")
                        {
                            bone.pivot[0] -= Program.skeletonparam.arm_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.arm_pivot_height;
                        }
                        else if (bone.parent == "left_leg")
                        {
                            bone.pivot[0] += Program.skeletonparam.leg_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.leg_length;
                        }
                        else if (bone.parent == "right_leg")
                        {
                            bone.pivot[0] -= Program.skeletonparam.leg_interval / 2;
                            bone.pivot[1] += Program.skeletonparam.leg_length;
                        }
                        else if (bone.parent != null)
                        {
                            bone.pivot[0] *= -1;
                            parentbone = listnewbone.Find(x => x.name == bone.parent);
                            bone.pivot[0] += parentbone.pivot[0];
                            bone.pivot[1] += parentbone.pivot[1];
                            bone.pivot[2] += parentbone.pivot[2];
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
                j++;
                node avlaue = readeexpression3(expression);
                return readeexpression2(expression, new node("*", new node[2] { avlaue, new node(-1) }));
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
                if (!Program.variable.ContainsKey(temp))
                {
                    Console.WriteLine("a variable was detected : " + temp);
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
                    Console.CursorTop -= 4;
                    Program.variable.Add(temp, value);
                }
                return readeexpression2(expression, new node(Program.variable[temp]));
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
                if (!Program.variable.ContainsKey(temp))
                {
                    Console.WriteLine("a variable was detected : " + temp);
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
                    Console.CursorTop -= 4;
                    Program.variable.Add(temp, value);
                }
                return new node(Program.variable[temp]);
            }
            return new node(0);
        }

        static float evaluate(node lanode)
        {
            if (lanode.Type == "value")
            {
                return lanode.Value;
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
    }
}