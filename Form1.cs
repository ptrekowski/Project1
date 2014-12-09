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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Penguin2
{
    public partial class Form1 : Form
    {
        bool continueRunning = true;
        // must hardcode this with a module
        // written to address for direction (ESI)+ 0x02
        // long add = 0xa270040; 
        long add = 0xa1d0040;

        public static String fileName = "C:/PathTester.bin";

        //private bool stopPathingLoop = false;
        private bool looping = false;
        private bool movingForward = false;
        private bool playerHasValidTarget = false;

        PlayerActions playerActions = new PlayerActions();
        WinHandleMethods windowHandle;
        Player firstPlayer;
        Mob firstMob;


        // Game Specific Information
        static string gameProcessName = "game.dll"; // upgrade to process picker

      

        // Loop Variables //////////
        // /////////////////////////
        int accuracyEpsilon = 10; // Set this for deviation from actual target

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
            looping = true;

            while (looping)
            {
                windowHandle.setGameToFocusWindow();
                System.Threading.Thread.Sleep(50);
                playerActions.startMoveForward();
                firstPlayer.updatePosition();

                looping = true;
                now = DateTime.Now;

                // If destination time > start time
                if (now >= next)
                {
                    playerActions.stopMoveForward();
                    listBoxWaypoints.Items.Add("Stopping character at" + now.ToString() + ".");
                    System.Threading.Thread.Sleep(50);
                    firstPlayer.updatePosition();
                    looping = false;
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
            listBoxWaypoints.Items.Add(firstPlayer.AbsoluteFacing);
            windowHandle.setGameToFocusWindow();
        }

        private void update() {
            firstPlayer.updatePosition();
            firstPlayer.updateTargetInfo();
            firstPlayer.updatePlayerStats();
            lblX.Text = firstPlayer.AbsoluteX.ToString();
            lblY.Text = firstPlayer.AbsoluteY.ToString();
            lblZ.Text = firstPlayer.AbsoluteZ.ToString();
            lblFacing.Text = (firstPlayer.AbsoluteFacing).ToString();
            lblTarHealth.Text = firstPlayer.targetHealth.ToString();
            lblTarName.Text = firstPlayer.targetName.ToString();
            
            

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            update();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //firstPlayer.playerOneLList.First();
            windowHandle.setGameToFocusWindow();
            System.Threading.Thread.Sleep(50);
            bool face = true;
            firstPlayer.updatePosition();
            firstPlayer.updateNextWaypoint();

            listBoxWaypoints.Items.Add("Starting character at " + firstPlayer.CurrWaypoint.ToString() + ".");
            listBoxWaypoints.Items.Add("Projected stop at " + firstPlayer.NextWaypoint.ToString() + ".");
            looping = true;

            // Safety for testing purposes
            DateTime stopTime = DateTime.Now.AddMilliseconds(5000);
            

            while (looping && stopTime >= DateTime.Now)
            {
                if (!continueRunning)
                {
                    break;
                }

                bool paused = false;
                tmrTargetSearch.Enabled = true;

                firstPlayer.updatePosition();

                if(firstPlayer.targetHealth > 0 && firstPlayer.targetHealth <= 100)
                {
                    if (!continueRunning)
                    {
                        break;
                    }
                    // pause the loop
                    if (!paused)
                    {
                        paused = true;
                        playerActions.stopMoveForward();
                        movingForward = false;
                    }

                    DateTime tarStopTime = DateTime.Now.AddMinutes(1);
                    DateTime nextAttack = DateTime.Now.AddMilliseconds(1500);
                    bool mobPulled = false;
                    bool coolDownReady = true;
                    DateTime nextCoolDown = DateTime.Now;

                    // attack loop
                    while ((firstPlayer.targetHealth > 0 && DateTime.Now < tarStopTime) || (firstPlayer.playerHealth < 100 && !coolDownReady))
                    {
                        if (!continueRunning)
                        {
                            break;
                        }
                        tmrTargetSearch.Enabled = false;
                        if (DateTime.Now > nextCoolDown)
                        {
                            coolDownReady = true;
                            listBoxWaypoints.Items.Add("Cool down Ready!");
                        }

                        if (!mobPulled)
                        {
                            playerActions.faceMob();
                            System.Threading.Thread.Sleep(50);
                            //playerActions.stickTarget();
                            System.Threading.Thread.Sleep(50);
                            playerActions.pullMob();
                            coolDownReady = false;
                            nextCoolDown = DateTime.Now.AddMilliseconds(31000);
                            mobPulled = true;
                            listBoxWaypoints.Items.Add("Attacking: " + firstPlayer.targetName);
                        }
                        

                        if (DateTime.Now >= nextAttack && firstPlayer.targetHealth > 0)
                        {
                            // attack
                            if (firstPlayer.isTargetOutOfRange())
                            {
                                playerActions.pressEscape();
                                break;
                            }
                            playerActions.attack2();
                            playerActions.attack1();
                            listBoxWaypoints.Items.Add("With: Attack1");
                            nextAttack = DateTime.Now.AddMilliseconds(1500);
                        }

                        Application.DoEvents();
                    }

                    if (tmrTargetSearch.Enabled == false)
                    {
                        tmrTargetSearch.Enabled = true;
                    }

                    stopTime = DateTime.Now.AddMilliseconds(4000);
                    face = true; // trigger this to reestablish the correct heading
                    paused = false;
                }

                firstPlayer.updatePosition();
                float wpDir = firstPlayer.calcNextWpDir();
                int intWpDir = (int)Math.Floor(wpDir);
                float currDir = firstPlayer.CurrWaypoint.Facing;
                int intCurrDir = (int)Math.Ceiling(currDir);
                if (currDir != wpDir && face)
                {
                    ReadMemory.WriteInt(add, intWpDir);
                    listBoxWaypoints.Items.Clear();
                    listBoxWaypoints.Items.Add((intWpDir).ToString());
                    listBoxWaypoints.Items.Add("Writing to memory");
                    listBoxWaypoints.Items.Add("Curr: " + currDir.ToString());
                    listBoxWaypoints.Items.Add("Dest: " + wpDir.ToString());
                    face = false;
                }

                windowHandle.setGameToFocusWindow();
                //System.Threading.Thread.Sleep(100);
                if (!movingForward && !paused)
                {
                    playerActions.startMoveForward();
                }
                firstPlayer.updatePosition();


                lblDestDelta.Text = firstPlayer.calcDistToPoint().ToString();

                // If destination time > start time
                if (firstPlayer.calcDistToPoint() < accuracyEpsilon)
                {
                    playerActions.stopMoveForward();
                    listBoxWaypoints.Items.Add("Stopping character at" + firstPlayer.NextWaypoint.ToString() + ".");
                    //System.Threading.Thread.Sleep(100);
                    face = true;

                    if (firstPlayer.PeekNextWaypoint() == null)
                    {
                        looping = false;
                    }
                    else
                    {
                        firstPlayer.updateNextWaypoint();
                        listBoxWaypoints.Items.Add("Starting character at " + firstPlayer.CurrWaypoint.ToString() + ".");
                        listBoxWaypoints.Items.Add("Projected stop at " + firstPlayer.NextWaypoint.ToString() + ".");
                        stopTime = DateTime.Now.AddMilliseconds(10000);
                    }
                }

                // give process time to other events
                Application.DoEvents();
            }
            tmrTargetSearch.Enabled = false;
            if (looping)
            {
                playerActions.stopMoveForward();
            }
        }

        private void btnFaceTar_Click(object sender, EventArgs e)
        {
            
            listBoxWaypoints.Items.Add( firstPlayer.calcNextWpDir());
            lblFaceDir.Text = firstPlayer.calcNextWpDir().ToString();
            
        }

        private void btnSavePath_Click(object sender, EventArgs e)
        {
              // to serialize the saved path
            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream inOutFile = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            SaveFileDialog saveFile = new SaveFileDialog();
            //if (saveFile.ShowDialog() == DialogResult.OK)
            //{

            //}

                binFormat.Serialize(inOutFile, firstPlayer.returnQueueForSave());
            
            inOutFile.Close();
        }

        private void btnLoadPath_Click(object sender, EventArgs e)
        {
            listBoxWaypoints.Items.Add("Path Loaded! Ready.");
            BinaryFormatter binFormat = new BinaryFormatter();
            FileStream inOutFile = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            firstPlayer.loadQueueFromSave((Queue<Waypoint>)binFormat.Deserialize(inOutFile));
        }

        private void wpTimer_Tick(object sender, EventArgs e)
        {
            btnAddWaypoint_Click(this, null);
        }

        private void btnToggleMakeWP_Click(object sender, EventArgs e)
        {
            
            if (btnToggleMakeWP.Text == "WP Off")
            {
                btnToggleMakeWP.Text = "Tracking";

                wpTimer.Enabled = true;
            }
            else
            {
                btnToggleMakeWP.Text = "WP Off";
                wpTimer.Enabled = false;
            }
        }

        private void tmrTargetSearch_Tick(object sender, EventArgs e)
        {
            playerActions.findTarget();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            continueRunning = false;
            Application.Exit();
        }
    }
}
                                                                                        