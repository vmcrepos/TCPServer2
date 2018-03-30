namespace TCPServer2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.rtbIncoming = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnStart = new System.Windows.Forms.Button();
            this.rtbOutgoing = new System.Windows.Forms.RichTextBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnTest = new System.Windows.Forms.Button();
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.timer3 = new System.Windows.Forms.Timer(this.components);
            this.lstUnits2 = new System.Windows.Forms.CheckedListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.lblUpdateFW = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.SerialNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSendTime = new System.Windows.Forms.Button();
            this.btnReqVer = new System.Windows.Forms.Button();
            this.timer4 = new System.Windows.Forms.Timer(this.components);
            this.btnChgPort = new System.Windows.Forms.Button();
            this.btnReboot = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnReqModVer = new System.Windows.Forms.Button();
            this.btnFWUpdate = new System.Windows.Forms.Button();
            this.btnClrIncoming = new System.Windows.Forms.Button();
            this.btnClrOutgoing = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // rtbIncoming
            // 
            this.rtbIncoming.Location = new System.Drawing.Point(58, 301);
            this.rtbIncoming.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbIncoming.Name = "rtbIncoming";
            this.rtbIncoming.Size = new System.Drawing.Size(697, 225);
            this.rtbIncoming.TabIndex = 0;
            this.rtbIncoming.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(58, 271);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Incoming";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(55, 565);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Outgoing";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(61, 11);
            this.btnStart.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 25);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // rtbOutgoing
            // 
            this.rtbOutgoing.Location = new System.Drawing.Point(58, 609);
            this.rtbOutgoing.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.rtbOutgoing.Name = "rtbOutgoing";
            this.rtbOutgoing.Size = new System.Drawing.Size(697, 225);
            this.rtbOutgoing.TabIndex = 5;
            this.rtbOutgoing.Text = "";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(257, 38);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 17);
            this.lblStatus.TabIndex = 6;
            this.lblStatus.Visible = false;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(143, 11);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 25);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnSend
            // 
            this.btnSend.Enabled = false;
            this.btnSend.Location = new System.Drawing.Point(788, 183);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(175, 27);
            this.btnSend.TabIndex = 8;
            this.btnSend.Text = "Enter FW Update Mode";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // timer1
            // 
            this.timer1.Interval = 10000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(815, 371);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 27);
            this.btnTest.TabIndex = 9;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // timer2
            // 
            this.timer2.Enabled = true;
            this.timer2.Interval = 60000;
            this.timer2.Tick += new System.EventHandler(this.timer2_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(58, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(142, 17);
            this.label3.TabIndex = 11;
            this.label3.Text = "Serial Number List";
            // 
            // timer3
            // 
            this.timer3.Interval = 60000;
            this.timer3.Tick += new System.EventHandler(this.timer3_Tick);
            // 
            // lstUnits2
            // 
            this.lstUnits2.FormattingEnabled = true;
            this.lstUnits2.Location = new System.Drawing.Point(92, 96);
            this.lstUnits2.Name = "lstUnits2";
            this.lstUnits2.Size = new System.Drawing.Size(126, 157);
            this.lstUnits2.TabIndex = 12;
            this.lstUnits2.Visible = false;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(788, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Test List";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblUpdateFW
            // 
            this.lblUpdateFW.AutoSize = true;
            this.lblUpdateFW.BackColor = System.Drawing.SystemColors.Control;
            this.lblUpdateFW.Enabled = false;
            this.lblUpdateFW.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUpdateFW.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblUpdateFW.Location = new System.Drawing.Point(788, 240);
            this.lblUpdateFW.Name = "lblUpdateFW";
            this.lblUpdateFW.Size = new System.Drawing.Size(0, 20);
            this.lblUpdateFW.TabIndex = 14;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SerialNumber});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.Yellow;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridView1.Location = new System.Drawing.Point(58, 96);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dataGridView1.Size = new System.Drawing.Size(210, 157);
            this.dataGridView1.TabIndex = 15;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.Scroll += new System.Windows.Forms.ScrollEventHandler(this.dataGridView1_Scroll);
            this.dataGridView1.MouseLeave += new System.EventHandler(this.dataGridView1_MouseLeave);
            // 
            // SerialNumber
            // 
            this.SerialNumber.DataPropertyName = "SerialNumber";
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SerialNumber.DefaultCellStyle = dataGridViewCellStyle5;
            this.SerialNumber.HeaderText = "Serial Number";
            this.SerialNumber.Name = "SerialNumber";
            this.SerialNumber.ReadOnly = true;
            this.SerialNumber.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.SerialNumber.Width = 140;
            // 
            // btnSendTime
            // 
            this.btnSendTime.Location = new System.Drawing.Point(26, 58);
            this.btnSendTime.Name = "btnSendTime";
            this.btnSendTime.Size = new System.Drawing.Size(212, 29);
            this.btnSendTime.TabIndex = 16;
            this.btnSendTime.Text = "Send \"Set Time\" Message";
            this.btnSendTime.UseVisualStyleBackColor = true;
            this.btnSendTime.Click += new System.EventHandler(this.btnSendTime_Click);
            // 
            // btnReqVer
            // 
            this.btnReqVer.Location = new System.Drawing.Point(26, 98);
            this.btnReqVer.Name = "btnReqVer";
            this.btnReqVer.Size = new System.Drawing.Size(212, 29);
            this.btnReqVer.TabIndex = 17;
            this.btnReqVer.Text = "Send \"Version Request\"";
            this.btnReqVer.UseVisualStyleBackColor = true;
            this.btnReqVer.Click += new System.EventHandler(this.btnReqVer_Click);
            // 
            // timer4
            // 
            this.timer4.Interval = 5000;
            this.timer4.Tick += new System.EventHandler(this.timer4_Tick);
            // 
            // btnChgPort
            // 
            this.btnChgPort.Location = new System.Drawing.Point(26, 138);
            this.btnChgPort.Name = "btnChgPort";
            this.btnChgPort.Size = new System.Drawing.Size(212, 29);
            this.btnChgPort.TabIndex = 18;
            this.btnChgPort.Text = "Change Port/IP Address";
            this.btnChgPort.UseVisualStyleBackColor = true;
            this.btnChgPort.Click += new System.EventHandler(this.btnChgPort_Click);
            // 
            // btnReboot
            // 
            this.btnReboot.Location = new System.Drawing.Point(26, 178);
            this.btnReboot.Name = "btnReboot";
            this.btnReboot.Size = new System.Drawing.Size(212, 29);
            this.btnReboot.TabIndex = 19;
            this.btnReboot.Text = "Reboot";
            this.btnReboot.UseVisualStyleBackColor = true;
            this.btnReboot.Click += new System.EventHandler(this.btnReboot_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Yellow;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(23, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(211, 18);
            this.label4.TabIndex = 20;
            this.label4.Text = "<== Select a Unit from List";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnCreate);
            this.groupBox1.Controls.Add(this.btnReqModVer);
            this.groupBox1.Controls.Add(this.btnFWUpdate);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnReboot);
            this.groupBox1.Controls.Add(this.btnSendTime);
            this.groupBox1.Controls.Add(this.btnChgPort);
            this.groupBox1.Controls.Add(this.btnReqVer);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(284, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 224);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Actions";
            // 
            // btnReqModVer
            // 
            this.btnReqModVer.Location = new System.Drawing.Point(253, 98);
            this.btnReqModVer.Name = "btnReqModVer";
            this.btnReqModVer.Size = new System.Drawing.Size(212, 67);
            this.btnReqModVer.TabIndex = 22;
            this.btnReqModVer.Text = "Send \"Modem Version Request\"";
            this.btnReqModVer.UseVisualStyleBackColor = true;
            this.btnReqModVer.Click += new System.EventHandler(this.btnReqModVer_Click);
            // 
            // btnFWUpdate
            // 
            this.btnFWUpdate.Location = new System.Drawing.Point(253, 58);
            this.btnFWUpdate.Name = "btnFWUpdate";
            this.btnFWUpdate.Size = new System.Drawing.Size(212, 29);
            this.btnFWUpdate.TabIndex = 21;
            this.btnFWUpdate.Text = "Update Firmware";
            this.btnFWUpdate.UseVisualStyleBackColor = true;
            this.btnFWUpdate.Click += new System.EventHandler(this.btnFWUpdate_Click);
            // 
            // btnClrIncoming
            // 
            this.btnClrIncoming.Location = new System.Drawing.Point(815, 404);
            this.btnClrIncoming.Name = "btnClrIncoming";
            this.btnClrIncoming.Size = new System.Drawing.Size(125, 29);
            this.btnClrIncoming.TabIndex = 22;
            this.btnClrIncoming.Text = "Clear Incoming";
            this.btnClrIncoming.UseVisualStyleBackColor = true;
            this.btnClrIncoming.Click += new System.EventHandler(this.btnClrIncoming_Click);
            // 
            // btnClrOutgoing
            // 
            this.btnClrOutgoing.Location = new System.Drawing.Point(815, 703);
            this.btnClrOutgoing.Name = "btnClrOutgoing";
            this.btnClrOutgoing.Size = new System.Drawing.Size(125, 29);
            this.btnClrOutgoing.TabIndex = 23;
            this.btnClrOutgoing.Text = "Clear Outgoing";
            this.btnClrOutgoing.UseVisualStyleBackColor = true;
            this.btnClrOutgoing.Click += new System.EventHandler(this.btnClrOutgoing_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(898, 137);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(156, 23);
            this.button2.TabIndex = 24;
            this.button2.Text = "Display Socket List";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(792, 9);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(262, 112);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = "";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(253, 178);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(212, 29);
            this.btnCreate.TabIndex = 23;
            this.btnCreate.Text = "Create && Send Message";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1139, 905);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.btnClrOutgoing);
            this.Controls.Add(this.btnClrIncoming);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.lblUpdateFW);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.lstUnits2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnTest);
            this.Controls.Add(this.btnSend);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.rtbOutgoing);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rtbIncoming);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "TCP Server 2";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbIncoming;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.RichTextBox rtbOutgoing;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnSend;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Timer timer3;
        private System.Windows.Forms.CheckedListBox lstUnits2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblUpdateFW;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn SerialNumber;
        private System.Windows.Forms.Button btnSendTime;
        private System.Windows.Forms.Button btnReqVer;
        private System.Windows.Forms.Timer timer4;
        private System.Windows.Forms.Button btnChgPort;
        private System.Windows.Forms.Button btnReboot;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFWUpdate;
        private System.Windows.Forms.Button btnClrIncoming;
        private System.Windows.Forms.Button btnClrOutgoing;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button btnReqModVer;
        private System.Windows.Forms.Button btnCreate;
    }
}

