using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace websocker_push
{
    public partial class Form1 : Form
    {

        private WebSocketServer server;
        private Thread serverThread;

        private string message = "";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new WebSocketServer("ws://localhost:8080");
            server.AddWebSocketService<Echo>("/Echo");

        }

       private void btnStart_Click(object sender, EventArgs e)
        {
            server.Start();
            serverThread = new Thread(() => server.WebSocketServices.Broadcast("Server started."));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            server.Stop();
            serverThread = new Thread(() => server.WebSocketServices.Broadcast("Server stopped."));
            serverThread.Start();

            button4.BackColor = Color.FromArgb(255, 200, 200, 200);

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            server.WebSocketServices.Broadcast("00000000000");
            //메세지를 보내는 영역 .  id를 개별로 받아서 서로 통신 할 수 있도록 정리 필요.
        }
        public string Message
        {
            get
            {

                return message;
            }
            set
            {
                message = value;
                textBox1.Text = value; // 텍스트박스에 값을 설정
            }
        }

        public void SetText(string text)
        {
            textBox1.Invoke((MethodInvoker)delegate {
                textBox1.Text = text + "/r/n" + textBox1.Text;
                button4.BackColor = Color.FromArgb(255, 255, 0, 0);
            });
        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.FromArgb(255, 200, 200, 200);
        }
    }


    public class Echo : WebSocketBehavior
    {
        protected override void OnMessage(MessageEventArgs e)
        {
           // Send("websocket" + e.Data);

            var msg = e.Data;
            Console.WriteLine("Received message: " + msg);
            Send(msg);
            // Form1.SetText(msg);
            var form = Application.OpenForms.OfType<Form1>().FirstOrDefault();
            if (form != null)
            {
                form.SetText(msg);
            }


        }

}


}
