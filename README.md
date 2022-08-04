# CPM_Converter
This tool can convert 1.12 CPM model to 1.16 CPM model.

for now i will consider this tool done.

it can take a model that have animation, physics and multiple texture file and export a working 1.16 model.
if the old model have multiple texture file, they will be mmerge into one single texture file
the animation will be ported and physics will be reported as well.

# NOTE:
my tool does not handle scalling.

scalling does not work the same in 1.12 and in 1.16 so there is no easy conversion methode.

Things this tool doesn't do of do badly :
	-it will create a lot of bones because of of rotation work in CPM 1.12
	-the final .geo file is really hard to read because there is no indentation so if you want to modify it, i recommand you to import it in blockbench
	-the physics value aren't converted, so the physics are really bad. it is up to you to change them, they are in the animation.js file at the end of the init function (just befor the tick function)
	-i know that in some case, a pivot point might not end up at the correct position but i don't know why.

▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

# HOW TO USE:

-when run, the programme will first ask the model file.
You must provide a full path to the unzip model folder (exemple : D:\jeu\minecraft\.minecraft_1.16\custom-model\models\ORI)

quick video available here : https://youtu.be/R-NdDb2_T1Q



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
	
1.6:
	-now export the physics to the new model (keep the old value so it is not the best physics)

▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀▄▀

Credit:

zerustu (me) for the program.

thank you to :
Gamepiaynmo for the CPM mod
and everyone on the CPM discord server

the program was made in C# with microsoft visual studio
