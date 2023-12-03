using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private List<Structure> structures;
    [SerializeField] private Tilemap walls;
    [SerializeField] private Tilemap ground;
    [SerializeField] private Transform props;
    [SerializeField] private Transform enemies;
    [SerializeField] private BaseLevelTiles baseLevelTiles;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject doorPrefab;

    // exit arrow stuff
    private GameObject door;
    private GameObject player;
    
    private int _width = 64;
    private int _height = 40;
    private Vector3 _enemyOffset = new Vector3(0.5f, 1.0f, 0.0f);
    private int _enemyCount;

    // level music
    public LevelMusic backgroundMusic;

    public ClearBanner banner;

    private void Start()
    {
        GenerateLevel();
        player = GameObject.FindWithTag("Player");
        StartCoroutine(DelayScan());
    }

    /// <summary>
    /// Delays the Pathfinding grid scan until after the level is generated.
    /// If called right after, misses walls and props for some reason, so needs to be delayed a significant
    /// amount of time
    /// </summary>
    private IEnumerator DelayScan()
    {
        yield return new WaitForSeconds(0.1f);
        AstarPath.active.Scan();
    }

    /// <summary>
    /// Keeps track of the number of enemies defeated and opens the door once they all are
    /// </summary>
    public void EnemyDefeated()
    {
        print("Enemy count was " + _enemyCount + ", now " + (_enemyCount - 1));
        _enemyCount -= 1;
        if (_enemyCount <= 0)
        {
            OpenExit();
            // level music change
            backgroundMusic.changeBGM();

            // Show "Clear" Banner 
            banner.ShowBanner();
            StartCoroutine(banner.HideBanner());

        }
    }
    
    /// <summary>
    /// Clear all existing tiles, props, and enemies from the level
    /// </summary>
    private void ClearLevel()
    {
        walls.ClearAllTiles();
        ground.ClearAllTiles();
        List<Transform> propsInScene = new List<Transform>();
        foreach (Transform prop in props)
        {
            propsInScene.Add(prop);
        }
        foreach (Transform prop in propsInScene)
        {
            Destroy(prop.gameObject);
        }

        List<Transform> enemiesInScene = new List<Transform>();
        foreach (Transform enemy in enemies)
        {
            enemiesInScene.Add(enemy);
        }
        foreach (Transform enemy in enemiesInScene)
        {
            Destroy(enemy.gameObject);
        }
    }
    
    /// <summary>
    /// Generate a complete level with borders, structures, and enemies
    /// </summary>
    private void GenerateLevel()
    {
        (List<Rect>, List<Rect>) partitionedLevel = PartitionLevel();
        List<Rect> partitionedAreas = partitionedLevel.Item1;
        List<Rect> borders = partitionedLevel.Item2;
        CreateBaseGroundTiles();
        CreateOutsideWallTiles();
        SpawnStructures(partitionedAreas);
        SpawnInnerBorders(borders);
        AddExit();
        int possibleEnemyCount = GenerateEnemyCount();
        List<Vector3Int> enemyTiles = GenerateEnemyTiles(possibleEnemyCount);
        _enemyCount = enemyTiles.Count;
        SpawnEnemies(enemyTiles);
    }

    /// <summary>
    /// Partition the level into different rooms by randomly drawing lines through the level. Return a list of
    /// rectangles representing the rooms and borders between rooms.
    /// </summary>
    private (List<Rect>, List<Rect>) PartitionLevel()
    {
        List<Rect> partitions = new List<Rect>();
        List<Rect> borders = new List<Rect>();
        partitions.Add(new Rect(0,_height - 1, _width, _height));
        int numPartitions = Random.Range(1, 5);

        for (int i = 0; i < numPartitions; i++)
        {
            // Choose an area to partition, making sure that it is not too small
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
                // Cut vertically through the given area to partition
                int partitionPoint = Random.Range(1, (int) rectToPartition.width - 1);
                Rect leftRect = new Rect(rectToPartition.x, rectToPartition.y, partitionPoint,rectToPartition.height);
                Rect borderRect = new Rect(rectToPartition.x + partitionPoint, rectToPartition.y, 1, rectToPartition.height);
                Rect rightRect = new Rect(rectToPartition.x + partitionPoint + 1, rectToPartition.y,
                    rectToPartition.width - (partitionPoint + 1), rectToPartition.height);
                partitions.Add(leftRect);
                partitions.Add(rightRect);
                borders.Add(borderRect);
            }
            else
            {
                // Cut horizontally through the given area to partition
                int partitionPoint = Random.Range(1, (int) rectToPartition.height - 1);
                Rect topRect = new Rect(rectToPartition.x, rectToPartition.y, rectToPartition.width,partitionPoint);
                Rect borderRect = new Rect(rectToPartition.x, rectToPartition.y - partitionPoint, rectToPartition.width,
                    1);
                Rect bottomRect = new Rect(rectToPartition.x, rectToPartition.y - partitionPoint - 1,
                    rectToPartition.width, rectToPartition.height - partitionPoint - 1); 
                partitions.Add(topRect);
                partitions.Add(bottomRect);
                borders.Add(borderRect);
            }
        }

        partitions = AddBufferToAreas(partitions);
        return (partitions, borders);
    }
    
    /// <summary>
    /// Add a buffer area around an area for a list of areas
    /// </summary>
    List<Rect> AddBufferToAreas(List<Rect> areas)
    {
        List<Rect> bufferedAreas = new List<Rect>();
        foreach (Rect area in areas)
        {
            bufferedAreas.Add(new Rect(area.x + 1,area.y - 1, area.width - 2, area.height - 2));
        }
        return bufferedAreas;
    }
    
    /// <summary>
    /// Fills the ground in and around the level with random ground tiles
    /// </summary>
    private void CreateBaseGroundTiles()
    {
        for (int x = -_width / 2; x < _width * 1.5; x++)
        {
            for (int y = -_height; y < _height * 2; y++)
            {
                Tile groundTile = baseLevelTiles.groundTiles[Random.Range(0, baseLevelTiles.groundTiles.Count)];
                ground.SetTile(new Vector3Int(x,y, 0), groundTile);
            }
        }
    }

    /// <summary>
    /// Creates the outer wall around the level
    /// </summary>
    private void CreateOutsideWallTiles()
    {
        // Create bottom and top walls
        for (int x = 0; x < _width; x++)
        {
            walls.SetTile(new Vector3Int(x, -1, 0), baseLevelTiles.bottomWall);
            walls.SetTile(new Vector3Int(x, _height + 1, 0), baseLevelTiles.upperTopWall);
            walls.SetTile(new Vector3Int(x, _height, 0), baseLevelTiles.lowerTopWall);
        }
        // Create left and right walls
        for (int y = 0; y < _height + 1; y++)
        {
            walls.SetTile(new Vector3Int(-1, y, 0), baseLevelTiles.leftWall);
            walls.SetTile(new Vector3Int(_width, y, 0), baseLevelTiles.rightWall);
        }
        // Create corner walls
        walls.SetTile(new Vector3Int(-1,-1, 0), baseLevelTiles.leftBottomWallCorner);
        walls.SetTile(new Vector3Int(-1,_height + 1, 0), baseLevelTiles.leftTopWallCorner);
        walls.SetTile(new Vector3Int(_width,-1, 0), baseLevelTiles.rightBottomWallCorner);
        walls.SetTile(new Vector3Int(_width,_height + 1, 0), baseLevelTiles.rightTopWallCorner);
        // Modify top and bottom walls tiles to point to spawn/end jut outs
        walls.SetTile(new Vector3Int( (_width / 2) - 3,-1, 0), baseLevelTiles.bottomRightWallCorner);
        walls.SetTile(new Vector3Int( (_width / 2) + 2,-1, 0), baseLevelTiles.bottomLeftWallCorner);
        walls.SetTile(new Vector3Int( (_width / 2) - 3,_height, 0), baseLevelTiles.lowerTopLeftCorner);
        walls.SetTile(new Vector3Int( (_width / 2) - 3,_height + 1, 0), baseLevelTiles.upperTopLeftCorner);
        walls.SetTile(new Vector3Int( (_width / 2) + 2,_height, 0), baseLevelTiles.lowerTopRightCorner);
        walls.SetTile(new Vector3Int( (_width / 2) + 2,_height + 1, 0), baseLevelTiles.upperTopRightCorner);
        // Clear wall in front of jut outs and create new top and bottom walls
        for (int x = _width / 2 - 2; x < _width / 2 + 2; x++)
        {
            walls.SetTile(new Vector3Int(x, -1, 0), null);
            walls.SetTile(new Vector3Int(x, -5, 0), baseLevelTiles.bottomWall);
            
            walls.SetTile(new Vector3Int(x, _height, 0), null);
            walls.SetTile(new Vector3Int(x, _height + 1, 0), null);
            walls.SetTile(new Vector3Int(x, _height + 4, 0), baseLevelTiles.lowerTopWall);
            walls.SetTile(new Vector3Int(x, _height + 5, 0), baseLevelTiles.upperTopWall);
        }
        // Create new left and right walls for jut outs
        for (int y = -2; y > -5; y--)
        {
            walls.SetTile(new Vector3Int(_width / 2 - 3, y, 0), baseLevelTiles.leftWall);
            walls.SetTile(new Vector3Int(_width / 2 + 2, y, 0), baseLevelTiles.rightWall);
        }
        for (int y = _height + 2; y < _height + 5; y++)
        {
            walls.SetTile(new Vector3Int(_width / 2 - 3, y, 0), baseLevelTiles.leftWall);
            walls.SetTile(new Vector3Int(_width / 2 + 2, y, 0), baseLevelTiles.rightWall);
        }
        // Add jut out corners
        walls.SetTile(new Vector3Int(_width / 2 - 3, -5, 0), baseLevelTiles.leftBottomWallCorner);
        walls.SetTile(new Vector3Int(_width / 2 + 2, -5, 0), baseLevelTiles.rightBottomWallCorner);
        walls.SetTile(new Vector3Int(_width / 2 - 3, _height + 5, 0), baseLevelTiles.leftTopWallCorner);
        walls.SetTile(new Vector3Int(_width / 2 + 2, _height + 5, 0), baseLevelTiles.rightTopWallCorner);
        
    }
    
    /// <summary>
    /// Recursively fills a partitioned level with random structures
    /// </summary>
    private void SpawnStructures(List<Rect> partitionedLevel)
    {
        foreach (Rect partitionedArea in partitionedLevel)
        {
            Structure structureToGen = ChooseStructureToPlace(partitionedArea);
            if (structureToGen == null)
            {
                continue;
            }
            SpawnStructures(SpawnStructure(structureToGen, partitionedArea));
        }
    }
    
    /// <summary>
    /// Spawns a given structure in the level in a given area. Returns a list of areas where there is still room to
    /// place more structures
    /// </summary>
    private List<Rect> SpawnStructure(Structure structureToGen, Rect partitionedArea)
    {
        // Choose offsets to randomize the placement of the structure in the area
        int widthOffset = Random.Range(0, (int) (partitionedArea.width - structureToGen.width));
        int heightOffset = Random.Range(-(int) (partitionedArea.height - structureToGen.height), 0);
        
        GameObject structure = Instantiate(structureToGen.structure, new Vector3(partitionedArea.x + widthOffset, partitionedArea.y + heightOffset, 0),
            Quaternion.identity);
        Tilemap structWallsTilemap = structure.transform.Find("Walls").GetComponent<Tilemap>();
        Tilemap structGroundTilemap = structure.transform.Find("Ground").GetComponent<Tilemap>();
        // Copy the ground and wall tiles from the structure to corresponding tilemaps of the level generator
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
        // Re-parent the props from the structure to the level generator
        Transform structProps = structure.transform.Find("Props");
        List<Transform> childProps = new List<Transform>();
        foreach (Transform prop in structProps.transform)
        {
            childProps.Add(prop);
        }
        foreach (Transform childProp in childProps)
        {
            childProp.SetParent(props);
        }
        
        Destroy(structure);
        return GetRemainingAreas(partitionedArea, structureToGen, widthOffset, heightOffset);
    }

    /// <summary>
    /// Get the remaining areas in a partitioned room after placing a structure inside of it with a given width and
    /// height offset
    /// </summary>
    List<Rect> GetRemainingAreas(Rect initialArea, Structure generatedStructure, int widthOffset, int heightOffset)
    {
        // Get the areas to the right and left of where the structure was placed
        if (initialArea.width - generatedStructure.width > initialArea.height - generatedStructure.height)
        {
            return new List<Rect>
            {
                new(initialArea.x, initialArea.y, Mathf.Max(0,widthOffset - 1), initialArea.height),
                new(initialArea.x + widthOffset + generatedStructure.width + 1, initialArea.y,
                    Mathf.Max(0,initialArea.width - widthOffset - generatedStructure.width - 1), initialArea.height)
            };
        }
        // Get the areas above and below where the structure was placed
        else
        {
            return new List<Rect>
            {
                new(initialArea.x, initialArea.y, initialArea.width, Mathf.Max(0,Mathf.Abs(heightOffset) - 1)),
                new(initialArea.x, initialArea.y + heightOffset - generatedStructure.height - 1,
                    initialArea.width, Mathf.Max(0,initialArea.height + heightOffset - generatedStructure.height - 1))
            };
        }
    }

    /// <summary>
    /// Choose a random structure to place that will fit within the given area
    /// </summary>
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

    /// <summary>
    /// Create the inner level borders to separate partitioned rooms
    /// </summary>
    private void SpawnInnerBorders(List<Rect> borders)
    {
        foreach (Rect border in borders)
        {
            // Create the borders
            for (int i = 0; i < border.width; i++)
            {
                for (int j = 0; j > -border.height; j--)
                {
                    walls.SetTile(
                        new Vector3Int((int) border.x + i, (int) border.y + j, 0), baseLevelTiles.inLevelRoomBorder);
                }
            }
            // Add openings in each border in order to connect the rooms
            List<Vector3Int> tiles = GenerateTilesFromRect(border);
            for (int i = 0; i < 3; i++)
            {
                int randomTileIndexToRemove = Random.Range(1, tiles.Count - 1);
                walls.SetTile(tiles[randomTileIndexToRemove], null);
                walls.SetTile(tiles[randomTileIndexToRemove - 1], null);
                walls.SetTile(tiles[randomTileIndexToRemove + 1], null);
            }
        }
    }

    /// <summary>
    /// Generate a list of tiles from a given area defined by a rect
    /// </summary>
    private List<Vector3Int> GenerateTilesFromRect(Rect rect)
    {
        List<Vector3Int> tiles = new List<Vector3Int>();
        for (int i = (int) rect.x; i < rect.x + rect.width; i++)
        {
            for (int j = (int) rect.y; j > rect.y - rect.height; j--)
            {
                tiles.Add(new Vector3Int(i, j, 0));
            }
        }

        return tiles;
    }
    
    /// <summary>
    /// Generate the amount of enemies that should generate on the current level
    /// </summary>
    private int GenerateEnemyCount()
    {
        int minEnemies = Mathf.Min(10,Mathf.Max(2, StaticData.level - 3));
        int maxEnemies = Mathf.Min(StaticData.level + 2, 11);
        return Random.Range(minEnemies, maxEnemies);
    }

    /// <summary>
    /// Generate a list of tile positions for the given numEnemies
    /// </summary>
    private List<Vector3Int> GenerateEnemyTiles(int numEnemies)
    {
        List<Vector3Int> possibleEnemyTiles = FindEmptyTiles();
        List<Vector3Int> enemyTiles = new List<Vector3Int>();
        for (int i = 0; i < Math.Min(numEnemies, possibleEnemyTiles.Count); i++)
        {
            int randomTileIndex = Random.Range(0, possibleEnemyTiles.Count);
            Vector3Int enemyTile = possibleEnemyTiles[randomTileIndex];
            enemyTiles.Add(enemyTile);
            possibleEnemyTiles.RemoveAt(randomTileIndex);
        }
        return enemyTiles;
    }
    
    /// <summary>
    /// Finds all tiles in the level that are not occupied by props or wall tiles
    /// </summary>
    private List<Vector3Int> FindEmptyTiles()
    {
        // Get tiles that are not occupied by walls
        HashSet<Vector3Int> tiles = new HashSet<Vector3Int>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (walls.GetTile(tilePos) == null)
                {
                    tiles.Add(tilePos);
                }
            }
        }

        int i = 0;
        // Remove tiles that are occupied by props
        foreach (Transform prop in props.transform)
        {
            if (i >= 1)
            {
                continue;
            }

            i += 2;
            Vector3 propPosition = prop.position;
            List<Vector3Int> invalidPositions = GenerateTilesFromRect(new Rect(propPosition.x - 1.5f, propPosition.y + 1.0f, 3,3));
            foreach (Vector3Int invalidPosition in invalidPositions)
            {
                if (tiles.Contains(invalidPosition))
                {
                    tiles.Remove(invalidPosition);
                }
            }
        }
        return tiles.ToList();
    }
    
    /// <summary>
    /// Spawns enemies on the given location tiles and adds perks to them based on the current level
    /// </summary>
    private void SpawnEnemies(List<Vector3Int> locations)
    {
        List<Tuple<int, int>> possiblePerks = Perks.GetValidEnemyPerks();
        int powerBudget = 10 + 30 * StaticData.level;
        List<Perks> enemyPerksList = new List<Perks>();
        foreach (Vector3Int location in locations)
        {
            GameObject enemy = Instantiate(enemyPrefab, location + _enemyOffset, Quaternion.identity, enemies);
            enemy.GetComponent<Health>().maxHealth += (StaticData.level - 1) * 10;
            enemy.GetComponent<Health>().currentHealth = enemy.GetComponent<Health>().maxHealth;
            enemyPerksList.Add(enemy.GetComponent<Perks>());
        }
        // Add perks until the power budget is used up
        while (powerBudget > 0)
        {
            Tuple<int, int> selectedPerk = possiblePerks[Random.Range(0, possiblePerks.Count)];
            enemyPerksList[Random.Range(0, enemyPerksList.Count)].AddPerk(selectedPerk.Item1);
            powerBudget -= selectedPerk.Item2;
        }
    }

    /// <summary>
    /// Creates the exit door for the level and blocks it off
    /// </summary>
    private void AddExit()
    {
        door = Instantiate(doorPrefab, new Vector3(_width / 2,_height + 2.5f, 0), Quaternion.identity);
        ChangeScene changeScene = door.GetComponent<ChangeScene>();
        changeScene.sceneBuildIndex = 2;
        for (int x = _width / 2 - 2; x < _width / 2 + 2; x++)
        {
            walls.SetTile(new Vector3Int(x, _height, 0), baseLevelTiles.exitDoor);
        }
    }

    /// <summary>
    /// Un-blocks the exit door of the level
    /// </summary>
    private void OpenExit()
    {
        for (int x = _width / 2 - 2; x < _width / 2 + 2; x++)
        {
            walls.SetTile(new Vector3Int(x, _height, 0), null);
        }
        // give exitArrow exit coords and enable it
        // Check if the parentObject is found
        if (player != null)
        {
            // Find the child object by name
            Transform childTransform = player.transform.Find("ExitArrow");

            // Check if the childTransform is found
            if (childTransform != null)
            {
                // Enable the child GameObject
                childTransform.gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("No child object found with the specified name.");
            }
        }
        else
        {
            Debug.LogWarning("No object found with the specified tag.");
        }
    }
}
