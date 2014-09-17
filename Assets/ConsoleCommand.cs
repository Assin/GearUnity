using UnityEngine;
using System.Collections;

public class ConsoleCommand : AbstractCommand {
	public int index;

	public override void Execute()
	{
		Debug.Log("test command: " + index);
		InvokeComplete ();
	}

}
