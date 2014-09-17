using UnityEngine;
using System.Collections;

public class TestLog : MonoBehaviour, ITicker{
	EnterFrameTimer t;
	// Use this for initialization
	void Start(){
		t = new EnterFrameTimer(200);
		t.OnTimer = onTimer;
		t.Start();

		ConsoleCommand command;
		CompositeCommand cc = new CompositeCommand(CompositeCommandMode.SEQUENCE);
		cc.OnCommandItemComplete += delegate(AbstractCommand _command) {
			Debug.Log("Item Complete" + " " + (_command as ConsoleCommand).index.ToString());
		};
		cc.OnCommandComplete += delegate(AbstractCommand _command) {
			Debug.Log("All Complete");
		};
		int max = 10;
		for (int i = 0; i < max; i++) {
			command = new ConsoleCommand();
			command.index = i;
			cc.AddCommand(command);
		}

		cc.Execute();

	}
	
	public void OnTick(){
		Debug.Log("tick");
	}

	public void onTimer(){
		Debug.Log("timer");
	}
}
