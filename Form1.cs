﻿using System;
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
            firstPlayer.updateMob();
            listBoxWaypoints.Items.Add(firstPlayer.AbsoluteX + ", " + firstPlayer.AbsoluteY + ", " + firstPlayer.AbsoluteZ);
            listBoxWaypoints.Items.Add(firstPlayer.calculateDistanceToNextPoint());
            listBoxWaypoints.Items.Add(firstPlayer.calculateDestinationDirection());
            listBoxWaypoints.Items.Add(firstPlayer.MobFacing);

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
            firstPlayer.updateMob();
            lblX.Text = firstPlayer.AbsoluteX.ToString();
            lblY.Text = firstPlayer.AbsoluteY.ToString();
            lblZ.Text = firstPlayer.AbsoluteZ.ToString();
            lblFacing.Text = firstPlayer.MobFacing.ToString();
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