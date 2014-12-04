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
        public int calculateDestinationDirection()
        {
            double degrees;
            double deltaX;
            double deltaY;
            //double deltaZ;
            double radians;

            deltaX = absoluteX - NextWaypoint.X;
            deltaY = absoluteY - NextWaypoint.X;

            radians = Math.Atan2(deltaY, deltaX);
            System.Console.WriteLine("radians: " + radians.ToString());

            //radians = Math.PI / 2 - radians;
            System.Console.WriteLine("radians2: " + radians.ToString());
            degrees = radians * (180 / Math.PI);

            System.Console.WriteLine("delta X: " + deltaX.ToString());
            System.Console.WriteLine("delta y: " + deltaY.ToString());

            degrees += 270.0f;
            if (degrees >= 360)
            {
                degrees -= 360.0f;
            }


            return (int)Math.Ceiling(degrees);
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
