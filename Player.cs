using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using Interceptor;

namespace Penguin2
{

    // This class and mob should inherit most methods and properties from a base class
    class Player
    {
        private float absoluteX;
        private float absoluteY;
        private float absoluteZ;
        private int playerFacing;
        private float distanceToWaypoint;

        private Queue<Waypoint> playerOneQueue = new Queue<Waypoint>();

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

            ReadMemory.OpenProcess(gameProcessName, index);
        }

        public void updatePosition()
        {
            absoluteX = ReadMemory.readFloat(MemoryAddresses.absoluteXAddress);
            absoluteY = ReadMemory.readFloat(MemoryAddresses.absoluteYAddress);
            absoluteZ = ReadMemory.readFloat(MemoryAddresses.absoluteZAddress);
            playerFacing = ReadMemory.readInt(MemoryAddresses.mobFacingAddress);
        }

        public float calculateDistanceToNextPoint()
        {
            float distanceToNextPoint;
            distanceToNextPoint = (float)Math.Sqrt(Math.Abs(((double)Waypoint.nextX - (double)absoluteX) + ((double)Waypoint.nextY - (double)absoluteY)));
            return distanceToNextPoint;
        }

        // ** Finish by adding z-index to calculation
        public int calculateDestinationDirection() // Point startPoint, Point endPoint 
        {
            double degrees;
            double deltaX;
            double deltaY;
            //double deltaZ;
            double radians;

            deltaX = absoluteX - Waypoint.nextX;
            deltaY = absoluteY - Waypoint.nextY;

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
            float realX = Waypoint.nextX - this.absoluteX;
            float realY = Waypoint.nextY - this.absoluteY;

            if (realX < 0)
            {
                realX = realX * -1;
            }

            if (realY < 0)
            {
                realY = realY * -1;
            }

            if ((absoluteX > Waypoint.nextX) && (absoluteY > Waypoint.nextY))
            {
                directionInDegrees = radiansToDegrees(realX, realY);
            }

            if ((absoluteX > Waypoint.nextX) && (absoluteY < Waypoint.nextY))
            {
                directionInDegrees = ((radiansToDegrees(realX, realY) * -1) + 90) + 270;
            }

            if ((absoluteX < Waypoint.nextX) && absoluteY < Waypoint.nextY)
            {
                directionInDegrees = radiansToDegrees(realX, realY) + 180;
            }

            if ((absoluteX < Waypoint.nextX) && (absoluteY > Waypoint.nextY))
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
                distanceToWaypoint = calculateDistanceToNextPoint();
                return distanceToWaypoint;
            }
        }

        public void addToQueue(float x, float y, float z, int facing)
        {
            playerOneQueue.Enqueue(new Waypoint(x, y, z, facing));
        }

        public Object removeFromQueue()
        {
            return playerOneQueue.Dequeue();
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
