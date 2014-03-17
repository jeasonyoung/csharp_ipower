namespace iPower.WinService.Test
{
    partial class TestForm
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
            this.panelWorkArea = new System.Windows.Forms.Panel();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.lbRunTime = new System.Windows.Forms.Label();
            this.lbTimer = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.chkOnce = new System.Windows.Forms.CheckBox();
            this.panelWorkArea.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelWorkArea
            // 
            this.panelWorkArea.Controls.Add(this.richTextBoxMessage);
            this.panelWorkArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelWorkArea.Location = new System.Drawing.Point(0, 0);
            this.panelWorkArea.Name = "panelWorkArea";
            this.panelWorkArea.Size = new System.Drawing.Size(696, 479);
            this.panelWorkArea.TabIndex = 0;
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.richTextBoxMessage.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBoxMessage.Location = new System.Drawing.Point(3, 3);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.Size = new System.Drawing.Size(690, 400);
            this.richTextBoxMessage.TabIndex = 0;
            this.richTextBoxMessage.Text = "";
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.Transparent;
            this.panelBottom.Controls.Add(this.chkOnce);
            this.panelBottom.Controls.Add(this.lbRunTime);
            this.panelBottom.Controls.Add(this.lbTimer);
            this.panelBottom.Controls.Add(this.btnStop);
            this.panelBottom.Controls.Add(this.btnPause);
            this.panelBottom.Controls.Add(this.btnStart);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 409);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(696, 70);
            this.panelBottom.TabIndex = 1;
            // 
            // lbRunTime
            // 
            this.lbRunTime.AutoSize = true;
            this.lbRunTime.BackColor = System.Drawing.Color.Tan;
            this.lbRunTime.Font = new System.Drawing.Font("微软雅黑", 10.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbRunTime.ForeColor = System.Drawing.Color.Red;
            this.lbRunTime.Location = new System.Drawing.Point(456, 46);
            this.lbRunTime.Name = "lbRunTime";
            this.lbRunTime.Size = new System.Drawing.Size(51, 19);
            this.lbRunTime.TabIndex = 4;
            this.lbRunTime.Text = "label1";
            this.lbRunTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbTimer
            // 
            this.lbTimer.AutoSize = true;
            this.lbTimer.BackColor = System.Drawing.Color.Tan;
            this.lbTimer.Font = new System.Drawing.Font("微软雅黑", 10.5F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTimer.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.lbTimer.Location = new System.Drawing.Point(456, 19);
            this.lbTimer.Name = "lbTimer";
            this.lbTimer.Size = new System.Drawing.Size(51, 19);
            this.lbTimer.TabIndex = 3;
            this.lbTimer.Text = "label1";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(238, 19);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(90, 46);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "停止服务";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(123, 19);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(90, 46);
            this.btnPause.TabIndex = 1;
            this.btnPause.Text = "暂停服务";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 19);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 46);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "启动服务";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // timer
            // 
            this.timer.Interval = 1000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // chkOnce
            // 
            this.chkOnce.AutoSize = true;
            this.chkOnce.Checked = true;
            this.chkOnce.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOnce.Location = new System.Drawing.Point(345, 21);
            this.chkOnce.Name = "chkOnce";
            this.chkOnce.Size = new System.Drawing.Size(108, 16);
            this.chkOnce.TabIndex = 5;
            this.chkOnce.Text = "仅运行一个周期";
            this.chkOnce.UseVisualStyleBackColor = true;
            this.chkOnce.CheckedChanged += new System.EventHandler(this.chkOnce_CheckedChanged);
            // 
            // TestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(696, 479);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelWorkArea);
            this.Name = "TestForm";
            this.Text = "Windows服务测试工具";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.panelWorkArea.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelWorkArea;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Label lbTimer;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.Label lbRunTime;
        private System.Windows.Forms.CheckBox chkOnce;
    }
}