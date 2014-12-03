using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penguin2
{
    class Player:Mob
    {
        private float absoluteX;
        private float absoluteY;
        private float absoluteZ;
        private float playerFacing;

        public Player(String gameProcessName, int index) :base(gameProcessName, index)
        {
            absoluteX = 0.0f;
            absoluteY = 0.0f;
            absoluteZ = 0.0f;
            playerFacing = 0;

            ReadMemory.OpenProcess(gameProcessName, index);
        }
    }
}
