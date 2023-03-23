using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    private int emptyLocation;
    public int size = 3;

    // Start is called before the first frame update
    void Start()
    {
        CreateGamePieces(0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //creates the game setup with the size x size pieces
    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f/size;

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
                else
                {
                    float gap = gapThickness / 2f;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    //cord order: (0,1), (1,1), (0,0), (1,0)
                    uv[0] = new Vector2((width * col), 1 - ((width * (row + 1))));
                    uv[1] = new Vector2((width * (col + 1)), 1 - ((width * (row + 1))));
                    uv[2] = new Vector2((width * col), (width * row));
                    uv[3] = new Vector2((width * (col + 1)), 1 - ((width * row)));

                    mesh.uv = uv;
                }
            }
        }
    }
}
