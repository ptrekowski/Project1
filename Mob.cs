using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsInput;
using Interceptor;

namespace Penguin2
{
    public class Mob
    {
        private float absoluteX = 0.0f;
        private float absoluteY = 0.0f;
        private float absoluteZ = 0.0f;
        private int currentFacing;
        private int health;
        private int level;

        public struct MemoryAddresses
        {
            public static long absoluteXAddress = 0x22d18dc + baseGameAddress;
            public static long absoluteYAddress = 0x22d18e0 + baseGameAddress;
            public static long absoluteZAddress = 0x22d18e4 + baseGameAddress;
            public static long mobFacingAddress = 0x0AAa0a0 + baseGameAddress;
            public static long baseGameAddress = 0x0400000;
        }

        public Mob(string gameProcessName, int index)
        {
            absoluteX = 0.0f;
            absoluteY = 0.0f;
            absoluteZ = 0.0f;
            currentFacing = 0;

            ReadMemory.OpenProcess(gameProcessName, index);
        }

        public void updatePosition()
        {
            absoluteX = ReadMemory.readFloat(MemoryAddresses.absoluteXAddress);
            absoluteY = ReadMemory.readFloat(MemoryAddresses.absoluteYAddress);
            absoluteZ = ReadMemory.readFloat(MemoryAddresses.absoluteZAddress);
            currentFacing = ReadMemory.readInt(MemoryAddresses.mobFacingAddress);
        }

        public float AbsoluteX
        {
            get { return this.absoluteX; }
        }

        public float AbsoluteY
        {
            get { return this.absoluteY; }
        }

        public float AbsoluteZ
        {
            get { return this.absoluteZ; }
        }

        public int AbsoluteFacing
        {
            get { return this.currentFacing; }
        }

    }
}
