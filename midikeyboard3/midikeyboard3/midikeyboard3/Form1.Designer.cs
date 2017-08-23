namespace midikeyboard3
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudOctaveID = new System.Windows.Forms.NumericUpDown();
            this.cbConnect = new System.Windows.Forms.CheckBox();
            this.nudPort = new System.Windows.Forms.NumericUpDown();
            this.tbServerIp = new System.Windows.Forms.TextBox();
            this.cbEnableMonitor = new System.Windows.Forms.CheckBox();
            this.cbxMonitor = new System.Windows.Forms.ComboBox();
            this.cbEnableServer = new System.Windows.Forms.CheckBox();
            this.reconnectTimer = new System.Windows.Forms.Timer(this.components);
            this.cmbMidiChannel = new System.Windows.Forms.ComboBox();
            this.cbDedicatedOctaveMode = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cbxSerialPort = new System.Windows.Forms.ComboBox();
            this.cbHardwareKeyboard = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOctaveID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudOctaveID);
            this.groupBox1.Controls.Add(this.cbConnect);
            this.groupBox1.Controls.Add(this.nudPort);
            this.groupBox1.Controls.Add(this.tbServerIp);
            this.groupBox1.Location = new System.Drawing.Point(13, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 89);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Remote Info";
            // 
            // nudOctaveID
            // 
            this.nudOctaveID.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::midikeyboard3.Properties.Settings.Default, "octaveID", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudOctaveID.Location = new System.Drawing.Point(131, 43);
            this.nudOctaveID.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudOctaveID.Name = "nudOctaveID";
            this.nudOctaveID.Size = new System.Drawing.Size(79, 20);
            this.nudOctaveID.TabIndex = 3;
            this.nudOctaveID.Value = global::midikeyboard3.Properties.Settings.Default.octaveID;
            this.nudOctaveID.ValueChanged += new System.EventHandler(this.nudOctaveID_ValueChanged);
            // 
            // cbConnect
            // 
            this.cbConnect.AutoSize = true;
            this.cbConnect.Location = new System.Drawing.Point(24, 46);
            this.cbConnect.Name = "cbConnect";
            this.cbConnect.Size = new System.Drawing.Size(66, 17);
            this.cbConnect.TabIndex = 2;
            this.cbConnect.Text = "Connect";
            this.cbConnect.UseVisualStyleBackColor = true;
            this.cbConnect.CheckedChanged += new System.EventHandler(this.cbConnect_CheckedChanged);
            // 
            // nudPort
            // 
            this.nudPort.DataBindings.Add(new System.Windows.Forms.Binding("Value", global::midikeyboard3.Properties.Settings.Default, "Port", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.nudPort.Location = new System.Drawing.Point(131, 20);
            this.nudPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudPort.Name = "nudPort";
            this.nudPort.Size = new System.Drawing.Size(79, 20);
            this.nudPort.TabIndex = 1;
            this.nudPort.Value = global::midikeyboard3.Properties.Settings.Default.Port;
            this.nudPort.ValueChanged += new System.EventHandler(this.nudPort_ValueChanged);
            // 
            // tbServerIp
            // 
            this.tbServerIp.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::midikeyboard3.Properties.Settings.Default, "remoteHost", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbServerIp.Location = new System.Drawing.Point(24, 19);
            this.tbServerIp.Name = "tbServerIp";
            this.tbServerIp.Size = new System.Drawing.Size(100, 20);
            this.tbServerIp.TabIndex = 0;
            this.tbServerIp.Text = global::midikeyboard3.Properties.Settings.Default.remoteHost;
            this.tbServerIp.TextChanged += new System.EventHandler(this.tbServerIp_TextChanged);
            // 
            // cbEnableMonitor
            // 
            this.cbEnableMonitor.AutoSize = true;
            this.cbEnableMonitor.Location = new System.Drawing.Point(37, 152);
            this.cbEnableMonitor.Name = "cbEnableMonitor";
            this.cbEnableMonitor.Size = new System.Drawing.Size(97, 17);
            this.cbEnableMonitor.TabIndex = 3;
            this.cbEnableMonitor.Text = "Enable Monitor";
            this.cbEnableMonitor.UseVisualStyleBackColor = true;
            this.cbEnableMonitor.CheckedChanged += new System.EventHandler(this.cbEnableMonitor_CheckedChanged);
            // 
            // cbxMonitor
            // 
            this.cbxMonitor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMonitor.FormattingEnabled = true;
            this.cbxMonitor.Location = new System.Drawing.Point(188, 150);
            this.cbxMonitor.Name = "cbxMonitor";
            this.cbxMonitor.Size = new System.Drawing.Size(121, 21);
            this.cbxMonitor.TabIndex = 4;
            this.cbxMonitor.SelectedIndexChanged += new System.EventHandler(this.cbxMonitor_SelectedIndexChanged);
            // 
            // cbEnableServer
            // 
            this.cbEnableServer.AutoSize = true;
            this.cbEnableServer.Location = new System.Drawing.Point(37, 197);
            this.cbEnableServer.Name = "cbEnableServer";
            this.cbEnableServer.Size = new System.Drawing.Size(93, 17);
            this.cbEnableServer.TabIndex = 5;
            this.cbEnableServer.Text = "Enable Server";
            this.cbEnableServer.UseVisualStyleBackColor = true;
            this.cbEnableServer.CheckedChanged += new System.EventHandler(this.cbEnableServer_CheckedChanged);
            // 
            // reconnectTimer
            // 
            this.reconnectTimer.Interval = 20000;
            this.reconnectTimer.Tick += new System.EventHandler(this.reconnectTimer_Tick);
            // 
            // cmbMidiChannel
            // 
            this.cmbMidiChannel.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::midikeyboard3.Properties.Settings.Default, "midiChannel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cmbMidiChannel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMidiChannel.FormattingEnabled = true;
            this.cmbMidiChannel.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.cmbMidiChannel.Location = new System.Drawing.Point(188, 119);
            this.cmbMidiChannel.Margin = new System.Windows.Forms.Padding(2);
            this.cmbMidiChannel.Name = "cmbMidiChannel";
            this.cmbMidiChannel.Size = new System.Drawing.Size(92, 21);
            this.cmbMidiChannel.TabIndex = 7;
            this.cmbMidiChannel.Text = global::midikeyboard3.Properties.Settings.Default.midiChannel;
            this.cmbMidiChannel.SelectedIndexChanged += new System.EventHandler(this.cmbMidiChannel_SelectedIndexChanged);
            // 
            // cbDedicatedOctaveMode
            // 
            this.cbDedicatedOctaveMode.AutoSize = true;
            this.cbDedicatedOctaveMode.Checked = global::midikeyboard3.Properties.Settings.Default.DedicatedOctaveMode;
            this.cbDedicatedOctaveMode.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::midikeyboard3.Properties.Settings.Default, "DedicatedOctaveMode", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbDedicatedOctaveMode.Location = new System.Drawing.Point(37, 174);
            this.cbDedicatedOctaveMode.Name = "cbDedicatedOctaveMode";
            this.cbDedicatedOctaveMode.Size = new System.Drawing.Size(141, 17);
            this.cbDedicatedOctaveMode.TabIndex = 6;
            this.cbDedicatedOctaveMode.Text = "dedicated Octave Mode";
            this.cbDedicatedOctaveMode.UseVisualStyleBackColor = true;
            this.cbDedicatedOctaveMode.CheckedChanged += new System.EventHandler(this.cbDedicatedOctaveMode_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = global::midikeyboard3.Properties.Settings.Default.EnableMidi;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::midikeyboard3.Properties.Settings.Default, "EnableMidi", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Location = new System.Drawing.Point(37, 120);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(81, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Enable Midi";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // cbxSerialPort
            // 
            this.cbxSerialPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSerialPort.FormattingEnabled = true;
            this.cbxSerialPort.Location = new System.Drawing.Point(188, 193);
            this.cbxSerialPort.Name = "cbxSerialPort";
            this.cbxSerialPort.Size = new System.Drawing.Size(121, 21);
            this.cbxSerialPort.TabIndex = 8;
            // 
            // cbHardwareKeyboard
            // 
            this.cbHardwareKeyboard.AutoSize = true;
            this.cbHardwareKeyboard.Checked = global::midikeyboard3.Properties.Settings.Default.EnableMidi;
            this.cbHardwareKeyboard.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::midikeyboard3.Properties.Settings.Default, "EnableMidi", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.cbHardwareKeyboard.Location = new System.Drawing.Point(189, 174);
            this.cbHardwareKeyboard.Name = "cbHardwareKeyboard";
            this.cbHardwareKeyboard.Size = new System.Drawing.Size(120, 17);
            this.cbHardwareKeyboard.TabIndex = 9;
            this.cbHardwareKeyboard.Text = "Hardware Keyboard";
            this.cbHardwareKeyboard.UseVisualStyleBackColor = true;
            this.cbHardwareKeyboard.CheckedChanged += new System.EventHandler(this.cbHardwareKeyboard_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(347, 298);
            this.Controls.Add(this.cbHardwareKeyboard);
            this.Controls.Add(this.cbxSerialPort);
            this.Controls.Add(this.cmbMidiChannel);
            this.Controls.Add(this.cbDedicatedOctaveMode);
            this.Controls.Add(this.cbEnableServer);
            this.Controls.Add(this.cbxMonitor);
            this.Controls.Add(this.cbEnableMonitor);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "Midi Keyboarder Live";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudOctaveID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbServerIp;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudOctaveID;
        private System.Windows.Forms.CheckBox cbConnect;
        private System.Windows.Forms.NumericUpDown nudPort;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox cbEnableMonitor;
        private System.Windows.Forms.ComboBox cbxMonitor;
        private System.Windows.Forms.CheckBox cbEnableServer;
        private System.Windows.Forms.CheckBox cbDedicatedOctaveMode;
        private System.Windows.Forms.Timer reconnectTimer;
        private System.Windows.Forms.ComboBox cmbMidiChannel;
        private System.Windows.Forms.ComboBox cbxSerialPort;
        private System.Windows.Forms.CheckBox cbHardwareKeyboard;
    }
}

