var isUserActive = true;

function idle() {
	var t;
	// Thanks: http://stackoverflow.com/questions/667555/detecting-idle-time-in-javascript-elegantly

	// reset the screensaver if you do something;
	window.onload = resetTimer;
	window.onmousemove = resetTimer;
	window.onmousedown = resetTimer; // catches touchscreen presses
	window.onclick = resetTimer;     // catches touchpad clicks
	window.onscroll = resetTimer;    // catches scrolling with arrow keys
	window.onkeypress = resetTimer;
	var screensaver;

	function idleHelper() {
		isUserActive = false;
		// console.log("Not active on screen");
		window.drawEnv.updateInterval = null;
	}

	function resetTimer() {
		clearTimeout(t);
		t = setTimeout(idleHelper, 60000);  // time is in milliseconds 188400 == 3,14 minute

		if (!isUserActive) {
			// console.log("User is back again");
			window.drawEnv.updateInterval = 1;
		}

		isUserActive = true;
	}
}

idle();
