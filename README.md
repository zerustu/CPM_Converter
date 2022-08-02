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
You must provide a full path to the unziped model folder (exemple : "C:\Users\zerustu\AppData\Roaming\.minecraft 1.12\custom-models\Ori")

-The tool should be able to detect the size of the texture file. If once finish, the texture is not correct, try changing the texture size in the model.geo.json file.

▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

# UPDATE:
1.2:
	-Auto-detect the texture size (custom resolution or default size)
	-read variables and tickvars (for tickvars, the initialisation value will be used)
	-add default value for is_first_person and age
	-fix the geometry file having defernt name in the model.json then the real file
	-auto copy the texturefile (so you don't have to :) )

2.0:
	-create a animation file to keep the animation from 1.12 model
	(only position and rotation animation are kept for now, and some variables aren't converted)

▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

Credit:

zerustu (me) for the program.

thank you to :
Gamepiaynmo for the CPM mod
and everyone on the CPM discord server

the program was made in C# with microsoft visual studio
