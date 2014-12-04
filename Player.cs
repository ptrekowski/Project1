using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interceptor;

namespace Penguin2
{
    class Player:Mob
    {
        private float absoluteX;
        private float absoluteY;
        private float absoluteZ;
        private float playerFacing;
        private float distanceToWaypoint;

        public struct Waypoint
        {
            public static float nextX = 765525.0625f;
            public static float nextY = 675271.6875f;
        }

        public Player(String gameProcessName, int index) :base(gameProcessName, index)
        {
            absoluteX = 0.0f;
            absoluteY = 0.0f;
            absoluteZ = 0.0f;
            playerFacing = 0;

            ReadMemory.OpenProcess(gameProcessName, index);
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
    }
}
