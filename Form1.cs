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
        // for interrupt key
        private KeyHandler ghk;
        private bool stopPathingLoop = false;

        // Game Specific Information

        static string gameProcessName = "game.dll";
        
        static IntPtr gameWindowHandle; // specific to each individual game window

        // image Resource
        //Image img = pctCompass.Image;


        // Loop Variables //////////
        // /////////////////////////
        bool movingLoop = true;
        bool notThere = true;

        int accuracyEpsilon = 5; // Set this for accuracy to target

        Mob firstMob = new Mob(gameProcessName, 0); // First Character
        //Mob secondMob = new Mob(gameProcessName, 1); // Second Character

        WinHandleMethods winHndForFirstMob;
        //WinHandleMethods winHndForSecondMob = new WinHandleMethods(gameProcessName, 1);

        public Form1()
        {
            InitializeComponent();
            winHndForFirstMob = new WinHandleMethods(gameProcessName, 0);
            ghk = new KeyHandler(Keys.NumLock, this);
            ghk.Register();
        }

 

        private void btnFaceDestination_Click(object sender, EventArgs e)
        {
            while (movingLoop)
            {

            }
        }

        private void btnAddWaypoint_Click(object sender, EventArgs e)
        {
            firstMob.updateMob();
            listBoxWaypoints.Items.Add(firstMob.AbsoluteX + ", " + firstMob.AbsoluteY + ", " + firstMob.AbsoluteZ);
            listBoxWaypoints.Items.Add(firstMob.calculateDistanceToNextPoint());
            //listBoxWaypoints.Items.Add(firstMob.calculateDirectionToNextPoint());
            listBoxWaypoints.Items.Add(firstMob.calculateDestinationDirection());
            listBoxWaypoints.Items.Add(firstMob.MobFacing);
            //img = ImageRotator.RotateImage(img, -5.0f);
            //pctCompass.Image = img;
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
            firstMob.updateMob();
           lblX.Text = firstMob.absoluteX.ToString();
           lblY.Text = firstMob.absoluteY.ToString();
           lblZ.Text = firstMob.absoluteZ.ToString();
            lblFacing.Text = firstMob.mobFacing.ToString();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            firstMob.updateMob();
            int thisStepCount = 0;
            bool movingForward = false;
            // prime the time2
            DateTime now = DateTime.Now;
            DateTime next = now.AddMilliseconds(1000);

            while ( thisStepCount < 30)
            {
                
                

                // fire this event
                if (now.Millisecond >= next.Millisecond)
                {
                    winHndForFirstMob.setGameToFocusWindow();
                    System.Threading.Thread.Sleep(50);
                    firstMob.startMoveForward();
                    firstMob.updateMob();
                    movingForward = true;
                    now = DateTime.Now;
                    next = now.AddMilliseconds(1000);
                }
             
                
                thisStepCount++;
                Application.DoEvents();
            }
            firstMob.stopMoveForward();
            System.Threading.Thread.Sleep(50);
            firstMob.updateMob();
            movingForward = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            updateUI();
        }

    }
}
