using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<Structure> structures;
    [SerializeField] private Tilemap walls;
    [SerializeField] private Tilemap ground;
    private int _width = 32;
    private int _height = 20;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            GenerateLevel();
        }
    }

    private void GenerateLevel()
    {
        List<Vector2Int> tiles = GenerateBaseTiles();
        List<Rect> partitionedLevel = PartitionLevel();
        List<Vector2Int> enemyTiles = GenerateEnemyTiles(tiles, 0);

        Texture2D texture = GenerateTexture(enemyTiles, partitionedLevel);
        SpawnStructures(partitionedLevel);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite =
            Sprite.Create(texture, new Rect(0, 0, _width, _height), new Vector2(0.5f, 0.5f));
        spriteRenderer.sprite.texture.filterMode = FilterMode.Point;
    }

    private void SpawnStructures(List<Rect> partitionedLevel)
    {
        foreach (Rect partitionedArea in partitionedLevel)
        {
            Structure structureToGen = ChooseStructureToPlace(partitionedArea);
            if (structureToGen == null)
            {
                continue;
            }
            SpawnStructure(structureToGen, partitionedArea);
        }
    }

    private void SpawnStructure(Structure structureToGen, Rect partitionedArea)
    {
        GameObject structure = Instantiate(structureToGen.structure, new Vector3(partitionedArea.x, partitionedArea.y, 0),
            Quaternion.identity);
        Transform structWalls = structure.transform.Find("Walls");
        Tilemap structWallsTilemap = structWalls.GetComponent<Tilemap>();
        Transform structGround = structure.transform.Find("Ground");
        Tilemap structGroundTilemap = structGround.GetComponent<Tilemap>();
        int widthOffset = Random.Range(0, (int) (partitionedArea.width - structureToGen.width));
        int heightOffset = Random.Range(-(int) (partitionedArea.height - structureToGen.height), 0);
        for (int i = 0; i < structureToGen.width; i++)
        {
            for (int j = 0; j > -structureToGen.height; j--)
            {
                TileBase wallTile = structWallsTilemap.GetTile(new Vector3Int(i, j, 0));
                TileBase groundTile = structGroundTilemap.GetTile(new Vector3Int(i, j, 0));
                if (wallTile != null)
                {
                    walls.SetTile(new Vector3Int((int) partitionedArea.x + i + widthOffset, (int) partitionedArea.y + j + heightOffset, 0), wallTile);
                }

                if (groundTile != null)
                {
                    ground.SetTile(new Vector3Int((int) partitionedArea.x + i + widthOffset, (int) partitionedArea.y + j + heightOffset, 0), groundTile);
                }
            }
        }

        Destroy(structWalls.gameObject);
        Destroy(structGround.gameObject);
    }

    Structure ChooseStructureToPlace(Rect area)
    {
        List<Structure> validStructs = new List<Structure>();
        for (int i = 0; i < structures.Count; i++)
        {
            if ((int) area.width >= structures[i].width && (int) area.height >= structures[i].height)
            {
                validStructs.Add(structures[i]);
            }
        }

        if (validStructs.Count > 0)
        {
            return validStructs[Random.Range(0, validStructs.Count)];
        }
        return null;
    }

    List<Rect> PartitionLevel()
    {
        List<Rect> partitions = new List<Rect>();
        partitions.Add(new Rect(0,_height - 1, _width, _height));
        //int numPartitions = Random.Range(1, 5);
        int numPartitions = 4;
        
        for (int i = 0; i < numPartitions; i++)
        {
            Rect rectToPartition = Rect.zero;
            int indexOfPartition = -1;
            while (rectToPartition.width <= 2 || rectToPartition.height <= 2)
            {
                indexOfPartition = Random.Range(0, partitions.Count);
                rectToPartition = partitions[indexOfPartition];
            }
            partitions.RemoveAt(indexOfPartition);
            if (i % 2 == 0)
            {
                // vertical
                int partitionPoint = Random.Range(1, (int) rectToPartition.width - 1);
                Rect leftRect = new Rect(rectToPartition.x, rectToPartition.y, partitionPoint,rectToPartition.height);
                Rect rightRect = new Rect(rectToPartition.x + partitionPoint, rectToPartition.y,
                    rectToPartition.width - partitionPoint, rectToPartition.height);
                partitions.Add(leftRect);
                partitions.Add(rightRect);
            }
            else
            {
                //horizontal
                int partitionPoint = Random.Range(1, (int) rectToPartition.height - 1);
                Rect topRect = new Rect(rectToPartition.x, rectToPartition.y, rectToPartition.width,partitionPoint);
                Rect bottomRect = new Rect(rectToPartition.x, rectToPartition.y - partitionPoint,
                    rectToPartition.width, rectToPartition.height - partitionPoint);
                partitions.Add(topRect);
                partitions.Add(bottomRect);
            }
        }
        
        return partitions;
    }

    List<Vector2Int> GenerateTilesFromRect(Rect rect)
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        for (int i = (int) rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = (int) rect.y; j > rect.y - rect.height; j--)
            {
                tiles.Add(new Vector2Int(i, j));
            }
        }

        return tiles;
    }

    Texture2D GenerateTexture(List<Vector2Int> enemyTiles, List<Rect> partitionedLevel)
    {
        Color baseColor = new Color(1, 1, 1);
        Color enemyColor = new Color(1, 0, 0);
        Texture2D texture = new Texture2D(_width, _height);
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                texture.SetPixel(x, y, baseColor);
            }
        }

        Color[] regionColours = new[] {
            new Color(0.0f, 0.5f, 1f),
            new Color(1.0f,0.5f,1f),
            new Color(0.7f,0.5f,0.0f),
            new Color(0.6f,0.1f,0.6f),
            new Color(0.0f,0.9f,0.6f),
        };
        int i = 0;
        foreach (Rect rect in partitionedLevel)
        {
            Color color = regionColours[i];
            i++;
            foreach (Vector2Int tile in GenerateTilesFromRect(rect))
            {
                texture.SetPixel(tile.x, tile.y, color);
            }
        }
        foreach (var enemyTile in enemyTiles)
        {
            texture.SetPixel(enemyTile.x, enemyTile.y, enemyColor);
        }
        texture.Apply();
        return texture;
    }

    List<Vector2Int> GenerateBaseTiles()
    {
        List<Vector2Int> tiles = new List<Vector2Int>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                tiles.Add(new Vector2Int(x, y));
            }
        }
        return tiles;
    }

    List<Vector2Int> GenerateEnemyTiles(List<Vector2Int> tiles, int numEnemies)
    {
        List<Vector2Int> enemyTiles = new List<Vector2Int>();
        for (int i = 0; i < numEnemies; i++)
        {
            Vector2Int tile = tiles[Random.Range(0, tiles.Count)];
            enemyTiles.Add(tile);
        }
        return enemyTiles;
    }
}
