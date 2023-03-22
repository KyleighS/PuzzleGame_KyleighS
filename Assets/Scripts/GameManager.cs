using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    private int emptyLocation;
    private int size;

    // Start is called before the first frame update
    void Start()
    {
        size = 3;
        CreateGamePieces(0.01f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //creates the game setup with the size x size pieces
    private void CreateGamePieces(float gapThickness)
    {
        float width = 1/(float)size;

        for(int row = 0; row < size; row++)
        {
            for(int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);

                piece.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

                if((row == size -1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
            }
        }
    }
}
