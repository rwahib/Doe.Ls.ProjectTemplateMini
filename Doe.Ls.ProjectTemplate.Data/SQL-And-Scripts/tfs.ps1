#REM Puplishing trunk script

set %PATH%=%PATH%;"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE"
$ tf merge /baseless C:\Projects\LS\Workspaces\Doe.Ls.PositionDescriptions\Dev-branch C:\Projects\LS\Workspaces\Doe.Ls.PositionDescriptions\Trunk /recursive
$ tf resolve C:\Projects\LS\Workspaces\Doe.Ls.PositionDescriptions\Trunk -r -auto:acceptTheirs
