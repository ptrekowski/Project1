using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Interceptor;
using System.IO;

namespace Penguin2
{

    // This class and mob should inherit most methods and properties from 
    // a base class to lessen memory overhead
    class Player
    {
        private float absoluteX;
        private float absoluteY;
        private float absoluteZ;
        private int playerFacing;
        private float distanceToWaypoint;

        public string targetName;
        public int targetHealth;
        public int targetType;

        public int playerHealth;
        //public int playerName;
        public int playerEndurance;
        public int playerPower;

        public Queue<Waypoint> playerOneQueue = new Queue<Waypoint>();
        public LinkedList<Waypoint> playerOneLList = new LinkedList<Waypoint>();

        private Waypoint prevWaypoint = new Waypoint();
        private Waypoint currWaypoint = new Waypoint();
        private Waypoint nextWaypoint = new Waypoint();

        public Queue<Waypoint> queueToLoad = new Queue<Waypoint>();

        public bool getNextWaypoint = true;

        public struct MemoryAddresses
        {
            public static long absoluteXAddress = 0x22d18dc + baseGameAddress;
            public static long absoluteYAddress = 0x22d18e0 + baseGameAddress;
            public static long absoluteZAddress = 0x22d18e4 + baseGameAddress;
            public static long mobFacingAddress = 0x0AAa0a0 + baseGameAddress;
            public static long targetName = 0x1499620 + baseGameAddress;
            public static long targetHealth = 0x14991ac + baseGameAddress;
            public static long targetType = 0x14991b0 + baseGameAddress; // 2 = friendly; 3 = nothing; 6 = mob
            public static long playerHealth = 0x1499198 + baseGameAddress;
            public static long playerEndurance = 0x14991a0 + baseGameAddress;
            public static long playerPower = 0x14991a4 + baseGameAddress;

            public static long baseGameAddress = 0x0400000;
        }

        public Player(String gameProcessName, int index)
        {
            absoluteX = 0.0f;
            absoluteY = 0.0f;
            absoluteZ = 0.0f;
            playerFacing = 0;
            currWaypoint.updateWaypoint(absoluteX, absoluteY, absoluteZ, playerFacing);
            ReadMemory.OpenProcess(gameProcessName, index);
        }

        public void updatePosition()
        {
            absoluteX = ReadMemory.readFloat(MemoryAddresses.absoluteXAddress);
            absoluteY = ReadMemory.readFloat(MemoryAddresses.absoluteYAddress);
            absoluteZ = ReadMemory.readFloat(MemoryAddresses.absoluteZAddress);
            playerFacing = ReadMemory.readInt(MemoryAddresses.mobFacingAddress);
            currWaypoint.updateWaypoint(absoluteX, absoluteY, absoluteZ, playerFacing);
        }

        public void updateTargetInfo()
        {
            targetName = ReadMemory.ReadAsciiString(MemoryAddresses.targetName, 20);
            targetHealth = ReadMemory.readInt(MemoryAddresses.targetHealth);
            targetType = ReadMemory.readInt(MemoryAddresses.targetType);
        }

        public void updatePlayerStats()
        {
            playerHealth = ReadMemory.readInt(MemoryAddresses.playerHealth);
            playerEndurance = ReadMemory.readInt(MemoryAddresses.playerEndurance);
            playerPower = ReadMemory.readInt(MemoryAddresses.playerPower);
        }

        public void updateNextWaypoint()
        {  
            // update next Waypoint only if we have already reached
            if (getNextWaypoint)
            {
                this.NextWaypoint = NextWaypoint;
                // turn off this flag to avoid losing Waypoints
                //getNextWaypoint = false;
            }
        }

        public float calcDistToPoint()
        {
            float distanceToNextPoint;
            float a = Math.Abs(CurrWaypoint.X - NextWaypoint.X);
            float b = Math.Abs(CurrWaypoint.Y - NextWaypoint.Y);
            float c = Math.Abs(CurrWaypoint.Z - NextWaypoint.Z);
            distanceToNextPoint = (float)Math.Sqrt((a*a)+(b*b)+(c*c));
            return distanceToNextPoint;
        }

        // ** UpdateNextWaypoint MUST be called before this **
        /*  Atan2 = 
         *              |
         *              |
         *    y/x       |   x/y
         *   + 1024     |   + 2048
         * -------------+-----------------
         *    x/y       |   y/x
         *   + 0        |   + 3072
         *              |
         *              |
         * 
         * Rotation goes clock-wise 
         * South is 0/4095
         * North is 2048
         * 
         * */
        public float calcNextWpDir()
        {
            // destination - current
            // (0,-1)   N
            // (-1,-1)  NW
            // (-1,0)   W
            // (-1,1)   SW
            // (0,1)    S
            // (1,1)    SE
            // (1,0)    E
            // (1,-1    )NE

            this.updatePosition();
            float dir = 0;
            float deltaX = (nextWaypoint.X - currWaypoint.X);
            float deltaY = (nextWaypoint.Y - currWaypoint.Y);
            double radians = 0;

            if (deltaX < 0)
            {
                // NW, W, SW
                if (deltaY < 0)
                {
                    // NW
                    // 2047 > dir > 1024
                    
                    radians = Math.Atan2(Math.Abs(deltaY), Math.Abs(deltaX));
                    dir = (float)(radians * 2048 / Math.PI) + 1024;
                }
                else if (deltaY > 0)
                {
                    // SW
                    // 1024 > dir >= 0  This is the edge-case, 0 represents the first degree in rotation metric
                    radians = Math.Atan2(Math.Abs(deltaX), Math.Abs(deltaY));
                    dir = (float)(radians * 2048 / Math.PI);
                }
                else
                {
                    // W
                    // dir = 1024
                    dir = 1024;
                }

            }
            else if (deltaX > 0)
            {
                //SE, E, NE
                if (deltaY < 0)
                {
                    // NE
                    // 3072 > dir > 2047
                    radians = Math.Atan2(Math.Abs(deltaX), Math.Abs(deltaY));
                    dir = (float)(radians * 2048 / Math.PI) + 2048;

                }
                else if (deltaY > 0)
                {
                    // SE
                    // 4095 >= dir > 3072 This is the opposite edge-case of SW
                    radians = Math.Atan2(Math.Abs(deltaY), Math.Abs(deltaX));
                    dir = (float)(radians * 2048 / Math.PI) + 3072;
                }
                else
                {
                    // E
                    // dir = 3072
                    dir = 3072;
                }
            }
            else
            {
                // N, S
                if (deltaY < 0)
                {
                    // N
                    dir = 2047;
                }
                else
                {
                    // S
                    dir = 4095;
                }
            }

            return dir;
        }

        public float DistanceToWaypoint
        {
            get
            {
                distanceToWaypoint = calcDistToPoint();
                return distanceToWaypoint;
            }
        }

        public void addToQueue(float x, float y, float z, int facing)
        {
            this.playerOneQueue.Enqueue(new Waypoint(x, y, z, facing));
        }

        public Waypoint removeFromQueue()
        {
            Waypoint prev = playerOneQueue.Dequeue();
            playerOneLList.AddLast(prev);
            prevWaypoint = prev;
            return prev;
        }

        public Queue<Waypoint> returnQueueForSave()
        {
            return playerOneQueue;
        }

        public void loadQueueFromSave(Queue<Waypoint> wpQueue)
        {
            this.playerOneQueue = (wpQueue);
        }

        public Waypoint CurrWaypoint
        {
            get { return this.currWaypoint; }
        }

        public Waypoint PeekNextWaypoint()
        {
            try
            {
                return playerOneQueue.Peek();
            }
            catch
            {
                return null;
            }

        }

        public Waypoint NextWaypoint
        {
            get { return nextWaypoint; }

            set { nextWaypoint.updateWaypoint(removeFromQueue()); }
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
            get { return this.playerFacing; }
        }

    }
}
