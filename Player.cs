using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Interceptor;

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

        public Queue<Waypoint> playerOneQueue = new Queue<Waypoint>();
        private Waypoint currWaypoint = new Waypoint();
        private Waypoint nextWaypoint = new Waypoint();

        public bool getNextWaypoint = true;

        public struct MemoryAddresses
        {
            public static long absoluteXAddress = 0x22d18dc + baseGameAddress;
            public static long absoluteYAddress = 0x22d18e0 + baseGameAddress;
            public static long absoluteZAddress = 0x22d18e4 + baseGameAddress;
            public static long mobFacingAddress = 0x0AAa0a0 + baseGameAddress;
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

        // ** Finish by adding z-index to calculation
        public int calcNextWpDir()
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
            float dir;
            float deltaX = (nextWaypoint.X - currWaypoint.X);
            float deltaY = (nextWaypoint.Y - currWaypoint.Y);
            float deltaZ = (nextWaypoint.Z - currWaypoint.Z);

            if (deltaX < 0)
            {
                // NW, W, SW
                if (deltaY < 0)
                {
                    // NW
                    // 2047 > dir > 1024
                }
                else if (deltaY > 0)
                {
                    // SW
                    // 1024 > dir >= 0  This is the edge-case, 0 represents the first degree in rotation metric
                }
                else
                {
                    // W
                    // dir = 1024
                }

            }
            else if (deltaX > 0)
            {
                //SE, E, NE
                if (deltaY < 0)
                {
                    // NE
                    // 3072 > dir > 2047

                }
                else if (deltaY > 0)
                {
                    // SE
                    // 4095 >= dir > 3072 This is the opposite edge-case of SW

                }
                else
                {
                    // E
                    // dir = 3072
                }
            }
            else
            {
                // N, S
                if (deltaY < 0)
                {
                    // N
                }
                else
                {
                    // S
                }
            }

            //dir = (float)(Math.Sqrt(Math.Pow(deltaX, 2) + Math.Pow(deltaY, 2) + Math.Pow(deltaZ,2)));
            return (int)dir;
        }

        public float calculateDirectionToNextPoint()
        {
            float directionInDegrees = 0;
            float realX = NextWaypoint.X - this.absoluteX;
            float realY = NextWaypoint.Y - this.absoluteY;

            if (realX < 0)
            {
                realX = realX * -1;
            }

            if (realY < 0)
            {
                realY = realY * -1;
            }

            if ((absoluteX > NextWaypoint.X) && (absoluteY > NextWaypoint.Y))
            {
                directionInDegrees = radiansToDegrees(realX, realY);
            }

            if ((absoluteX > NextWaypoint.X) && (absoluteY < NextWaypoint.Y))
            {
                directionInDegrees = ((radiansToDegrees(realX, realY) * -1) + 90) + 270;
            }

            if ((absoluteX < NextWaypoint.X) && absoluteY < NextWaypoint.Y)
            {
                directionInDegrees = radiansToDegrees(realX, realY) + 180;
            }

            if ((absoluteX < NextWaypoint.X) && (absoluteY > NextWaypoint.Y))
            {
                directionInDegrees = (((radiansToDegrees(realX, realY)) * -1) + 90) + 90;
            }

            // use to convert into a radian value
            //float rotateToThisDirection = (float)((2 * Math.PI) / 360) * directionInDegrees;
            float rotateToThisDirection = directionInDegrees;

            return rotateToThisDirection;
        }

        public float radiansToDegrees(float realX, float realY)
        {
            float degrees;
            degrees = (float)(Math.Atan2(realY, realX));

            return degrees;
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
            playerOneQueue.Enqueue(new Waypoint(x, y, z, facing));
        }

        public Waypoint removeFromQueue()
        {
            return playerOneQueue.Dequeue();
        }

        public Waypoint CurrWaypoint
        {
            get { return this.currWaypoint; }
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
