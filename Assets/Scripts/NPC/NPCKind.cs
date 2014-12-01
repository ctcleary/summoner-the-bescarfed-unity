using UnityEngine;
using System.Collections;

public class NPCKind {
	private readonly int kind;
	private readonly string name;
	
	public static readonly NPCKind SUMMONED = new NPCKind(0, "Summoned");

	public static readonly NPCKind ENEMY    = new NPCKind(20, "Enemy");

	private NPCKind(int kind, string name) {
		this.kind = kind;
		this.name = name;
	}

	public int Kind {
		get { return kind; }
	}
	
	public string Name {
		get { return name; }
	}

	public string Tag {
		get { return name; }
	}
}
