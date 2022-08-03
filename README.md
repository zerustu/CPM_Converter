# CPM_Converter
This tool can convert 1.12 CPM model to 1.16 CPM model.

this tool is nearly done. it can output working models with animation from the old one. some known issue include pivot point not at the correct position for complex model and not importing phisics yet.

# NOTE:
my tool does not handle scalling and multi texture.

scalling does not work the same in 1.12 and in 1.16 so there is no easy conversion methode. I may work on that latter.

For any modification after my tool, i recommand importing it in blockbench as my output file doesn't contain breakline or tabulation so it is especialy hard to read.

▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

# HOW TO USE:

-when run, the programme will first ask the model file.
You must provide a full path to the unzip model folder (exemple : D:\jeu\minecraft\.minecraft_1.16\custom-model\models\ORI)


▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

# UPDATE:
1.2:
	-Auto-detect the texture size (custom resolution or default size)
	-read variables and tickvars (for tickvars, the initialisation value will be used)
	-add default value for is_first_person and age
	-fix the geometry file having defernt name in the model.json then the real file
	-auto copy the texturefile (so you don't have to :) )
1.4:
	-add default value for all CPM variales
	-add animation conversion and animation.js generation
	-remove tool asking for variables value
	-change ouput model name
	-some conversion issue fix (hopefully)

1.5:
	-add texture merger and rework how texture are handle : now can convert a multi-texture model into a single texture model.
	
▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

Credit:

zerustu (me) for the program.

thank you to :
Gamepiaynmo for the CPM mod
and everyone on the CPM discord server

the program was made in C# with microsoft visual studio
