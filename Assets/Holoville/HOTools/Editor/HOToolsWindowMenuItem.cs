using Holoville.HOTools;
using UnityEditor;
public class HOToolsWindowMenuItem
{
   [MenuItem ("Window/HOTools %#a")]
   static void ShowWindow() {
       EditorWindow.GetWindow(typeof(HOToolsWindow), false, "HOTools");
   }
}