using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Make your generation algorithm here
public class Generate : MonoBehaviour {

    public TileType[] tileTypes; // all the tiletypes given in the editor
    public GameObject player;
    public GameObject enemy;

    // the size of the generated map
    private int mapSizeX = 10;
    private int mapSizeY = 10;

    // apumuuttuja satunnaisten tilejen luomiselle
    private int temp;

    // Lista kartan peliobjekteista
    public List<GameObject> tiles = new List<GameObject>();

    // Apumuuttujaobjekti
    public GameObject current;

    void Start()
    {
        GenerateMap();
        ValidateMap();
    }

    void Update()
    {
        // Scenen voi ladata uudestaan == tason voi regeneroida painamalla "R" painiketta
        if (Input.GetButtonDown("Regenerate")) SceneManager.LoadScene("Generointi");
        // Ohjelman voi sulkea painamalla "escape" painiketta
        if (Input.GetButtonDown("Quit")) Application.Quit();
    }

    // Generoidaan kartta
    void GenerateMap()
    {
        for (int x = 0; x < mapSizeX; x++)
        {
            for (int y = 0; y < mapSizeY; y++)
            {
                // Int random range on minInclusive ja maxExclusive
                temp = Random.Range(0, 2);
                TileType tt = tileTypes[temp];

                // Alkupiste aina tason vasempaan alalaitaan
                if (x == 0 && y == 0)
                {
                    tt = tileTypes[2];
                }
                // Loppupiste aina tason oikeaan ylälaitaan
                else if (x == 9 && y == 9)
                {
                    tt = tileTypes[3];
                }
                GameObject go = Instantiate(tt.tileVisual, new Vector3(x, y, 0), Quaternion.identity);
                tiles.Add(go);
            }
        }
        GameObject newPlayer = Instantiate(player, new Vector3(0,0,-0.5f),Quaternion.identity); //spawn one player
        GameObject newEnemy = Instantiate(enemy, new Vector3(9, 9, -0.5f), Quaternion.identity); //and one enemy
    }

    // Valitoidaan kartta läpikuljettavaksi
    void ValidateMap()
    {
        // Luodaan apumuuttujat
        int tempX = 0;
        int tempY = 0;

        // Satunnaisesti aloitetaan jommasta kummasta lähtöruudun naapurista
        temp = Random.Range(0, 2);
        if (temp == 0) tempX++;
        if (temp == 1) tempY++;

        // Laitetaan positio muistiin
        Vector3 position = new Vector3(tempX, tempY, 0);

        // Haetaan tile
        foreach (GameObject tile in tiles)
        {
            if (tile.transform.position == position)
            {
                // Varmistetaan että tile on maata
                Destroy(tile);
                current = Instantiate(tileTypes[0].tileVisual, position, Quaternion.identity);
                break;
            }
        }

        // Lähdetään käymään kenttää läpi
        while (tempX <= 9 || tempY <= 9)
        {
            // Jos jompi kumpi akseli saavuttaa päädyn
            if ((tempX == 9 && tempY == 8) || (tempY == 9 && tempX == 8)) break;
            // Jos X-akseli saavuttaa päädyn
            else if (tempX == 9) tempY++;
            // Jos Y-akseli saavuttaa päädyn
            else if (tempY == 9) tempX++;
            // Mikäli kumpikaan akseli ei ole saavuttanut päätyä
            else if (tempX < 9 && tempY < 9)
            {
                // Arvotaan uusi suunta mihin edetään
                temp = Random.Range(0, 2);
                if (temp == 0) tempX++;
                if (temp == 1) tempY++;
            }

            // Vaihdetaan positio sen mukaiseksi
            position = new Vector3(tempX, tempY, 0);

            // Käydään lista läpi, etsitään oikeassa kohdassa oleva tiili, tuhotaan se ja korvataan se polulla
            foreach (GameObject tile in tiles)
            {
                if (tile.transform.position == position)
                {
                    Destroy(tile);
                    current = Instantiate(tileTypes[0].tileVisual, position, Quaternion.identity);
                    break;
                }
            }
        }
    }
}
