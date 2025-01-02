namespace Template
{
    partial class FormClustering
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
            this.splitContainer_Clustering_controls = new System.Windows.Forms.SplitContainer();
            this.checkBox_Clustering_optimize_graph_area = new System.Windows.Forms.CheckBox();
            this.splitContainer_Clustering_main = new System.Windows.Forms.SplitContainer();
            this.textBox_Clustering = new System.Windows.Forms.TextBox();
            this.pictureBox_Clustering = new System.Windows.Forms.PictureBox();
            this.splitContainer_Clustering_mousehover = new System.Windows.Forms.SplitContainer();
            this.pictureBox_Clustering_mousehover = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Clustering_controls)).BeginInit();
            this.splitContainer_Clustering_controls.Panel1.SuspendLayout();
            this.splitContainer_Clustering_controls.Panel2.SuspendLayout();
            this.splitContainer_Clustering_controls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Clustering_main)).BeginInit();
            this.splitContainer_Clustering_main.Panel1.SuspendLayout();
            this.splitContainer_Clustering_main.Panel2.SuspendLayout();
            this.splitContainer_Clustering_main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Clustering)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Clustering_mousehover)).BeginInit();
            this.splitContainer_Clustering_mousehover.Panel1.SuspendLayout();
            this.splitContainer_Clustering_mousehover.Panel2.SuspendLayout();
            this.splitContainer_Clustering_mousehover.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Clustering_mousehover)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer_Clustering_controls
            // 
            this.splitContainer_Clustering_controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Clustering_controls.IsSplitterFixed = true;
            this.splitContainer_Clustering_controls.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Clustering_controls.Name = "splitContainer_Clustering_controls";
            this.splitContainer_Clustering_controls.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Clustering_controls.Panel1
            // 
            this.splitContainer_Clustering_controls.Panel1.Controls.Add(this.checkBox_Clustering_optimize_graph_area);
            // 
            // splitContainer_Clustering_controls.Panel2
            // 
            this.splitContainer_Clustering_controls.Panel2.Controls.Add(this.splitContainer_Clustering_main);
            this.splitContainer_Clustering_controls.Size = new System.Drawing.Size(1117, 603);
            this.splitContainer_Clustering_controls.SplitterDistance = 26;
            this.splitContainer_Clustering_controls.TabIndex = 0;
            // 
            // checkBox_Clustering_optimize_graph_area
            // 
            this.checkBox_Clustering_optimize_graph_area.AutoSize = true;
            this.checkBox_Clustering_optimize_graph_area.Location = new System.Drawing.Point(816, 4);
            this.checkBox_Clustering_optimize_graph_area.Name = "checkBox_Clustering_optimize_graph_area";
            this.checkBox_Clustering_optimize_graph_area.Size = new System.Drawing.Size(117, 17);
            this.checkBox_Clustering_optimize_graph_area.TabIndex = 0;
            this.checkBox_Clustering_optimize_graph_area.Text = "Optimize graph size";
            this.checkBox_Clustering_optimize_graph_area.UseVisualStyleBackColor = true;
            this.checkBox_Clustering_optimize_graph_area.CheckedChanged += new System.EventHandler(this.checkBox_Clustering_optimize_graph_area_CheckedChanged);
            // 
            // splitContainer_Clustering_main
            // 
            this.splitContainer_Clustering_main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Clustering_main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Clustering_main.Name = "splitContainer_Clustering_main";
            // 
            // splitContainer_Clustering_main.Panel1
            // 
            this.splitContainer_Clustering_main.Panel1.Controls.Add(this.splitContainer_Clustering_mousehover);
            // 
            // splitContainer_Clustering_main.Panel2
            // 
            this.splitContainer_Clustering_main.Panel2.AutoScroll = true;
            this.splitContainer_Clustering_main.Panel2.Controls.Add(this.pictureBox_Clustering);
            this.splitContainer_Clustering_main.Size = new System.Drawing.Size(1117, 573);
            this.splitContainer_Clustering_main.SplitterDistance = 372;
            this.splitContainer_Clustering_main.TabIndex = 0;
            // 
            // textBox_Clustering
            // 
            this.textBox_Clustering.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox_Clustering.Location = new System.Drawing.Point(3, 3);
            this.textBox_Clustering.Multiline = true;
            this.textBox_Clustering.Name = "textBox_Clustering";
            this.textBox_Clustering.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox_Clustering.Size = new System.Drawing.Size(366, 457);
            this.textBox_Clustering.TabIndex = 12;
            this.textBox_Clustering.WordWrap = false;
            // 
            // pictureBox_Clustering
            // 
            this.pictureBox_Clustering.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_Clustering.Name = "pictureBox_Clustering";
            this.pictureBox_Clustering.Size = new System.Drawing.Size(735, 567);
            this.pictureBox_Clustering.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox_Clustering.TabIndex = 0;
            this.pictureBox_Clustering.TabStop = false;
            this.pictureBox_Clustering.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_Clustering_MouseMove);
            // 
            // splitContainer_Clustering_mousehover
            // 
            this.splitContainer_Clustering_mousehover.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Clustering_mousehover.IsSplitterFixed = true;
            this.splitContainer_Clustering_mousehover.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Clustering_mousehover.Name = "splitContainer_Clustering_mousehover";
            this.splitContainer_Clustering_mousehover.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Clustering_mousehover.Panel1
            // 
            this.splitContainer_Clustering_mousehover.Panel1.Controls.Add(this.textBox_Clustering);
            // 
            // splitContainer_Clustering_mousehover.Panel2
            // 
            this.splitContainer_Clustering_mousehover.Panel2.Controls.Add(this.pictureBox_Clustering_mousehover);
            this.splitContainer_Clustering_mousehover.Size = new System.Drawing.Size(372, 573);
            this.splitContainer_Clustering_mousehover.SplitterDistance = 463;
            this.splitContainer_Clustering_mousehover.TabIndex = 0;
            // 
            // pictureBox_Clustering_mousehover
            // 
            this.pictureBox_Clustering_mousehover.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox_Clustering_mousehover.Location = new System.Drawing.Point(3, 3);
            this.pictureBox_Clustering_mousehover.Name = "pictureBox_Clustering_mousehover";
            this.pictureBox_Clustering_mousehover.Size = new System.Drawing.Size(366, 100);
            this.pictureBox_Clustering_mousehover.TabIndex = 0;
            this.pictureBox_Clustering_mousehover.TabStop = false;
            // 
            // FormClustering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 603);
            this.Controls.Add(this.splitContainer_Clustering_controls);
            this.Name = "FormClustering";
            this.Text = "Clustering";
            this.Resize += new System.EventHandler(this.FormClustering_Resize);
            this.splitContainer_Clustering_controls.Panel1.ResumeLayout(false);
            this.splitContainer_Clustering_controls.Panel1.PerformLayout();
            this.splitContainer_Clustering_controls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Clustering_controls)).EndInit();
            this.splitContainer_Clustering_controls.ResumeLayout(false);
            this.splitContainer_Clustering_main.Panel1.ResumeLayout(false);
            this.splitContainer_Clustering_main.Panel2.ResumeLayout(false);
            this.splitContainer_Clustering_main.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Clustering_main)).EndInit();
            this.splitContainer_Clustering_main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Clustering)).EndInit();
            this.splitContainer_Clustering_mousehover.Panel1.ResumeLayout(false);
            this.splitContainer_Clustering_mousehover.Panel1.PerformLayout();
            this.splitContainer_Clustering_mousehover.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Clustering_mousehover)).EndInit();
            this.splitContainer_Clustering_mousehover.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_Clustering_mousehover)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Clustering_controls;
        private System.Windows.Forms.SplitContainer splitContainer_Clustering_main;
        private System.Windows.Forms.TextBox textBox_Clustering;
        private System.Windows.Forms.PictureBox pictureBox_Clustering;
        private System.Windows.Forms.CheckBox checkBox_Clustering_optimize_graph_area;
        private System.Windows.Forms.SplitContainer splitContainer_Clustering_mousehover;
        private System.Windows.Forms.PictureBox pictureBox_Clustering_mousehover;
    }
}