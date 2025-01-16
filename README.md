# AssemblyPublicizer

A tool that makes every field, property, event and method public from an .net assembly (for example Assembly-CSharp.dll from unity games).

## How to Use?

- Method 1 --
Drag and drop the .dll to the console application and it will generate a directory called Publicized-Assemblies in the folder with both specified dll and its publicized version.

- Method 2 --
Open a powershell or cmd terminal in the .exe location and execute `./AssemblyPublicizer.exe DLLName.dll PathToWriteHere`.
Replacing the DLLName parameter to your dll name + .dll (keep in mind the dll has to be in the exe folder aswell).
Replacing the PathToWriteHere to your desired path.
