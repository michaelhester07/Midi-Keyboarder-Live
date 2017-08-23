using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
//the simple live only version
namespace midikeyboard3
{
   
    public partial class Form1 : Form
    {
        bool dedicatedOctave = false;
        Mutex OctaveChangeLock;
        System.IO.Ports.SerialPort hwkeyboard;



        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        void setGW2Foreground()
        {
            var Guildwars2 = System.Diagnostics.Process.GetProcessesByName("Gw2");
            if (Guildwars2.Length == 0)
                Guildwars2 = System.Diagnostics.Process.GetProcessesByName("Gw2-64");
            if (Guildwars2.Length != 0)
                SetForegroundWindow(Guildwars2[0].MainWindowHandle);
        }

       public class clientInstrument
        {
            public int octaveID; //2 = bass, 3, 4 ,5 = harp/bell
            //note that 5 is always the local output
        }
        Dictionary<int, System.Net.Sockets.TcpClient> instrumentRegistry;
        int reconnectAttempts = 5;
        IPAddress resolveIP(string[] ParsedIPAddress)
        {
            IPAddress oIP = null;
            IPHostEntry oIPHostEntry = null;
            ushort iPort;

            if (!IPAddress.TryParse(ParsedIPAddress[0], out oIP))
            {
                try
                {

                    oIPHostEntry = Dns.GetHostEntry(ParsedIPAddress[0]);

                    // System.Diagnostics.Trace.WriteLine("sw1 : " + sw1.Elapsed.TotalSeconds.ToString());
                }
                catch (Exception) { }

                if (oIPHostEntry != null)
                {
                    for (int a = 0; a < oIPHostEntry.AddressList.Length; a++)
                    {
                        if (oIPHostEntry.AddressList[a].AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            oIP = oIPHostEntry.AddressList[a];

                            break;
                        }
                    }
                }
            }
            return oIP;

        }
        System.Threading.Tasks.TaskFactory taskfactory;

        public Form1()
        {
            InitializeComponent();
            
        }
        MidiDeviceDriver midiDriver;
        System.Net.Sockets.TcpListener server;
        private void Form1_Load(object sender, EventArgs e)
        {
            MidiTransformTable.init();
            instrumentRegistry = new Dictionary<int, System.Net.Sockets.TcpClient>();
            midiDriver = new MidiDeviceDriver();
            midiDriver.onNote += MidiDriver_onNote;
            downKeys = new List<Keys>();

            dedicatedOctave = cbDedicatedOctaveMode.Checked;
            midiDriver.enableInput(checkBox1.Checked);
            midiDriver.setChannel(int.Parse(cmbMidiChannel.Text));
            var monitors = midiDriver.listMonitors();
            foreach (var monitor in monitors)
                cbxMonitor.Items.Add(monitor);
            int targetIndex = cbxMonitor.Items.IndexOf(cbxMonitor.Text);
            if (targetIndex == -1)
                targetIndex = 0;
            if(cbxMonitor.Items.Count > 0) //microsoft removed the GS wavesynth in windows 10! there is no longer a guaranteed monitor
                cbxMonitor.SelectedIndex = targetIndex;
            OctaveChangeLock = new Mutex();
            taskfactory = new TaskFactory();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();
            foreach (string p in ports)
                cbxSerialPort.Items.Add(p);

          //  System.Threading.Thread delayedNoteHandler = new Thread(delayedNoteHandlerThread);
           // delayedNoteHandler.Start();
        }
        


        
        List<Keys> downKeys;

       

        int currentOctave = 4;
       void doKeyPress(Keys k, int delay)
        {
            if(cbHardwareKeyboard.Checked && hwkeyboard != null) //arduino handles delay
            {
                switch(k)
                {

                    case Keys.D1: hwkeyboard.Write("1"); break;
                    case Keys.D2: hwkeyboard.Write("2"); break;
                    case Keys.D3: hwkeyboard.Write("3"); break;
                    case Keys.D4: hwkeyboard.Write("4"); break;
                    case Keys.D5: hwkeyboard.Write("5"); break;
                    case Keys.D6: hwkeyboard.Write("6"); break;
                    case Keys.D7: hwkeyboard.Write("7"); break;
                    case Keys.D8: hwkeyboard.Write("8"); break;
                    case Keys.D9: hwkeyboard.Write("9"); break;
                    case Keys.D0: hwkeyboard.Write("0"); break;
                       

                }
            }
            else
            {
                InputManager.Keyboard.KeyPress(k, delay);
            }
        }
        void handleNote(string note)
        {
           
            //if(this.InvokeRequired) //notes handle on the main thread
            //{
            //    MidiDeviceDriver.onNoteDelegate del = new MidiDeviceDriver.onNoteDelegate(handleNote);
            //    this.BeginInvoke(del, note);
            //    return;

            //}
            int lagBudget = (int)200;
           // setGW2Foreground();
            if (note == string.Empty && cbConnect.Checked)
            {
               

                return; //disconnected
            }
            OctaveChangeLock.WaitOne(1000);
            bool down = note[note.Length - 1] == '+';
            int octave = int.Parse("" + note[note.Length - 2]);
            if (!dedicatedOctave && octave <= 2 && nudOctaveID.Value >= 3)
                octave = 3;
            bool highC = false;
            if (octave - 1 == currentOctave && note[0] == 'C' && !dedicatedOctave)
            {
                System.Diagnostics.Trace.WriteLine("high c detected");
                highC = true;
                octave = currentOctave;
            }
            if (down)
            {
               
                if (!dedicatedOctave || nudOctaveID.Value < 2 ) //set to 6 when using bell to use upper octave
                {
                  
                    while (currentOctave < octave)
                    {
                        
                        doKeyPress(Keys.D0, 25);
                        lagBudget -= (int)dynamicDelay;
                        System.Threading.Thread.Sleep((int)100);
                        currentOctave++;
                        if (octave - 1 == currentOctave && note[0] == 'C' && !dedicatedOctave)
                        {
                            highC = true;
                            octave = currentOctave;
                        }
                    }
                    while (currentOctave > octave)
                    {
                        doKeyPress(Keys.D9, 25);
                        lagBudget -= (int)dynamicDelay;
                         System.Threading.Thread.Sleep((int)100);
                        currentOctave--;
                    }
                 
                }
                var key = MidiTransformTable.getKey(note);
                if (highC)
                    key = Keys.D8;
               
                if (!downKeys.Contains(key))
                {
                    System.Diagnostics.Trace.WriteLine("high c");
                  
                        InputManager.Keyboard.KeyDown(key);
                    downKeys.Add(key);
                }
               
            }
            else
            {
                var key = MidiTransformTable.getKey(note);
                if (highC)
                    key = Keys.D8;
                if (downKeys.Contains(key))
                {
                   
                    {
                        InputManager.Keyboard.KeyUp(key);
                    }
                    downKeys.Remove(key);
                }

            }
            OctaveChangeLock.ReleaseMutex();
        }
        public struct keyDelay
        {
            public int delay;
            public bool down;
            public Keys k;
        }
        void delayedKeyDown(object o)
        {
            keyDelay key = (keyDelay)o ;
            System.Threading.Thread.Sleep(key.delay);
            if (key.down)
                InputManager.Keyboard.KeyDown(key.k);
            else
                InputManager.Keyboard.KeyUp(key.k);

        }
        private void MidiDriver_onNote(string note)
        {
            //ASharp3+
            //note direction is length-1
            //octave is length -2
            //rest of string is the note itslef from the lookup table.

            //if octave is < 5 and in dedicated octave mode send to the instrument which registered with the octave id
            //for Bass, send to the bass instrument for 1 and 2 //maybe later these get dedicated octaves too
            bool down = note[note.Length - 1] == '+';
            int octave = int.Parse(""+note[note.Length - 2]);
            System.Diagnostics.Trace.WriteLine("msg " + note);
            if (octave == 6) octave = 5;//for the purpose of note distribution
            if (octave != nudOctaveID.Value && dedicatedOctave) //new play octave is 4 to avoid heartbeat on the main user
            {
                //todo: transmit note
                byte[] notemsg = ASCIIEncoding.ASCII.GetBytes(note + ";");
                if (octave < 2) octave = 2;
               
                if (instrumentRegistry.ContainsKey(octave))
                    instrumentRegistry[octave].Client.Send(notemsg);

            }
            else
                handleNote(note);

        }

        private void cbDedicatedOctaveMode_CheckedChanged(object sender, EventArgs e)
        {
            dedicatedOctave = cbDedicatedOctaveMode.Checked;
            Properties.Settings.Default.Save();
        }

        private void cbEnableServer_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (cbEnableServer.Checked)
                {
                    server = new System.Net.Sockets.TcpListener((int)nudPort.Value);
                    server.Start();
                    server.BeginAcceptTcpClient(serverConnectioncallback, server);
                }
                else
                {
                    if (server != null)
                    {
                        server.Stop();
                        foreach (var client in instrumentRegistry.Values)
                            if (client.Connected)
                                client.Close();
                    }
                }
            }
            catch
            {
                cbEnableServer.Checked = false;
            }
        }

        void serverConnectioncallback(IAsyncResult res)
        {
            var localserver = (System.Net.Sockets.TcpListener)res.AsyncState;
            var client = localserver.EndAcceptTcpClient(res);

            byte[] buffer = new byte[512];
            int len = client.Client.Receive(buffer);
            //registry
            //registerInstrument;<id>
            string cmd = ASCIIEncoding.ASCII.GetString(buffer,0,len); //messages short enough to not need socket receive logic
            if(cmd.Contains("registerInstrument"))
            {
                string[] registryparts = cmd.Split(';');
                int octave = int.Parse(registryparts[1]);
                if (instrumentRegistry.ContainsKey(octave))
                {
                    instrumentRegistry[octave].Close(); //close an existing connection
                }
                instrumentRegistry[octave] = client; //adds the client
                if(octave == 6)
                {
                    if (instrumentRegistry.ContainsKey(5))
                    {
                        //instrumentRegistry[5].Close(); //close an existing connection
                    }
                    instrumentRegistry[5] = client; //adds the client
                }
            }
            server.BeginAcceptTcpClient(serverConnectioncallback, server);
        }

        System.Net.Sockets.TcpClient client;
        byte[] clientRecvBuffer;
       
        void connectClient(string addr, int port)
        {
            
            client = new System.Net.Sockets.TcpClient(addr, port);
            clientRecvBuffer = new byte [512];
            currentOctave = (int)nudOctaveID.Value;
            string registryRequest = string.Format("registerInstrument;{0}", currentOctave);
            byte[] msg = ASCIIEncoding.ASCII.GetBytes(registryRequest);
            client.Client.Send(msg);
            
            client.Client.BeginReceive(clientRecvBuffer,0, 512, System.Net.Sockets.SocketFlags.None, clientRecvCallback, client);
           // System.Threading.Thread t = new System.Threading.Thread(clientRecvCallback);
           // t.Start();
        }
        
        void clientRecvCallback(IAsyncResult res)
        {
            try
            {

                int count = client.Client.EndReceive(res); ;


                        string cmd = ASCIIEncoding.ASCII.GetString(clientRecvBuffer, 0, count);
                        client.Client.BeginReceive(clientRecvBuffer, 0, 512, System.Net.Sockets.SocketFlags.None, clientRecvCallback, client);
                        System.Diagnostics.Trace.WriteLine("recv " + cmd);
                        string[] notes = cmd.Split(';');
                        foreach (string note in notes)
                        {
                           // System.Threading.Thread.Sleep(5);
                            handleNote(note);
                        }
                   
               
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e.ToString());
                reconnectTimerStart(false);
                if (reconnectAttempts == 0)
                    connectError(false);
            }

        }
        void reconnectTimerStart(bool start)
        {
            if (this.InvokeRequired)
            {
                errordelegate del = new errordelegate(reconnectTimerStart);
                this.BeginInvoke(del, start);
                return;

            }
            cbConnect.Checked = false;
            if (start)
                reconnectTimer.Start();
            else
                reconnectTimer.Stop();
        }
        delegate void errordelegate(bool start);
        void connectError(bool start)
        {
            if(this.InvokeRequired)
            {
                errordelegate del = new errordelegate(connectError);
                this.BeginInvoke(del, start);
                return;
                
            }
            MessageBox.Show("Couldn't connect to multibox server");
        }

        private void cbConnect_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            if (cbConnect.Checked)
            {
                reconnectAttempts = 5;
                try
                {
                    connectClient(tbServerIp.Text, (int)nudPort.Value);
                    reconnectTimer.Start();
                }
                catch
                {
                    MessageBox.Show("no server found");
                    cbConnect.Checked = false;
                }
                
            }
            else
                reconnectTimer.Stop();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
             Properties.Settings.Default.Save();
            midiDriver.enableInput(checkBox1.Checked);
            if (checkBox1.Checked)
                reconnectTimer.Start();
            else
                reconnectTimer.Stop();
        }

        private void cbEnableMonitor_CheckedChanged(object sender, EventArgs e)
        {
            midiDriver.enableMonitor(cbEnableMonitor.Checked);
        }
        long dynamicDelay = 120;
        private void reconnectTimer_Tick(object sender, EventArgs e)
        {
            int delay = 120;
            if(!dedicatedOctave) //ping the server you put in to establish a running lag
            {
                try
                {
                    System.Net.NetworkInformation.Ping p = new System.Net.NetworkInformation.Ping();
                    var reply = p.Send(IPAddress.Parse(tbServerIp.Text));
                    dynamicDelay = reply.RoundtripTime + 20;
                    System.Diagnostics.Trace.WriteLine("dynamic delay set to " + dynamicDelay.ToString());
                }
                catch { }
            }
            OctaveChangeLock.WaitOne(1000);
            switch((int)nudOctaveID.Value)
            {
                case 3:
                     
                        doKeyPress(Keys.D0, 25);
                        System.Threading.Thread.Sleep(delay);
                        doKeyPress(Keys.D9, 25);
                     System.Threading.Thread.Sleep(delay);
                        doKeyPress(Keys.D9, 25);
                        break;
                case 5:
                        if (nudOctaveID.Value == 5)
                        {
                            doKeyPress(Keys.D9, 25);
                            System.Threading.Thread.Sleep(delay);
                            doKeyPress(Keys.D0, 25);
                            System.Threading.Thread.Sleep(delay);
                            doKeyPress(Keys.D0, 25);
                        }
                        else
                        {
                            doKeyPress(Keys.D0, 25);
                            System.Threading.Thread.Sleep(delay);
                            doKeyPress(Keys.D0, 25);
                            System.Threading.Thread.Sleep(delay);
                            doKeyPress(Keys.D9, 25);
                        }
                        break;
                case 6:
                     doKeyPress(Keys.D9, 25);
                     System.Threading.Thread.Sleep(delay);
                        doKeyPress(Keys.D0, 25);
                     System.Threading.Thread.Sleep(delay);
                        doKeyPress(Keys.D0, 25);
                        break;
                case 2:
                    if(currentOctave == 1)
                    {
                        doKeyPress(Keys.D0, 25);
                        System.Threading.Thread.Sleep(200);
                        doKeyPress(Keys.D9, 25);
                    }
                    else
                    {
                        doKeyPress(Keys.D9, 25);
                        System.Threading.Thread.Sleep(200);
                        doKeyPress(Keys.D0, 25);
                    }
                    break;



            }
            OctaveChangeLock.ReleaseMutex();


        }

        private void tbServerIp_TextChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void nudPort_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void nudOctaveID_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void cmbMidiChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
            midiDriver.setChannel(int.Parse(cmbMidiChannel.Text));
        }

        private void cbxMonitor_SelectedIndexChanged(object sender, EventArgs e)
        {
            midiDriver.selectMonitor(cbxMonitor.SelectedIndex);
        }

        private void cbArtificialLag_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbHardwareKeyboard_CheckedChanged(object sender, EventArgs e)
        {
            if (cbHardwareKeyboard.Checked)
            {
                if (hwkeyboard != null && hwkeyboard.IsOpen)
                {
                    hwkeyboard.Close();
                }
                try
                {
                    hwkeyboard = new System.IO.Ports.SerialPort((string)cbxSerialPort.Items[cbxSerialPort.SelectedIndex]);

                    hwkeyboard.Open();
                }
                catch { MessageBox.Show("couldn't open com port"); cbHardwareKeyboard.Checked = false; }
            }
            else
            {
                if (hwkeyboard != null && hwkeyboard.IsOpen)
                {
                    hwkeyboard.Close();
                }
            }
        }
    }

    public class MidiTransformTable
    {
        public static Dictionary<string, Keys> standard;
       // public static Dictionary<string, Keys> bass;

        public static void init()
        {
            standard = new Dictionary<string, Keys>();
            //bass = new Dictionary<string, Keys>();
            string[] notenames = { "C", "D", "E", "F", "G", "A", "B" };//todo: semitones when anet adds them (when hell freezes over)
            Keys[] keynames = { Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8 };

            for (int o = 1; o < 6; o++)
                for (int n = 0; n < 7; n++)
                {
                    //if (o < 3)
                    //    bass.Add(notenames[n] + o.ToString(), keynames[n]);
                    //else
                        standard.Add(notenames[n] + o.ToString(), keynames[n]);

                }
            standard.Add("C6", Keys.D8);


        }
        public static Keys getKey(string note)
        {
            if (note.Contains("Sharp"))
                note = note.Replace("Sharp", "");
            note = note.Remove(note.Length - 1);
            if (standard.ContainsKey(note))
                return standard[note];
            return Keys.None;
        }

    }

    public class MidiDeviceDriver
    {
        List<Midi.InputDevice> myInputDevices;
        List<Midi.OutputDevice> myOutputDevices;
        Midi.Channel inputChannel;
        bool monitorEnabled;
        bool inputEnabled;
        int selectedMonitor;
        public void enableInput(bool b)
        {
            inputEnabled = b;
        }
        public void enableMonitor(bool b)
        {
            monitorEnabled = b;
        }
        public void setChannel(int c)
        {
            inputChannel = (Midi.Channel)(c-1);
        }
        public MidiDeviceDriver()
        {
            reconnectMidi();
            

        }

        public string [] listMonitors()
        {
            List<string> mons = new List<string>();
            foreach (var dev in myOutputDevices)
                mons.Add(dev.Name);
            return mons.ToArray();
        }
        public void selectMonitor(int id)
        {
            selectedMonitor = id;
        }
        public void reconnectMidi()
        {
            if(myInputDevices == null)
                myInputDevices = new List<Midi.InputDevice>();
            if(myOutputDevices == null)
                myOutputDevices = new List<Midi.OutputDevice>();

            foreach (Midi.InputDevice dev in myInputDevices)
                if (dev.IsOpen)
                {
                    dev.StopReceiving();
                    dev.Close();
                }
            foreach (Midi.OutputDevice dev in myOutputDevices)
                if (dev.IsOpen)
                    dev.Close();
           
            foreach (var midiDevice in Midi.InputDevice.InstalledDevices)
            {
                var myMidi = midiDevice;
                try
                {
                    myMidi.Open();
                    myMidi.NoteOn += MyMidi_NoteOn;
                    myMidi.NoteOff += MyMidi_NoteOff;
                    myMidi.StartReceiving(new Midi.Clock(999));
                    myInputDevices.Add(myMidi);
                    System.Diagnostics.Trace.WriteLine("Connected to " + myMidi.Name);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Trace.WriteLine("Failed to open midi device " + myMidi.Name);
                    //dev didn't open... ignore it

                }
            }
            foreach (var midiDevice in Midi.OutputDevice.InstalledDevices)
            {
                var myMidi = midiDevice;
                try
                {
                    myMidi.Open();
                    myOutputDevices.Add(myMidi);
                    System.Diagnostics.Trace.WriteLine("Connected to " + myMidi.Name);
                }
                catch
                {
                    //dev didn't open... ignore it
                    System.Diagnostics.Trace.WriteLine("Failed to open midi device " + myMidi.Name);
                }
            }
        }
        public delegate void onNoteDelegate(string note);
        /// <summary>
        /// Note format:  ASharp3+ 
        /// A:  Pitch
        /// Sharp: If a semitone, otherwise not present
        /// 3: octave
        /// +: + if on, - if off
        /// </summary>
        public event onNoteDelegate onNote;


        private void MyMidi_NoteOff(Midi.NoteOffMessage msg)
        {
            if (msg.Channel != inputChannel)
                return;
            Midi.NoteMessage match;
            if (isNoteOn(msg, out match))
            {
                onNotes.Remove(match);
            }
            var stringmsg = msg.Pitch.ToString() + "-";
            if (onNote != null && inputEnabled)
                onNote(stringmsg);
            if (monitorEnabled)
                myOutputDevices[selectedMonitor].SendNoteOff(Midi.Channel.Channel1, msg.Pitch, msg.Velocity);
        }
        private void MyMidi_NoteOff(Midi.NoteOnMessage msg)
        {
            if (msg.Channel != inputChannel)
                return;
            Midi.NoteMessage match;
            if (isNoteOn(msg, out match))
            {
                onNotes.Remove(match);
            }
            var stringmsg = msg.Pitch.ToString() + "-";
            if (onNote != null && inputEnabled)
                onNote(stringmsg);
            if (monitorEnabled)
                myOutputDevices[selectedMonitor].SendNoteOff(Midi.Channel.Channel1, msg.Pitch, msg.Velocity);
        }
        List<Midi.NoteMessage> onNotes;
        bool isNoteOn(Midi.NoteMessage msg, out Midi.NoteMessage match)
        {
            if (onNotes == null)
            {
                onNotes = new List<Midi.NoteMessage>();
                match = null;
                return false;
            }
            foreach (Midi.NoteMessage m in onNotes)
                if (m.Device == msg.Device && m.Pitch == msg.Pitch && m.Channel == msg.Channel)
                {
                    
                    match = m as Midi.NoteMessage;
                    return true;
                }
            match = null;
            return false;
        }
        bool isNoteDuplicate(Midi.NoteMessage msg)
        {
            foreach (Midi.NoteMessage m in onNotes)
                if (m.Device == msg.Device && m.Channel != msg.Channel)
                {
                   
                    return true;
                }
            return false;
        }
        private void MyMidi_NoteOn(Midi.NoteOnMessage msg)
        {
            if (msg.Channel != inputChannel)
                return;
            Midi.NoteMessage match;
            if (isNoteOn(msg, out match))
            {
                if(match.GetType() == typeof(Midi.NoteOnMessage))
                    MyMidi_NoteOff((Midi.NoteOnMessage)match);
                try
                {
                    MyMidi_NoteOff((Midi.NoteOffMessage)match);  //some midi devices don't properly support note off, but instead send note on again.  If the note is already on, treat it as a note off
                }
                catch
                {
                    MyMidi_NoteOff((Midi.NoteOnMessage)match);
                }
            }
            else
            {
                if (isNoteDuplicate(msg))  //for the FA-08 or other devices which support multi channel broadcast, don't duplicate the midi message
                    return;
                onNotes.Add(msg);
                var stringmsg = msg.Pitch.ToString() + "+";
                if (onNote != null && inputEnabled)
                    onNote(stringmsg);
            }
            if (monitorEnabled)
                myOutputDevices[selectedMonitor].SendNoteOn(Midi.Channel.Channel1, msg.Pitch, msg.Velocity);
        }
    }
}
