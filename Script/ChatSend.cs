using UnityEngine;
using System.Collections;
//dfhddd
//dfhdsf
//dfdfdf
public class ChatSend : MonoBehaviour {
	
	public UILabel label;
	
	void OnClick(){
		//Debug.Log ("Click");
		if(label.text == string.Empty || label.text == null){
			return;
		}
		StringDTO dto = new StringDTO(label.text);
		string message = Coding<StringDTO>.encode (dto);
		NetWorkScript.getInstance().sendMessage (Protocol.MAP,GameInfo.myModel.map,MapProtocol.TALK_CREQ,message);
		label.text = string.Empty;
	}
}
