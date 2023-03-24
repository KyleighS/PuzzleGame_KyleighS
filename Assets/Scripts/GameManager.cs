using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Transform gameTransform;
    [SerializeField] private Transform piecePrefab;

    private List<Transform> pieces;
    private int emptyLocation;
    public int size = 3;
    private bool shuffling = false;

    // Start is called before the first frame update
    void Start()
    {
        pieces = new List<Transform>();
        CreateGamePieces(0.04f);
    }

    // Update is called once per frame
    void Update()
    {
        //checks for completion
        if (!shuffling && CheckCompletion())
        {
            shuffling = true;
            StartCoroutine(WaitShuffle(0.5F));
        }


        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        //checks each direction to see if valid move
                        //break once success
                        if (SwapIfVaild(i, -size, size)) { break; }
                        if (SwapIfVaild(i, +size, size)) { break; }
                        if (SwapIfVaild(i, -1, 0)) { break; }
                        if (SwapIfVaild(i, +1, size - 1)) { break; }
                    }
                }
            }
        }
    }

    //used to stop horzontal moves wrapping
    private bool SwapIfVaild(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            //swaps the pieces in the game state
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            //swap their transform
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));

            emptyLocation = i;
            return true;
        }
        return false;
    }
    //creates the game setup with the size x size pieces
    private void CreateGamePieces(float gapThickness)
    {
        float width = 1f / size;

        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);

                piece.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

                if ((row == size - 1) && (col == size - 1))
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
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));

                    mesh.uv = uv;
                }
            }
        }
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;

        while (count < (size * size * size))
        {
            //pick a random location
            int rnd = Random.Range(0, size * size);

            //only thing we forbid is undoing the last move
            if (rnd == last) { continue; }
            last = emptyLocation;

            //looks for valid omves
            if (SwapIfVaild(rnd, -size, size))
            {
                count++;
            }
            else if (SwapIfVaild(rnd, +size, size))
            {
                count++;
            }
            else if (SwapIfVaild(rnd, -1, 0))
            {
                count++;
            }
            else if (SwapIfVaild(rnd, +1, size - 1))
            {
                count++;
            }
        }
    }
}
