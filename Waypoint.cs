using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Penguin2
{
    class Waypoint
    {
        private float x;
        private float y;
        private float z;
        private int facing;

        public Waypoint(Waypoint w)
        {
            this.x = w.x;
            this.y = w.y;
            this.z = w.z;
            this.facing = w.facing;
        }

        public Waypoint(float x, float y, float z, int facing)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.facing = facing;
        }

        public void updateWaypoint (float x, float y, float z, int facing)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.facing = facing;
        }

        public void updateWaypoint(Waypoint w)
        {
            this.x = w.x;
            this.y = w.y;
            this.z = w.z;
            this.facing = w.facing;   
        }

        public float X
        {
            get {return this.x;}
        }

        public float Y
        {
            get {return this.y;}
        }

        public float Z
        {
            get {return this.z;}
        }

        public int Facing
        {
            get { return this.facing; }
        }

        public override string ToString()
        {
            return (this.x + " " + this.y + " " + this.z + " " + this.facing);
        }
    }
}
