using UnityEngine;

public class PlayerStat : MonoBehaviour {
	public PlayerCharacter player;

	public void Init (int index) {
		player = new PlayerCharacter(DataManager.Instance.characterList[index]);
	}
}
