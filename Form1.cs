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


namespace Penguin2
{
    public partial class Form1 : Form
    {
        public static float intToDegrees = 11.3778f;
        // for interrupt key
        private KeyHandler ghk;
        private bool stopPathingLoop = false;
        private bool movingForward = false;

        // Game Specific Information

        static string gameProcessName = "game.dll";
        
        static IntPtr gameWindowHandle; // specific to each individual game window



        // Loop Variables //////////
        // /////////////////////////
        bool movingLoop = true;
        bool notThere = true;

        int accuracyEpsilon = 5; // Set this for accuracy to target

        Player firstPlayer = new Player(gameProcessName, 0); // First Character
        

        WinHandleMethods winHndForFirstMob;

        public Form1()
        {
            InitializeComponent();
            winHndForFirstMob = new WinHandleMethods(gameProcessName, 0);
            ghk = new KeyHandler(Keys.NumLock, this);
            ghk.Register();
        }

 

        private void btnFaceDestination_Click(object sender, EventArgs e)
        {

        }

        private void btnAddWaypoint_Click(object sender, EventArgs e)
        {
            firstPlayer.updatePosition();
            listBoxWaypoints.Items.Add(firstPlayer.AbsoluteX + ", " + firstPlayer.AbsoluteY + ", " + firstPlayer.AbsoluteZ);
            listBoxWaypoints.Items.Add(firstPlayer.calculateDistanceToNextPoint());
            //listBoxWaypoints.Items.Add(firstPlayer.calculateDestinationDirection());
            listBoxWaypoints.Items.Add(firstPlayer.AbsoluteFacing / intToDegrees + " degrees");

        }

        private void HandleHotkey()
        {
                stopPathingLoop = true;
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == Constants.WM_HOTKEY_MSG_ID)
                HandleHotkey();
            base.WndProc(ref m);
        }
        private void updateUI () {
            firstPlayer.updatePosition();
            lblX.Text = firstPlayer.AbsoluteX.ToString();
            lblY.Text = firstPlayer.AbsoluteY.ToString();
            lblZ.Text = firstPlayer.AbsoluteZ.ToString();
            lblFacing.Text = (firstPlayer.AbsoluteFacing / intToDegrees + " degrees").ToString();

        }


        // Tester
        private void btnStart_Click(object sender, EventArgs e)
        {
            firstPlayer.updatePosition();
            int thisStepCount = 0;

            // prime the time
            DateTime now = DateTime.Now;
            DateTime next = now.AddMilliseconds(1000);

            while ( thisStepCount < 30)
            {
                // fire this event
                if (now.Millisecond >= next.Millisecond)
                {
                    winHndForFirstMob.setGameToFocusWindow();
                    System.Threading.Thread.Sleep(50);
                    firstPlayer.startMoveForward();
                    firstPlayer.updatePosition();
                    movingForward = true;
                    now = DateTime.Now;
                    next = now.AddMilliseconds(1000);
                }
             
                
                thisStepCount++;
                Application.DoEvents();
            }
            firstPlayer.stopMoveForward();
            System.Threading.Thread.Sleep(50);
            firstPlayer.updatePosition();
            movingForward = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateUI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            winHndForFirstMob.setGameToFocusWindow();
            System.Threading.Thread.Sleep(100);
            firstPlayer.attack1();
        }

    }
}
