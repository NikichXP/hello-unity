using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class GenerateWorldScript : MonoBehaviour
{
    public GameObject Player;
    public int GenerationDistance;

    private List<Vector3> _world = new();
    private Dictionary<int, List<int>> _createdBlocks = new();
    private List<Vector3> _bakedVectors;

    
    private BlockingCollection<Vector3> _possibleVerticles = new();
    private List<Vector3> _blocksToCreate = new();
    
    
    private int _counter = 0;
    private Vector3 _lastGenerationPoint = new(-1000, -1000, -1000);

    void Start()
    {
        new Thread(verticlesInitializer).Start();
        _bakedVectors = _bakeVectorMap();
        CreateCubeAt(new Vector3(0, 0, 0));
    }

    // TODO Add chunks of data to not test against every possible block
    void Update()
    {
        if (DistanceBetween(_lastGenerationPoint, Player.transform.position) < Math.Sqrt(GenerationDistance))
        {
            return;
        }

        while (_blocksToCreate.Count > 0)
        {
            var block = _blocksToCreate.Take(1).First();
            CreateCubeAt(block);
            _blocksToCreate.Remove(block);
        }

        var groundOnPlayer = Player.transform.position;
        groundOnPlayer.y = 0;
        var neededPositions = _generatePositionsToCheck(groundOnPlayer);
        neededPositions.ForEach(_possibleVerticles.Add);
        // _createPositions(neededPositions);

        _lastGenerationPoint = Player.transform.position;
    }

    private void verticlesInitializer()
    {
        while (true)
        {
            var verticle = _possibleVerticles.Take();
            if (_world.All(position => position != verticle))
            {
                CreateCubeAt(verticle);
            }
        }
    }

    private List<Vector3> _generatePositionsToCheck(Vector3 position)
    {
        return _bakedVectors.Select(vector => vector + position).ToList();
    }

    private List<Vector3> _bakeVectorMap()
    {
        var neededPositions = new List<Vector3>();
        var roundedPosition = new Vector3(0, 0, 0);
        for (int x = -GenerationDistance; x <= GenerationDistance; x++)
        {
            for (int z = -GenerationDistance; z < GenerationDistance; z++)
            {
                var testVector = new Vector3(x, 0, z);
                if (DistanceBetween(roundedPosition, testVector) <= GenerationDistance)
                {
                    neededPositions.Add(testVector);
                }
            }
        }

        return neededPositions;
    }

    // private void _createPositions(List<Vector3> neededPositions)
    // {
    //     foreach (var position in neededPositions)
    //     {
    //         if (_world.All(obj => obj.transform.position != position))
    //         {
    //             CreateCubeAt(position);
    //         }
    //     }
    // }

    private void CreateCubeAt(Vector3 position)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        _world.Add(cube.transform.position);
        // Debug.Log("Created cube " + _counter++);
    }

    private static Vector3 RoundVector(Vector3 src)
    {
        return new Vector3(
            (float) Math.Round(src.x),
            (float) Math.Round(src.y),
            (float) Math.Round(src.z)
        );
    }

    private static float DistanceBetween(Vector3 a, Vector3 b)
    {
        return (float) Math.Sqrt(Math.Pow((a.x - b.x), 2) + Math.Pow((a.y - b.y), 2) + Math.Pow((a.z - b.z), 2));
    }
}