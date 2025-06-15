using System.Collections.Generic;
using UnityEngine;

public class InputBuffer : MonoBehaviour
{
    [SerializeField] private SpriteRenderer direction;
    [SerializeField] private Sprite[] directions;

    [SerializeField] private BaseCharacter myCharacter;
    
    [SerializeField] private List<BufferedInput> inputs = new List<BufferedInput>();

    void Update()
    {
        int d = 4;

        if (Input.GetKey(KeyCode.A))
        {
            d -= 1;
        } 
        if (Input.GetKey(KeyCode.S))
        {
            d -= 3;
        }
		if (Input.GetKey(KeyCode.D))
		{
            d += 1;
		}
		if (Input.GetKey(KeyCode.Space))
		{
            d += 3;
		}

		direction.sprite = directions[d];

        myCharacter.CharUpdate();
	}
}
