namespace Penguin2
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnAddWaypoint = new System.Windows.Forms.Button();
            this.listBoxWaypoints = new System.Windows.Forms.ListBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.lblX = new System.Windows.Forms.Label();
            this.lblY = new System.Windows.Forms.Label();
            this.lblZ = new System.Windows.Forms.Label();
            this.lblFacing = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDestDelta = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.btnWpShow = new System.Windows.Forms.Button();
            this.btnFaceTar = new System.Windows.Forms.Button();
            this.lblFaceDir = new System.Windows.Forms.Label();
            this.btnUpdateWP = new System.Windows.Forms.Button();
            this.btnFaceThisDir = new System.Windows.Forms.Button();
            this.txtboxData = new System.Windows.Forms.TextBox();
            this.btnSavePath = new System.Windows.Forms.Button();
            this.btnLoadPath = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnAddWaypoint
            // 
            this.btnAddWaypoint.Location = new System.Drawing.Point(13, 13);
            this.btnAddWaypoint.Name = "btnAddWaypoint";
            this.btnAddWaypoint.Size = new System.Drawing.Size(75, 36);
            this.btnAddWaypoint.TabIndex = 0;
            this.btnAddWaypoint.Text = "Add Waypoint";
            this.btnAddWaypoint.UseVisualStyleBackColor = true;
            this.btnAddWaypoint.Click += new System.EventHandler(this.btnAddWaypoint_Click);
            // 
            // listBoxWaypoints
            // 
            this.listBoxWaypoints.FormattingEnabled = true;
            this.listBoxWaypoints.Location = new System.Drawing.Point(291, 2);
            this.listBoxWaypoints.Name = "listBoxWaypoints";
            this.listBoxWaypoints.Size = new System.Drawing.Size(235, 381);
            this.listBoxWaypoints.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(13, 88);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // lblX
            // 
            this.lblX.AutoSize = true;
            this.lblX.Location = new System.Drawing.Point(13, 219);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(35, 13);
            this.lblX.TabIndex = 4;
            this.lblX.Text = "label1";
            // 
            // lblY
            // 
            this.lblY.AutoSize = true;
            this.lblY.Location = new System.Drawing.Point(12, 236);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(35, 13);
            this.lblY.TabIndex = 5;
            this.lblY.Text = "label2";
            // 
            // lblZ
            // 
            this.lblZ.AutoSize = true;
            this.lblZ.Location = new System.Drawing.Point(12, 253);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(35, 13);
            this.lblZ.TabIndex = 6;
            this.lblZ.Text = "label3";
            // 
            // lblFacing
            // 
            this.lblFacing.AutoSize = true;
            this.lblFacing.Location = new System.Drawing.Point(15, 270);
            this.lblFacing.Name = "lblFacing";
            this.lblFacing.Size = new System.Drawing.Size(35, 13);
            this.lblFacing.TabIndex = 7;
            this.lblFacing.Text = "label4";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 117);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 8;
            this.button1.Text = "Loop Start";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 283);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "dist to Target";
            // 
            // lblDestDelta
            // 
            this.lblDestDelta.AutoSize = true;
            this.lblDestDelta.Location = new System.Drawing.Point(18, 296);
            this.lblDestDelta.Name = "lblDestDelta";
            this.lblDestDelta.Size = new System.Drawing.Size(35, 13);
            this.lblDestDelta.TabIndex = 10;
            this.lblDestDelta.Text = "label2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 146);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Loop Stop";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnWpShow
            // 
            this.btnWpShow.Location = new System.Drawing.Point(12, 193);
            this.btnWpShow.Name = "btnWpShow";
            this.btnWpShow.Size = new System.Drawing.Size(75, 23);
            this.btnWpShow.TabIndex = 12;
            this.btnWpShow.Text = "WP show";
            this.btnWpShow.UseVisualStyleBackColor = true;
            this.btnWpShow.Click += new System.EventHandler(this.btnWpShow_Click);
            // 
            // btnFaceTar
            // 
            this.btnFaceTar.Location = new System.Drawing.Point(3, 312);
            this.btnFaceTar.Name = "btnFaceTar";
            this.btnFaceTar.Size = new System.Drawing.Size(75, 23);
            this.btnFaceTar.TabIndex = 13;
            this.btnFaceTar.Text = "Face Tar";
            this.btnFaceTar.UseVisualStyleBackColor = true;
            this.btnFaceTar.Click += new System.EventHandler(this.btnFaceTar_Click);
            // 
            // lblFaceDir
            // 
            this.lblFaceDir.AutoSize = true;
            this.lblFaceDir.Location = new System.Drawing.Point(18, 370);
            this.lblFaceDir.Name = "lblFaceDir";
            this.lblFaceDir.Size = new System.Drawing.Size(35, 13);
            this.lblFaceDir.TabIndex = 14;
            this.lblFaceDir.Text = "label2";
            // 
            // btnUpdateWP
            // 
            this.btnUpdateWP.Location = new System.Drawing.Point(3, 342);
            this.btnUpdateWP.Name = "btnUpdateWP";
            this.btnUpdateWP.Size = new System.Drawing.Size(75, 23);
            this.btnUpdateWP.TabIndex = 15;
            this.btnUpdateWP.Text = "Update WP";
            this.btnUpdateWP.UseVisualStyleBackColor = true;
            this.btnUpdateWP.Click += new System.EventHandler(this.btnUpdateWP_Click);
            // 
            // btnFaceThisDir
            // 
            this.btnFaceThisDir.Location = new System.Drawing.Point(184, 13);
            this.btnFaceThisDir.Name = "btnFaceThisDir";
            this.btnFaceThisDir.Size = new System.Drawing.Size(75, 23);
            this.btnFaceThisDir.TabIndex = 16;
            this.btnFaceThisDir.Text = "button3";
            this.btnFaceThisDir.UseVisualStyleBackColor = true;
            this.btnFaceThisDir.Click += new System.EventHandler(this.button3_Click);
            // 
            // txtboxData
            // 
            this.txtboxData.Location = new System.Drawing.Point(168, 42);
            this.txtboxData.Name = "txtboxData";
            this.txtboxData.Size = new System.Drawing.Size(100, 20);
            this.txtboxData.TabIndex = 17;
            // 
            // btnSavePath
            // 
            this.btnSavePath.Location = new System.Drawing.Point(168, 342);
            this.btnSavePath.Name = "btnSavePath";
            this.btnSavePath.Size = new System.Drawing.Size(75, 23);
            this.btnSavePath.TabIndex = 18;
            this.btnSavePath.Text = "Save Path";
            this.btnSavePath.UseVisualStyleBackColor = true;
            this.btnSavePath.Click += new System.EventHandler(this.btnSavePath_Click);
            // 
            // btnLoadPath
            // 
            this.btnLoadPath.Location = new System.Drawing.Point(168, 311);
            this.btnLoadPath.Name = "btnLoadPath";
            this.btnLoadPath.Size = new System.Drawing.Size(75, 23);
            this.btnLoadPath.TabIndex = 19;
            this.btnLoadPath.Text = "Load Path";
            this.btnLoadPath.UseVisualStyleBackColor = true;
            this.btnLoadPath.Click += new System.EventHandler(this.btnLoadPath_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(526, 389);
            this.Controls.Add(this.btnLoadPath);
            this.Controls.Add(this.btnSavePath);
            this.Controls.Add(this.txtboxData);
            this.Controls.Add(this.btnFaceThisDir);
            this.Controls.Add(this.btnUpdateWP);
            this.Controls.Add(this.lblFaceDir);
            this.Controls.Add(this.btnFaceTar);
            this.Controls.Add(this.btnWpShow);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.lblDestDelta);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lblFacing);
            this.Controls.Add(this.lblZ);
            this.Controls.Add(this.lblY);
            this.Controls.Add(this.lblX);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.listBoxWaypoints);
            this.Controls.Add(this.btnAddWaypoint);
            this.Name = "Form1";
            this.Text = "Waypoints";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddWaypoint;
        private System.Windows.Forms.ListBox listBoxWaypoints;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label lblX;
        private System.Windows.Forms.Label lblY;
        private System.Windows.Forms.Label lblZ;
        private System.Windows.Forms.Label lblFacing;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDestDelta;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btnWpShow;
        private System.Windows.Forms.Button btnFaceTar;
        private System.Windows.Forms.Label lblFaceDir;
        private System.Windows.Forms.Button btnUpdateWP;
        private System.Windows.Forms.Button btnFaceThisDir;
        private System.Windows.Forms.TextBox txtboxData;
        private System.Windows.Forms.Button btnSavePath;
        private System.Windows.Forms.Button btnLoadPath;

    }
}

