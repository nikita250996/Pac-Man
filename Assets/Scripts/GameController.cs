using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts
{
    public class GameController : MonoBehaviour
    {
        public Energizer[] Energizers;

        private readonly List<Vector3> _possiblePositions = new List<Vector3>
        {
            //y == 2
            new Vector3(2, 2, 0), new Vector3(3, 2, 0),
            new Vector3(4, 2, 0), new Vector3(5, 2, 0),
            new Vector3(6, 2, 0), new Vector3(7, 2, 0),
            new Vector3(8, 2, 0), new Vector3(9, 2, 0),
            new Vector3(10, 2, 0), new Vector3(11, 2, 0),
            new Vector3(12, 2, 0), new Vector3(13, 2, 0),
            new Vector3(14, 2, 0), new Vector3(15, 2, 0),
            new Vector3(16, 2, 0), new Vector3(17, 2, 0),
            new Vector3(18, 2, 0), new Vector3(19, 2, 0),
            new Vector3(20, 2, 0), new Vector3(21, 2, 0),
            new Vector3(22, 2, 0), new Vector3(23, 2, 0),
            new Vector3(24, 2, 0), new Vector3(25, 2, 0),
            new Vector3(26, 2, 0), new Vector3(27, 2, 0),
            //y == 3
            new Vector3(2, 3, 0), new Vector3(7, 3, 0),
            new Vector3(13, 3, 0), new Vector3(16, 3, 0),
            new Vector3(22, 3, 0), new Vector3(27, 3, 0),
            //y == 4
            new Vector3(2, 4, 0), new Vector3(7, 4, 0),
            new Vector3(13, 4, 0), new Vector3(16, 4, 0),
            new Vector3(22, 4, 0), new Vector3(27, 4, 0),
            //y == 5
            new Vector3(2, 5, 0), new Vector3(3, 5, 0),
            new Vector3(4, 5, 0), new Vector3(5, 5, 0),
            new Vector3(6, 5, 0), new Vector3(7, 5, 0),
            new Vector3(10, 5, 0), new Vector3(11, 5, 0),
            new Vector3(12, 5, 0), new Vector3(13, 5, 0),
            new Vector3(16, 5, 0), new Vector3(17, 5, 0),
            new Vector3(18, 5, 0), new Vector3(19, 5, 0),
            new Vector3(22, 5, 0), new Vector3(23, 5, 0),
            new Vector3(24, 5, 0), new Vector3(25, 5, 0),
            new Vector3(26, 5, 0), new Vector3(27, 5, 0),
            //y == 6
            new Vector3(4, 6, 0), new Vector3(7, 6, 0),
            new Vector3(10, 6, 0), new Vector3(19, 6, 0),
            new Vector3(22, 6, 0), new Vector3(25, 6, 0),
            //y == 7
            new Vector3(4, 7, 0), new Vector3(7, 7, 0),
            new Vector3(10, 7, 0), new Vector3(19, 7, 0),
            new Vector3(22, 7, 0), new Vector3(25, 7, 0),
            //y == 8
            new Vector3(2, 8, 0), new Vector3(3, 8, 0),
            new Vector3(4, 8, 0), new Vector3(7, 8, 0),
            new Vector3(8, 8, 0), new Vector3(9, 8, 0),
            new Vector3(10, 8, 0), new Vector3(11, 8, 0),
            new Vector3(12, 8, 0), new Vector3(13, 8, 0),
            new Vector3(16, 8, 0), new Vector3(17, 8, 0),
            new Vector3(18, 8, 0), new Vector3(19, 8, 0),
            new Vector3(20, 8, 0), new Vector3(21, 8, 0),
            new Vector3(22, 8, 0), new Vector3(25, 8, 0),
            new Vector3(26, 8, 0), new Vector3(27, 8, 0),
            //y == 9
            new Vector3(2, 9, 0), new Vector3(7, 9, 0),
            new Vector3(13, 9, 0), new Vector3(16, 9, 0),
            new Vector3(22, 9, 0), new Vector3(27, 9, 0),
            //y == 10
            new Vector3(2, 10, 0), new Vector3(7, 10, 0),
            new Vector3(13, 10, 0), new Vector3(16, 10, 0),
            new Vector3(22, 10, 0), new Vector3(27, 10, 0),
            //y == 11
            new Vector3(2, 11, 0), new Vector3(3, 11, 0),
            new Vector3(4, 11, 0), new Vector3(5, 11, 0),
            new Vector3(6, 11, 0), new Vector3(7, 11, 0),
            new Vector3(8, 11, 0), new Vector3(9, 11, 0),
            new Vector3(10, 11, 0), new Vector3(11, 11, 0),
            new Vector3(12, 11, 0), new Vector3(13, 11, 0),
            new Vector3(14, 11, 0), new Vector3(15, 11, 0),
            new Vector3(16, 11, 0), new Vector3(17, 11, 0),
            new Vector3(18, 11, 0), new Vector3(19, 11, 0),
            new Vector3(20, 11, 0), new Vector3(21, 11, 0),
            new Vector3(22, 11, 0), new Vector3(23, 11, 0),
            new Vector3(24, 11, 0), new Vector3(25, 11, 0),
            new Vector3(26, 11, 0), new Vector3(27, 11, 0),
            //y == 12
            new Vector3(4, 12, 0), new Vector3(7, 12, 0),
            new Vector3(10, 12, 0), new Vector3(19, 12, 0),
            new Vector3(22, 12, 0), new Vector3(25, 12, 0),
            //y = 13
            new Vector3(4, 13, 0), new Vector3(7, 13, 0),
            new Vector3(10, 13, 0), new Vector3(19, 13, 0),
            new Vector3(22, 13, 0), new Vector3(25, 13, 0),
            //y == 14
            new Vector3(2, 14, 0), new Vector3(3, 14, 0),
            new Vector3(4, 14, 0), new Vector3(7, 14, 0),
            new Vector3(10, 14, 0), new Vector3(11, 14, 0),
            new Vector3(12, 14, 0), new Vector3(16, 14, 0),
            new Vector3(17, 14, 0), new Vector3(18, 14, 0),
            new Vector3(19, 14, 0), new Vector3(22, 14, 0),
            new Vector3(25, 14, 0), new Vector3(26, 14, 0),
            new Vector3(27, 14, 0),
            //y == 15
            new Vector3(2, 15, 0), new Vector3(7, 15, 0),
            new Vector3(10, 15, 0), new Vector3(19, 15, 0),
            new Vector3(22, 15, 0), new Vector3(27, 15, 0),
            //y == 16
            new Vector3(2, 16, 0), new Vector3(7, 16, 0),
            new Vector3(10, 16, 0), new Vector3(19, 16, 0),
            new Vector3(22, 16, 0), new Vector3(27, 16, 0),
            //y == 17
            new Vector3(2, 17, 0), new Vector3(3, 17, 0),
            new Vector3(4, 17, 0), new Vector3(5, 17, 0),
            new Vector3(6, 17, 0), new Vector3(7, 17, 0),
            new Vector3(10, 17, 0), new Vector3(19, 17, 0),
            new Vector3(22, 17, 0), new Vector3(23, 17, 0),
            new Vector3(24, 17, 0), new Vector3(25, 17, 0),
            new Vector3(26, 17, 0), new Vector3(27, 17, 0),
            //y == 18
            new Vector3(2, 18, 0), new Vector3(7, 18, 0),
            new Vector3(10, 18, 0), new Vector3(19, 18, 0),
            new Vector3(22, 18, 0), new Vector3(27, 18, 0),
            //y == 19
            new Vector3(2, 19, 0), new Vector3(7, 19, 0),
            new Vector3(10, 19, 0), new Vector3(19, 19, 0),
            new Vector3(22, 19, 0), new Vector3(27, 19, 0),
            //y == 20
            new Vector3(2, 20, 0), new Vector3(3, 20, 0),
            new Vector3(4, 20, 0), new Vector3(7, 20, 0),
            new Vector3(8, 20, 0), new Vector3(9, 20, 0),
            new Vector3(10, 20, 0), new Vector3(11, 20, 0),
            new Vector3(12, 20, 0), new Vector3(13, 20, 0),
            new Vector3(14, 20, 0), new Vector3(15, 20, 0),
            new Vector3(16, 20, 0), new Vector3(17, 20, 0),
            new Vector3(18, 20, 0), new Vector3(19, 20, 0),
            new Vector3(20, 20, 0), new Vector3(21, 20, 0),
            new Vector3(22, 20, 0), new Vector3(25, 20, 0),
            new Vector3(26, 20, 0), new Vector3(27, 20, 0),
            //y == 21
            new Vector3(4, 21, 0), new Vector3(7, 21, 0),
            new Vector3(13, 21, 0), new Vector3(16, 21, 0),
            new Vector3(22, 21, 0), new Vector3(25, 21, 0),
            //y == 22
            new Vector3(4, 22, 0), new Vector3(7, 22, 0),
            new Vector3(13, 22, 0), new Vector3(16, 22, 0),
            new Vector3(22, 22, 0), new Vector3(25, 22, 0),
            //y == 23
            new Vector3(2, 23, 0), new Vector3(3, 23, 0),
            new Vector3(4, 23, 0), new Vector3(5, 23, 0),
            new Vector3(6, 23, 0), new Vector3(7, 23, 0),
            new Vector3(10, 23, 0), new Vector3(11, 23, 0),
            new Vector3(12, 23, 0), new Vector3(13, 23, 0),
            new Vector3(16, 23, 0), new Vector3(17, 23, 0),
            new Vector3(18, 23, 0), new Vector3(19, 23, 0),
            new Vector3(22, 23, 0), new Vector3(23, 23, 0),
            new Vector3(24, 23, 0), new Vector3(25, 23, 0),
            new Vector3(26, 23, 0), new Vector3(27, 23, 0),
            //y == 24
            new Vector3(2, 24, 0), new Vector3(7, 24, 0),
            new Vector3(10, 24, 0), new Vector3(19, 24, 0),
            new Vector3(22, 24, 0), new Vector3(27, 24, 0),
            //y == 25
            new Vector3(2, 25, 0), new Vector3(7, 25, 0),
            new Vector3(10, 25, 0), new Vector3(19, 25, 0),
            new Vector3(22, 25, 0), new Vector3(27, 25, 0),
            //y == 26
            new Vector3(2, 26, 0), new Vector3(3, 26, 0),
            new Vector3(4, 26, 0), new Vector3(5, 26, 0),
            new Vector3(6, 26, 0), new Vector3(7, 26, 0),
            new Vector3(8, 26, 0), new Vector3(9, 26, 0),
            new Vector3(10, 26, 0), new Vector3(11, 26, 0),
            new Vector3(12, 26, 0), new Vector3(13, 26, 0),
            new Vector3(16, 26, 0), new Vector3(17, 26, 0),
            new Vector3(18, 26, 0), new Vector3(19, 26, 0),
            new Vector3(20, 26, 0), new Vector3(21, 26, 0),
            new Vector3(22, 26, 0), new Vector3(23, 26, 0),
            new Vector3(24, 26, 0), new Vector3(25, 26, 0),
            new Vector3(26, 26, 0), new Vector3(27, 26, 0),
            //y == 27
            new Vector3(2, 27, 0), new Vector3(7, 27, 0),
            new Vector3(13, 27, 0), new Vector3(16, 27, 0),
            new Vector3(22, 27, 0), new Vector3(27, 27, 0),
            //y == 28
            new Vector3(2, 28, 0), new Vector3(7, 28, 0),
            new Vector3(13, 28, 0), new Vector3(14, 28, 0),
            new Vector3(15, 28, 0), new Vector3(16, 28, 0),
            new Vector3(22, 28, 0), new Vector3(27, 28, 0),
            //y == 29
            new Vector3(2, 29, 0), new Vector3(7, 29, 0),
            new Vector3(13, 29, 0), new Vector3(16, 29, 0),
            new Vector3(22, 29, 0), new Vector3(27, 29, 0),
            //y == 30
            new Vector3(2, 30, 0), new Vector3(3, 30, 0),
            new Vector3(4, 30, 0), new Vector3(5, 30, 0),
            new Vector3(6, 30, 0), new Vector3(7, 30, 0),
            new Vector3(8, 30, 0), new Vector3(9, 30, 0),
            new Vector3(10, 30, 0), new Vector3(11, 30, 0),
            new Vector3(12, 30, 0), new Vector3(13, 30, 0),
            new Vector3(16, 30, 0), new Vector3(17, 30, 0),
            new Vector3(18, 30, 0), new Vector3(19, 30, 0),
            new Vector3(20, 30, 0), new Vector3(21, 30, 0),
            new Vector3(22, 30, 0), new Vector3(23, 30, 0),
            new Vector3(24, 30, 0), new Vector3(25, 30, 0),
            new Vector3(26, 30, 0), new Vector3(27, 30, 0)
        };

        private void Start()
        {
            Random random = new Random();
            foreach (Energizer energizer in Energizers)
            {
                int positionIndex = random.Next(0, _possiblePositions.Count);
                energizer.transform.position = _possiblePositions[positionIndex];
                _possiblePositions.RemoveAt(positionIndex);
            }
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}