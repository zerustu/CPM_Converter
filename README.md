# CPM_Converter
This tool can convert 1.12 CPM model to 1.16 CPM model.

It is not perfect a can require some work on the output file to have the good model but it should output a functionning model.
the output geometrie file can be easely inport in block bench for easy modification without any external tool.

# NOTE:
my tool does not handle scalling and multi texture.

scalling does not work the same in 1.12 and in 1.16 so there is no easy conversion methode. I may work on that latter.

for multi-texture, 1.16 model does not support multi texture so if the initial model have multiple texture, you will have to fuse them and recalculate the UV.

For any modification after my tool, i recommand importing it in blockbench as my output file doesn't contain breakline or tabulation so it is especialy hard to read.

▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

# HOW TO USE:

-when run, the programme will first ask the model file.
You must provide a full path to the model.json file (exemple : C:\Users\zerustu\AppData\Roaming\.minecraft 1.12\custom-models\kemono\model.json)

-during the conversion, the programme may ask the value for variables use in the model (for animation in the position or rotation of bones).
You may enter the value for the ask variable
NOTE:   only number are accepted (can be decimal). for boolean, enter 1 if the boolean is true and 0 if it is false.
	The tool will display the formula in witch it have found the variable in the model. IT DOES NOT ASK THE FINAL VALUE OF THE FORMULA, just enter the ask varaible value and it will calculate the final value by it-self.

-in the end, it will ask the texture width and height. if the model use the default resolution of minecraft, it will be the real size of the texture file.
if the model use a higher resolution, the enter value might be a multible/fraction of the real size. If once finish, the texture is not correct, try changing the texture size in the model.geo.json file.

▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

# Credit:

zerustu (me) for the program.

thank you to :
Gamepiaynmo for the CPM mod
and everyone on the CPM discord server

the program was made in C# with microsoft visual studio
