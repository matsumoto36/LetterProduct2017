using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public struct ControllerInfo {
	public ControlType type { get; private set; }
	public int joystickNum { get; private set; }

	public ControllerInfo(ControlType type, int joystickNum) {
		this.type = type;
		this.joystickNum = joystickNum;
	}
}