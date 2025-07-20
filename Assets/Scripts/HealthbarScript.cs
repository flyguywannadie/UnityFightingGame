using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthbarScript : MonoBehaviour
{
	[SerializeField] private Slider health;
	[SerializeField] private TextMeshProUGUI combo;
	[SerializeField] private BaseCharacter character;

	private void Start()
	{
		health.maxValue = character.GetMaxHealth();
	}

	private void Update()
	{
		health.value = character.GetHealth();
		combo.gameObject.SetActive(character.GetCombo() > 1);
		combo.text = character.GetCombo().ToString();
	}
}
