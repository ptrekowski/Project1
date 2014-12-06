using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Drawing.Drawing2D;
using WindowsInput;
using Interceptor;


namespace Penguin2
{
    public partial class Form1 : Form
    {
        public static float intToDegrees = 11.3778f;

        //private bool stopPathingLoop = false;
        private bool movingForward = false;
        private bool keepGoing = true;

        PlayerActions playerActions = new PlayerActions();
        WinHandleMethods windowHandle;
        Player firstPlayer;
        Mob firstMob;

        // Game Specific Information
        static string gameProcessName = "game.dll"; // upgrade to process picker

        // Loop Variables //////////
        // /////////////////////////
        int accuracyEpsilon = 50; // Set this for deviation from actual target

        public struct MemoryAddresses
        {
            public static long absoluteXAddress = 0x22d18dc + baseGameAddress;
            public static long absoluteYAddress = 0x22d18e0 + baseGameAddress;
            public static long absoluteZAddress = 0x22d18e4 + baseGameAddress;
            public static long mobFacingAddress = 0x0AAa0a0 + baseGameAddress;
            public static long baseGameAddress = 0x0400000;
        }

        public Form1()
        {
            InitializeComponent();
            windowHandle = new WinHandleMethods(gameProcessName, 0);
            firstPlayer = new Player(gameProcessName, 0);
            firstMob = new Mob(gameProcessName, 0);
        }

        // Tester
        private void btnStart_Click(object sender, EventArgs e)
        {
            firstPlayer.updatePosition();

            // prime the timeLine
            DateTime now = DateTime.Now;
            DateTime next = now.AddMilliseconds(3000);

            listBoxWaypoints.Items.Add("Starting character at " + now.ToString() + ".");
            listBoxWaypoints.Items.Add("Projected stop at " + next.ToString() + ".");
            movingForward = true;

            while (movingForward)
            {
                windowHandle.setGameToFocusWindow();
                System.Threading.Thread.Sleep(50);
                playerActions.startMoveForward();
                firstPlayer.updatePosition();

                movingForward = true;
                now = DateTime.Now;

                // If destination time > start time
                if (now >= next)
                {
                    playerActions.stopMoveForward();
                    listBoxWaypoints.Items.Add("Stopping character at" + now.ToString() + ".");
                    System.Threading.Thread.Sleep(50);
                    firstPlayer.updatePosition();
                    movingForward = false;
                }

                // give process time to other events
                Application.DoEvents();
            }


        }

        private void btnAddWaypoint_Click(object sender, EventArgs e)
        {
            firstPlayer.updatePosition();
            firstPlayer.addToQueue(firstPlayer.AbsoluteX, firstPlayer.AbsoluteY, firstPlayer.AbsoluteZ, firstPlayer.AbsoluteFacing);
            listBoxWaypoints.Items.Add("Player X: " + firstPlayer.AbsoluteX + " Player Y: " + firstPlayer.AbsoluteY + " Player Z " + firstPlayer.AbsoluteZ);
            listBoxWaypoints.Items.Add(firstPlayer.calcDistToPoint());
            //listBoxWaypoints.Items.Add(firstPlayer.calculateDestinationDirection());
            listBoxWaypoints.Items.Add(firstPlayer.AbsoluteFacing / intToDegrees + " degrees");
        }

        private void update() {
            firstPlayer.updatePosition();
            lblX.Text = firstPlayer.AbsoluteX.ToString();
            lblY.Text = firstPlayer.AbsoluteY.ToString();
            lblZ.Text = firstPlayer.AbsoluteZ.ToString();
            lblFacing.Text = (firstPlayer.AbsoluteFacing).ToString();
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            firstPlayer.updatePosition();
            firstPlayer.updateNextWaypoint();

            listBoxWaypoints.Items.Add("Starting character at " + firstPlayer.CurrWaypoint.ToString() + ".");
            listBoxWaypoints.Items.Add("Projected stop at " + firstPlayer.NextWaypoint.ToString() + ".");
            movingForward = true;

            // Safety for testing purposes
            DateTime stopTime = DateTime.Now.AddMilliseconds(30000);
            //playerActions.mouseRightDown();

            while (movingForward && stopTime >= DateTime.Now)
            {
                // load current
                // load next
                firstPlayer.updatePosition();
                

                windowHandle.setGameToFocusWindow();
                System.Threading.Thread.Sleep(50);
                playerActions.startMoveForward();
                firstPlayer.updatePosition();
                float wpDir = firstPlayer.calcNextWpDir();
                float currDir = firstPlayer.CurrWaypoint.Facing;
                float facingDelta = Math.Abs(wpDir - currDir);
                if ((wpDir < currDir) && facingDelta > 10)
                {
                    windowHandle.setGameToFocusWindow();
                    //playerActions.tapMouseLeft();
                    playerActions.turnLeft();
                    firstPlayer.updatePosition();
                }
                else if (wpDir > currDir && facingDelta > 10)
                {
                    windowHandle.setGameToFocusWindow();
                    //playerActions.tapMouseRight();
                    playerActions.turnRight();
                    firstPlayer.updatePosition();
                }

                lblDestDelta.Text = firstPlayer.calcDistToPoint().ToString();

                // If destination time > start time
                if (firstPlayer.calcDistToPoint() < accuracyEpsilon)
                {
                    playerActions.stopMoveForward();
                    listBoxWaypoints.Items.Add("Stopping character at" + firstPlayer.NextWaypoint.ToString() + ".");
                    System.Threading.Thread.Sleep(50);
                    firstPlayer.updatePosition();
                    movingForward = false;
                    //playerActions.mouseRightUp();
                }

                // give process time to other events
                Application.DoEvents();
            }
            if (movingForward)
            {
                playerActions.stopMoveForward();
                //playerActions.mouseRightUp();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            keepGoing = false;
            foreach (Waypoint w in firstPlayer.playerOneQueue)
            {
                listBoxWaypoints.Items.Add(w.ToString());
            }
        }

        private void btnWpShow_Click(object sender, EventArgs e)
        {
            foreach (Waypoint wp in firstPlayer.playerOneQueue)
            {
                listBoxWaypoints.Items.Add(wp.ToString());
                
            }

            firstPlayer.updateNextWaypoint();
            listBoxWaypoints.Items.Add("next: " + firstPlayer.NextWaypoint.ToString());
        }

        private void btnFaceTar_Click(object sender, EventArgs e)
        {
            
            listBoxWaypoints.Items.Add( firstPlayer.calcNextWpDir());
            lblFaceDir.Text = firstPlayer.calcNextWpDir().ToString();
            
        }

        private void btnUpdateWP_Click(object sender, EventArgs e)
        {
            firstPlayer.updateNextWaypoint();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // face dir that is in textbox
            int data = Int32.Parse(txtboxData.Text);
            //listBoxWaypoints.Items.Add(data);

            // convert to a byte array
            
            listBoxWaypoints.Items.Add(data.ToString("X"));
        }

    }
}
                                                                                                