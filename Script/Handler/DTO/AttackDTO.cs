using System;

public class AttackDTO	
{
	 public int attType { get; set; }

     public Assets.Model.Vector3 point { get; set; }

     public Assets.Model.Vector4 Rotation { get; set; }

     public string targetId { get; set; }

     public string userId { get; set; }
}


