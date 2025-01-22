# AssemblyPublicizer

A tool that makes every field, property, event and method public from an .net assembly (for example Assembly-CSharp.dll from unity games).

Please read the Note section before proceding with the use of this tool.

## How to Use?

- Method 1 --
Drag and drop the .dll to the console application and it will generate a directory called Publicized-Assemblies in the folder with both specified dll and its publicized version.

- Method 2 --
Open a powershell or cmd terminal in the .exe location and execute `./AssemblyPublicizer.exe DLLName.dll PathToWriteHere`.
Replacing the DLLName parameter to your dll name + .dll (keep in mind the dll has to be in the exe folder aswell).
Replacing the PathToWriteHere to your desired path.

## Note
If you came here to try this tool in a GameAssembly.dll (IL2CPP compiled unity assembly), this won't work with those, only works with .dll files that has a valid METADATA, which means they can be decompiled using DnSpy (or ILSpy).

This tool doesn't remove the readonly modifier from something, or makes a private setter public.
