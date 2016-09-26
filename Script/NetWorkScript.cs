using UnityEngine;
using System.Collections;
using System.Net.Sockets;
using System.IO;
using System;
using System.Text;
using LitJson;
using System.Threading;
using System.Collections.Generic;

public  class NetWorkScript 
{
    
    private static NetWorkScript script;
    private Socket socket;
    public static string host ="127.0.0.1";//服务器IP地址
    private int port = 10100;//服务器端口
    private byte[] readM = new byte[1024];
	private ByteArray ioBuff=new ByteArray();
	private int dataSize;
    private List<SocketModel> messages = new List<SocketModel>();
    private bool isRead = false;

    //获取连接对象
    public static NetWorkScript getInstance() {
        if (script == null) {
            //第一次调用的时候 创建单例对象 并进行初始化操作
            script = new NetWorkScript();
            script.init();
        }
       
        return script;
    }
    private void init() {
        try
        {
            //创建socket连接对象
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接到服务器
            socket.Connect(host, port);
            //连接后开始从服务器读取网络消息
            socket.BeginReceive(readM, 0, 1024, SocketFlags.None, ReceiveCallBack,readM);
            Debug.Log("socket session open");
        }
        catch (Exception e) {
            //连接失败 打印异常
            Debug.Log("Connect error:"+e.Message);
        }
    }

    public void sendMessage(int type, int area, int command, string message) {
        ByteArray arr= new ByteArray();
        arr.WriteInt(type);
        arr.WriteInt(area);
        arr.WriteInt(command);
        if (message != null)
        {
            arr.WriteInt(message.Length);
            arr.WriteUTFBytes(message);
            //  arr.WriteUTFBytes(message);
        }
        else {
            arr.WriteInt(0);
        }
        try
        {
            socket.Send(arr.Buffer);
        }
        catch {
           Debug.Log("socket error");
        }
    }

    public void onData()
    {
        //消息读取完成后开始解析 
		if(ioBuff.Length<4){
			//包头为长度4的整型
            isRead = false;
			return;
		}
		dataSize=ioBuff.ReadInt();
		Debug.Log("dataSize"+dataSize);
		if(dataSize>ioBuff.Length-4){
			//包长不够 等下个包的到来
			Debug.Log("包长不够");
			ioBuff.Postion=0;
            isRead = false;
			return;
		}
		ByteArray ioData=new ByteArray();
		ioData.WriteBytes(ioBuff.Buffer,4,dataSize);
		ioBuff.Postion+=dataSize;

                
                int type = ioData.ReadInt();//表示消息类型  我们这里有两种
                int area = ioData.ReadInt();//这里表示消息的区域码 在登录这样的服务器单例模块中 没有效果 在地图消息的时候用于区分属于哪张地图来的消息
                int command = ioData.ReadInt();//模块内部协议---具体稍后描述
                int len = ioData.ReadInt();

                string m=null;
                if (len > 0) { m = ioData.ReadUTFBytes((uint)len); }//这里开始就是读取服务器传过来的消息对象了 是一串json字符串
             //转换为Socket消息模型
                SocketModel model= new SocketModel();
                model.type = type;
                model.area = area;
                model.command = command;
                model.message = m;
                Debug.Log(type+"   "+area+"  "+command+"length"+(16+len));

                //消息接收完毕后，存入收到消息队列
                messages.Add(model);
		ByteArray bytes=new ByteArray();
        bytes.WriteBytes(ioBuff.Buffer, ioBuff.Postion, ioBuff.Buffer.Length - ioBuff.Postion);
		ioBuff=bytes;
		onData();
				//Debug.Log("插入队列后"+arr.Buffer[12+len+1]);
        
    }
    //这是读取服务器消息的回调--当有消息过来的时候BgenReceive方法会回调此函数
    private void ReceiveCallBack(IAsyncResult ar)
    {
        
        int readCount = 0;
        try
        {
            //读取消息长度
            readCount = socket.EndReceive(ar);//调用这个函数来结束本次接收并返回接收到的数据长度。 
            Debug.Log("读取消息长度" + readCount);
            byte[] bytes = new byte[readCount];//创建长度对等的bytearray用于接收
            Buffer.BlockCopy(readM, 0, bytes, 0, readCount);//拷贝读取的消息到 消息接收数组
            ioBuff.WriteBytes(bytes);
			Debug.Log(ioBuff.Buffer.Length);
            if (!isRead){
                isRead = true;
			    onData();//消息读取完成
            }
        }
        catch (SocketException)//出现Socket异常就关闭连接 
        {
           socket.Close();//这个函数用来关闭客户端连接 
            return;
        }
        socket.BeginReceive(readM, 0, 1024, SocketFlags.None, ReceiveCallBack,readM);
        
    }

    public List<SocketModel> getList() {
        return messages;
    }
    
}
